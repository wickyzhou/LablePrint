using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class QuerySchemaConfigModel
    {
        public int UserId { get; set; }

        public int BtnN { get; set; }

        public int SchemaSeq { get; set; }

        public string SchemaName { get; set; }

        public int Id { get; set; }

        public string OrgId { get; set; }

        public string Label { get; set; }

        public string BatchNo { get; set; }

        public string ProductionModel { get; set; }

        public string ContentTrans { get; set; }

        public bool IsConditionOut { get; set; }

        public string SafeCode { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
