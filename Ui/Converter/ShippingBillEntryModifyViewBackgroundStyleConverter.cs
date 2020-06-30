using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Ui.Converter
{
    public class ShippingBillEntryModifyViewBackgroundStyleConverter : BaseValueConverter<ShippingBillEntryModifyViewBackgroundStyleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.LightGray);
            if((int)value == 1 || (int)value == 3)
                return new SolidColorBrush(Colors.LightGray);
            return new SolidColorBrush(Colors.Snow);
        }
    }
}
