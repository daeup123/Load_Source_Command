using NationalInstruments.Visa;
using RelayTest.Devices;
using System;
using System.Linq;
using System.Windows;
using RelayTest.ViewModel.Control;
using Ivi.Visa;

namespace RelayTest.Model
{
    internal class RelayMannger : ObservableObject
    {
        public RelayMannger()
        {
            _realy32 = new RS232_RELAY_32CH();
            Text = "안녕하세요!";
        }

        RS232_RELAY_32CH _realy32; // 릴레이 디바이스
        public string Text = "";

        private SerialSession ConnectRelay(ResourceManager rm) // 리소스매니져로 릴레이 연결
        {
            try
            {
                //string _resource = "ASRL" + "::INSTR";
                // 가장 일반적으로 사용되는 시리얼 포트 검색 표현식
                string expression = "ASRL?*::INSTR";
                var tmp = rm.Find(expression);
                int tmplen = tmp.Count();

                SerialSession _session = new SerialSession(tmp.First());

                _session.BaudRate = 9600;
                _session.DataBits = 8;
                _session.StopBits = Ivi.Visa.SerialStopBitsMode.One;
                _session.Parity = Ivi.Visa.SerialParity.None;
                _session.TimeoutMilliseconds = 3000;

                return _session;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                //return null;
            }          
        }

        public void ClickedSingleRelay(int num, bool isOn) // 단일릴레이 클릭
        {
            using(ResourceManager _rm = new ResourceManager())
            {
                SerialSession _session = null;
                _session = ConnectRelay(_rm);
                if (_session != null)
                {
                    _realy32.ClickedSingle(num, isOn); // 릴레이 연결
                    //_realy32.RelayOnAll();            // 신호보내보기
                }
                else
                {
                    MessageBox.Show("ClickedSingleRelay XXX");
                }
            }
        }

        public string RelayOn() // 릴레이 모두켜기
        {
            using (ResourceManager _rm = new ResourceManager())
            {
                SerialSession _session = null;
                string _returnStr = "";
                _session = ConnectRelay(_rm);
                if (_session != null)
                {
                    _realy32.SetSession(_session); // 릴레이 연결
                    _realy32.RelayOnAll();            // 신호보내보기
                    _returnStr = "All On OK";
                    return _returnStr;
                }
                else
                {
                    _returnStr = "All On Fail";
                    return _returnStr;
                }
            }
        }
    
        public byte[] RelayOff() // 릴레이 모두끄기
        {
            using (ResourceManager _rm = new ResourceManager())
            {
                SerialSession _session = null;
                byte[] _returnByte = new byte[8];
                _session = ConnectRelay(_rm);
                if (_session != null)
                {
                    _realy32.SetSession(_session); // 릴레이 연결
                    _realy32.RelayOffAll();            // 신호보내보기
                    //_session.ReadStatusByte();
                    _session.RawIO.Read(8); // 8바이트를 읽음

                    return _returnByte;
                }
                else
                {
                    
                    return _returnByte;
                }
            }
        }
        
        private byte[] CheckReceive()
        {
            byte[] ret = new byte[8];

            return ret;
        }

        //private 

        //public string Quary(int fun, int num)
        //{
        //    using (ResourceManager _rm = new ResourceManager())
        //    {
        //        SerialSession _session = null;
        //        _session = ConnectRelay(_rm);
        //        if (_session != null)
        //        {
        //            _realy32.SetSession(_session); // 릴레이 연결
        //            _realy32.SetRelayControl(fun, num);
        //            return "ok";
        //        }
        //        else
        //        {
        //            return "Fail";
        //        }
        //    }
        //}

        //private string RecevieByte()
        //{
        //    string _returnStr = "";


        //    return _returnStr;
        //}
    }
}
