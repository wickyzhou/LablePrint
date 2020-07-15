using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Ui.Converter
{
    public class ValidationResultValueConverter : BaseValueConverter<ValidationResultValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
           return  (bool)value ? "成功" : "失败";
        }
    }
}
