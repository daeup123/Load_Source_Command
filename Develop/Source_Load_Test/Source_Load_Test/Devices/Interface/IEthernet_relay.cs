using Source_Load_Test.Device.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Device.Interface
{
    interface IEthernet_relay : IVisaUsbDevice
    {
        string GetIDN();
        string GetError();
        int No { get; set; }
        double Current { get; set; }
        double Voltage { get; set; }
        double Power { get; set; }
        //void SetVoltage(string voltage);
        //void SetCurrent(string current);
        //void SetOutput(string s);

        void GetDataStatus();
    }
}

