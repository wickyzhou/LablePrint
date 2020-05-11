using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Ui.MVVM.Converter
{
    public class BoolConverter : IValueConverter
    {
        //在绑定时给字段的数据类做转换用
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "是" : "否";
        }

        //如果是双向绑定，同时需要添加类型转换逻辑代码
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;
            if (s.Equals("是", StringComparison.CurrentCultureIgnoreCase))
                return true;
            if (s.Equals("否", StringComparison.CurrentCultureIgnoreCase))
                return false;
            throw new Exception(string.Format("Cannot convert, unknown value {0}", value));
        }
    }
}
