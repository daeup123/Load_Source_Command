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

        private string AddrnToStr(ref string str) // 문자열끝에 개행추가
        {
            string returnstr = str + "\r\n";

            return returnstr;
        }

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
            IsCV = mode == Mode.CV;
            IsCC = mode == Mode.CC;
            IsCP = mode == Mode.CP;
            IsCR = mode == Mode.CR;
        }
    }
}
