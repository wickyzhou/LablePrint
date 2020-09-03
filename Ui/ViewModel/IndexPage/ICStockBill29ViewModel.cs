using Common;
using ImportVerificationModel;
using K3ApiModel;
using K3ApiModel.ICStockBill29;
using K3ApiModel.Request;
using K3ApiModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel.IndexPage
{
    public class ICStockBill29ViewModel : K3ApiBaseViewModel
    {
        private ICStockService _stockService;
        public ICStockBill29ViewModel() : base("Miscellaneous_Delivery")
        {
            _stockService = new ICStockService();
            InitData();
            InitCommand();
        }

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                CheckedICStockBill29VerificationLists = new ObservableCollection<ICStockBill29ImportVerificationModel>();
                K3InsertResponseData = new K3ApiInsertDataMultiResponseModel();
            });
        }

        private void InitCommand()
        {
            ICStockBill29ImportCommand = new DelegateCommand((obj) =>
            {
                System.Windows.Forms.OpenFileDialog opd = new System.Windows.Forms.OpenFileDialog();
                opd.Title = "选择文件";
                opd.Filter = "EXCEL文件|*.xls*";
                if (opd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CommonService.ImportExcelToDatabaseTable(opd.FileName, "SJICStockBill29ExcelTemplate");
                    ImportFileFullName = opd.FileName;
                    // 数据验证后加载验证模型列表到表格
                    CheckedICStockBill29VerificationLists.Clear();
                    _stockService.GetICStockBill29ImportVerificationLists().ForEach(x => CheckedICStockBill29VerificationLists.Add(x));
                }
                opd.Dispose();
            });
            ICStockBill29InsertK3Command = new DelegateCommand((obj) =>
            {   


                if (CheckedICStockBill29VerificationLists.Count == 0)
                    MessageBox.Show("请先导入Excel模板数据");
                else if(CheckedICStockBill29VerificationLists.Where(m=>!m.IsPassed).Count()>0)
                    MessageBox.Show("必须全部数据验证成功才允许导入，请修改Excel数据");
                else
                {   
                    var depts = CheckedICStockBill29VerificationLists.GroupBy(m => m.DeptFNumber);
                    foreach (var item in depts)
                    {
                        var data = CheckedICStockBill29VerificationLists.Where(m=>m.DeptFNumber==item.Key);
                        var first = data.First();
                        var main = new MiscellaneousDeliveryMainModel()
                        {
                            Fuse = "研发领料",
                            FHeadSelfB0939 = first.BillNo,
                            FDate = DateTime.Now.Date.ToShortDateString(),
                            FDeptID = new BaseNumberNameModel() { FNumber = first.DeptFNumber, FName = first.DeptFName },
                            FBillerID = new BaseNumberNameModel() { FNumber = "何科威", FName = "何科威" },
                            // FFManagerID = new BaseNumberNameModel() { FNumber = "1033", FName = "付志刚" },  领料人
                            FFManagerID = new BaseNumberNameModel() { FNumber = first.FFManagerFNumber, FName = first.FFManagerName },
                            FSManagerID = new BaseNumberNameModel() { FNumber = "1033", FName = "付志刚" },
                            Note = first.Note
                        };

                        var sons = new List<MiscellaneousDeliverySonModel>();
                        foreach (ICStockBill29ImportVerificationModel entry in data)
                        {
                            var son = new MiscellaneousDeliverySonModel()
                            {
                                FItemID = new BaseNumberNameModel() { FNumber = entry.MaterialFNumber, FName = entry.MaterialFName },
                                FUnitID = new BaseNumberNameModel() { FNumber = "kg", FName = "kg" },
                                FDCStockID1 = new BaseNumberNameModel() { FNumber = entry.StockFNumber, FName = entry.StockFName },
                                FDCSPID = new BaseNumberNameModel() { FNumber = entry.StockPlaceFNumber, FName = entry.StockPlaceFName },
                                FPlanMode = new BaseNumberNameModel() { FNumber = "MTS", FName = "MTS计划模式" },
                                FQty = entry.Quantity,
                                FScanQty = entry.Quantity,
                                Fauxqty = entry.Quantity,
                                FEntrySelfB0947 = entry.BrandName,//"品牌",
                                FEntrySelfB0948 = entry.CaseName,//"案子",
                                FBatchNo = entry.BatchNo,
                            };
                            sons.Add(son);
                        }

                        var requestModel = new K3ApiInsertRequestModel<MiscellaneousDeliveryMainModel, MiscellaneousDeliverySonModel>()
                        {
                            Data = new K3ApiInsertDataRequestModel<MiscellaneousDeliveryMainModel, MiscellaneousDeliverySonModel>()
                            {
                                Page1 = new List<MiscellaneousDeliveryMainModel> { main },
                                Page2 = sons
                            }
                        };

                        string postJson = JsonHelper.ObjectToJson(requestModel);
                        K3ApiInsertResponseModel response = new K3ApiService("Miscellaneous_Delivery").Insert(postJson);
                        if (response.StatusCode == 200)
                        {
                            K3InsertResponseData.BillNo += response.Data.BillNo + "\t";
                            K3InsertResponseData.BillInterID += response.Data.BillInterID + "\t";
                        }
                        else
                        {
                            MessageBox.Show($"{response.Message}");
                            return;
                        }
                    }
                    MessageBox.Show($"导入成功");
                }
            });
        }

        public DelegateCommand ICStockBill29ImportCommand { get; set; }
        public DelegateCommand ICStockBill29InsertK3Command { get; set; }

        private string importFileFullName;

        public string ImportFileFullName
        {
            get { return importFileFullName; }
            set
            {
                importFileFullName = value;
                this.RaisePropertyChanged(nameof(ImportFileFullName));
            }
        }

        private ObservableCollection<ICStockBill29ImportVerificationModel> checkedICStockBill29VerificationLists;

        public ObservableCollection<ICStockBill29ImportVerificationModel> CheckedICStockBill29VerificationLists
        {
            get { return checkedICStockBill29VerificationLists; }
            set
            {
                checkedICStockBill29VerificationLists = value;
                this.RaisePropertyChanged(nameof(CheckedICStockBill29VerificationLists));
            }
        }

        private K3ApiInsertDataMultiResponseModel k3InsertResponseData;

        public K3ApiInsertDataMultiResponseModel K3InsertResponseData
        {
            get { return k3InsertResponseData; }
            set
            {
                k3InsertResponseData = value;
                this.RaisePropertyChanged(nameof(K3InsertResponseData));
            }
        }

    }
}


