using Model;
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
using System.Windows.Shapes;
using Ui.ViewModel;

namespace Ui.View.InfoWindow
{
    /// <summary>
    /// SalesRebateAddAndCopyView.xaml 的交互逻辑
    /// </summary>
    public partial class SalesRebateCreateAndCopyView : Window
    {
        public SalesRebateCreateAndCopyView()
        {
            InitializeComponent();
            this.DataContext = new SalesRebateCreateAndCopyViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));

            // CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, .OrgNameSearchCommand));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }));
            this.MouseLeftButtonDown += (sender, e) => { if(e.LeftButton== MouseButtonState.Pressed) this.DragMove(); };

        }


        public string CbText { get; set; } = "";


        private void material_KeyUp(object sender, KeyEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var textBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
            var viewModel = this.DataContext as SalesRebateCreateAndCopyViewModel;
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {

                if (e.Key == Key.Enter)
                {
                    if (viewModel.MaterialSearchedItem != null)
                    {
                        comboBox.IsDropDownOpen = false;
                        //comboBox.Text = viewModel.MaterialSearchedItem.SearchText;
                        //CbText = viewModel.MaterialSearchedItem.SearchText;
                        textBox.Select(0, 0);
                    }
                }
                else if (e.Key == Key.Up || e.Key == Key.Down)
                {
                    if (comboBox.Items.Count > 0)
                    {
                        comboBox.IsDropDownOpen = true;
                        //按键盘上下键选择item,不按确认，又直接输入第二个搜索关键字， 再次按上下键时，会聚焦到下拉框滚动条（上下键会控制滚动条滚动）
                        if (comboBox.SelectedIndex == -1)
                            comboBox.SelectedIndex = 0;

                        //comboBox.Text = viewModel.MaterialSearchedItem.SearchText;
                        //CbText = viewModel.MaterialSearchedItem.SearchText;
                    }
                }
                else
                {
                    if (CbText != textBox.Text)
                    {
                        comboBox.IsDropDownOpen = true;
                        string textShow = textBox.Text;
                        comboBox.ItemsSource = viewModel.ComboBoxSearchService.GetMaterialLists(textShow);
                        comboBox.Text = textShow;
                        CbText = textShow;
                        textBox.Select(textBox.Text.Length, 0);
                    }
                }
            }
        }

    }
}
