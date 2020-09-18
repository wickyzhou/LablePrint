using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApiModel.Response
{
    public class QueryXoqlResponseModel<T>
    {
        public int code { get; set; }

        public string msg { get; set; }

        public string errorInfo { get; set; }

        public QueryXoqlDataResponseModel<T> data { get; set; }
    }
}
