using Common;
using ImportVerificationModel;
using K3ApiModel;
using K3ApiModel.PurchaseRequisition;
using K3ApiModel.Request;
using Model;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel.IndexPage
{
    public class MaterialPlanInventoryViewModel : BaseViewModel
    {
        //private string fBillNo; // 插入后返回的单号，用来审核
        private MaterialPlanInventoryService _materialPlanInventoryService;
        private IList<PurchaseRequisitionFKModel> _purchaseRequisitionICItemLists;
        private IList<BaseNumberNameModel> _employeeLists;
        private IList<BaseNumberNameModel> _measureUnitLists;
        public MaterialPlanInventoryViewModel() //: base("Purchase_Requisition")
        {
            _materialPlanInventoryService = new MaterialPlanInventoryService();
            // _commonService = new CommonService();
            InitData();
            InitCommand();
        }

        private void InitData()
        {
            Filter = new MaterialPlanInventoryParameterModel
            {
                ParamBeginDate = DateTime.Now.AddDays(-7),
                ParamEndDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString()),
            };

            Task.Factory.StartNew(() =>
            {
                _purchaseRequisitionICItemLists = K3ApiFKService.GetPurchaseRequisitionICItem();
                _measureUnitLists = K3ApiFKService.GetMeasureUnit();
                _employeeLists = K3ApiFKService.GetK3Employee();

                if (_materialPlanInventoryService.DeleteMaterialPlanInventory())
                    MaterialPlanInventoryLists.Clear();

                MaterialPlanSeOrderFullLists = _materialPlanInventoryService.GetMaterialPlanSeorderFullLists(Filter.ParamBeginDate, Filter.ParamEndDate);

                UIExecute.RunAsync(() =>
                {
                    MaterialPlanSeOrderFullLists.GroupBy(x => new { x.FBillNo, x.FInterID, x.FDate, x.FCustName, x.FEmpName, x.FDeptName })
                    .Select(x => new MaterialPlanSeOrderShowModel { FCustName = x.Key.FCustName, FEmpName = x.Key.FEmpName, FDeptName = x.Key.FDeptName, FDate = x.Key.FDate, FInterID = x.Key.FInterID, FBillNo = x.Key.FBillNo })
                    .OrderByDescending(x => x.FDate).ThenBy(x => x.FBillNo)
                    .ToList().ForEach(m => MaterialPlanSeorderLists.Add(m));
                });
            });
        }

        public K3ApiFKService K3ApiFKService { get; set; } = new K3ApiFKService();

        public List<MaterialPlanSeOrderFullModel> MaterialPlanSeOrderFullLists { get; set; } = new List<MaterialPlanSeOrderFullModel>();

        private ObservableCollection<MaterialPlanSeOrderShowModel> materialPlanSeorderLists = new ObservableCollection<MaterialPlanSeOrderShowModel>();

        public ObservableCollection<MaterialPlanSeOrderShowModel> MaterialPlanSeorderLists
        {
            get { return materialPlanSeorderLists; }
            set
            {
                materialPlanSeorderLists = value;
                this.RaisePropertyChanged(nameof(MaterialPlanSeorderLists));
            }
        }

        private ObservableCollection<MaterialPlanSeOrderEntryShowModel> materialPlanSeOrderEntryLists = new ObservableCollection<MaterialPlanSeOrderEntryShowModel>();

        public ObservableCollection<MaterialPlanSeOrderEntryShowModel> MaterialPlanSeOrderEntryLists
        {
            get { return materialPlanSeOrderEntryLists; }
            set
            {
                materialPlanSeOrderEntryLists = value;
                this.RaisePropertyChanged(nameof(MaterialPlanSeOrderEntryLists));
            }
        }





        private MaterialPlanInventoryParameterModel filter;

        public MaterialPlanInventoryParameterModel Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                this.RaisePropertyChanged(nameof(Filter));
            }
        }

        private ObservableCollection<MaterialDemandModel> materialDemandLists = new ObservableCollection<MaterialDemandModel>();

        public ObservableCollection<MaterialDemandModel> MaterialDemandLists
        {
            get { return materialDemandLists; }
            set
            {
                materialDemandLists = value;
                this.RaisePropertyChanged(nameof(MaterialDemandLists));
            }
        }

        private MaterialPlanSeOrderShowModel materialPlanSeorderSelectedItem = new MaterialPlanSeOrderShowModel();

        public MaterialPlanSeOrderShowModel MaterialPlanSeorderSelectedItem
        {
            get { return materialPlanSeorderSelectedItem; }
            set
            {
                materialPlanSeorderSelectedItem = value;
                this.RaisePropertyChanged(nameof(MaterialPlanSeorderSelectedItem));
            }
        }


        private ObservableCollection<MaterialBomModel> materialBomLists = new ObservableCollection<MaterialBomModel>();

        public ObservableCollection<MaterialBomModel> MaterialBomLists
        {
            get { return materialBomLists; }
            set
            {
                materialBomLists = value;
                this.RaisePropertyChanged(nameof(MaterialBomLists));
            }
        }

        private string importFileFullName = "";

        public string ImportFileFullName
        {
            get { return importFileFullName; }
            set
            {
                importFileFullName = value;
                this.RaisePropertyChanged(nameof(ImportFileFullName));
            }
        }

        private string importFileFullName1;

        public string ImportFileFullName1
        {
            get { return importFileFullName1; }
            set
            {
                importFileFullName1 = value;
                this.RaisePropertyChanged(nameof(ImportFileFullName1));
            }
        }

        private K3ApiInsertDataResponseModel k3InsertResponseData = new K3ApiInsertDataResponseModel();

        public K3ApiInsertDataResponseModel K3InsertResponseData
        {
            get { return k3InsertResponseData; }
            set
            {
                k3InsertResponseData = value;
                this.RaisePropertyChanged(nameof(K3InsertResponseData));
            }
        }


        private ObservableCollection<PurchaseRequisitionImportVerificationModel> checkedPurchaseRequisitionMaterialLists = new ObservableCollection<PurchaseRequisitionImportVerificationModel>();

        public ObservableCollection<PurchaseRequisitionImportVerificationModel> CheckedPurchaseRequisitionMaterialLists
        {
            get { return checkedPurchaseRequisitionMaterialLists; }
            set
            {
                checkedPurchaseRequisitionMaterialLists = value;
                this.RaisePropertyChanged(nameof(CheckedPurchaseRequisitionMaterialLists));
            }
        }

        private bool isCheckedAll = false;

        public bool IsCheckedAll
        {
            get { return isCheckedAll; }
            set
            {
                isCheckedAll = value;
                this.RaisePropertyChanged(nameof(IsCheckedAll));
            }
        }

        private ObservableCollection<MaterialPlanInventoryModel> materialPlanInventoryLists = new ObservableCollection<MaterialPlanInventoryModel>();

        public ObservableCollection<MaterialPlanInventoryModel> MaterialPlanInventoryLists
        {
            get { return materialPlanInventoryLists; }
            set
            {
                materialPlanInventoryLists = value;
                this.RaisePropertyChanged(nameof(MaterialPlanInventoryLists));
            }
        }



        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand MouseLeftClickCommand { get; set; }
        public DelegateCommand ImportCommand { get; set; }
        public DelegateCommand MouseLeftClickCommand1 { get; set; }
        public DelegateCommand PurchaseRequisitionImportCommand { get; set; }
        public DelegateCommand PurchaseRequisitionInsertK3Command { get; set; }
        public DelegateCommand PurchaseRequisitionCheckBill1K3Command { get; set; }
        public DelegateCommand PurchaseRequisitionCheckBill2K3Command { get; set; }
        public DelegateCommand AllCheckCommand { get; set; }
        public DelegateCommand CalculateCommand { get; set; }
        public DelegateCommand UnLockCommand { get; set; }


        private void InitCommand()
        {

            AllCheckCommand = new DelegateCommand((obj) =>
            {
                if (IsCheckedAll)
                {
                    foreach (var item in MaterialPlanSeorderLists)
                    {
                        if (!item.IsLocked)
                            item.IsChecked = true;
                    }

                }
                else
                {
                    foreach (var item in MaterialPlanSeorderLists)
                    {
                        if (!item.IsLocked)
                            item.IsChecked = false;
                    }

                }

            });

            QueryCommand = new DelegateCommand((obj) =>
            {
                MaterialPlanSeOrderFullLists.Clear();
                MaterialPlanSeOrderFullLists = _materialPlanInventoryService.GetMaterialPlanSeorderFullLists(Filter.ParamBeginDate, Filter.ParamEndDate);

                MaterialPlanSeorderLists.Clear();
                MaterialPlanSeOrderFullLists.GroupBy(x => new { x.FCustName, x.FEmpName, x.FDeptName, x.FDate, x.FInterID, x.FBillNo })
                .Select(x => new MaterialPlanSeOrderShowModel { FCustName = x.Key.FCustName, FEmpName = x.Key.FEmpName, FDeptName = x.Key.FDeptName, FDate = x.Key.FDate, FInterID = x.Key.FInterID, FBillNo = x.Key.FBillNo })
                .ToList().ForEach(m => MaterialPlanSeorderLists.Add(m));
                MaterialPlanSeOrderEntryLists.Clear();

                _materialPlanInventoryService.DeleteMaterialPlanInventory();
               MaterialPlanInventoryLists.Clear();
            });

            UnLockCommand = new DelegateCommand((obj) =>
            {
                // 将列表的锁定状态归0并且清空原料锁定总数
                MaterialPlanSeOrderEntryLists.Clear();
                foreach (var item in MaterialPlanSeorderLists)
                {
                    item.IsChecked = false;
                    item.IsLocked = false;
                }
                _materialPlanInventoryService.DeleteMaterialPlanInventory();
                MaterialPlanInventoryLists.Clear();
            });

            CalculateCommand = new DelegateCommand((obj) =>
            {

                //var data = MaterialPlanSeorderLists.Where(x => !x.IsLocked && x.IsChecked).Select(x => x.FDetailId);
                if (MaterialPlanSeOrderEntryLists.Count() > 0)
                {
                    // 将选定计算行，锁定数据
                    string ids = string.Join(",", MaterialPlanSeOrderEntryLists.Select(x=>x.FDetailId));
                    MaterialPlanInventoryLists.Clear();
                    _materialPlanInventoryService.GetMaterialPlanInventoryLists(ids).OrderByDescending(x=>x.IsVisible).ToList().ForEach(x => MaterialPlanInventoryLists.Add(x));
                    MaterialPlanSeOrderEntryLists.Clear();
                    foreach (var item in MaterialPlanSeorderLists)
                    {
                        if (!item.IsLocked && item.IsChecked)
                            item.IsLocked = true;
                    }
                }
                else
                    MessageBox.Show("没有选择最新的数据");
            });

            MouseLeftClickCommand = new DelegateCommand((obj) =>
            {
                MaterialPlanSeOrderShowModel dr = (obj as DataGridRow).Item as MaterialPlanSeOrderShowModel;
                if (!dr.IsLocked)
                {
                    dr.IsChecked = !dr.IsChecked;
                    var cz = MaterialPlanSeOrderEntryLists.Any(x=>x.FBillNo == dr.FBillNo);
                    if (cz)
                    {
                        for (int i = 0; i < MaterialPlanSeOrderEntryLists.Count; i++)
                        {

                            if (MaterialPlanSeOrderEntryLists[i].FBillNo == dr.FBillNo)
                            {
                                MaterialPlanSeOrderEntryLists.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        var entries = MaterialPlanSeOrderFullLists.Where(x => x.FBillNo == dr.FBillNo).Select(x => new MaterialPlanSeOrderEntryShowModel
                        { FBillNo = x.FBillNo, FInterID = x.FInterID, FEntryID = x.FEntryID, FDetailId = x.FDetailId, DeliveryDate = x.DeliveryDate, FitemId = x.FitemId, FName = x.FName, FQty = x.FQty });
                        foreach (var item in entries)
                        {
                            MaterialPlanSeOrderEntryLists.Add(item);
                        }
                    }
                }

            });

            MouseLeftClickCommand1 = new DelegateCommand((obj) =>
            {
                //MaterialBomModel dr = (obj as DataGridRow).Item as MaterialBomModel;
                //dr.IsChecked = !dr.IsChecked;
            });

            ImportCommand = new DelegateCommand((obj) =>
            {
                StringBuilder sb = new StringBuilder();
                //文件选择窗口
                System.Windows.Forms.OpenFileDialog opd = new System.Windows.Forms.OpenFileDialog();
                opd.Title = "选择文件";
                //第一个参数是名称，随意取，第二个是模式匹配， 多个也是用“|”分割
                opd.Filter = "EXCEL文件|*.xls*";

                if (opd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ImportFileFullName = opd.FileName;
                    DataTable dataTable1 = new FileHelper().ConvertExcelToDataTable(opd.FileName, true);
                    DataTable dataTable2 = _materialPlanInventoryService.GetMaterialBomLists();

                    var query1 = from a in dataTable1.AsEnumerable()
                                 join b in dataTable2.AsEnumerable()
                                 on a.Field<string>("物料代码") equals b.Field<string>("Number")
                                    into temp
                                 from tt in temp.DefaultIfEmpty()
                                 select new MaterialBomModel
                                 {
                                     Seq = Convert.ToInt32(a.Field<string>("Seq")),
                                     Number = a.Field<string>("物料代码"),
                                     ItemName = tt == null ? "" : tt.Field<string>("ItemName"),
                                     BomCount = tt == null ? 0 : tt.Field<int>("BomCount"),
                                     ItemId = tt == null ? 0 : tt.Field<int>("ItemId")
                                 };
                    MaterialBomLists.Clear();
                    MaterialBomLists = new ObservableCollection<MaterialBomModel>(query1);

                }
                opd.Dispose();
            });

            PurchaseRequisitionImportCommand = new DelegateCommand((obj) =>
            {
                StringBuilder sb = new StringBuilder();
                //文件选择窗口
                System.Windows.Forms.OpenFileDialog opd = new System.Windows.Forms.OpenFileDialog();
                opd.Title = "选择文件";
                //第一个参数是名称，随意取，第二个是模式匹配， 多个也是用“|”分割
                opd.Filter = "EXCEL文件|*.xls*";

                if (opd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ImportFileFullName1 = opd.FileName;

                    // 读取Excel内容到导入Model
                    DataTable dataTable1 = new FileHelper().ConvertExcelToDataTable(opd.FileName, true);

                    // 读取数据库底表，跟上述excel数据做左连接来验证哪些是有合法的
                    DataTable dataTable2 = CommonService.GetAllItems();

                    var query1 = from a in dataTable1.AsEnumerable()
                                 join b in dataTable2.AsEnumerable()
                                 on a.Field<string>("物料代码") equals b.Field<string>("FNumber")
                                    into temp
                                 from tt in temp.DefaultIfEmpty()
                                 select new PurchaseRequisitionImportVerificationModel
                                 {
                                     Seq = Convert.ToInt32(a.Field<string>("Seq")),
                                     FNumber = a.Field<string>("物料代码"),
                                     Quantity = Convert.ToDouble(a.Field<string>("数量")),
                                     FName = tt == null ? "" : tt.Field<string>("FName"),
                                     SystemId = tt == null ? 0 : Convert.ToInt32(tt["FItemID"]),
                                     IsPassed = tt == null || string.IsNullOrEmpty(a.Field<string>("物料代码")) || Convert.ToDouble(a.Field<string>("数量")) <= 0 ? false : true
                                 };
                    CheckedPurchaseRequisitionMaterialLists.Clear();
                    CheckedPurchaseRequisitionMaterialLists = new ObservableCollection<PurchaseRequisitionImportVerificationModel>(query1);
                }
                opd.Dispose();
            });

            PurchaseRequisitionInsertK3Command = new DelegateCommand((obj) =>
            {

                MessageBoxResult result = System.Windows.MessageBox.Show("验证失败的数据将会被过滤，是否继续？", "【温馨提示】", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    var sons = new List<PurchaseRequisitionSonModel>();
                    var ss = CheckedPurchaseRequisitionMaterialLists.AsQueryable().Where(x => x.IsPassed);
                    var emp = _employeeLists.Where(m => m.FName == User.UserName).FirstOrDefault();
                    foreach (var item in ss)
                    {
                        var refSource = _purchaseRequisitionICItemLists.Where(m => m.FNumber == item.FNumber).FirstOrDefault();
                        if (refSource == null)
                        {
                            MessageBox.Show($" 此物料{item.FNumber}在t_icitem中不存在，请联系管理员");
                            return;
                        }
                        var son = new PurchaseRequisitionSonModel()
                        {
                            FItemID = new BaseNumberNameModel() { FNumber = refSource.FNumber, FName = refSource.FName },
                            FLeadTime = refSource.FFixLeadTime, //这个字段是根据FNumber带出来的，本身表里面没有是不是不用传递参数？
                            FAPurchTime = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                            FFetchTime = DateTime.Now.Date.AddDays(refSource.FFixLeadTime).ToString("yyyy-MM-dd"),
                            FAuxPropID = new BaseNumberNameModel() { FNumber = "", FName = "" },
                            FUnitID = _measureUnitLists.Where(m => m.FId == refSource.FUnitID).FirstOrDefault(), // FUnitID = new BaseNumberNameModel() { FNumber = "kg", FName = "kg" },
                            FBomInterID = new BaseNumberNameModel() { FNumber = "", FName = "" },
                            FSupplyID = new BaseNumberNameModel() { FNumber = "", FName = "" },
                            FPlanMode = new BaseNumberNameModel() { FNumber = "MTS", FName = "MTS计划模式" },

                            //FItemID = new BaseNumberNameModel() { FNumber = "YL.SC.RJ.00825", FName = "110" },
                            //FAuxPropID = new BaseNumberNameModel() { FNumber = "", FName = "" },
                            //FUnitID = new BaseNumberNameModel() { FNumber = "kg", FName = "kg" },
                            //FBomInterID = new BaseNumberNameModel() { FNumber = "", FName = "" },
                            //FSupplyID = new BaseNumberNameModel() { FNumber = "", FName = "" },
                            //FPlanMode = new BaseNumberNameModel() { FNumber = "MTS", FName = "MTS计划模式" },
                            FQty = item.Quantity,
                            Fauxqty = item.Quantity,

                            FOrderQty = 0,
                            Fuse = "",
                            FNumber = "",
                            FIsInquiry = 0,
                            FMTONo = "",
                            FMrpLockFlag = 0,
                            FSourceEntryID = 0,
                            FSourceInterId = 0,
                            FSourceTranType = 0,
                            FSourceBillNo = "",
                            FPlanOrderInterID = 0,
                            FSecQty = 0,
                            FSecCoefficient = 0,
                            FSecUnitID = null,
                            FAuxPropCls = null
                        };
                        sons.Add(son);
                    }
                    PurchaseRequisitionMainModel main = new PurchaseRequisitionMainModel()
                    {
                        //FHeadSelfP0131 = new BaseNumberNameModel() { FNumber = "10.01", FName = "销售部" },

                        FHeadSelfP0131 = new BaseNumberNameModel() { FNumber = "FSQLX02", FName = "生产部" },
                        FPlanCategory = new BaseNumberNameModel() { FNumber = "STD", FName = "标准" },
                        FBizType = new BaseNumberNameModel() { FNumber = "FPLX01", FName = "外购入库" },
                        FDeptID = new BaseNumberNameModel() { FNumber = "05.04.02", FName = "生产制造组" },
                        //FCheckerID = new BaseNumberNameModel(),
                        FSelTranType = new BaseNumberNameModel() { FNumber = "81", FName = "销售订单" },
                        FRequesterID = new BaseNumberNameModel() { FNumber = emp.FNumber, FName = emp.FName },
                        Fdate = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                        FMRP = 0,
                        FTranType = 70,
                        FGeneratePurBudQty = 0,
                        Fnote = "",
                        FNumber = "",
                        FBillerID = new BaseNumberNameModel { FNumber = emp.FNumber, FName = emp.FName }
                    };

                    var requestModel = new K3ApiInsertRequestModel<PurchaseRequisitionMainModel, PurchaseRequisitionSonModel>()
                    {
                        Data = new K3ApiInsertDataRequestModel<PurchaseRequisitionMainModel, PurchaseRequisitionSonModel>()
                        {
                            Page1 = new List<PurchaseRequisitionMainModel> { main },
                            Page2 = sons
                        }
                    };

                    string postJson = JsonHelper.ObjectToJson(requestModel);
                    K3ApiInsertResponseModel response = new K3ApiService("Purchase_Requisition").Insert(postJson);
                    K3InsertResponseData = response.Data;

                    MessageBox.Show($"{response.Message}");
                }
            });

            PurchaseRequisitionCheckBill1K3Command = new DelegateCommand((obj) =>
            {
                //var check = new K3ApiCheckBillRequestMode()
                //{
                //    Data = new K3ApiCheckBillDataRequestMode()
                //    {
                //        FBillNo = "POREQ005735",
                //        FChecker = "Administrator",
                //        FCheckDirection = 1,// 1 开启多级审核，预审
                //        FDealComment = ""
                //    }
                //};
                //string json = JsonHelper.ObjectToJson(check);
                //var checkRes = new K3ApiService("Purchase_Requisition").CheckBill(json);

            });

            PurchaseRequisitionCheckBill2K3Command = new DelegateCommand((obj) =>
            {
                if (string.IsNullOrEmpty(K3InsertResponseData.BillNo))
                {
                    MessageBox.Show("请生成或者输入单据号");
                    return;
                }

                var check = new K3ApiCheckBillRequestMode()
                {
                    Data = new K3ApiCheckBillDataRequestMode()
                    {
                        FBillNo = K3InsertResponseData.BillNo,
                        FChecker = User.UserName,
                        FCheckDirection = 2,// 1 开启多级审核，预审
                        FDealComment = ""
                    }
                };
                string json = JsonHelper.ObjectToJson(check);
                var checkRes = new K3ApiService("Purchase_Requisition").CheckBill(json);
                MessageBox.Show(checkRes.Message);
            });
        }

    }
}
