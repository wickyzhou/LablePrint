using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.Service;

namespace Ui.ViewModel
{
   public class K3ApiXBaseViewModel : BaseViewModel
    {
        public K3ApiFKService K3ApiFKService { get; set; } = new K3ApiFKService();

        public K3ApiXService K3ApiXService { get; set; } = new K3ApiXService();

        public string Token { get; set; }

    }
}
