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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ui.Command;

namespace Ui.Extension
{
    /// <summary>
    /// PrintConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class PrintConfigControl : UserControl
    {
        public PrintConfigControl()
        {
            InitializeComponent();
        }


        // 将依赖属性包装为普通属性
        public DelegateCommand PrintConfigurationSaveCommand
        {
            get { return (DelegateCommand)GetValue(PrintConfigurationSaveCommandProperty); }
            set { SetValue(PrintConfigurationSaveCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrintConfigurationSaveCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrintConfigurationSaveCommandProperty =
            DependencyProperty.Register("PrintConfigurationSaveCommand", typeof(DelegateCommand), typeof(ICommand), new PropertyMetadata(0));

        // 普通的类则应该传递PropertyMetadata,如果是FrameworkElement则可以传递FrameworkPropertyMetadata
        //    public static readonly DependencyProperty TimeProperty =
        //    DependencyProperty.Register("Time", typeof(DateTime), typeof(ClockUserCtrl),
        //    new FrameworkPropertyMetadata(DateTime.Now, new PropertyChangedCallback(TimePropertyChangedCallback)));

        //public DelegateCommand PrintConfigurationSaveCommand { get; set; }


        public IEnumerable<string> ComputerPrinters { get; set; }

        public DelegateCommand PrintBaseCommand { get; set; }

        public DelegateCommand TemplateSelectBaseCommand { get; set; }



        //private IEnumerable<BarTenderTemplateModel> pirntTemplates;

        //public IEnumerable<BarTenderTemplateModel> PirntTemplates
        //{
        //    get { return pirntTemplates; }
        //    set
        //    {
        //        pirntTemplates = value;
        //        this.RaisePropertyChanged(nameof(PirntTemplates));
        //    }
        //}

        //private BarTenderTemplateModel templateSelectedItem;

        //public BarTenderTemplateModel TemplateSelectedItem
        //{
        //    get { return templateSelectedItem; }
        //    set
        //    {
        //        templateSelectedItem = value;
        //        this.RaisePropertyChanged(nameof(TemplateSelectedItem));
        //    }
        //}

        //private BarTenderPrintConfigModelXX printConfiguration;

        //public BarTenderPrintConfigModelXX PrintConfiguration
        //{
        //    get { return printConfiguration; }
        //    set
        //    {
        //        printConfiguration = value;
        //        this.RaisePropertyChanged(nameof(PrintConfiguration));
        //    }
        //}


    }
}
