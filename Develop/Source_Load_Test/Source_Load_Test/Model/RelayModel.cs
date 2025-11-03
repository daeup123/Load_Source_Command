using RelayTest.ViewModel.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Source_Load_Test.Model
{
    public class RelayModel : ObservableObject
    {
        public string Name { get; set; }     // 예: "Relay 1"

        private bool _isOn; // 예: true면 켜짐, false면 꺼짐
        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (_isOn != value)
                {
                    _isOn = value;
                    OnPropertyChanged(nameof(IsOn));
                }
            }
        }       
        public int Num { get; set; } // 번호. 편의상..

    }
}
