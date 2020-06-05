using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
    public class ConsignmentBillEntryIsSystemConverter : BaseValueConverter<ConsignmentBillEntryIsSystemConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            if ((bool)value)
                return "系统";
            return "手动";
        }
    }
}
