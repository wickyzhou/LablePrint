using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Ui.ViewModel;

namespace Ui.Converters
{
    //public class LabelPrintCommonAdjustmentConverter : IMultiValueConverter
    //{
    //    //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    //{
    //    //    string orgID = values[0]?.ToString();
    //    //    string label = values[1]?.ToString();
    //    //    string productionName = values[2]?.ToString();
    //    //    string expirationMonth = values[3]?.ToString();
    //    //    int.TryParse(values[4]?.ToString(), out int expirationDay);
    //    //    CbIdentTypeModel cb = new CbIdentTypeModel();
    //    //    if (values[5] != null)
    //    //    {
    //    //        cb= values[5]  as CbIdentTypeModel;
    //    //    }

    //    //    var r= LabelPrintCommonAdjustmentViewModel.CommonAdjustments.Where(m=>m.OrgID==orgID);
    //    //    if (r==null)
    //    //    {
    //    //        return null;
    //    //    }
           

    //    //    return new LabelPrintCommonAdjustmentModel
    //    //    {
    //    //        OrgID = orgID,
    //    //        Label = label,
    //    //        ProductionName =productionName,
    //    //        ExpirationMonth = expirationMonth,
    //    //        ExpirationDay = expirationDay,
    //    //        IdentityType= cb.IdentityType,
    //    //        IdentityTypeDesc= cb.IdentityTypeDesc
    //    //    };
    //    //}

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
