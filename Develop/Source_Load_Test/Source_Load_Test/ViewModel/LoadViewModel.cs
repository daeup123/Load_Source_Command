using LiveCharts;
using LiveCharts.Wpf;
using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.View;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Source_Load_Test.ViewModel
{
    public class LoadViewModel : ObservableObject
    {
        public LoadViewModel()
        {
            Debug.WriteLine("LoadViewModel Init");

            DataListLoad = DataRepository.Instance.LoadDataList;
            ChartSeries = DataRepository.Instance.ChartSeriesLoad;
            TimeArray = DataRepository.Instance.TimeArrayLoad;

            Init();
        }
        private void Init()
        {
            SetC = "0";
            SetV = "0";
            SetP = "0";
            SetR = "0";

            if (!(DataListLoad.Count == 0))
            {
                DataListLoad.Clear();
            }
            if (!(ChartSeries[0].Values.Count == 0))
            {
                ChartSeries.Clear();
            }

            SetMode(Mode.CC);

            _countDataNo = 1;
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
            string param = commandParameter.ToString();
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
            //Debug.WriteLine(SetV + SetC);
            //string value = (string)commandparamter;
            switch (currentmode)
            {
                case Mode.CV:
                    if(float.TryParse(SetV, out float SetVtoint))
                    {
                        if ((0 <= SetVtoint) && (SetVtoint <= 80))
                        {
                            DeviceManager.Load.SetValue(Mode.CV, SetV);
                        }
                        else
                        {
                            MessageBox.Show("0~80 전압 범위 초과");
                        }
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 전압 값입니다. 정수만 입력해 주세요");
                    }
                    break;
                case Mode.CC: // Constant Current (정전류): 0 ~ 30
                    if (float.TryParse(SetC, out float currentSetPoint))
                    {
                        if (currentSetPoint >= 0.0 && currentSetPoint <= 30.0)
                        {
                            DeviceManager.Load.SetValue(Mode.CC, SetC);
                        }
                        else
                        {
                            MessageBox.Show("정전류(CC) 값은 0.0 A에서 30.0 A 사이여야 합니다.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 전류(CC) 설정 값입니다. 숫자를 입력해 주세요.");
                    }
                    break;

                case Mode.CR: // Constant Resistance (정저항): 20 ~ 2000
                    if (float.TryParse(SetR, out float resistanceSetPoint))
                    {
                        if (resistanceSetPoint >= 20.0 && resistanceSetPoint <= 2000.0)
                        {
                            DeviceManager.Load.SetValue(Mode.CR, SetR);
                        }
                        else
                        {
                            MessageBox.Show("정저항(CR) 값은 20 Ω에서 2000 Ω 사이여야 합니다.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 저항(CR) 설정 값입니다. 숫자를 입력해 주세요.");
                    }
                    break;

                case Mode.CP: // Constant Power (정전력): 0 ~ 250
                    if (float.TryParse(SetP, out float powerSetPoint))
                    {
                        if (powerSetPoint >= 0.0 && powerSetPoint <= 250.0)
                        {
                            DeviceManager.Load.SetValue(Mode.CP, SetP);
                        }
                        else
                        {
                            MessageBox.Show("정전력(CP) 값은 0.0 W에서 250.0 W 사이여야 합니다.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 전력(CP) 설정 값입니다. 숫자를 입력해 주세요.");
                    }
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

        #region MyRegion // On Off 버튼 그래프, 차트

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
                LoadStatus = "OutPut ON 입니다.";
                DataListLoad.Clear();
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
            responce = await Task.Run(() => DeviceManager.Load.GetValue()); // 설정값 받기

            GetV = responce.Voltage.ToString();
            GetC = responce.Current.ToString();
            GetP = responce.Power.ToString();
            responce.Mode = currentmode.ToString();
            responce.No = _countDataNo++;
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
        #endregion

        // 초기화 버튼
        private RelayCommand _rstBtn = null;
        private void RstBtn(object param)
        {
            DeviceManager.Load.Init();
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

        private ObservableCollection<Data> _dataListLoad; // 생성자에서 참조
        public ObservableCollection<Data> DataListLoad
        {
            get => _dataListLoad;
            set => SetProperty(ref _dataListLoad, value);
        }
        private string _inputStatus = "Input OFF 입니다.";
        public string LoadStatus
        {
            get => _inputStatus;
            set => SetProperty(ref _inputStatus, value);
        }

        private RelayCommand _openListMode = null;

        private void OpenListMode(object commandparameter)
        {
            var dialogvm = new LoadListModeViewModel();
            var dialog = new LoadListModeView
            {
                DataContext = dialogvm
            };

            // ✅ 자식 ViewModel의 이벤트 구독
            dialogvm.EventListModeStarted += CloseListMode;

            dialog.Show();
        }

        private void CloseListMode(object? sender, bool commandparameter)
        {
            bool isOn = (bool)commandparameter;
            // 닫기 동작
            if (isOn == true)
            {

            }
            else
            {

            }
        }
        public ICommand OpenListModeCommand
        {
            get
            {
                if (_openListMode == null)
                {
                    _openListMode = new RelayCommand(OpenListMode);
                }
                return _openListMode;
            }
        }
    }
}
