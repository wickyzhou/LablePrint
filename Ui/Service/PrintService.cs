using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Ui.Service
{
    public class PrintService
    {
        public int UserId { get; set; }

        public PrintService(int userId)
        {
            UserId = userId;
        }
        public PrintService()
        {

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

        public bool BarTenderExpressPrint(BarTenderPrintConfigModelXX config, decimal formmainId)
        {
            string printerName = config.PrinterName;
            string templateName = config.TemplateSelectedItem.TemplateFullName;
            BarTender.Application btApp = new BarTender.Application();
            try
            {
                var data = new OilSampleService().GetExpressPrintData(formmainId);
                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;

                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var list = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);
                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                btFormat.PrintSetup.NumberSerializedLabels = 1;

                //btFormat.SetNamedSubStringValue("AA", data.SendName);

                if (list.Contains("SendName"))
                    btFormat.SetNamedSubStringValue("SendName", data.SendName);
                if (list.Contains("SendPhone"))
                    btFormat.SetNamedSubStringValue("SendPhone", data.SendPhone);
                if (list.Contains("SendCompanyName"))
                    btFormat.SetNamedSubStringValue("SendCompanyName", data.SendCompanyName);
                if (list.Contains("SendAddress"))
                    btFormat.SetNamedSubStringValue("SendAddress", data.SendAddress);
                if (list.Contains("ContractMan"))
                    btFormat.SetNamedSubStringValue("ContractMan", data.ContractMan);
                if (list.Contains("ContractPhone"))
                    btFormat.SetNamedSubStringValue("ContractPhone", data.ContractPhone);
                if (list.Contains("ContractCompanyName"))
                    btFormat.SetNamedSubStringValue("ContractCompanyName", data.ContractCompanyName);
                if (list.Contains("ContractAddress"))
                    btFormat.SetNamedSubStringValue("ContractAddress", data.ContractAddress);

                /* var s= 结果是0 可能是成功的意思 */
                var s = btFormat.PrintOut(false, false);
                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                return true;
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

        public string BarTenderOilSamplePrint(BarTenderPrintConfigModel config, OilSampleEntryModel data)
        {
            string weightPerBucket = string.Empty;
            string weightPerBucketLast = string.Empty;
            string printerName = config.PrinterName;
            string templateName = config.TemplateFullName;
            BarTender.Application btApp = new BarTender.Application();
            try
            {
                // 获取批次号(先获取打印记录的，如果没有就获取新的)
                string batchNo = new OilSampleService().GetOilSampleEntryBatchNo(data.Id);
                if (string.IsNullOrEmpty(batchNo))
                {
                    var seq = new CommonService().GetCurrentDateNextSerialNumber(data.ProductionDate, "OilSamplePrintBatchNo");
                    batchNo = data.ProductionDate.ToString("yyMMdd") + seq.ToString().PadLeft(3, '0');
                }

                //int printCount = data.CurrencyPrintCount % config.TemplatePerPage == 0 ? data.CurrencyPrintCount / config.TemplatePerPage : data.CurrencyPrintCount / config.TemplatePerPage + 1;

                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;

                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var list = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);
                btFormat.PrintSetup.NumberSerializedLabels = 1;
                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                weightPerBucket = data.WeightPerBucket.ToString();
                weightPerBucketLast = data.WeightPerBucket.ToString();

                //for (int i = 0; i < printCount; i++)
                //{
                // 最后一张(已打张数+本次打印数量 >=总张数  的最后一页)
                if (data.TotalWeight % data.WeightPerBucket != 0 && data.PrintedCount + data.CurrencyPrintCount >= data.PrintTotalCount)
                    weightPerBucketLast = (Math.Round(data.TotalWeight % data.WeightPerBucket, 2)).ToString();
                if (list.Contains("ProductionDate"))
                    btFormat.SetNamedSubStringValue("ProductionDate", data.ProductionDate.ToString("yyyy-MM-dd"));
                if (list.Contains("ProductionModel"))
                    btFormat.SetNamedSubStringValue("ProductionModel", data.ProductionModel);
                if (list.Contains("ProductionName"))
                    btFormat.SetNamedSubStringValue("ProductionName", data.ProductionName);
                if (list.Contains("ExpirationMonth"))
                    btFormat.SetNamedSubStringValue("ExpirationMonth", data.ExpirationMonth);
                if (list.Contains("BatchNo"))
                    btFormat.SetNamedSubStringValue("BatchNo", batchNo);
                if (list.Contains("CheckNo"))
                    btFormat.SetNamedSubStringValue("CheckNo", data.CheckNo);
                if (list.Contains("RoughWeight"))
                    btFormat.SetNamedSubStringValue("RoughWeight", data.RoughWeight);

                if (list.Contains("WeightPerBucket"))
                    btFormat.SetNamedSubStringValue("WeightPerBucket", weightPerBucket);

                if (list.Contains("WeightPerBucketLast"))
                    btFormat.SetNamedSubStringValue("WeightPerBucketLast", weightPerBucketLast);

                var s = btFormat.PrintOut(false, false);
                //}

                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                return batchNo;
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



        public bool BarTenderMergePrint(BarTenderPrintConfigModel config, ObservableCollection<OilSampleEntryModel> data, int printTotalNum, List<BarTenderTemplateModel> barTenderTemplates)
        {
            string printerName = config.PrinterName;

            string templateName = barTenderTemplates.FirstOrDefault(m => m.TemplatePerPage == printTotalNum && m.TemplateTotalPage == 4).TemplateFullName;
            List<OilSampleFlowPrintLogModel> logs = new List<OilSampleFlowPrintLogModel>();
            BarTender.Application btApp = new BarTender.Application();
            try
            {
                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;
                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var list = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);
                btFormat.PrintSetup.NumberSerializedLabels = 1;
                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                int z = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    var entry = data[i];
                    string batchNo = new OilSampleService().GetOilSampleEntryBatchNo(entry.Id);
                    if (string.IsNullOrEmpty(batchNo))
                    {
                        var seq = new CommonService().GetCurrentDateNextSerialNumber(entry.ProductionDate, "OilSamplePrintBatchNo");
                        batchNo = entry.ProductionDate.ToString("yyMMdd") + seq.ToString().PadLeft(3, '0');
                    }
                    entry.BatchNo = batchNo;
                    logs.Add(new OilSampleFlowPrintLogModel
                    {
                        FormsonId = entry.Id,
                        FormmainId = entry.FormmainId,
                        EntryId = entry.EntryId,
                        PrintCount = entry.CurrencyPrintCount,
                        PrintedCount = entry.PrintedCount + entry.CurrencyPrintCount,
                        BatchNo = entry.BatchNo,
                        TypeId = config.TemplateTypeId,
                        TypeDesc = config.TemplateTypeName
                    });

                    for (int j = 0; j < entry.CurrencyPrintCount; j++)
                    {
                        z++;
                        if (entry.PrintedCount + entry.CurrencyPrintCount >= entry.PrintTotalCount && j == entry.CurrencyPrintCount - 1 && entry.TotalWeight % entry.WeightPerBucket != 0)
                            entry.WeightPerBucket = (float)Math.Round(entry.TotalWeight % entry.WeightPerBucket, 2);
                        switch (z)
                        {
                            case 1: SetTemplateNamedSubStringValueToPart1(btFormat, list, entry); break;
                            case 2: SetTemplateNamedSubStringValueToPart2(btFormat, list, entry); break;
                            case 3: SetTemplateNamedSubStringValueToPart3(btFormat, list, entry); break;
                            case 4: SetTemplateNamedSubStringValueToPart4(btFormat, list, entry); break;
                            default: break;
                        }
                    }
                }

                var s = btFormat.PrintOut(false, false);

                // 写日志
                foreach (var item in logs)
                {
                    var result = new OilSampleService().InsertOilSampleFlowLog2(item);
                    if (!result)
                        return false;
                }


                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                return true;
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

        private bool SetTemplateNamedSubStringValueToPart1(BarTender.Format btFormat, List<string> list, OilSampleEntryModel data)
        {
            if (list.Contains("ProductionDate1"))
                btFormat.SetNamedSubStringValue("ProductionDate1", data.ProductionDate.ToString("yyyy-MM-dd"));
            if (list.Contains("ProductionModel1"))
                btFormat.SetNamedSubStringValue("ProductionModel1", data.ProductionModel);
            if (list.Contains("ProductionName1"))
                btFormat.SetNamedSubStringValue("ProductionName1", data.ProductionName);
            if (list.Contains("ExpirationMonth1"))
                btFormat.SetNamedSubStringValue("ExpirationMonth1", data.ExpirationMonth);
            if (list.Contains("BatchNo1"))
                btFormat.SetNamedSubStringValue("BatchNo1", data.BatchNo);
            if (list.Contains("CheckNo1"))
                btFormat.SetNamedSubStringValue("CheckNo1", data.CheckNo);
            if (list.Contains("RoughWeight1"))
                btFormat.SetNamedSubStringValue("RoughWeight1", data.RoughWeight);
            if (list.Contains("WeightPerBucket1"))
                btFormat.SetNamedSubStringValue("WeightPerBucket1", data.WeightPerBucket.ToString());
            return true;
        }

        private bool SetTemplateNamedSubStringValueToPart2(BarTender.Format btFormat, List<string> list, OilSampleEntryModel data)
        {
            if (list.Contains("ProductionDate2"))
                btFormat.SetNamedSubStringValue("ProductionDate2", data.ProductionDate.ToString("yyyy-MM-dd"));
            if (list.Contains("ProductionModel2"))
                btFormat.SetNamedSubStringValue("ProductionModel2", data.ProductionModel);
            if (list.Contains("ProductionName2"))
                btFormat.SetNamedSubStringValue("ProductionName2", data.ProductionName);
            if (list.Contains("ExpirationMonth2"))
                btFormat.SetNamedSubStringValue("ExpirationMonth2", data.ExpirationMonth);
            if (list.Contains("BatchNo2"))
                btFormat.SetNamedSubStringValue("BatchNo2", data.BatchNo);
            if (list.Contains("CheckNo2"))
                btFormat.SetNamedSubStringValue("CheckNo2", data.CheckNo);
            if (list.Contains("RoughWeight2"))
                btFormat.SetNamedSubStringValue("RoughWeight2", data.RoughWeight);
            if (list.Contains("WeightPerBucket2"))
                btFormat.SetNamedSubStringValue("WeightPerBucket2", data.WeightPerBucket.ToString());

            return true;
        }

        private bool SetTemplateNamedSubStringValueToPart3(BarTender.Format btFormat, List<string> list, OilSampleEntryModel data)
        {
            if (list.Contains("ProductionDate3"))
                btFormat.SetNamedSubStringValue("ProductionDate3", data.ProductionDate.ToString("yyyy-MM-dd"));
            if (list.Contains("ProductionModel3"))
                btFormat.SetNamedSubStringValue("ProductionModel3", data.ProductionModel);
            if (list.Contains("ProductionName3"))
                btFormat.SetNamedSubStringValue("ProductionName3", data.ProductionName);
            if (list.Contains("ExpirationMonth3"))
                btFormat.SetNamedSubStringValue("ExpirationMonth3", data.ExpirationMonth);
            if (list.Contains("BatchNo3"))
                btFormat.SetNamedSubStringValue("BatchNo3", data.BatchNo);
            if (list.Contains("CheckNo3"))
                btFormat.SetNamedSubStringValue("CheckNo3", data.CheckNo);
            if (list.Contains("RoughWeight3"))
                btFormat.SetNamedSubStringValue("RoughWeight3", data.RoughWeight);
            if (list.Contains("WeightPerBucket3"))
                btFormat.SetNamedSubStringValue("WeightPerBucket3", data.WeightPerBucket.ToString());

            return true;
        }

        private bool SetTemplateNamedSubStringValueToPart4(BarTender.Format btFormat, List<string> list, OilSampleEntryModel data)
        {
            if (list.Contains("ProductionDate4"))
                btFormat.SetNamedSubStringValue("ProductionDate4", data.ProductionDate.ToString("yyyy-MM-dd"));
            if (list.Contains("ProductionModel4"))
                btFormat.SetNamedSubStringValue("ProductionModel4", data.ProductionModel);
            if (list.Contains("ProductionName4"))
                btFormat.SetNamedSubStringValue("ProductionName4", data.ProductionName);
            if (list.Contains("ExpirationMonth4"))
                btFormat.SetNamedSubStringValue("ExpirationMonth4", data.ExpirationMonth);
            if (list.Contains("BatchNo4"))
                btFormat.SetNamedSubStringValue("BatchNo4", data.BatchNo);
            if (list.Contains("CheckNo4"))
                btFormat.SetNamedSubStringValue("CheckNo4", data.CheckNo);
            if (list.Contains("RoughWeight4"))
                btFormat.SetNamedSubStringValue("RoughWeight4", data.RoughWeight);
            if (list.Contains("WeightPerBucket4"))
                btFormat.SetNamedSubStringValue("WeightPerBucket4", data.WeightPerBucket.ToString());

            return true;
        }

        private void SetTemplateNamedSubStringValueToPart<T>(BarTender.Format btFormat, List<string> list, T entry)
        {
            var t = entry.GetType();
            foreach (PropertyInfo item in t.GetProperties())
            {
                var name = item.Name; // 属性名称
                if (list.Contains(name))
                {
                    btFormat.SetNamedSubStringValue(name, Convert.ToString(item.GetValue(entry, null)));
                }
                // var value = item.GetValue(entry, null); // 属性值
                // var type = value?.GetType() ?? typeof(object);//获得属性的类型
            }
        }

        private void SetTemplateNamedSubStringValueToPartN<T>(BarTender.Format btFormat, List<string> list, T entry, int partId)
        {
            var t = entry.GetType();
            foreach (PropertyInfo item in t.GetProperties())
            {
                var name = item.Name + (partId + 1).ToString(); // 属性名称
                if (list.Contains(name))
                {
                    btFormat.SetNamedSubStringValue(name, Convert.ToString(item.GetValue(entry, null)));
                }
                // var value = item.GetValue(entry, null); // 属性值
                // var type = value?.GetType() ?? typeof(object);//获得属性的类型
            }
        }




   
        public string BarTenderPrintLS<T>(IEnumerable<T> lists, BarTenderPrintConfigModelXX config)
        {
            string printerName = config.PrinterName; // 打印机名称
            string templateName = config.TemplateSelectedItem.TemplateFullName; //通用模板名称
            BarTender.Application btApp = new BarTender.Application();
            try
            {
                // StringBuilder stringBuilder = new StringBuilder();
                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;
                btFormat.PrintSetup.NumberSerializedLabels = 1;
                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var fieldLists = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);

                foreach (var entry in lists)
                {
                    SetTemplateNamedSubStringValueToPart(btFormat, fieldLists, entry);
                    var s1 = btFormat.PrintOut(false, false);
                    if (s1 != 0)
                    {
                        btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        return "打印结果不正常，打开模板手动打印取消警告窗口";
                    }
                    else
                        WriteRowHashValueLog(entry);
                }
                return "打印成功";
            }
            catch (Exception ex)
            {
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (btApp != null)
                    btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
            }
        }


        private int GetPrintCountAndWriteLog<T>(T item)
        {
            int printCount = 0;
            string batchNo = string.Empty;
            Byte[] rowHashValue = new byte[16];

            foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
            {
                var name = propertyInfo.Name; // 属性名称
                if (name == "PrintCount")
                    printCount = Convert.ToInt32(propertyInfo.GetValue(item, null));
                else if (name == "BatchNo")
                    batchNo = Convert.ToString(propertyInfo.GetValue(item, null));
                else if (name == "RowHashValue")
                    rowHashValue = propertyInfo.GetValue(item, null) as Byte[];
            }
            SqlHelper.ExecuteNonQuery(" insert into SJLabelPrintA4Log(PrintBucket,PrintUserID,RowHashValue,BatchNo,PrintTime) values(@PrintBucket,@PrintUserID,@RowHashValue,@BatchNo,@PrintTime) ",
                new SqlParameter[] { new SqlParameter("@PrintBucket", printCount), new SqlParameter("@PrintUserID", UserId),new SqlParameter("@RowHashValue", rowHashValue),new SqlParameter("@BatchNo", batchNo)
                , new SqlParameter("@PrintTime", DateTime.Now) });
            return printCount;
        }

       
        private void WriteRowHashValueLog<T>(T item)
        {
            int printCount = 1;
            Byte[] rowHashValue = new byte[16];

            foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
            {
                var name = propertyInfo.Name; // 属性名称
                if (name == "PrintCount")
                    printCount = Convert.ToInt32(propertyInfo.GetValue(item, null));
                else if (name == "RowHashValue")
                    rowHashValue = propertyInfo.GetValue(item, null) as Byte[];
            }
            SqlHelper.ExecuteNonQuery(" insert into SJLabelPrintRowHashValueLog(RowHashValue,BucketCount,UserId,PrintTime) values(@RowHashValue,@BucketCount,@UserId,@PrintTime) ",
                new SqlParameter[] { new SqlParameter("@BucketCount", printCount), 
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@RowHashValue", rowHashValue),
                    new SqlParameter("@PrintTime", DateTime.Now) });
        }

        public bool BarTenderOilSampleEntryMergePrint(BarTenderPrintConfigModel config, ObservableCollection<OilSampleEntryModel> data, int printTotalNum, List<BarTenderTemplateModel> barTenderTemplates)
        {
            string printerName = config.PrinterName;

            string templateName = barTenderTemplates.FirstOrDefault(m => m.TemplatePerPage == printTotalNum && m.TemplateTotalPage == 4).TemplateFullName;
            List<OilSampleFlowPrintLogModel> logs = new List<OilSampleFlowPrintLogModel>();
            BarTender.Application btApp = new BarTender.Application();
            try
            {
                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;
                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var list = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);
                btFormat.PrintSetup.NumberSerializedLabels = 1;
                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                int z = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    var entry = data[i];
                    string batchNo = new OilSampleService().GetOilSampleEntryBatchNo(entry.Id);
                    if (string.IsNullOrEmpty(batchNo))
                    {
                        var seq = new CommonService().GetCurrentDateNextSerialNumber(entry.ProductionDate, "OilSamplePrintBatchNo");
                        batchNo = entry.ProductionDate.ToString("yyMMdd") + seq.ToString().PadLeft(3, '0');
                    }
                    entry.BatchNo = batchNo;
                    logs.Add(new OilSampleFlowPrintLogModel
                    {
                        FormsonId = entry.Id,
                        FormmainId = entry.FormmainId,
                        EntryId = entry.EntryId,
                        PrintCount = entry.CurrencyPrintCount,
                        PrintedCount = entry.PrintedCount + entry.CurrencyPrintCount,
                        BatchNo = entry.BatchNo,
                        TypeId = config.TemplateTypeId,
                        TypeDesc = config.TemplateTypeName
                    });

                    for (int j = 0; j < entry.CurrencyPrintCount; j++)
                    {
                        z++;
                        if (entry.PrintedCount + entry.CurrencyPrintCount >= entry.PrintTotalCount && j == entry.CurrencyPrintCount - 1 && entry.TotalWeight % entry.WeightPerBucket != 0)
                            entry.WeightPerBucket = (float)Math.Round(entry.TotalWeight % entry.WeightPerBucket, 2);
                        switch (z)
                        {
                            case 1: SetTemplateNamedSubStringValueToPart1(btFormat, list, entry); break;
                            case 2: SetTemplateNamedSubStringValueToPart2(btFormat, list, entry); break;
                            case 3: SetTemplateNamedSubStringValueToPart3(btFormat, list, entry); break;
                            case 4: SetTemplateNamedSubStringValueToPart4(btFormat, list, entry); break;
                            default: break;
                        }
                    }
                }

                var s = btFormat.PrintOut(false, false);

                // 写日志
                foreach (var item in logs)
                {
                    var result = new OilSampleService().InsertOilSampleFlowLog2(item);
                    if (!result)
                        return false;
                }


                btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                return true;
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

        public string BarTenderPrintA4<T>(IEnumerable<T> lists, BarTenderPrintConfigModelXX config, int totalPages)
        {
            string printerName = config.PrinterName; // 打印机名称
            int templateTotalPage = config.TemplateSelectedItem.TemplateTotalPage; //一张A4纸里面的小张数
            int pages = (int)Math.Ceiling(totalPages / (double)templateTotalPage); // 应该打印A4纸张数
            int initPages = pages; //原始打印张数
            int remainCount = totalPages % templateTotalPage; // 最后一个模板张数

            string templateName = config.TemplateSelectedItem.TemplateFullName; //通用模板名称
            string newtemplateName = remainCount == 0 ? Path.Combine(config.TemplateSelectedItem.TemplateFolderPath, $"1{config.TemplateSelectedItem.TemplateFileName.Substring(1)}")
                : Path.Combine(config.TemplateSelectedItem.TemplateFolderPath, $"{remainCount}{config.TemplateSelectedItem.TemplateFileName.Substring(1)}"); // 最后一个模板名称

            BarTender.Application btApp = new BarTender.Application();
            try
            {
                #region  整数张数数据打印

                // 最多纸张模板
                BarTender.Format btFormat = btApp.Formats.Open(templateName, false, "");
                btFormat.PrintSetup.Printer = printerName;
                btFormat.PrintSetup.NumberSerializedLabels = 1;
                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                string nameValues = "," + btFormat.NamedSubStrings.GetAll("|", ",");
                Regex rg = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                var fieldLists = GetTendarFieldName(nameValues.Replace(Environment.NewLine, ""), rg);

                // 剩余部分模板
                BarTender.Format btFormat1 = null;
                List<string> fieldLists1 = new List<string>();
                if (remainCount > 0)
                {
                    btFormat1 = btApp.Formats.Open(newtemplateName, false, "");
                    btFormat1.PrintSetup.Printer = printerName;
                    btFormat1.PrintSetup.NumberSerializedLabels = 1;
                    btFormat1.PrintSetup.IdenticalCopiesOfLabel = 1;
                    string nameValues1 = "," + btFormat1.NamedSubStrings.GetAll("|", ",");
                    Regex rg1 = new Regex(@",([^|]*)", RegexOptions.IgnoreCase);
                    fieldLists1 = GetTendarFieldName(nameValues1.Replace(Environment.NewLine, ""), rg1);
                }


                //对各个部分赋值
                int beginPartId = 0;

                //StringBuilder logStrings = new StringBuilder();
                foreach (var entry in lists)
                {

                    //某行记录的打印张数,直接写日志
                    int printCount = GetPrintCountAndWriteLog(entry);

                    // 如果本次打印张数超过一张A4纸上
                    while (printCount + beginPartId >= templateTotalPage)
                    {
                        for (int z = beginPartId; z < templateTotalPage; z++)
                        {
                            SetTemplateNamedSubStringValueToPartN(btFormat, fieldLists, entry, z);
                        }

                        printCount = printCount + beginPartId - templateTotalPage;
                        beginPartId = 0;
                        pages--;
                        var s1 = btFormat.PrintOut(false, false);
                        if (s1 != 0)
                        {
                            btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                            return "打印结果不正常，打开模板手动打印取消警告窗口";
                        }
                    }

                    //单条记录完全打印最后新开一张A4打印的数据(先部分赋值，和剩下行记录数据一起打印)
                    if (printCount > 0)
                    {
                        for (int z = beginPartId; z < printCount + beginPartId; z++)
                        {
                            if (pages == 1 && remainCount > 0) //最后一页把值赋到第二个模板上
                                SetTemplateNamedSubStringValueToPartN(btFormat1, fieldLists1, entry, z);
                            else
                                SetTemplateNamedSubStringValueToPartN(btFormat, fieldLists, entry, z);
                        }
                        beginPartId += printCount;
                    }
                }

                // 如果是最后一张A4的打印情况下， 新开模板赋值打印
                if (pages == 1)
                {
                    if (remainCount == 0)
                    {
                        var s111 = btFormat.PrintOut(false, false);
                        btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        if (s111 != 0)
                        {
                            btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                            return "打印结果不正常，打开模板手动打印取消警告窗口";
                        }
                    }
                    else
                    {
                        var s222 = btFormat1.PrintOut(false, false);
                        btFormat1.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                        if (s222 != 0)
                        {
                            btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
                            return "打印结果不正常，打开模板手动打印取消警告窗口";
                        }

                    }
                }
                #endregion
                // 写日志 
                //new CommonService().ExecuteSqlAsyncReturns(logStrings.ToString());
                return "打印成功";
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

        public string BarTenderPrintA4WeightRem<T>(IEnumerable<T> lists, BarTenderPrintConfigModelXX config)
        {
            return null;
        }

    }
}
