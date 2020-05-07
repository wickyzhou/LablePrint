using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class QuerySchemaDynamicBtnModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int SchemaSeq { get; set; }

        public string SchemaName { get; set; }

        public string Content { get; set; }

        public double MarginLeft { get; set; }

        public double MarginTop { get; set; }

        public double MarginRight { get; set; }

        public double MarginBottom { get; set; }

        public string TemplateFullName { get; set; }

    }
}
