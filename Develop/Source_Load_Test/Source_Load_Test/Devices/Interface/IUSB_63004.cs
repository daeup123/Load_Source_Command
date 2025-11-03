using Source_Load_Test.Device.Interface;
using Source_Load_Test.Devices.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Devices.Interface
{
    interface IUSB_D63004 : IVisaUsbDevice
    {
        string GetIDN();
        string GetError();
        void SetVoltage(string voltage);
        void SetCurrent(string current);
        void SetOutput(string s);
        string SetCurrentRange(string s);
        double GetMeasureCurrent();
        double GetMeasureVoltage();
        void LoadProptectionClear();

        void SetLowVoltRange();

        void SetMidVoltRange();
        void SetHighVoltRange();
        void SetSetNSEL(string s);
        void SetSetNSEL(int n);
        void SetWaveInterN(string s);
        void SetWaveInterN(int n1, double d, int n2, int n3, string s);
        void SetWaveClear(int n);
        void SetWaveDataPoint(double d);
        void GetDataStatus();
    }
}

