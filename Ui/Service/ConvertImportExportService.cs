using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Reflection;

namespace Ui.Service
{
    public static class ConvertImportExportService
    {
     

        /// <summary>
        /// 将界面绑定的Lists数据导出Excel
        /// </summary>
        /// <typeparam name="T">List的类型</typeparam>
        /// <param name="lists">数据Lists</param>
        /// <param name="filePath">导出目录</param>
        /// <param name="fileName">导出文件名</param>
        /// <param name="selectedColumns">自定义导出列</param>
        public  static void ExportDataTableToExcel<T>(List<T> lists, string filePath, string fileName, List<string> selectedColumns)
        {
            string fullName = Path.Combine(filePath, $"{fileName}{DateTime.Now:yyyy-MM-dd}.xls");

            //如果存在此文件则添加1
            if (File.Exists(fullName))
                fullName = fullName.Replace(".xls", DateTime.Now.ToString("--HH-mm-ss") + ".xls");

            IWorkbook wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet(fileName);
            sheet.ForceFormulaRecalculation = true;

            IRow row0 = sheet.CreateRow(0);
            row0.Height = (short)20 * 20;

            ////表头
            //for (int i = 0; i < dataTable.Columns.Count; i++)
            //{
            //    row0.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
            //}

            ////数据
            //for (int j = 0; j < dataTable.Rows.Count; j++)
            //{
            //    IRow row1 = sheet.CreateRow(j + 1);
            //    row1.Height = (short)15 * 20;
            //    for (int z = 0; z < dataTable.Columns.Count; z++)
            //    {
            //        var s = dataTable.Columns[z].DataType;
            //        if (s.Name == "Int16" || s.Name == "Int32" || s.Name == "Int64" || s.Name == "Float" || s.Name == "Double" || s.Name == "Decimal")
            //        {
            //            var x = dataTable.Rows[j][z];
            //            object value = x.GetType().Name == "DBNull" ? 0 : x;
            //            row1.CreateCell(z).SetCellValue(Convert.ToDouble(value));
            //        }

            //        else if (s.Name == "DateTime")
            //            row1.CreateCell(z).SetCellValue(Convert.ToDateTime(dataTable.Rows[j][z]).ToString("yyyy-MM-dd HH:mm:ss"));
            //        else
            //            row1.CreateCell(z).SetCellValue(Convert.ToString(dataTable.Rows[j][z]));
            //    }
            //}

            FileStream fs = new FileStream(fullName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件  //fs.Flush();
            fs.Close();
        }
    }
}
