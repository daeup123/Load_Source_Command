using RelayTest.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Devices
{
    public class Array3720A : VisaSerialDevice
    {
        public Array3720A() 
        { 
        
        }
        public override bool IsConnected => GetConnected(); //Session.Connected;

        public void Init()
        {
            //string msg = "*RST" 장비상태 초기화
            SendMessage("*RST");
        }
    }
}
