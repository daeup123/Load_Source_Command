using NationalInstruments.Visa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YHtec_Load_Tester.Devices
{
    public abstract class VisaGpibDevice
    {
        public Exception LastException;
        public string HardwareInterfaceName => Session?.HardwareInterfaceName;
        public short HardwareInterfaceNumber => Session.HardwareInterfaceNumber;
        public Ivi.Visa.HardwareInterfaceType HardwareInterfaceType => Session.HardwareInterfaceType;

        public string ResourceName => Session.ResourceName;
        public short PrimaryAddress => Session.PrimaryAddress;
        public short SecondaryAddress => Session.SecondaryAddress;

        public bool IsDisposed => Session != null && Session.IsDisposed;
        public abstract bool IsConnected { get; }
        protected GpibSession Session { get; set; }         // Static으로 하면 VisaGpibDevice클래스가 GPIB 통신 장비 갯수 만큼 있어야 함. 

        public virtual void SetSession(GpibSession session) => Session = session;
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
