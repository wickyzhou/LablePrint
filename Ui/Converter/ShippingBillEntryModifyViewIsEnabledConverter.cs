using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
    public class ShippingBillEntryModifyViewIsEnabledConverter : BaseValueConverter<ShippingBillEntryModifyViewIsEnabledConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;
            if ((bool)value)
                return false;
            return true;
        }
    }
}
