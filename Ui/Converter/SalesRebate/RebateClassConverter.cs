using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Ui.Service;

namespace Ui.Converter.SalesRebate
{
    public class RebateClassConverter:BaseValueConverter<RebateClassConverter>
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new CommonService().GetEnumLists(6).Where(m=>m.ID==(int)value).FirstOrDefault().ItemValue;
        }
    }
}
