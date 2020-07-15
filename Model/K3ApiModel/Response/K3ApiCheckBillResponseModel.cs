using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel
{
    public class K3ApiCheckBillResponseModel
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public dynamic Data { get; set; }
    }
}
