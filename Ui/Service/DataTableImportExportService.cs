using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Ui.Service
{
    public class DataTableImportExportService
    {
        public void ExportItemSourceToExcel(DataTable dataTable,string filePath,string fileName)
        {
            string fullName = Path.Combine(filePath, $"{fileName}{DateTime.Now:yyyy-MM-dd}.xls");

            //如果存在此文件则添加1
            if (File.Exists(fullName))
                fullName = fullName.Replace(".xls", DateTime.Now.ToString("HHmmss") + ".xls");


        }
        
    }
}
