using Common;
using Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
            string fullName = Path.Combine(filePath, $"{fileName}___{DateTime.Now:yyyyMMddHHmmssfffffff}.xls");

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
                        object value = x.GetType().Name == "DBNull" ? null : x;
                        row1.CreateCell(z).SetCellValue(Convert.ToDouble(value));
                    }
                    else if (s.Name == "DateTime")
                    {
                        var x = dataTable.Rows[j][z];
                        object value = x.GetType().Name == "DBNull" ? null : x;
                        row1.CreateCell(z).SetCellValue(Convert.ToString(value));
                    }
                    else
                    {
                        var x = dataTable.Rows[j][z];
                        object value = x.GetType().Name == "DBNull" ? "" : x;
                        row1.CreateCell(z).SetCellValue(Convert.ToString(value));
                    }

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
        public void ExportDataTableToExcel(DataTable dataTable, string filePath, string fileName, int checkBoxValue, int groupId, List<string> orderedName)
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

            List<ICellStyle> cellStyles = new List<ICellStyle>() { stringStyle, fitStyle };

            int rowIndex = 0;
            while (rowIndex > -1 && rowIndex < dataTable.Rows.Count)
            {
                rowIndex = CreateNewSheet(wb, dataTable, orderedName, rowIndex);
            }


            FileStream fs = new FileStream(fullName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件  //fs.Flush();
            fs.Close();
            currencyBatchSeq = 1;
            previousBatchBeginIndex = 0;
        }

        /// <summary>
        /// 最近的通用导出功能20201026
        /// </summary>
        /// <param name="dataTable">导出的结果集</param>
        /// <param name="filePath">导出文件目录</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="typeId">导出类别 1-汇总 2 明细 3-分类</param>
        /// <param name="checkBoxValue">分类选中的类别Id和</param>
        /// <param name="groupId">同页面Id</param>
        /// <param name="orderedName">分类选中的类别名字顺序类别</param>
        /// <param name="hasTitle">第一行是否有标题</param>
        /// <param name="isExportLogo">第一行标题的情况下，是否导出logo图标</param>
        /// <param name="titleName">如果有不是分类，此名字也是sheetName,如果是分类且需要标题，则用占位符$$$代替分类名</param>
        public void ExportDataTableToExcel(DataTable dataTable, string filePath, string fileName, int typeId, List<string> orderedName, bool hasTitle, string titleName, bool isExportLogo, string logoPicFileName = "")
        {
            string fullName = Path.Combine(filePath, $"{fileName}.xls");

            //如果存在此文件则添加1
            if (File.Exists(fullName))
                fullName = fullName.Replace(".xls", DateTime.Now.ToString("--HH-mm-ss") + ".xls");

            IWorkbook wb = new HSSFWorkbook();

            int rowIndex = 0;
            if (typeId == 3)//分类导出
            {
                while (rowIndex > -1 && rowIndex < dataTable.Rows.Count)
                {
                    rowIndex = CreateNewSheet(wb, dataTable, hasTitle, titleName, isExportLogo, logoPicFileName, orderedName, rowIndex);
                }

            }
            else// 普通datatable 导出
            {
                CreateNewSheet(wb, dataTable, hasTitle, titleName, isExportLogo, logoPicFileName);
            }

            FileStream fs = new FileStream(fullName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件
            fs.Close();

            currencyBatchSeq = 1;
            previousBatchBeginIndex = 0;
        }


        // 旧功能分类导出的创建Sheet，保留防止程序报错
        private int CreateNewSheet(IWorkbook wb, DataTable dataTable, List<string> orderedName, int rowIndex)
        {
            string sheetName = GetSheetName(dataTable, currencyBatchSeq, orderedName);

            ISheet sheet = wb.CreateSheet(sheetName);
            sheet.ForceFormulaRecalculation = true;



            IRow row0 = sheet.CreateRow(0);
            row0.Height = (short)20 * 20;

            //表头
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                row0.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
            }

            //数据
            for (int j = 0; j < dataTable.Rows.Count - previousBatchBeginIndex; j++)
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
                    var x = dataTable.Rows[rowIndex][z];
                    object value = x.GetType().Name == "DBNull" ? null : x;
                    var s = dataTable.Columns[z].DataType;
                    if (s.Name == "Int16" || s.Name == "Int32" || s.Name == "Int64" || s.Name == "Float" || s.Name == "Double" || s.Name == "Decimal")
                    {

                        row1.CreateCell(z).SetCellValue(Convert.ToDouble(value));
                    }
                    else if (s.Name == "DateTime")
                    {
                        row1.CreateCell(z).SetCellValue(Convert.ToString(value));
                    }
                    else
                    {
                        row1.CreateCell(z).SetCellValue(Convert.ToString(value));
                    }
                }
                rowIndex++;
            }

            return rowIndex;
        }

        /// <summary>
        ///  新功能分类导出创建Sheet20201026
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="dataTable"></param>
        /// <param name="orderedName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="fileName"></param>
        /// <param name="hasTitle"></param>
        /// <param name="isExportLogo"></param>
        /// <param name="titleName"></param>
        /// <returns></returns>
        private int CreateNewSheet(IWorkbook wb, DataTable dataTable, bool hasTitle, string titleName, bool isExportLogo, string picfileName, List<string> orderedName, int rowIndex)
        {
            #region 标题样式
            IFont titleFont = wb.CreateFont();
            titleFont.FontName = "宋体";
            titleFont.Boldweight = (short)FontBoldWeight.Bold;
            titleFont.FontHeight = 24 * 20; //字体大小
            ICellStyle titleStyle = wb.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(titleFont);
            #endregion

            #region 表头样式
            IFont headerFont = wb.CreateFont();
            headerFont.FontName = "宋体";
            headerFont.Boldweight = (short)FontBoldWeight.Bold;
            headerFont.FontHeight = 14 * 20; //字体大小
            ICellStyle headerStyle = wb.CreateCellStyle();
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.SetFont(headerFont);
            #endregion

            #region 数据样式
            IFont dataFont = wb.CreateFont();
            dataFont.FontName = "宋体";
            dataFont.Boldweight = (short)FontBoldWeight.Normal;
            dataFont.FontHeight = 11.5 * 20; //和行高一样？ 也是20倍
            ICellStyle dataStyle = wb.CreateCellStyle();
            dataStyle.Alignment = HorizontalAlignment.Left;
            dataStyle.VerticalAlignment = VerticalAlignment.Center;
            dataStyle.SetFont(dataFont);
            #endregion



            string sheetName = GetSheetName(dataTable, currencyBatchSeq, orderedName);

            ISheet sheet = wb.CreateSheet(sheetName);
            sheet.ForceFormulaRecalculation = true;
            //设置统一列宽
            for (int i = 0; i < dataTable.Columns.Count - 1; i++)
            {
                sheet.SetColumnWidth(i, 15 * 256);
            }

            int firstRowindex = hasTitle ? 1 : 0;
            if (hasTitle)
            {
                IRow rowTitle = sheet.CreateRow(0);
                rowTitle.Height = (short)20 * 50;
                ICell cell = rowTitle.CreateCell(dataTable.Columns.Count / 2);
                cell.SetCellValue(titleName.Replace("$$$", sheetName));
                cell.CellStyle = titleStyle;
                if (isExportLogo)//导出logo到第一行第一列
                {
                    ExportImgToExcel(sheet, (HSSFWorkbook)wb, picfileName);
                }
            }

            //表头
            IRow row0 = sheet.CreateRow(firstRowindex);
            row0.Height = (short)20 * 25;
            for (int i = 0; i < dataTable.Columns.Count - 1; i++)
            {
                var s = row0.CreateCell(i);
                s.SetCellValue(dataTable.Columns[i].ColumnName);
                s.CellStyle = headerStyle;
            }


            //数据
            for (int j = 0; j < dataTable.Rows.Count - previousBatchBeginIndex; j++)
            {
                // 一个sheet数据导出完成的条件
                if (currencyBatchSeq != Convert.ToInt32(dataTable.Rows[rowIndex][dataTable.Columns.Count - 1]))
                {
                    previousBatchBeginIndex = rowIndex;
                    currencyBatchSeq++;
                    return rowIndex;
                }

                IRow row1 = sheet.CreateRow(j + 1 + firstRowindex);
                row1.Height = (short)18 * 20;


                for (int z = 0; z < dataTable.Columns.Count - 1; z++)
                {
                    var x = dataTable.Rows[rowIndex][z];
                    object value = x.GetType().Name == "DBNull" ? null : x;
                    var data = row1.CreateCell(z);
                    var s = dataTable.Columns[z].DataType;
                    if (s.Name == "Int16" || s.Name == "Int32" || s.Name == "Int64" || s.Name == "Float" || s.Name == "Double" || s.Name == "Decimal")
                    {
                        data.SetCellValue(Convert.ToDouble(value));
                        data.CellStyle = dataStyle;
                    }
                    else if (s.Name == "DateTime")
                    {
                        data.SetCellValue(Convert.ToString(value));
                        data.CellStyle = dataStyle;
                    }
                    else
                    {
                        data.SetCellValue(Convert.ToString(value));
                        data.CellStyle = dataStyle;
                    }
                }
                rowIndex++;
            }

            return rowIndex;
        }


        /// <summary>
        /// 新功能汇总和明细导出创建Sheet20201026
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="dataTable"></param>
        /// <param name="hasTitle"></param>
        /// <param name="titleName">标题名称和Sheet名称</param>
        /// <param name="isExportLogo">是否导出logo</param>
        /// <param name="titleName">默认导出指定的图片，如果不一致则传入图片的文件名，图片放入Image文件夹</param>
        private void CreateNewSheet(IWorkbook wb, DataTable dataTable, bool hasTitle, string titleName, bool isExportLogo, string picfileName)
        {
            #region 标题样式
            IFont titleFont = wb.CreateFont();
            titleFont.FontName = "宋体";
            titleFont.Boldweight = (short)FontBoldWeight.Bold;
            titleFont.FontHeight = 24 * 20; //字体大小
            ICellStyle titleStyle = wb.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(titleFont);
            #endregion

            #region 表头样式
            IFont headerFont = wb.CreateFont();
            headerFont.FontName = "宋体";
            headerFont.Boldweight = (short)FontBoldWeight.Bold;
            headerFont.FontHeight = 14 * 20; //字体大小
            ICellStyle headerStyle = wb.CreateCellStyle();
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.SetFont(headerFont);
            #endregion

            #region 数据样式
            IFont dataFont = wb.CreateFont();
            dataFont.FontName = "宋体";
            dataFont.Boldweight = (short)FontBoldWeight.Normal;
            dataFont.FontHeight = 11.5 * 20; //和行高一样？ 也是20倍
            ICellStyle dataStyle = wb.CreateCellStyle();
            dataStyle.Alignment = HorizontalAlignment.Left;
            dataStyle.VerticalAlignment = VerticalAlignment.Center;
            dataStyle.SetFont(dataFont);
            #endregion

            ISheet sheet = wb.CreateSheet(titleName.Replace("$$$", ""));
            sheet.ForceFormulaRecalculation = true;

            //设置统一列宽
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sheet.SetColumnWidth(i, 15 * 256);
            }


            int firstRowindex = hasTitle ? 1 : 0;
            if (hasTitle)
            {
                IRow rowTitle = sheet.CreateRow(0);
                rowTitle.Height = (short)20 * 50;
                ICell cell = rowTitle.CreateCell(dataTable.Columns.Count / 2);
                cell.SetCellValue(titleName.Replace("$$$", ""));
                cell.CellStyle = titleStyle;
                if (isExportLogo)//导出logo到第一行第一列
                {
                    ExportImgToExcel(sheet, (HSSFWorkbook)wb, picfileName);
                }
            }

            IRow row0 = sheet.CreateRow(firstRowindex);
            row0.Height = (short)20 * 25;

            //表头
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var s = row0.CreateCell(i);
                s.SetCellValue(dataTable.Columns[i].ColumnName);
                s.CellStyle = headerStyle;
            }

            //数据
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                IRow row1 = sheet.CreateRow(j + firstRowindex + 1);
                row1.Height = (short)18 * 20;
                for (int z = 0; z < dataTable.Columns.Count; z++)
                {
                    var s = dataTable.Columns[z].DataType;
                    if (s.Name == "Int16" || s.Name == "Int32" || s.Name == "Int64" || s.Name == "Float" || s.Name == "Double" || s.Name == "Decimal")
                    {
                        var x = dataTable.Rows[j][z];
                        object value = x.GetType().Name == "DBNull" ? null : x;
                        var data = row1.CreateCell(z);
                        data.SetCellValue(Convert.ToDouble(value));
                        data.CellStyle = dataStyle;
                    }
                    else if (s.Name == "DateTime")
                    {
                        var x = dataTable.Rows[j][z];
                        object value = x.GetType().Name == "DBNull" ? null : x;
                        var data = row1.CreateCell(z);
                        data.SetCellValue(Convert.ToString(value));
                        data.CellStyle = dataStyle;
                    }
                    else
                    {
                        var x = dataTable.Rows[j][z];
                        object value = x.GetType().Name == "DBNull" ? "" : x;
                        var data = row1.CreateCell(z);
                        data.SetCellValue(Convert.ToString(value));
                        data.CellStyle = dataStyle;
                    }

                }
            }
        }

        /// <summary>
        /// 获取分类的Sheet名称
        /// </summary>
        /// <param name="dataTable">导出的数据集</param>
        /// <param name="batchSeq">组号</param>
        /// <param name="columns">分类排序列表</param>
        /// <returns></returns>
        private string GetSheetName(DataTable dataTable, int batchSeq, List<string> columns)
        {
            string sheetName = string.Empty;
            DataRow[] row = dataTable.Select($"组号 ={batchSeq}");
            foreach (var item in columns)
            {
                if (DateTime.TryParse(row[0][item].ToString(), out DateTime date))
                {
                    sheetName += "," + date.Date.ToString("yyyy-MM-dd");
                }
                else
                {
                    sheetName += "," + row[0][item].ToString();
                }

            }
            return string.IsNullOrEmpty(sheetName.Substring(1)) ? "空" : sheetName.Substring(1);
        }


        /// <summary>
        /// 将图片导出到Excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="xssfworkbook"></param>
        /// <param name="imageName"></param>
        private void ExportImgToExcel(ISheet sheet, HSSFWorkbook xssfworkbook, string imageName)
        {

            try
            {
                string picName = string.IsNullOrEmpty(imageName) ? @"Image\sjlogo.png" : imageName;
                var picType = string.IsNullOrEmpty(imageName) ? PictureType.PNG : GetPicType(imageName);
                Image imgOutput = Bitmap.FromFile(Path.Combine(Directory.GetCurrentDirectory(), picName));

                //Image imgOutput = System.Drawing.Bitmap.FromStream()
                Image img = imgOutput.GetThumbnailImage(160, 115, null, IntPtr.Zero);
                //图片转换为文件流
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                BinaryReader br = new BinaryReader(ms);
                var picBytes = ms.ToArray();
                ms.Close();
                //插入图片
                if (picBytes != null && picBytes.Length > 0)
                {
                    int pictureIdx = xssfworkbook.AddPicture(picBytes, picType);  //添加图片

                    HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                    HSSFClientAnchor anchor = new HSSFClientAnchor(200, 90, 240, 200, 0, 0, 1, 0);// 90 200 图片高度110 
                    // new HSSFClientAnchor(X1, Y1,  X2, Y2,  列索引1,行索引1 , 列索引2, 行索引2); 行列索引从0开始 ,行列索引指的是 图片左上角所在单元格的行列和 图片右下角所在单元格的行列
                    //X: 0-1024  Y:0-256 ； X1\X2相对本单元格，距离Y轴的偏移量，最大值1023；Y1\Y2相对本单元格，距离X轴的偏移量，最大值255；
                    HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                    //picture.Resize(); //使图像恢复到原始大小
                    picBytes = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据文件名字转化为对应的图片类型
        /// </summary>
        /// <param name="picName">图片文件名</param>
        /// <returns></returns>
        private PictureType GetPicType(string picName)
        {

            if (picName.IndexOf(".JPEG", StringComparison.OrdinalIgnoreCase) > 0 || picName.IndexOf(".JPG", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.JPEG;
            else if (picName.IndexOf(".GIF", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.GIF;
            else if (picName.IndexOf(".PNG", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.PNG;
            else if (picName.IndexOf(".BMP", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.BMP;
            else if (picName.IndexOf(".DIB", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.DIB;
            else if (picName.IndexOf(".EMF", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.EMF;
            else if (picName.IndexOf(".EPS", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.EPS;
            else if (picName.IndexOf(".PICT", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.PICT;
            else if (picName.IndexOf(".TIFF", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.TIFF;
            else if (picName.IndexOf(".WMF", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.WMF;
            else if (picName.IndexOf(".WPG", StringComparison.OrdinalIgnoreCase) > 0)
                return PictureType.WPG;
            return PictureType.Unknown;
        }


        #region 导出发料数据，特定格式
        /// <summary>
        /// 最近的通用导出功能20201026
        /// </summary>
        /// <param name="dataTable">导出的结果集</param>
        /// <param name="filePath">导出文件目录</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="typeId">导出类别 1-汇总 2 明细 3-分类</param>
        /// <param name="checkBoxValue">分类选中的类别Id和</param>
        /// <param name="groupId">同页面Id</param>
        /// <param name="orderedName">分类选中的类别名字顺序类别</param>
        /// <param name="hasTitle">第一行是否有标题</param>
        /// <param name="isExportLogo">第一行标题的情况下，是否导出logo图标</param>
        /// <param name="titleName">如果有不是分类，此名字也是sheetName,如果是分类且需要标题，则用占位符$$$代替分类名</param>
        public void ExportDeliveryDataToExcel(DateTime date, DataTable dataTable, string filePath, string fileName, int typeId, List<string> orderedName, bool hasTitle, string titleName, bool isExportLogo, string logoPicFileName = "")
        {
            string fullName = Path.Combine(filePath, $"{fileName}.xls");

            //如果存在此文件则添加1
            if (File.Exists(fullName))
                fullName = fullName.Replace(".xls", DateTime.Now.ToString("--HH-mm-ss") + ".xls");

            IWorkbook wb = new HSSFWorkbook();

            int rowIndex = 0;
            if (typeId == 3)//分类导出
            {
                while (rowIndex > -1 && rowIndex < dataTable.Rows.Count)
                {
                    rowIndex = CreateNewDeliverySheet(date, wb, dataTable, hasTitle, titleName, isExportLogo, logoPicFileName, orderedName, rowIndex);
                }

            }
            else// 普通datatable 导出
            {
                CreateNewSheet(wb, dataTable, hasTitle, titleName, isExportLogo, logoPicFileName);
            }

            FileStream fs = new FileStream(fullName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件
            fs.Close();

            currencyBatchSeq = 1;
            previousBatchBeginIndex = 0;
        }

        private int CreateNewDeliverySheet(DateTime date, IWorkbook wb, DataTable dataTable, bool hasTitle, string titleName, bool isExportLogo, string picfileName, List<string> orderedName, int rowIndex)
        {
            #region 标题样式
            IFont titleFont = wb.CreateFont();
            titleFont.FontName = "宋体";
            titleFont.Boldweight = (short)FontBoldWeight.Bold;
            titleFont.FontHeight = 24 * 20; //字体大小
            ICellStyle titleStyle = wb.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.BorderBottom = titleStyle.BorderLeft = titleStyle.BorderTop = titleStyle.BorderRight = BorderStyle.Thin;
            titleStyle.SetFont(titleFont);
            #endregion

            #region 表头样式
            IFont headerFont = wb.CreateFont();
            headerFont.FontName = "宋体";
            headerFont.Boldweight = (short)FontBoldWeight.Bold;
            headerFont.FontHeight = 14 * 20; //字体大小
            ICellStyle headerStyle = wb.CreateCellStyle();
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.BorderBottom = headerStyle.BorderLeft = headerStyle.BorderTop = headerStyle.BorderRight = BorderStyle.Thin;
            headerStyle.SetFont(headerFont);
            #endregion

            #region 数据左对齐自动换行缩小字体
            IFont dataFont = wb.CreateFont();
            dataFont.FontName = "宋体";
            dataFont.Boldweight = (short)FontBoldWeight.Normal;
            dataFont.FontHeight = 14 * 20; //和行高一样？ 也是20倍
            ICellStyle dataStyle = wb.CreateCellStyle();
            dataStyle.Alignment = HorizontalAlignment.Left;
            dataStyle.VerticalAlignment = VerticalAlignment.Center;
            dataStyle.BorderBottom = dataStyle.BorderLeft = dataStyle.BorderTop = dataStyle.BorderRight = BorderStyle.Thin;
            dataStyle.WrapText = true;
            dataStyle.SetFont(dataFont);
            #endregion

            #region 数据居中对齐自动换行缩小字体
            IFont dataFont2 = wb.CreateFont();
            dataFont2.FontName = "宋体";
            dataFont2.Boldweight = (short)FontBoldWeight.Normal;
            dataFont2.FontHeight = 14 * 20; //和行高一样？ 也是20倍
            ICellStyle dataStyle2 = wb.CreateCellStyle();
            dataStyle2.Alignment = HorizontalAlignment.Center;
            dataStyle2.VerticalAlignment = VerticalAlignment.Center;
            dataStyle2.BorderBottom = dataStyle2.BorderLeft = dataStyle2.BorderTop = dataStyle2.BorderRight = BorderStyle.Thin;
            dataStyle2.WrapText = true;
            dataStyle2.SetFont(dataFont2);
            #endregion

            IFont dataFont3 = wb.CreateFont();
            dataFont3.FontName = "宋体";
            dataFont3.Boldweight = (short)FontBoldWeight.Normal;
            dataFont3.FontHeight = 10 * 20; //和行高一样？ 也是20倍
            ICellStyle dataStyle3 = wb.CreateCellStyle();
            dataStyle3.Alignment = HorizontalAlignment.Left;
            dataStyle3.VerticalAlignment = VerticalAlignment.Center;
            dataStyle3.BorderBottom = dataStyle3.BorderLeft = dataStyle3.BorderTop = dataStyle3.BorderRight = BorderStyle.Thin;
            dataStyle3.WrapText = true;
            dataStyle3.SetFont(dataFont3);


            string sheetName = GetSheetName(dataTable, currencyBatchSeq, orderedName);

            ISheet sheet = wb.CreateSheet(sheetName);
            sheet.SetMargin(MarginType.RightMargin, (double)0.1);
            sheet.SetMargin(MarginType.TopMargin, (double)1);
            sheet.SetMargin(MarginType.LeftMargin, (double)0.12);
            sheet.SetMargin(MarginType.BottomMargin, (double)1);
            //sheet.PrintSetup.Copies = 1;
            //sheet.PrintSetup.NoColor = true;
            //sheet.PrintSetup.Landscape = true;
            //sheet.PrintSetup.PaperSize = (short)PaperSize.A4;
            sheet.FitToPage = false;
            sheet.RepeatingRows = new CellRangeAddress(0, 1, 0, 6);

            //设置列宽
            sheet.SetColumnWidth(0, 15 * 256); sheet.SetColumnWidth(1, 15 * 256); sheet.SetColumnWidth(2, 15 * 256); sheet.SetColumnWidth(3, (int)(18.5 * 256)); sheet.SetColumnWidth(4, 28 * 256); sheet.SetColumnWidth(5, 10 * 256);


            int firstRowindex = hasTitle ? 1 : 0;
            if (hasTitle)
            {
                //标题加边框
                IRow rowTitle = sheet.CreateRow(0);
                CellRangeAddress region = new CellRangeAddress(0, 0, 0, 5);
                sheet.AddMergedRegion(region);
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, BorderStyle.Thin, HSSFColor.Black.Index);
                rowTitle.Height = (short)(20 * 50);//50 磅
                ICell cell = rowTitle.CreateCell(0);
                cell.SetCellValue(titleName.Replace("$$$", sheetName) + date.ToString("MMdd"));
                cell.CellStyle = titleStyle;
                if (isExportLogo)//导出logo到第一行第一列
                {
                    ExportImgToExcel(sheet, (HSSFWorkbook)wb, picfileName);
                }
            }

            //表头
            IRow row0 = sheet.CreateRow(firstRowindex);
            row0.Height = (short)(20 * 35.25); //35.25 磅
            for (int i = 0; i < dataTable.Columns.Count - 2; i++)
            {
                var s = row0.CreateCell(i);
                s.SetCellValue(dataTable.Columns[i].ColumnName);
                s.CellStyle = headerStyle;
            }

            // 获取该组号的最大数量
            int maxCount = (int)Math.Ceiling((dataTable.Select($"组号={currencyBatchSeq}").Count()) / 15.0) * 15;
            //数据固定15行一页
            for (int j = 0; j < maxCount ; j++)
            {
                // 一个sheet数据导出完成的条件
                if (rowIndex < dataTable.Rows.Count && currencyBatchSeq == Convert.ToInt32(dataTable.Rows[rowIndex][dataTable.Columns.Count - 1])  )
                {
                    IRow row1 = sheet.CreateRow(j + 1 + firstRowindex);
                    row1.Height = (short)(20 * 35.25); //35.25 磅
                    var cell0 = row1.CreateCell(0); cell0.SetCellValue(Convert.ToString(dataTable.Rows[rowIndex][0])); cell0.CellStyle = dataStyle;
                    var cell1 = row1.CreateCell(1); cell1.SetCellValue(Convert.ToString(Convert.ToDouble(dataTable.Rows[rowIndex][1]))); cell1.CellStyle = dataStyle2;
                    var cell2 = row1.CreateCell(2); cell2.SetCellValue(Convert.ToString(Convert.ToDouble(dataTable.Rows[rowIndex][2]))); cell2.CellStyle = dataStyle2;
                    var cell3 = row1.CreateCell(3); cell3.SetCellValue(Convert.ToString(Convert.ToDouble(dataTable.Rows[rowIndex][3]))); cell3.CellStyle = dataStyle2;
                    var cell4 = row1.CreateCell(4);
                    var value = Convert.ToString(dataTable.Rows[rowIndex][4]);
                    cell4.SetCellValue(value); 
                    if(value.Replace("（", "(").Replace("(","").Length + 3 == value.Length)
                        cell4.CellStyle = dataStyle3;
                    else
                        cell4.CellStyle = dataStyle;
                    var cell5 = row1.CreateCell(5); cell5.SetCellValue(Convert.ToString(Convert.ToDouble(dataTable.Rows[rowIndex][5]))); cell5.CellStyle = dataStyle2;
                    rowIndex++;
                }
                else
                {
                    IRow rownull = sheet.CreateRow(j + 1 + firstRowindex);
                    rownull.Height = (short)(20 * 35.25); //35.25 磅
                    for (int z = 0; z < 6; z++)
                    {
                        var cell = rownull.CreateCell(z);
                        cell.SetCellValue("");
                        cell.CellStyle = dataStyle2;
                    }
                }
            }
            currencyBatchSeq++;
            return rowIndex;
        }
        #endregion


    }
}
