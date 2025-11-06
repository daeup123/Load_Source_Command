using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Source_Load_Test.ViewModel
{
    public class LoadViewModel : ObservableObject
    {
        public LoadViewModel()
        {
            Console.WriteLine("LoadViewModel Init");
        }

        public string DeviceInfo
        {
            get => DeviceManager.Source.GetIDN();
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
            string value = (string)commandparamter;

            DeviceManager.Load.SetValue(currentmode, value);
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
        private void StartStop(object commandparameter) // 장비 Output ON/OFF
        {
            List<string> responce = new List<string>();

            DeviceManager.Load.Power((string)commandparameter);

            //responce = DeviceManager.Source.GetValue(); // 설정값 받기

            //if (responce.Count > 0)
            //{
            //    GetV = responce[0]; // 전압
            //    GetC = responce[1]; // 전류
            //    GetP = responce[2]; // 전력
            //}
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
    }
}
