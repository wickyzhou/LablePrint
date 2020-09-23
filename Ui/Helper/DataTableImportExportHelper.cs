using Common;
using Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ui.Service;

namespace Ui.Helper
{
    public class DataTableImportExportHelper
    {
        int currencyBatchSeq = 1;
        int previousBatchBeginIndex = 0;

        /// <summary>
        /// 将datatable导出到excel，适用于直接用sql查询来导出数据
        /// </summary>
        /// <param name="dataTable">查询数据集中的表</param>
        /// <param name="filePath">导出目录</param>
        /// <param name="fileName">导出文件</param>
        public void ExportDataTableToExcel(DataTable dataTable, string filePath, string fileName)
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

            //表头
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                row0.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
            }

            //数据
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                IRow row1 = sheet.CreateRow(j + 1);
                row1.Height = (short)15 * 20;
                for (int z = 0; z < dataTable.Columns.Count; z++)
                {
                    var s = dataTable.Columns[z].DataType;
                    if (s.Name == "Int16" || s.Name == "Int32" || s.Name == "Int64" || s.Name == "Float" || s.Name == "Double" || s.Name == "Decimal")
                    {
                        var x = dataTable.Rows[j][z];
                        object value = x.GetType().Name == "DBNull" ? 0 : x;
                        row1.CreateCell(z).SetCellValue(Convert.ToDouble(value));
                    }

                    else if (s.Name == "DateTime")
                        row1.CreateCell(z).SetCellValue(Convert.ToDateTime(dataTable.Rows[j][z]).ToString("yyyy-MM-dd HH:mm:ss"));
                    else
                        row1.CreateCell(z).SetCellValue(Convert.ToString(dataTable.Rows[j][z]));
                }
            }

            FileStream fs = new FileStream(fullName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件  //fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 将list按照选定的列分别导出到不同的sheet
        /// </summary>
        /// <param name="dataTable">list转化过来的</param>
        /// <param name="filePath">导出文件目录</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="checkBoxValue">勾选的值</param>
        /// <param name="groupId">没用到。。。</param>
        /// <param name="orderedName">选择的多列的唯一值，相当于groupby选择列作为sheet的名称</param>
        public void ExportDataTableToExcel(DataTable dataTable, string filePath, string fileName, int checkBoxValue, int groupId,List<string> orderedName)
        {
            string fullName = Path.Combine(filePath, $"{fileName}{DateTime.Now:yyyy-MM-dd}.xls");

            //如果存在此文件则添加1
            if (File.Exists(fullName))
                fullName = fullName.Replace(".xls", DateTime.Now.ToString("--HH-mm-ss") + ".xls");

            IWorkbook wb = new HSSFWorkbook();

            #region 设置样式
            #region 缩小填充
            ICellStyle fitStyle = wb.CreateCellStyle();
            fitStyle.ShrinkToFit = true;
            fitStyle.BorderBottom = BorderStyle.Thin; fitStyle.BorderLeft = BorderStyle.Thin; fitStyle.BorderTop = BorderStyle.Thin; fitStyle.BorderRight = BorderStyle.Thin;
            IFont fitFont = wb.CreateFont();
            fitFont.FontHeight = 14 * 20;
            fitFont.FontName = "宋体";
            fitStyle.SetFont(fitFont);
            #endregion

            #region 字符串
            ICellStyle stringStyle = wb.CreateCellStyle();
            stringStyle.BorderBottom = BorderStyle.Thin; stringStyle.BorderLeft = BorderStyle.Thin; stringStyle.BorderTop = BorderStyle.Thin; stringStyle.BorderRight = BorderStyle.Thin;
            IFont stringFont = wb.CreateFont();
            stringFont.FontHeight = 25 * 20;
            stringFont.FontName = "宋体";
            stringStyle.SetFont(stringFont);
            #endregion
            #endregion

            List<ICellStyle> cellStyles = new List<ICellStyle>() { stringStyle,fitStyle };
            //#region 简单粗暴排序
            //DataTable exportData = new DataTable();
            //if (orderedName.Count == 1)
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).CopyToDataTable();
            //else if (orderedName.Count == 2)
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).ThenBy(m => m.Field<string>(orderedName[1])).CopyToDataTable();
            //else if (orderedName.Count == 3)
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).ThenBy(m => m.Field<string>(orderedName[11])).ThenBy(m => m.Field<string>(orderedName[2])).CopyToDataTable();
            //else if (orderedName.Count == 4)
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).ThenBy(m => m.Field<string>(orderedName[1])).ThenBy(m => m.Field<string>(orderedName[2]))
            //        .ThenBy(m => m.Field<string>(orderedName[3])).CopyToDataTable();
            //else if (orderedName.Count == 5)
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).ThenBy(m => m.Field<string>(orderedName[1]))
            //        .ThenBy(m => m.Field<string>(orderedName[2])).ThenBy(m => m.Field<string>(orderedName[3])).ThenBy(m => m.Field<string>(orderedName[4])).CopyToDataTable();
            //else if (orderedName.Count == 6)
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).ThenBy(m => m.Field<string>(orderedName[1])).ThenBy(m => m.Field<string>(orderedName[2]))
            //        .ThenBy(m => m.Field<string>(orderedName[3])).ThenBy(m => m.Field<string>(orderedName[4])).ThenBy(m => m.Field<string>(orderedName[5])).CopyToDataTable();
            //else if (orderedName.Count == 7)
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).ThenBy(m => m.Field<string>(orderedName[1])).ThenBy(m => m.Field<string>(orderedName[2])).ThenBy(m => m.Field<string>(orderedName[3]))
            //        .ThenBy(m => m.Field<string>(orderedName[4])).ThenBy(m => m.Field<string>(orderedName[5])).ThenBy(m => m.Field<string>(orderedName[6])).CopyToDataTable();
            //else
            //    exportData = dataTable.AsEnumerable().OrderBy(m => m.Field<string>(orderedName[0])).ThenBy(m => m.Field<string>(orderedName[1])).ThenBy(m => m.Field<string>(orderedName[2])).ThenBy(m => m.Field<string>(orderedName[3]))
            //        .ThenBy(m => m.Field<string>(orderedName[4])).ThenBy(m => m.Field<string>(orderedName[5])).ThenBy(m => m.Field<string>(orderedName[6])).ThenBy(m => m.Field<string>(orderedName[7])).CopyToDataTable();
            //#endregion


            int rowIndex = 0;
            while (rowIndex > -1 && rowIndex<dataTable.Rows.Count)
            {
                rowIndex = CreateNewSheet(wb, dataTable, orderedName, rowIndex);
            }
        

            FileStream fs = new FileStream(fullName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件  //fs.Flush();
            fs.Close();
        }

        private int CreateNewSheet(IWorkbook wb,DataTable dataTable,List<string> orderedName, int rowIndex)
        {
            string sheetName = GetSheetName(dataTable,currencyBatchSeq, orderedName);

            ISheet sheet = wb.CreateSheet(sheetName);
            sheet.ForceFormulaRecalculation = true;

            //for (int z = 0; z < dataTable.Columns.Count; z++)
            //{
            //    sheet.SetColumnWidth(z, (int)15 * 256);
            //}

            IRow row0 = sheet.CreateRow(0);
            row0.Height = (short)20 * 20;

            //表头
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                row0.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
            }

            //数据
            for (int j = 0; j < dataTable.Rows.Count- previousBatchBeginIndex; j++)
            {   
                // 一个sheet数据导出完成的条件
                if (currencyBatchSeq != Convert.ToInt32(dataTable.Rows[rowIndex][0]))
                {
                    previousBatchBeginIndex = rowIndex;
                    currencyBatchSeq++;
                    return rowIndex;
                }

                IRow row1 = sheet.CreateRow(j + 1);
                row1.Height = (short)15 * 20;
               
          
                for (int z = 0; z < dataTable.Columns.Count; z++)
                {   
                    //previousBatchSeq = Convert.ToInt32(dataTable.Rows[j][0]);
                    var s = dataTable.Columns[z].DataType;
                    if (s.Name == "Int16" || s.Name == "Int32" || s.Name == "Int64" || s.Name == "Float" || s.Name == "Double" || s.Name == "Decimal")
                    {
                        var x = dataTable.Rows[rowIndex][z];
                        object value = x.GetType().Name == "DBNull" ? 0 : x;
                        row1.CreateCell(z).SetCellValue(Convert.ToDouble(value));
                    }

                    else if (s.Name == "DateTime")
                        row1.CreateCell(z).SetCellValue(Convert.ToDateTime(dataTable.Rows[rowIndex][z]).ToString("yyyy-MM-dd HH:mm:ss"));
                    else
                        row1.CreateCell(z).SetCellValue(Convert.ToString(dataTable.Rows[rowIndex][z]));
                }
                rowIndex++;
            }
         
            return rowIndex;
        }

        private string GetSheetName(DataTable dataTable,int batchSeq,List<string> columns)
        {
            string sheetName = string.Empty;
            DataRow[] row = dataTable.Select($"组号 ={batchSeq}");
            foreach (var item in columns)
            {
                sheetName += ","+row[0][item].ToString();
            }
            return string.IsNullOrEmpty(sheetName.Substring(1))?"空": sheetName.Substring(1);
        }

    }
}
