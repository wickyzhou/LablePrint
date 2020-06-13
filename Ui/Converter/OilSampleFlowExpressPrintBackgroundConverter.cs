using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Ui.Converter
{
    public class OilSampleFlowExpressPrintBackgroundConverter : BaseValueConverter<OilSampleFlowExpressPrintBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#43a9c7"));

            return (int)value > 0 ? new SolidColorBrush(Colors.LightGray): new SolidColorBrush((Color)ColorConverter.ConvertFromString("#43a9c7"));
        }
    }
}
