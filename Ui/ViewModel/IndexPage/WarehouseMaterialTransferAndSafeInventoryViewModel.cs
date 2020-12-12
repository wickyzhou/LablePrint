using Common;
using K3ApiModel;
using K3ApiModel.ICStockBill41;
using K3ApiModel.Request;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ui.Command;
using Ui.Service;
using Ui.View;

namespace Ui.ViewModel.IndexPage
{
    public class WarehouseMaterialTransferAndSafeInventoryViewModel : K3ApiBaseViewModel
    {
        private readonly WarehouseMaterialTransferAndSafeInventoryService _service;

        public WarehouseMaterialTransferAndSafeInventoryViewModel()
        {
            _service = new WarehouseMaterialTransferAndSafeInventoryService();
            InitData();
            InitCommand();
        }


        public DelegateCommand BatchNoQtyK3InsertCommand { get; set; }
        public DelegateCommand NewDataGenerateCommand { get; set; }





        public DelegateCommand MaterialRequestGenerateCommand { get; set; }
        public DelegateCommand WorkshopInventoryRefreshCommand { get; set; }
        public DelegateCommand DeliverCommand { get; set; }
        public DelegateCommand DeliveryDeleteCommand { get; set; }
        public DelegateCommand TransferDeleteCommand { get; set; }
        public DelegateCommand SelectionChangedCommand { get; set; }


        private void InitCommand()
        {
            DirectorySelectBaseCommand = new DelegateCommand((obj) =>
            {
                // 导出目录选择
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    HostConfig.HostValue = fbd.SelectedPath;
                    var result = CommonService.SaveHostConfig(HostConfig);
                    if (result)
                    {
                        HostConfig = CommonService.GetHostConfig(Menu.ID, HostName, User.ID);
                    }
                }
            });

            ExportBaseCommand = new DelegateCommand((obj) =>
            {
                if (Directory.Exists(HostConfig.HostValue))
                {
                    ExportView view = new ExportView(Menu.ID, 3);// Menu.ID-SJExportViewTypedColumn中的ViewGroupId; 3 - 第几个radioButton被默认选中从1开始
                    //Export((type - 导出1 取消0, outputEntity 界面选中的radiobuttion 同上, checkBoxValue 选择分类导出的情况下，选择的类别和, orderedColumns 选择分类导出的情况下，选择类别的顺序列表) 
                    (view.DataContext as ExportViewModel).Export((type, outputEntity, checkBoxValue, orderedColumns) =>
                    {
                        view.Close();
                        if (type == 1)
                        {
                            if (outputEntity == 3)
                            {
                                DataTable datatable = _service.GetSJBatchBomRequestDeliveryExportData(GeneralParameter.ParamBeginDate.Value, string.Join(",", orderedColumns));
                                if (datatable.Rows.Count > 0)
                                {
                                    ExportHelper.ExportDeliveryDataToExcel(GeneralParameter.ParamBeginDate.Value, datatable, HostConfig.HostValue, HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(GeneralParameter)
                                        , outputEntity, orderedColumns, true, "$$$发料单", true);
                                    MessageBox.Show("导出成功");
                                }
                                else
                                    MessageBox.Show($"【{GeneralParameter.ParamBeginDate.Value}】 没有可导出的数据");
                            }
                            else
                            {
                                DataTable datatable = _service.GeSJBatchBOMSummaryExportData(GeneralParameter.ParamBeginDate.Value); 
                                if (datatable.Rows.Count > 0)
                                {
                                    ExportHelper.ExportDataTableToExcel(datatable, HostConfig.HostValue, "物料需求" + CommonService.GetQueryParameterValueString(GeneralParameter), outputEntity, null, false, "物料需求", false);
                                    MessageBox.Show("导出成功");
                                }
                                else
                                    MessageBox.Show($"【{GeneralParameter.ParamBeginDate.Value}】 没有可导出的数据");
                            }
                         

                        }
                    });
                    view.ShowDialog();
                }
                else
                {
                    MessageBox.Show("目录不存在，请先选择导出的目录");
                    DirectorySelectBaseCommand.Execute(null);
                }
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "ExportBaseCommand", ActionDesc = HostConfig.TypeDesciption + HostConfig.TypeId.ToString(), UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            MaterialRequestGenerateCommand = new DelegateCommand((obj) =>
            {
                _service.SplitBatchBomRequest(GeneralParameter.ParamBeginDate.Value);
                QueryBaseCommand.Execute(null);
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "MaterialRequestGenerateCommand", ActionDesc = "生成物料需求", UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            WorkshopInventoryRefreshCommand = new DelegateCommand((obj) =>
            {
                BatchBomRequestSummaryLists.Clear();
                _service.RefreshWorkshopInventoryQty(CommonService.GetSqlWhereString(Filter));
                QueryBaseCommand.Execute(null);
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "WorkshopInventoryRefreshCommand", ActionDesc = "获取现场库存", UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            QueryBaseCommand = new DelegateCommand((obj) =>
            {
                BatchBomRequestSummaryLists.Clear();
                InventoryLists.Clear();
                DeliverTransferLists.Clear();
                _service.GetBatchBomRequestDetailSummaryLists(CommonService.GetSqlWhereString(Filter)).ForEach(x => BatchBomRequestSummaryLists.Add(x));
            });

            DeliverCommand = new DelegateCommand((obj) =>
            {
                if (BatchBomRequestSummarySelectedItem != null)
                {
                    if (InventorySelectedItem != null && InventorySelectedItem.TransferingWeight > 0 && InventorySelectedItem.TransferingWeight <= InventorySelectedItem.TotalWeight)
                    {
                        if (_service.InsertDeliverTransfer(InventorySelectedItem))
                        {
                            DeliverTransferLists.Clear();
                            _service.GetDeliverTransferLists(BatchBomRequestSummarySelectedItem).ForEach(x => DeliverTransferLists.Add(x));
                            BatchBomRequestSummarySelectedItem.QtyTransfering = DeliverTransferLists.Where(x => string.IsNullOrEmpty(x.TransferedBillNo)).Sum(x => x.TransferingWeight).Value;
                        }
                    }
                    else
                        MessageBox.Show("发料数量必须大于0且小于总重量");
                }
                else
                    MessageBox.Show("先选择主表行数据");
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "DeliverCommand", ActionDesc ="发料", UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            DeliveryDeleteCommand = new DelegateCommand((obj) =>
            {
                int id = -1;
                if (BatchBomRequestSummarySelectedItem != null)
                {

                    var selectedLists = ((obj as DataGrid).SelectedItems).Cast<MaterialTimelyInventoryModel>().ToList();
                    if (selectedLists.Count == 1)
                    {
                        var model = selectedLists.First();
                        if (_service.DeleteDeliverTransfer(model.Id))
                        {
                            id = model.Id;
                            DeliverTransferLists.Clear();
                            _service.GetDeliverTransferLists(BatchBomRequestSummarySelectedItem).ForEach(x => DeliverTransferLists.Add(x));
                            BatchBomRequestSummarySelectedItem.QtyTransfering = DeliverTransferLists.Sum(x => x.TransferingWeight).Value;
                        }
                    }
                    else
                        MessageBox.Show("每次必须选中且删除一行记录");
                }
                else
                    MessageBox.Show("先选择主表行数据");
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "DeliveryDeleteCommand", ActionDesc ="删除发料", UserId = User.ID, MainMenuId = Menu.ID, PKId = id, HostName = HostName });
            });

            TransferDeleteCommand = new DelegateCommand((obj) =>
            {
                if (BatchBomRequestSummarySelectedItem != null)
                {
                    var selectedLists = ((obj as DataGrid).SelectedItems).Cast<MaterialTimelyInventoryModel>().ToList();
                    if (selectedLists.Count == 1)
                    {
                        var model = selectedLists.First();
                        if (!_service.ExistsK3Bill(model.TransferedBillNo))
                        {
                            if (_service.DeleteDeliverTransfer(model.TransferedBillNo))
                            {
                                //重新加载主表格
                                QueryBaseCommand.Execute(null);
                            }
                        }
                        else
                            MessageBox.Show("请先将选择的调拨单号，在K3里面删除");
                    }
                    else
                        MessageBox.Show("每次必须选中且删除一行记录");
                }
                else
                    MessageBox.Show("先选择主表行数据");
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "TransferDeleteCommand", ActionDesc = "删除调拨单", UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            SelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (BatchBomRequestSummarySelectedItem == null)
                    return;
                InventoryLists.Clear();
                _service.GetGetMaterialTimelyInventoryLists(BatchBomRequestSummarySelectedItem.MaterialId, BatchBomRequestSummarySelectedItem.ProductionDate, BatchBomRequestSummarySelectedItem.BatchTypeId, BatchBomRequestSummarySelectedItem.StockId).ForEach(x => InventoryLists.Add(x));
                DeliverTransferLists.Clear();
                _service.GetDeliverTransferLists(BatchBomRequestSummarySelectedItem).ForEach(x => DeliverTransferLists.Add(x));
            });

            BatchNoQtyK3InsertCommand = new DelegateCommand((obj) =>
            {

                // 获取当天所有已发料未调拨数据  var selectedLists = ((obj as DataGrid).SelectedItems).Cast<MaterialTimelyInventoryModel>().ToList();
                var selectedLists = _service.GetK3InsertData(GeneralParameter.ParamBeginDate.Value);
                if (selectedLists.Count > 0)
                {
                    var emp = K3ApiFKService.GetEmployeeByUserName(User.UserName);
                    if (emp != null)
                    {
                        TransferMainModel main = new TransferMainModel
                        {
                            FBillerID = new BaseNumberNameModelX { FNumber = emp.FName, FName = emp.FName },
                            FFManagerID = emp,// BaseNumberNameModelX { FNumber = "111", FName = "吴强" },
                            FSManagerID = emp,// new BaseNumberNameModelX { FNumber = "111", FName = "吴强" },
                            FRefType = new BaseNumberNameModelX { FNumber = "01", FName = "成本调拨" },
                            FClassTypeID = 41,
                            FDate = DateTime.Now.Date.ToString("yyyy-MM-dd")
                        };

                        List<TransferSonModel> sons = new List<TransferSonModel>();

                        foreach (MaterialTimelyInventoryModel item in selectedLists)
                        {
                            sons.Add(new TransferSonModel
                            {
                                FItemID = new BaseNumberNameModelX { FNumber = item.MaterialNumber, FName = item.MaterialName },// K3ApiFKService.GetMaterialById
                                FChkPassItem = new BaseNumberNameModelX { FNumber = "Y", FName = "是" },
                                FPlanMode = new BaseNumberNameModelX { FNumber = "MTS", FName = "MTS计划模式" },
                                FDCStockID1 = new BaseNumberNameModelX { FNumber = item.ParentStockNumber, FName = item.ParentStockName },//K3ApiFKService.GetStockById(item.ParentStockId),
                                FSCStockID1 = new BaseNumberNameModelX { FNumber = item.StockNumber, FName = item.StockName },
                                FUnitID = new BaseNumberNameModelX { FNumber = "kg", FName = "kg" },
                                FAuxQty = item.TransferingWeight.Value,
                                FQty = item.TransferingWeight.Value,
                                FBatchNo = item.BatchNo,
                            });
                        }
                        var requestModel = new K3ApiInsertRequestModel<TransferMainModel, TransferSonModel>()
                        {
                            Data = new K3ApiInsertDataRequestModel<TransferMainModel, TransferSonModel>()
                            {
                                Page1 = new List<TransferMainModel> { main },
                                Page2 = sons
                            }
                        };

                        string postJson = JsonHelper.ObjectToJson(requestModel);
                        K3ApiInsertResponseModel response = K3ApiService.Insert("Transfer", postJson);
                        if (response.StatusCode == 200)
                        {
                            //double totalQty = selectedLists.Sum(x => x.TransferingWeight).Value;
                            string ids = string.Join(",", selectedLists.Select(x => x.Id));
                            if (_service.UpdateBillNo(ids, response.Data.BillNo))
                            {
                                QueryBaseCommand.Execute(null);
                            }
                            else
                                MessageBox.Show($"插入K3调拨单，更新发料表失败，请联系管理员");
                        }
                        else
                            MessageBox.Show($"{response.Message}");
                    }
                    else
                        MessageBox.Show($"【{User.UserName}】 在K3里面不存在，或不一致");

                }
                else
                    MessageBox.Show($"【{GeneralParameter.ParamBeginDate.Value}】 没有已发料且未调拨的数据");

                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "BatchNoQtyK3InsertCommand", ActionDesc = "生成调拨单", UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });
        }

        private void InitData()
        {
            GeneralParameter = new GeneralParameterModel { ParamBeginDate = DateTime.Now.Date };// ProductionDate;

            Filter = new BatchBomRequestQueryParameterModel
            {
                ProductionDate1 = DateTime.Now.Date,
                ProductionDate2 = DateTime.Now.Date
            };

            InventoryLists = new ObservableCollection<MaterialTimelyInventoryModel>();
            BatchBomRequestSummaryLists = new ObservableCollection<BatchBomRequestSummaryModel>();
            DeliverTransferLists = new ObservableCollection<MaterialTimelyInventoryModel>();
            HostConfig = CommonService.GetHostConfig(Menu.ID, HostName, User.ID) ?? new HostConfigModel() { TypeId = Menu.ID, Host = HostName, UserId = User.ID, TypeDesciption = Menu.TB2Text };
            DeliveryStockLists = CommonService.GetDeliveryStock();
            //Task.Factory.StartNew(() =>
            //{
            //    UIExecute.RunAsync(() =>
            //    {

            //        QueryBaseCommand.Execute(null);

            //    });
            //});
        }

        private GeneralParameterModel generalParameter;

        public GeneralParameterModel GeneralParameter
        {
            get { return generalParameter; }
            set
            {
                generalParameter = value;
                this.RaisePropertyChanged(nameof(GeneralParameter));
            }
        }


        private BatchBomRequestQueryParameterModel queryParameter;

        public BatchBomRequestQueryParameterModel Filter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(Filter));
            }
        }


        private ObservableCollection<BatchBomRequestSummaryModel> batchBomRequestSummaryLists;

        public ObservableCollection<BatchBomRequestSummaryModel> BatchBomRequestSummaryLists
        {
            get { return batchBomRequestSummaryLists; }
            set
            {
                batchBomRequestSummaryLists = value;
                this.RaisePropertyChanged(nameof(BatchBomRequestSummaryLists));
            }
        }

        private BatchBomRequestSummaryModel batchBomRequestSummarySelectedItem;

        public BatchBomRequestSummaryModel BatchBomRequestSummarySelectedItem
        {
            get { return batchBomRequestSummarySelectedItem; }
            set
            {
                batchBomRequestSummarySelectedItem = value;
                this.RaisePropertyChanged(nameof(BatchBomRequestSummarySelectedItem));
            }
        }



        private ObservableCollection<MaterialTimelyInventoryModel> inventoryLists;

        public ObservableCollection<MaterialTimelyInventoryModel> InventoryLists
        {
            get { return inventoryLists; }
            set
            {
                inventoryLists = value;
                this.RaisePropertyChanged(nameof(InventoryLists));
            }
        }

        private MaterialTimelyInventoryModel inventorySelectedItem;

        public MaterialTimelyInventoryModel InventorySelectedItem
        {
            get { return inventorySelectedItem; }
            set
            {
                inventorySelectedItem = value;
                this.RaisePropertyChanged(nameof(InventorySelectedItem));
            }
        }

        private ObservableCollection<MaterialTimelyInventoryModel> deliverTransferLists;

        public ObservableCollection<MaterialTimelyInventoryModel> DeliverTransferLists
        {
            get { return deliverTransferLists; }
            set
            {
                deliverTransferLists = value;
                this.RaisePropertyChanged(nameof(DeliverTransferLists));
            }
        }

        public List<DeliveryStockModel> DeliveryStockLists { get; set; }

    }
}
