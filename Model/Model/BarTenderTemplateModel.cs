using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class BarTenderTemplateModel:NotificationObject
    {
        public int TemplateId { get; set; }

        private int templatePerPage;

        public int TemplatePerPage
        {
            get { return templatePerPage; }
            set
            {
                templatePerPage = value;
                this.RaisePropertyChanged(nameof(TemplatePerPage));
            }
        }


        private string templateFileName;

        public string TemplateFileName
        {
            get { return templateFileName; }
            set
            {
                templateFileName = value;
                this.RaisePropertyChanged(nameof(TemplateFileName));
            }
        }


        private string templateFullName;

        public string TemplateFullName
        {
            get { return templateFullName; }
            set
            {
                templateFullName = value;
                this.RaisePropertyChanged(nameof(TemplateFullName));
            }
        }


        private string templateFolderPath;

        public string TemplateFolderPath
        {
            get { return templateFolderPath; }
            set
            {
                templateFolderPath = value;
                this.RaisePropertyChanged(nameof(TemplateFolderPath));
            }
        }


        private int templateTotalPage;

        public int TemplateTotalPage
        {
            get { return templateTotalPage; }
            set
            {
                templateTotalPage = value;
                this.RaisePropertyChanged(nameof(TemplateTotalPage));
            }
        }

        private string templateDisplayName;

        public string TemplateDisplayName
        {
            get { return templateDisplayName; }
            set
            {
                templateDisplayName = value;
                this.RaisePropertyChanged(nameof(TemplateDisplayName));
            }
        }

    }
}
