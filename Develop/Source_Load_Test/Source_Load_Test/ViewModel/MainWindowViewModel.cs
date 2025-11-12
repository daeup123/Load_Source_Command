using Source_Load_Test.Devices;
using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.Properties;
using Source_Load_Test.View;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Source_Load_Test.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            
            Setting();  // 처음에 다 생성

            CheckDevice();  // 장비연결 체크 비동기
        }

        private Dictionary<PageType, ObservableObject> _pages = null;
        private DeviceCheckConnection _deviceCheckConnection = new DeviceCheckConnection();

        private bool _isSourceConnected = false;
        private bool _isLoadConnected = false;

        public bool IsSourceConnected
        {
            get => _isSourceConnected;
            set => SetProperty(ref _isSourceConnected, value);
        }
        public bool IsLoadConnected
        {
            get => _isLoadConnected;
            set => SetProperty(ref _isLoadConnected, value);
        }

        private async Task<bool> CheckDevice()
        {
            bool retry = true;
            while(true)
            {
                IsSourceConnected = await _deviceCheckConnection.CheckConnectionSource();
                IsLoadConnected = await _deviceCheckConnection.CheckConnectionLoad();

                if(!(IsSourceConnected && IsLoadConnected) && (retry == true))
                {                   
                    GotoConnectView();
                    retry = false;
                }
                else
                {
                    retry = true;
                }

                Debug.WriteLine("장비 연결 상태 체크중...");
                await Task.Delay(1000);
            }           
        }

        private void Setting() // 바인딩된 뷰모델들
        {
            s_Page_Connect = new ConnectViewModel();
            s_Page_Load = new LoadViewModel();
            s_Page_Source = new SourceViewModel();
            s_Page_Main = new MonitorViewModel();
                        
            _pages = new Dictionary<PageType, ObservableObject>
            {
                { PageType.Main, s_Page_Main },
                { PageType.Connect, s_Page_Connect },
                { PageType.Load, s_Page_Load },
                { PageType.Source, s_Page_Source }
            };

            GotoConnectView();
        }

        private void GotoConnectView()
        {
            CurrentView = s_Page_Connect;  //
            BtnColor(PageType.Connect); // 
        }
        #region 화면전환

        private static ObservableObject s_Page_Connect = null;
        private static ObservableObject s_Page_Load = null;
        private static ObservableObject s_Page_Source = null;
        private static ObservableObject s_Page_Main = null;

        private object _currentView; // 
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value); // 똑같다!
            //set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        private RelayCommand _clickLeftMenu = null;
        public ICommand ClickLeftMenu
        {
            get
            {
                if (_clickLeftMenu == null)
                {
                    _clickLeftMenu = new RelayCommand(ClickLeftbtns);
                }
                return _clickLeftMenu;
            }
        }

        private void ClickLeftbtns(object sender)
        {
            Debug.WriteLine("페이지바꿔");
            Debug.WriteLine((string)sender);
            string param = (string)sender;

            PageType currnetpage = (PageType)Enum.Parse(typeof(PageType), param);
            if ((currnetpage == PageType.Load))
            {
                Debug.WriteLine("로드화면이동");

                //if (!DeviceManager.Load.IsConnected)
                //{
                //    Debug.WriteLine("로드장비없음");
                //    MessageBox.Show("로드장비가 연결되어있지 않습니다.");
                //    return;
                //}
                //else
                //{

                //}
            }
            else if(currnetpage == PageType.Source)
            {
                //Debug.WriteLine("쏘스화면이동");
                //if(!DeviceManager.Source.IsConnected)
                //{
                //    Debug.WriteLine("쏘스장비없음");
                //    MessageBox.Show("쏘스장비가 연결되어있지 않습니다.");
                //    return;
                //}
                //else
                //{

                //}
            }
            else if(currnetpage == PageType.Main)
            {
                // 장비연결되어있고 장비가 동작중일떄만 접근 가능 로직
            }

                BtnColor(currnetpage);
            if (_pages.TryGetValue(currnetpage, out var vm))
            {
                Debug.WriteLine("페이지 변경!");
                CurrentView = vm;   // ✅ 이제 실제 ViewModel이 바인딩됨
            }
        }

        #endregion

        #region 왼쪽 버튼 색깔

        private bool _btnMain = false;       // 메인
        private bool _btnConnect = false;    // 커넥트
        private bool _btnLoad = false;       // 로드
        private bool _btnSource = false;     // 쏘스

        public bool BtnMain
        {
            get => _btnMain;
            set
            {
                SetProperty(ref _btnMain, value);
            }
        }
        public bool BtnConnect
        { 
            get 
            { 
                return _btnConnect; 
            }
            set
            {
                SetProperty(ref _btnConnect, value);
            }        
        }
        public bool BtnLoad
        { 
            get 
            { 
                return _btnLoad; 
            } 
            set 
            {
                SetProperty(ref _btnLoad, value);
            } 
        }
        public bool BtnSource
        {
            get 
            { 
                return _btnSource; 
            } 
            set 
            {
                SetProperty(ref _btnSource, value);
            } 
        }

        private void BtnColor(PageType activePage)
        {
            BtnMain = activePage == PageType.Main;
            BtnConnect = activePage == PageType.Connect;
            BtnLoad = activePage == PageType.Load;
            BtnSource = activePage == PageType.Source;
        }

        #endregion

    }
}
