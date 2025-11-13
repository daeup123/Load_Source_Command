using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Model
{
    public class DeviceCheckConnection
    {
        public DeviceCheckConnection() 
        { 

        }
        public async Task<bool> CheckConnectionSource()
        {
            await Task.Delay(1000); // Simulate some async work
            return DeviceManager.Source.IsConnected;
        }

        public async Task<bool> CheckConnectionLoad()
        {
            await Task.Delay(1000); // Simulate some async work
            return DeviceManager.Load.IsConnected;
        }
    }
}
