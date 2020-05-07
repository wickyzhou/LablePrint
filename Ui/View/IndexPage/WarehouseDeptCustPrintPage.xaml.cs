using Bll.Services;
using Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Caching;
using System.Windows;
using System.Windows.Controls;
using Ui.View.PrintPreviewWindow;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// WarehouseDeptCustPrintPage.xaml 的交互逻辑
    /// </summary>
    public partial class WarehouseDeptCustPrintPage : Page
    {
        List<CustomerModel> lists;
        readonly static UserModel user = MemoryCache.Default["user"] as UserModel;
        public WarehouseDeptCustPrintPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region 组织机构
            var org = new OrganizationService().GetOrganization(user.SuperAdmin,user.OrgId);
            this.CbOrganization.DataContext = org;
            this.CbOrganization.ItemsSource = org;
            this.CbOrganization.SelectedIndex = 0;
            #endregion

            #region 打印机配置
            var printers = PrintHelper.GetComputerPrinter();
            this.CbPrinter.ItemsSource = printers;
            var config = new CrystalPrintConfigService().GetCrystalPrintConfig(1, Dns.GetHostName());
            if (config != null)
            {
                string printer = config.PrinterName;
                
                if (!printers.Contains(printer))
                {
                    this.CbPrinter.SelectedIndex = -1;
                    this.CbPaperSize.SelectedIndex = -1;
                }
            }

            this.gpConfig.DataContext = config?? new CrystalPrintConfigModel { MarginRight = 10,MarginLeft = 10,MarginBottom = 0, MarginTop = 0,Orientation= "横向" };

            this.CbOrientation.ItemsSource = new List<string> { "横向", "竖向" };
            #endregion

            #region 列表
            lists = new CustomerService().GetCustomers(Convert.ToInt32((this.CbOrganization.SelectedItem as OrganizationModel).Id));
            this.MainDataGrid.ItemsSource = new ObservableCollection<CustomerModel>(lists);
            #endregion
          
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (this.MainDataGrid.SelectedItems.Count == 1)
            {
                CustomerModel customer = this.MainDataGrid.SelectedItem as CustomerModel;
                CrystalPrintConfigModel validConfig = GetValidationModel();
                if (validConfig != null)
                {
                    DataTable dataTable = UniversalFunction.ModelToTable<CustomerModel>(new List<CustomerModel> { this.MainDataGrid.SelectedItems[0] as CustomerModel });
                    ReportDocument reportDoc = new ReportDocument();

                    string reportDataSource = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Report/Customer.rpt");
                    reportDoc.Load(reportDataSource);
                    reportDoc.SetDataSource(dataTable);



                    ////打印机名称设置
                    //reportDoc.PrintOptions.PrinterName = validConfig.PrinterName;

                    //// 打印方向设置（此处和对应的模板方向一致即可）
                    //if (validConfig.Orientation == "横向")
                    //    reportDoc.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                    //else if (validConfig.Orientation == "竖向")
                    //    reportDoc.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                    //else
                    //    reportDoc.PrintOptions.PaperOrientation = PaperOrientation.DefaultPaperOrientation;

                    ////打印机页面大小设置
                    //PageSettings settings = new PageSettings(new PrinterSettings { PrinterName = validConfig.PrinterName });
                    //var pageSizes = PrintHelper.GetPrinterPageSizes(validConfig.PrinterName);
                    //int rawKind = Convert.ToInt32(this.CbPaperSize.SelectedValue);
                    //reportDoc.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;


                    // 打印机页边距设置
                   //PageMargins margins = reportDoc.PrintOptions.PageMargins;
                    
                   // margins.bottomMargin = validConfig.MarginBottom * 100;
                   // margins.leftMargin = validConfig.MarginLeft * 100;
                   // margins.rightMargin = validConfig.MarginRight * 100;
                   // margins.topMargin = validConfig.MarginTop * 100;
                   // reportDoc.PrintOptions.ApplyPageMargins(margins);     //应用页边距。  


                   
                    PrinterSettings printerSettings = new PrinterSettings();
                    printerSettings.PrinterName= validConfig.PrinterName;
                    PageSettings pageSettings = new PageSettings(printerSettings);
                    pageSettings.Margins = new Margins { Left= validConfig.MarginLeft*1000/254, Top=validConfig.MarginTop * 1000 / 254, Right=validConfig.MarginRight * 1000 / 254, Bottom= validConfig.MarginBottom * 1000 / 254 };
                    pageSettings.Landscape = validConfig.Orientation == "竖向"?true:false;

            
                        // 打印
                        //reportDoc.PrintToPrinter(1, true, 0, 0); 完全按照模板【页面设置】配置参数，自定义需要用重载方法，载入打印机配置。
                    reportDoc.PrintToPrinter(printerSettings, pageSettings, true);
                    reportDoc.Close();
                    reportDoc.Dispose();
                    MessageBox.Show(" 打印成功 ");
                }
                else
                {
                    MessageBox.Show(" 打印参数错误 ");
                }

            }
            else
            {
                MessageBox.Show("先选择一条需要打印的记录");
            }

        }

        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            this.MainDataGrid.ItemsSource = GetFilteredItemSource();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            CrystalPrintConfigModel validConfig = GetValidationModel();
            if (validConfig != null)
            {
                var model = this.gpConfig.DataContext as CrystalPrintConfigModel;
                if (model.Id==0) // 插入打印配置
                {
                    string result = new CrystalPrintConfigService().AddConfig(validConfig);
                    MessageBox.Show(result);
                }
                else // 更新打印配置
                {
                    validConfig.Id = model.Id;
                    string result = new CrystalPrintConfigService().ModifyConfig(validConfig);
                    MessageBox.Show(result);

                }
                this.gpConfig.DataContext = new CrystalPrintConfigService().GetCrystalPrintConfig(1, Dns.GetHostName());
            }
            else
            {
                MessageBox.Show(" 参数录入不合法 ");
            }
        }

        private void CbOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded && e.AddedItems.Count > 0)
            {
                this.tbCompanyCode.Text = null;
                this.tbCompanyName.Text = null;
                int orgId = (e.AddedItems[0] as OrganizationModel).Id;
                this.MainDataGrid.ItemsSource = new ObservableCollection<CustomerModel>(new CustomerService().GetCustomers(orgId));
            }
        }

        public ObservableCollection<CustomerModel> GetFilteredItemSource()
        {

            var expression = GetFilterExpression<CustomerModel>(new List<CustomerModel> {
                new CustomerModel{
                    CustCode=this.tbCompanyCode.Text.ToUpper(),
                    CustName=this.tbCompanyName.Text.ToUpper()
                }
            });
            if (expression != null)
            {
                return new ObservableCollection<CustomerModel>(lists.AsQueryable().Where(expression));
            }
            return new ObservableCollection<CustomerModel>(lists);
        }

        public static Expression<Func<T, bool>> GetFilterExpression<T>(List<CustomerModel> entries)
        {
            Expression<Func<T, bool>> condition = null;
            try
            {
                if (entries != null && entries.Count > 0)
                {
                    foreach (CustomerModel entryModel in entries)
                    {
                        Expression<Func<T, bool>> tempCondition = CreateLambda<T>(entryModel);
                        condition = condition == null ? tempCondition : condition.Or(tempCondition);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return condition;
        }

        public static Expression<Func<T, bool>> CreateLambda<T>(CustomerModel entryModel)
        {
            Expression<Func<T, bool>> condition = null;

            if (!string.IsNullOrEmpty(entryModel.CustCode))
                condition = UniversalFunction.AndCondition("CustCode", entryModel.CustCode, condition);

            if (!string.IsNullOrEmpty(entryModel.CustName))
                condition = UniversalFunction.AndCondition("CustName", entryModel.CustName, condition);

            return condition;
        }

        private CrystalPrintConfigModel GetValidationModel()
        {
            if (!int.TryParse(this.tbLeft.Text, out int left))
                return null;
            if (!int.TryParse(this.tbTop.Text, out int top))
                return null;
            if (!int.TryParse(this.tbRight.Text, out int right))
                return null;
            if (!int.TryParse(this.tbBottom.Text, out int bottom))
                return null;
            if (string.IsNullOrEmpty(this.CbOrientation.Text))
                return null;
            if (string.IsNullOrEmpty(this.CbPaperSize.Text))
                return null;
            if (string.IsNullOrEmpty(this.CbPrinter.Text))
                return null;

            return new CrystalPrintConfigModel
            {
                MarginLeft = left,
                MarginTop = top,
                MarginRight = right,
                MarginBottom = bottom,
                Orientation = this.CbOrientation.Text,
                PaperName=this.CbPaperSize.Text,
                PrinterName = this.CbPrinter.Text,
                HostName = Dns.GetHostName(),
                TypeId = 1,
                TypeDesciption = "仓库信封打印"
            };
        }

        private void BtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            if (this.MainDataGrid.SelectedItems.Count > 0)
            {
                CustPreviewWindow window = new CustPreviewWindow(this.MainDataGrid.SelectedItems[0] as CustomerModel);
                window.Owner = System.Windows.Window.GetWindow(this);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show(" 请先选择打印的行 ");
            }
        }

        private void CbPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded && e.AddedItems.Count > 0)
            {
                this.CbPaperSize.ItemsSource = PrintHelper.GetPrinterPageSizes(e.AddedItems[0].ToString()); 
            }
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var model = (sender as DataGridRow).Item as CustomerModel;
            model.Selected = !model.Selected;
        }
    }
}
