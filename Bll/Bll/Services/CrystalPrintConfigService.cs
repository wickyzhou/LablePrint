using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Services
{
    public class CrystalPrintConfigService
    {
        private readonly CrystalPrintConfigDAL dal = new CrystalPrintConfigDAL();

        public CrystalPrintConfigModel GetCrystalPrintConfig(int typeId, string hostName)
        {
            return dal.GetCrystalPrintConfig(typeId, hostName).FirstOrDefault();
        }

        public string AddConfig(CrystalPrintConfigModel model)
        {
            int count = dal.AddCrystalPrintConfig(model);
            if (count != 1)
            {
                return " 新增参数配置失败";
            }
            return " 保存成功 ";
        }

        public string ModifyConfig(CrystalPrintConfigModel model)
        {

            int count = dal.ModifyCrystalPrintConfig(model);
            if (count != 1)
            {
                return " 更改参数配置失败";
            }
            return " 保存成功 ";
        }

    }
}
