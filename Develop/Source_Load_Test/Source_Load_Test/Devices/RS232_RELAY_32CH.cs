using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace RelayTest.Devices
{
    public class RS232_RELAY_32CH : VisaSerialDevice
    {
        //private static byte[] s_data = new byte[8]; // 어떤방법이 좋은가?

        public override bool IsConnected => GetConnected(); //Session.Connected;

        public void RelayOnAll()
        {
            //byte[] msg = new byte[] { 0x55, 0x01, 0x32, 0x00, 0x00, 0x00, 0x01, 0x89 };
            byte[] msg = new byte[] { 0x55, 0x01, 0x33, 0xFF, 
                                      0xFF, 0xFF, 0xFF, 0x85 };
            
            SendByte(msg);
        }

        public void RelayOffAll()
        {
            //byte[] msg = new byte[] { 0x55, 0x01, 0x32, 0x00, 0x00, 0x00, 0x01, 0x89 };
            byte[] msg = new byte[] { 0x55, 0x01 ,0x33 ,0x00 ,
                                      0x00 ,0x00 ,0x00 ,0x89 };
            SendByte(msg);
        }
      
        public void CmdFunc(byte head, byte func) 
        {
            byte sum = 0x00;
            byte[] msg = new byte[] {head, 0x01,  func, 0x00, 0x00, 0x00, 0x00, 0x00};
                
            sum = Calc_checksum(msg);
            msg[7] = sum;
        }

        public bool GetIDN()
        {
            byte[] readData = QueryByte(new byte[] { 0x55, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x66 });

            if (readData[0] == 0x22 && readData[1] == 0x01 && readData[2] == 0x10)
            {
                MessageBox.Show(readData.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        //**************************************************************
        // nRelayCh : 1 ~ 16 (1CH ~ 32CH)
        // bState   : _ON or _OFF
        //**************************************************************

        public void SetRelay(int nRelayCh, int state)
        {
            byte ch = (byte)nRelayCh;
            byte checksum = 0x00;
            byte onoff = 0x00;

            if (state == 0) onoff = 0x31;   //Open
            else if (state == 1) onoff = 0x32;   //Close

            byte[] cmd = new byte[] { 0x55, 0x01, onoff, 0x00, 0x00, 0x00, ch, 0x00 };
            checksum = Calc_checksum(cmd);

            cmd[7] = checksum;

            SendByte(cmd);

            Thread.Sleep(10);
        }

        public void CheckStatus() // 테스트
        {
            byte[] cmd = new byte[] {0x55, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00};
            byte checksum = Calc_checksum(cmd);

            cmd[7] = checksum;
            MessageBox.Show(QueryByte(cmd).ToString());
            //QueryByte(cmd);
        }

        public void SetRelayControl(int fun, int nRelayCh) // 1 << 1, 9, 17
        {
            byte checksum = 0x00;
            byte ch = (byte)nRelayCh;

            byte[] cmd = new byte[] { 0x55, 0x01, (byte)fun, 0x00, ch, ch, ch, 0x00 };

            checksum = Calc_checksum(cmd);

            cmd[7] = checksum;

            SendByte(cmd);

            Thread.Sleep(20);
        }

        public void ClickedSingle(int num, bool isOn)
        {
            byte func = 0x11; // 끊기
            byte relayNum = (byte)num;
            byte checksum = 0x00;

            if(isOn == true)
            {
                func = 0x12; // 연결
            }
            byte[] cmd = new byte[] { 0x55, 0x01, (byte)func, 0x00, 0x00, 0x00, relayNum, 0x00 };

            checksum = Calc_checksum(cmd);
            cmd[7] = checksum;

            SendByte(cmd);
        } 

        public void SetAllRelay(byte[] state)  
        {
            // 55 01 33 FF FF FF FF 85
            // 55 01 33 00 00 00 00 89

            byte[] cmd = new byte[] { 0x55, 0x01, 0x33, state[0], state[1], state[2], state[3], 0x00 };

            byte checksum = Calc_checksum(cmd);

            cmd[7] = checksum;

            SendByte(cmd);

            Thread.Sleep(10);
        }

        public byte Calc_checksum(byte[] cmd) // 합 계산기
        {
            byte chksum = 0x00;

            foreach (byte b in cmd)
            {
                chksum += b;
            }

            return chksum;
        }
    }
}
