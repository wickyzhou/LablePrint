using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace Ui.Converter
{
    public class ExportDynamicGridVisibilityConverter : BaseValueConverter<ExportDynamicGridVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Hidden; ;

            if ((bool)value)
                return Visibility.Visible;

            return Visibility.Hidden;
        }
    }
}
