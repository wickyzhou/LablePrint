using Common;
using Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel.IndexPage
{
    public class MaterialPlanInventoryViewModel : NotificationObject
    {

        private MaterialPlanInventoryService _materialPlanInventoryService;
        private static readonly string _hostName = Dns.GetHostName();
        private static readonly UserModel user = MemoryCache.Default["user"] as UserModel;

        public MaterialPlanInventoryViewModel()
        {
            _materialPlanInventoryService = new MaterialPlanInventoryService();


            InitData();
            InitCommand();


        }

        private void InitData()
        {
            MaterialPlanSeorderLists = new ObservableCollection<MaterialPlanSeorderModel>();
            MaterialDemandLists = new ObservableCollection<MaterialDemandModel>();
            MaterialPlanSeorderSelectedItem = new MaterialPlanSeorderModel();
            MaterialBomLists = new ObservableCollection<MaterialBomModel>();


            Filter = new MaterialPlanInventoryParameterModel
            {
                ParamBeginDate = Convert.ToDateTime(System.DateTime.Now.AddDays(-7).ToShortDateString()),
                ParamEndDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString()),
            };

            Task.Factory.StartNew(() =>
            {
                UIExecute.RunAsync(() =>
                {
                    
                    _materialPlanInventoryService.GetMaterialPlanSeorderLists(Filter.ParamBeginDate, Filter.ParamEndDate).ToList().ForEach(x => MaterialPlanSeorderLists.Add(x));
                });
            });
        }

        private ObservableCollection<MaterialPlanSeorderModel>  materialPlanSeorderLists;

        public ObservableCollection<MaterialPlanSeorderModel> MaterialPlanSeorderLists
        {
            get { return materialPlanSeorderLists; }
            set
            {
                materialPlanSeorderLists = value;
                this.RaisePropertyChanged(nameof(MaterialPlanSeorderLists));
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

        private ObservableCollection<MaterialDemandModel> materialDemandLists;

        public ObservableCollection<MaterialDemandModel> MaterialDemandLists
        {
            get { return materialDemandLists; }
            set
            {
                materialDemandLists = value;
                this.RaisePropertyChanged(nameof(MaterialDemandLists));
            }
        }

        private MaterialPlanSeorderModel materialPlanSeorderSelectedItem;

        public MaterialPlanSeorderModel MaterialPlanSeorderSelectedItem
        {
            get { return materialPlanSeorderSelectedItem; }
            set
            {
                materialPlanSeorderSelectedItem = value;
                this.RaisePropertyChanged(nameof(MaterialPlanSeorderSelectedItem));
            }
        }


        private ObservableCollection<MaterialBomModel> materialBomLists;

        public ObservableCollection<MaterialBomModel> MaterialBomLists
        {
            get { return materialBomLists; }
            set
            {
                materialBomLists = value;
                this.RaisePropertyChanged(nameof(MaterialBomLists));
            }
        }

        private string importFileFullName="";

        public string ImportFileFullName 
        {
            get { return importFileFullName; }
            set
            {
                importFileFullName = value;
                this.RaisePropertyChanged(nameof(ImportFileFullName));
            }
        }


        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand MouseLeftClickCommand { get; set; }
        public DelegateCommand ImportCommand { get; set; }
        public DelegateCommand MouseLeftClickCommand1 { get; set; }



        private void InitCommand()
        {
            QueryCommand = new DelegateCommand(Query);
            MouseLeftClickCommand = new DelegateCommand((obj)=> {
                MaterialPlanSeorderModel dr = (obj as DataGridRow).Item as MaterialPlanSeorderModel;
                dr.IsChecked = !dr.IsChecked;
            });

            MouseLeftClickCommand1 = new DelegateCommand((obj) => {
                MaterialBomModel dr = (obj as DataGridRow).Item as MaterialBomModel;
                dr.IsChecked = !dr.IsChecked;
            });
            ImportCommand = new DelegateCommand(Import);
        }

        private void Import(object obj)
        {
            StringBuilder sb = new StringBuilder();
            //文件选择窗口
            OpenFileDialog opd = new OpenFileDialog();
            opd.Title = "选择文件";
            //第一个参数是名称，随意取，第二个是模式匹配， 多个也是用“|”分割
            opd.Filter = "EXCEL文件|*.xls*";

            if (opd.ShowDialog() == DialogResult.OK)
            {
                ImportFileFullName = opd.FileName;
               DataTable dt=  new FileHelper().ConvertExcelToDataTable(opd.FileName,true);
                foreach (var item in dt.AsEnumerable().Select(m => m.Field<string>(0)))
                {
                    if (!string.IsNullOrEmpty(item))
                        sb.Append(",'"+item+"'");
                }
                string itemNames = sb.ToString().Substring(1);
                var itemList = _materialPlanInventoryService.GetMaterialFItemIds(itemNames);
                if (itemList.Count>0)
                {
                    _materialPlanInventoryService.GetMaterialBomLists(string.Join(",", itemList) ).ToList().ForEach(x => MaterialBomLists.Add(x));
                }
            }
            opd.Dispose();
        }



        private void Query(object obj)
        {
            MaterialPlanSeorderLists.Clear();
            _materialPlanInventoryService.GetMaterialPlanSeorderLists(Filter.ParamBeginDate, Filter.ParamEndDate).ToList().ForEach(x => MaterialPlanSeorderLists.Add(x));
        }

    }
}
