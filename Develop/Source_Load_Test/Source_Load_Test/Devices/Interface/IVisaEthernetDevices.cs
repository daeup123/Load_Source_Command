using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Device.Interface
{
    public interface IVisaEthernetDevices : IDisposable
    {
        //string ModelName { get; }
        //string ManufacturerName { get; }
        //string UsbSerialNumber { get; }
        bool IsDisposed { get; }
        void SetSocket(TcpipSocket socket);
    }
}
