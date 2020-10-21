using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.Request
{
    public class K3ApiUpdateDataRequestModel<T>
    {
        public string FNumber { get; set; }

        public T Data { get; set; }
    }
}
