using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class QuerySchemaModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int SchemaSeq  { get; set; }

        public string SchemaName { get; set; }

        public string TemplateFullName { get; set; }
    }
}
