using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
    class SalesRebateRecentOrgCaseIsEnabledConverter : BaseValueConverter<SalesRebateRecentOrgCaseIsEnabledConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int p = System.Convert.ToInt32(parameter);
            int v = System.Convert.ToInt32(value);
            if ((p & v) == p)
                return true;
            return false;
        }
    }
}
