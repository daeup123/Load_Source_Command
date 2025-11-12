using Source_Load_Test.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.Model
{
    public class Avg : ObservableObject
    {
        private float _voltage;
        private float _current;
        private float _power;

        public float Voltage
        {
            get => _voltage;
            set => SetProperty(ref _voltage, value);
        }
        public float Current
        {
            get => _current;
            set => SetProperty(ref _current, value);
        }
        public float Power
        {
            get => _power;
            set => SetProperty(ref _power, value);
        }
    }
}
