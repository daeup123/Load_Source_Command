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
    public static class DeviceManager
    {
        public static USB_D63004 DMLoad = new USB_D63004();

        public static List<VisaUsbDevice> visaUsbDevices = new List<VisaUsbDevice>()
        {
            DMLoad
        };

        public static void DisposeDevices()
        {
            DMLoad.Dispose();
        }

        public static bool ConnectDevices()
        {
            try
            {
                using (ResourceManager rm = new ResourceManager())
                {
                    string resource = "USB0::0x0A69::0x0880::630041500831::INSTR";
                    UsbSession usbSession = new UsbSession(resource);

                    DMLoad.SetSession(usbSession);

                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while connecting the device\n" + e.Message);
                return false;
            }
        }
    }
}