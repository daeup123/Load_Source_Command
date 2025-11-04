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
    public class SourceViewModel : ObservableObject
    {
        public SourceViewModel()
        {
            Console.WriteLine("SourceViewModel init");
        }
        private RelayCommand _startstop = null;

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

        private void StartStopBtn(object commandparameter) // 장비 Output ON/OFF
        {
            List<string> responce = new List<string>();

            responce = DeviceManager.Source.Power((string)commandparameter, _setV, _setI);

            if (responce.Count > 0)
            {
                
            }
        }
        // 설정 전압, 전류
        private string _setV = "0";
        private string _setI = "0";

        public string SetV
        {
            get => _setV;

            set => SetProperty(ref _setV, value);
        }
        public string SetI
        {
            get => _setI;

            set => SetProperty(ref _setI, value);
        }

        // 조회된 전압, 전류, 전력
        private string _getV = "0";
        private string _getI = "0";
        private string _getP = "0";

        public string GetV
        {
            get => _getV;

            set => SetProperty(ref _getV, value);
        }
        public string GetI
        {
            get => _getI;

            set => SetProperty(ref _getI, value);
        }
        public string GetP
        {
            get => _getP;

            set => SetProperty(ref _getP, value);
        }
    }
}
