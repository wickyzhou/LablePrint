using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApiModel.EntityDescription
{
    public class EntityDescFieldsModel
    {
        public string apiKey { get; set; }

        public string label { get; set; }

        public string type { get; set; }

        public string itemType { get; set; }

        public string defaultValue { get; set; }

        public bool enabled { get; set; }

        public bool required { get; set; }

        public bool createable { get; set; }

        public bool updateable { get; set; }

        public bool sortable { get; set; }

        public int? minLength { get; set; }

        public int? maxLength { get; set; }

        public string dependentPropertyName { get; set; }

        public EntityDescReferToModel referTo { get; set; }

        public object joinTo { get; set; }

        public EntityDescSelectitemModel[] selectitem { get; set; }

        public object[] checkitem { get; set; }

    }
}
