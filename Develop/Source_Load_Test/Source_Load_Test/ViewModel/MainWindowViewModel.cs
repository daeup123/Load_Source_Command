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
        private void Setting() // 바인딩된 뷰모델들
        {
            s_Page_Connect = new ConnectViewModel();
            s_Page_Load = new LoadViewModel();
            s_Page_Source = new SourceViewModel();
            s_Page_Main = new MonitorViewModel();
        }

        #region 화면전환
        private static object s_Page_Connect = null;
        private static object s_Page_Load = null;
        private static object s_Page_Source = null;
        private static object s_Page_Main = null;

        private object _currentView; // 
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
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
            Console.WriteLine("클릭");
            Console.WriteLine(sender.ToString());   
            string page = sender.ToString();
            Navigate(page);
        }

        private void Navigate(string page)
        {
            string _page = page.ToString();
            switch (_page)
            {
                case "Main":
                    Console.Write("Main");
                    CurrentView = s_Page_Main;
                    break;

                case "Connect":
                    Console.WriteLine("Connect");
                    CurrentView = s_Page_Connect;
                    break;

                case "Load":
                    Console.WriteLine("Load");
                    CurrentView = s_Page_Load;
                    break;

                case "Source":
                    Console.WriteLine("Source");
                    CurrentView = s_Page_Source;
                    break;

                default:
                    Console.WriteLine("Error!!");
                    break;
            }
        }
        #endregion
        
        //public ObservableObject CurrentTopMenu { get; set; }

    }
}
