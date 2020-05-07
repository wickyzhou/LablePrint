namespace Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class t_CheckEntry : INotifyPropertyChanged
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

        public string CheckMethodName { get; set; }

        public string CheckName { get; set; }

        public string CheckResults { get; set; }

        public string CheckStanardName { get; set; }

        public int FEntryID { get; set; }

        public int FInterID { get; set; }

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

