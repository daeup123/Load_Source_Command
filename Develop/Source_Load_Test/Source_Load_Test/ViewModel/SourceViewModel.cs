using Source_Load_Test.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source_Load_Test.ViewModel
{
    public class SourceViewModel : ObservableObject
    {
        public SourceViewModel() 
        {
            Console.WriteLine("SourceViewModel init");
        }
    }
}
