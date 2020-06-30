using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
    public class ShippingBillAddViewAmountIsEnableConverter : BaseValueConverter<ShippingBillAddViewAmountIsEnableConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if ((int)value > 0)
                return true;
            return false;
        }
    }
}
