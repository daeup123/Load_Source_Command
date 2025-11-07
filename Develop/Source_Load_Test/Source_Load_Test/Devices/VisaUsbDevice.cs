using Source_Load_Test.Device.Interface;
using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayTest.Devices
{

    public abstract class VisaUsbDevice
    {
        public Exception LastException;
        public string ModelName => Session?.ModelName;
        public string ManufacturerName => Session?.ManufacturerName;
        public string UsbSerialNumber => Session?.UsbSerialNumber;
        public bool IsDisposed => Session != null && Session.IsDisposed;
        public abstract bool IsConnected { get; }
        protected UsbSession Session { get; set; }

        public virtual void SetSession(UsbSession session) => Session = session;
        public void Dispose()
        {
            if (Session != null && IsDisposed == false)
            {
                Session.Dispose();
            }
        }

        protected virtual void SendMessage(string msg)
        {
            Session.FormattedIO.WriteLine(msg);
        }
        protected virtual string ReceiveMessage()
        {
            return Session.FormattedIO.ReadLine();
        }
        protected virtual string QueryMessage(string msg)
        {
            SendMessage(msg);
            return ReceiveMessage();
        }
        protected virtual void SendByte(byte[] bytes)
        {
            Session.RawIO.Write(bytes);
        }
        protected virtual byte[] ReceiveByte()
        {
            return Session.RawIO.Read();
        }
        protected virtual byte[] QueryByte(byte[] bytes)
        {
            SendByte(bytes);
            return ReceiveByte();
        }
    }
}
