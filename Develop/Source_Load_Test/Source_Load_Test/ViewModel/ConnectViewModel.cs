using Source_Load_Test.Viewmodel;
using Source_Load_Test.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Source_Load_Test.Model;

namespace Source_Load_Test.ViewModel
{
    public class ConnectViewModel : ObservableObject
    {        
        public ConnectViewModel() 
        {
            Console.WriteLine("ConnectViewModel Init");
        }

        public ICommand Connect
        {
            get
            {
                if (_connect == null)
                {
                    _connect = new RelayCommand(ConnectDevice);
                }

                return _connect;
            }
        }

        private RelayCommand _connect = null; // 연결&연결해제

        private void ConnectDevice(object commandParameter)
        {
            //string param = commandParameter.ToString();
            string param = (string)commandParameter;

            if(param == "Load Connect")
            {
                Console.WriteLine("로드를 연결.");

                DeviceManager.ConnectSerialDevices();
            }
            else if (param == "Source Connect")
            {
                Console.WriteLine("쏘스를 연결.");

                DeviceManager.ConnectUsbDevices();
                Console.WriteLine("값 설정.");

                DeviceManager.Source.SetValue("20", "14");
                Console.WriteLine("값 설정끝.");

                Console.WriteLine("값 조회.");
                List<string> lists = DeviceManager.Source.GetValue();
                
                foreach (string item in lists)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("값 조회끝.");

                DeviceManager.Source.Power("ON");
                Console.WriteLine("온!");
                //DeviceManager.Source.Power("OFF");
                Console.WriteLine("오프!!!");

                DeviceManager.Source.Init();


            }
            else
            {
                Console.WriteLine("장비연결해제.");

                DeviceManager.DisposeDevices();
            }
        }
    }
}
