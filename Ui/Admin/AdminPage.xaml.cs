using Common;
using CRMApiModel.Response;
using K3ApiModel;
using K3ApiModel.PurchaseRequisition;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Ui.MVVM.View;
using Ui.Service;
using Ui.ViewModel.IndexPage;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// AdminPage.xaml 的交互逻辑
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            this.DataContext = new AdminPageViewModel();
            //new CommonService().GetDataGridColumnHeaderDefault(this.DGActionOperationLog,1);
            this.MultiDataGrid.ItemsSource = new List<BaseNumberNameModelX>() 
            { 
                new BaseNumberNameModelX { FName = "111", FNumber = "aaa" }, 
                new BaseNumberNameModelX { FName = "222", FNumber = "bbb" }, 
                new BaseNumberNameModelX { FName = "333", FNumber = "ccc" }, 
                new BaseNumberNameModelX { FName = "444", FNumber = "ddd" }, 
                new BaseNumberNameModelX { FName = "555", FNumber = "eee" }
            };
        }


        private void BtnDemo_Click(object sender, RoutedEventArgs e)
        {
            DemoMainWindow window = new DemoMainWindow()
            {
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow window = new UserManagementWindow()
            {
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void BtnPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn21_Click(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1()
            {
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void ShowJson_Click(object sender, RoutedEventArgs e)
        {
            PurchaseRequisitionMainModel main = new PurchaseRequisitionMainModel()
            {
                FHeadSelfP0131 = new BaseNumberNameModel() { FNumber = "FSQLX02", FName = "生产部" },
                FPlanCategory = new BaseNumberNameModel() { FNumber = "STD", FName = "标准" },
                FBizType = new BaseNumberNameModel() { FNumber = "FPLX01", FName = "外购入库" },
                FDeptID = new BaseNumberNameModel() { FNumber = "05.04.02", FName = "生产制造组" },
                //FCheckerID = new BaseNumberNameModel(),
                FSelTranType = new BaseNumberNameModel() { FNumber = "81", FName = "销售订单" },
                FRequesterID = new BaseNumberNameModel() { FNumber = "22", FName = "付子明" },
                Fdate = DateTime.Now.Date.ToString("yyyy-MM-dd")
            };
            PurchaseRequisitionSonModel son = new PurchaseRequisitionSonModel()
            {
                //FItemID = 681,//new BaseNumberNameModel(),
                //FAuxPropID = 0,// new BaseNumberNameModel(),
                //FUnitID = 287,//new BaseNumberNameModel(),
                //FBomInterID = 0,//new BaseNumberNameModel(),
                //FSupplyID = 0,//new BaseNumberNameModel(),
                //FPlanMode = 14036//new BaseNumberNameModel()
            };
            //PurchaseRequisitionApiModel model = new PurchaseRequisitionApiModel() {Data = new PurchaseRequisitionDataModel() {Page1= new PurchaseRequisitionMainModel[] { main } ,Page2= new PurchaseRequisitionSonModel[] { son } } }; 
            //this.RtbJson.AppendText(JsonHelper.ObjectToJson(model));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (BaseNumberNameModelX item in this.MultiDataGrid.SelectedItems)
            {
                MessageBox.Show($"{item.FName} \t {item.FNumber}");
            }  
        }

        private void MultiDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
                return;
            var ss = sender as DataGrid;
            var ss1 = ss.CurrentItem;
            //MessageBox.Show($"{((sender as DataGrid).CurrentItem as BaseNumberNameModelX).FName} \t {((sender as DataGrid).CurrentItem as BaseNumberNameModelX).FNumber}");
        }

        private void JsonToModel_Click(object sender, RoutedEventArgs e)
        {
            string json = @"{
  ""code"": 200,
  ""msg"": ""OK"",
  ""ext"": [],
  ""result"": {
    ""apiKey"": ""opportunity"",
    ""custom"": false,
    ""label"": ""销售机会"",
    ""disabled"": true,
    ""createable"": true,
    ""deletable"": true,
    ""updateable"": true,
    ""queryable"": true,
    ""feedEnabled"": true,
  ""fields"":[ {
        ""apiKey"": ""id"",
        ""label"": ""id"",
        ""type"": ""id"",
        ""itemType"": ""Long"",
        ""defaultValue"": null,
        ""enabled"": true,
        ""required"": false,
        ""createable"": false,
        ""updateable"": false,
        ""sortable"": true,
        ""minLength"": 0,
        ""maxLength"": 20,
        ""dependentPropertyName"": null,
        ""referTo"": {},
        ""joinTo"": {},
        ""selectitem"": [],
        ""checkitem"": []
      }],
     ""entityTypes"": [
      {
        ""id"": 1417431,
        ""label"": ""前期开发项目"",
        ""apiKey"": ""defaultBusiType_1"",
        ""disabled"": false,
        ""default"": true,
        ""description"": """"
      },
      {
        ""id"": 1417873,
        ""label"": ""模厂项目"",
        ""apiKey"": ""defaultBusiType_2"",
        ""disabled"": false,
        ""default"": false,
        ""description"": """"
      }
        ]
  }}";
           var sss= JsonHelper.DeserializeObject<EntityDescriptionResponseModel>(json);
        }
    }
}
