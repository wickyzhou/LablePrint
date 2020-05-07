using Bll.Services;
using System.Windows;

namespace Ui.View.Bucket
{
    /// <summary>
    /// PackageBucketInfo.xaml 的交互逻辑
    /// </summary>
    public partial class BucketInfoWindow : System.Windows.Window
    {
        public BucketInfoWindow()
        {   
            InitializeComponent();
            this.DG1.ItemsSource = new BucketService().GetAllBucket();
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            if (DG1.SelectedItem!=null)
            {
                BucketModifyWindow bucketModify = new BucketModifyWindow();
                bucketModify.DG1.DataContext = DG1.SelectedItem;
                bucketModify.ShowDialog();
            }
        }

        private void BtnSync_Click(object sender, RoutedEventArgs e)
        {
            int result= new BucketService().SyncBucketInfo();
            MessageBox.Show($"成功同步 {result} 条信息");
            this.DG1.ItemsSource = new BucketService().GetAllBucket();
        }
    }
}
