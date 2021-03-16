using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter.Generic
{
    public class NegationConverter : BaseValueConverter<NegationConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;
            return (bool)value == true ? false : true;
        }
    }
}
