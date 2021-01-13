using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel
{
    public class ExportViewModel : BaseViewModel
    {
        public ExportViewModel(int viewGroupId,int defaultRadio)
        {

            DynamicVisibility = defaultRadio == 3 ? Visibility.Visible : Visibility.Hidden;
            Entity = defaultRadio;

            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
            DynamicGrid1ShowCommand = new DelegateCommand((obj) =>
            {
                    DynamicVisibility = Visibility.Hidden;
                    Entity = 1;
            });
            DynamicGrid2ShowCommand = new DelegateCommand((obj) =>
            {
                    DynamicVisibility = Visibility.Hidden;
                Entity = 2;
            });
            DynamicGrid3ShowCommand = new DelegateCommand((obj) =>
            {
                    DynamicVisibility = Visibility.Visible;
                Entity = 3;
            });

            CheckBoxSelectCommand = new DelegateCommand((x)=> 
            {
                var p1 = (int)((List<object>)x)[0];  // checkboxId 1\2\4\8
                var p2 = (string)((List<object>)x)[1];
                if ((CheckBoxSelectedValue | p1) == CheckBoxSelectedValue)
                    CheckBoxColumns.Remove(p2);
                else
                    CheckBoxColumns.Add(p2);
                CheckBoxSelectedValue ^= p1;
                //MessageBox.Show($"{CheckBoxSelectedValue} \t {string.Join(",",CheckBoxColumns)}");
            });

            var typedColumnModel = new CommonService().GetExportViewTypedColumnWithCheckBox(viewGroupId).Where(m => m.IsChecked).FirstOrDefault();
            var firstColumn = typedColumnModel==null?"": typedColumnModel.TypedColumnName;
            CheckBoxColumns = new List<string>() { firstColumn };


        }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand DynamicGrid1ShowCommand { get; set; }
        public DelegateCommand DynamicGrid2ShowCommand { get; set; }
        public DelegateCommand DynamicGrid3ShowCommand { get; set; }
        public DelegateCommand CheckBoxSelectCommand { get; set; }


        public List<string> CheckBoxColumns { get; set; } 


        private int  checkBoxSelectedValue = 1;

        public int CheckBoxSelectedValue
        {
            get { return checkBoxSelectedValue; }
            set
            {
                checkBoxSelectedValue = value;
                this.RaisePropertyChanged(nameof(CheckBoxSelectedValue));
            }
        }



        private Visibility  dynamicVisibility;

        public Visibility DynamicVisibility
        {
            get { 
                return dynamicVisibility; }
            set
            {
                dynamicVisibility = value;
                this.RaisePropertyChanged(nameof(DynamicVisibility));
            }
        }


        public virtual void Save(object obj)
        {
            if (Entity==3 && CheckBoxSelectedValue == 0)
            {
                MessageBox.Show("分类汇总导出必须选择类别");
                return;
            }
            CallBack?.Invoke(1, Entity, CheckBoxSelectedValue, CheckBoxColumns);
        }

        private void Exit(object obj)
        {
            CallBack?.Invoke(0, 0,0,null);
        }

        /// <summary>
        /// 第一个参数：保存或者取消，默认都是传1； 
        /// 第二个参数：选中的是哪个radiobutton， 
        /// 第三个参数：基于选中分类导出的基础上， 选择的是哪几个导出类别
        /// 第四个参数：选择类别时选中选择框的顺序
        /// </summary>
        public Action<int, int, int,List<string>> CallBack { get; set; }

        private int entity ;

        public int Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                this.RaisePropertyChanged(nameof(Entity));
            }
        }

        public void Export( Action<int, int,int, List<string>> callBack)
        {   
            CallBack = callBack;
        }
    }
}
