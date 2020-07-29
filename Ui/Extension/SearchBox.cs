using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ui.Extension
{
    public class SearchBox : ComboBox
    {
        private static readonly Type _ownerType = typeof(SearchBox);
        private TextBox _editableTextBox;

        public IEnumerable DataSource
        {
            get { return (IEnumerable)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register(nameof(DataSource), typeof(IEnumerable), _ownerType, new PropertyMetadata(Enumerable.Empty<object>()));

        public SearchBox()
        {
            IsEditable = true;
            StaysOpenOnEdit = true;
            IsTextSearchEnabled = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _editableTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
        }


        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            Search();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.OriginalSource == _editableTextBox)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        IsDropDownOpen = !IsDropDownOpen;
                        e.Handled = true;
                        return;
                    case Key.Down:
                        if (SelectedItem == null)
                        {
                            var enumerator = ItemsSource.GetEnumerator();
                            if (enumerator.MoveNext())
                            {
                                SelectedItem = enumerator.Current;
                                e.Handled = true;
                                return;
                            }
                        }
                        break;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            switch (e.Key)
            {
                case Key.Back:
                    Search();
                    break;
            }
        }

        private void Search()
        {
            var empty = string.IsNullOrWhiteSpace(Text);
            if (empty)
            {
                ItemsSource = Enumerable.Empty<object>();
            }
            else
            {
                var oriText = Text;
                var searchedItemList = new List<object>();
                foreach (var item in DataSource)
                {
                    var text = item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();
                    if (text.IndexOf(Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        searchedItemList.Add(item);
                    }
                }
                ItemsSource = searchedItemList;
                SelectedItem = null;
                Text = oriText;
            }

            if (!IsDropDownOpen)
            {
                IsDropDownOpen = true;
            }

            _editableTextBox.Select(Text.Length, 0);
        }
    }
}
