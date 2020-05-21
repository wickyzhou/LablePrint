using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ui.Command;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel
{
   public class ConsignmentShippingViewModel:NotificationObject
    {
        private ShippingBillService _shippingService;
        private ConsignmentBillService _consignmentService;

        public ConsignmentShippingViewModel()
        {
            _shippingService = new ShippingBillService();
            _consignmentService = new ConsignmentBillService();
            ConsignmentBills = new ObservableCollection<ConsignmentBillModel>();
            ShippingBills = new ObservableCollection<ShippingBillModel>();
            Filter = new ConsignmentBillParameterModel() { ParamDate= Convert.ToDateTime(System.DateTime.Now.AddDays(-1).ToShortDateString()),ParamRestQuatity=0
        };

            Query(null);
            //GetShippingBills();
            //GetConsignmentBills();

            ModifyConsignmentBillCommand = new DelegateCommand(ModifyConsignmentBill);
            ClearSelectedConsignmentBillsCommand = new DelegateCommand(ClearSelectedConsignmentBills);
            SelectAllConsignmentBillsCommand = new DelegateCommand(SelectAllConsignmentBills);
            ConsignmentMergeCommand = new DelegateCommand(ConsignmentMerge);
            QueryCommand = new DelegateCommand(Query);
        }

        #region 数据属性
        private ConsignmentBillParameterModel filter;

        public ConsignmentBillParameterModel Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                this.RaisePropertyChanged(nameof(Filter));
            }
        }

        private ObservableCollection<ConsignmentBillModel> consignmentBills;

        public ObservableCollection<ConsignmentBillModel> ConsignmentBills
        {
            get { return consignmentBills; }
            set
            {
                consignmentBills = value;
                this.RaisePropertyChanged(nameof(ConsignmentBills));
            }
        }

        private ConsignmentBillModel selectedConsignmentBill;

        public ConsignmentBillModel SelectedConsignmentBill
        {
            get { return selectedConsignmentBill; }
            set
            {
                selectedConsignmentBill = value;
                this.RaisePropertyChanged(nameof(SelectedConsignmentBill));
            }
        }

        private ObservableCollection<ShippingBillModel> shippingBills;

        public ObservableCollection<ShippingBillModel> ShippingBills
        {
            get { return shippingBills; }
            set
            {
                shippingBills = value;
                this.RaisePropertyChanged(nameof(ShippingBills));
            }
        }

        private ShippingBillModel selectedShippingBill;

        public ShippingBillModel SelectedShippingBill
        {
            get { return selectedShippingBill; }
            set
            {
                selectedShippingBill = value;
                this.RaisePropertyChanged(nameof(SelectedShippingBill));
            }
        }

        private int consignmentSum;

        public int ConsignmentSum
        {
            get { return consignmentSum; }
            set
            {
                consignmentSum = value;
                this.RaisePropertyChanged(nameof(ConsignmentSum));
            }
        }


        #endregion

        #region 命令属性
        public DelegateCommand ModifyConsignmentBillCommand { get; set; }
        public DelegateCommand ClearSelectedConsignmentBillsCommand { get; set; }
        public DelegateCommand SelectAllConsignmentBillsCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand ConsignmentMergeCommand { get; set; }
        #endregion

        public void GetConsignmentBills()
        {   
            ConsignmentBills.Clear();
            _consignmentService.GetAllConsignmentBills().ToList().ForEach( x=> ConsignmentBills.Add(x));
            ConsignmentSum = ConsignmentBills.Count();
        }

        public void GetShippingBills()
        {
            ShippingBills.Clear();
            _shippingService.GetAllShippingBills().ToList().ForEach(x=> ShippingBills.Add(x));
        }



        private void ModifyConsignmentBill(object obj)
        {
      
            if (SelectedConsignmentBill == null) return;

            ConsignmentBillModifyView edit = new ConsignmentBillModifyView();

            (edit.DataContext as ConsignmentBillModifyViewModel).WithParam(SelectedConsignmentBill, (type, val) =>
            {
                edit.Close();
                if (type == 1)
                {
                    _consignmentService.Update(val);
                    //GetConsignmentBills();
                }
            });
            edit.ShowDialog();
        }

        private void ClearSelectedConsignmentBills(object obj)
        {

            foreach (var item in ConsignmentBills)
            {
                item.IsChecked = false;
            }
        }

        private void SelectAllConsignmentBills(object obj)
        {
            foreach (var item in ConsignmentBills)
            {
                item.IsChecked = true;
            }
        }

        private void Query(object obj)
        {
            ConsignmentBills.Clear();
            var para = Filter ;//(ConsignmentBillParameterModel)obj;
            
            List<string> filters = new List<string>();
            if (!string.IsNullOrEmpty(para.ParamBillNo) )
            {
                filters.Add($" and FBillNo like '%{para.ParamBillNo}%' ");
            }
            if (!string.IsNullOrEmpty(para.ParamDeptName))
            {
                filters.Add($" and DeptName like '%{para.ParamDeptName}%' ");
            }
            if (!string.IsNullOrEmpty(para.ParamCustName))
            {
                filters.Add($" and CustName like '%{para.ParamCustName}%' ");
            }
            if (!string.IsNullOrEmpty(para.ParamMaterialName))
            {
                filters.Add($" and MaterialName like '%{para.ParamMaterialName}%' ");
            }
            string filter = $" and FDate >= '{para.ParamDate}' and FRestQuatity > {para.ParamRestQuatity} "+ string.Join(" ",filters);
            _consignmentService.GetAllConsignmentBills(filter).ToList().ForEach(x => ConsignmentBills.Add(x));
            ConsignmentSum = ConsignmentBills.Count();
        }

        private void ConsignmentMerge(object obj)
        { 

        }
    }
}
