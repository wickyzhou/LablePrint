using Model;
using QueryParameterModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Helper;
using Ui.Service;

namespace Ui.ViewModel
{
    public class PrintCommonAdjustmentViewModel : BaseViewModel
    {
        public PrintCommonAdjustmentViewModel()
        {
            InitCommand();
            InitData();
        }

        private void InitCommand()
        {
            SaveCommand = new DelegateCommand((obj) =>
            {

                //验证数据是否异常
                var r = CommonAdjustmentLists.GroupBy(x => new { x.TypeId, x.OrgId, x.Label }).Select(x => new {Rc= x.Count() });
                if (!r.Any(x=>x.Rc>1))
                {
                    // 将界面数据插入到后台
                    CommonService.LoadIEnumerableToDatabase(CommonAdjustmentLists, "SJPrintCommonAdjustment");

                    // 将之前的后台数据删除
                    var ids = string.Join(",", CommonAdjustmentLists.Where(x => x.Id > 0).Select(x => x.Id));
                    LabelPrintService.DeletePrintCommonAdjustment(ids);
                    QueryBaseCommand.Execute(null);
                }
                else
                    MessageBox.Show("类型+客户+标签 必须唯一");

            });

            RemoveCommand = new DelegateCommand((obj) =>
            {
                if (PrintCommonAdjustmentSelectedItem != null)
                {
                    int id = PrintCommonAdjustmentSelectedItem.Id;
                    CommonAdjustmentLists.Remove(PrintCommonAdjustmentSelectedItem);
                    if (id > 0)
                        LabelPrintService.DeletePrintCommonAdjustment(id);
                }
           
            });

            CopyCommand = new DelegateCommand((obj) =>
            {
                var cloneData = PrintCommonAdjustmentSelectedItem == null ? new PrintCommonAdjustmentModel() : ObjectDeepCopyHelper<PrintCommonAdjustmentModel, PrintCommonAdjustmentModel>.Trans(PrintCommonAdjustmentSelectedItem);
                cloneData.Id = 0;
                CommonAdjustmentLists.Insert(0, cloneData);
            });


            QueryBaseCommand = new DelegateCommand((obj) =>
            {
                CommonAdjustmentLists.Clear();
                LabelPrintService.GetLists(CommonService.GetSqlWhereString(QueryParameter)).ForEach(x => CommonAdjustmentLists.Add(x));
            });
        }

        private void InitData()
        {

            Task.Factory.StartNew(() =>
            {
                CommonService.GetEnumLists(12).ToList().ForEach(x => TypeSelectorLists.Add(x));
                UIExecute.RunAsync(() =>
                {
                    QueryBaseCommand.Execute(null);
                });
            });
        }

        public LabelPrintService LabelPrintService { get; set; } = new LabelPrintService();

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand RemoveCommand { get; set; }
        public DelegateCommand CopyCommand { get; set; }


        private PrintCommonAdjustmentModel printCommonAdjustmentSelectedItem;

        public PrintCommonAdjustmentModel PrintCommonAdjustmentSelectedItem
        {
            get { return printCommonAdjustmentSelectedItem; }
            set
            {
                printCommonAdjustmentSelectedItem = value;
                this.RaisePropertyChanged(nameof(PrintCommonAdjustmentSelectedItem));
            }
        }

        private EnumModel typeSeletedItem ;

        public EnumModel TypeSeletedItem
        {
            get { return typeSeletedItem ; }
            set
            {
                typeSeletedItem  = value;
                this.RaisePropertyChanged(nameof(TypeSeletedItem));
            }
        }



        private ObservableCollection<PrintCommonAdjustmentModel> commonAdjustmentLists = new ObservableCollection<PrintCommonAdjustmentModel>();

        public ObservableCollection<PrintCommonAdjustmentModel> CommonAdjustmentLists
        {
            get { return commonAdjustmentLists; }
            set
            {
                commonAdjustmentLists = value;
                this.RaisePropertyChanged(nameof(CommonAdjustmentLists));
            }
        }

        private ObservableCollection<EnumModel> typeSelectorLists = new ObservableCollection<EnumModel>();

        public ObservableCollection<EnumModel> TypeSelectorLists
        {
            get { return typeSelectorLists; }
            set
            {
                typeSelectorLists = value;
                this.RaisePropertyChanged(nameof(TypeSelectorLists));
            }
        }


        private PrintCommonAdjustmentQueryParameterModel queryParameter = new PrintCommonAdjustmentQueryParameterModel();

        public PrintCommonAdjustmentQueryParameterModel QueryParameter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }


    }
}
