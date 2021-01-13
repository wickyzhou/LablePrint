using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ui.Extension
{
    public class NavigationRailItem : ListBoxItem
    {
        static NavigationRailItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationRailItem), new FrameworkPropertyMetadata(typeof(NavigationRailItem)));
        }

        //public PackIconKind IconKind
        //{
        //    get { return (PackIconKind)GetValue(IconKindProperty); }
        //    set { SetValue(IconKindProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IconKindProperty =
        //    DependencyProperty.Register(nameof(IconKind), typeof(PackIconKind), typeof(NavigationRailItem), new PropertyMetadata(default(PackIconKind)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(NavigationRailItem), new PropertyMetadata(""));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(NavigationRailItem),
                new PropertyMetadata(default(ICommand)));
    }
}
