
using System.Windows.Input;
using System.Windows;

namespace Ui.View.HelpWindow
{
    /// <summary>
    /// LabelPrintSpecialRequestHelpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintSpecialRequestHelpWindow : System.Windows.Window
    {
        public LabelPrintSpecialRequestHelpWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(
              new CommandBinding(
                  ApplicationCommands.Close,
                 (send, e) => { this.Close(); }));
        }
    }
}
