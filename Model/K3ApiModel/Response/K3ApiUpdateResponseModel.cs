using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.Response
{
    public class K3ApiUpdateResponseModel
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public K3ApiUpdateDataResponseModel[] Data { get; set; }
    }
}
