using Model;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using static System.Drawing.Printing.PrinterSettings;

namespace Ui.Helper
{
    public static class PrintHelper
    {
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

        // 获取打印机所有纸张大小
        public static PaperSizeCollection GetPrinterPageSizes2(string printerName)
        {
            PageSettings settings = new PageSettings(new PrinterSettings { PrinterName = printerName });
            return settings.PrinterSettings.PaperSizes;
        }

        // 验证打印参数是否选择
        public static string VerifyPrintConfiguration(BarTenderPrintConfigModelXX config)
        {
            if (string.IsNullOrEmpty(config.PrinterName) || config.TemplateSelectedItem == null)
                return ("请选择模板和打印机");
            return null;
        }

        // 获取A4纸小模板选择模型
        public static List<BarTenderTemplateModel> GetTenderPrintA4Templates(string folderPath)
        {
            List<BarTenderTemplateModel> lists = new List<BarTenderTemplateModel>();
            if (Directory.Exists(folderPath))
            {
                foreach (var item in Directory.GetFiles(folderPath, "*.btw"))
                {
                    string filname = Path.GetFileName(item);
                    var s = filname.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Count() == 2 && int.TryParse(s[0], out int pageIndex))
                    {
                        string fileNamePart2 = s[1].Substring(1);
                        string displayName = fileNamePart2.Replace(".btw", "");
                        int totalPages = Convert.ToInt32(s[1].Replace(fileNamePart2, ""));
                        if (totalPages == pageIndex)
                            lists.Add(new BarTenderTemplateModel { TemplatePerPage = totalPages,TemplateTotalPage = totalPages, TemplateFileName = filname, TemplateFullName = item, TemplateFolderPath = folderPath, TemplateDisplayName = displayName });
                    }
                }
            }
            return lists;
        }


        // 获取目录中模板选择模型
        public static List<BarTenderTemplateModel> GetTenderPrintTemplates(string folderPath)
        {
            List<BarTenderTemplateModel> lists = new List<BarTenderTemplateModel>();
            if (Directory.Exists(folderPath))
            {
                foreach (var item in Directory.GetFiles(folderPath, "*.btw"))
                {
                    lists.Add( new BarTenderTemplateModel { TemplatePerPage =1, TemplateTotalPage=1, TemplateFileName = Path.GetFileName(item), TemplateFullName = item, TemplateFolderPath = folderPath, TemplateDisplayName = Path.GetFileName(item).Replace(".btw", "") });
                }
            }
            return lists;
        }


        // 获取打印机名称
        public static List<string> GetComputerPrinters()
        {
            List<string> printer = new List<string>();
            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                printer.Add(sPrint);
            }
            return printer;
        }
    }
}
