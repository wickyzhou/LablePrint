using Model;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Ui.Admin.View;
using Ui.Command;
using Ui.Service;
using Ui.ViewModel;

namespace Ui.Admin.ViewModel
{
    public class AdminPageViewModel : BaseViewModel
    {
        private  readonly AdminService _service;
        public AdminPageViewModel()
        {
            _service = new AdminService();
            DataInit(); 
            InitCommand();
        }


        public DelegateCommand DataGridDefaultConfigCommand { get; set; }
        public DelegateCommand TextBlockMouseLeftButtonDownCommand { get; set; }
        public DelegateCommand DataGridRowMouseLeftClickBaseCommand { get; set; }
        

        private void InitCommand()
        {
            DataGridDefaultConfigCommand = new DelegateCommand((obj) =>
            {
                DataGridManagementWinodw window = new DataGridManagementWinodw();
                window.ShowDialog();
            });

            TextBlockMouseLeftButtonDownCommand = new DelegateCommand((obj) =>
            {
                
            });

            DataGridRowMouseLeftClickBaseCommand = new DelegateCommand((obj) =>
            {
                long dt1 = DateTime.Now.Ticks;
                ActionOperationLogModel dr = (obj as DataGridRow).Item as ActionOperationLogModel;
                foreach (var item in Lists)
                {
                    if (item.Id == dr.Id)
                        dr.IsChecked = !dr.IsChecked;
                    else
                        item.IsChecked = false;
                }
                long dt2 = DateTime.Now.Ticks;
                
                MessageBox.Show($"{dt2 - dt1} \t  此数据的：{dr.Id}");

            });
        }

        private void DataInit()
        {
            Lists = new ObservableCollection<ActionOperationLogModel>();
            _service.GetActionOperationLogLists().ForEach(x=>Lists.Add(x));
            //Lists.Add(new ActionOperationLogModel { Id = 34, ActionDesc = "自定义值6" });
            //Lists.Add(new ActionOperationLogModel { Id=31, ActionDesc = "自定义值1" });
            //Lists.Add(new ActionOperationLogModel { Id = 32, ActionDesc = "自定义值2" });
            //Lists.Add(new ActionOperationLogModel { Id = 33, ActionDesc = "自定义值3" });
           

        }

        private ObservableCollection<ActionOperationLogModel> lists;

        public ObservableCollection<ActionOperationLogModel> Lists
        {
            get { return lists; }
            set
            {
                lists = value;
                this.RaisePropertyChanged(nameof(Lists));
            }
        }

    }
}
