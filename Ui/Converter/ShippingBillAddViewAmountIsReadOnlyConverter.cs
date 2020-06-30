using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
    public class ShippingBillAddViewAmountIsReadOnlyConverter : BaseValueConverter<ShippingBillAddViewAmountIsReadOnlyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if ((int)value == 3 || (int)value == 1)
                return true;
            return false;
        }
    }
}
