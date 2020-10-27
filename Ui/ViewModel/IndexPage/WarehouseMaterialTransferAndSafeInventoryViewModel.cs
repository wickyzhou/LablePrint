﻿using Common;
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
                    ExportView view = new ExportView(Menu.ID,3);// Menu.ID-SJExportViewTypedColumn中的ViewGroupId; 3 - 第几个radioButton被默认选中从1开始
                    //Export((type - 导出1 取消0, outputEntity 界面选中的radiobuttion 同上, checkBoxValue 选择分类导出的情况下，选择的类别和, orderedColumns 选择分类导出的情况下，选择类别的顺序列表) 
                    (view.DataContext as ExportViewModel).Export((type, outputEntity, checkBoxValue, orderedColumns) =>
                    {
                        view.Close();
                        if (type == 1)
                        {
                            DataTable datatable = _service.GetSJBatchBomRequestDeliveryExportData(GeneralParameter.ParamBeginDate.Value,string.Join(",", orderedColumns));
                            if (datatable.Rows.Count > 0)
                            {
                                ExportHelper.ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(GeneralParameter)
                                    , outputEntity, orderedColumns,true,"$$$发料单",true);
                                MessageBox.Show("导出成功");
                            }
                            else
                                MessageBox.Show("没有可导出的数据");
                           
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
            });

            WorkshopInventoryRefreshCommand = new DelegateCommand((obj) =>
            {
                BatchBomRequestSummaryLists.Clear();
                _service.RefreshWorkshopInventoryQty(CommonService.GetSqlWhereString(Filter));
                QueryBaseCommand.Execute(null);
            });

            QueryBaseCommand = new DelegateCommand((obj) =>
            {
                BatchBomRequestSummaryLists.Clear();
                _service.GetBatchBomRequestDetailSummaryLists(CommonService.GetSqlWhereString(Filter)).ForEach(x=> BatchBomRequestSummaryLists.Add(x));
            });

            DeliverCommand = new DelegateCommand((obj) =>
            {
                if (InventorySelectedItem != null && InventorySelectedItem.TransferingWeight > 0 && InventorySelectedItem.TransferingWeight <= InventorySelectedItem.TotalWeight&& BatchBomRequestSummarySelectedItem != null)
                {
                    if (_service.InsertDeliverTransfer(InventorySelectedItem))
                    {
                        DeliverTransferLists.Clear();
                        _service.GetDeliverTransferLists(InventorySelectedItem.MaterialId, InventorySelectedItem.ProductionDate).ForEach(x => DeliverTransferLists.Add(x));
                        BatchBomRequestSummarySelectedItem.QtyTransfering = DeliverTransferLists.Where(x => string.IsNullOrEmpty(x.TransferedBillNo)).Sum(x=>x.TransferingWeight).Value;
                    }
                        
                }
                else
                    MessageBox.Show("发料数量必须大于0且小于总重量");
            });

            DeliveryDeleteCommand = new DelegateCommand((obj) =>
            {
                if (BatchBomRequestSummarySelectedItem == null)
                {
                    MessageBox.Show("先选择主表行数据");
                    return;
                }
                  
                var selectedLists = ((obj as DataGrid).SelectedItems).Cast<MaterialTimelyInventoryModel>().ToList();
                if (selectedLists.Count == 1)
                {   
                    var model = selectedLists.First();
                    if (string.IsNullOrEmpty(model.TransferedBillNo))
                    {
                        if (_service.DeleteDeliverTransfer(model.Id))
                        {
                            DeliverTransferLists.Clear();
                            _service.GetDeliverTransferLists(model.MaterialId, model.ProductionDate).ForEach(x => DeliverTransferLists.Add(x));
                            BatchBomRequestSummarySelectedItem.QtyTransfering = DeliverTransferLists.Where(x => string.IsNullOrEmpty(x.TransferedBillNo)).Sum(x => x.TransferingWeight).Value;
                        }
                    }
                    else
                        MessageBox.Show("已经调拨数据，需要用【删除调拨】功能");
               
                }
                else
                    MessageBox.Show("每次必须选中且删除一行记录");
            });

            TransferDeleteCommand = new DelegateCommand((obj) =>
            {
                if (BatchBomRequestSummarySelectedItem == null)
                {
                    MessageBox.Show("先选择主表行数据");
                    return;
                }

                var selectedLists = ((obj as DataGrid).SelectedItems).Cast<MaterialTimelyInventoryModel>().ToList();
                if (selectedLists.Count == 1)
                {
                    var model = selectedLists.First();
                    if (!_service.ExistsK3Bill(model.TransferedBillNo))
                    {
                        if (_service.DeleteDeliverTransfer(model.TransferedBillNo))
                        {
                            DeliverTransferLists.Clear();
                            _service.GetDeliverTransferLists(model.MaterialId, model.ProductionDate).ForEach(x => DeliverTransferLists.Add(x));
                            BatchBomRequestSummarySelectedItem.QtyTransfering = DeliverTransferLists.Where(x => string.IsNullOrEmpty(x.TransferedBillNo)).Sum(x => x.TransferingWeight).Value;
                            BatchBomRequestSummarySelectedItem.QtyTransfered = DeliverTransferLists.Where(x => !string.IsNullOrEmpty(x.TransferedBillNo)).Sum(x => x.TransferingWeight).Value;
                        }
                    }
                    else
                        MessageBox.Show("请先将选择的调拨单号，在K3里面删除");
                }
                else
                    MessageBox.Show("每次必须选中且删除一行记录");

            });

            SelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (BatchBomRequestSummarySelectedItem == null)
                    return;
                InventoryLists.Clear();
                _service.GetGetMaterialTimelyInventoryLists(BatchBomRequestSummarySelectedItem.MaterialId, BatchBomRequestSummarySelectedItem.ProductionDate).ForEach(x => InventoryLists.Add(x));
                DeliverTransferLists.Clear();
                _service.GetDeliverTransferLists(BatchBomRequestSummarySelectedItem.MaterialId, BatchBomRequestSummarySelectedItem.ProductionDate).ForEach(x => DeliverTransferLists.Add(x));
            });

            BatchNoQtyK3InsertCommand = new DelegateCommand((obj) =>
            {
                if (BatchBomRequestSummarySelectedItem == null)
                {
                    MessageBox.Show("先选择主表行数据");
                    return;
                }

                var selectedLists = ((obj as DataGrid).SelectedItems).Cast<MaterialTimelyInventoryModel>().ToList();

                if (selectedLists.Where(x=>!string.IsNullOrEmpty(x.TransferedBillNo)).Count()>0)
                {
                    MessageBox.Show("不能重复调拨");
                    return;
                }

                TransferMainModel main = new TransferMainModel
                {
                    FBillerID = new BaseNumberNameModelX { FNumber = "吴强", FName = "吴强" },
                    FFManagerID = new BaseNumberNameModelX { FNumber = "111", FName = "吴强" },
                    FSManagerID = new BaseNumberNameModelX { FNumber = "111", FName = "吴强" },
                    FRefType = new BaseNumberNameModelX { FNumber = "01", FName = "成本调拨" },
                    FClassTypeID = 41,
                    FDate = DateTime.Now.Date.ToString("yyyy-MM-dd")
                };

                List<TransferSonModel> sons = new List<TransferSonModel>();
          
                foreach (MaterialTimelyInventoryModel item in selectedLists)
                {
                    sons.Add(new TransferSonModel
                    {
                        FItemID = new BaseNumberNameModelX { FNumber = BatchBomRequestSummarySelectedItem.MaterialNumber, FName = BatchBomRequestSummarySelectedItem.MaterialName },// K3ApiFKService.GetMaterialById
                        FChkPassItem = new BaseNumberNameModelX { FNumber = "Y", FName = "是" },
                        FPlanMode = new BaseNumberNameModelX { FNumber = "MTS", FName = "MTS计划模式" },
                        FDCStockID1 = K3ApiFKService.GetStockById(BatchBomRequestSummarySelectedItem.StockId),
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
                    double totalQty = selectedLists.Sum(x => x.TransferingWeight).Value;
                    string ids = string.Join(",", selectedLists.Select(x=>x.Id));
                    if (_service.UpdateBillNo(ids, totalQty,response.Data.BillNo))
                    {
                        DeliverTransferLists.Clear();
                        _service.GetDeliverTransferLists(BatchBomRequestSummarySelectedItem.MaterialId, BatchBomRequestSummarySelectedItem.ProductionDate).ForEach(x => DeliverTransferLists.Add(x));
                        BatchBomRequestSummarySelectedItem.QtyTransfering = DeliverTransferLists.Where(x => string.IsNullOrEmpty(x.TransferedBillNo)).Sum(x => x.TransferingWeight).Value;
                        BatchBomRequestSummarySelectedItem.QtyTransfered = DeliverTransferLists.Where(x => !string.IsNullOrEmpty(x.TransferedBillNo)).Sum(x => x.TransferingWeight).Value;
                    }
                }
                else
                    MessageBox.Show($"{response.Message}");

            });
        }

        private void InitData()
        {
            GeneralParameter = new GeneralParameterModel { ParamBeginDate = DateTime.Now.Date };// ProductionDate;

            Filter = new BatchBomRequestQueryParameterModel
            {
                ProductionDateBegin = DateTime.Now.Date,
                ProductionDateEnd = DateTime.Now.Date
            };

            InventoryLists = new ObservableCollection<MaterialTimelyInventoryModel>();
            BatchBomRequestSummaryLists = new ObservableCollection<BatchBomRequestSummaryModel>();
            DeliverTransferLists = new ObservableCollection<MaterialTimelyInventoryModel>();
            Task.Factory.StartNew(() =>
            {
                UIExecute.RunAsync(() =>
                {
                    HostConfig = CommonService.GetHostConfig(Menu.ID, HostName, User.ID) ?? new HostConfigModel() { TypeId = Menu.ID, Host = HostName, UserId = User.ID, TypeDesciption = Menu.TB2Text };
                    QueryBaseCommand.Execute(null);
                    
                });
            });
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



    }
}
