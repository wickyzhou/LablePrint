using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Ui.Converter.Generic
{
    public class LighterForegroundConverter : BaseValueConverter<LighterForegroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            if ((double)value == 0)
                return new SolidColorBrush((Color)Application.Current.Resources["FontColor1"]);
            else
                return new SolidColorBrush((Color)Application.Current.Resources["FontColor9"]);
        }
    }
}
