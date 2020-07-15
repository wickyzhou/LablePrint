using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel
{
    public class K3ApiTokenResponseModel
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public K3ApiTokenDataResponseModel Data { get; set; }
    }
}
