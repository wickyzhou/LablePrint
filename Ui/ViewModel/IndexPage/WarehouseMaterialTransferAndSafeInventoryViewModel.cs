using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel.IndexPage
{
    public class WarehouseMaterialTransferAndSafeInventoryViewModel : BaseViewModel
    {
        private readonly WarehouseMaterialTransferAndSafeInventoryService _Service;

        public WarehouseMaterialTransferAndSafeInventoryViewModel()
        {
            _Service = new WarehouseMaterialTransferAndSafeInventoryService();
            InitData();
            InitCommand();
            var s = Menu;
        }

        public DelegateCommand BatchNoQtySaveCommand { get; set; }
        public DelegateCommand BatchNoQtyK3InsertCommand { get; set; }
        public DelegateCommand NewDataGenerateCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand SelectionChangedCommand { get; set; }
        public DelegateCommand DirectorySelectCommand { get; set; }
        public DelegateCommand ListsExportCommand { get; set; }



        private void InitCommand()
        {
            NewDataGenerateCommand = new DelegateCommand((obj) =>
            {
                var result = _Service.GenerationNewData(DateParamter.ParamBeginDate, DateParamter.ParamEndDate);
                if (result == null)
                {
                    InventoryLists.Clear();
                    _Service.GetWarehouseTransferToWorkshopLists(" and Deleted = 0  and QtyTransfering > 0 ").ForEach(x => WarehouseTransferToWorkshopLists.Add(x));
                }
                else
                    MessageBox.Show(result.ToString());
            });

            BatchNoQtySaveCommand = new DelegateCommand((obj) =>
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in InventoryLists)
                {
                    if (item.TransferWeight != null && item.TransferWeight.Value > 0)
                        stringBuilder.Append($", {item.BatchNo}({item.TransferWeight.Value})");
                }
                if (stringBuilder.Length > 0)
                {
                    string s = stringBuilder.ToString().Substring(2);
                    //if (_Service.Update(WarehouseTransferToWorkshopSelectedItem.Id, s))
                    WarehouseTransferToWorkshopSelectedItem.FBatchNoAndActualQty = s;
                }


            });

            SelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (WarehouseTransferToWorkshopSelectedItem == null)
                    return;
                InventoryLists.Clear();
                _Service.GetInventoryBatchNoLists(WarehouseTransferToWorkshopSelectedItem.FItemId).ForEach(x => InventoryLists.Add(x));
            });

            BatchNoQtyK3InsertCommand = new DelegateCommand((obj) =>
            {

            });



            QueryCommand = new DelegateCommand((obj) =>
            {
                StringBuilder filters = new StringBuilder();
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
                _Service.GetWarehouseTransferToWorkshopLists(filters.ToString()).ForEach(x => WarehouseTransferToWorkshopLists.Add(x));


            });

            DirectorySelectCommand = new DelegateCommand((obj) =>
            {
                HostConfig.HostValue = "";
                Add(null);
                DateParamter.ParamBeginDate = new DateTime(2020, 8, 18);
                // 导出目录选择
                //System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

                //if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    HostConfig.HostValue = fbd.SelectedPath;
                //    var result = CommonService.SaveHostConfig(HostConfig);
                //    if (result)
                //    {
                //        HostConfig = CommonService.GetHostConfig(6, HostName, User.ID);
                //    }
                //}


            });

            ListsExportCommand = new DelegateCommand((obj) =>
            {
                //CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "ListsExportCommand", ActionDesc = "导出发料安全库存", UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
                HostConfig.HostValue = "";
                AddAsync(null);
                DateParamter.ParamEndDate = new DateTime (2020, 8, 8);
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
