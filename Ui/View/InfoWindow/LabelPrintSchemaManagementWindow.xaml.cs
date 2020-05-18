using Bll.Services;
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
using static Ui.Command.DelegateCommand;

namespace Ui.View
{

    

    /// <summary>
    /// LabelPrintSchemaManagementWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintSchemaManagementWindow : System.Windows.Window
    {
        public event RefreshParentDelegate RefreshEvent;
        private ObservableCollection<QuerySchemaModel> names;
        private UserModel User { get; set; }
        public LabelPrintSchemaManagementWindow(UserModel user)
        {
            InitializeComponent();
            this.User = user;
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); RefreshEvent(); }));
            names = new ObservableCollection<QuerySchemaModel>(new LabelPrintService().GetAllSchemaName(user.ID));
            this.DataContext = names;
        }

        //private void BtnSave_Click(object sender, RoutedEventArgs e)
        //{
        //    var name = this.TbSchemaName.Text;
        //    var item = this.MainDataGrid.SelectedItem as QuerySchemaModel;
        //    if (item == null)
        //    {
        //        MessageBox.Show("请先选择行");
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(this.TbSchemaName.Text))
        //    {
        //        MessageBox.Show("方案名称不能为空");
        //        return;
        //    }

        //    var entry = names.FirstOrDefault(m => m.SchemaName == name);
        //    if (entry != null)
        //    {
        //        MessageBox.Show("方案名称不能重复");
        //        return;
        //    }


        //    // 同步数据库
        //    string r = new LabelPrintService().UpdateQuerySchemaName(item.Id, name);
        //    if (r == null)
        //    {
        //        MessageBox.Show("更新成功");

        //        BindingExpression be1 = TbSchemaName.GetBindingExpression(TextBox.TextProperty);
        //        be1.UpdateSource();
        //    }
        //    else
        //    {
        //        MessageBox.Show(r);
        //    }
        //}

        //private void BtnClear_Click(object sender, RoutedEventArgs e)
        //{
        //    this.TbSchemaName.Text = null;
        //    this.MainDataGrid.SelectedItem = null;
        //}



        //private void BtnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(this.TbSchemaName.Text))
        //    {
        //        MessageBox.Show("方案名称不能为空");
        //        return;
        //    }

        //    // 获取当前第一个间断的序号，保证号码是连续的
        //    int seq = GetContinuousSeq();

        //    var name = this.TbSchemaName.Text;

        //    var entry = names.FirstOrDefault(m => m.SchemaName == name);
        //    if (entry != null)
        //    {
        //        MessageBox.Show("名称不能重复");
        //        return;
        //    }

        //    QuerySchemaModel model = new QuerySchemaModel
        //    {
        //        UserId = User.ID,
        //        SchemaName = name,
        //        SchemaSeq = seq,
        //    };
        //    int id = new LabelPrintService().AddQuerySchemaName(model);
        //    if (id <= 0)
        //    {
        //        MessageBox.Show("添加失败");
        //        return;
        //    }
        //    model.Id = id;
        //    this.names.Insert(seq - 1, model);
        //    this.MainDataGrid.SelectedItem = null;
        //    MessageBox.Show("添加成功");

        //}

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = this.MainDataGrid.SelectedItem as QuerySchemaModel;
            if (item == null)
            {
                MessageBox.Show("请先选择行");
                return;
            }

            MessageBoxResult result = MessageBox.Show("删除方案名称会将对应的查询方案明细同时删除", "温馨提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string r = new LabelPrintService().DeleteQuerySchema(item.Id);
                if (r == null)
                {
                    this.names.Remove(item);
                    this.MainDataGrid.SelectedItem = null;
                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show(r);
                }
            }
        }

        public int GetContinuousSeq()
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i].SchemaSeq != i + 1)
                {
                    return i + 1;
                }
            }
            return names.Count == 0 ? 1 : names.Max(m => m.SchemaSeq) + 1;
        }
    }
}
