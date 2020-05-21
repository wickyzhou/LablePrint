using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.Command;

namespace Ui.ViewModel
{
    public class ConsignmentBillModifyViewModel:NotificationObject
    {
        private Action<int, ConsignmentBillModel> _callBack;

        public ConsignmentBillModifyViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
        }

        private ConsignmentBillModel consignmentBill;

        public ConsignmentBillModel ConsignmentBill
        {
            get { return consignmentBill; }
            set
            {
                consignmentBill = value;
                this.RaisePropertyChanged(nameof(ConsignmentBill));
            }
        }


        public void WithParam(ConsignmentBillModel consignmentBill, Action<int, ConsignmentBillModel> callBack)
        {
            ConsignmentBill = consignmentBill;
            _callBack = callBack;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }


        private void Save(object obj)
        {
            _callBack?.Invoke(1, ConsignmentBill);
        }

        private void Exit(object obj)
        {
            _callBack?.Invoke(0, null);
        }

    }
}
