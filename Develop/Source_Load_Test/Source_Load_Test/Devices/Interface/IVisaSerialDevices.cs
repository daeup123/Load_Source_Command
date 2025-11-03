using Ivi.Visa;
using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayTest.Device.Interface
{
    public interface IVisaSerialDevices : IDisposable
    {
        int BaudRate { get; set; }
        short DataBits { get; set; }
        SerialParity Parity { get; set; }
        SerialStopBitsMode StopBits { get; set; }
        SerialFlowControlModes FlowControl { get; set; }
        bool IsDisposed { get; }
        bool IsConnected { get; }
        void SetSession(SerialSession session);
    }
}
