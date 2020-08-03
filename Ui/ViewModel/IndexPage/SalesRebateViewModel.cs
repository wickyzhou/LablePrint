using Common;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class SalesRebateViewModel : BaseViewModel
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
            Filter = new GeneralParameterModel() { ParamBeginDate = DateTime.Now.Date, ParamEndDate = DateTime.Now.Date };
            SalesRebateLists = new ObservableCollection<SalesRebateModel>();
            QueryParameter = new SalesRebateQueryParameterModel();
            SalesRebateSelectedItem = new SalesRebateModel();

            Task.Factory.StartNew(() =>
            {

                UIExecute.RunAsync(() =>
                {
                    _salesRebateService.GetSalesRebateLists().ForEach(x => SalesRebateLists.Add(x));
                });
            });
        }

        private void InitCommand()
        {

            SalesRebateAmountCalculateCommand = new DelegateCommand((obj) =>
            {
                _salesRebateService.CalculateSalesRebateAmount(Filter.ParamBeginDate, Filter.ParamEndDate);
                SalesRebateLists.Clear();
                _salesRebateService.GetSalesRebateLists().ForEach(x => SalesRebateLists.Add(x));
            });

            SalesRebateQueryCommand = new DelegateCommand((obj) =>
            {
                string filter = $" where MaterialName like '%{QueryParameter.ProductionModelName}%' and CaseName like '%{QueryParameter.CaseName}%' and OrgName like '%{QueryParameter.OrgName}%' ";
                SalesRebateLists.Clear();
                _salesRebateService.GetSalesRebateLists(filter).ForEach(x => SalesRebateLists.Add(x));
            });

            SalesRebateModifyCommand = new DelegateCommand((obj) =>
            {
                var cloneData = ObjectDeepCopyHelper<SalesRebateModel, SalesRebateModel>.Trans(SalesRebateSelectedItem);
                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();
                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(cloneData, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateService.Update(outputEntity))
                        {
                            SalesRebateLists.Clear();
                            _salesRebateService.GetSalesRebateLists().ForEach(x => SalesRebateLists.Add(x));
                        }

                        //ModelTypeHelper.PropertyMapper(SalesRebateSelectedItem, outputEntity);
                    }
                });
                view.ShowDialog();
            });

            SalesRebateCreateCommand = new DelegateCommand((obj) =>
            {
                SalesRebateModel inputEntity = new SalesRebateModel() { Guid = Guid.NewGuid(), OrgId = -1, CaseId = -1, MaterialId = -1 };

                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();

                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateService.Insert(outputEntity))
                            SalesRebateLists.Insert(0, _salesRebateService.GetSalesRebate(outputEntity.Guid));
                    }
                });
                view.ShowDialog();
            });

            SalesRebateCopyCommand = new DelegateCommand((obj) =>
            {
                SalesRebateModel inputEntity = _salesRebateService.GetSalesRebate(SalesRebateSelectedItem.Guid);
                Guid newGuid = Guid.NewGuid();
                // 如果比例类型是分段比例的话，先将分段数据插入到后台表(新增、复制、修改可以共用一个视图)
                if (inputEntity.RebatePctType == 2 )
                    _salesRebateService.Copy(inputEntity.Guid, newGuid);
                inputEntity.Guid = newGuid;

                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();

                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateService.Insert(outputEntity))
                            SalesRebateLists.Insert(0, _salesRebateService.GetSalesRebate(outputEntity.Guid));
                    }
                });
                view.ShowDialog();
            });

            SalesRebateRemoveCommand = new DelegateCommand((obj) =>
            {
                MessageBoxResult result = MessageBox.Show("此操作不可恢复，确认删除？", "温馨提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (_salesRebateService.Delete(SalesRebateSelectedItem))
                        SalesRebateLists.Remove(SalesRebateSelectedItem);  
                    else
                        MessageBox.Show("删除失败,系统异常，请联系管理员");
                }
            });

        }


        public DelegateCommand SalesRebateAmountCalculateCommand { get; set; }
        public DelegateCommand SalesRebateQueryCommand { get; set; }
        public DelegateCommand SalesRebateModifyCommand { get; set; }
        public DelegateCommand SalesRebateCreateCommand { get; set; }
        public DelegateCommand SalesRebateCopyCommand { get; set; }
        public DelegateCommand SalesRebateRemoveCommand { get; set; }



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
