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
using Ui.View.HelpWindow;
using static Ui.Command.DelegateCommand;

namespace Ui.View
{
    /// <summary>
    /// LabelPrintSpecialRequestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintSpecialRequestWindow : System.Windows.Window
    {
        private static ObservableCollection<LabelPrintSpecialRequestModel> SpecialRequests;
        private IEnumerable<CbIdentTypeModel> IdentTypes;
        private IEnumerable<CbRequestNameModel> RequestNames;
        private bool isRefreshParentDataGrid = false;//是否需要刷新标签打印界面表格
        public event RefreshParentDelegate RefreshEvent;

        public LabelPrintSpecialRequestWindow()
        {
            InitializeComponent();
            SpecialRequests = new ObservableCollection<LabelPrintSpecialRequestModel>(new LabelPrintService().GetAllSpecialRequest());
            DataContext = SpecialRequests;

            var enums = new CommonDAL().GetEnumModels();
            IdentTypes = enums.Where(m => m.GroupSeq == 1).Select(n => new CbIdentTypeModel { IdentityType = n.ItemSeq, IdentityTypeDesc = n.ItemValue });
            RequestNames = enums.Where(m => m.GroupSeq == 2).Select(n => new CbRequestNameModel { RequestSeq = n.ItemSeq, RequestName = n.ItemValue });
            this.CbRequestName.ItemsSource = RequestNames;
            this.CbIdentType.ItemsSource = IdentTypes;

            //新建window后台代码
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
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
                LabelPrintSpecialRequestHelpWindow window = new LabelPrintSpecialRequestHelpWindow();
                window.Owner = System.Windows.Window.GetWindow(this);
                window.ShowDialog();
            }));

            this.TbLabel.IsEnabled = false;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // 输入数据验证
            var model = ADDVerificationInput();
            if (model != null)
            {
                // 同步数据库
                int id = new LabelPrintService().AddSpecialRequest(model);
                if (id <= 0)
                {
                    MessageBox.Show("添加失败");
                    return;
                }
                model.ID = id;
                // 更改界面
                (this.MainDataGrid.ItemsSource as ObservableCollection<LabelPrintSpecialRequestModel>).Insert(0, model);
                SpecialRequests.Insert(0, model);
                this.MainDataGrid.SelectedItem = null;
                isRefreshParentDataGrid = true;
                MessageBox.Show("添加成功");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var item = this.MainDataGrid.SelectedItem as LabelPrintSpecialRequestModel;
            if (item == null)
            {
                MessageBox.Show("请先选择行");
                return;
            }

            int id = item.ID;
            string value = this.TbRequestValue.Text;
            string label = null;
            if (item.IdentityType == 1)
            {
                label = this.TbLabel.Text;
                // 如果客户和标签存在则返回
                var custLabel = new LabelPrintService().GetAllSpecialRequest().Where(m => m.ID != id && m.OrgID.ToUpper() == item.OrgID.ToUpper() && m.Label?.ToUpper() == label.ToUpper());
                if (custLabel.Count() > 0)
                {
                    MessageBox.Show("该客户标签已存在");
                    return;
                }
            }

            // 同步数据库
            string r = new LabelPrintService().UpdateLabelPrintSpecialRequestModel(id, value, label);
            if (r == null)
            {
                MessageBox.Show("更新成功");

                BindingExpression be1 = TbRequestValue.GetBindingExpression(TextBox.TextProperty);
                if (item.IdentityType == 1)
                {
                    BindingExpression be2 = TbLabel.GetBindingExpression(TextBox.TextProperty);
                    be2.UpdateSource();
                }
                be1.UpdateSource();
                isRefreshParentDataGrid = true;
                return;
            }
            else
            {
                MessageBox.Show(r);
                return;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as LabelPrintSpecialRequestModel;
            if (item == null)
            {
                MessageBox.Show("请先选择行");
                return;
            }
            string r = new LabelPrintService().DeleteSpecialRequest(item.ID);
            if (string.IsNullOrEmpty(r))
            {
                (this.MainDataGrid.ItemsSource as ObservableCollection<LabelPrintSpecialRequestModel>).Remove(item);
                SpecialRequests.Remove(item);
                this.MainDataGrid.SelectedItem = null;
                isRefreshParentDataGrid = true;
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show(r);
            }

        }

        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            var result = SpecialRequests as IEnumerable<LabelPrintSpecialRequestModel>;

            if (!string.IsNullOrEmpty(this.TbOrgID.Text))
                result = result.Where(m => m.OrgID.ToUpper().Contains(this.TbOrgID.Text.ToUpper()));

            if (!string.IsNullOrEmpty(this.CbRequestName.Text))
            {
                result = result.Where(m => m.RequestName.ToUpper() == this.CbRequestName.Text.ToUpper());
            }
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
            this.MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintSpecialRequestModel>(result);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            SpecialRequests = new ObservableCollection<LabelPrintSpecialRequestModel>(new LabelPrintService().GetAllSpecialRequest());
            this.MainDataGrid.ItemsSource = SpecialRequests;
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

        private void BtnAddRequestName_Click(object sender, RoutedEventArgs e)
        {
            LabelPrintSpecialRequestAddRequestNameWindow window = new LabelPrintSpecialRequestAddRequestNameWindow
            {
                Owner = System.Windows.Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            this.CbIdentType.SelectedIndex = -1;
            this.CbRequestName.SelectedIndex = -1;
            this.MainDataGrid.SelectedItem = null;
            this.TbLabel.IsEnabled = false;
        }


        private LabelPrintSpecialRequestModel ADDVerificationInput()
        {

            var model = CommonVerificationInput();
            if (model != null)
            {
                SpecialRequests = new ObservableCollection<LabelPrintSpecialRequestModel>(new LabelPrintService().GetAllSpecialRequest());
                var org = SpecialRequests.Where(m => m.OrgID.ToUpper() == model.OrgID.ToUpper());

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
                    var custlabel = org.FirstOrDefault(m => m.Label.ToUpper() == model.Label.ToUpper());
                    if (custlabel != null)
                    {
                        MessageBox.Show("该客户和标签已存在");
                        return null;
                    }
                }
            }
            return model;
        }

        private LabelPrintSpecialRequestModel CommonVerificationInput()
        {
            int.TryParse(this.TbID.Text, out int id);
            var identityTypeDesc = this.CbIdentType.Text;
            var requestName = this.CbRequestName.Text;
            if (string.IsNullOrEmpty(identityTypeDesc) || string.IsNullOrEmpty(requestName))
            {
                MessageBox.Show("必须选择类型和名称");
                return null;
            }
            int identityType = IdentTypes.FirstOrDefault(m => m.IdentityTypeDesc.ToUpper() == identityTypeDesc.ToUpper()).IdentityType;
            int requestSeq = RequestNames.FirstOrDefault(m => m.RequestName.ToUpper() == requestName.ToUpper()).RequestSeq;

            string orgID = this.TbOrgID.Text;
            string label = this.TbLabel.Text;
            if (identityType == 1 && string.IsNullOrWhiteSpace(label))
            {
                MessageBox.Show("此类型必须填标签型号");
                return null;
            }

            var requestValue = !string.IsNullOrEmpty(this.TbRequestValue.Text) ? this.TbRequestValue.Text : null;


            if (string.IsNullOrEmpty(orgID))
            {
                MessageBox.Show("客户不能为空");
                return null;
            }

            return new LabelPrintSpecialRequestModel
            {
                ID = id,
                OrgID = orgID,
                Label = label,
                IdentityType = identityType,
                IdentityTypeDesc = identityTypeDesc,
                RequestName = requestName,
                RequestSeq = requestSeq,
                RequestValue = requestValue
            };
        }
    }
}
