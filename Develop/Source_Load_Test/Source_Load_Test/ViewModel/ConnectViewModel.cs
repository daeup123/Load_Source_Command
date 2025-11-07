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
            Console.WriteLine("ConnectViewModel ConnectDevice: " + commandParameter.ToString());
            //string param = commandParameter.ToString();
            string param = (string)commandParameter;

            if(param == "Load Connect")
            {
                Console.WriteLine("Load Connect");
                DeviceManager.ConnectSerialDevices();
                Console.WriteLine("Load Connect !!!");
                //DeviceManager.Load.Init();
            }
            else if (param == "Source Connect")
            {
                Console.WriteLine("Source Connect");
                DeviceManager.ConnectUsbDevices();
                Console.WriteLine("Source Connect !!!");

                //DeviceManager.Source.Init();
            }
            else
            {
                Console.WriteLine("장비연결해제.");
                //DeviceManager.Load.Init();
                //DeviceManager.Source.Init();
                DeviceManager.DisposeDevices();
            }
        }
    }
}
