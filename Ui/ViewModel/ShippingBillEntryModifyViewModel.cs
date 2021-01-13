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
    public class ShippingBillEntryModifyViewModel : BaseViewModel, IValidationExceptionHandler
    {
        private Action<int, ShippingBillEntryModel> _callBack;

        public ShippingBillEntryModifyViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
            Init();
        }

        private ShippingBillEntryModel shippingBillEntry;

        public ShippingBillEntryModel ShippingBillEntry
        {
            get { return shippingBillEntry; }
            set
            {
                shippingBillEntry = value;
                this.RaisePropertyChanged(nameof(ShippingBillEntry));
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



        public void WithParam(ShippingBillEntryModel entry, Action<int, ShippingBillEntryModel> callBack)
        {
            ShippingBillEntry = entry;
            _callBack = callBack;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }

        public void Save(object obj)
        {
            if (!this.IsValid || ShippingBillEntry.Quantity <= 0|| ShippingBillEntry.GoodsType<=0 || ShippingBillEntry.EntryId<=0 || (ShippingBillEntry.Amount <= 0 && ShippingBillEntry.GoodsType != 1 && ShippingBillEntry.GoodsType != 3))
            {
                MessageBox.Show("输入的格式有误 --> 数量必填 --> 除成品样油其余需填金额");
                return;
            }
            _callBack?.Invoke(1, ShippingBillEntry);
        }

        private void Exit(object obj)
        {
            _callBack?.Invoke(0, null);
        }

        private void Init()
        {
            var _enumModel = new CommonService().GetEnumLists().ToList();
            GoodsTypeLists = _enumModel.Where(m => m.GroupSeq == 4).ToList();
        }


        private bool isValid = true;

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
