using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                QueryParameter = new DataGridColumnHeaderModel();
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
                var ss = DataGridSelectedItem;
            });

            NewCommand = new DelegateCommand((obj) =>
            {
                DataGridColumnHeaderModel inputEntity = DataGridSelectedItem == null? new DataGridColumnHeaderModel {MainMenuId = Menu.ID } : ObjectDeepCopyHelper<DataGridColumnHeaderModel,DataGridColumnHeaderModel>.Trans(DataGridSelectedItem); 

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
                
            });

            MoveDownCommand = new DelegateCommand((obj) =>
            {

            });

            QueryCommand = new DelegateCommand((obj) =>
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (!string.IsNullOrEmpty(QueryParameter.DataGridName))
                    stringBuilder.Append($" and DataGridName like '%{QueryParameter.DataGridName}%'");

                if (!string.IsNullOrEmpty(QueryParameter.TableName))
                    stringBuilder.Append($" and TableName like '%{QueryParameter.TableName}%'");

                if (!string.IsNullOrEmpty(QueryParameter.ColumnFieldName))
                    stringBuilder.Append($" and ColumnFieldName like '%{QueryParameter.ColumnFieldName}%'");

                if (!string.IsNullOrEmpty(QueryParameter.ColumnHeaderName))
                    stringBuilder.Append($" and ColumnHeaderName like '%{QueryParameter.ColumnHeaderName}%'");

                DataGridLists.Clear();

                _service.GetDataGridLists(stringBuilder.ToString()).ForEach(x=> DataGridLists.Add(x));

            });
        }

        public DelegateCommand MoveUpCommand { get; set; }
        public DelegateCommand MoveDownCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand NewCommand { get; set; }


        private DataGridColumnHeaderModel queryParameter;

        public DataGridColumnHeaderModel QueryParameter
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
