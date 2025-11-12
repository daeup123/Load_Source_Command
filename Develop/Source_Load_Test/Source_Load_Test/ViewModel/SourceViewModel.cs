using LiveCharts;
using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Source_Load_Test.ViewModel
{
    public class SourceViewModel : ObservableObject
    {
        public SourceViewModel()
        {
            Console.WriteLine("SourceViewModel init");

            DataListSource = DataRepository.Instance.SourceDataList;
            ChartSeries = DataRepository.Instance.ChartSeriesSource;
            TimeArray = DataRepository.Instance.TimeArraySource;

            Init();
        }

        public string DeviceInfo
        {
            get => "SPS 5041X ";
        }

        private void Init()
        {
            if (!(DataListSource.Count == 0))
            {
                DataListSource.Clear();
            }
            if (!(ChartSeries[0].Values.Count == 0))
            {
                ChartSeries.Clear();
            }

            SetMode(Mode.CC);
            _countDataNo = 1;

            SetV = "0";
            SetC = "0";
            GetC = "0";
            GetV = "0";
            GetP = "0";
        }
        //private RelayCommand _startstop = null;

        //private void StartStopBtn(object commandparameter) // 장비 Output ON/OFF
        //{
        //    List<string> responce = new List<string>();

        //    DeviceManager.Source.Power((string)commandparameter, _setV, _setC);

        //    responce = DeviceManager.Source.GetValue(); // 설정값 받기

        //    if (responce.Count > 0)
        //    {
        //        GetV = responce[0]; // 전압
        //        GetC = responce[1]; // 전류
        //        GetP = responce[2]; // 전력
        //    }
        //}
        //public ICommand StartStopCommand
        //{
        //    get
        //    {
        //        if (_startstop == null)
        //        {
        //            _startstop = new RelayCommand(StartStopBtn);
        //        }

        //        return _startstop;
        //    }
        //}

        #region 모드 변경
        private RelayCommand _setMode = null;

        private void SetMode(object commandparamter) // 모드변경
        {
            Mode mode;

            if (commandparamter is Mode)
            {
                mode = (Mode)commandparamter;
            }
            else
            {
                string param = (string)commandparamter;

                mode = (Mode)Enum.Parse(typeof(Mode), param);
            }
            _currentmode = mode;
            IsCC = mode == Mode.CC;
            IsCV = mode == Mode.CV;
        }
        public ICommand SetModeCommand // 모드 변경
        {
            get
            {
                if (_setMode == null)
                {
                    _setMode = new RelayCommand(SetMode);
                }
                return _setMode;
            }
        }

        private bool _isCC = false;
        private bool _isCV = false;
        private Enum _currentmode;
        public Enum CurrentMode
        {
            get => _currentmode;
            set => SetProperty(ref _currentmode, value);
        }
        public bool IsCC
        {
            get => (_isCC);
            set => SetProperty(ref _isCC, value);
        }
        public bool IsCV
        {
            get => (_isCV);
            set => SetProperty(ref (_isCV), value);
        }
        #endregion

        // 설정 전압, 전류
        private string _setV = "0";
        private string _setC = "0";

        public string SetV
        {
            get => _setV;

            set => SetProperty(ref _setV, value);
        }
        public string SetC
        {
            get => _setC;

            set => SetProperty(ref _setC, value);
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
            DeviceManager.Source.SetValue(SetV, SetC);

            if(VoltageRise != "0" || VoltageFall != "0" || CurrentRise != "0" || CurrentFall != "0")
            {
                string mode = "";
                bool result = false;

                if ((VoltageRise != "0") || (CurrentRise != "0"))
                {
                    mode = "Rise";
                    result = DeviceManager.Source.SetSlewRate(mode, VoltageRise, CurrentRise);
                }
                else if ((VoltageFall != "0") || (CurrentFall != "0"))
                {
                    mode = "Fall";
                    result = DeviceManager.Source.SetSlewRate(mode, VoltageFall, CurrentFall);
                }

                if (!result)
                {
                    MessageBox.Show("Slew Rate 설정에 실패하였습니다.");
                    // 설정 실패 메시지
                }
                else
                {
                    MessageBox.Show("Slew Rate 설정에 성공하였습니다.");
                }
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

        // 초기화 버튼
        private RelayCommand _rstBtn = null;
        private void RstBtn(object param)
        {
            DeviceManager.Source.Init();
            Init();
        }
        public ICommand RstBtnCommand
        {
            get
            {
                if (_rstBtn == null)
                {
                    _rstBtn = new RelayCommand(RstBtn);
                }
                return _rstBtn;
            }
        }

        #region On Off 버튼 그래프, 차트

        private ObservableCollection<Data> _dataListSource;
        public ObservableCollection<Data> DataListSource
        {
            get => _dataListSource;
            set => SetProperty(ref _dataListSource, value);
        }

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
            result = DeviceManager.Source.Power((string)commandparameter);

            if (result)
            {
                OutputStatus = "inPut ON 입니다.";
                DataListSource.Clear();
                //ChartSeries.Clear();
                for (int i = 0; i < ChartSeries?.Count; i++)
                {
                    ChartSeries[i].Values.Clear();
                }
                _timeArray = new string[0];

                float time = (float)(float.TryParse(_responseTime, out float t) ? t : 0.1);

                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(time)
                };
                _timer.Tick += async (s, e) => await GetData();
                _timer.Start();
            }
            else
            {
                _timer?.Stop();
                Init();
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

        private int _countDataNo = 1;
        private async Task GetData()
        {
            Data responce = new Data();
            responce = await Task.Run(() => DeviceManager.Source.GetMeans()); // 설정값 받기

            GetV = responce.Voltage.ToString();
            GetC = responce.Current.ToString();
            GetP = responce.Power.ToString();
            _currentmode = (Mode)Enum.Parse(typeof(Mode), responce.Mode);
            responce.No = _countDataNo++;

            UpdateLivechart(responce);
            _dataListSource.Add(responce);
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
        #endregion

        private string _outputStatus = "inPut OFF 입니다.";
        public string OutputStatus
        {
            get => _outputStatus;
            set => SetProperty(ref _outputStatus, value);
        }

        #region slewRate Property
        private string _voltageRise = "0";
        public string VoltageRise
        {
            get => _voltageRise;
            set => SetProperty(ref _voltageRise, value);
        }
        private string _voltageFall = "0";
        public string VoltageFall
        {
            get => _voltageFall;
            set => SetProperty(ref _voltageFall, value);
        }
        private string _currentRise = "0";
        public string CurrentRise
        {
            get => _currentRise;
            set => SetProperty(ref _currentRise, value);
        }
        private string _currentFall = "0";
        public string CurrentFall
        {
            get => _currentFall;
            set => SetProperty(ref _currentFall, value);
        }

        #endregion
    }
}
