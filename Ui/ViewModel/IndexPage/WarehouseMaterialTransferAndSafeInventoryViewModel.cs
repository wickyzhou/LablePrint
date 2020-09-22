using Common;
using K3ApiModel;
using K3ApiModel.ICStockBill41;
using K3ApiModel.Request;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Service;

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

        public DelegateCommand BatchNoQtySaveCommand { get; set; }
        public DelegateCommand BatchNoQtyK3InsertCommand { get; set; }
        public DelegateCommand NewDataGenerateCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand SelectionChangedCommand { get; set; }
        public DelegateCommand DirectorySelectCommand { get; set; }
        public DelegateCommand ListsExportCommand { get; set; }
        public DelegateCommand TransferClearCommand { get; set; }




        private void InitCommand()
        {
            NewDataGenerateCommand = new DelegateCommand((obj) =>
            {
                var result = _service.GenerationNewData(DateParamter.ParamBeginDate, DateParamter.ParamEndDate);
                if (result == null)
                {
                    InventoryLists.Clear();
                    _service.GetWarehouseTransferToWorkshopLists(" and Deleted = 0  and QtyTransfering > 0 ").ForEach(x => WarehouseTransferToWorkshopLists.Add(x));
                }
                else
                    MessageBox.Show(result.ToString());
            });

            BatchNoQtySaveCommand = new DelegateCommand((obj) =>
            {
                if (WarehouseTransferToWorkshopSelectedItem.TransferedBillNo.Length > 0)
                {
                    MessageBox.Show("已调拨不能执行【发料】操作");
                    return;
                }

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in InventoryLists)
                {
                    if (item.TransferWeight != null && item.TransferWeight.Value > 0)
                        stringBuilder.Append($", {item.BatchNo}({item.TransferWeight.Value})");
                }
                if (stringBuilder.Length > 0)
                {
                    string s = stringBuilder.ToString().Substring(2);
                    if (_service.Update(WarehouseTransferToWorkshopSelectedItem.Id, s))
                        WarehouseTransferToWorkshopSelectedItem.FBatchNoAndActualQty = s;
                }


            });

            SelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (WarehouseTransferToWorkshopSelectedItem == null)
                    return;
                InventoryLists.Clear();
                _service.GetInventoryBatchNoLists(WarehouseTransferToWorkshopSelectedItem.FItemId).ForEach(x => InventoryLists.Add(x));
            });

            BatchNoQtyK3InsertCommand = new DelegateCommand((obj) =>
            {
                if (WarehouseTransferToWorkshopSelectedItem.FBatchNoAndActualQty.Length < 3)
                {
                    MessageBox.Show("请先执行【发料】操作");
                    return;
                }

                if (WarehouseTransferToWorkshopSelectedItem.TransferedBillNo.Length > 0)
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
                double totalQty = 0;
                foreach (InventoryBatchNoModel item in InventoryLists)
                {
                    if (item.TransferWeight == null || item.TransferWeight.Value <= 0)
                        continue;

                    sons.Add(new TransferSonModel
                    {
                        FItemID = new BaseNumberNameModelX { FNumber = WarehouseTransferToWorkshopSelectedItem.FNumber, FName = WarehouseTransferToWorkshopSelectedItem.FItemName },// K3ApiFKService.GetMaterialById
                        FChkPassItem = new BaseNumberNameModelX { FNumber = "Y", FName = "是" },
                        FPlanMode = new BaseNumberNameModelX { FNumber = "MTS", FName = "MTS计划模式" },
                        FDCStockID1 = K3ApiFKService.GetStockById(WarehouseTransferToWorkshopSelectedItem.FStockID),
                        FSCStockID1 = new BaseNumberNameModelX { FNumber = item.StockNumber, FName = item.StockName },
                        FUnitID = new BaseNumberNameModelX { FNumber = "kg", FName = "kg" },
                        FAuxQty = item.TransferWeight.Value,
                        FQty = item.TransferWeight.Value,
                        FBatchNo = item.BatchNo,
                    });
                    totalQty += item.TransferWeight.Value;
                }

                if (totalQty == 0)
                {
                    MessageBox.Show("调拨明细重量均为0，无法调拨");
                    return;
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
                    if (_service.UpdateK3Bill(WarehouseTransferToWorkshopSelectedItem.Id, response.Data.BillNo, totalQty))
                    {
                        WarehouseTransferToWorkshopSelectedItem.TransferedBillNo = response.Data.BillNo;
                        WarehouseTransferToWorkshopSelectedItem.QtyTransfered = totalQty;
                        WarehouseTransferToWorkshopSelectedItem.TransferTime = DateTime.Now;
                    }
                }
                else
                {
                    MessageBox.Show($"{response.Message}");
                    return;
                }
            });



            QueryCommand = new DelegateCommand((obj) =>
            {
                StringBuilder filters = new StringBuilder($" and CreateTime >= '{QueryParameter.BeginDate.Date}' and CreateTime <= '{QueryParameter.EndDate.AddDays(1).Date}' ");
                string typeName = QueryParameter.BatchTypeCode.Replace("，", ",");
                if (typeName.IndexOf(",") > 0)
                {
                    string orfield = string.Empty;
                    foreach (var item in typeName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        orfield += $"or BatchTypeCode like '%{item}%' ";
                    }
                    orfield = orfield.Length > 2 ? orfield.Substring(2) : " 1 = 1 ";
                    filters.Append($" and ( {orfield}  )");
                }
                else
                    filters.Append($" and BatchTypeCode like '%{typeName}%' ");

                filters.Append(QueryParameter.IsTransfering ? " and QtyTransfering > 0 " : "");
                filters.Append(QueryParameter.IsTransfered ? " and QtyTransfered > 0 " : "");
                filters.Append(QueryParameter.IsDeleted ? "" : " and Deleted = 0 ");

                WarehouseTransferToWorkshopLists.Clear();
                _service.GetWarehouseTransferToWorkshopLists(filters.ToString()).ForEach(x => WarehouseTransferToWorkshopLists.Add(x));
            });

            DirectorySelectCommand = new DelegateCommand((obj) =>
            {
                // 导出目录选择
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    HostConfig.HostValue = fbd.SelectedPath;
                    var result = CommonService.SaveHostConfig(HostConfig);
                    if (result)
                    {
                        HostConfig = CommonService.GetHostConfig(6, HostName, User.ID);
                    }
                }
            });

            ListsExportCommand = new DelegateCommand((obj) =>
            {
                var s = obj as DataTable;
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "ListsExportCommand", ActionDesc = "导出发料安全库存", UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
                //HostConfig.HostValue = "";
                //AddAsync(null);
                //DateParamter.ParamEndDate = new DateTime (2020, 8, 8);
            });


            TransferClearCommand = new DelegateCommand((obj) =>
            {
                if (WarehouseTransferToWorkshopSelectedItem == null)
                    return;

                if (_service.DeleteTransferBillNo(WarehouseTransferToWorkshopSelectedItem.Id))
                {
                    WarehouseTransferToWorkshopSelectedItem.QtyTransfered = null;
                    WarehouseTransferToWorkshopSelectedItem.TransferedBillNo = null;
                    WarehouseTransferToWorkshopSelectedItem.TransferTime = null;
                    WarehouseTransferToWorkshopSelectedItem.FBatchNoAndActualQty = null;
                }
            });
        }


        public Task AddAsync(ActionOperationLogModel model)
        {
            return Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    HostConfig.HostValue += i.ToString();
                }

            });
        }

        public void Add(ActionOperationLogModel model)
        {
            Task.Factory.StartNew(() =>
           {
               for (int i = 0; i < 10; i++)
               {
                   Thread.Sleep(1000);
                   HostConfig.HostValue += i.ToString();
               }

           });
        }




        private void InitData()
        {
            DateParamter = new GeneralParameterModel { ParamBeginDate = DateTime.Now.AddDays(-3).Date, ParamEndDate = DateTime.Now.AddDays(1).Date };

            QueryParameter = new WarehouseMaterialTransferAndSafeInventoryParameterModel()
            {
                BeginDate = DateTime.Now.AddDays(-7).Date,
                EndDate = DateTime.Now.AddDays(1).Date,
                IsTransfering = true,
                BatchTypeName = "",
                IsDeleted = false,
                IsTransfered = false,
                BatchTypeCode = ""
            };
            WarehouseTransferToWorkshopLists = new ObservableCollection<WarehouseTransferToWorkshopModel>();
            InventoryLists = new ObservableCollection<InventoryBatchNoModel>();

            Task.Factory.StartNew(() =>
            {
                UIExecute.RunAsync(() =>
                {
                    HostConfig = CommonService.GetHostConfig(6, HostName, User.ID) ?? new HostConfigModel() { TypeId = 6, Host = HostName, UserId = User.ID, TypeDesciption = "发料安全库存" };
                });
            });
        }



        private ObservableCollection<InventoryBatchNoModel> inventoryLists;

        public ObservableCollection<InventoryBatchNoModel> InventoryLists
        {
            get { return inventoryLists; }
            set
            {
                inventoryLists = value;
                this.RaisePropertyChanged(nameof(InventoryLists));
            }
        }

        private ObservableCollection<WarehouseTransferToWorkshopModel> warehouseTransferToWorkshopLists;

        public ObservableCollection<WarehouseTransferToWorkshopModel> WarehouseTransferToWorkshopLists
        {
            get { return warehouseTransferToWorkshopLists; }
            set
            {
                warehouseTransferToWorkshopLists = value;
                this.RaisePropertyChanged(nameof(WarehouseTransferToWorkshopLists));
            }
        }

        private WarehouseTransferToWorkshopModel warehouseTransferToWorkshopSelectedItem;

        public WarehouseTransferToWorkshopModel WarehouseTransferToWorkshopSelectedItem
        {
            get { return warehouseTransferToWorkshopSelectedItem; }
            set
            {
                warehouseTransferToWorkshopSelectedItem = value;
                this.RaisePropertyChanged(nameof(WarehouseTransferToWorkshopSelectedItem));
            }
        }

        private WarehouseMaterialTransferAndSafeInventoryParameterModel parameterModel;

        public WarehouseMaterialTransferAndSafeInventoryParameterModel QueryParameter
        {
            get { return parameterModel; }
            set
            {
                parameterModel = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }

        private GeneralParameterModel dateParamter;

        public GeneralParameterModel DateParamter
        {
            get { return dateParamter; }
            set
            {
                dateParamter = value;
                this.RaisePropertyChanged(nameof(DateParamter));
            }
        }



    }
}
