using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.Command;

namespace Ui.ViewModel
{
  public  class ConsignmentBillEntryCopyViewModel : NotificationObject, IValidationExceptionHandler
    {
        private Action<int, ConsignmentBillEntryModel> _callBack;

        public ConsignmentBillEntryCopyViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
        }

        private ConsignmentBillEntryModel consignmentBillEntry;

        public ConsignmentBillEntryModel ConsignmentBillEntry
        {
            get { return consignmentBillEntry; }
            set
            {
                consignmentBillEntry = value;
                this.RaisePropertyChanged(nameof(ConsignmentBillEntry));
            }
        }


        public void WithParam(ConsignmentBillEntryModel entry, Action<int, ConsignmentBillEntryModel> callBack)
        {
            ConsignmentBillEntry = entry;
            _callBack = callBack;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }


        private void Save(object obj)
        {
            if (this.IsValid)
            {
                if (ConsignmentBillEntry.ECurrencyQuantity <= 0)
                {
                    MessageBox.Show("输入的数据不合法,请重新输入!");
                    return;
                }

            }
            _callBack?.Invoke(1, ConsignmentBillEntry);
        }

        private void Exit(object obj)
        {
            _callBack?.Invoke(0, null);
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
