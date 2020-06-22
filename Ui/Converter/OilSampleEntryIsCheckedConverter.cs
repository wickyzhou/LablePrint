using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Ui.Converter
{
    public class OilSampleEntryIsCheckedConverter : BaseValueConverter<OilSampleEntryIsCheckedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.LightGray);


            if ((bool)value)
                return new SolidColorBrush(Colors.ForestGreen);

            return new SolidColorBrush(Colors.LightGray);
        }
    }
}
