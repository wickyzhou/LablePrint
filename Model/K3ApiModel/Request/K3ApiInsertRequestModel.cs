using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.Request
{
    public class K3ApiInsertRequestModel<T1, T2>
    {
        public K3ApiInsertDataRequestModel<T1,T2> Data { get; set; }
    }
}
