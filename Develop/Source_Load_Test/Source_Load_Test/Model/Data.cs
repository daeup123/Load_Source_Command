using Source_Load_Test.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Model
{
    public class Data : ObservableObject
    {
        public int No { get; set; }
        public string Time
        {
            get => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public float Voltage { get; set; }
        public float Current { get; set; }
        public float Resistance { get; set; }
        public float Power 
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
