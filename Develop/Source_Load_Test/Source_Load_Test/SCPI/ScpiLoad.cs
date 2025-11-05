using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.SCPI
{
    /// <summary>
    /// SCPI 명령어 ARRAY 3720A 
    /// </summary>
    /// 전력은 직접계산
    public static class ScpiLoad
    {
        // ======================================================
        // 기본 명령 (Common Commands)
        // ======================================================
        public const string Identify = "*IDN?";                   // 장비 정보 조회
        public const string Reset = "*RST";                       // 장비 리셋
        public const string ClearStatus = "*CLS";                 // 상태 레지스터 초기화
        public const string SelfTest = "*TST?";                   // 자체 테스트

        // ======================================================
        // 로드 제어 (Load Control)
        // ======================================================
        public const string LoadOn = "INPut ON";                  // 부하 ON
        public const string LoadOff = "INPut OFF";                // 부하 OFF
        public const string LoadStateQuery = "INPut?";            // 부하 상태 조회 (1=ON, 0=OFF)

        // ======================================================
        // 동작 모드 (Operating Mode)
        // ======================================================
        public const string ModeCC = ":FUNCtion:MODE CC";         // 정전류(CC)
        public const string ModeCV = ":FUNCtion:MODE CV";         // 정전압(CV)
        public const string ModeCR = ":FUNCtion:MODE CR";         // 정저항(CR)
        public const string ModeCP = ":FUNCtion:MODE CP";         // 정전력(CP)
        public const string ModeQuery = ":FUNCtion:MODE?";        // 현재 동작 모드 조회

        // ======================================================
        // 전류 설정 (Current)
        // ======================================================
        public const string CurrentSet = ":CURRent {0}";          // 전류 설정 (예: 1.0)
        public const string CurrentQuery = ":CURRent?";           // 설정된 전류값 조회
        public const string CurrentMeasure = ":MEASure:CURRent?"; // 실제 전류 측정값 조회
        public const string SlewRateSet = ":SLEW:RATE {0}";       // Slew Rate 설정 (A/μs)
        public const string SlewRateQuery = ":SLEW:RATE?";        // Slew Rate 조회

        // ======================================================
        // 전압 관련 (Voltage / Protection)
        // ======================================================
        public const string VoltageMeasure = ":MEASure:VOLTage?"; // 실제 전압 측정값 조회
        public const string OVPSet = ":PROTection:VOLTage {0}";   // 과전압 보호 설정
        public const string OVPQuery = ":PROTection:VOLTage?";    // 과전압 한계 조회

        // ======================================================
        // 저항 / 전력 (Resistance / Power)
        // ======================================================
        public const string ResistanceSet = ":RESistance {0}";    // 저항 설정
        public const string ResistanceQuery = ":RESistance?";     // 저항 설정 조회
                                                                  // 전력(Power) 직접 측정은 지원 안 함 → V×I 계산 필요

        // ======================================================
        // 트랜지언트 (Transient Mode)
        // ======================================================
        public const string TransientMode = ":TRANsient:MODE {0}";       // CONT / PULSE / TOGGLE
        public const string TransientHigh = ":TRANsient:CURRent:HIGH {0}"; // High Level (A)
        public const string TransientLow = ":TRANsient:CURRent:LOW {0}";   // Low Level (A)
        public const string TransientFrequency = ":TRANsient:FREQuency {0}"; // 주파수 (Hz)
        public const string TransientDuty = ":TRANsient:DUTY {0}";        // 듀티비 (%)
        public const string TransientStateOn = ":TRANsient ON";           // 트랜지언트 ON
        public const string TransientStateOff = ":TRANsient OFF";         // 트랜지언트 OFF
        public const string TransientQuery = ":TRANsient?";               // 트랜지언트 설정 조회

        // ======================================================
        // 리스트 모드 (List Mode)
        // ======================================================
        public const string ListEnable = ":LIST:STATe ON";        // 리스트 모드 ON
        public const string ListDisable = ":LIST:STATe OFF";      // 리스트 모드 OFF
        public const string ListStepSet = ":LIST:CURRent {0}";    // 리스트 전류 단계 설정
        public const string ListRun = ":LIST:RUN";                // 리스트 실행
        public const string ListAbort = ":LIST:ABORt";            // 리스트 중단
        public const string ListQuery = ":LIST:CURRent?";         // 리스트 전류 단계 조회

        // ======================================================
        // 단락 (Short)
        // ======================================================
        public const string ShortOn = ":SHORt ON";                // 단락 ON
        public const string ShortOff = ":SHORt OFF";              // 단락 OFF
        public const string ShortStateQuery = ":SHORt?";          // 단락 상태 조회

        // ======================================================
        // 보호 및 오류 (Protection / Error)
        // ======================================================
        public const string OCPSet = ":PROTection:CURRent {0}";   // 과전류 보호 설정
        public const string OCPQuery = ":PROTection:CURRent?";    // 과전류 보호 조회
        public const string OPPSet = ":PROTection:POWer {0}";     // 과전력 보호 설정
        public const string OPPQuery = ":PROTection:POWer?";      // 과전력 보호 조회
        public const string ProtectionClear = ":PROTection:CLEar";// 보호 상태 초기화
        public const string ErrorQuery = ":SYSTem:ERRor?";        // 오류 코드 조회
    }
}
