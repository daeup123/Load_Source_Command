using Microsoft.SqlServer.Server;
using RelayTest.Devices;
using Source_Load_Test.Enums;
using Source_Load_Test.Model;
using Source_Load_Test.SCPI;
using Source_Load_Test.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        protected override string QueryMessage(string message) // 
        {
            SendMessage(message);
            string msg = ReceiveMessage();
            Session.FormattedIO.GetType(); // 버퍼클리어

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

        public Data GetValue() // 측정값 받기
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

        public async Task<List<LIST>> GetListMemo()
        {
            List<LIST> lists = new List<LIST>();
            string msg = "";
            string msgReceive = "";

            // 0 ~ 6번 각번호의 리스트의 길이를 조회해서 0보다크면
            // 해당리스트의 내용을 불러와서 리스트컬렉션에 추가
            // 리스트를 선택하고 불러오기를 했을떄 가져오면 좋지만 .. 나중에


            //SendMessage(msg);

            //string memos = Session.FormattedIO.ReadString();
            //memos = Session.FormattedIO.ReadString();
            //Debug.WriteLine("리스트 메모 조회 결과 : " + memos);
            //memos = Session.FormattedIO.ReadString();
            //Debug.WriteLine("리스트 메모 조회 결과 : " + memos);
            //memos = Session.FormattedIO.ReadString();
            //Debug.WriteLine("리스트 메모 조회 결과 : " + memos);
            //memos = Session.FormattedIO.ReadString();
            //Debug.WriteLine("리스트 메모 조회 결과 : " + memos);
            //memos = Session.FormattedIO.ReadString();
            //Debug.WriteLine("리스트 메모 조회 결과 : " + memos);
            //memos = Session.FormattedIO.ReadString();
            //Debug.WriteLine("리스트 메모 조회 결과 : " + memos);

            ////                                  // .. 나중에 리턴값 : KB\s\s\s\s\s\s\s\s\r\n
            ////                                  // 두번째 띄워쓰기에도 스텝이있음..

            for (int i = 0; i < 7; i++)
            {
                msg = string.Format(ScpiLoad.ListNum, i);
                SendMessage(msg); // 리스트 번호로 이동
                msg = (ScpiLoad.ListLength);
                int length = int.Parse(QueryMessage(msg));
                if(length > 0) // 길이가 0보다크면 해당리스트의 내용을 불러와서 리스트컬렉션에 추가
                {
                    List<ListStep> steps = new List<ListStep>();
                    msg = (ScpiLoad.ListMemorize);
                    string memos = QueryMessage(msg); // 메모된 내용도 넣어주고싶은데

                    for (int j = 0; j < length; j++)
                    {
                        msg = string.Format(ScpiLoad.ListEdit, (j + 1)); // 리스트 스탭내용을 조회
                        msgReceive = QueryMessage(msg);
                        string[] stepstr = msgReceive.Split(',');

                        ListStep step = new ListStep()
                        {
                            StepNumber = j + 1,
                            Mode = stepstr[0],
                            Value = float.Parse(stepstr[1]),
                            Time = float.Parse(stepstr[2])
                        };

                        steps.Add(step);
                    }

                    LIST list = new LIST()
                    {
                        ListNumber = i,
                        Steps = steps,
                        ListName = memos
                    };
                    lists.Add(list);
                }
                else
                {
                    // 길이가 0 이면 패스
                    continue;
                }
            }

            return lists;
        }

        public void ListOnOff(string commandParameter, int num)
        {
            string msg = "";
            if (commandParameter == "ON")
            {
                msg = string.Format(ScpiLoad.ListNum, num);
                SendMessage(msg);
                Debug.WriteLine("Load List ON");
                msg = (ScpiLoad.ListEnable);
            }
            else if (commandParameter == "OFF")
            {
                Debug.WriteLine("Load List OFF");
                msg = (ScpiLoad.ListDisable);
            }
            else
            {
                Debug.WriteLine("Load List OFF");
                msg = (ScpiLoad.ListDisable);
                Debug.WriteLine("Load List ON/OFF 실패");
            }
            SendMessage(msg);
        }
    
        public async Task ListDelete(int ListNum, int listNumber)
        {
            string msg = string.Format(ScpiLoad.ListNum, ListNum);
            SendMessage(msg);
            msg  = string.Format(ScpiLoad.ListDelete, listNumber);
            SendMessage(msg);
        }

        public async Task ListAdd(int ListNum, List<string> listStep)
        {
            string msg = string.Format(ScpiLoad.ListNum, ListNum);
            Debug.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&List Add Msg : " + msg);

            SendMessage(msg);
            string Step = string.Join(",", listStep);
            
            Debug.WriteLine("List Add Step : " + Step);
            msg = string.Format(ScpiLoad.ListAdd, Step);
            Debug.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&List Add Msg : " + msg);
            SendMessage(msg);
        }

        public async Task ListInsert()
        {
            // 나중에
            string msg = string.Format(ScpiLoad.ListInsert, 1, "CV", 5, 2); // 스텝번호, 모드, 값, 시간
            SendMessage(msg);
        }
        public async Task ListSave()
        {
            string msg = (ScpiLoad.ListSave);
            SendMessage(msg);
        }
    }
}
