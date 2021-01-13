using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Ui.Extension
{
    public class NavigationRail : ListBox
    {

        static NavigationRail()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationRail), new FrameworkPropertyMetadata(typeof(NavigationRail)));
        }

        public override void BeginInit()
        {
            TopContent = new ObservableCollection<NavigationRailItem>();
            BottomContent = new ObservableCollection<NavigationRailItem>();
            base.BeginInit();
        }




        public ObservableCollection<NavigationRailItem> TopContent
        {
            get { return (ObservableCollection<NavigationRailItem>)GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopContentProperty =
            DependencyProperty.Register(nameof(TopContent), typeof(ObservableCollection<NavigationRailItem>), typeof(NavigationRail),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public ObservableCollection<NavigationRailItem> BottomContent
        {
            get { return (ObservableCollection<NavigationRailItem>)GetValue(BottomContentProperty); }
            set { SetValue(BottomContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomContentProperty =
            DependencyProperty.Register(nameof(BottomContent), typeof(ObservableCollection<NavigationRailItem>), typeof(NavigationRail),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        new public NavigationRailItem SelectedItem
        {
            get { return (NavigationRailItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        new public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(NavigationRailItem), typeof(NavigationRail), new PropertyMetadata(null));
    }
}
