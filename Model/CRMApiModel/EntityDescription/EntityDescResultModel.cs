using CRMApiModel.EntityDescription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ui
{
    public class EntityDescResultModel
    {
        public string apiKey { get; set; }

        public bool custom { get; set; }

        public string label { get; set; }

        public bool disabled { get; set; }

        public bool createable { get; set; }

        public bool deletable { get; set; }

        public bool updateable { get; set; }

        public bool queryable { get; set; }

        public bool feedEnabled { get; set; }

        public EntityDescFieldsModel[] fields { get; set; }

        public EntityDescEntityTypesModel[] entityTypes { get; set; }
    }
}
