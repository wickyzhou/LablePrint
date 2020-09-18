using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApiModel.EntityDescription
{
    public class EntityDescEntityTypesModel
    {
        public long? id { get; set; }

        public string label { get; set; }

        public string apiKey { get; set; }

        public bool disabled { get; set; }

        public bool Default { get; set; }

        public string description { get; set; }

    }
}
