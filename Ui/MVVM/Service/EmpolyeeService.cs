using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;

namespace Ui.MVVM.Service
{
    public class EmpolyeeService
    {

        public bool Insert(Employee employee)
        {
            string sql = @"insert into SJEmployee(Id,Name,Sex,Age,CreateTime) values(@Id,@Name,@Sex,@Age,@CreateTime)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, employee) > 0;
            }
        }

        public bool Update(Employee employee)
        {
            string sql = @"update SJEmployee set Name=@Name,Sex=@Sex,Age=@Age,CreateTime=@CreateTime where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, employee) > 0;
            }
        }

        public bool Delete(Guid id)
        {
            string sql = @"delete from SJEmployee where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }

        public IList<Employee> GetAll()
        {
            string sql = @"select * from SJEmployee";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<Employee>(sql).ToList();
            }
        }

    }
}
