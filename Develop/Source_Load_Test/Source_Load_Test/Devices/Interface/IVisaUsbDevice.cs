using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Device.Interface
{
    public interface IVisaUsbDevice : IDisposable
    {
        string ModelName { get; }
        string ManufacturerName { get; }
        string UsbSerialNumber { get; }
        bool IsDisposed { get; }
        void SetSession(UsbSession session);
    }
}
