using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Services
{
    public class CustomerService
    {
        private readonly CustomerDAL dal = new CustomerDAL();

        public List<CustomerModel> GetCustomers(int orgId)
        {
            if (orgId == 1)
                return dal.GetSongJingCustomers().ToList();
            return dal.GetSongRunCustomers().ToList();
        }
    }
}
