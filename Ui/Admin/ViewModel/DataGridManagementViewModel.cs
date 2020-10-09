using Model;
using QueryParameterModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Ui.Admin.View;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.ViewModel;

namespace Ui.Admin.ViewModel
{
    public class DataGridManagementViewModel:BaseViewModel
    {
        private readonly DataGridManagementService _service;
        public DataGridManagementViewModel()
        {
            _service = new DataGridManagementService();
            InitData();
            InitCommand();
        }

  

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                QueryParameter = new DataGridColumnHeaderQueryParameterModel();
                DataGridLists = new ObservableCollection<DataGridColumnHeaderModel>();
                UIExecute.RunAsync(() =>
                {

                    
                });
            });
        }

        private void InitCommand()
        {

            SaveCommand = new DelegateCommand((obj) =>
            {
                CommonService.LoadIEnumerableToDatabase(DataGridLists, "SJDataGridColumnHeaderTemplate");
                // 修改后台数据
                if (_service.BatchUpdate())
                    QueryCommand.Execute(null);
            });

            NewCommand = new DelegateCommand((obj) =>
            {
                DataGridColumnHeaderModel inputEntity = DataGridSelectedItem == null? new DataGridColumnHeaderModel {MainMenuId = Menu.ID, ColumnOrder=1000,ColumnWidth = 150 } : ObjectDeepCopyHelper<DataGridColumnHeaderModel,DataGridColumnHeaderModel>.Trans(DataGridSelectedItem); 

                DataGridColumnAddWindow view = new DataGridColumnAddWindow();

                (view.DataContext as DataGridColumnAddViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_service.Insert(outputEntity))
                            QueryCommand.Execute(null);
                    }
                });
                view.ShowDialog();
            });

            MoveUpCommand = new DelegateCommand((obj) =>
            {
                var cloneData = ObjectDeepCopyHelper<DataGridColumnHeaderModel, DataGridColumnHeaderModel>.Trans(DataGridSelectedItem);

                int index = DataGridLists.IndexOf(DataGridSelectedItem);
                if (index == 0)
                    return;

                int upOrder = DataGridLists.ElementAt(index - 1).ColumnOrder;
                int currentOrder = DataGridSelectedItem.ColumnOrder;
                cloneData.ColumnOrder = upOrder;
                DataGridLists.ElementAt(index - 1).ColumnOrder = currentOrder;

                DataGridLists.RemoveAt(index);
                index -- ;
                DataGridLists.Insert(index, cloneData);
                DataGridSelectedItem = cloneData;
            });

            MoveDownCommand = new DelegateCommand((obj) =>
            {
                var cloneData = ObjectDeepCopyHelper<DataGridColumnHeaderModel, DataGridColumnHeaderModel>.Trans(DataGridSelectedItem);
                int index = DataGridLists.IndexOf(DataGridSelectedItem);
                if (index == DataGridLists.Count - 1)
                    return;

                int downOrder = DataGridLists.ElementAt(index + 1).ColumnOrder;
                int currentOrder = DataGridSelectedItem.ColumnOrder;
                cloneData.ColumnOrder = downOrder;
                DataGridLists.ElementAt(index + 1).ColumnOrder = currentOrder;

                DataGridLists.RemoveAt(index);
                index ++;
                DataGridLists.Insert(index, cloneData);
                DataGridSelectedItem = cloneData;
            });

            QueryCommand = new DelegateCommand((obj) =>
            {
                DataGridLists.Clear();
                _service.GetDataGridLists(CommonService.GetSqlWhereString(QueryParameter)).ForEach(x=> DataGridLists.Add(x));
            });
        }

        public DelegateCommand MoveUpCommand { get; set; }
        public DelegateCommand MoveDownCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand NewCommand { get; set; }


        private DataGridColumnHeaderQueryParameterModel queryParameter;

        public DataGridColumnHeaderQueryParameterModel QueryParameter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }


        private ObservableCollection<DataGridColumnHeaderModel> dataGridLists;

        public ObservableCollection<DataGridColumnHeaderModel> DataGridLists
        {
            get { return dataGridLists; }
            set
            {
                dataGridLists = value;
                this.RaisePropertyChanged(nameof(DataGridLists));
            }
        }

        private DataGridColumnHeaderModel dataGridSelectedItem;

        public DataGridColumnHeaderModel DataGridSelectedItem
        {
            get { return dataGridSelectedItem; }
            set
            {
                dataGridSelectedItem = value;
                this.RaisePropertyChanged(nameof(DataGridSelectedItem));
            }
        }

    }
}
