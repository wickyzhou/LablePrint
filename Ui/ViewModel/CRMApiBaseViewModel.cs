using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.Service;

namespace Ui.ViewModel
{
    public class CRMApiBaseViewModel: BaseViewModel
    {
        public CRMApiBaseViewModel()
        {
            CRMService = new CRMApiService();
        }

        public CRMApiService CRMService { get; set; }

       
    }
}
