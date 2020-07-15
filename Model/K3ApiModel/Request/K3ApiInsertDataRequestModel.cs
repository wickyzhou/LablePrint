using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.Request
{
    public class K3ApiInsertDataRequestModel<T1,T2>
    {
        public List<T1> Page1 { get; set; }

        public List<T2> Page2 { get; set; }
    }
}
