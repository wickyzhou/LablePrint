using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Ui.Converter
{
    public class SelectedBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {   

            if (value == null)
                return DependencyProperty.UnsetValue;
            int status = (int)value;
            switch (status)
            {
                case 0:
                    return new SolidColorBrush(Colors.Transparent);

                case 1:

                    return new SolidColorBrush((Color)Application.Current.Resources["GenericOrangeColor"]);
                case 2:

                    return new SolidColorBrush((Color)Application.Current.Resources["GenericBlueColor"]);
                  

                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
