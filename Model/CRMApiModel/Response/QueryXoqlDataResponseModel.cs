using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApiModel.Response
{
    public class QueryXoqlDataResponseModel<T> 
    {
        public int? totalSize { get; set; }

        public int? count { get; set; }

        public T[] records { get; set; }
    }
}
