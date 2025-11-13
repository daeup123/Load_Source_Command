using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.SCPI
{
    /// <summary>
    ///  SCPI 명령어 SIGLENT SPS5041X
    /// </summary>
    public static class ScpiSource
    {
        // 기본 명령
        public const string Identify = "*IDN?";                            // 장비 정보 조회
        public const string Reset = "*RST";                                // 장비 리셋
        public const string ClearStatus = "*CLS";                          // 상태 레지스터 초기화
        public const string SelfTest = "*TST?";                            // 자체 테스트

        //// 출력 제어
        //public const string OutputOn = "OUTPut 1";                    // 출력 ON
        //public const string OutputOff = "OUTPut 0";                  // 출력 OFF
        //public const string OutputStateQuery = "OUTPut? CH1";              // 출력 상태 조회

        public const string OutputOn = "OUTPut ON";                    // 출력 ON
        public const string OutputOff = "OUTPut 0FF";                  // 출력 OFF
        public const string OutputStateQuery = "OUTPut?";              // 출력 상태 조회

        // 전압 설정 / 측정
        public const string VoltageSet = "SOUR:VOLT {0}";    // 전압 설정
        public const string VoltageSetQuery = "SOUR:VOLT?";  // 설정 전압 조회
        public const string VoltageMeasure = "MEASure:VOLTage?";       // 실제 전압 측정

        //"SOUR:VOLT:SLEW 0.01"
        //public const string VoltageSet = ":SOURce:VOLTage:SET CH1,{0}";    // 전압 설정
        //public const string VoltageSetQuery = ":SOURce:VOLTage:SET? CH1";  // 설정 전압 조회
        //public const string VoltageMeasure = "MEASure:VOLTage? CH1";       // 실제 전압 측정
        public const string VoltageSlewRIESSet = ":SOURce:VOLTage:RISE:SLOPe CH1,{0}";   // 전류 Slew 설정
        public const string VoltageSlewRIESQuery = ":SOURce:VOLTage:RISE:SLOPe? CH1 ";   // 전류 설정 조회
        public const string VoltageSlewFALLSet = ":SOURce:VOLTage:FALL:SLOPe CH1,{0}";   // 전류 Slew 설정
        public const string VoltageSlewFALLQuery = ":SOURce:VOLTage:FALL:SLOPe? CH1 ";   // 전류 설정 조회

        //FETCH?? == Measurment?


        // 전류 설정 / 측정

        public const string CurrentSet = "SOUR:CURR {0}";    // 전류 설정
        public const string CurrentSetQuery = ":SOURce:CURRent:SET? CH1";  // 설정 전류 조회
        public const string CurrentMeasure = "MEASure:CURRent?";       // 실제 전류 측정

        public const string CurrentSlewGetMode = "SOUR:CURR:SLEW?";
        public const string CurrentSlewSetModeABLE = "SOUR:CURR:SLEWINF DISABLE";
        public const string CurrentSlewSetModeDISABLE = "SOUR:CURR:SLEWINF ENABLE";
        //public const string CurrentSet = ":SOURce:CURRent:SET CH1,{0}";    // 전류 설정
        //public const string CurrentSetQuery = ":SOURce:CURRent:SET? CH1";  // 설정 전류 조회
        //public const string CurrentMeasure = "MEASure:CURRent? CH1";       // 실제 전류 측정
        public const string CurrentSlewRIESSet = ":SOURce:CURRent:RISE:SLOPe CH1,{0}";   // 전류 Slew 설정
        public const string CurrentSlewRIESQuery = ":SOURce:CURRent:RISE:SLOPe? CH1 ";   // 전류 설정 조회
        public const string CurrentSlewFALLSet = ":SOURce:CURRent:FALL:SLOPe CH1,{0}";   // 전류 Slew 설정
        public const string CurrentSlewFALLQuery = ":SOURce:CURRent:FALL:SLOPe? CH1 ";   // 전류 설정 조회

        // 전력
        public const string PowerMeasure = "MEASure:POWer?";           // 전력 측정

        //public const string PowerMeasure = "MEASure:POWER? CH1";           // 전력 측정

        // 동작 모드 / 우선 모드
        public const string ModeQuery = "MEASure:RUN:MODE? CH1";           // 현재 동작모드 (CV/CC/CP)
        public const string PrioritySet = ":SOURce:CVCC:PRIOrity CH1,{0}"; // CC 또는 CV 설정 (예: CC → 전류 우선)
        public const string PriorityQuery = ":SOURce:CVCC:PRIOrity? CH1";  // 현재 우선 모드 조회

        // 보호 기능
        public const string OVPSet = ":SOURce:PROTection:OVP {0}";         // 과전압 설정
        public const string OVPQuery = ":SOURce:PROTection:OVP?";          // 과전압 조회
        public const string OCPSet = ":SOURce:PROTection:OCP {0}";         // 과전류 설정
        public const string OCPQuery = ":SOURce:PROTection:OCP?";          // 과전류 조회
        public const string OPPSet = ":SOURce:PROTection:OPP {0}";         // 과전력 설정
        public const string OPPQuery = ":SOURce:PROTection:OPP?";          // 과전력 조회
        public const string ProtectionClear = ":SOURce:PROTection:CLEar";  // 보호 트립 초기화

        // Sense
        public const string SenseEnable = ":SOURce:SENSE:STATE ON";         // Remote Sense 활성화
        public const string SenseDisable = ":SOURce:SENSE:STATE OFF";       // Remote Sense 비활성화
        public const string SenseQuery = ":SOURce:SENSE:STATE?";            // Sense 상태 조회

        // Bleeder
        public const string BleederEnable = ":SOURce:BLEEder:STATE ON";     // Bleeder ON
        public const string BleederDisable = ":SOURce:BLEEder:STATE OFF";   // Bleeder OFF
        public const string BleederForce = ":SOURce:BLEEder:FORCe";         // 강제 방전
        public const string BleederQuery = ":SOURce:BLEEder:STATE?";        // Bleeder 상태 조회
    }
}
