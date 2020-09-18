using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApiModel.EntityDescription
{
    public class EntityDescSelectitemModel
    {
        public string label { get; set; }

        public long? value { get; set; }

        public object[] dependentValue { get; set; }
    }
}
