using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PrintTemplateRefModel:NotificationObject
    {

        private int id;

        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged(nameof(ID));
            }
        }

        private string moduleName;

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;
                this.RaisePropertyChanged(nameof(ModuleName));
            }
        }

        private string moduleTable;

        public string ModuleTable
        {
            get { return moduleTable; }
            set
            {
                moduleTable = value;
                this.RaisePropertyChanged(nameof(ModuleTable));
            }
        }


        private string templateFieldName;

        public string TemplateFieldName
        {
            get { return templateFieldName; }
            set
            {
                templateFieldName = value;
                this.RaisePropertyChanged(nameof(TemplateFieldName));
            }
        }

        private string templateFieldDesc;

        public string TemplateFieldDesc
        {
            get { return templateFieldDesc; }
            set
            {
                templateFieldDesc = value;
                this.RaisePropertyChanged(nameof(TemplateFieldDesc));
            }
        }

        private string example;

        public string Example
        {
            get { return example; }
            set
            {
                example = value;
                this.RaisePropertyChanged(nameof(Example));
            }
        }




        //public int ID { get; set; }

        //public string ModuleName { get; set; }

        //public string ModuleTable { get; set; }

        //public string TemplateFieldName { get; set; }

        //public string TemplateFieldDesc { get; set; }

        //public string Example { get; set; }
    }
}
