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

namespace Source_Load_Test.Devices
{
    public class Array3720A : VisaSerialDevice
    {
        public Array3720A() 
        {
            Console.WriteLine("Load Init");
        }
        public override bool IsConnected => Session != null && !Session.IsDisposed;
        private string AddrnToStr(string str) // 문자열끝에 개행추가
        {
            string returnstr = str + "\r\n";

            return returnstr;
        }

        //public string _deviceIDN
        //{
        //    get => GetIDN();
        //    set =>
        //}

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

            if (mode == Mode.CC) // 정전압
            {
                msg = AddrnToStr(ScpiLoad.ModeCC);
                SendMessage(msg);

                msg = AddrnToStr(string.Format(ScpiLoad.VoltageSet, value));
                SendMessage(msg);
                // 전압설정이 없네?
            }
            else if (mode == Mode.CC) // 정전류
            {
                msg = AddrnToStr(ScpiLoad.ModeCC);
                SendMessage(msg);
                
                msg = AddrnToStr(string.Format(ScpiLoad.CurrentSet, value));
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
                SendMessage(msg);
        }
        
        public void Power(string commandParameter)
        {
            string msg = "";
            if(commandParameter == "ON")
            {
                msg = AddrnToStr(ScpiLoad.LoadOn);
            }
            else if(commandParameter == "OFF")
            {
                msg = AddrnToStr(ScpiLoad.LoadOff);
            }
            else
            {
                Console.WriteLine("Load ON/OFF 실패");
            }
            SendMessage(msg);
        }
    }
}
