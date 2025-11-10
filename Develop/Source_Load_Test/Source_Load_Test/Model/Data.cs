using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Model
{
    public class Data
    {
        public int No { get; set; }
        public string Time
        {
            get => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Resistance { get; set; }
        public double Power 
        { 
            get => Voltage * Current;
        }
        public string Mode { get; set; }
        //public void Tmp()
        //{
        //    //string tmp = Currenttime.ToString("HH:mm:ss.fff");
        //    string tmp = (string)DateTime,Now;
        //}
    }
}
