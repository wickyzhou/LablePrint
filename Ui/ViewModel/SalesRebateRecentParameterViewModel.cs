using Model;
using QueryParameterModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel
{
    public class SalesRebateRecentParameterViewModel : RefreshDialogViewModel<SalesRebateRecentParameterViewModel>
    {
        //private readonly SalesRebateService SalesRebateService;
        //private readonly SalesRebateAmountRangeService SalesRebateAmountRangeService;

        public SalesRebateService SalesRebateService { get; set; } = new SalesRebateService();
        public SalesRebateAmountRangeService SalesRebateAmountRangeService { get; set; } = new SalesRebateAmountRangeService();
        public SalesRebateBatchParameterModel BatchParameter { get; set; }

        public SalesRebateRecentParameterViewModel(SalesRebateBatchParameterModel batchParameter)
        {
            BatchParameter = batchParameter;
            QueryParameter = new SalesRebateRecentQueryParameterModel() { OrgId = BatchParameter.OrganizationSearchedItem.Id, RebateClass = BatchParameter.RebateClassSeletedItem.ItemSeq };
            QueryParameter1 = new SalesRebateRecentQueryParameterModel { RebateClass = BatchParameter.RebateClassSeletedItem.ItemSeq };
            InitData();
            InitCommand();
        }

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                SalesRebateRecentParameterLists = new ObservableCollection<SalesRebateRecentParameterMainModel>();
                SalesRebateK3RecordParameterLists = new ObservableCollection<SalesRebateRecentParameterMainModel>();

                // 按客户、类别、时间生成数据
                SalesRebateService.InsertCurrentOrgRebateClassParameter(BatchParameter);

                RebateClassLists = CommonService.GetEnumLists(6);

                var org = new OrganizationService().GetOrganizationById(BatchParameter.OrganizationSearchedItem.Id);
                OrgLists = new List<EnumModel> { new EnumModel { ItemSeq = org.Id, ItemValue = org.FName } };

                UIExecute.RunAsync(() =>
                {
                    QueryBaseCommand.Execute(null);
                });


            });
        }

        private void InitCommand()
        {
            QueryBaseCommand = new DelegateCommand((obj) =>
            {
                AmountRangeString = "";
                SalesRebateRecentParameterLists.Clear();
                SalesRebateService.GetSalesRebateOrgRecentParameterLists(CommonService.GetSqlWhereString(QueryParameter)).ForEach(x => SalesRebateRecentParameterLists.Add(x));
            });

            QueryCommand = new DelegateCommand((obj) =>
            {
                SalesRebateK3RecordParameterLists.Clear();
                SalesRebateService.GetSalesRebateK3RecordParameterLists(CommonService.GetSqlWhereString(QueryParameter1)).ForEach(x => SalesRebateK3RecordParameterLists.Add(x));
            });

            DoubleClickModifyCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateRecentParameterSelectedItem != null)
                {
                    IsChanged = true;
                    var cloneData = ObjectDeepCopyHelper<SalesRebateRecentParameterMainModel, SalesRebateRecentParameterMainModel>.Trans(SalesRebateRecentParameterSelectedItem);
                    SalesRebateRecentParameterModifyView view = new SalesRebateRecentParameterModifyView();
                    (view.DataContext as SalesRebateRecentParameterModifyViewModel).WithParam(cloneData, (type, outputEntity) =>
                    {
                        view.Close();
                        if (type == 1)
                        {
                            SalesRebateService.RecentParameterUpdate(outputEntity);
                            QueryBaseCommand.Execute(null);
                            
                        }
                    });
                    view.ShowDialog();
                }

            });

            RecentParameterClearCommand = new DelegateCommand((obj) =>
            {
                var selectedItems = (obj as DataGrid).SelectedItems;
                if (selectedItems.Count>0)
                {
                    IsChanged = true;
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (SalesRebateRecentParameterMainModel item in selectedItems)
                    {
                        stringBuilder.Append($",'{item.Guid}'");
                    }
                    string guids = stringBuilder.ToString().Substring(1);
                    SalesRebateService.ClearRecentMainParameter(guids);
                    QueryBaseCommand.Execute(null);
                }
                else
                    MessageBox.Show("必须先选择一行或多行");

            });

            RecentParameterCopyCommand = new DelegateCommand((obj) =>
            {
                var selectedItems = (obj as DataGrid).SelectedItems;
                if (selectedItems.Count == 2)
                {
                    IsChanged = true;
                    var first = selectedItems[0] as SalesRebateRecentParameterMainModel;
                    var last = selectedItems[1] as SalesRebateRecentParameterMainModel;
                    if ((last.IsPassed && !first.IsPassed))
                    {
                        SalesRebateService.SalesRebateParameterCopy(first.Id, last.Id, first.Guid, last.Guid, 1);
                        QueryBaseCommand.Execute(null);
                    }
                    else if (!last.IsPassed && first.IsPassed)
                    {
                        SalesRebateService.SalesRebateParameterCopy(last.Id, first.Id, last.Guid, first.Guid, 1);
                        QueryBaseCommand.Execute(null);
                    }
                    else if(last.IsPassed && first.IsPassed)
                        MessageBox.Show("所选行的参数都已通过验证，请先选择一行【清空参数】");
                    else
                        MessageBox.Show("必须有一行参数是已通过验证,用来覆盖先选的");
                }
                else
                    MessageBox.Show("每次只能选择2行，【后选的行】参数 覆盖 【先选的行】");

            });

            K3RecordParameterCopyCommand = new DelegateCommand((obj) =>
            {
                var selectedItems = (obj as DataGrid).SelectedItems;
                if (selectedItems.Count == 1)
                {
                    if (!SalesRebateRecentParameterSelectedItem.IsPassed)
                    {
                        if (SalesRebateK3RecordParameterSelectedItem != null)
                        {
                            IsChanged = true;
                            SalesRebateService.SalesRebateParameterCopy(SalesRebateRecentParameterSelectedItem.Id, SalesRebateK3RecordParameterSelectedItem.Id, SalesRebateRecentParameterSelectedItem.Guid, SalesRebateK3RecordParameterSelectedItem.Guid, 2);
                            QueryBaseCommand.Execute(null);
                        }
                        else
                            MessageBox.Show("需要选择一行历史参数，【历史待选的行】参数 覆盖 【当前参数】");
                    }
                    else MessageBox.Show("已通过验证的行参数不能被覆盖，请先【清空参数】");
                }
                else
                    MessageBox.Show("只能选择一行被复制的行，【历史待选的行】参数 覆盖 【当前参数】");
            });

            SelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (obj != null)
                {
                    var model = obj as SalesRebateRecentParameterMainModel;
                    if (model.RebatePctType == 2)
                    {
                        var lists = SalesRebateAmountRangeService.GetSalesRebateAmountRangeRecentParameterLists(model.PGuid);
                        StringBuilder result = new StringBuilder();
                        foreach (var item in lists)
                        {
                            result.Append($"金额区间：{item.AmountLower}-{item.AmountUpper} 万元   比例：{item.SalesRebatePctValue}% \t\t");
                        }

                        AmountRangeString = result.ToString();
                    }
                    else
                        AmountRangeString = string.Empty;

                }
            });
        }

        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand DoubleClickModifyCommand { get; set; }
        public DelegateCommand DoubleClickShowCommand { get; set; }
        public DelegateCommand RecentParameterClearCommand { get; set; }
        public DelegateCommand K3RecordParameterCopyCommand { get; set; }
        public DelegateCommand RecentParameterCopyCommand { get; set; }
        public DelegateCommand SelectionChangedCommand { get; set; }


        private SalesRebateRecentParameterMainModel salesRebateRecentParameterSelectedItem;

        public SalesRebateRecentParameterMainModel SalesRebateRecentParameterSelectedItem
        {
            get { return salesRebateRecentParameterSelectedItem; }
            set
            {
                salesRebateRecentParameterSelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateRecentParameterSelectedItem));
            }
        }

        private SalesRebateRecentParameterMainModel salesRebateK3RecordParameterSelectedItem;

        public SalesRebateRecentParameterMainModel SalesRebateK3RecordParameterSelectedItem
        {
            get { return salesRebateK3RecordParameterSelectedItem; }
            set
            {
                salesRebateK3RecordParameterSelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateK3RecordParameterSelectedItem));
            }
        }

        private ObservableCollection<SalesRebateRecentParameterMainModel> salesRebateRecentParameterLists;

        public ObservableCollection<SalesRebateRecentParameterMainModel> SalesRebateRecentParameterLists
        {
            get { return salesRebateRecentParameterLists; }
            set
            {
                salesRebateRecentParameterLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateRecentParameterLists));
            }
        }

        private ObservableCollection<SalesRebateRecentParameterMainModel> salesRebateK3RecordParameterLists;

        public ObservableCollection<SalesRebateRecentParameterMainModel> SalesRebateK3RecordParameterLists
        {
            get { return salesRebateK3RecordParameterLists; }
            set
            {
                salesRebateK3RecordParameterLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateK3RecordParameterLists));
            }
        }

        private SalesRebateRecentQueryParameterModel queryParameter;

        public SalesRebateRecentQueryParameterModel QueryParameter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }

        private SalesRebateRecentQueryParameterModel queryParameter1;

        public SalesRebateRecentQueryParameterModel QueryParameter1
        {
            get { return queryParameter1; }
            set
            {
                queryParameter1 = value;
                this.RaisePropertyChanged(nameof(QueryParameter1));
            }
        }

        private IEnumerable<EnumModel> rebateClassLists;

        public IEnumerable<EnumModel> RebateClassLists
        {
            get { return rebateClassLists; }
            set
            {
                rebateClassLists = value;
                this.RaisePropertyChanged(nameof(RebateClassLists));
            }
        }

        private IEnumerable<EnumModel> orgLists;

        public IEnumerable<EnumModel> OrgLists
        {
            get { return orgLists; }
            set
            {
                orgLists = value;
                this.RaisePropertyChanged(nameof(OrgLists));
            }
        }

        private string amountRangeString;

        public string AmountRangeString
        {
            get { return amountRangeString; }
            set
            {
                amountRangeString = value;
                this.RaisePropertyChanged(nameof(AmountRangeString));
            }
        }

    }
}
