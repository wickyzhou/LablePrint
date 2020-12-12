
using System.Windows;
using System.Windows.Input;
using Bll.Services;
using Model;

namespace Ui.View.Bucket
{
    /// <summary>
    /// BucketModify.xaml 的交互逻辑
    /// </summary>
    public partial class BucketModifyWindow : System.Windows.Window
    {
        public BucketModifyWindow()
        {
            InitializeComponent();
            this.DataContext = new  BucketModifyViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }));
            this.MouseLeftButtonDown += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) this.DragMove(); };
        }

        //private void BtnSave_Click(object sender, RoutedEventArgs e)
        //{

        //    BucketModel bm =this.DG1.DataContext  as BucketModel;
        //   int result= new BucketService().BucketModify(bm);
        //    if (result!=1)
        //    {
        //        MessageBox.Show("更新不止一条数据，请联系管理员");
        //        return;
        //    }
        //    this.Close();
        //}
    }
}
