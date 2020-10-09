using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Ui.Command;
using Ui.Helper;
using Ui.Service;

namespace Ui.ViewModel
{
    public class UserDataGridFormatConfigurationViewModel: NewDialogViewModel<UserDataGridFormatConfigurationViewModel>
    {
        private readonly DataGridManagementService _service;
        private readonly string _dataGridName;

        public UserDataGridFormatConfigurationViewModel(string dataGridName)
        {
            _dataGridName = dataGridName;
            _service = new DataGridManagementService();
            InitData();
            InitCommand();
        }

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                DataGridLists = new ObservableCollection<DataGridColumnHeaderUserCustomModel>();
                UIExecute.RunAsync(() =>
                {
                    _service.GetUserDataGridColumnLists(_dataGridName, User.ID,false).ForEach(x => DataGridLists.Add(x));
                });
            });
        }

        public DelegateCommand MoveUpCommand { get; set; }
        public DelegateCommand MoveDownCommand { get; set; }
       // public DelegateCommand SaveConfigurationCommand { get; set; }

        private void InitCommand()
        {
            //SaveConfigurationCommand = new DelegateCommand((obj) =>
            //{   
            //    // 把界面数据同步到模板上
            //    CommonService.LoadIEnumerableToDatabase(DataGridLists, "SJDataGridColumnHeaderTemplate");
                
            //    // Insert或者Update数据
            //    _service.SaveColumnConfigurationInManagementView(_dataGridName, User.ID);

            //});

            MoveUpCommand = new DelegateCommand((obj) =>
            {
                var cloneData = ObjectDeepCopyHelper<DataGridColumnHeaderUserCustomModel, DataGridColumnHeaderUserCustomModel>.Trans(DataGridSelectedItem);

                int index = DataGridLists.IndexOf(DataGridSelectedItem);
                if (index == 0)
                    return;

                int upOrder = DataGridLists.ElementAt(index - 1).ColumnOrder;
                int currentOrder = DataGridSelectedItem.ColumnOrder;
                cloneData.ColumnOrder = upOrder;
                DataGridLists.ElementAt(index - 1).ColumnOrder = currentOrder;

                DataGridLists.RemoveAt(index);
                index--;
                DataGridLists.Insert(index, cloneData);
                DataGridSelectedItem = cloneData;
            });

            MoveDownCommand = new DelegateCommand((obj) =>
            {
                var cloneData = ObjectDeepCopyHelper<DataGridColumnHeaderUserCustomModel, DataGridColumnHeaderUserCustomModel>.Trans(DataGridSelectedItem);
                int index = DataGridLists.IndexOf(DataGridSelectedItem);
                if (index == DataGridLists.Count - 1)
                    return;

                int downOrder = DataGridLists.ElementAt(index + 1).ColumnOrder;
                int currentOrder = DataGridSelectedItem.ColumnOrder;
                cloneData.ColumnOrder = downOrder;
                DataGridLists.ElementAt(index + 1).ColumnOrder = currentOrder;

                DataGridLists.RemoveAt(index);
                index++;
                DataGridLists.Insert(index, cloneData);
                DataGridSelectedItem = cloneData;
            });
        }

        public override void Save(object obj)
        {
            // 把界面数据同步到模板上
            CommonService.LoadIEnumerableToDatabase(DataGridLists, "SJDataGridColumnHeaderTemplate");

            // Insert或者Update数据
            _service.SaveColumnConfigurationInManagementView(_dataGridName, User.ID);

            base.Save(obj);
        }

        private DataGridColumnHeaderUserCustomModel dataGridSelectedItem;

        public DataGridColumnHeaderUserCustomModel DataGridSelectedItem
        {
            get { return dataGridSelectedItem; }
            set
            {
                dataGridSelectedItem = value;
                this.RaisePropertyChanged(nameof(DataGridSelectedItem));
            }
        }


        private ObservableCollection<DataGridColumnHeaderUserCustomModel> dataGridLists;

        public ObservableCollection<DataGridColumnHeaderUserCustomModel> DataGridLists
        {
            get { return dataGridLists; }
            set
            {
                dataGridLists = value;
                this.RaisePropertyChanged(nameof(DataGridLists));
            }
        }

   

    }
}
