using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ui.Service;

namespace Ui.ViewModel
{
    public class ShippingBillDetailLogViewModel:NotificationObject
    {
        private ShippingBillService _shippingService;
        private int _mainId;
        public ShippingBillDetailLogViewModel(int mainId)
        {
            _shippingService = new ShippingBillService();
            _mainId = mainId;
            ShippingBillDetails = new ObservableCollection<ShippingBillDetailLogModel>();
            Init();
        }


        private ObservableCollection<ShippingBillDetailLogModel> shippingBillDetails;

        public ObservableCollection<ShippingBillDetailLogModel> ShippingBillDetails
        {
            get { return shippingBillDetails; }
            set
            {
                shippingBillDetails = value;
                this.RaisePropertyChanged(nameof(ShippingBillDetails));
            }
        }

        private void Init()
        {
            ShippingBillDetails.Clear();
            _shippingService.GetAllShippingBillDetails(_mainId).ToList().ForEach(x =>
            {
                ShippingBillDetails.Add(x);

            });
        }

    
    }
}
