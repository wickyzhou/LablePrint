using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace Ui.Converter
{
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {

        private static T Converter = null;

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter ?? new T();
        }
    }
}
