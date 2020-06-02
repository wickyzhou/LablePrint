using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ui.Converter
{
    public class SelectedStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            int r = (int)value;
            if (r == 0)
                return string.Empty;
            else if (r == 2)
                return "全部";
            else
                return "部分";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
