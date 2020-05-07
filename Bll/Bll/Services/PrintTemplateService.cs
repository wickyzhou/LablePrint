using Dal;
using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bll.Services
{
    public class PrintTemplateService
    {
        private readonly LabelPrintDAL dal = new LabelPrintDAL();

        public IEnumerable<PrintTemplateRefModel> GetPrintTemplateRef()
        {
            return dal.GetPrintTemplateRef();
        }
    }
}
