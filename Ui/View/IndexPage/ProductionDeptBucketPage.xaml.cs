using Bll.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ui.View.Bucket;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// ProductionDeptBucket.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionDeptBucketPage : Page
    {
        public ProductionDeptBucketPage()
        {
            InitializeComponent();
            this.DG1.ItemsSource = new BucketService().GetAllBucket();
            this.MainGrid.Height = SystemParameters.PrimaryScreenHeight - 160;
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            //ProductionDeptBucketModifyPage page = new ProductionDeptBucketModifyPage();
            //((UserMainWindow)Window.GetWindow(this)).SubFrame.Content= page;
            ////.Navigate(new Uri("/View/Pages/ProductionDeptBucketModifyPage.xaml", UriKind.Relative));
            ////

            //page.DG1.DataContext = DG1.SelectedItem;
            ////(this.Parent as Window).subFrame.Navigate(new Uri("/View/Pages/ProductionDeptBucketModifyPage.xaml", UriKind.Relative));

            if (DG1.SelectedItem != null)
            {
                BucketModifyWindow bucketModify = new BucketModifyWindow();
                bucketModify.DG1.DataContext = DG1.SelectedItem;
                bucketModify.ShowDialog();
            }
        }
    }
}
