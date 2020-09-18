using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui;

namespace CRMApiModel.Response
{
    public class EntityDescriptionResponseModel
    {
        public int code { get; set; }

        public string msg { get; set; }

        public object[] ext { get; set; }

        public EntityDescResultModel result { get; set; }
    }
}
