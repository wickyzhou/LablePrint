using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Ui.Command;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class SalesRebateViewModel:BaseViewModel
    {

        private SalesRebateService _salesRebateService;

        public SalesRebateViewModel()
        {
            _salesRebateService = new SalesRebateService();
            InitCommand();
            InitData();
        }

        private void InitData()
        {
            Filter = new GeneralParameterModel() { ParamBeginDate = DateTime.Now.Date , ParamEndDate = DateTime.Now.Date };
            QueryParameter = new SalesRebateQueryParameterModel();
            SalesRebateSelectedItem = new SalesRebateModel();

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

            SalesRebateModifyCommand = new DelegateCommand((obj) =>
            {

            });

            SalesRebateCreateCommand = new DelegateCommand((obj) =>
            {
                SalesRebateModel inputEntity = new SalesRebateModel();
                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();

                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    //if (type == 1)
                    //{
                    //    if (_salesRebateService.Insert(outputEntity))
                    //        SalesRebateLists.Add(outputEntity);
                    //}
                });
                view.ShowDialog();
            });

            SalesRebateCopyCommand = new DelegateCommand((obj) =>
            {
                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();
                view.ShowDialog();
            });

        }

        public DelegateCommand SalesRebatePercentageCalculateCommand { get; set; }
        public DelegateCommand SalesRebateAmountCalculateCommand { get; set; }
        public DelegateCommand SalesRebateQueryCommand { get; set; }
        public DelegateCommand SalesRebateModifyCommand { get; set; }
        public DelegateCommand SalesRebateCreateCommand { get; set; }
        public DelegateCommand SalesRebateCopyCommand { get; set; }



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


        private SalesRebateModel salesRebateSelectedItem;

        public SalesRebateModel SalesRebateSelectedItem
        {
            get { return salesRebateSelectedItem; }
            set
            {
                salesRebateSelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateSelectedItem));
            }
        }


    }
}
