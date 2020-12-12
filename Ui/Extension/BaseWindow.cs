using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Ui.Extension
{
    public partial  class BaseWindow : Window
    {
        public BaseWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }));
            this.MouseLeftButtonDown += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) this.DragMove(); };
            this.MouseLeftButtonDown += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) this.DragMove(); };
        }


    }
}
