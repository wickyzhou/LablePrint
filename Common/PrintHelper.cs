﻿
using BarTender;
using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Drawing.Printing.PrinterSettings;

namespace Common
{
    public delegate void DisplayUpdate(List<LabelPrintCurrencyModel> lists);

    public class PrintHelper
    {

        // 普通标签打印
        public string PrintLabel(PrintSchemaParameterModel config, List<LabelPrintCurrencyModel> data)
        {

            string printerName = config.PrinterName;
            string templateName = config.TemplateFullName;
            string orientation = config.Orientation;
            string previousBatchNo = string.Empty;
            int currentPrintBeginValue = 0;
            string currentPrintSeqValue = string.Empty;
            BarTender.Application btApp = new BarTender.Application();

            var printTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                if (data.Count() == 0)
                {
                    return "请先添加到 当天 待打印 数据";
                }


                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;


                if (orientation == "横向")
                {
                    btFormat.PageSetup.Orientation = BarTender.BtOrientation.btPortrait;//0 横向 默认值
                }
                else
                {
                    btFormat.PageSetup.Orientation = BarTender.BtOrientation.btLandscape;//1 纵向
                }

                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var list = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);



                foreach (LabelPrintCurrencyModel model in data)
                {
                    if (list.Contains("RowQuantity"))
                        btFormat.SetNamedSubStringValue("RowQuantity", Convert.ToString(model.RowQuantity));

                    if (list.Contains("SampleOilPrintArea"))
                        btFormat.SetNamedSubStringValue("SampleOilPrintArea", model.SampleOilPrintArea);
                    string ss = model.Seq2678 > 0 ? Convert.ToString(model.Seq2678).PadLeft(6, '0') : "000001";
                    if (list.Contains("Seq2678"))
                        btFormat.SetNamedSubStringValue("Seq2678", ss);

                    if (list.Contains("TwoDimensionCode1"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode1", model.TwoDimensionCode1);

                    if (list.Contains("TwoDimensionCode2"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode2", model.TwoDimensionCode2);

                    if (list.Contains("TwoDimensionCode3"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode3", model.TwoDimensionCode3);

                    if (list.Contains("TwoDimensionCode4"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode4", model.TwoDimensionCode4);

                    if (list.Contains("ProductionDate"))
                        btFormat.SetNamedSubStringValue("ProductionDate", model.ProductionDate.ToString("yyyy-MM-dd"));
                    if (list.Contains("ProductionModel"))
                        btFormat.SetNamedSubStringValue("ProductionModel", model.ProductionModel);

                    if (list.Contains("OrgID"))
                        btFormat.SetNamedSubStringValue("OrgID", model.OrgID);
                    if (list.Contains("Label"))
                        btFormat.SetNamedSubStringValue("Label", model.Label);
                    if (list.Contains("OrgCode"))
                        btFormat.SetNamedSubStringValue("OrgCode", model.OrgCode);
                    if (list.Contains("BatchNo"))
                        btFormat.SetNamedSubStringValue("BatchNo", model.BatchNo);
                    if (list.Contains("RoughWeight"))
                        btFormat.SetNamedSubStringValue("RoughWeight", model.RoughWeight);
                    if (list.Contains("NetWeight"))
                        btFormat.SetNamedSubStringValue("NetWeight", model.NetWeight);
                    if (list.Contains("CheckNo"))
                        btFormat.SetNamedSubStringValue("CheckNo", model.CheckNo);
                    if (list.Contains("ExpirationDate"))
                        btFormat.SetNamedSubStringValue("ExpirationDate", model.ExpirationDate.ToString("yyyy-MM-dd"));
                    if (list.Contains("ExpirationMonth"))
                        btFormat.SetNamedSubStringValue("ExpirationMonth", model.ExpirationMonth);
                    if (list.Contains("TwoDimensionCode"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode", model.TwoDimensionCode);
                    if (list.Contains("SpecialRequest"))
                        btFormat.SetNamedSubStringValue("SpecialRequest", model.SpecialRequest);
                    if (list.Contains("CaseName"))
                        btFormat.SetNamedSubStringValue("CaseName", model.CaseName);
                    if (list.Contains("SafeCode"))
                        btFormat.SetNamedSubStringValue("SafeCode", model.SafeCode);
                    if (list.Contains("DangerousIngredient"))
                        btFormat.SetNamedSubStringValue("DangerousIngredient", model.DangerousIngredient);
                    if (list.Contains("DangerousComment"))
                        btFormat.SetNamedSubStringValue("DangerousComment", model.DangerousComment);
                    if (list.Contains("GB"))
                        btFormat.SetNamedSubStringValue("GB", model.GB);
                    if (list.Contains("QB"))
                        btFormat.SetNamedSubStringValue("QB", model.QB);
                    if (list.Contains("GQB"))
                        btFormat.SetNamedSubStringValue("GQB", model.GQB);

                    if (list.Contains("FS1"))
                        btFormat.SetNamedSubStringValue("FS1", model.FS1);
                    if (list.Contains("FS2"))
                        btFormat.SetNamedSubStringValue("FS2", model.FS2);
                    if (list.Contains("FS3"))
                        btFormat.SetNamedSubStringValue("FS3", model.FS3);
                    if (list.Contains("FS4"))
                        btFormat.SetNamedSubStringValue("FS4", model.FS4);
                    if (list.Contains("FS5"))
                        btFormat.SetNamedSubStringValue("FS5", model.FS5);
                    if (list.Contains("FS6"))
                        btFormat.SetNamedSubStringValue("FS6", model.FS6);
                    if (list.Contains("FS7"))
                        btFormat.SetNamedSubStringValue("FS7", model.FS7);
                    if (list.Contains("FS8"))
                        btFormat.SetNamedSubStringValue("FS8", model.FS8);
                    if (list.Contains("FS9"))
                        btFormat.SetNamedSubStringValue("FS9", model.FS9);
                    if (list.Contains("FS10"))
                        btFormat.SetNamedSubStringValue("FS10", model.FS10);

                    if (list.Contains("FD01"))
                        btFormat.SetNamedSubStringValue("FD01", model.FD01);
                    if (list.Contains("FD02"))
                        btFormat.SetNamedSubStringValue("FD02", model.FD02);
                    if (list.Contains("FD03"))
                        btFormat.SetNamedSubStringValue("FD03", model.FD03);
                    if (list.Contains("FD04"))
                        btFormat.SetNamedSubStringValue("FD04", model.FD04);
                    if (list.Contains("FD05"))
                        btFormat.SetNamedSubStringValue("FD05", model.FD05);
                    if (list.Contains("FD06"))
                        btFormat.SetNamedSubStringValue("FD06", model.FD06);
                    if (list.Contains("FD07"))
                        btFormat.SetNamedSubStringValue("FD07", model.FD07);
                    if (list.Contains("FD08"))
                        btFormat.SetNamedSubStringValue("FD08", model.FD08);
                    if (list.Contains("FD09"))
                        btFormat.SetNamedSubStringValue("FD09", model.FD09);
                    if (list.Contains("FD10"))
                        btFormat.SetNamedSubStringValue("FD10", model.FD10);

                    if (model.NoPrintVocName) //不打VOC名称的数据
                    {
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = model.PrintCount;

                        // 设置了序列号，更新配置
                        if (list.Contains("Seq"))
                        {
                            if (string.IsNullOrEmpty(model.Seq))
                            {
                                if (previousBatchNo != model.BatchNo)
                                {
                                    // 重新获取打印开始序号
                                    currentPrintBeginValue = model.BatchCurrentSeq; //new LabelPrintDAL().GetBatchPrintTotal(model.BatchNo);
                                    previousBatchNo = model.BatchNo;
                                }
                            }
                            currentPrintSeqValue = string.IsNullOrEmpty(model.Seq) ? (currentPrintBeginValue + 1).ToString().PadLeft(3, '0') : model.Seq;
                            btFormat.SetNamedSubStringValue("Seq", currentPrintSeqValue);

                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                            btFormat.PrintSetup.NumberSerializedLabels = model.PrintCount;  // 模板打印序列号（如果模板设置了序列号，这个值相当于打印多少份）
                        }

                        // 设置了序列号，更新配置
                        if (list.Contains("ProductionName"))
                            btFormat.SetNamedSubStringValue("ProductionName", model.ProductionName);


                        /* var s= 结果是0 可能是成功的意思 */
                        var s = btFormat.PrintOut(false, false);
                        if (s != 0)
                        {
                            btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                            btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                            return "打印结果不正常，打开模板手动打印取消警告窗口";
                        }

                        currentPrintBeginValue += model.PrintCount;
                    }
                    else
                    {
                        // 获取该批号的VOC分类，如果只有一个分类则打印张数不变，如果有多个，打印张数则必须按VOC分类逐项打印
                        //var vocLists = model.ICMOOrderBillNo == null
                        //    ? new LabelPrintDAL().GetVocNameListsByBatchNo(model.ProductionDate, model.BatchNo, model.Label, model.OrgID) : new LabelPrintDAL().GetVocNameListsByICMOBillNo(model.ICMOOrderBillNo);

                        //if (vocLists.Count == 1)
                        //{
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = model.PrintCount;

                            // 设置了序列号，更新配置
                            if (list.Contains("Seq"))
                            {
                                if (string.IsNullOrEmpty(model.Seq))
                                {
                                    if (previousBatchNo != model.BatchNo)
                                    {
                                        // 重新获取打印开始序号
                                        currentPrintBeginValue = model.BatchCurrentSeq; //new LabelPrintDAL().GetBatchPrintTotal(model.BatchNo);
                                        previousBatchNo = model.BatchNo;
                                    }
                                }
                                currentPrintSeqValue = string.IsNullOrEmpty(model.Seq) ? (currentPrintBeginValue + 1).ToString().PadLeft(3, '0') : model.Seq;
                                btFormat.SetNamedSubStringValue("Seq", currentPrintSeqValue);

                                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                                btFormat.PrintSetup.NumberSerializedLabels = model.PrintCount;  // 模板打印序列号（如果模板设置了序列号，这个值相当于打印多少份）
                            }

                            // 设置了序列号，更新配置
                            if (list.Contains("ProductionName"))
                                btFormat.SetNamedSubStringValue("ProductionName", model.VOCName);


                            /* var s= 结果是0 可能是成功的意思 */
                            var s = btFormat.PrintOut(false, false);
                            if (s != 0)
                            {
                                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                                return "打印结果不正常，打开模板手动打印取消警告窗口";
                            }

                            currentPrintBeginValue += model.PrintCount;

                        //}
                        //else
                        //{
                        //    foreach (VOCBucketCountModel item in vocLists)
                        //    {
                        //        btFormat.PrintSetup.IdenticalCopiesOfLabel = item.BucketCount;

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

                        //            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                        //            btFormat.PrintSetup.NumberSerializedLabels = item.BucketCount;  // 模板打印序列号（如果模板设置了序列号，这个值相当于打印多少份）
                        //        }

                        //        // 设置了序列号，更新配置
                        //        if (list.Contains("ProductionName"))
                        //            btFormat.SetNamedSubStringValue("ProductionName", item.VOCName);

                        //        var s = btFormat.PrintOut(false, false);
                        //        if (s != 0)
                        //        {
                        //            btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        //            btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        //            return "打印结果不正常，打开模板手动打印取消警告窗口";
                        //        }

                        //        currentPrintBeginValue += model.PrintCount;
                        //    }
                        //}
                    }


                }



                //#region 测试打印出纸，不附加数据，只需要以下代码
                //    btFormat.PrintSetup.Printer = printerName;
                //    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                //    btFormat.PrintOut(false, false);
                //#endregion

                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);

                /* 将当前未打印数据置为已打印，写打印记录 */
                if (data.Count() >= 1)
                {
                    //更新数据库,
                    new LabelPrintDAL().ModifyHistoryAndCurrentData(string.Join(",", data.Select(a => a.ID)), printTime);
                }

                /* sw.Flush();
                sw.Close(); */
                return null;
            }
            catch (Exception ex)
            {
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                throw new Exception(ex.Message);
            }
            finally
            {
                /*  if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    } */
                if (btApp != null)
                {
                    btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                }
            }
        }

        // 样油标签打印
        public string PrintLabelSampleOil(PrintSchemaParameterModel config, List<LabelPrintCurrencyModel> data)
        {

            string printerName = config.PrinterName;
            string templateName = config.TemplateFullName;
            string orientation = config.Orientation;
            string previousBatchNo = string.Empty;
            int currentPrintBeginValue = 0;
            string currentPrintSeqValue = string.Empty;
            BarTender.Application btApp = new BarTender.Application();

            var printTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                if (data.Count() == 0)
                    return "请先添加到 当天 待打印 数据";

                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;

                if (orientation == "横向")
                    btFormat.PageSetup.Orientation = BarTender.BtOrientation.btPortrait;//0 横向 默认值
                else
                    btFormat.PageSetup.Orientation = BarTender.BtOrientation.btLandscape;//1 纵向

                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var list = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);

                foreach (LabelPrintCurrencyModel model in data)
                {
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = model.SampleOilPrintCount;
                    // 设置了序列号，更新配置
                    if (list.Contains("Seq"))
                    {
                        if (string.IsNullOrEmpty(model.Seq))
                        {
                            if (previousBatchNo != model.BatchNo)
                            {
                                // 重新获取打印开始序号
                                currentPrintBeginValue = model.BatchCurrentSeq; // new LabelPrintDAL().GetBatchPrintTotal(model.BatchNo);
                                previousBatchNo = model.BatchNo;
                            }
                        }
                        currentPrintSeqValue = string.IsNullOrEmpty(model.Seq) ? (currentPrintBeginValue + 1).ToString().PadLeft(3, '0') : model.Seq;
                        btFormat.SetNamedSubStringValue("Seq", currentPrintSeqValue);

                        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                        btFormat.PrintSetup.NumberSerializedLabels = model.PrintCount;  // 模板打印序列号（如果模板设置了序列号，这个值相当于打印多少份）
                    }
                    //.TrimEnd('0').TrimEnd('.')
                    if (list.Contains("RowQuantity"))
                        btFormat.SetNamedSubStringValue("RowQuantity", model.RowQuantity.ToString());

                    if (list.Contains("SampleOilPrintProductionName"))
                        btFormat.SetNamedSubStringValue("SampleOilPrintProductionName", model.SampleOilPrintProductionName);
                    if (list.Contains("SampleOilPrintArea"))
                        btFormat.SetNamedSubStringValue("SampleOilPrintArea", model.SampleOilPrintArea);
                    if (list.Contains("Seq2678"))
                        btFormat.SetNamedSubStringValue("Seq2678", "000001");
                    if (list.Contains("TwoDimensionCode1"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode1", model.TwoDimensionCode1);
                    if (list.Contains("TwoDimensionCode2"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode2", model.TwoDimensionCode2);
                    if (list.Contains("TwoDimensionCode3"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode3", model.TwoDimensionCode3);
                    if (list.Contains("TwoDimensionCode4"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode4", model.TwoDimensionCode4);
                    if (list.Contains("ProductionDate"))
                        btFormat.SetNamedSubStringValue("ProductionDate", model.ProductionDate.ToString("yyyy-MM-dd"));
                    if (list.Contains("ProductionModel"))
                        btFormat.SetNamedSubStringValue("ProductionModel", model.ProductionModel);
                    if (list.Contains("ProductionName"))
                        btFormat.SetNamedSubStringValue("ProductionName", model.ProductionName);
                    if (list.Contains("OrgID"))
                        btFormat.SetNamedSubStringValue("OrgID", model.OrgID);
                    if (list.Contains("Label"))
                        btFormat.SetNamedSubStringValue("Label", model.Label);
                    if (list.Contains("OrgCode"))
                        btFormat.SetNamedSubStringValue("OrgCode", model.OrgCode);
                    if (list.Contains("BatchNo"))
                        btFormat.SetNamedSubStringValue("BatchNo", model.BatchNo);
                    if (list.Contains("RoughWeight"))
                        btFormat.SetNamedSubStringValue("RoughWeight", model.RoughWeight);
                    if (list.Contains("NetWeight"))
                        btFormat.SetNamedSubStringValue("NetWeight", model.NetWeight);
                    if (list.Contains("CheckNo"))
                        btFormat.SetNamedSubStringValue("CheckNo", model.CheckNo);
                    if (list.Contains("ExpirationDate"))
                        btFormat.SetNamedSubStringValue("ExpirationDate", model.ExpirationDate.ToString("yyyy-MM-dd"));
                    if (list.Contains("ExpirationMonth"))
                        btFormat.SetNamedSubStringValue("ExpirationMonth", model.ExpirationMonth);
                    if (list.Contains("TwoDimensionCode"))
                        btFormat.SetNamedSubStringValue("TwoDimensionCode", model.TwoDimensionCode);
                    if (list.Contains("SpecialRequest"))
                        btFormat.SetNamedSubStringValue("SpecialRequest", model.SpecialRequest);
                    if (list.Contains("CaseName"))
                        btFormat.SetNamedSubStringValue("CaseName", model.CaseName);
                    if (list.Contains("SafeCode"))
                        btFormat.SetNamedSubStringValue("SafeCode", model.SafeCode);
                    if (list.Contains("DangerousIngredient"))
                        btFormat.SetNamedSubStringValue("DangerousIngredient", model.DangerousIngredient);
                    if (list.Contains("DangerousComment"))
                        btFormat.SetNamedSubStringValue("DangerousComment", model.DangerousComment);

                    if (list.Contains("FS1"))
                        btFormat.SetNamedSubStringValue("FS1", model.FS1);
                    if (list.Contains("FS2"))
                        btFormat.SetNamedSubStringValue("FS2", model.FS2);
                    if (list.Contains("FS3"))
                        btFormat.SetNamedSubStringValue("FS3", model.FS3);
                    if (list.Contains("FS4"))
                        btFormat.SetNamedSubStringValue("FS4", model.FS4);
                    if (list.Contains("FS5"))
                        btFormat.SetNamedSubStringValue("FS5", model.FS5);
                    if (list.Contains("FS6"))
                        btFormat.SetNamedSubStringValue("FS6", model.FS6);
                    if (list.Contains("FS7"))
                        btFormat.SetNamedSubStringValue("FS7", model.FS7);
                    if (list.Contains("FS8"))
                        btFormat.SetNamedSubStringValue("FS8", model.FS8);
                    if (list.Contains("FS9"))
                        btFormat.SetNamedSubStringValue("FS9", model.FS9);
                    if (list.Contains("FS10"))
                        btFormat.SetNamedSubStringValue("FS10", model.FS10);

                    if (list.Contains("FD01"))
                        btFormat.SetNamedSubStringValue("FD01", model.FD01);
                    if (list.Contains("FD02"))
                        btFormat.SetNamedSubStringValue("FD02", model.FD02);
                    if (list.Contains("FD03"))
                        btFormat.SetNamedSubStringValue("FD03", model.FD03);
                    if (list.Contains("FD04"))
                        btFormat.SetNamedSubStringValue("FD04", model.FD04);
                    if (list.Contains("FD05"))
                        btFormat.SetNamedSubStringValue("FD05", model.FD05);
                    if (list.Contains("FD06"))
                        btFormat.SetNamedSubStringValue("FD06", model.FD06);
                    if (list.Contains("FD07"))
                        btFormat.SetNamedSubStringValue("FD07", model.FD07);
                    if (list.Contains("FD08"))
                        btFormat.SetNamedSubStringValue("FD08", model.FD08);
                    if (list.Contains("FD09"))
                        btFormat.SetNamedSubStringValue("FD09", model.FD09);
                    if (list.Contains("FD10"))
                        btFormat.SetNamedSubStringValue("FD10", model.FD10);

                    /* var s= 结果是0 可能是成功的意思 */
                    var s = btFormat.PrintOut(false, false);
                    if (s != 0)
                    {
                        btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        return "打印结果不正常，打开模板手动打印取消警告窗口";
                    }
                    currentPrintBeginValue += model.PrintCount;
                }

                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);

                /* 将当前未打印数据置为已打印，写打印记录 */
                if (data.Count() >= 1)
                {
                    //更新数据库,
                    new LabelPrintDAL().ModifyHistoryAndCurrentDataSampleOil(string.Join(",", data.Select(a => a.ID)), printTime);
                }
                return null;
            }
            catch (Exception ex)
            {
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (btApp != null)
                {
                    btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                }
            }
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

        public static void VerifyPrintConfiguration(BarTenderPrintConfigModelXX config)
        {
            if (string.IsNullOrEmpty(config.PrinterName) || config.TemplateSelectedItem == null)
            {
                MessageBox.Show("请选择模板和打印机");
                return;
            }
        }


    }
}
