using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.Command;

namespace Ui.ViewModel
{
    class OilSampleEntryModifyViewModel : NotificationObject, IValidationExceptionHandler
    {
        private Action<int, OilSampleEntryModel> _callBack;

        public OilSampleEntryModifyViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
            Init();
        }


        private OilSampleEntryModel oilSampleEntry;

        public OilSampleEntryModel OilSampleEntry
        {
            get { return oilSampleEntry; }
            set
            {
                oilSampleEntry = value;
                this.RaisePropertyChanged(nameof(OilSampleEntry));
            }
        }

        public void WithParam(OilSampleEntryModel entry, Action<int, OilSampleEntryModel> callBack)
        {
            OilSampleEntry = entry;
            _callBack = callBack;
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }


        public void Save(object obj)
        {
            if (!this.IsValid )
            {
                MessageBox.Show("输入的格式有误或者有数据未填!");
                return;
            }
            _callBack?.Invoke(1, OilSampleEntry);
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
