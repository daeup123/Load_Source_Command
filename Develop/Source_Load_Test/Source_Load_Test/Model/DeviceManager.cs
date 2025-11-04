using NationalInstruments.Visa;
using RelayTest.Devices;
using Source_Load_Test.Devices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Source_Load_Test.Model
{
    public static class DeviceManager // 장치관리자
    {
        public static SIGLENT_SPS5041X Source = new SIGLENT_SPS5041X();
        public static Array3720A Load = new Array3720A();

        // 배열에두고 관리
        public static List<VisaUsbDevice> visaUsbDevices = new List<VisaUsbDevice>()
        {
            Source
        };
        public static List<VisaSerialDevice> visaSerialDevices = new List<VisaSerialDevice>()
        {
            Load
        };
        
        public static void DisposeDevices() // 모든장비 연결해제
        {
            foreach (var item in visaUsbDevices)
            {
                item.Dispose();               
            }

            foreach (var item in visaSerialDevices)
            {
                item.Dispose();
            }
        } 
        public static bool ConnectSerialDevices() // RS232 장비 연결
        {
            try
            {
                using (ResourceManager rm = new ResourceManager())
                {
                    string expression = "ASRL?*::INSTR";
                    IEnumerable<string> findList = rm.Find(expression); // RE232 검색

                    foreach (var item in findList)
                    {
                        SerialSession session = new SerialSession(item.ToString());

                        session.BaudRate = 9600;
                        session.DataBits = 8;
                        session.StopBits = Ivi.Visa.SerialStopBitsMode.One;
                        session.Parity = Ivi.Visa.SerialParity.None;
                        session.TimeoutMilliseconds = 3000;

                        session.FormattedIO.WriteLine("*IDN?"); // 장비검색
                        string response = session.FormattedIO.ReadString(); // 응답

                        if (response.Contains("3702A")) // 연결
                        {
                            Load.SetSession(session);
                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while connecting the Serialdevice\n" + e.Message);
                return false;
            }
        }
        public static bool ConnectUsbDevices() // Usb 장비 연결
        {
            Console.WriteLine("ConnectUsbDevices");

            try
            {
                using (ResourceManager rm = new ResourceManager())
                {
                    string expression = "USB?*::INSTR";
                    // "USB0::0xF4EC::0x1450::SPS41ABQ800122::INSTR"
                    string resource = "USB0::0xF4EC::0x1450::SPS41ABQ800122::INSTR"; // 진짜 시리얼번호
                    IEnumerable<string> findList = rm.Find(expression); // usb검색
                    foreach (var item in findList)
                    {
                        if ((string)item == resource)
                        {
                            UsbSession session = new UsbSession((string)item);
                            Source.SetSession(session);
                            Console.WriteLine("쏘스연결성공");
                            //Source.S
                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while connecting the Usbdevice\n" + e.Message);
                return false;
            }
        } 
    }
}