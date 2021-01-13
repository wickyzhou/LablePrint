using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.Command;

namespace Ui.ViewModel
{
   public class OilSampleFlowLogModifyViewModel : BaseViewModel, IValidationExceptionHandler
    {
        private Action<int, OilSampleFlowPrintLogModel> _callBack;

        public OilSampleFlowLogModifyViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
            Init();
        }


        private OilSampleFlowPrintLogModel oilSampleFlowPrintLog;

        public OilSampleFlowPrintLogModel OilSampleFlowPrintLog
        {
            get { return oilSampleFlowPrintLog; }
            set
            {
                oilSampleFlowPrintLog = value;
                this.RaisePropertyChanged(nameof(OilSampleFlowPrintLog));
            }
        }

        public void WithParam(OilSampleFlowPrintLogModel entry, Action<int, OilSampleFlowPrintLogModel> callBack)
        {
            OilSampleFlowPrintLog = entry;
            _callBack = callBack;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }


        public void Save(object obj)
        {
            if (!this.IsValid)
            {
                MessageBox.Show("输入的格式有误或者有数据未填!");
                return;
            }
            _callBack?.Invoke(1, OilSampleFlowPrintLog);
        }

        private void Exit(object obj)
        {
            _callBack?.Invoke(0, null);
        }

        private void Init()
        {
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
