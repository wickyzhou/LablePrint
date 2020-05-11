﻿using System;
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
using Ui.MVVM.ViewModel;

namespace Ui.MVVM.View
{
    /// <summary>
    /// UserAddView.xaml 的交互逻辑
    /// </summary>
    public partial class UserAddView : Window
    {
        public UserAddView()
        {
            InitializeComponent();
            this.DataContext = new UserAddViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) =>{this.Close(); }));
        }
    }
}
