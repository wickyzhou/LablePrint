using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
