using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ui.Service
{
    public class PrintService
    {


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
            string templateName = config.ExpressTemplateSelectedItem.TemplateFullName;
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

        public bool BarTenderOilSampleEntryMergePrint(BarTenderPrintConfigModel config, ObservableCollection<OilSampleEntryModel> data,int printTotalNum,List<BarTenderTemplateModel> barTenderTemplates)
        {
            string printerName = config.PrinterName;

            string templateName = barTenderTemplates.FirstOrDefault(m=>m.TemplatePerPage== printTotalNum && m.TemplateTotalPage==4).TemplateFullName;
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


    }
}
