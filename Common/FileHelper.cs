using Dal;
using Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using wf = System.Windows.Forms;

namespace Common
{
    public class FileHelper
    {
        public void ExportItemSourceToExcel(List<ProductiveTaskListModel> lists, string filePath)
        {

            if (lists.Count == 0)
            {
                return;
            }
            string part1 = lists[0].FProductionDate.ToLongDateString().ToString();
            string fName = Path.Combine(filePath, $"{part1}生产任务清单.xls");

            //如果存在此文件则添加1
            if (File.Exists(fName))
                fName = fName.Replace(".xls", DateTime.Now.ToString("HHmmss") + ".xls");

            IWorkbook wb = new HSSFWorkbook();

            //定义单元格格式

            #region 缩小填充
            ICellStyle fitStyle = wb.CreateCellStyle();
            fitStyle.ShrinkToFit = true;
            fitStyle.BorderBottom = BorderStyle.Thin; fitStyle.BorderLeft = BorderStyle.Thin; fitStyle.BorderTop = BorderStyle.Thin; fitStyle.BorderRight = BorderStyle.Thin;
            IFont fitFont = wb.CreateFont();
            fitFont.FontHeight = 14 * 20;
            fitFont.FontName = "宋体";
            fitStyle.SetFont(fitFont);
            #endregion

            #region 数字左对齐
            ICellStyle numberStyle = wb.CreateCellStyle();
            numberStyle.ShrinkToFit = true;
            numberStyle.BorderBottom = BorderStyle.Thin; numberStyle.BorderLeft = BorderStyle.Thin; numberStyle.BorderTop = BorderStyle.Thin; numberStyle.BorderRight = BorderStyle.Thin;
            numberStyle.Alignment = HorizontalAlignment.Right;
            //numberStyle.DataFormat= HSSFDataFormat.GetBuiltinFormat("0");
            IFont numberFont = wb.CreateFont();
            numberFont.FontHeight = 14 * 20;
            numberFont.FontName = "宋体";
            numberStyle.SetFont(numberFont);

            #endregion


            #region 红色数字
            ICellStyle warningStyle = wb.CreateCellStyle();
            warningStyle.ShrinkToFit = true;
            warningStyle.BorderBottom = BorderStyle.Thin; numberStyle.BorderLeft = BorderStyle.Thin; numberStyle.BorderTop = BorderStyle.Thin; numberStyle.BorderRight = BorderStyle.Thin;

            IFont warningFont = wb.CreateFont();
            warningFont.FontHeight = 14 * 20;
            warningFont.FontName = "宋体";
            warningFont.Color = HSSFColor.Red.Index;
            warningStyle.SetFont(warningFont);

            #endregion


            #region 生产类型为返工时候，值为"灌"，设置样式
            ICellStyle guanStyle = wb.CreateCellStyle();
            guanStyle.Alignment = HorizontalAlignment.Center;
            guanStyle.ShrinkToFit = true;
            IFont guanFont = wb.CreateFont();
            guanFont.Color = (short)FontColor.Red;
            guanFont.FontName = "宋体";
            guanFont.FontHeight = 15 * 20;
            guanStyle.SetFont(guanFont);
            #endregion

            #region 标题样式
            IFont titleFont = wb.CreateFont();
            titleFont.FontName = "宋体";
            titleFont.Boldweight = (short)FontBoldWeight.Bold;
            titleFont.FontHeight = 24 * 20; //和行高一样？ 也是20倍
            ICellStyle titleStyle = wb.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(titleFont);
            #endregion

            IDataFormat format = wb.CreateDataFormat();
            //cellsylenumber.DataFormat = format.GetFormat("0.00");

            //获取不同的类型
            var countGroup = lists.Where(m => m.FType != null).GroupBy(m => m.FType).Select(t => new { FType = t.Key, Count = t.Count() });

            foreach (var item in countGroup)
            {
                var typedlists = lists.Where(n => n.FType == item.FType).ToList();
                string previousBatch = string.Empty;
                double batchQuantity = 0;
                //string prevoousProductionModel = string.Empty;

                ISheet sheet = wb.CreateSheet(item.FType);
                sheet.ForceFormulaRecalculation = true;
                sheet.SetColumnWidth(0, 20);

                //6.设置列宽   SetColumnWidth(列的索引号从0开始, N * 256) 第二个参数的单位是1/256个字符宽度。例：将第四列宽度设置为了30个字符
                //7.设置行高   Height的单位是1/20个点。例：设置高度为50个点 row0.Height = 50 * 20;
                sheet.SetColumnWidth(0, (int)19 * 256);
                sheet.SetColumnWidth(1, (int)14 * 256);
                sheet.SetColumnWidth(2, (int)11.5 * 256);
                sheet.SetColumnWidth(3, (int)4 * 256);
                sheet.SetColumnWidth(4, (int)13 * 256);
                sheet.SetColumnWidth(5, (int)25.35 * 256);
                sheet.SetColumnWidth(6, (int)6 * 256);
                sheet.SetColumnWidth(7, (int)19.49 * 256);
                sheet.SetColumnWidth(8, (int)18.2 * 256);
                sheet.SetColumnWidth(9, (int)5.35 * 256);
                sheet.SetColumnWidth(10, (int)9.92 * 256);
                sheet.SetColumnWidth(11, (int)6 * 256);
                sheet.SetColumnWidth(12, (int)7 * 256);
                sheet.SetColumnWidth(13, (int)18 * 256);  //灌和做货                                 
                sheet.SetColumnWidth(14, 8 * 256); //差额
                sheet.SetColumnWidth(15, 0 * 256); //第几次做货
                sheet.SetColumnWidth(16, 15 * 256); //订单号
                sheet.SetColumnWidth(17, 40 * 256); //唯一值
                sheet.SetColumnWidth(18, 14 * 256); //安全编号
                sheet.SetColumnWidth(19, 10 * 256); //样油重量


                #region Logo
                ExportImgToExcel(sheet, (HSSFWorkbook)wb);
                #endregion

                #region 标题
                IRow row0 = sheet.CreateRow(0);
                row0.Height = 30 * 20;
                //CellRangeAddress region = new CellRangeAddress(0, 0, 0, 13);//CellRangeAddress rg = new CellRangeAddress(j + 2, j + 2, 8, 11);
                //sheet.AddMergedRegion(region);
                ICell cell = row0.CreateCell(5);
                cell.SetCellValue(typedlists[0].FProductionDate.ToString("yyyy年MM月dd日") + typedlists[0].FType + "生产任务清单");
                /*第一个参数：从第几行开始合并  第二个参数：到第几行结束合并  第三个参数：从第几列开始合并 第四个参数：到第几列结束合并 */
                cell.CellStyle = titleStyle;
                #endregion


                #region 表头
                IRow row1 = sheet.CreateRow(1);
                row1.Height = (short)20.5 * 20;
                var A = row1.CreateCell(0); A.SetCellValue("产品型号"); A.CellStyle = fitStyle;
                var B = row1.CreateCell(1); B.SetCellValue("产品批号"); B.CellStyle = fitStyle;
                var C = row1.CreateCell(2); C.SetCellValue("生产数量"); C.CellStyle = fitStyle;
                var D = row1.CreateCell(3); D.SetCellValue("小料"); D.CellStyle = fitStyle;
                var E = row1.CreateCell(4); E.SetCellValue("包装桶数"); E.CellStyle = fitStyle;
                var F = row1.CreateCell(5); F.SetCellValue("包装桶"); F.CellStyle = fitStyle;
                var G = row1.CreateCell(6); G.SetCellValue("客户代码"); G.CellStyle = fitStyle;
                var H = row1.CreateCell(7); H.SetCellValue("标签型号"); H.CellStyle = fitStyle;
                var I = row1.CreateCell(8); I.SetCellValue("备注"); I.CellStyle = fitStyle;
                var J = row1.CreateCell(9); J.SetCellValue("留样"); J.CellStyle = fitStyle;
                var K = row1.CreateCell(10); K.SetCellValue("合格证"); K.CellStyle = fitStyle;
                var L = row1.CreateCell(11); L.SetCellValue("接收人"); L.CellStyle = fitStyle;
                var M = row1.CreateCell(12); M.SetCellValue("残液"); M.CellStyle = fitStyle;
                var N = row1.CreateCell(13); N.SetCellValue(""); N.CellStyle = guanStyle;//灌不灌
                var O = row1.CreateCell(14); O.SetCellValue("差额"); O.CellStyle = fitStyle;
                var P = row1.CreateCell(15); P.SetCellValue("生产次数"); P.CellStyle = numberStyle;
                var Q = row1.CreateCell(16); Q.SetCellValue("订单号"); Q.CellStyle = fitStyle;
                var R = row1.CreateCell(17); R.SetCellValue("唯一值"); R.CellStyle = fitStyle;
                var S = row1.CreateCell(18); S.SetCellValue("安全编号"); S.CellStyle = fitStyle;
                var T = row1.CreateCell(19); T.SetCellValue("样油重量(g)"); T.CellStyle = fitStyle;
                #endregion


                for (int j = 0; j < typedlists.Count; j++)
                {
                    IRow row = sheet.CreateRow(j + 2);
                    row.Height = (short)20.5 * 20;

                    string currentBatch = typedlists[j].FBatchNo;
                    var A0 = row.CreateCell(0); var B0 = row.CreateCell(1); var C0 = row.CreateCell(2); var D0 = row.CreateCell(3); var M0 = row.CreateCell(12); var O0 = row.CreateCell(14);

                    if (currentBatch != previousBatch)
                    {
                        batchQuantity = Convert.ToDouble(typedlists[j].FQuantity) - Convert.ToDouble(typedlists[j].RowQuantity) + Convert.ToDouble(typedlists[j].FResidue);
                        A0.SetCellValue(typedlists[j].FitemName);
                        B0.SetCellValue(currentBatch);
                        C0.SetCellValue(Convert.ToDouble(typedlists[j].FQuantity));
                        D0.SetCellValue(typedlists[j].FHasSmallMaterial);
                        M0.SetCellValue(Convert.ToDouble(typedlists[j].FResidue.ToString("0.0")));
                        O0.CellStyle = fitStyle;
                    }
                    else
                    {
                        batchQuantity -= Convert.ToDouble(typedlists[j].RowQuantity);
                        O0.CellStyle = warningStyle;
                    }
                    O0.SetCellValue(batchQuantity);
                    A0.CellStyle = fitStyle; B0.CellStyle = fitStyle; C0.CellStyle = numberStyle; M0.CellStyle = numberStyle; D0.CellStyle = fitStyle;

                    var E0 = row.CreateCell(4); E0.SetCellValue(typedlists[j].FPackage); E0.CellStyle = fitStyle;
                    var F0 = row.CreateCell(5); F0.SetCellValue(typedlists[j].FBucketName); F0.CellStyle = fitStyle;
                    var G0 = row.CreateCell(6); G0.SetCellValue(typedlists[j].FOrgID); G0.CellStyle = fitStyle;
                    var H0 = row.CreateCell(7); H0.SetCellValue(typedlists[j].FLabel); H0.CellStyle = fitStyle;



                    #region 备注字体设置
                    var I0 = row.CreateCell(8);
                    string value = typedlists[j].FNote;
                    I0.SetCellValue(value);

                    ICellStyle noteStyle = wb.CreateCellStyle();
                    noteStyle.BorderBottom = BorderStyle.Thin; noteStyle.BorderLeft = BorderStyle.Thin; noteStyle.BorderTop = BorderStyle.Thin;
                    IFont noteFont = wb.CreateFont();
                    noteFont.FontName = "宋体";
                    int dataLength = UniversalFunction.GetLength(value);
                    if (dataLength >= 50)
                        noteFont.FontHeight = 7 * 20;
                    else if (dataLength >= 40)
                        noteFont.FontHeight = 8.5 * 20;
                    else if (dataLength >= 30)
                        noteFont.FontHeight = 10 * 20;
                    else if (dataLength >= 25)
                        noteFont.FontHeight = 12 * 20;
                    else
                        noteFont.FontHeight = 14 * 20;
                    noteStyle.SetFont(noteFont);
                    I0.CellStyle = noteStyle;

                    #endregion

                    #region 留样开始3列不赋值(赋值空都会有个空格)
                    ICellStyle blankStyle = wb.CreateCellStyle();
                    blankStyle.BorderLeft = BorderStyle.Thin; blankStyle.BorderTop = BorderStyle.Thin; blankStyle.BorderBottom = BorderStyle.Thin;

                    var J0 = row.CreateCell(9); J0.CellStyle = blankStyle;
                    var K0 = row.CreateCell(10); K0.CellStyle = blankStyle;
                    var L0 = row.CreateCell(11); L0.CellStyle = blankStyle;
                    #endregion
                    string productionNumber = typedlists[j].ProductionNumber > 0 && typedlists[j].ProductionNumber <= 3 ? $"第{typedlists[j].ProductionNumber}次生产" : "";
                    string guan = typedlists[j].ProductionType == "返工" ? "灌" : "";

                    var N0 = row.CreateCell(13); N0.SetCellValue(guan + " " + productionNumber); N0.CellStyle = guanStyle;//灌不灌
                    //var P0 = row.CreateCell(15); P0.SetCellValue(); P0.CellStyle = fitStyle; // 第多少次生产和 灌写入同一个单元格，本单元格已设置宽度为0，


                    var Q0 = row.CreateCell(16); Q0.SetCellValue(typedlists[j].FBillNo); Q0.CellStyle = fitStyle;
                    var R0 = row.CreateCell(17); R0.SetCellValue(UniversalFunction.ToHexString(typedlists[j].RowHashValue)); R0.CellStyle = fitStyle;
                    var S0 = row.CreateCell(18); S0.SetCellValue(typedlists[j].SafeCode); S0.CellStyle = fitStyle;
                    var T0 = row.CreateCell(19); T0.SetCellValue(Convert.ToInt32(typedlists[j].PaintSampleTotal)); T0.CellStyle = numberStyle;

                    previousBatch = currentBatch.Length != 0 ? currentBatch : previousBatch;
                }

            }


            FileStream fs = new FileStream(fName, FileMode.Create);//新建才不会报错
            wb.Write(fs);//会自动关闭流文件  //fs.Flush();
            fs.Close();
        }

        public string ImportExcelToDatabase(string filePath)
        {
            IWorkbook wb = null;
            if (!File.Exists(filePath))
            {
                return "文件名不存在";
            }

            try
            {

                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//FileShare 及时文件打开也可以读取里面的内容

                if (filePath.IndexOf("xlsx") > 0)
                {
                    wb = new XSSFWorkbook(fs);
                    fs.Close();
                }
                else if (filePath.IndexOf("xls") > 0)
                {
                    wb = new HSSFWorkbook(fs);
                    fs.Close();
                }
                else
                {
                    return "请输入正确的路径";
                }


                for (int i = 0; i < wb.NumberOfSheets; i++)
                {
                    if (!wb.IsSheetHidden(i))//对所有不是隐藏的表执行转换
                    {
                        AddVisiableSheetToDataSet(wb, wb.GetSheetAt(i).SheetName);
                    }
                }
            }

            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());
            }

            return null;
        }

        /// <summary>
        /// 根据循环遍历Sheet获取数据
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="sheetName"></param>
        private void AddVisiableSheetToDataSet(IWorkbook wb, string sheetName)
        {
            ISheet sh = wb.GetSheet(sheetName);
            DateTime productionDate = DateTime.Now.Date.AddDays(10);
            //List<ProductiveTaskListModel> listModels = new List<ProductiveTaskListModel>();

            //创建列
            DataTable dt = ConvertHelper.CreateDataTableFromModel<ProductiveTaskListModel>();


            string previousBatch = string.Empty, previousProductionModel = string.Empty, previousHasSmallMaterial = string.Empty;
            decimal previousQuantity = 0;

            if (sh != null && sh.PhysicalNumberOfRows > 1)//包括表头大于1条记录
            {

                //获取第一行日期
                string title = sh.GetRow(0).GetCell(0).StringCellValue;
                string productionDateString = title.Substring(0, title.IndexOf("日")).Replace("年", "-").Replace("月", "-").Replace("日", "-");
                try
                {
                    productionDate = DateTime.Parse(productionDateString);
                }
                catch
                {
                    new Exception("标题日期格式错误");
                }

                //具体数据获取sh.FirstRowNum 第一行为表头 从第二行开始获取;LastRowNum多加一行
                for (int i = 2; i <= sh.LastRowNum + 1; i++)
                {
                    IRow rowdata = sh.GetRow(i);
                    //ProductiveTaskListModel model = new ProductiveTaskListModel();
                    DataRow dr = dt.NewRow();

                    //空行跳过
                    if (rowdata == null || rowdata.Cells.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        //批号有数据则，将当行获取的数据写入
                        if (GetCellValue(rowdata.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString().Length > 0)
                        {
                            previousProductionModel = GetCellValue(rowdata.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                            previousBatch = GetCellValue(rowdata.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                            previousQuantity = Convert.ToDecimal(GetCellValue(rowdata.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString());
                            previousHasSmallMaterial = GetCellValue(rowdata.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();

                            dr["FitemName"] = previousProductionModel;
                            dr["FBatchNo"] = previousBatch;
                            dr["FQuantity"] = previousQuantity;
                            dr["FHasSmallMaterial"] = previousHasSmallMaterial;
                        }
                        else //没有批号，则取上一个值
                        {
                            dr["FitemName"] = previousProductionModel;
                            dr["FBatchNo"] = previousBatch;
                            dr["FQuantity"] = previousQuantity;
                            dr["FHasSmallMaterial"] = previousHasSmallMaterial;
                        }

                        dr["FProductionDate"] = productionDate;
                        dr["FType"] = sheetName;
                        dr["ID"] = GetCellValue(rowdata.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                        dr["FPackage"] = GetCellValue(rowdata.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                        dr["FBucketName"] = GetCellValue(rowdata.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                        dr["FOrgID"] = GetCellValue(rowdata.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                        dr["FLabel"] = GetCellValue(rowdata.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                        dr["FNote"] = GetCellValue(rowdata.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                        dr["FResidue"] = Convert.ToDecimal(GetCellValue(rowdata.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString());
                        dr["FBillNo"] = GetCellValue(rowdata.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK)).ToString();
                        dr["CreateDate"] = DateTime.Now.Date;
                        dt.Rows.Add(dr);
                    }
                }
            }

            // 将DataTable dt 导入到数据库 覆盖任务
            new ProductiveTaskListDAL().ImportDataTableSync(dt);
        }

        /// <summary>
        /// 将单元格的值转化为对应类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private object GetCellValue(ICell cell)
        {
            if (cell == null)
                return "";
            object _result = " ";
            switch (cell.CellType)
            {
                case CellType.Numeric:  //日期也会被识别为数字单元格类型
                    bool r;
                    try {  r = DateUtil.IsCellDateFormatted(cell); }
                    catch { r = false; }
                    if (r)
                    {
                        _result = cell.DateCellValue.ToString("yyyy-MM-dd"); break;
                    }
                    else
                    {
                        _result = cell.NumericCellValue; break;
                    }
                case CellType.String: _result = cell.StringCellValue; break;
                case CellType.Boolean: _result = cell.BooleanCellValue; break;
                case CellType.Error: _result = cell.ErrorCellValue; break;
                case CellType.Blank: _result = ""; break;

                case CellType.Formula:
                    if (cell.CachedFormulaResultType == CellType.Numeric)
                    {
                        bool r1;
                        try { r1 = DateUtil.IsCellDateFormatted(cell); }
                        catch { r1 = false; }
                        if (r1)
                        {
                            _result = cell.DateCellValue.ToString("yyyy-MM-dd"); break;
                        }
                        else
                        {
                            _result = cell.NumericCellValue; break;
                        }
                    }
                    else if (cell.CachedFormulaResultType == CellType.String)
                    {
                        _result = cell.StringCellValue;
                    }
                    else if (cell.CachedFormulaResultType == CellType.Boolean)
                    {
                        _result = cell.BooleanCellValue;
                    }
                    else if (cell.CachedFormulaResultType == CellType.Error)
                    {
                        _result = cell.ErrorCellValue;
                    }
                    else if (cell.CachedFormulaResultType == CellType.Blank)
                    {
                        _result = "";
                    }
                    else
                    {
                        _result = "这个新类型没有包含进来";
                    }
                    break;
                default: _result = "这个新类型没有包含进来"; break;

            }
            return _result;
        }

        public static string[] GetTenderPrintTemplates(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath, "*.btw");
            }
            return null;
        }

        private void ExportImgToExcel(ISheet sheet, HSSFWorkbook xssfworkbook)
        {
            try
            {

                //Assembly _assembly = Assembly.GetExecutingAssembly();
                //string[] resNames = _assembly.GetManifestResourceNames();

                Image imgOutput = Bitmap.FromFile(Path.Combine(Directory.GetCurrentDirectory(), @"Image\excelLogo.jpg"));

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
                    int pictureIdx = xssfworkbook.AddPicture(picBytes, PictureType.JPEG);  //添加图片
                    //XSSFDrawing drawing = (XSSFDrawing)sheet.CreateDrawingPatriarch();
                    //XSSFClientAnchor anchor = new XSSFClientAnchor(0, 0, 0, 0, col, row, col + 1, row + 1);// 0 行0列
                    //XSSFPicture picture = (XSSFPicture)drawing.CreatePicture(anchor, pictureIdx);

                    HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                    HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 1);// 0 行0列
                    HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                    //picture.Resize();
                    picBytes = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ExportShippingBillToExcel(IList<ShippingBillExportModel> lists, string hostValue)
        {
            try
            {
                string fName = Path.Combine(hostValue, $"托运清单.xls");
                if (File.Exists(fName))
                    fName = fName.Replace(".xls", DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".xls");

                IWorkbook wb = new HSSFWorkbook();
                ISheet sheet = wb.CreateSheet("托运清单");
                sheet.ForceFormulaRecalculation = true;


                sheet.SetColumnWidth(0, 15 * 256); sheet.SetColumnWidth(1, 10 * 256); sheet.SetColumnWidth(2, 10 * 256); sheet.SetColumnWidth(3, 10 * 256); sheet.SetColumnWidth(4, 10 * 256);
                sheet.SetColumnWidth(5, 10 * 256); sheet.SetColumnWidth(6, 10 * 256); sheet.SetColumnWidth(7, 10 * 256); sheet.SetColumnWidth(8, 10 * 256); sheet.SetColumnWidth(9, 10 * 256);
                sheet.SetColumnWidth(10, 10 * 256); sheet.SetColumnWidth(11, 10 * 256); sheet.SetColumnWidth(12, 10 * 256); sheet.SetColumnWidth(13, 10 * 256); sheet.SetColumnWidth(14, 10 * 256);
                sheet.SetColumnWidth(15, 10 * 256); sheet.SetColumnWidth(16, 10 * 256); sheet.SetColumnWidth(17, 10 * 256); sheet.SetColumnWidth(18, 10 * 256); sheet.SetColumnWidth(19, 10 * 256);
                sheet.SetColumnWidth(20, 10 * 256); sheet.SetColumnWidth(21, 10 * 256); sheet.SetColumnWidth(22, 10 * 256); sheet.SetColumnWidth(23, 10 * 256); sheet.SetColumnWidth(24, 10 * 256);
                sheet.SetColumnWidth(25, 10 * 256); sheet.SetColumnWidth(26, 10 * 256); sheet.SetColumnWidth(27, 10 * 256); sheet.SetColumnWidth(28, 10 * 256); sheet.SetColumnWidth(29, 40 * 256);
                IRow row1 = sheet.CreateRow(0);
                row1.Height = (short)20.5 * 20;

                row1.CreateCell(0).SetCellValue("托运单号"); row1.CreateCell(1).SetCellValue("托运日期"); row1.CreateCell(2).SetCellValue("总数量"); row1.CreateCell(3).SetCellValue("总费用"); row1.CreateCell(4).SetCellValue("物流类型");
                row1.CreateCell(5).SetCellValue("物流公司"); row1.CreateCell(6).SetCellValue("物流单号"); row1.CreateCell(7).SetCellValue("运输费"); row1.CreateCell(8).SetCellValue("邮费"); row1.CreateCell(9).SetCellValue("过路费");
                row1.CreateCell(10).SetCellValue("差旅费"); row1.CreateCell(11).SetCellValue("维修费"); 
                
                row1.CreateCell(12).SetCellValue("国内端"); row1.CreateCell(13).SetCellValue("国际端"); row1.CreateCell(14).SetCellValue("运输端");

                
                row1.CreateCell(15).SetCellValue("需求人"); row1.CreateCell(16).SetCellValue("其他费用");
                row1.CreateCell(17).SetCellValue("主表备注"); row1.CreateCell(18).SetCellValue("商品类型"); row1.CreateCell(19).SetCellValue("明细序号"); row1.CreateCell(20).SetCellValue("案子名称"); row1.CreateCell(21).SetCellValue("品牌名称");
                row1.CreateCell(22).SetCellValue("部门名称"); row1.CreateCell(23).SetCellValue("客户名称"); row1.CreateCell(24).SetCellValue("明细数量"); row1.CreateCell(25).SetCellValue("明细金额"); row1.CreateCell(26).SetCellValue("子表备注");

                for (int i = 0; i < lists.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    row.Height = (short)20 * 20;

                    row.CreateCell(0).SetCellValue(lists[i].BillNo); row.CreateCell(1).SetCellValue(Convert.ToDateTime(lists[i].BillDate).ToString("yyyy-MM-dd")); row.CreateCell(2).SetCellValue(lists[i].TotalQuantity); row.CreateCell(3).SetCellValue(lists[i].TotalAmount); row.CreateCell(4).SetCellValue(lists[i].LogisticsTypeName);
                    row.CreateCell(5).SetCellValue(lists[i].LogisticsCompanyName); row.CreateCell(6).SetCellValue(lists[i].LogisticsBillNo); row.CreateCell(7).SetCellValue(lists[i].YunShuFei); row.CreateCell(8).SetCellValue(lists[i].YouFei); row.CreateCell(9).SetCellValue(lists[i].GuoLuFei);
                    row.CreateCell(10).SetCellValue(lists[i].ChaiLvFei); row.CreateCell(11).SetCellValue(lists[i].WeiXiuFei); 
                    
                    row.CreateCell(12).SetCellValue(lists[i].GuoNeiDuanFeiYong); row.CreateCell(13).SetCellValue(lists[i].GuoJiDuanFeiYong); row.CreateCell(14).SetCellValue(lists[i].YunShuDuanFeiYong);

                    row.CreateCell(15).SetCellValue(lists[i].Demander); row.CreateCell(16).SetCellValue(lists[i].OtherCosts);
                    row.CreateCell(17).SetCellValue(lists[i].NoteA); row.CreateCell(18).SetCellValue(lists[i].GoodsTypeName); row.CreateCell(19).SetCellValue(lists[i].EntryId); row.CreateCell(20).SetCellValue(lists[i].CaseName); row.CreateCell(21).SetCellValue(lists[i].BrandName);
                    row.CreateCell(22).SetCellValue(lists[i].DeptName); row.CreateCell(23).SetCellValue(lists[i].CustName); row.CreateCell(24).SetCellValue(lists[i].Quantity); row.CreateCell(25).SetCellValue(lists[i].Amount);
                    row.CreateCell(26).SetCellValue(lists[i].NoteB);
                }

                FileStream fs = new FileStream(fName, FileMode.Create);//新建才不会报错
                wb.Write(fs);//会自动关闭流文件  //fs.Flush();
                fs.Close();
            }
            catch
            {
                throw;
            }

        }

        public DataTable ConvertExcelToDataTable(string fileName,bool firstSheet)
        {
            IWorkbook wb = null;
            DataTable dataTable = new DataTable();
            if (!File.Exists(fileName))
                return null;

            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//FileShare 及时文件打开也可以读取里面的内容

                if (fileName.IndexOf("xlsx") > 0)
                {
                    wb = new XSSFWorkbook(fs);
                    fs.Close();
                }
                else if (fileName.IndexOf("xls") > 0)
                {
                    wb = new HSSFWorkbook(fs);
                    fs.Close();
                }
                else
                    return null;

                if (firstSheet)
                {   
                    // 增加列
                    ISheet sh = wb.GetSheetAt(0);
                    if (sh == null) return null;
                    IRow header = sh.GetRow(0);
                    if (header == null) return null;
                    for (int i = 0; i < header.Cells.Count; i++)
                    {
                        DataColumn dc = new DataColumn(header.GetCell(i).StringCellValue);
                        dataTable.Columns.Add(dc);
                    }
                    dataTable.Columns.Add(new DataColumn("Seq"));
                    int colCount = dataTable.Columns.Count;

                    // 增加行 
                    for (int i = 1; i <= sh.LastRowNum ; i++)
                    {
                        IRow rowdata = sh.GetRow(i);
                        DataRow dr = dataTable.NewRow();

                        //空行跳过
                        if (rowdata == null || rowdata.Cells.Count == 0)
                            continue;
                        else
                        {
      
                            for (int j = 0; j < colCount-1; j++)
                            {
                                dr[header.GetCell(j).StringCellValue] = j<= rowdata.Cells.Count()? GetCellValue(rowdata.GetCell(j)):"";
                            }
                            dr[colCount-1] = i + 1;
                            dataTable.Rows.Add(dr);
                        }
                    }
                }
                else
                {   // 此处没有实现
                    for (int i = 0; i < wb.NumberOfSheets; i++)
                    {
                        if (!wb.IsSheetHidden(i))//对所有不是隐藏的表执行转换
                        {
                            AddVisiableSheetToDataSet(wb, wb.GetSheetAt(i).SheetName);
                        }
                    }
                }
                return dataTable;
            }

            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());
            }
        }



    }

}
