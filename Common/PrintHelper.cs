
using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Seagull.Framework;
using static System.Drawing.Printing.PrinterSettings;

namespace Common
{
    public delegate void DisplayUpdate(List<LabelPrintCurrencyModel> lists);

    public class PrintHelper
    {



        // 打印
        public string PrintLabel(PrintSchemaParameterModel config, List<LabelPrintCurrencyModel> data)
        {
            return null;
            //    string printerName = config.PrinterName;
            //    string templateName = config.TemplateFullName;
            //    string orientation = config.Orientation;
            //    string previousBatchNo = string.Empty;
            //    int currentPrintBeginValue = 0;
            //    string currentPrintSeqValue = string.Empty;
            //    BarTender.Application btApp = new BarTender.Application();

            //    var printTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //try
            //{
            //    if (data.Count() == 0)
            //    {
            //        return "请先添加到 当天 待打印 数据";
            //    }


            //    BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
            //    btFormat.PrintSetup.Printer = printerName;


            //    if (orientation == "横向")
            //    {
            //        btFormat.PageSetup.Orientation = BarTender.BtOrientation.btPortrait;//0 横向 默认值
            //    }
            //    else
            //    {
            //        btFormat.PageSetup.Orientation = BarTender.BtOrientation.btLandscape;//1 纵向
            //    }
            //    /* btFormat.PageSetup.Orientation = BarTender.BtOrientation.btPortrait;//0 横向 默认值
            //        btFormat.PageSetup.Orientation = BarTender.BtOrientation.btLandscape;//1 纵向 
            //        btFormat.PageSetup.Orientation = BarTender.BtOrientation.btPortrait180;//2 横向旋转180 不支持
            //        btFormat.PageSetup.Orientation = BarTender.BtOrientation.btLandscape180;//3 纵向旋转180 不支持 */
            //    //btFormat.PrintToFileLicense = "SEQ7RP291SWNRF2JBHPP958W84EW6Z82KDS";


            //    string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
            //    Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
            //    var list = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);



            //    foreach (LabelPrintCurrencyModel model in data)
            //    {

            //        btFormat.PrintSetup.IdenticalCopiesOfLabel = model.PrintCount;

            //        // 设置了序列号，更新配置
            //        if (list.Contains("Seq"))
            //        {
            //            if (string.IsNullOrEmpty(model.Seq))
            //            {
            //                if (previousBatchNo != model.BatchNo)
            //                {
            //                    // 重新获取打印开始序号
            //                    currentPrintBeginValue = model.BatchCurrentSeq; //new LabelPrintDAL().GetBatchPrintTotal(model.BatchNo);
            //                    previousBatchNo = model.BatchNo;
            //                }
            //            }
            //            currentPrintSeqValue = string.IsNullOrEmpty(model.Seq) ? (currentPrintBeginValue + 1).ToString().PadLeft(3, '0') : model.Seq;
            //            btFormat.SetNamedSubStringValue("Seq", currentPrintSeqValue);
            //            btFormat.PrintSetup.NumberSerializedLabels = model.PrintCount;  // 模板打印序列号（如果模板设置了序列号，这个值相当于打印多少份）
            //            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
            //        }


            //        if (list.Contains("ProductionDate"))
            //            btFormat.SetNamedSubStringValue("ProductionDate", model.ProductionDate.ToString("yyyy-MM-dd"));
            //        if (list.Contains("ProductionModel"))
            //            btFormat.SetNamedSubStringValue("ProductionModel", model.ProductionModel);
            //        if (list.Contains("ProductionName"))
            //            btFormat.SetNamedSubStringValue("ProductionName", model.ProductionName);
            //        if (list.Contains("OrgID"))
            //            btFormat.SetNamedSubStringValue("OrgID", model.OrgID);
            //        if (list.Contains("Label"))
            //            btFormat.SetNamedSubStringValue("Label", model.Label);
            //        if (list.Contains("OrgCode"))
            //            btFormat.SetNamedSubStringValue("OrgCode", model.OrgCode);
            //        if (list.Contains("BatchNo"))
            //            btFormat.SetNamedSubStringValue("BatchNo", model.BatchNo);
            //        if (list.Contains("RoughWeight"))
            //            btFormat.SetNamedSubStringValue("RoughWeight", model.RoughWeight);
            //        if (list.Contains("NetWeight"))
            //            btFormat.SetNamedSubStringValue("NetWeight", model.NetWeight);
            //        if (list.Contains("CheckNo"))
            //            btFormat.SetNamedSubStringValue("CheckNo", model.CheckNo);
            //        if (list.Contains("ExpirationDate"))
            //            btFormat.SetNamedSubStringValue("ExpirationDate", model.ExpirationDate.ToString("yyyy-MM-dd"));
            //        if (list.Contains("ExpirationMonth"))
            //            btFormat.SetNamedSubStringValue("ExpirationMonth", model.ExpirationMonth);
            //        if (list.Contains("TwoDimensionCode"))
            //            btFormat.SetNamedSubStringValue("TwoDimensionCode", model.TwoDimensionCode);
            //        if (list.Contains("SpecialRequest"))
            //            btFormat.SetNamedSubStringValue("SpecialRequest", model.SpecialRequest);
            //        if (list.Contains("CaseName"))
            //            btFormat.SetNamedSubStringValue("CaseName", model.CaseName);
            //        if (list.Contains("SafeCode"))
            //            btFormat.SetNamedSubStringValue("SafeCode", model.SafeCode);
            //        if (list.Contains("DangerousIngredient"))
            //            btFormat.SetNamedSubStringValue("DangerousIngredient", model.DangerousIngredient);
            //        if (list.Contains("DangerousComment"))
            //            btFormat.SetNamedSubStringValue("DangerousComment", model.DangerousComment);

            //        /* var s= 结果是0 可能是成功的意思 */
            //        var s = btFormat.PrintOut(false, false);

            //        currentPrintBeginValue += model.PrintCount;
            //    }



            //    //#region 测试打印出纸，不附加数据，只需要以下代码
            //    //    btFormat.PrintSetup.Printer = printerName;
            //    //    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
            //    //    btFormat.PrintOut(false, false);
            //    //#endregion


            //    btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
            //    btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);

            //    /* 将当前未打印数据置为已打印，写打印记录 */
            //    if (data.Count() >= 1)
            //    {
            //        //更新数据库,
            //        new LabelPrintDAL().ModifyHistoryAndCurrentData(string.Join(",", data.Select(a => a.ID)), printTime);
            //    }

            //    /* sw.Flush();
            //    sw.Close(); */
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    /*  if (sw != null)
            //        {
            //            sw.Flush();
            //            sw.Close();
            //        } */
            //    if (btApp != null)
            //    {
            //        btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
            //    }
            //}
        }






        // 获取打印机名称
        public static List<string> GetComputerPrinter()
        {
            List<string> printer = new List<string>();
            //PrintDocument print = new PrintDocument();
            //string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                printer.Add(sPrint);
            }
            return printer;
        }

        // 获取打印机有的纸张 
        public static List<PageSizeModel> GetPrinterPageSizes(string printerName)
        {
            List<PageSizeModel> lists = new List<PageSizeModel>();
            PageSettings settings = new PageSettings(new PrinterSettings { PrinterName = printerName });
            foreach (PaperSize item in settings.PrinterSettings.PaperSizes)
            {
                lists.Add(new PageSizeModel
                {
                    Height = item.Height,
                    Width = item.Width,
                    RawKind = item.RawKind,
                    Kind = item.Kind,
                    PaperName = item.PaperName
                });
            }

            return lists;
        }

        public static PaperSizeCollection GetPrinterPageSizes2(string printerName)
        {
            PageSettings settings = new PageSettings(new PrinterSettings { PrinterName = printerName });
            return settings.PrinterSettings.PaperSizes;
        }



        // 获取barTender字段
        public static List<string> GetTendarFieldName(string nameValues, Regex rg)
        {
            List<string> list = new List<string>();

            foreach (Match match in rg.Matches(nameValues))
            {
                GroupCollection groups = match.Groups;
                if (!string.IsNullOrEmpty(groups[1].ToString()))
                {
                    list.Add(groups[1].ToString());
                }
            }
            return list;
        }

    }
}
