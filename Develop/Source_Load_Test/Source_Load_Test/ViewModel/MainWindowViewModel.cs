using Source_Load_Test.Devices;
using Source_Load_Test.Model;
using Source_Load_Test.Properties;
using Source_Load_Test.View;
using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Source_Load_Test.Enums;

namespace Source_Load_Test.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            // 처음에 다 생성
            Setting();

            //// 시작 시 표시할 View 지정
            CurrentView = s_Page_Connect;
        }

        private Dictionary<PageType, ObservableObject> _pages = null;

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
            Console.WriteLine("dlsjddd");
            Console.WriteLine((string)sender);
            string param = (string)sender;

            PageType currnetpage = (PageType)Enum.Parse(typeof(PageType), param);
            BtnColor(currnetpage);
            if (_pages.TryGetValue(currnetpage, out var vm))
            {
                Console.WriteLine("dlsjddd");
                CurrentView = vm;   // ✅ 이제 실제 ViewModel이 바인딩됨
            }
        }

        #endregion

        #region 왼쪽 버튼

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
