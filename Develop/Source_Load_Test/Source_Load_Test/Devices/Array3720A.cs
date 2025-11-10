using RelayTest.Devices;
using Source_Load_Test.SCPI;
using Source_Load_Test.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Windows.Interop;
using Source_Load_Test.Model;
using System.Threading;
using System.Globalization;

namespace Source_Load_Test.Devices
{
    public class Array3720A : VisaSerialDevice
    {
        public Array3720A() 
        {
            Console.WriteLine("Load Init");
        }
        protected override string QueryMessage(string message) // 공백한줄을보내서 리시브한번추가
        {
            SendMessage(message);
            string msg = ReceiveMessage();
            Console.WriteLine("쿼리메시지 리턴 문자열 : " + msg);
            ReceiveMessage();

            return msg;
        }
        public override bool IsConnected => Session != null && !Session.IsDisposed;
        //protected override string ReceiveMessage()
        //{
        //    return Session.FormattedIO.ReadString();
        //}

        private string AddrnToStr(string str) // 문자열끝에 개행추가가 필요가없네요
        {
            string returnstr = str + "\r\n";

            return returnstr;
        }

        public string GetIDN()
        {
            return QueryMessage(ScpiLoad.Identify);
        }
        public void Init()
        {
            string msg = AddrnToStr(ScpiLoad.Reset);
            //string msg = "*RST" 장비상태 초기화
            SendMessage(msg);
        }
        
        public void SetValue(Mode mode, string value) // 설정하기
        {
            // v = 0 ~ 40
            // i = 0 ~ 30
            //string msg = ":SOURce:VOLTage:SET CH1, " + v; // 2로 설정시 0.002 A로 설정됨
            string msg = "";

            if (mode == Mode.CV) // 정전압
            {
                msg = AddrnToStr(ScpiLoad.ModeCV);
                SendMessage(msg);

                msg = AddrnToStr(string.Format(ScpiLoad.VoltageSet, value));
                SendMessage(msg);
                // 전압설정이 없네?
            }
            else if (mode == Mode.CC) // 정전류
            {
                msg = (ScpiLoad.ModeCC);
                SendMessage(msg);
                
                msg = (string.Format(ScpiLoad.CurrentSet, value));
                SendMessage(msg);
            }
            else if(mode == Mode.CR) // 정저항
            {
                msg = AddrnToStr(ScpiLoad.ModeCR);
                SendMessage(msg);

                msg = AddrnToStr(string.Format(ScpiLoad.ResistanceSet, value));
                SendMessage(msg);
            }
            else if(mode == Mode.CP) // 정전력
            {
                msg = AddrnToStr(ScpiLoad.ModeCP);
                SendMessage(msg);

                //msg = AddrnToStr()
            }
        }

        private readonly object _commLock = new();

        public async Task<Data> GetValue() // 측정값 받기
        {
            lock (_commLock)
            {
                Console.WriteLine("@@@@@@@@@@@@@Load GetValue Start@@@@@@@@@@");
                string msg = "";
                double voltage = 0;
                double current = 0;

                msg = (ScpiLoad.VoltageMeasure);
                voltage = double.Parse(QueryMessage(msg), CultureInfo.InvariantCulture);

                msg = (ScpiLoad.CurrentMeasure);
                current = double.Parse(QueryMessage(msg), CultureInfo.InvariantCulture);

                Data data = new Data() { Voltage = voltage, Current = current };

                return data;
            }
        }

        public bool Power(string commandParameter)
        {
            bool result = false;

            string msg = "";
            if (commandParameter == "ON")
            {
                msg = AddrnToStr(ScpiLoad.LoadOn);
                result = true;
            }
            else if (commandParameter == "OFF")
            {
                msg = AddrnToStr(ScpiLoad.LoadOff);
                result = false;
            }
            else
            { 
                msg = AddrnToStr(ScpiLoad.LoadOff);
                Console.WriteLine("Load ON/OFF 실패");
                result = false;
            }

            SendMessage(msg);
            return result;
        }
    }
}
