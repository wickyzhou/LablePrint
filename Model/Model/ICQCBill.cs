namespace Model
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ICQCBill : INotifyPropertyChanged//显示指定对字段监听，双向绑定，没有监听的即使赋值也没有用
    {

        ////2019.11.20 新增包装桶数： 用来计算复制后计算合格数量 = X * Y
        public string Package { get; set; }

        private bool isChecked = false;

        public event PropertyChangedEventHandler PropertyChanged; //监听类


        //示例
        //private int _age;
        //public int Age
        //{
        //    set
        //    {
        //        _age = value;
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("Age"));//对Age进行监听
        //        }
        //    }
        //    get
        //    {
        //        return _age;
        //    }
        //}


        //将监听字段封装为函数
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

        public string checkResults { get; set; }

        public string CheckResults
        {
            get
            {
                return this.checkResults;
            }
            set
            {
                if (this.checkResults != value)
                {
                    this.checkResults = value;
                    this.OnPropertyChanged("checkResults");
                }
            }
        }

        public string CheckStanardName { get; set; }

        public decimal FAuxQty { get; set; }

        public string FBatchNo { get; set; }

        public DateTime FDate { get; set; }

        public int FItemID { get; set; }

        public string FName { get; set; }

        public string FNumber { get; set; }

        public string FTypeName { get; set; }

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

