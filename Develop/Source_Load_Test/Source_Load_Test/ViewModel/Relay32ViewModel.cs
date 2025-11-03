using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using Source_Load_Test.ViewModel.Control;

namespace RelayTest.ViewModel
{
    internal class Relay32ViewModel : ObservableObject
    {
        public Relay32ViewModel()
        {
            Relay32Model = new RelayMannger();
            SetRelayList();// 릴레이배열 초기화하기
        }

        private RelayMannger Relay32Model { get; set; } // 디바이스 모델
        public ObservableCollection<RelayModel> s_relays { get; set; } // 릴레이 UI 컨트롤 배열

        private RelayCommand _relayClicked = null; // 개별 릴레이 클릭
        private RelayCommand _relayOffAll = null; // 릴레이 off
        private RelayCommand _relayOnAll = null; // 릴레이 ON
        //private RelayCommand _quary = null; // 쿼리

        private void SetRelayList() // 릴레이배열 초기화하기
        {
            s_relays = new ObservableCollection<RelayModel>();

            for (int i = 1; i <= 32; i++)
            {
                s_relays.Add(new RelayModel
                {
                    Name = $"Relay {i}",
                    Num = i,
                    IsOn = false
                });
            }
        }

        public ICommand RelayClicked // 개별 릴레이 제어 커맨드
        {
            get
            {
                if (_relayClicked == null)
                {
                    _relayClicked = new RelayCommand(ClickedSingle);
                }
                return _relayClicked;
            }
        }

        private void ClickedSingle(object commandParamenter) // 개별 릴레이 제어
        {
            int num = 0;
            bool isOn = false;
            if(commandParamenter is RelayModel relay)
            {
                relay.IsOn = !relay.IsOn; // 반전 
                num = relay.Num; // 번호받기
                isOn = relay.IsOn;
            }
            MessageBox.Show(num.ToString() + "  " + isOn.ToString());

            Relay32Model.ClickedSingleRelay(num, isOn);
        }

        public ICommand RelayOnAll // 릴레이 모두켜기
        {
            get
            {
                if (_relayOnAll == null)
                {
                    _relayOnAll = new RelayCommand(RelayOn);
                }
                return _relayOnAll;
            }
        }

        private  void RelayOn(object commandParameter) // 릴레이 모두켜기
        {
            foreach (RelayModel md in s_relays)
            {
                md.IsOn = true;
            }

            TextBind = Relay32Model.RelayOn();            
        }
        public ICommand RelayOffAll // 릴레이 모두 종료 커맨드 & 프로퍼티
        {
            get
            {
                if (_relayOffAll == null)
                {
                    _relayOffAll = new RelayCommand(RelayOff);
                }
                return _relayOffAll;
            }
        }
        private void RelayOff(object commandParamter) // 릴레이 모두 종료
        {
            foreach (RelayModel md in s_relays)
            {
                md.IsOn = false;
            }

            TextBind = Relay32Model.RelayOff().ToString();
        }

        public string TextBind
        {
            get => Relay32Model.Text;
            set
            {
                Relay32Model.Text = value;
                OnPropertyChanged(nameof(TextBind));
            }
        }
        //public ICommand SQuary
        //{
        //    get
        //    {
        //        if (_quary == null)
        //        {
        //            _quary = new RelayCommand(SendQuary);
        //        }
        //        return _quary;
        //    }
        //}

        //private void SendQuary(object commandParamter)
        //{
        //    MessageBox.Show(FucNum + "  " + RelayNum);

        //    TextBind = Relay32Model.Quary(Convert.ToInt16(FucNum), Convert.ToInt16(RelayNum));
        //}

        //    public string FucNum 
        //    { 
        //        get; 

        //        set
        //        {
        //            Relay32Model.Text = value;
        //            OnPropertyChanged();
        //        }          
        //    }
        //    public string RelayNum 
        //    {
        //        set
        //        {
        //            Relay32Model.Text = value;
        //            OnPropertyChanged();
        //        }
        //        get; 

    //}
}
    }
