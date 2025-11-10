using NationalInstruments.Visa;
using RelayTest.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Source_Load_Test.SCPI;

namespace Source_Load_Test.Devices
{
    public class SIGLENT_SPS5041X : VisaUsbDevice
    {
        public SIGLENT_SPS5041X()
        {
            Console.WriteLine("Source Init");
        }
        public override bool IsConnected => Session != null && !Session.IsDisposed;

        public void Init()
        {
            //string msg = "*RST" 장비상태 초기화
            SendMessage(ScpiSource.Reset);
        }
        
        public string GetIDN()
        {
            return QueryMessage(ScpiSource.Identify);
        }
        public void SetValue(string v = "0", string i = "0") // 설정하기
        {
            // v = 0 ~ 40
            // i = 0 ~ 30
            //string msg = ":SOURce:VOLTage:SET CH1, " + v; // 2로 설정시 0.002 A로 설정됨

            string msg = string.Format(ScpiSource.VoltageSet, v);
            SendMessage(msg);

            //msg = ":SOURce:CURRent:SET CH1, " + i;
            msg = string.Format(ScpiSource.CurrentSet, i);

            SendMessage(msg);
        }

        public List<string> GetValue() // 설정한 값 조회하기
        {
            List<string> value = new List<string>();
            // :SOURce:VOLTage:SET? CH1 해당 채널에 설정된 전압?
            value.Add(QueryMessage(ScpiSource.VoltageSetQuery)); // 전압
            value.Add(QueryMessage(ScpiSource.CurrentSetQuery)); // 전류
            value.Add(QueryMessage(ScpiSource.PowerMeasure));    // 전력

            return value;
        }
        //MEASure:RUN:MODE? CH1 동작모드 조회

        public List<string> GetMeans() // 실제 측정값 조회
        {
            List<string> value = new List<string>();

            value.Add(QueryMessage(ScpiSource.VoltageMeasure)); // 전압
            value.Add(QueryMessage(ScpiSource.CurrentMeasure)); // 전류
            value.Add(QueryMessage(ScpiSource.PowerMeasure));   // 전력

            return value;
        }
        //:MEAS:VOLT? CH1
        public void Power(string commandParam, string voltage, string currnet) // Output ON/OFF
        {
            string msg = "";
            if (commandParam == "ON")
            {
                SetValue(voltage, currnet); // 설정
                msg = "OUTPut 1";
            }
            else if (commandParam == "OFF")
            {
                Init(); // 초기화
                msg = "OUTPut 0";
            }

            SendMessage(msg);
        }
    }
}
