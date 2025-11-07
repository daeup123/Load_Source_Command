using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Source_Load_Test.Resources.Converter
{
    public class BoolToEllipseColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isConnected = value is bool b && b;
            // 연결 상태에 따른 색상 지정
            var connectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2ECC71"); // 초록색
            var disconnectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#E74C3C"); // 빨간색
            return new System.Windows.Media.SolidColorBrush(isConnected ? connectedColor : disconnectedColor);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
