﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
   public class ShippingBillEntryModifyViewIsReadOnlyConverter : BaseValueConverter<ShippingBillEntryModifyViewIsReadOnlyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if ((bool)value)
                return true;
            return false;
        }
    }
}
