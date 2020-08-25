using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalesRebateBatchParameterModel : NotificationObject
    {

        private SalesRebateModel salesRebateBatchParameter;

        public SalesRebateModel SalesRebateBatchParameter
        {
            get { return salesRebateBatchParameter; }
            set
            {
                salesRebateBatchParameter = value;
                this.RaisePropertyChanged(nameof(SalesRebateBatchParameter));
            }
        }



        private ObservableCollection<SalesRebateAmountRangeModel> salesRebateAmountRangeBatchParameter;

        public ObservableCollection<SalesRebateAmountRangeModel> SalesRebateAmountRangeBatchParameter
        {
            get { return salesRebateAmountRangeBatchParameter; }
            set
            {
                salesRebateAmountRangeBatchParameter = value;
                this.RaisePropertyChanged(nameof(SalesRebateAmountRangeBatchParameter));
            }
        }

    }
}
