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

namespace Ui.View.HelpWindow
{
    /// <summary>
    /// LabelPrintSchemaConfigHelpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintSchemaConfigHelpWindow : System.Windows.Window
    {
        public LabelPrintSchemaConfigHelpWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
        }
    }
}
