using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dal
{
    public class CrystalPrintConfigDAL
    {
        public IEnumerable<CrystalPrintConfigModel> GetCrystalPrintConfig(int typeId,string hostName)
        {
            DataTable data = SqlHelper.ExecuteDataTable(@" SELECT * FROM SJCrystalPrintConfig where TypeId=@TypeId and HostName=@HostName",
                                                     new SqlParameter[] { new SqlParameter("@TypeId", typeId), new SqlParameter("@HostName", hostName) });
            return SqlHelper.DataTableToModelList<CrystalPrintConfigModel>(data);
        }

        public int ModifyCrystalPrintConfig(CrystalPrintConfigModel model)
        {
            return SqlHelper.ExecuteNonQuery(" update SJCrystalPrintConfig set Orientation=@Orientation,PrinterName=@PrinterName,MarginLeft=@MarginLeft,MarginTop=@MarginTop,MarginRight=@MarginRight,MarginBottom=@MarginBottom ,PaperName=@PaperName where Id=@Id",
                 new SqlParameter[] { new SqlParameter("@Id", model.Id), 
                     new SqlParameter("@Orientation", model.Orientation??""), 
                     new SqlParameter("@PrinterName", model.PrinterName??""),
                     new SqlParameter("@MarginLeft", model.MarginLeft),
                     new SqlParameter("@MarginTop", model.MarginTop),
                     new SqlParameter("@MarginRight", model.MarginRight), 
                     new SqlParameter("@MarginBottom", model.MarginBottom),
                 new SqlParameter("@PaperName", model.PaperName)}
                 );
        }

        public int AddCrystalPrintConfig(CrystalPrintConfigModel model)
        {
            return SqlHelper.ExecuteNonQuery(" insert into SJCrystalPrintConfig(TypeId,TypeDesciption,HostName,Orientation,PrinterName,MarginLeft,MarginTop,MarginRight,MarginBottom,PaperName) values   (@TypeId,@TypeDesciption,@HostName,@Orientation,@PrinterName,@MarginLeft,@MarginTop,@MarginRight,@MarginBottom,@PaperName)",
                 new SqlParameter[] { 
                     new SqlParameter("@TypeId", model.TypeId),
                     new SqlParameter("@TypeDesciption", model.TypeDesciption),
                     new SqlParameter("@HostName", model.HostName),
                     new SqlParameter("@Orientation", model.Orientation??""),
                     new SqlParameter("@PrinterName", model.PrinterName??""),
                     new SqlParameter("@MarginLeft", model.MarginLeft),
                     new SqlParameter("@MarginTop", model.MarginTop),
                     new SqlParameter("@MarginRight", model.MarginRight),
                     new SqlParameter("@MarginBottom", model.MarginBottom),
                 new SqlParameter("@PaperName", model.PaperName)}
                 );
        }
    }
}
