using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Ui.Extension
{
    public class SearchComboBox : ComboBox
    {
        private static readonly Type _ownerType = typeof(SearchComboBox);
        private TextBox textBox;

        public string CbText { get; set; } = "";

        public IEnumerable DataSource
        {
            get { return (IEnumerable)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register(nameof(DataSource), typeof(IEnumerable), _ownerType, new PropertyMetadata(Enumerable.Empty<object>()));

        public SearchComboBox()
        {
            IsEditable = true;
            StaysOpenOnEdit = true;
            IsTextSearchEnabled = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textBox = GetTemplateChild("PART_EditableTextBox") as TextBox;

        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (e.Key == Key.Enter)
                {
                    if ((SelectedItem as ComboBoxSearchModel) != null)
                    {
                        IsDropDownOpen = false;
                        Text = (SelectedItem as ComboBoxSearchModel).SearchText;
                        CbText = (SelectedItem as ComboBoxSearchModel).SearchText;
                        textBox.Select(textBox.Text.Length, 0);
                        if ((SelectedItem as ComboBoxSearchModel) != null)
                        {
                            Console.WriteLine($"{ (SelectedItem as ComboBoxSearchModel).SearchText} \t {CbText}  \t {textBox.Text}");
                        }
}
                }
                else if (e.Key == Key.Up || e.Key == Key.Down)
                {
                  
                    if (Items.Count > 0)
                    {
                        IsDropDownOpen = true;
                        //按键盘上下键选择item,不按确认，又直接输入第二个搜索关键字， 再次按上下键时，会聚焦到下拉框滚动条（上下键会控制滚动条滚动）
                        if (SelectedIndex == -1)
                        SelectedIndex =  0;

                        Text = (SelectedItem as ComboBoxSearchModel).SearchText;
                        CbText = (SelectedItem as ComboBoxSearchModel).SearchText;
                        if ((SelectedItem as ComboBoxSearchModel) != null)
                        {
                            Console.WriteLine($"{ (SelectedItem as ComboBoxSearchModel).SearchText} \t {CbText}  \t {textBox.Text}");
                        }
                    }
                }
                //else if (e.Key == Key.Back)
                //{
                //    IsDropDownOpen = true;
                //    string textShow = textBox.Text;
                //    var searchedItemList = new List<object>();
                //    int i = 0;
                //    foreach (var item in DataSource)
                //    {
                //        if (i > 29)
                //            break;
                //        var text = item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();
                //        if (text.IndexOf(textShow.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                //        {
                //            i++;
                //            searchedItemList.Add(item);
                //        }
                //    }
                //    ItemsSource = searchedItemList;

                //    textBox.Text = textShow;
                //    CbText = textShow;
                //    textBox.Select(textBox.Text.Length, 0);
                //    if ((SelectedItem as ComboBoxSearchModel) != null)
                //    {
                //        Console.WriteLine((SelectedItem as ComboBoxSearchModel).SearchText);
                //    }
                //}
                else
                {
                    if (CbText != textBox.Text)
                    {
                        IsDropDownOpen = true;
                        string textShow = textBox.Text;
                       
                        var searchedItemList = new List<object>();
                        int i = 0;
                        foreach (var item in DataSource)
                        {
                            if (i > 29)
                                break;
                            var text = item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();
                            if (text.IndexOf(textShow.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                i++;
                                searchedItemList.Add(item);
                            }
                        }
                        SelectedItem = null;
                        ItemsSource = searchedItemList;
                        Text = textShow;
                        CbText = textShow;
                        textBox.Select(textBox.Text.Length, 0);
                        if ((SelectedItem as ComboBoxSearchModel)!=null)
                        {
                            Console.WriteLine($"{ (SelectedItem as ComboBoxSearchModel).SearchText} \t {CbText}  \t {textBox.Text}");
                        }
                     
                    }
                }
            }

    }
        //protected override void OnPreviewKeyUp(KeyEventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(_editableTextBox.Text))
        //    {

        //        if (e.Key == Key.Enter)
        //        {
        //            if (SelectedItem != null)
        //            {
        //                IsDropDownOpen = false;
        //                _editableTextBox.Text = (SelectedItem as ComboBoxSearchModel).SearchText;
        //                CbText = (SelectedItem as ComboBoxSearchModel).SearchText;
        //                _editableTextBox.Select(_editableTextBox.Text.Length, 0);
        //            }
        //        }
        //        //else if (e.Key == Key.Up || e.Key == Key.Down)
        //        //{
        //        //    var s = SelectedItem;
        //        //    if (Items.Count > 0)
        //        //    {
        //        //        IsDropDownOpen = true;
        //        //        if (SelectedIndex == -1)
        //        //            SelectedIndex = 0;
        //        //        _editableTextBox.Text = (SelectedItem as ComboBoxSearchModel).SearchText;
        //        //        CbText = (SelectedItem as ComboBoxSearchModel).SearchText;
        //        //    }
        //        //    e.Handled = true;
        //        //}
        //        //else if (e.Key == Key.Back)
        //        //{
            
        //        //    IsDropDownOpen = true;
        //        //    string textShow = _editableTextBox.Text;
        //        //    var searchedItemList = new List<object>();
        //        //    int i = 0;
        //        //    foreach (var item in DataSource)
        //        //    {
        //        //        if (i > 29)
        //        //            break;
        //        //        var text = item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();
        //        //        if (text.IndexOf(Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
        //        //        {
        //        //            i++;
        //        //            searchedItemList.Add(item);
        //        //        }
        //        //    }
        //        //    ItemsSource = searchedItemList;
        //        //    _editableTextBox.Text = textShow;
        //        //    CbText = textShow;
        //        //    _editableTextBox.Select(_editableTextBox.Text.Length, 0);
        //        //    e.Handled = true;
        //        //}
        //        else
        //        {
        //            if (CbText != _editableTextBox.Text)
        //            {
        //                var s1 = SelectedItem;
        //                IsDropDownOpen = true;
        //                string textShow = _editableTextBox.Text;
        //                var searchedItemList = new List<object>();
        //                int i = 0;
        //                foreach (var item in DataSource)
        //                {
        //                    if (i > 29)
        //                        break;
        //                    var text = item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();
        //                    if (text.IndexOf(textShow.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
        //                    {
        //                        i++;
        //                        searchedItemList.Add(item);
        //                    }
        //                }
        //                ItemsSource = searchedItemList;
        //                if (searchedItemList.Count()>0)
        //                {
        //                    SelectedIndex = 0;
        //                }
        //                _editableTextBox.Text = textShow;
        //                CbText = textShow;
        //                _editableTextBox.Select(_editableTextBox.Text.Length, 0);
        //                e.Handled = true;
        //            }
        //        }

        //        // base.OnPreviewKeyUp(e);
        //    }
        //}
    }
}
