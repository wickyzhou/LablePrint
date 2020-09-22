using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ui.Admin.View;
using Ui.Command;

namespace Ui.ViewModel.IndexPage
{
    public class AdminPageViewModel : BaseViewModel
    {
        public AdminPageViewModel()
        {
            //Lists = new ObservableCollection<ZiDongShengChengLieModel>();

            //Lists.Add(new ZiDongShengChengLieModel { Text = "自定义值1" });
            //Lists.Add(new ZiDongShengChengLieModel { Text = "自定义值2" });
            //Lists.Add(new ZiDongShengChengLieModel { Text = "自定义值3" });
            //Lists.Add(new ZiDongShengChengLieModel { Text = "自定义值4" });
            //Lists.Add(new ZiDongShengChengLieModel { Text = "自定义值5" });
            //Lists.Add(new ZiDongShengChengLieModel { Text = "自定义值6" });
            DataInit(); 
            InitCommand();
        }


        public DelegateCommand DataGridDefaultConfigCommand { get; set; }

        private void InitCommand()
        {
            DataGridDefaultConfigCommand = new DelegateCommand((obj) =>
            {
                DataGridManagementWinodw window = new DataGridManagementWinodw();
                window.ShowDialog();
            });
        }

        private void DataInit()
        {
            Lists = new ObservableCollection<ActionOperationLogModel>();
            
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
