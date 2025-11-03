using Ivi.Visa;
using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Device.Interface
{
    public interface IVisaTcpipDevice : IDisposable
    {
        bool IsDisposed { get; }
        bool IsConnected { get; }

        void SetSocket(TcpipSocket socket);
    }
}
