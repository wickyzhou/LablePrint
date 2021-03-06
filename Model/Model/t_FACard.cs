﻿namespace Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class t_FACard : INotifyPropertyChanged
    {
        private bool isChecked = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public string FAssetName { get; set; }

        public string FAssetNumber { get; set; }

        public DateTime FBeginUseDate { get; set; }

        public string FModel { get; set; }

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.OnPropertyChanged("IsChecked");
                }
            }
        }
    }
}

