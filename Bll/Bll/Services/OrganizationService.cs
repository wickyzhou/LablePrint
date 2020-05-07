using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Services
{
   public class OrganizationService
    {
        private readonly OrganizationDAL dal = new OrganizationDAL();

        public List<OrganizationModel> GetOrganization(bool superAdmin,int orgId)
        {
            if (superAdmin)
                return dal.GetOrganization().ToList();
            return dal.GetOrganization(orgId).ToList();

        }

    }
}
