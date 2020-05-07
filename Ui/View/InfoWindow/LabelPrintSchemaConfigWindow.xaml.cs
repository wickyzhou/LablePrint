
using Bll.Services;
using Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ui.View.HelpWindow;
using static Ui.Command.DelegateCommand;

namespace Ui.View.InfoWindow
{
    /// <summary>
    /// LabelPrintSchemaConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintSchemaConfigWindow : System.Windows.Window
    {
        public event RefreshParentDelegate RefreshEvent;
        public  UserModel User { get; }
        private static ObservableCollection<QuerySchemaConfigModel> SchemaEntries;

        public LabelPrintSchemaConfigWindow(UserModel user)
        {
            InitializeComponent();
            this.User = user;
            // 获取我的方案
            this.CbRequestName.ItemsSource =new LabelPrintService().GetMySchema(User.ID);
            SchemaEntries = new ObservableCollection<QuerySchemaConfigModel>(new LabelPrintService().GetSchemaEntryByUserId(User.ID).OrderBy(m => m.SchemaSeq));
            this.MainDataGrid.ItemsSource = SchemaEntries;

            //新建window后台代码
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { RefreshEvent(); this.Close();  }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                LabelPrintSchemaConfigHelpWindow window = new LabelPrintSchemaConfigHelpWindow();
                window.Owner = System.Windows.Window.GetWindow(this);
                window.ShowDialog();
            }));
        }

        private void SchemaManager_Click(object sender, RoutedEventArgs e)
        {
            LabelPrintSchemaManagementWindow window = new LabelPrintSchemaManagementWindow(User) {
                Owner = System.Windows.Window.GetWindow(this)
            };
            window.RefreshEvent += () => { this.CbRequestName.ItemsSource = new LabelPrintService().GetMySchema(User.ID); 
                                           this.MainDataGrid.ItemsSource= SchemaEntries = new ObservableCollection<QuerySchemaConfigModel>(new LabelPrintService().GetSchemaEntryByUserId(User.ID)); 
            }; 
            window.ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = this.MainDataGrid.SelectedItem as QuerySchemaConfigModel;
            if (item == null)
            {
                MessageBox.Show("请先选择行");
                return;
            }

                string r = new LabelPrintService().DeleteQuerySchemaEntry(item.Id);
                if (r == null)
                {
                    SchemaEntries.Remove(item);
                 //界面也要移除
                 (this.MainDataGrid.ItemsSource as ObservableCollection<QuerySchemaConfigModel>).Remove(item);
                    this.MainDataGrid.SelectedItem = null;
                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show(r);
                }
 
        }

        private void CbRequestName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded && e.AddedItems.Count>0)
            {
                int selectedValue = (e.AddedItems[0] as QuerySchemaModel).SchemaSeq;
                ObservableCollection<QuerySchemaConfigModel> result = new ObservableCollection<QuerySchemaConfigModel>(SchemaEntries.Where(m=>m.SchemaSeq==selectedValue));
                this.MainDataGrid.ItemsSource = result;
            }
        }
    }
}
