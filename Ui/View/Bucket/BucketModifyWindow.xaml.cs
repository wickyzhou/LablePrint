
using System.Windows;
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
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            BucketModel bm =this.DG1.DataContext  as BucketModel;
           int result= new BucketService().BucketModify(bm);
            if (result!=1)
            {
                MessageBox.Show("更新不止一条数据，请联系管理员");
                return;
            }
            this.Close();
        }
    }
}
