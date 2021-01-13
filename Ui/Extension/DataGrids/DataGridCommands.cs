using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ui.Extension.DataGrids
{
    public static class DataGridCommands
    {
        static  DataGridCommands()
        {
            RemoveRows = new RoutedCommand("RemoveRows", typeof(DataGrid));
            CopyAddRows = new RoutedCommand("CopyAddRows", typeof(DataGrid));
            ModifyRows = new RoutedCommand("ModifyRows", typeof(DataGrid));
        }

        public static ICommand RemoveRows { get; }

        public static ICommand CopyAddRows { get; }

        public static ICommand ModifyRows { get; }
    }
}
