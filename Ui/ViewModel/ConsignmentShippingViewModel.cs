﻿using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel
{
    public class ConsignmentShippingViewModel : NotificationObject
    {
        private ShippingBillService _shippingService;
        private ConsignmentBillService _consignmentService;
        private CommonService _commonService;
        private string _hostName = Dns.GetHostName();
        private static readonly UserModel user = MemoryCache.Default["user"] as UserModel;
        public ConsignmentShippingViewModel()
        {
            _shippingService = new ShippingBillService();
            _consignmentService = new ConsignmentBillService();
            _commonService = new CommonService();

            ConsignmentBills = new ObservableCollection<ConsignmentBillModel>();
            SelectedConsignmentBillLists = new ObservableCollection<ConsignmentBillModel>();
            ConsignmentBillEntries = new ObservableCollection<ConsignmentBillEntryModel>();
            ShippingBills = new ObservableCollection<ShippingBillModel>();
            ShippingBillEntries = new ObservableCollection<ShippingBillEntryModel>();
            SelectedShippingBillEntry = new ShippingBillEntryModel();
            Filter = new ConsignmentBillParameterModel()
            {
                ParamRestQuatity = 0,
                ParamBeginDate = Convert.ToDateTime(System.DateTime.Now.AddDays(-7).ToShortDateString()),
                ParamEndDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString())
            };
            //初始化表格数据
            HostConfig = GetHostConfig();
            BillTypes = _commonService.GetEnumLists(5).ToList();

            //DataInit();
            GetShippingBills();
            InitQueryConSignmentBill();


            #region 命令属性
            QueryCommand = new DelegateCommand(QuerySignmentBill);

            #region 销售出库、调拨单主表
            ConsignmentBillSelectedAllCommand = new DelegateCommand(SelectedAllConsignmentBill);
            ConsignmentBillUnSelectedAllCommand = new DelegateCommand(UnSelectedAllConsignmentBill);
            ConsignmentBillSelectionChangedCommand = new DelegateCommand(ChangeSelectedConsignmentBill);
            ConsignmentBillRemoveCommand = new DelegateCommand(RemoveConsignmentBill);
            ConsignmentBillSyncCommand = new DelegateCommand(SyncConsignmentBill);
            ConsignmentBillSelectedListsClearCommand = new DelegateCommand(ClearSelectedConsignmentBillLists);
            ConsignmentBillSelectedListsAddCommand = new DelegateCommand(AddConsignmentBill);
            ConsignmentBillMergeCommand = new DelegateCommand(MergeConsignmentBill);
            #endregion

            #region 销售出库、调拨单子表
            ConsignmentBillEntryCopyCommand = new DelegateCommand(CopyConsignmentBillEntry);
            ConsignmentBillEntryDeleteCommand = new DelegateCommand(DeleteConsignmentBillEntry);
            ConsignmentBillEntrySelectionChangedCommand = new DelegateCommand(ChangeSelectedConsignmentBillEntry);
            ConsignmentBillEntryCheckBoxCommand = new DelegateCommand(ClickCheckBoxConsignmentBillEntry);
            ConsignmentBillEntryModifyCommand = new DelegateCommand(ModifyConsignmentBillEntry);
            #endregion

            #region 托运单主表
            ShippingBillSelectionChangedCommand = new DelegateCommand(ShippingBillSelectionChanged);
            ShippingBillModifyCommand = new DelegateCommand(ModifyShippingBill);
            ShippingBillDeleteCommand = new DelegateCommand(DeleteShippingBill);
            ShippingBillExportCommand = new DelegateCommand(ExportShippingData);
            ShippingBillDetailLogShowCommand = new DelegateCommand(ShowShippingBillDetailLog);
            #endregion

            #region 托运单子表
            ShippingBillEntryAddCommand = new DelegateCommand(AddShippingBillEntry);
            ShippingBillEntryDeleteCommand = new DelegateCommand(DeleteShippingBillEntry);
            ShippingBillEntryUpdateCommand = new DelegateCommand(UpdateShippingBillEntry);
            ShippingBillEntrySelectionChangedCommand = new DelegateCommand(ShippingBillEntrySelectionChanged);
            #endregion

            #endregion
        }

        private void DeleteShippingBill(object obj)
        {

            MessageBoxResult result = MessageBox.Show($"将会将合并此单的销售调拨数据\r\n还原到未合并前的状态 \r\n \r\n 物流单号：【{SelectedShippingBill.LogisticsBillNo}】\r\n  总金额：【{SelectedShippingBill.TotalAmount}】"
                + $"\r\n   总重量：【{SelectedShippingBill.TotalQuantity}】"
                + $"\r\n  系统单号：【{SelectedShippingBill.BillNo}】"
                + $"\r\n  托运日期：【{SelectedShippingBill.BillDate}】"
                , "【删除警告！！！】", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                int id = SelectedShippingBill.Id;
                string billNos = _consignmentService.GetConsignmentBillNosByShippingBillId(id);
                if (!string.IsNullOrEmpty(billNos))
                {
                    string lockMessage = _consignmentService.GetConsignmentBillLock(billNos);
                    if (string.IsNullOrEmpty(lockMessage))
                    {
                        string rollbackMessage = _shippingService.DeleteShipingBill(id);
                        if (string.IsNullOrEmpty(rollbackMessage))
                        {
                            ShippingBills.Remove(SelectedShippingBill);
                            ShippingBillEntries.Clear();
                            QuerySignmentBill(null);
                        }
                        else
                        {
                            MessageBox.Show(rollbackMessage);
                        }
                    }
                    else
                    {
                        MessageBox.Show(lockMessage);
                    }
                }
                else
                {
                    MessageBox.Show("合成此托运单的明细单据丢失，请联系管理员");
                }
            }
        }

        private void ChangeSelectedConsignmentBillEntry(object obj)
        {
            if (obj != null)
            {
                SelectedConsignmentBillEntry = (ConsignmentBillEntryModel)obj;
            }
        }
        private void DeleteConsignmentBillEntry(object obj)
        {

            if (SelectedConsignmentBill == null)
                return;

            if (SelectedConsignmentBillEntry == null || SelectedConsignmentBillEntry.IsSystem)
            {
                MessageBox.Show("只能删除手动添加的数据");
                return;
            }

            string ownerName = _consignmentService.GetConsignmentBillLockOwner(SelectedConsignmentBill.BillNo, SelectedConsignmentBill.UserId);
            if (string.IsNullOrEmpty(ownerName))
            {
                // 修改主表界面和后台
                SelectedConsignmentBill.CurrencyQuantity -= SelectedConsignmentBillEntry.ECurrencyQuantity;
                SelectedConsignmentBill.UndoQuantity -= SelectedConsignmentBillEntry.ECurrencyQuantity;
                SelectedConsignmentBill.TotalQuantity -= SelectedConsignmentBillEntry.ECurrencyQuantity;
                _consignmentService.UpdateConsignmentBill(SelectedConsignmentBill);

                // 删除字表数据
                int id = SelectedConsignmentBillEntry.Id;
                ConsignmentBillEntries.Remove(SelectedConsignmentBillEntry);
                _consignmentService.DeleteConsignmentBillEntry(id);

            }
            else
            {
                MessageBox.Show($"该记录正在被【{ownerName}】使用，请选择其他数据");
            }



        }

        private void CopyConsignmentBillEntry(object obj)
        {
            if (SelectedConsignmentBill == null)
                return;

            string ownerName = _consignmentService.GetConsignmentBillLockOwner(SelectedConsignmentBill.BillNo, SelectedConsignmentBill.UserId);
            if (string.IsNullOrEmpty(ownerName))
            {
                if (SelectedConsignmentBillEntry == null) return;


                ConsignmentBillEntryCopyView add = new ConsignmentBillEntryCopyView();
                var cloneData = TransExpV2<ConsignmentBillEntryModel, ConsignmentBillEntryModel>.Trans(SelectedConsignmentBillEntry);
                cloneData.IsChecked = true;
                cloneData.EntryId = ConsignmentBillEntries.Max(m => m.EntryId) + 1;
                cloneData.IsSystem = false;
                cloneData.ECurrencyQuantity = 0;

                (add.DataContext as ConsignmentBillEntryCopyViewModel).WithParam(cloneData, (type, entry) =>
                {
                    add.Close();
                    if (type == 1)
                    {
                        // 修改字表界面和后台
                        int id = _consignmentService.AddConsignmentBillEntry(entry,user.ID);
                        entry.ETotalQuantity = entry.ECurrencyQuantity;
                        entry.EUndoQuantity = entry.ECurrencyQuantity;
                        entry.Id = id;
                        ConsignmentBillEntries.Add(entry);

                        // 修改主表界面和后台
                        SelectedConsignmentBill.CurrencyQuantity += entry.ECurrencyQuantity;
                        SelectedConsignmentBill.UndoQuantity += entry.ECurrencyQuantity;
                        SelectedConsignmentBill.TotalQuantity += entry.ECurrencyQuantity;
                        _consignmentService.UpdateConsignmentBill(SelectedConsignmentBill);
                    }
                });
                add.ShowDialog();

            }
            else
            {
                MessageBox.Show($"该记录正在被【{ownerName}】使用，请选择其他数据");
            }
        }

        public void DataInit()
        {
            Task.Factory.StartNew(() =>
            {
                UIExecute.RunAsync(GetShippingBills);
                UIExecute.RunAsync(InitQueryConSignmentBill);
            });
        }


        private void ShippingBillEntrySelectionChanged(object obj)
        {
            if (obj != null)
            {
                SelectedShippingBillEntry = (ShippingBillEntryModel)obj;
            }
        }

        private void UpdateShippingBillEntry(object obj)
        {
            if (SelectedShippingBillEntry != null)
            {
                    ShippingBillEntryModifyView edit = new ShippingBillEntryModifyView();
                    var quantityBeforeModify = SelectedShippingBillEntry.Quantity;
                    var amountBeforeModify = SelectedShippingBillEntry.ApportionedAmount;
                    var cloneData = TransExpV2<ShippingBillEntryModel, ShippingBillEntryModel>.Trans(SelectedShippingBillEntry);
                    (edit.DataContext as ShippingBillEntryModifyViewModel).WithParam(cloneData, (type, entry) =>
                    {
                        edit.Close();
                        if (type == 1)
                        {   
                            //系统单据只能修改Id
                            if (entry.IsSystem)
                            {
                                _shippingService.UpdateShippingBillEntry2(entry);
                                SelectedShippingBillEntry.GoodsType = entry.GoodsType;
                            }
                            else
                            {
                                float qtyDiff = entry.Quantity - quantityBeforeModify;
                                float amountDiff = entry.ApportionedAmount - amountBeforeModify;
                                // 更新后台
                                _shippingService.UpdateShippingBillEntry3(entry, qtyDiff, amountDiff);

                                // 前端显示
                                SelectedShippingBill.TotalQuantity += qtyDiff;
                                SelectedShippingBill.TotalAmount += amountDiff;
                                switch (entry.GoodsType)
                                {
                                    case 2: SelectedShippingBill.HaoCaiFei += amountDiff; break;
                                    case 3: SelectedShippingBill.YangYouFei += amountDiff; break;
                                    case 4: SelectedShippingBill.SheBeiFei += amountDiff; break;
                                    case 5: SelectedShippingBill.ChengPinTuiHuoFei += amountDiff; break;
                                    case 6: SelectedShippingBill.TuiYuanCaiLiaoFei += amountDiff; break;
                                }
                                GetAllShippingBillEntriesById(entry.MainId);
                            }
                        }
                    });
                    edit.ShowDialog();
            }
        }

        private void DeleteShippingBillEntry(object obj)
        {
            if (SelectedShippingBillEntry != null)
            {
                if (!SelectedShippingBillEntry.IsSystem)
                {
                    SelectedShippingBill.TotalQuantity -= SelectedShippingBillEntry.Quantity;
                    SelectedShippingBill.TotalAmount -= SelectedShippingBillEntry.ApportionedAmount;
                    switch (SelectedShippingBillEntry.GoodsType)
                    {
                        case 2: SelectedShippingBill.HaoCaiFei -= SelectedShippingBillEntry.ApportionedAmount; break;
                        case 3: SelectedShippingBill.YangYouFei -= SelectedShippingBillEntry.ApportionedAmount; break;
                        case 4: SelectedShippingBill.SheBeiFei -= SelectedShippingBillEntry.ApportionedAmount; break;
                        case 5: SelectedShippingBill.ChengPinTuiHuoFei -= SelectedShippingBillEntry.ApportionedAmount; break;
                        case 6: SelectedShippingBill.TuiYuanCaiLiaoFei -= SelectedShippingBillEntry.ApportionedAmount; break;
                    }
                    _shippingService.DeleteShippingBillEntry(SelectedShippingBillEntry);
                    // 重新加载明细 
                    //GetAllShippingBillEntriesById(SelectedShippingBillEntry.MainId);
                    ShippingBillEntries.Remove(SelectedShippingBillEntry);
                }
                else
                {
                    MessageBox.Show("无法删除系统生成的数据");
                }


            }
        }

        private void AddShippingBillEntry(object obj)
        {

                ShippingBillEntryAddView view = new ShippingBillEntryAddView();
                var copyEntry = ShippingBillEntries.LastOrDefault();

                var entry = new ShippingBillEntryModel
                {
                    MainId = copyEntry.MainId,
                    EntryId = copyEntry.EntryId + 1,
                    DeptId = copyEntry.DeptId,
                    DeptName = copyEntry.DeptName,
                    CaseId = 0,
                    IsSystem=false
                };
                (view.DataContext as ShippingBillEntryAddViewModel).WithParam(entry, (type, shippingBillEntry) =>
                {
                    view.Close();

                    if (type == 1)
                    {
                        SelectedShippingBill.TotalQuantity += shippingBillEntry.Quantity;
                        SelectedShippingBill.TotalAmount += shippingBillEntry.ApportionedAmount;
                        switch (shippingBillEntry.GoodsType)
                        {
                            case 2: SelectedShippingBill.HaoCaiFei+= shippingBillEntry.ApportionedAmount; break;
                            case 3: SelectedShippingBill.YangYouFei += shippingBillEntry.ApportionedAmount; break;
                            case 4: SelectedShippingBill.SheBeiFei += shippingBillEntry.ApportionedAmount; break;
                            case 5: SelectedShippingBill.ChengPinTuiHuoFei += shippingBillEntry.ApportionedAmount; break;
                            case 6: SelectedShippingBill.TuiYuanCaiLiaoFei += shippingBillEntry.ApportionedAmount; break;
                        }

                        // 新增条目，修改后台分摊金额
                        _shippingService.AddShipingBillEntry(shippingBillEntry);

                        // 重新加载明细 
                        GetAllShippingBillEntriesById(shippingBillEntry.MainId);
                        
                        //ShippingBillEntries.Add(shippingBillEntry);
                    }
                });
                view.ShowDialog();
            
        }

        private void SelectedAllConsignmentBill(object obj)
        {
            if (ConsignmentBills.Count == 0)
                return;

            if (ConsignmentBills.Where(m => m.SelectedStatus == 0).Count() > 0)
            {
                // 查询有没有被锁定的数据
                string billNos = "'" + string.Join("','", ConsignmentBills.Select(m => m.BillNo)) + "'";
                string lockstring = _consignmentService.GetConsignmentBillLockOwner(user.ID, billNos);
                bool r = ConsignmentBills.Where(m => m.CurrencyQuantity == 0).Count() > 0;
                StringBuilder sb = new StringBuilder();

                if (string.IsNullOrEmpty(lockstring) || r)
                {
                    foreach (var item in ConsignmentBills)
                    {
                        if (SelectedConsignmentBillLists.Where(x => x.InterId == item.InterId).Count() == 0)
                        {
                            SelectedConsignmentBillLists.Add(item);
                            sb.Append(",'" + item.BillNo + "'");
                            SelectedConsignmentSum += item.CurrencyQuantity;
                        }
                    }
                    _consignmentService.AddUserCurrencyOperation(user.ID, sb.ToString().Substring(1));
                    QuerySignmentBill(null);


                }
                else
                {
                    MessageBox.Show(lockstring + "！\r\n 或者已选数据的当前数量不能为0 ，不能批量选择");
                }
            }

        }

        private void UnSelectedAllConsignmentBill(object obj)
        {
            if (ConsignmentBills.Count == 0)
                return;

            if (ConsignmentBills.Where(m => m.SelectedStatus > 0).Count() > 0)// 反选
            {
                var bills = ConsignmentBills.Where(m => m.SelectedStatus > 0);
                if (bills.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in bills)
                    {
                        SelectedConsignmentBillLists.Remove(SelectedConsignmentBillLists.FirstOrDefault(x => x.InterId == item.InterId));
                        SelectedConsignmentSum -= item.CurrencyQuantity;
                        sb.Append(",'" + item.BillNo + "'");
                    }
                    SelectedConsignmentSum = (float)Math.Round((double)SelectedConsignmentSum, 2);
                    _consignmentService.RemoveUserCurrencyOperation(sb.ToString().Substring(1));
                    QuerySignmentBill(null);
                }
            }
        }

        private HostConfigModel GetHostConfig()
        {
            var config = _commonService.GetHostConfig(4, _hostName).FirstOrDefault();
            return config ?? new HostConfigModel();
        }

        private void ExportShippingData(object obj)
        {
            // 导出路径选择
            if (!Directory.Exists(HostConfig.HostValue))
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    HostConfig.HostValue = fbd.SelectedPath;
                    var result = _commonService.SaveHostConfig(new HostConfigModel
                    {
                        TypeId = 4,
                        TypeDesciption = "托运单导出",
                        Host = _hostName,
                        HostValue = fbd.SelectedPath,
                        UserId = user.ID
                    });
                }
            }

            // 导出数据 HostConfig.HostValue
            var lists = _shippingService.GetExprotShippingBill();
            if (lists.Count() > 0)
            {
                new FileHelper().ExportShippingBillToExcel(lists, HostConfig.HostValue);
                MessageBox.Show("导出成功");
            }
        }


        private void ShowShippingBillDetailLog(object obj)
        {
            if (SelectedShippingBill != null)
            {
                ShippingBillDetailLogView view = new ShippingBillDetailLogView(SelectedShippingBill.Id);
                view.Show();
            }
        }

        #region 数据属性

        public List<EnumModel> BillTypes { get; set; }

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

        private float consignmentBillsSumQuantity;

        public float ConsignmentBillsSumQuantity
        {
            get { return consignmentBillsSumQuantity; }
            set
            {
                consignmentBillsSumQuantity = value;
                this.RaisePropertyChanged(nameof(ConsignmentBillsSumQuantity));
            }
        }

        private int consignmentBillsCount;

        public int ConsignmentBillsCount
        {
            get { return consignmentBillsCount; }
            set
            {
                consignmentBillsCount = value;
                this.RaisePropertyChanged(nameof(ConsignmentBillsCount));
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

        private int consignmentCount;

        public int ConsignmentCount
        {
            get { return consignmentCount; }
            set
            {
                consignmentCount = value;
                this.RaisePropertyChanged(nameof(ConsignmentCount));
            }
        }

        private ObservableCollection<ConsignmentBillModel> seletedConsignmentBillLists;

        public ObservableCollection<ConsignmentBillModel> SelectedConsignmentBillLists
        {
            get { return seletedConsignmentBillLists; }
            set
            {
                seletedConsignmentBillLists = value;
                this.RaisePropertyChanged(nameof(SelectedConsignmentBillLists));
            }
        }

        private float selectedConsignmentSum;

        public float SelectedConsignmentSum
        {
            get { return selectedConsignmentSum; }
            set
            {
                selectedConsignmentSum = value;
                this.RaisePropertyChanged(nameof(SelectedConsignmentSum));
            }
        }



        #region 明细
        private ObservableCollection<ConsignmentBillEntryModel> consignmentBillEntries;

        public ObservableCollection<ConsignmentBillEntryModel> ConsignmentBillEntries
        {
            get { return consignmentBillEntries; }
            set
            {
                consignmentBillEntries = value;
                this.RaisePropertyChanged(nameof(ConsignmentBillEntries));
            }
        }

        private ConsignmentBillEntryModel selectedConsignmentBillEntry;

        public ConsignmentBillEntryModel SelectedConsignmentBillEntry
        {
            get { return selectedConsignmentBillEntry; }
            set
            {
                selectedConsignmentBillEntry = value;
                this.RaisePropertyChanged(nameof(SelectedConsignmentBillEntry));
            }
        }

        private ObservableCollection<ShippingBillEntryModel> shippingBillEntries;

        public ObservableCollection<ShippingBillEntryModel> ShippingBillEntries
        {
            get { return shippingBillEntries; }
            set
            {
                shippingBillEntries = value;
                this.RaisePropertyChanged(nameof(ConsignmentBillEntries));
            }
        }

        private ShippingBillEntryModel selectedShippingBillEntry;

        public ShippingBillEntryModel SelectedShippingBillEntry
        {
            get { return selectedShippingBillEntry; }
            set
            {
                selectedShippingBillEntry = value;
                this.RaisePropertyChanged(nameof(SelectedShippingBillEntry));
            }
        }

        #endregion

        private HostConfigModel hostConfig;

        public HostConfigModel HostConfig
        {
            get { return hostConfig; }
            set
            {
                hostConfig = value;
                this.RaisePropertyChanged(nameof(HostConfig));
            }
        }


        #endregion

        #region 命令属性
        public DelegateCommand ConsignmentBillEntryModifyCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand ConsignmentBillMergeCommand { get; set; }
        public DelegateCommand ConsignmentBillSelectedListsAddCommand { get; set; }
        public DelegateCommand ConsignmentBillRemoveCommand { get; set; }

        public DelegateCommand ConsignmentBillSelectionChangedCommand { get; set; }
        public DelegateCommand ConsignmentBillEntryCheckBoxCommand { get; set; }

        public DelegateCommand ShippingBillModifyCommand { get; set; }
        public DelegateCommand ShippingBillDeleteCommand { get; set; }
        public DelegateCommand ShippingBillSelectionChangedCommand { get; set; }
        public DelegateCommand ConsignmentBillSelectedListsClearCommand { get; set; }
        public DelegateCommand ConsignmentBillSyncCommand { get; set; }
        public DelegateCommand ShippingBillDetailLogShowCommand { get; set; }
        public DelegateCommand ShippingBillExportCommand { get; set; }
        public DelegateCommand ConsignmentBillSelectedAllCommand { get; set; }
        public DelegateCommand ConsignmentBillUnSelectedAllCommand { get; set; }
        public DelegateCommand ShippingBillEntryAddCommand { get; set; }
        public DelegateCommand ShippingBillEntryDeleteCommand { get; set; }
        public DelegateCommand ShippingBillEntryUpdateCommand { get; set; }
        public DelegateCommand ShippingBillEntrySelectionChangedCommand { get; set; }
        public DelegateCommand ConsignmentBillEntryCopyCommand { get; set; }
        public DelegateCommand ConsignmentBillEntryDeleteCommand { get; set; }
        public DelegateCommand ConsignmentBillEntrySelectionChangedCommand { get; set; }



        #endregion

        #region 命令委托

        public void GetConsignmentBills()
        {
            ConsignmentBills.Clear();
            _consignmentService.GetAllConsignmentBills(0).ToList().ForEach(x => ConsignmentBills.Add(x));
            ConsignmentCount = ConsignmentBills.Count();
        }

        public void GetShippingBills()
        {
            ShippingBills.Clear();
            _shippingService.GetAllShippingBills().ToList().ForEach(x => ShippingBills.Add(x));
        }

        private void GetAllShippingBillEntriesById(int id)
        {
            ShippingBillEntries.Clear();
            _shippingService.GetAllShippingBillEntryById(id).ToList().ForEach(x => ShippingBillEntries.Add(x));
        }

        private void ModifyConsignmentBillEntry(object obj)
        {
            if (SelectedConsignmentBill == null)
                return;
            if (SelectedConsignmentBillEntry == null) return;

            if (!SelectedConsignmentBillEntry.IsSystem)
            {
                MessageBox.Show("手动新增的数据无法修改，请先【删除】然后【新增】");
                return;
            }

            string ownerName = _consignmentService.GetConsignmentBillLockOwner(SelectedConsignmentBill.BillNo, SelectedConsignmentBill.UserId);
            if (string.IsNullOrEmpty(ownerName))
            {


                if (SelectedConsignmentBillEntry.IsChecked)
                {
                    float currencyQuatityBeforeModify = selectedConsignmentBillEntry.ECurrencyQuantity;
                    var cloneData = TransExpV2<ConsignmentBillEntryModel, ConsignmentBillEntryModel>.Trans(SelectedConsignmentBillEntry);
                    ConsignmentBillEntryModifyView edit = new ConsignmentBillEntryModifyView();

                    (edit.DataContext as ConsignmentBillEntryModifyViewModel).WithParam(cloneData, (type, entry) =>
                    {
                        edit.Close();
                        if (type == 1)
                        {
                            if (currencyQuatityBeforeModify != entry.ECurrencyQuantity)
                            {
                                SelectedConsignmentBillEntry.ECurrencyQuantity = entry.ECurrencyQuantity;
                                _consignmentService.UpdateConsignmentBillEntry(entry);
                                // 修改主表界面和后台
                                SelectedConsignmentBill.CurrencyQuantity += entry.ECurrencyQuantity - currencyQuatityBeforeModify;
                                _consignmentService.UpdateConsignmentBill(SelectedConsignmentBill);
                            }
                        }
                    });
                    edit.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show($"该记录正在被【{ownerName}】使用，请选择其他数据");
            }

        }

        private void InitQueryConSignmentBill()
        {
            QuerySignmentBill(null);

            SelectedConsignmentBillLists.Clear();
            _consignmentService.GetUserSelectedConsignmentBill(user.ID).ToList().ForEach(x => SelectedConsignmentBillLists.Add(x));

            //ConsignmentBills.Clear();
            //_consignmentService.GetAllConsignmentBills(user.ID).ToList().ForEach(x =>
            //{
            //    ConsignmentBills.Add(x);
            //    if (x.SelectedStatus > 0)
            //    {
            //        SelectedConsignmentBillLists.Add(x);
            //    }
            //});
            //ConsignmentCount = ConsignmentBills.Count();
            //SelectedConsignmentSum = SelectedConsignmentBillLists.Sum(x => x.CurrencyQuatity);
            //SelectedConsignmentBillEntry = null;
        }

        private void QuerySignmentBill(object obj)
        {
            ConsignmentBills.Clear();
            var para = Filter;//(ConsignmentBillParameterModel)obj;

            List<string> filters = new List<string>();


            if (!string.IsNullOrEmpty(para.ParamBillNo))
            {
                filters.Add($" and a.BillNo like '%{para.ParamBillNo}%' ");
            }

            if (!string.IsNullOrEmpty(para.ParamDeptName))
            {
                string deptName = para.ParamDeptName.Replace("，", ",");
                if (deptName.Contains(","))
                {
                    string orfield = string.Empty;
                    foreach (var item in deptName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        orfield += $"or DeptName like '%{item}%' ";
                    }
                    orfield = orfield.Length > 2 ? orfield.Substring(2) : " 1 = 1 ";
                    filters.Add($" and ( {orfield}  )");
                }
                else
                {
                    filters.Add($" and DeptName like '%{deptName}%' ");
                }
            }

            if (!string.IsNullOrEmpty(para.ParamCustName))
            {
                string custName = para.ParamCustName.Replace("，", ",");
                if (custName.Contains(","))
                {
                    string orfield = string.Empty;
                    foreach (var item in custName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        orfield += $"or CustName like '%{item}%'  ";
                    }
                    orfield = orfield.Length > 2 ? orfield.Substring(2) : " 1 = 1 ";
                    filters.Add($" and ( {orfield}  )");
                }
                else
                {
                    filters.Add($" and CustName like '%{custName}%' ");
                }
            }

            if (para.IsSelected)
            {
                filters.Add($" and SelectedStatus > 0  ");
            }

            string filter = $" and BillDate >= '{para.ParamBeginDate}' and BillDate <= '{para.ParamEndDate}'  and UndoQuantity>{Filter.ParamRestQuatity} " + string.Join(" ", filters);
            _consignmentService.GetAllConsignmentBills(user.ID, filter).ToList().ForEach(x =>
            {
                ConsignmentBills.Add(x);
            });
            ConsignmentBillsCount = ConsignmentBills.Count();
            ConsignmentBillsSumQuantity = ConsignmentBills.Sum(m=>m.CurrencyQuantity);
            SelectedConsignmentBillEntry = null;
            SelectedConsignmentBill = null;
            ConsignmentBillEntries.Clear();
        }

        private void MergeConsignmentBill(object obj)
        {
            if (SelectedConsignmentBillLists.Count() > 0)
            {
                string fInterIds = string.Join(",", SelectedConsignmentBillLists.Select(m => m.InterId));
                string fbillNos = "'" + string.Join("','", SelectedConsignmentBillLists.Select(m => m.BillNo)) + "'";
                string rollbackMsg=_consignmentService.MergeConsignmentBill(user.ID, fInterIds, fbillNos);
                if (string.IsNullOrEmpty(rollbackMsg))
                {
                    SelectedConsignmentBillLists.Clear();
                    SelectedConsignmentSum = 0;
                    QuerySignmentBill(null);
                    GetShippingBills();
                }
                else
                {
                    MessageBox.Show(rollbackMsg);
                }
            }
        }

        private void AddConsignmentBill(object obj)
        {
            // 直接用 SelectedConsignmentBill
            if (SelectedConsignmentBill != null && SelectedConsignmentBill.SelectedStatus == 0)
            {
                // 后台查询该记录是否被别人操作了
                string ownerName = _consignmentService.GetConsignmentBillLockOwner(SelectedConsignmentBill.BillNo, SelectedConsignmentBill.UserId);
                if (string.IsNullOrEmpty(ownerName))
                {
                    // 修改选择状态
                    if (SelectedConsignmentBill.UndoQuantity == selectedConsignmentBill.CurrencyQuantity)
                        SelectedConsignmentBill.SelectedStatus = 2;
                    else
                        SelectedConsignmentBill.SelectedStatus = 1;

                    // 添加已选数据，求和
                    SelectedConsignmentBillLists.Insert(0, SelectedConsignmentBill);
                    SelectedConsignmentSum += SelectedConsignmentBill.CurrencyQuantity;
                    SelectedConsignmentBillEntry = null;

                    // 同步数据到数据库
                    _consignmentService.AddConsignmentBill(SelectedConsignmentBill);
                }
                else
                {
                    MessageBox.Show($"【{ownerName}】，请选择其他数据");
                }



            }

        }

        private void RemoveConsignmentBill(object obj)
        {
            if (SelectedConsignmentBill.SelectedStatus > 0)
            {
                //移除数据求和
                SelectedConsignmentBillLists.Remove(SelectedConsignmentBillLists.FirstOrDefault(x => x.InterId == SelectedConsignmentBill.InterId));
                SelectedConsignmentSum -= SelectedConsignmentBill.CurrencyQuantity;

                // 修改选择状态
                SelectedConsignmentBill.SelectedStatus = 0;

                // 同步数据到数据库
                _consignmentService.DeleteConsignmentBill(SelectedConsignmentBill);
            }
        }

        private void ChangeSelectedConsignmentBill(object obj)
        {
            if (obj != null)
            {
                SelectedConsignmentBill = (ConsignmentBillModel)obj;
                ConsignmentBillEntries.Clear();

                _consignmentService.GetAllConsignmentBillEntryById(SelectedConsignmentBill.InterId).ToList().ForEach(x => ConsignmentBillEntries.Add(x));
            }
        }

        private void ClickCheckBoxConsignmentBillEntry(object obj)
        {
            string ownerName = _consignmentService.GetConsignmentBillLockOwner(SelectedConsignmentBill.BillNo, SelectedConsignmentBill.UserId);
            if (string.IsNullOrEmpty(ownerName))
            {
                if (SelectedConsignmentBillEntry.IsChecked)   // UnCheck -> Checked
                    selectedConsignmentBill.CurrencyQuantity += SelectedConsignmentBillEntry.ECurrencyQuantity;
                else // Checked -> UnCheck
                    selectedConsignmentBill.CurrencyQuantity -= SelectedConsignmentBillEntry.ECurrencyQuantity;

                _consignmentService.UpdateConsignmentBillEntry(SelectedConsignmentBillEntry);
                _consignmentService.UpdateConsignmentBill(SelectedConsignmentBill);
            }
            else
            {
                MessageBox.Show($"该记录正在被【{ownerName}】使用，请选择其他数据");
            }
        }

        private void ShippingBillSelectionChanged(object obj)
        {
            if (obj != null)
            {
                SelectedShippingBill = (ShippingBillModel)obj;
                ShippingBillEntries.Clear();

                _shippingService.GetAllShippingBillEntryById(SelectedShippingBill.Id).ToList().ForEach(x => ShippingBillEntries.Add(x));
            }
        }

        private void ModifyShippingBill(object obj)
        {
            if (SelectedShippingBill == null) return;

            ShippingBillModifyView edit = new ShippingBillModifyView();

            var totalAmountBeforeModify = SelectedShippingBill.TotalAmount;
            var cloneData = TransExpV2<ShippingBillModel, ShippingBillModel>.Trans(SelectedShippingBill);
            (edit.DataContext as ShippingBillModifyViewModel).WithParam(cloneData, (type, shippingBill) =>
            {
                edit.Close();

                if (type == 1)
                {
                    // 重新加载主表
                    _shippingService.UpdateShipingBill(shippingBill);

                    SelectedShippingBill.BaoXianFei = shippingBill.BaoXianFei;
                    SelectedShippingBill.BillDate = shippingBill.BillDate;
                    SelectedShippingBill.ChaiLvFei = shippingBill.ChaiLvFei;
                    SelectedShippingBill.Demander = shippingBill.Demander;
                    SelectedShippingBill.GuanShuiFei = shippingBill.GuanShuiFei;
                    SelectedShippingBill.GuoLuFei = shippingBill.GuoLuFei;
                    SelectedShippingBill.LogisticsBillNo = shippingBill.LogisticsBillNo;
                    SelectedShippingBill.LogisticsCompanyName = shippingBill.LogisticsCompanyName;
                    SelectedShippingBill.LogisticsType = shippingBill.LogisticsType;
                    SelectedShippingBill.Note = shippingBill.Note;
                    SelectedShippingBill.OtherCosts = shippingBill.OtherCosts;
                    SelectedShippingBill.PaiSongFei = shippingBill.PaiSongFei;
                    SelectedShippingBill.QingGuanFei = shippingBill.QingGuanFei;
                    SelectedShippingBill.TiHuoFei = shippingBill.TiHuoFei;
                    SelectedShippingBill.TotalAmount = shippingBill.TotalAmount;
                    SelectedShippingBill.WeiXianPinFei = shippingBill.WeiXianPinFei;
                    SelectedShippingBill.WeiXiuFei = shippingBill.WeiXiuFei;
                    SelectedShippingBill.YouFei = shippingBill.YouFei;
                    SelectedShippingBill.YunShuFei = shippingBill.YunShuFei;

                    // 重新加载明细
                    if (totalAmountBeforeModify != shippingBill.TotalAmount)
                    {
                        _shippingService.UpdateShipingBillEntry(shippingBill);
                        GetAllShippingBillEntriesById(shippingBill.Id);
                    }
                }
            });
            edit.ShowDialog();

        }

        private void ClearSelectedConsignmentBillLists(object obj)
        {
            if (SelectedConsignmentBillLists.Count() > 0)
            {
                string ids = "'" + string.Join("','", SelectedConsignmentBillLists.Select(x => x.BillNo)) + "'";
                SelectedConsignmentBillLists.Clear();
                SelectedConsignmentSum = 0;
                ConsignmentBillEntries.Clear();
                _consignmentService.ClearSelectedConsignmentBills(ids);
                QuerySignmentBill(null);
            }
        }

        private void SyncConsignmentBill(object obj)
        {

            MessageBoxResult result = MessageBox.Show($" 确认重新获取满足下列条件的最新单据数据：\r\n \r\n 1.时间段：【{Filter.ParamBeginDate.ToString("yyyy-MM-dd")} 至 {Filter.ParamEndDate.ToString("yyyy-MM-dd")} (包括边界日期) 】 \r\n 2.【单据号未增加明细】" +
                $"\r\n 3.【单据号未被选定】\r\n 4.【单据号从未合并托运单】", "【删除警告！！！】", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                _consignmentService.SyncConsignmentBill(Filter.ParamBeginDate,Filter.ParamEndDate);
                QuerySignmentBill(null);
                MessageBox.Show("已获取最新数据");
            }
        }
        #endregion


    }
}