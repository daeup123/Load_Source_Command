using LiveCharts;
using LiveCharts.Wpf;
using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Source_Load_Test.ViewModel
{
    public class LoadViewModel : ObservableObject
    {
        public LoadViewModel()
        {
            Console.WriteLine("LoadViewModel Init");

            ChartSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Voltage",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Current",
                    Values = new ChartValues<double>(),
                    PointGeometry = null
                }
            };
        }

        public string DeviceInfo
        {
            get => "Array3720A ";
        }

        #region 모드설정

        private RelayCommand _setModeCommand = null; // 로드의 모드를 선택

        public ICommand SetModeCommand // 로드의 모드를 선택
        {
            get
            {
                if (_setModeCommand == null)
                {
                    _setModeCommand = new RelayCommand(SetMode);
                }
                return _setModeCommand;
            }
        }

        private void SetMode(object commandParameter)
        {
            string param = (string)commandParameter;

            Mode mode = (Mode)Enum.Parse(typeof(Mode), param, true);
            CurrentMode(mode);
        }

        private Mode currentmode;

        private bool _isCV = false; // CV모드
        private bool _isCC = false; // CC모드
        private bool _isCR = false; // CR모드
        private bool _isCP = false; // CP모드

        public bool IsCV
        {
            get => _isCV;
            set => SetProperty(ref _isCV, value);
        }
        public bool IsCC
        {
            get => _isCC;
            set => SetProperty(ref _isCC, value);
        }
        public bool IsCP
        {
            get => _isCP;
            set => SetProperty(ref _isCP, value);
        }
        public bool IsCR
        {
            get => _isCR;
            set => SetProperty(ref _isCR, value);
        }
    
        private void CurrentMode(Mode mode)
        {
            currentmode = mode;

            IsCV = mode == Mode.CV;
            IsCC = mode == Mode.CC;
            IsCP = mode == Mode.CP;
            IsCR = mode == Mode.CR;
        }
        #endregion

        // 설정 전압, 전류, 저항, 전력
        private string _setV = "0";
        private string _setC = "0";
        private string _setR = "0";
        private string _setP = "0";

        public string SetV
        {
            get => _setV;

            set => SetProperty(ref _setV, value);
            //set
            //{
            //    if(int.TryParse(value, out int v))
            //    {
            //        if((0 <= v) && (v <= 40))
            //        {
            //            SetProperty(ref _setV, value);
            //        }
            //        else
            //        {
            //            OverflowException ex = new OverflowException("0~40 범위 초과");
            //        }
            //    }
            //    else
            //    {
            //        // 예외
            //    }
            //}
        }
        public string SetC
        {
            get => _setC;

            set => SetProperty(ref _setC, value);
        }
        public string SetR
        {
            get => _setR;

            set => SetProperty(ref _setR, value);
        }
        public string SetP
        {
            get => _setP;

            set => SetProperty(ref _setP, value);
        }

        // 조회된 전압, 전류, 전력
        private string _getV = "0";
        private string _getC = "0";
        private string _getP = "0";

        public string GetV
        {
            get => _getV;

            set => SetProperty(ref _getV, value);
        }
        public string GetC
        {
            get => _getC;

            set => SetProperty(ref _getC, value);
        }
        public string GetP
        {
            get => _getP;

            set => SetProperty(ref _getP, value);
        }

        // Apply 적용
        private RelayCommand _apply = null;
        private void Apply(object commandparamter)
        {
            //Console.WriteLine(SetV + SetC);
            //string value = (string)commandparamter;
            switch(currentmode)
            {
                case Mode.CV:
                    DeviceManager.Load.SetValue(Mode.CV, SetV);
                    break;
                case Mode.CC:
                    DeviceManager.Load.SetValue(Mode.CC, SetC);
                    break;
                case Mode.CR:
                    DeviceManager.Load.SetValue(Mode.CR, SetR);
                    break;
                case Mode.CP:
                    DeviceManager.Load.SetValue(Mode.CP, SetP);
                    break;
                default:
                    break;
            }
        }

        public ICommand ApplyCommand
        {
            get
            {
                if (_apply == null)
                {
                    _apply = new RelayCommand(Apply);
                }
                return _apply;
            }
        }

        // On Off 버튼
        private RelayCommand _startstop = null;
        private bool result = false;
        private DispatcherTimer _timer; // 타이머 객체
        private SeriesCollection _chartSeries;
        public SeriesCollection ChartSeries
        {
            get => _chartSeries;
            set => SetProperty(ref _chartSeries, value);
        }
        private int _timerCount = 0;
        private string[] _timeArray;
        public string[] TimeArray // 시간축
        {
            get => _timeArray;
            set => SetProperty(ref _timeArray, value);
        } 

        private void StartStop(object commandparameter) // 장비 Output ON/OFF
        {
            result = DeviceManager.Load.Power((string)commandparameter);

            if (result)
            {
                _dataListLoad.Clear();
                DataListLoad.Clear();
                //ChartSeries.Clear();
                for(int i = 0; i < ChartSeries?.Count; i++)
                {
                    ChartSeries[i].Values.Clear();
                }
                _timeArray = new string[0];
                double time = double.TryParse(_responseTime, out double t) ? t : 0.1;

                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(time)
                };
                _timer.Tick += async (s, e) => await LoadGetData();
                _timer.Start();
            }
            else
            {
                _timer?.Stop();
            }
        }
        public ICommand StartStopCommand
        {
            get
            {
                if (_startstop == null)
                {
                    _startstop = new RelayCommand(StartStop);
                }

                return _startstop;
            }
        }

        private async Task LoadGetData()
        {
            Data responce = new Data();
            responce = await Task.Run(() => DeviceManager.Load.GetValue()); // 설정값 받기
                
            GetV = responce.Voltage.ToString();
            GetC = responce.Current.ToString();
            GetP = responce.Power.ToString();
            responce.Mode = currentmode.ToString();
            UpdateLivechart(responce);
            _dataListLoad.Add(responce);
        }
        private void UpdateLivechart(Data data)
        {
            // 그래프 업데이트
            ChartSeries[0].Values.Add(data.Voltage);
            ChartSeries[1].Values.Add(data.Current);

            // X축 레이블
            TimeArray = TimeArray.Append((++_timerCount).ToString()).ToArray();

            // 최대 20개만 표시 (스크롤처럼 최신 20개)
            if (ChartSeries[0].Values.Count > 20)
            {
                ChartSeries[0].Values.RemoveAt(0);
                ChartSeries[1].Values.RemoveAt(0);
                TimeArray = TimeArray.Skip(1).ToArray();
            }
        }
        private string _responseTime = "1";
        public string ResponseTime
        {
            get => _responseTime;
            set => SetProperty(ref _responseTime, value);
        }
        // 초기화 버튼
        private RelayCommand _rstBtn = null;
        private void RstBtn(object param)
        {
            DeviceManager.Load.Init();
        }
        public ICommand RstBtnCommand
        {
            get
            {
                if(_rstBtn == null)
                {
                    _rstBtn = new RelayCommand(RstBtn);
                }
                return _rstBtn;
            }
        }
        
        private ObservableCollection<Data> _dataListLoad = new ObservableCollection<Data>();
        public ObservableCollection<Data> DataListLoad
        {
            get => _dataListLoad;
            set => SetProperty(ref _dataListLoad, value);
        }
        private void AddData()
        {
            double v = double.Parse(GetV);
            Data data = new Data();
            // 데이터를 저장하고 datas에 저장
            //datas.Add(new Data() { Voltage = , Current = , Resistance = , Power = , CurrentVoltage = , CurrentCurrent = , Currenttime =  });
        }

    }
}
