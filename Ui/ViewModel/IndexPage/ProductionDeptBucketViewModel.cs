
using Model;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.Bucket;

namespace Ui.ViewModel.IndexPage
{
    public class ProductionDeptBucketViewModel : BaseViewModel
    {
        public ProductionDeptBucketViewModel()
        {
            InitCommand();
            InitData();
        }

        public BucketService Service { get; set; } = new BucketService();

        public DelegateCommand SyncBucketInfo { get; set; }
        
        public DelegateCommand ModifyCommand { get; set; }


        private void InitCommand()
        {
            SyncBucketInfo = new DelegateCommand((obj) =>
            {
                int count = Service.SyncBucketInfo();
                MessageBox.Show($"成功更新【{count}】条桶子名称");
            });

            ModifyCommand = new DelegateCommand((obj) =>
            {

                var cloneData = ObjectDeepCopyHelper<BucketModel, BucketModel>.Trans(BucketSelectedItem);
                BucketModifyWindow view = new BucketModifyWindow();
                (view.DataContext as BucketModifyViewModel).WithParam(cloneData, (type, outputEntity, IsChanged) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        Service.BucketModify(outputEntity);
                        //if(IsChanged)
                        QueryBaseCommand.Execute(null);
                    }
                });
                view.ShowDialog();
            });

            QueryBaseCommand = new DelegateCommand((obj) =>
            {
                Lists.Clear();
                Service.GetLists($" and FName like '%{BucketName}%'").ForEach(x=>Lists.Add(x));
            });
        }

        private string bucketName="";

        public string BucketName
        {
            get { return bucketName; }
            set
            {
                bucketName = value;
                this.RaisePropertyChanged(nameof(BucketName));
            }
        }


        private ObservableCollection<BucketModel> lists = new ObservableCollection<BucketModel>();

        public ObservableCollection<BucketModel> Lists
        {
            get { return lists; }
            set
            {
                lists = value;
                this.RaisePropertyChanged(nameof(Lists));
            }
        }

        private BucketModel bucketSelectedItem;

        public BucketModel BucketSelectedItem
        {
            get { return bucketSelectedItem; }
            set
            {
                bucketSelectedItem = value;
                this.RaisePropertyChanged(nameof(BucketSelectedItem));
            }
        }



        private void InitData()
        {
            QueryBaseCommand.Execute(null);

            //RebateClassLists = CommonService.GetEnumLists(6);
            //OrganizationLists = ComboBoxSearchService.GetOrganizationLists();
            //MinusLastPeriodRebateLists = CommonService.GetEnumLists(999);
            //_salesRebateService.GetSalesRebateLists(_userDataId, IsHistory).ForEach(x => SalesRebateLists.Add(x));

        }
    }
}
