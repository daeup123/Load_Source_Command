using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Source_Load_Test.Resources.Converter
{
    public class BoolToColorConverter : IValueConverter
    {       
        // bool → Brush 변환
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isOn = value is bool b && b;

            // 원하는 색상 지정 (Hex 코드 사용 가능)
            var activeColor = (Color)ColorConverter.ConvertFromString("#3498DB");
            var inactiveColor = (Color)ColorConverter.ConvertFromString("#D3D3D3");
            return new SolidColorBrush(isOn ? activeColor : inactiveColor);
        }

        // 단방향 바인딩이라 보통은 안 씀
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
