using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
    public class RadioButtonCheckConverter : BaseValueConverter<RadioButtonCheckConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            if ((int)value == System.Convert.ToInt32(parameter))
                return true;
            return false;

            //string checkvalue = value.ToString();
            //string targetvalue = parameter.ToString();
            //bool r = checkvalue.Equals(targetvalue);
            //return r;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            if ((bool)value)
            {
                return System.Convert.ToInt32(parameter);
            }
            return null;

        }
    }
}
