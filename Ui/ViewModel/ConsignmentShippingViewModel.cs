using Common;
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
using System.Windows.Forms;
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
                ParamDate = Convert.ToDateTime(System.DateTime.Now.AddMonths(-1).ToShortDateString()),
            };

            //命令
            ModifyConsignmentBillEntryCommand = new DelegateCommand(ModifyConsignmentBillEntry);
            ConsignmentBillMergeCommand = new DelegateCommand(MergeConsignmentBill);
            AddConsignmentBillCommand = new DelegateCommand(AddConsignmentBill);
            RemoveConsignmentBillCommand = new DelegateCommand(RemoveConsignmentBill);
            QueryCommand = new DelegateCommand(QuerySignmentBill);
            ConsignmentBillSelectionChangedCommand = new DelegateCommand(ConsignmentBillSelectionChanged);
            ConsignmentBillEntryCheckBoxCommand = new DelegateCommand(ConsignmentBillEntryCheckBoxClick);
            ShippingBillSelectionChangedCommand = new DelegateCommand(ShippingBillSelectionChanged);
            ModifyShippingBillCommand = new DelegateCommand(ModifyShippingBill);
            ClearSelectedConsignmentBillListsCommand = new DelegateCommand(ClearSelectedConsignmentBillLists);
            SyncConsignmentBillCommand = new DelegateCommand(SyncConsignmentBill);
            ShowShippingBillDetailLogCommand = new DelegateCommand(ShowShippingBillDetailLog);
            ExportShippingDataCommand = new DelegateCommand(ExportShippingData);
            ConsignmentBillSelectedAllCommand = new DelegateCommand(SelectedAllConsignmentBill);
            ConsignmentBillUnSelectedAllCommand = new DelegateCommand(UnSelectedAllConsignmentBill);
            ShippingBillEntryAddCommand = new DelegateCommand(AddShippingBillEntry);
            ShippingBillEntryDeleteCommand = new DelegateCommand(DeleteShippingBillEntry);
            ShippingBillEntryUpdateCommand = new DelegateCommand(UpdateShippingBillEntry);
            ShippingBillEntrySelectionChangedCommand = new DelegateCommand(ShippingBillEntrySelectionChanged);



            //初始化表格数据
           
            HostConfig = GetHostConfig();
        
           // GetShippingBills();
           // InitQueryConSignmentBill();
        }

        public void Init()
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
                if (string.IsNullOrEmpty(SelectedShippingBillEntry.CustName))
                {
                    ShippingBillEntryModifyView edit = new ShippingBillEntryModifyView();

                    var quantityBeforeModify = SelectedShippingBillEntry.Quantity;
                    var cloneData = TransExpV2<ShippingBillEntryModel, ShippingBillEntryModel>.Trans(SelectedShippingBillEntry);
                    (edit.DataContext as ShippingBillEntryModifyViewModel).WithParam(cloneData, (type, entry) =>
                    {
                        edit.Close();
                        if (type == 1)
                        {
                            SelectedShippingBillEntry.Quantity = entry.Quantity;
                            // 简单修改
                            if (quantityBeforeModify == entry.Quantity)
                            {
                                _shippingService.UpdateShippingBillEntry2(entry);
                            }
                            else  // 重新分摊数量
                            {
                                float diff = entry.Quantity - quantityBeforeModify;
                                SelectedShippingBill.TotalQuantity += diff;
                                _shippingService.UpdateShippingBillEntry3(entry, diff);
                            }
                            GetAllShippingBillEntriesById(entry.MainId);
                        }
                    });
                    edit.ShowDialog();
                }
                else
                {
                    MessageBox.Show("无法修改系统生成的数据");
                }
            }
        }

        private void DeleteShippingBillEntry(object obj)
        {
            if (SelectedShippingBillEntry != null)
            {
                if (string.IsNullOrEmpty(SelectedShippingBillEntry.CustName))
                {
                    SelectedShippingBill.TotalQuantity -= SelectedShippingBillEntry.Quantity;
                    _shippingService.DeleteShippingBillEntry(SelectedShippingBillEntry);
                    // 重新加载明细 
                    GetAllShippingBillEntriesById(SelectedShippingBillEntry.MainId);
                }
                else
                {
                    MessageBox.Show("无法删除系统生成的数据");
                }


            }
        }

        private void AddShippingBillEntry(object obj)
        {
            if (ShippingBillEntries.Count() > 0)
            {
                ShippingBillEntryAddView view = new ShippingBillEntryAddView();
                var copyEntry = ShippingBillEntries.LastOrDefault();
                var m = copyEntry.TotalQuantity;

                var entry = new ShippingBillEntryModel
                {
                    MainId = copyEntry.MainId,
                    EntryId = copyEntry.EntryId + 1,
                    GoodsType = 3,
                    DeptId = copyEntry.DeptId,
                    DeptName = copyEntry.DeptName,
                    CaseId = 0,
                    CaseName = "样油",
                    TotalAmount = copyEntry.TotalAmount
                };
                (view.DataContext as ShippingBillEntryAddViewModel).WithParam(entry, (type, shippingBillEntry) =>
                {
                    view.Close();

                    if (type == 1)
                    {
                        shippingBillEntry.TotalQuantity = shippingBillEntry.Quantity + m;
                        SelectedShippingBill.TotalQuantity = shippingBillEntry.Quantity + m;
                        // 新增条目，修改后台分摊金额
                        _shippingService.AddShipingBillEntry(shippingBillEntry);

                        // 重新加载明细 
                        GetAllShippingBillEntriesById(shippingBillEntry.MainId);
                    }
                });
                view.ShowDialog();
            }
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
                bool r = ConsignmentBills.Where(m => m.CurrencyQuatity == 0).Count() > 0;
                StringBuilder sb = new StringBuilder();
        
                if (string.IsNullOrEmpty(lockstring) || r)
                {
                    foreach (var item in ConsignmentBills)
                    {
                        if (SelectedConsignmentBillLists.Where(x => x.InterId == item.InterId).Count() == 0)
                        {
                            SelectedConsignmentBillLists.Add(item);
                            sb.Append(",'" + item.BillNo + "'");
                            SelectedConsignmentSum += item.CurrencyQuatity;
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
                        SelectedConsignmentSum -= item.CurrencyQuatity;
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
        public DelegateCommand ModifyConsignmentBillEntryCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand ConsignmentBillMergeCommand { get; set; }
        public DelegateCommand AddConsignmentBillCommand { get; set; }
        public DelegateCommand RemoveConsignmentBillCommand { get; set; }

        public DelegateCommand ConsignmentBillSelectionChangedCommand { get; set; }
        public DelegateCommand ConsignmentBillEntryCheckBoxCommand { get; set; }

        public DelegateCommand ModifyShippingBillCommand { get; set; }
        public DelegateCommand ShippingBillSelectionChangedCommand { get; set; }
        public DelegateCommand ClearSelectedConsignmentBillListsCommand { get; set; }
        public DelegateCommand SyncConsignmentBillCommand { get; set; }
        public DelegateCommand ShowShippingBillDetailLogCommand { get; set; }
        public DelegateCommand ExportShippingDataCommand { get; set; }
        public DelegateCommand ConsignmentBillSelectedAllCommand { get; set; }
        public DelegateCommand ConsignmentBillUnSelectedAllCommand { get; set; }
        public DelegateCommand ShippingBillEntryAddCommand { get; set; }
        public DelegateCommand ShippingBillEntryDeleteCommand { get; set; }
        public DelegateCommand ShippingBillEntryUpdateCommand { get; set; }
        public DelegateCommand ShippingBillEntrySelectionChangedCommand { get; set; }

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
            if (SelectedConsignmentBill==null)
                return;

            string ownerName = _consignmentService.GetConsignmentBillLockOwner(SelectedConsignmentBill.BillNo, SelectedConsignmentBill.UserId);
            if (string.IsNullOrEmpty(ownerName))
            {
                if (SelectedConsignmentBillEntry == null) return;

                if (SelectedConsignmentBillEntry.IsChecked)
                {
                    float currencyQuatityBeforeModify = selectedConsignmentBillEntry.ECurrencyQuatity;
                    var cloneData = TransExpV2<ConsignmentBillEntryModel, ConsignmentBillEntryModel>.Trans(SelectedConsignmentBillEntry);
                    ConsignmentBillEntryModifyView edit = new ConsignmentBillEntryModifyView();

                    (edit.DataContext as ConsignmentBillEntryModifyViewModel).WithParam(cloneData, (type, entry) =>
                    {
                        edit.Close();
                        if (type == 1)
                        {
                            if (currencyQuatityBeforeModify != entry.ECurrencyQuatity)
                            {
                                SelectedConsignmentBillEntry.ECurrencyQuatity = entry.ECurrencyQuatity;
                                _consignmentService.UpdateConsignmentBillEntry(entry);
                                // 修改主表界面和后台
                                SelectedConsignmentBill.CurrencyQuatity += entry.ECurrencyQuatity - currencyQuatityBeforeModify;
                                _consignmentService.UpdateConsignmentBill(SelectedConsignmentBill);
                            }
                            //GetConsignmentBills();
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
            ConsignmentBills.Clear();
            //string filter = $" and BillDate >= '{Filter.ParamDate}' and UndoQuatity>{Filter.ParamRestQuatity}  ";
            _consignmentService.GetAllConsignmentBills(user.ID).ToList().ForEach(x =>
            {
                ConsignmentBills.Add(x);
                if (x.SelectedStatus > 0)
                {
                    SelectedConsignmentBillLists.Add(x);
                }
            });
            ConsignmentCount = ConsignmentBills.Count();
            SelectedConsignmentSum = SelectedConsignmentBillLists.Sum(x => x.CurrencyQuatity);
            SelectedConsignmentBillEntry = null;
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

            string filter = $" and BillDate >= '{para.ParamDate}'  and UndoQuatity>{Filter.ParamRestQuatity} " + string.Join(" ", filters);
            _consignmentService.GetAllConsignmentBills(user.ID, filter).ToList().ForEach(x =>
            {
                ConsignmentBills.Add(x);
            });
            ConsignmentCount = ConsignmentBills.Count();

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
                _consignmentService.MergeConsignmentBill(user.ID, fInterIds, fbillNos);

                SelectedConsignmentBillLists.Clear();
                SelectedConsignmentSum = 0;
                QuerySignmentBill(null);
                //InitQueryConSignmentBill(null);
                GetShippingBills();
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
                    if (SelectedConsignmentBill.UndoQuatity == selectedConsignmentBill.CurrencyQuatity)
                        SelectedConsignmentBill.SelectedStatus = 2;
                    else
                        SelectedConsignmentBill.SelectedStatus = 1;

                    // 添加已选数据，求和
                    SelectedConsignmentBillLists.Add(SelectedConsignmentBill);
                    SelectedConsignmentSum += SelectedConsignmentBill.CurrencyQuatity;
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
                SelectedConsignmentSum -= SelectedConsignmentBill.CurrencyQuatity;

                // 修改选择状态
                SelectedConsignmentBill.SelectedStatus = 0;

                // 同步数据到数据库
                _consignmentService.DeleteConsignmentBill(SelectedConsignmentBill);
            }
        }

        private void ConsignmentBillSelectionChanged(object obj)
        {
            if (obj != null)
            {
                SelectedConsignmentBill = (ConsignmentBillModel)obj;
                ConsignmentBillEntries.Clear();

                _consignmentService.GetAllConsignmentBillEntryById(SelectedConsignmentBill.InterId).ToList().ForEach(x => ConsignmentBillEntries.Add(x));
            }
        }

        private void ConsignmentBillEntryCheckBoxClick(object obj)
        {
            string ownerName = _consignmentService.GetConsignmentBillLockOwner(SelectedConsignmentBill.BillNo, SelectedConsignmentBill.UserId);
            if (string.IsNullOrEmpty(ownerName))
            {
                if (SelectedConsignmentBillEntry.IsChecked)   // UnCheck -> Checked
                    selectedConsignmentBill.CurrencyQuatity += SelectedConsignmentBillEntry.ECurrencyQuatity;
                else // Checked -> UnCheck
                    selectedConsignmentBill.CurrencyQuatity -= SelectedConsignmentBillEntry.ECurrencyQuatity;

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
                    // 修改参数
                    //int index = ShippingBills.IndexOf(SelectedShippingBill);
                    //ShippingBills.Remove(SelectedShippingBill);
                    //ShippingBills.Insert(index, entry);
                    //SelectedShippingBill = entry;
                    //SelectedShippingBill = TransExpV2<ShippingBillModel, ShippingBillModel>.Trans(entry); 
                    SelectedShippingBill.BaoXianFei = shippingBill.BaoXianFei;
                    SelectedShippingBill.BillDate = shippingBill.BillDate;
                    SelectedShippingBill.ChaiLvFei = shippingBill.ChaiLvFei;
                    SelectedShippingBill.Demander = shippingBill.Demander;
                    SelectedShippingBill.GoodsType = shippingBill.GoodsType;
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
            _consignmentService.SyncConsignmentBill();
            QuerySignmentBill(null);
            MessageBox.Show("已获取最新数据");
        }
        #endregion


    }
}
