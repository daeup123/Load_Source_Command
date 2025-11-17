#define Simulator

using Ivi.Visa;
using NationalInstruments.Visa;
using RelayTest.Device.Interface;
using System;
using System.Diagnostics;
using System.Threading;

namespace RelayTest.Devices
{
    public abstract class VisaSerialDevice : IVisaSerialDevices
    {

        public Exception LastException;
        public int BaudRate { get => Session.BaudRate; set => Session.BaudRate = value; }
        public short DataBits { get => Session.DataBits; set => Session.DataBits = value; }
        public SerialParity Parity { get => Session.Parity; set => Session.Parity = value; }
        public SerialStopBitsMode StopBits { get => Session.StopBits; set => Session.StopBits = value; }
        public SerialFlowControlModes FlowControl { get => Session.FlowControl; set => Session.FlowControl = value; }
        public bool IsDisposed => Session != null && Session.IsDisposed;

        public abstract bool IsConnected { get; }
        protected SerialSession Session;
        public virtual void SetSession(SerialSession session)
        {
            Session = session;

            // 0x0A가 종료 문자로 인식되지 않도록 설정
            session.TerminationCharacterEnabled = false; // 종료 문자 기능 비활성화
            session.SendEndEnabled = false; // 메시지 끝 자동 전송 비활성화

            // 혹시나 0x0A가 잘리는 문제가 여전히 발생한다면,
            // TerminationCharacter를 다른 값으로 설정
            session.TerminationCharacter = (byte)'\n'; // '\r'로 변경
        }

        public virtual SerialSession GetSession() => Session;

        public void Dispose()
        {
            if (Session != null && IsDisposed == false)
            {
                Session.Dispose();
            }
        }
        public bool GetConnected()
        {
            return Session.Connected;
            //return true; //Session.Connected; => INVALID가 IO Trace에서 나오는 이유를 모르겠음. 
        }

        protected virtual void SendMessage(string msg)
        {
            Session.FormattedIO.WriteLine(msg);
        }
        protected virtual string ReceiveMessage()
        {
            string str = Session.FormattedIO.ReadString();
            //string str = Session.FormattedIO.ReadLine();
            return str; // 무한스레드때문에
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
        protected virtual byte[] ReceiveByte(int lenth)
        {
           return Session.RawIO.Read(lenth);
        }
        protected virtual byte[] QueryByte(byte[] bytes, int length)
        {
            SendByte(bytes);
            Thread.Sleep(70);
            return ReceiveByte(length);            
        }
    }
}
