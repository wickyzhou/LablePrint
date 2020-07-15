using K3ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.Service;

namespace Ui.ViewModel
{
    public class K3ApiBaseViewModel:BaseViewModel
    {
        public K3ApiBaseViewModel(string entity)
        {
            K3ApiFKService = new K3ApiFKService();
            K3ApiService = new K3ApiService(entity);
        }
        
        public K3ApiFKService K3ApiFKService { get; set; }

        public K3ApiService K3ApiService { get; set; }


        public virtual K3ApiResponseModel Insert() 
        {
            throw new NotImplementedException();
        }

        public virtual K3ApiResponseModel Update()
        {
            throw new NotImplementedException();
        }

        public virtual K3ApiResponseModel Delete()
        {
            throw new NotImplementedException();
        }

        public virtual K3ApiResponseModel GetTemplate()
        {
            throw new NotImplementedException();
        }

        public virtual K3ApiResponseModel GetDetail()
        {
            throw new NotImplementedException();
        }

        public virtual K3ApiResponseModel CheckBill()
        {
            throw new NotImplementedException();
        }

    }
}
