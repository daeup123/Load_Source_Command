using RelayTest.Devices;
using Source_Load_Test.Devices.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Source_Load_Test.Devices
{
    public class USB_D63004 : VisaUsbDevice
    {
        public string GetIDN()
        {
            return QueryMessage("*IDN?");
        }
        public void SetClear()
        {
            SendMessage("*CLS");
        }

        public void SetVoltageon_CC(string voltage)
        {
            SendMessage("CONF:VOLT:ON " + voltage);
        }

        public void SetLowVoltRange_CC()
        {
            SendMessage("CONF:VOLT:RANG L ");
        }
        public void SetMidVoltRange_CC()
        {
            SendMessage("CONF:VOLT:RANG M ");
        }
        public void SetHighVoltRange_CC()
        {
            SendMessage("CONF:VOLT:RANG H ");
        }

        public void SetLowVoltRange_CP()
        {
            SendMessage("CONF:VOLT:RANG L ");
        }
        public void SetMidVoltRange_CP()
        {
            SendMessage("CONF:VOLT:RANG M ");
        }
        public void SetHighVoltRange_CP()
        {
            SendMessage("CONF:VOLT:RANG H ");
        }
        public void SetLowCurRange_CR()
        {
            SendMessage("RES:STAT:IRNG L ");
        }
        public void SetMidCurRange_CR()
        {
            SendMessage("RES:STAT:IRNG M ");
        }
        public void SetHighCurRange_CR()
        {
            SendMessage("RES:STAT:IRNG H ");
        }
        public void SetLowPowerRange_CP()
        {
            SendMessage("POW:STAT:PRNG L ");
        }
        public void SetMidPowerRange_CP()
        {
            SendMessage("POW:STAT:PRNG M ");
        }
        public void SetHighPowerRange_CP()
        {
            SendMessage("POW:STAT:PRNG H ");
        }

        public void SetCurrent_CC(string current)
        {
            SendMessage("CURR:STAT:L1 " + current);
        }

        public void SetResistance_CR(string resistance)
        {
            SendMessage("RES:STAT:L1 " + resistance);
        }

        public void SetPower_CP(string resistance)
        {
            SendMessage("POW:STAT:L1 " + resistance);
        }

        public void SetVoltageon_CR(string voltage)
        {
            SendMessage("CONF:VOLT:ON " + voltage);
        }
        public void SetVoltageon(string voltage)
        {
            SendMessage("CONF:VOLT:ON " + voltage);
        }

        public void SetVoltageon_CP(string voltage)
        {
            SendMessage("CONF:VOLT:ON " + voltage);
        }
        public void SetVoltageon_CCD(string voltage)
        {
            SendMessage("CONF:VOLT:ON " + voltage);
        }

        public void SetVoltage_CV(string voltage)
        {
            SendMessage("VOLT:STAT:L1 " + voltage);
        }

        public string SetCurrentRange_CC(string s)
        {
            return QueryMessage("CURR:STAT:VRNG " + s);
        }

        public void SetCurrentIimit_CV(string current)
        {
            SendMessage("VOLT:STAT:ILIM " + current);
        }
        public void SetSlowResponse_CV()
        {
            SendMessage("VOLT:STAT:RES SLOW ");
        }
        public void SetNormalResponse_CV()
        {
            SendMessage("VOLT:STAT:RES NORMAL ");
        }
        public void SetFastResponse_CV()
        {
            SendMessage("VOLT:STAT:RES FAST ");
        }
        public string SetCurrentRange_CR(string s)
        {
            return QueryMessage("RES:STAT:IRNG " + s);
        }
        public string SetVoltageRange_CCD(string s)
        {
            return QueryMessage("CURR:DYN:VRNG " + s);
        }
        public void SetOutput(string s)
        {
            SendMessage("LOAD " + s);
        }

        public void LoadProptectionClear()
        {
            SendMessage("LOAD:PROT:CLE");
        }
        public void SetCurrent1_CCD(string current)
        {
            SendMessage("CURR:DYN:L1 " + current);
        }
        public void SetCurrent2_CCD(string current)
        {
            SendMessage("CURR:DYN:L2 " + current);
        }
        public void SetParameter1_CCD(string parameter)
        {
            SendMessage("CURR:DYN:T1 " + parameter);
        }
        public void SetParameter2_CCD(string parameter)
        {
            SendMessage("CURR:DYN:T2 " + parameter);
        }
        public void SetRepeatCount_CCD(string repeatcount)
        {
            SendMessage("CURR:DYN:REP " + repeatcount);
        }
        public void SetSlewRise_CCD(string rate)
        {
            SendMessage("CURR:DYN:RISE " + rate);
        }
        public void SetSlewFall_CCD(string rate)
        {
            SendMessage("CURR:DYN:FALL " + rate);
        }
        public void SetLowVoltRange_CCD()
        {
            SendMessage("CURR:DYN:VANG L ");
        }
        public void SetMidVoltRange_CCD()
        {
            SendMessage("CURR:DYN:VANG M ");
        }
        public void SetHighVoltRange_CCD()
        {
            SendMessage("CURR:DYN:VANG H ");
        }
        public void SetResistance1_CRD(string current)
        {
            SendMessage("RES:DYN:L1 " + current);
        }
        public void SetResistance2_CRD(string current)
        {
            SendMessage("RES:DYN:L2 " + current);
        }
        public void SetParameter1_CRD(string parameter)
        {
            SendMessage("RES:DYN:T1 " + parameter);
        }
        public void SetParameter2_CRD(string parameter)
        {
            SendMessage("RES:DYN:T2 " + parameter);
        }
        public void SetRepeatCount_CRD(string repeatcount)
        {
            SendMessage("RES:DYN:REP " + repeatcount);
        }
        public void SetSlewRateRise_CRD(string rate)
        {
            SendMessage("RES:DYN:RISE " + rate);
        }
        public void SetSlewRateFall_CRD(string rate)
        {
            SendMessage("RES:DYN:FALL " + rate);
        }
        public void SetLowCurrentRange_CRD()
        {
            SendMessage("RES:DYN:IANG L ");
        }
        public void SetMidCurrentRange_CRD()
        {
            SendMessage("RES:DYN:IANG M ");
        }
        public void SetHighCurrentRange_CRD()
        {
            SendMessage("RES:DYN:IANG H ");
        }
        public void SetSlewRise_CC(string rate)
        {
            SendMessage("CURR:STAT:RISE " + rate);
        }
        public void SetSlewfall_CC(string rate)
        {
            SendMessage("CURR:STAT:FALL " + rate);
        }
        public void SetSlewRise_CR(string rate)
        {
            SendMessage("RES:STAT:RISE " + rate);
        }
        public void SetSlewfall_CR(string rate)
        {
            SendMessage("RES:STAT:FALL " + rate);
        }
        public void SetSlewRise_CP(string rate)
        {
            SendMessage("POW:STAT:RISE " + rate);
        }
        public void SetSlewfall_CP(string rate)
        {
            SendMessage("POW:STAT:FALL " + rate);
        }
        public void SetMode(string mode)
        {
            SendMessage("MODE " + mode);
        }
        public void SetSamplingTime(string s)
        {
            SendMessage("DIG:SAMP:TIME " + s);
            //return QueryMessage("");
        }
        public void SetSamplingTime40()
        {
            SendMessage("DIG:SAMP:TIME 40ms");
            //return QueryMessage("");
        }
        public string GetSamplingTime()
        {
            return QueryMessage("DIG: SAMP:TIME?");
        }

        //public void SetMeasuretime()
        //{
        //    SendMessage("CONF:WIND 40ms");
        //return QueryMessage("");
        //}
        public void SetMeasuretime(string s)
        {
            SendMessage("CONF:WIND " + s);
            //return QueryMessage("");
        }
        public void SetCurrent_UDW(string current)
        {
            SendMessage("UDW:STAT:L1 " + current);
        }
        public void SetVoltage_UDW(string Voltage)
        {
            SendMessage("UDW:STAT:L1 " + Voltage);
        }
        public void SetPower_UDW(string power)
        {
            SendMessage("UDW:STAT:L1 " + power);
        }
        public void SetNSEL(string s)
        {
            SendMessage("USER:WAV:NSEL " + s);
        }
        public void SetNSEL(int n)
        {
            SendMessage("USER:WAV:NSEL " + n);
        }
        public void SetWaveInterN(string s)
        {
            SendMessage("USER:WAV:DATA " + s);
        }
        public void SetWaveInterN(int n) // n은 기본1 
        {
            SendMessage("USER:WAV:DATA " + n);
        }
        public void SetWaveInterN(int n1, double d, int n2, int n3, string s)
        {
            SendMessage("USER:WAV:DATA " + n1 + "," + d + "," + n2 + "," + n3 + "," + s);
        }
        public void SetWaveDataPoint(double d) //point
        {
            SendMessage("USER:WAV:DATA:POIN " + d);
        }
        public void SetWaveClear(int n)
        {
            SendMessage("USER:WAV:CLE? " + n);
        }
        public int GetDataStatus()
        {
            string msg = QueryMessage("USER:WAV:DATA:STAT");
            msg = msg.Trim();
            return int.Parse(msg);
        }
        
        public string GetMode()
        {
            return QueryMessage($"MODE?");
        }

        public string GetMeasuretime()
        {
            return QueryMessage("CONF:WIND?");
        }

        public string GetError()
        {
            return QueryMessage("SYST:ERR?");
        }
        public double GetMeasureCurrent()
        {
            string msg = QueryMessage("MEAS:CURR?");
            msg = msg.Trim();
            return double.Parse(msg);
        }

        public double GetMeasureVoltage()
        {
            string msg = QueryMessage("MEAS:VOLT?");
            msg = msg.Trim();
            return double.Parse(msg);
        }
        public double GetMeasurePower()
        {
            string msg = QueryMessage("MEAS:POW?");
            msg = msg.Trim();
            return double.Parse(msg);
        }

        //public void SetLoadTime()
        //{
        //    SendMessage("CONF:WIND 2");
        //}
        public double SetLoadTime()
        {
            SendMessage("CONF:WIND 2");
            string msg = QueryMessage("CONF:WIND 2");
            msg = msg.Trim();
            return double.Parse(msg);
        }
       
        public void SetParametert1_CCD(string parameter)
        {
            SendMessage("CURR:DYN:T1 " + parameter);
        }
        public void SetParametert2_CCD(string parameter)
        {
            SendMessage("CURR:DYN:T2 " + parameter);
        }
     
        public void SetSlewRateRise_CCD(string rate)
        {
            SendMessage("CURR:DYN:RISE " + rate);
        }
        public void SetSlewRateFall_CCD(string rate)
        {
            SendMessage("CURR:DYN:FALL " + rate);
        }
      
        public void SetSlewfall_CCD(string rate)
        {
            SendMessage("CURR:DYN:FALL " + rate);
        }
        
        public void SetMeasuretime()
        {
            SendMessage("CONF:WIND 40ms");
            //return QueryMessage("");
        }
      
    }
}
