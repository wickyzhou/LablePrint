using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.MVVM.Common;

namespace Ui.MVVM.ViewModel
{
   public class WindowBase
    {
        public WindowBase()
        {
            //WindowCloseCommand = new RelayCommand<Window>(wd => {
            //    if (wd != null)
            //    {
            //        wd.Close();
            //    }
            //});
        }
        public virtual RelayCommand WindowCloseCommand { get; set; } //关闭窗口
    }
}
