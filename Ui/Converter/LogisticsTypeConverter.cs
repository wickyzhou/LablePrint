using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Ui.Service;

namespace Ui.Converter
{
    public class LogisticsTypeConverter : IValueConverter
    {
        private List<EnumModel> lists;

        public LogisticsTypeConverter()
        {
            lists= new CommonService().GetEnumLists().Where(m => m.GroupSeq == 3).ToList();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var xx = lists.FirstOrDefault(x => x.ItemSeq == (int)value);
            if (xx == null)
                return null;
            return lists.FirstOrDefault(x => x.ItemSeq == (int)value).ItemValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
