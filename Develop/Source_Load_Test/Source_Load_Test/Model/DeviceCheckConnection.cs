using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Model
{
    public class DeviceCheckConnection
    {
        public DeviceCheckConnection() 
        { 
            Console.WriteLine("DeviceCheckConnection 생성자 호출");
        }
        public async Task<bool> CheckConnectionSource()
        {
            await Task.Delay(1000); // Simulate some async work
            Console.WriteLine("CheckConnectionSource 호출됨");
            return DeviceManager.Source.IsConnected;
        }

        public async Task<bool> CheckConnectionLoad()
        {
            await Task.Delay(1000); // Simulate some async work
            Console.WriteLine("CheckConnectionLoad 호출됨");
            return DeviceManager.Load.IsConnected;
        }
    }
}
