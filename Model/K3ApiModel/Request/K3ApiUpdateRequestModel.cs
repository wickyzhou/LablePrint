using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.Request
{
    public class K3ApiUpdateRequestModel<T> where T:class
    {
        public K3ApiUpdateDataRequestModel<T> Data { get; set; }
    }
}
