using System.Windows;
using System.Windows.Input;
using Ui.MVVM.ViewModel;

namespace Ui.MVVM.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DemoMainWindow : Window
    {
        public DemoMainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) =>
            {
                this.Close();
            }));
        }
    }
}
