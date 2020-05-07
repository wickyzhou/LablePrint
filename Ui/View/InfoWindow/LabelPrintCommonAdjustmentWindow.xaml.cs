using Bll.Services;
using Common;
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
using Ui.View.HelpWindow;
using Ui.ViewModel;
using static Ui.Command.DelegateCommand;

namespace Ui.View
{
    /// <summary>
    /// LabelPrintCommonAdjustmentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintCommonAdjustmentWindow : System.Windows.Window
    {
        private readonly UserModel user;
        private static ObservableCollection<LabelPrintCommonAdjustmentModel> CommonAdjustments;
        private IEnumerable<CbIdentTypeModel> IdentTypes;
        private bool isRefreshParentDataGrid = false;
        public event RefreshParentDelegate RefreshEvent;

        public LabelPrintCommonAdjustmentWindow(UserModel user)
        {
            InitializeComponent();
            this.user = user;
            CommonAdjustments = new ObservableCollection<LabelPrintCommonAdjustmentModel>(new LabelPrintService().GetAllLabelPrintCommonAdjustment());
            IdentTypes = new CommonDAL().GetEnumModels().Where(m => m.GroupSeq == 1).Select(n => new CbIdentTypeModel { IdentityType = n.ItemSeq, IdentityTypeDesc = n.ItemValue });
            DataContext = CommonAdjustments;
            this.DGViewModel.ItemsSource = CommonAdjustments;
            this.CbIdentType.ItemsSource = IdentTypes;

            //新建window后台代码
            this.MouseLeftButtonDown  += (sender, e) => { this.DragMove(); };
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) =>
             {
                 this.Close();
                 if (isRefreshParentDataGrid)
                 {
                     //重新生成数据，并且绑定到父窗口
                     new LabelPrintService().ReGenPrintData();
                     RefreshEvent();
                 }
             }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                LabelPrintCommonAdjustmentHelpWindow window = new LabelPrintCommonAdjustmentHelpWindow();
                window.Owner = System.Windows.Window.GetWindow(this);
                window.ShowDialog();
            }));
        }

        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            var result = CommonAdjustments as IEnumerable<LabelPrintCommonAdjustmentModel>;
            if (!string.IsNullOrEmpty(this.TbOrgID.Text))
                result = result.Where(m => m.OrgID.ToUpper().Contains(this.TbOrgID.Text.ToUpper()));

            if (!string.IsNullOrEmpty(this.CbIdentType.Text))
            {
                if (this.CbIdentType.Text == "客户")
                {
                    result = result.Where(m => m.IdentityType == 2);
                }
                else
                {
                    result = result.Where(m => m.IdentityType == 1);
                    if (!string.IsNullOrEmpty(this.TbLabel.Text))
                        result = result.Where(m => m.Label.ToUpper().Contains(this.TbLabel.Text.ToUpper()));
                }
            }
            this.DGViewModel.ItemsSource = new ObservableCollection<LabelPrintCommonAdjustmentModel>(result);

        }

        private void CbIdentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (this.IsLoaded)
            {
                bool iscust = true;
                if (e.AddedItems.Count > 0)
                {
                    iscust = (e.AddedItems[0] as CbIdentTypeModel).IdentityTypeDesc == "客户";
                }
                else
                {
                    iscust = this.CbIdentType.Text == "客户";
                }
                this.TbLabel.IsEnabled = !iscust;
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            CommonAdjustments = new ObservableCollection<LabelPrintCommonAdjustmentModel>(new LabelPrintService().GetAllLabelPrintCommonAdjustment());
            this.DGViewModel.ItemsSource = CommonAdjustments;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // 输入数据验证
            var model = ADDVerificationInput();
            if (model != null)
            {
                // 同步数据库
                int id = new LabelPrintService().AddCommonAdjustment(model);
                model.ID = id;

                // 更改界面
                (this.DGViewModel.ItemsSource as ObservableCollection<LabelPrintCommonAdjustmentModel>).Insert(0, model);
                CommonAdjustments.Insert(0, model);
                this.DGViewModel.SelectedItem = null;
                isRefreshParentDataGrid = true;
                MessageBox.Show("添加成功");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var item = DGViewModel.SelectedItem as LabelPrintCommonAdjustmentModel;
            if (item == null)
            {
                MessageBox.Show("请先选择行");
                return;
            }

            // 输入验证
            var model = CommonVerificationInput();
            if (model != null)
            {
                model.ID = item.ID;
                if (item.IdentityType != model.IdentityType || item.OrgID.ToUpper() != model.OrgID.ToUpper())
                {
                    MessageBox.Show("类型和客户编号不能修改");
                    return;
                }


                // 同步数据库
                int row = new LabelPrintService().UpdateCommonAdjustment(model);
                if (row != 1)
                {  
                    MessageBox.Show("数据已更改，操作失败");
                    return;
                }
                UpdateSource();
                isRefreshParentDataGrid = true;
                MessageBox.Show("修改成功");
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = DGViewModel.SelectedItem as LabelPrintCommonAdjustmentModel;
            if (item == null)
            {
                MessageBox.Show("请先选择行");
                return;
            }
            string r = new LabelPrintService().DeleteCommonAdjustment(item.ID);
            (this.DGViewModel.ItemsSource as ObservableCollection<LabelPrintCommonAdjustmentModel>).Remove(item);
            CommonAdjustments.Remove(item);
            this.DGViewModel.SelectedItem = null;
            isRefreshParentDataGrid = true;
            MessageBox.Show(r);
        }

        private LabelPrintCommonAdjustmentModel ADDVerificationInput()
        {

            var model = CommonVerificationInput();
            if (model != null)
            {
                CommonAdjustments = new ObservableCollection<LabelPrintCommonAdjustmentModel>(new LabelPrintService().GetAllLabelPrintCommonAdjustment());
                var org = CommonAdjustments.Where(m => m.OrgID == model.OrgID);

                if (org.Count() > 0)
                {
                    // 同一个客户只能有一个类型
                    int currentType = org.FirstOrDefault().IdentityType;
                    if (currentType > 0 && currentType != model.IdentityType)
                    {
                        MessageBox.Show("同一个客户只能有一个类型");
                        return null;
                    }

                    // 同一个客户只能有一个类型和标签
                    var custlabel = org.FirstOrDefault(m => m.Label == model.Label);
                    if (custlabel != null)
                    {
                        MessageBox.Show("该客户和标签已存在");
                        return null;
                    }
                }
            }
            return model;
        }

        private LabelPrintCommonAdjustmentModel CommonVerificationInput()
        {
            int.TryParse(this.TbID.Text, out int id);
            var identityTypeDesc = this.CbIdentType.Text;
            if (string.IsNullOrEmpty(identityTypeDesc))
            {
                MessageBox.Show("必须选择类型");
                return null;
            }
            var identityType = IdentTypes.FirstOrDefault(m => m.IdentityTypeDesc == identityTypeDesc).IdentityType;

            var orgID = this.TbOrgID.Text;
            var label = this.TbLabel.Text;
            var productionName = this.TbProductionName.Text;
            bool r1 = UniversalFunction.ConvertStringToNullableInt(this.TbMonth.Text, out int? expirationMonth);
            bool r2 = UniversalFunction.ConvertStringToNullableInt(this.TbDay.Text, out int? expirationDay);
            string netWeight = this.TbNetWeight.Text;

            if (!r1 || !r2 || expirationMonth < 0 || expirationMonth > 36 || expirationDay < 0 || expirationDay > 13140)
            {
                MessageBox.Show("请输入正确的月份或天数");
                return null;
            }

            if (string.IsNullOrEmpty(identityTypeDesc) || string.IsNullOrEmpty(orgID))
            {
                MessageBox.Show("类型和客户不能为空");
                return null;
            }



            return new LabelPrintCommonAdjustmentModel
            {
                ID = id,
                OrgID = orgID,
                Label = label,
                ProductionName = productionName,
                ExpirationMonth = expirationMonth,
                ExpirationDay = expirationDay,
                IdentityType = identityType,
                IdentityTypeDesc = identityTypeDesc,
                NetWeight= netWeight
            };
        }

        private void UpdateSource()
        {
            // 更新数据到界面 
            BindingExpression be1 = TbOrgID.GetBindingExpression(TextBox.TextProperty);
            BindingExpression be2 = TbLabel.GetBindingExpression(TextBox.TextProperty);
            BindingExpression be3 = TbProductionName.GetBindingExpression(TextBox.TextProperty);
            BindingExpression be4 = TbMonth.GetBindingExpression(TextBox.TextProperty);
            BindingExpression be5 = TbDay.GetBindingExpression(TextBox.TextProperty);
            BindingExpression be6 = CbIdentType.GetBindingExpression(ComboBox.TextProperty);
            BindingExpression be7 = TbNetWeight.GetBindingExpression(TextBox.TextProperty);
            be1.UpdateSource(); be2.UpdateSource(); be3.UpdateSource(); be4.UpdateSource(); be5.UpdateSource(); be6.UpdateSource(); be7.UpdateSource();
        }

        private void DGViewModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CbIdentType.Text == "客户")
            {
                this.TbLabel.IsEnabled = false;
            }
            else
            {
                this.TbLabel.IsEnabled = true;
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            this.TbID.Text = null;
            this.TbProductionName.Text = null;
            this.TbOrgID.Text = null;
            this.TbLabel.Text = null;
            this.TbMonth.Text = null;
            this.TbDay.Text = null;
            this.CbIdentType.SelectedIndex = -1;
            this.DGViewModel.SelectedItem = null;
            this.CbIdentType.Focus();
        }

        //private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    base.DragMove();
        //}
    }
}
