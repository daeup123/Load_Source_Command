using NationalInstruments.Visa;
using RelayTest.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Devices
{
    public class SIGLENT_SPS5041X : VisaUsbDevice
    {
        public SIGLENT_SPS5041X()
        {
            Console.WriteLine("Source Init");
        }

        public string SetV { get; private set; } = "0";
        public string SetI { get; private set; } = "0";
        public string GetV { get; private set; } = "0";
        public string GetI { get; private set; } = "0";
        public string GetP { get; private set; } = "0";

        public void Init()
        {
            //string msg = "*RST" 장비상태 초기화
            SendMessage("*RST");
        }
        public void SetValue(string v = "0", string i = "0") // 설정하기
        {
            // v = 0 ~ 40
            // i = 0 ~ 30
            string msg = ":SOURce:VOLTage:SET CH1, " + v; // 2로 설정시 0.002 A로 설정됨

            SendMessage(msg);

            msg = ":SOURce:CURRent:SET CH1, " + i;

            SendMessage(msg);
        }

        public List<string> GetValue() // 설정한 값 조회하기
        {
            List<string> value = new List<string>();

            value.Add(QueryMessage(":SOURce:VOLTage:SET? CH1")); // 전압
            value.Add(QueryMessage(":SOURce:CURRent:SET? CH1")); // 전류
            value.Add(QueryMessage("MEASure:POWER? CH1"));       // 전력

            return value;
        }
        //MEASure:RUN:MODE? CH1 동작모드 조회

        public List<string> GetMeans() // 실제 측정값 조회
        {
            List<string> value = new List<string>();

            value.Add(QueryMessage("MEASure:VOLTage? CH1")); // 전압
            value.Add(QueryMessage("MEASure:CURRent? CH1")); // 전류
            value.Add(QueryMessage("MEASure:POWER? CH1"));       // 전력

            return value;
        }
        //:MEAS:VOLT? CH1
        public List<string> Power(string commandParam, string voltage, string currnet) // Output ON/OFF
        {
            List<string> setResponce = new List<string>();

            List<string> getResopne = new List<string>();

            string msg = "";
            if(commandParam == "Start")
            {
                SetValue(voltage, currnet); // 설정
                setResponce = GetValue();      // 설정값 확인
                //getResopne = GetMeans(); // 실제 측정값 확인
                msg = "OUTPut 1";
            }
            else if(commandParam == "Stop")
            {
                Init();
                msg = "OUTPut 0";
            }

            SendMessage(msg);
            return setResponce;
        }
    }
}
