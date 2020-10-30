using Bll.Services;
using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ui.View
{
    /// <summary>
    /// LabelPrintSpecialRequestAddRequestNameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintSpecialRequestAddRequestNameWindow : System.Windows.Window
    {
        private  ObservableCollection<CbRequestNameModel> RequestNames;

        public LabelPrintSpecialRequestAddRequestNameWindow()
        {
            InitializeComponent();
            //新建window后台代码
            this.MouseLeftButtonDown += (sender, e) => { if(e.LeftButton== MouseButtonState.Pressed) this.DragMove(); };
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            var enums = new CommonDAL().GetEnumModels();
            RequestNames = new ObservableCollection<CbRequestNameModel>( enums.Where(m => m.GroupSeq == 2).Select(n => new CbRequestNameModel { RequestSeq = n.ItemSeq, RequestName = n.ItemValue }).OrderByDescending(m=>m.RequestSeq));
            this.DataContext = RequestNames;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           var seq= RequestNames.Max(m=>m.RequestSeq)+1;
           var name=  this.TbRequestName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("名称不能为空");
                return;
            }
            var model = new CbRequestNameModel {
                RequestSeq = seq,
                RequestName = name,
            };

            string r = new LabelPrintService().AddRequestName(model);
            if (string.IsNullOrEmpty(r))
            {
                RequestNames.Insert(0,model);
                MessageBox.Show("添加成功");
            }
            else
            {
                MessageBox.Show(r);
            }
         
        }
    }
}
