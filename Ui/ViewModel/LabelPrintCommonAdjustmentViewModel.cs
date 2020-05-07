using Bll.Services;
using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Ui.ViewModel
{
    public static class LabelPrintCommonAdjustmentViewModel
    {
        public static ObservableCollection<LabelPrintCommonAdjustmentModel> CommonAdjustments { get; set; }

        public static IEnumerable<CbIdentTypeModel> IdentTypes { get; set; }

         static LabelPrintCommonAdjustmentViewModel()
        {
            CommonAdjustments = new ObservableCollection<LabelPrintCommonAdjustmentModel>(new LabelPrintService().GetAllLabelPrintCommonAdjustment());
            IdentTypes = new CommonDAL().GetEnumModels().Where(m => m.GroupSeq == 1).Select(n=>new CbIdentTypeModel { IdentityType=n.ItemSeq,IdentityTypeDesc=n.ItemValue});
        }
        
    }
}
