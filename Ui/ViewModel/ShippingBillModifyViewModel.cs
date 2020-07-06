using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel
{
    public class ShippingBillModifyViewModel : NotificationObject, IValidationExceptionHandler
    {
        private Action<int, ShippingBillModel> _callBack;

        public ShippingBillModifyViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
            LogisticsTypeSelectionChangedCommand = new DelegateCommand(ChangeLogisticsTypeSelection);
            Init();
        }

        private void ChangeLogisticsTypeSelection(object obj)
        {
            if ((int)obj == 1)
            {
                ShippingBill.YunShuFei = (float)(ShippingBill.SystemApportionedQuantity * 0.81);
            }
        }

        private ShippingBillModel shippingBill;

        public ShippingBillModel ShippingBill
        {
            get { return shippingBill; }
            set
            {
                shippingBill = value;
                this.RaisePropertyChanged(nameof(ShippingBill));
            }
        }

        private List<EnumModel> logisticsTypeLists;

        public List<EnumModel> LogisticsTypeLists
        {
            get { return logisticsTypeLists; }
            set
            {
                logisticsTypeLists = value;
                this.RaisePropertyChanged(nameof(LogisticsTypeLists));
            }
        }

        private List<EnumModel> goodsTypeLists;

        public List<EnumModel> GoodsTypeLists
        {
            get { return goodsTypeLists; }
            set
            {
                goodsTypeLists = value;
                this.RaisePropertyChanged(nameof(GoodsTypeLists));
            }
        }

        public void WithParam(ShippingBillModel entry, Action<int, ShippingBillModel> callBack)
        {
            ShippingBill = entry;
            _callBack = callBack;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand LogisticsTypeSelectionChangedCommand { get; set; }

        //public bool IsValid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public  void Save(object obj)
        {
            if (!this.IsValid||ShippingBill.LogisticsType==0)
            {
                MessageBox.Show("输入的格式有误或者有数据未填!");
                return;
            }
            _callBack?.Invoke(1, ShippingBill);
        }

        private void Exit(object obj)
        {
            _callBack?.Invoke(0, null);
        }

        private void Init()
        {
            var _enumModel = new CommonService().GetEnumLists().ToList();
            LogisticsTypeLists = _enumModel.Where(m => m.GroupSeq == 3).ToList();
            GoodsTypeLists = _enumModel.Where(m => m.GroupSeq == 4).ToList();
        }


        private bool isValid=true;

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                this.RaisePropertyChanged(nameof(IsValid));
            }
        }


    }
}
