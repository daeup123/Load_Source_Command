using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Model
{
    public class LoadData : ViewModel.Control.ObservableObject
    {
        public int No { get; set; }
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double Power { get; set; }
        //public double Time { get; set; }

        public double Timer { get; set; }
        public string Time { get; set; }

        public double Time2 { get; set; }

    }
}
