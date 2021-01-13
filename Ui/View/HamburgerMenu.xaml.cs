using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ui.View
{
    /// <summary>
    /// HamburgerMenu.xaml 的交互逻辑
    /// </summary>
    public partial class HamburgerMenu : UserControl
    {
        public HamburgerMenu()
        {
            InitializeComponent();
            this.DataContext = new HamburgerMenuViewModel();
        }

        private void MenuToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton hamburgerMenuButton = (ToggleButton)sender;
            if (hamburgerMenuButton.IsChecked == false)
            {
                Storyboard sb = this.FindResource("OpenMenu") as Storyboard;
                sb.Begin();

            }
            else
            {
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin();
            }
            
            (this.DataContext as HamburgerMenuViewModel).IsOpen = !(this.DataContext as HamburgerMenuViewModel).IsOpen;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.MainFrame.Height = SystemParameters.PrimaryScreenHeight - 155;
            //this.GridMenu.Width = SystemParameters.PrimaryScreenWidth;
        }
    }
}
