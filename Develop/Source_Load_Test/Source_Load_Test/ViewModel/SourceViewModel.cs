using Source_Load_Test.Model;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using Source_Load_Test.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Source_Load_Test.ViewModel
{
    public class SourceViewModel : ObservableObject
    {
        public SourceViewModel()
        {
            Console.WriteLine("SourceViewModel init");
        }

        public string DeviceInfo
        {
            get => DeviceManager.Source.GetIDN();
        }

        private RelayCommand _startstop = null;

        private void StartStopBtn(object commandparameter) // 장비 Output ON/OFF
        {
            List<string> responce = new List<string>();

            DeviceManager.Source.Power((string)commandparameter, _setV, _setC);

            responce = DeviceManager.Source.GetValue(); // 설정값 받기

            if (responce.Count > 0)
            {
                GetV = responce[0]; // 전압
                GetC = responce[1]; // 전류
                GetP = responce[2]; // 전력
            }
        }
        public ICommand StartStop
        {
            get
            {
                if (_startstop == null)
                {
                    _startstop = new RelayCommand(StartStopBtn);
                }

                return _startstop;
            }
        }

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

        private RelayCommand _setMode = null;

        private void SetMode(object commandparamter) // 모드변경
        {
            string param = (string)commandparamter;

            Mode mode = (Mode)Enum.Parse(typeof(Mode), param);

            IsCC = mode == Mode.CC;
            IsCV = mode == Mode.CV;
        }
        public ICommand SetModeCommand // 모드 변경
        {
            get
            {
                if(_setMode == null)
                {
                    _setMode = new RelayCommand(SetMode);
                }
                return _setMode;
            }
        }
        
        private bool _isCC = false;
        private bool _isCV = false;

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

        private RelayCommand _setting = null;
        private void Setting(object commandparamter) // 설정
        {
            Console.WriteLine(SetV + SetC);
            DeviceManager.Source.SetValue(SetV, SetC);
        }

        public ICommand SettingCommand
        {
            get
            {
                if(_setting == null)
                {
                    _setting = new RelayCommand(Setting);
                }
                return _setting;
            }
        }
    }
}
