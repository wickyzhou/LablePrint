using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Ui.Converter
{
    public class ShippingBillEntryModifyViewIsEnabledStyleConverter : BaseValueConverter<ShippingBillEntryModifyViewIsEnabledStyleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush((Color)Application.Current.Resources["GenericWhiteColor"]);
            if ((bool)value)
                return new SolidColorBrush((Color)Application.Current.Resources["GenericReadOnlyColor"]);

            return new SolidColorBrush((Color)Application.Current.Resources["GenericWhiteColor"]);
        }
    }
}
