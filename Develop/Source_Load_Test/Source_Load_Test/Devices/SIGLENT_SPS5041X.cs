using NationalInstruments.Visa;
using RelayTest.Devices;
using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.SCPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Devices
{
    public class SIGLENT_SPS5041X : VisaUsbDevice
    {
        public SIGLENT_SPS5041X()
        {
            
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
        public void SetValue(Mode mode, string v = "0", string i = "0") // 설정하기
        {
            // v = 0 ~ 40
            // i = 0 ~ 30
            //string msg = ":SOURce:VOLTage:SET CH1, " + v; // 2로 설정시 0.002 A로 설정됨
            string msg = string.Format(ScpiSource.ModeSet, mode.ToString());
            Debug.WriteLine(msg);
            SendMessage(msg);

            msg = string.Format(ScpiSource.VoltageSet, v);
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

        private static readonly object _lock = new();

        public Data GetMeans() // 실제 측정값 조회
        {
            lock(_lock)
            {
                Data value = new Data();

                value.Voltage = float.Parse(QueryMessage(ScpiSource.VoltageMeasure)); // 전압
                value.Current = float.Parse(QueryMessage(ScpiSource.CurrentMeasure)); // 전류
                value.Mode = QueryMessage(ScpiSource.ModeQuery).Substring(0, 2); // 동작모드
                return value;
            }
        }
        //:MEAS:VOLT? CH1
        public bool Power(string commandParameter)
        {
            bool result = false;

            string msg = "";
            if (commandParameter == "ON")
            {
                msg = (ScpiSource.OutputOn);
                result = true;
            }
            else if (commandParameter == "OFF")
            {
                msg = (ScpiSource.OutputOff);
                result = false;
            }
            else
            {
                msg = (ScpiSource.OutputOff);
                Debug.WriteLine("Load ON/OFF 실패");
                result = false;
            }

            SendMessage(msg);
            return result;
        }
    
        public bool SetSlewRate(string mode, string voltageSlew = "0", string currentSlew = "0")
        {
            try
            {
                bool result = false;

                if ((voltageSlew != "0" ) && (currentSlew != "0"))
                {
                    if (mode == "RISE")
                    {
                        result = SetSlewRies(voltageSlew, currentSlew);

                    }
                    else if (mode == "FALL")
                    {
                        result = SetSlewFall(voltageSlew, currentSlew); 
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SetSlewRate 실패: " + ex.Message);
                return false;
            }
        }

        private bool SetSlewRies(string voltageSlew = "0", string currentSlew = "0")
        {
            string msg = "";
            string receiveStr = "";
            bool result = false;

            if (voltageSlew != "0")
            {
                msg = string.Format(ScpiSource.VoltageSlewRIESSet, voltageSlew);
                SendMessage(msg);
                msg = (ScpiSource.VoltageSlewRIESQuery);
                receiveStr = QueryMessage(msg);
                if(voltageSlew.ToString() == receiveStr)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            if (currentSlew != "0")
            {
                msg = string.Format(ScpiSource.CurrentSlewRIESSet, currentSlew);
                SendMessage(msg);
                msg = (ScpiSource.CurrentSlewRIESQuery);
                receiveStr = QueryMessage(msg);
                if(currentSlew.ToString() == receiveStr)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        private bool SetSlewFall(string voltageSlew = "0", string currentSlew = "0")
        {
            string msg = "";
            string receiveStr = "";
            bool result = false;

            if(voltageSlew != "0")
            {
                msg = string.Format(ScpiSource.VoltageSlewFALLSet, voltageSlew);
                SendMessage(msg);
                msg = (ScpiSource.VoltageSlewFALLQuery);
                receiveStr = QueryMessage(msg);
                if(voltageSlew.ToString() == receiveStr)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }   
            }
            if(currentSlew != "0")
            {
                msg = string.Format(ScpiSource.CurrentSlewFALLSet, currentSlew);
                SendMessage(msg);
                msg = (ScpiSource.CurrentSlewFALLQuery);
                receiveStr = QueryMessage(msg);
                if(currentSlew.ToString() == receiveStr)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

    }
}
