using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ui.Command;

namespace Ui.ViewModel.IndexPage
{
    public class SalesRebateViewModel:BaseViewModel
    {
        public SalesRebateViewModel()
        {
            InitCommand();
            InitData();
        }

        private void InitData()
        {
            Filter = new GeneralParameterModel() { ParamBeginDate = DateTime.Now.Date , ParamEndDate = DateTime.Now.Date };
            QueryParameter = new SalesRebateQueryParameterModel();
            

            Task.Factory.StartNew(() =>
            {
                SalesRebateLists = new ObservableCollection<SalesRebateModel>();
                int i = 0;
                Thread.Sleep(10000);
                UIExecute.RunAsync(() =>
                {
                    QueryParameter.ProductionModelName += "sdfsd";
                });
            });
        }

        private void InitCommand()
        {
            SalesRebatePercentageCalculateCommand = new DelegateCommand((obj)=> 
            {

            });

            SalesRebateAmountCalculateCommand = new DelegateCommand((obj) =>
            {

            });

            SalesRebateQueryCommand = new DelegateCommand((obj) =>
            {

            });
        }

        public DelegateCommand SalesRebatePercentageCalculateCommand { get; set; }
        public DelegateCommand SalesRebateAmountCalculateCommand { get; set; }
        public DelegateCommand SalesRebateQueryCommand { get; set; }

        private GeneralParameterModel filter;

        public GeneralParameterModel Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                this.RaisePropertyChanged(nameof(Filter));
            }
        }

        private SalesRebateQueryParameterModel queryParameter;

        public SalesRebateQueryParameterModel QueryParameter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }


        private ObservableCollection<SalesRebateModel> salesRebateLists;

        public ObservableCollection<SalesRebateModel> SalesRebateLists
        {
            get { return salesRebateLists; }
            set
            {
                salesRebateLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateLists));
            }
        }




    }
}
