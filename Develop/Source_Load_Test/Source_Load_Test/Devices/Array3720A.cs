using Microsoft.SqlServer.Server;
using RelayTest.Devices;
using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.SCPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Source_Load_Test.Devices
{
    public class Array3720A : VisaSerialDevice
    {
        public Array3720A() 
        {
            Debug.WriteLine("Load Init");
        }
        protected override string QueryMessage(string message) // 공백한줄을보내서 리시브한번추가
        {
            SendMessage(message);
            string msg = ReceiveMessage();
            Debug.WriteLine("쿼리메시지 리턴 문자열 : " + msg);

            return msg;
        }
        public override bool IsConnected => Session != null && !Session.IsDisposed;

        //private string AddrnToStr(string str) // 문자열끝에 개행추가가 필요가없네요
        //{
        //    string returnstr = str + "\r\n";

        //    return returnstr;
        //}

        public string GetIDN()
        {
            return QueryMessage(ScpiLoad.Identify);
        }
        public void Init()
        {
            string msg = (ScpiLoad.Reset);
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
                msg = (ScpiLoad.ModeCV);
                SendMessage(msg);

                msg = (string.Format(ScpiLoad.VoltageSet, value));
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
                msg = (ScpiLoad.ModeCR);
                SendMessage(msg);

                msg = (string.Format(ScpiLoad.ResistanceSet, value));
                SendMessage(msg);
            }
            else if(mode == Mode.CP) // 정전력
            {
                msg = (ScpiLoad.ModeCP);
                SendMessage(msg);

                //msg = AddrnToStr()
            }
        }       
        
        private static readonly object _commLock = new();

        public async Task<Data> GetValue() // 측정값 받기
        {
            lock (_commLock)
            {
                Debug.WriteLine("@@@@@@@@@@@@@Load GetValue Start@@@@@@@@@@");
                string msg = "";
                float voltage = 0;
                float current = 0;

                msg = (ScpiLoad.VoltageMeasure);
                voltage = float.Parse(QueryMessage(msg), CultureInfo.InvariantCulture);

                msg = (ScpiLoad.CurrentMeasure);
                current = float.Parse(QueryMessage(msg), CultureInfo.InvariantCulture);

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
                msg = (ScpiLoad.LoadOn);
                result = true;
            }
            else if (commandParameter == "OFF")
            {
                msg = (ScpiLoad.LoadOff);
                result = false;
            }
            else
            { 
                msg = (ScpiLoad.LoadOff);
                Debug.WriteLine("Load ON/OFF 실패");
                result = false;
            }

            SendMessage(msg);
            return result;
        }
    }
}
