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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// UserDefaultPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserDefaultPage : Page
    {
        public UserDefaultPage()
        {
            InitializeComponent();
            this.MainGrid.Height = SystemParameters.PrimaryScreenHeight - 160;
            ImageBrush b = new ImageBrush();
            b.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/DefaultImg2.jpg"));
            b.Stretch = Stretch.Fill;
            this.Background = b;
          
        }
    }
}
