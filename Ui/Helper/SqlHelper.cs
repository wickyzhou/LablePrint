using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ui.Helper
{
    public static class SqlHelper
    {
        private static string connStr = "Password=K3admin;Persist Security Info=True;User ID=sa;Initial Catalog=AIS20200115215325;Data Source=192.168.2.150;Pooling=true;Max Pool Size=40000;Min Pool Size=0;";
        private static string connStrSR = "Password=K3admin;Persist Security Info=True;User ID=sa;Initial Catalog=AIS20170824161725;Data Source=192.168.2.150;Pooling=true;Max Pool Size=40000;Min Pool Size=0;";
        private static string connStrOa = "Password=sokan,123;Persist Security Info=True;User ID=oa;Initial Catalog=v3x;Data Source=192.168.1.240;Pooling=true;Max Pool Size=40000;Min Pool Size=0;";

        public static DataTable ExecuteDataTableSR(string sql, params SqlParameter[] paras)
        {
            DataTable table;
            using (SqlConnection connection = new SqlConnection(connStrSR))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        sda.Fill(dataSet);
                    }
                    //new SqlDataAdapter(command).Fill(dataSet);
                    table = dataSet.Tables[0];
                }
            }
            return table;
        }

        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] paras)
        {
            DataTable table;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        sda.Fill(dataSet);
                    }
                    //new SqlDataAdapter(command).Fill(dataSet);
                    table = dataSet.Tables[0];
                }
            }
            return table;
        }

        public static DataTable ExecuteDataTableOa(string sql, params SqlParameter[] paras)
        {
            DataTable table;
            using (SqlConnection connection = new SqlConnection(connStrOa))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        sda.Fill(dataSet);
                    }
                    //new SqlDataAdapter(command).Fill(dataSet);
                    table = dataSet.Tables[0];
                }
            }
            return table;
        }

        public static DataTable ExecuteDataTableProcedure(string procedureName, params SqlParameter[] paras)
        {
            DataTable table;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        sda.Fill(dataSet);
                    }
                    //new SqlDataAdapter(command).Fill(dataSet);
                    table = dataSet.Tables[0];
                }
            }
            return table;
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] paras)
        {
            int num;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    num = command.ExecuteNonQuery();
                }
            }
            return num;
        }

        public static int ExecuteNonQuerySR(string sql, params SqlParameter[] paras)
        {
            int num;
            using (SqlConnection connection = new SqlConnection(connStrSR))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    num = command.ExecuteNonQuery();
                }
            }
            return num;
        }

        public static int ExecuteNonQueryProcedure(string procedureName, params SqlParameter[] paras)
        {
            int num;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    num = command.ExecuteNonQuery();
                }
            }
            return num;
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] paras)
        {
            object obj2;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    obj2 = command.ExecuteScalar();
                }
            }
            return obj2;
        }

        public static object ExecuteScalarProcedure(string procedureName, params SqlParameter[] paras)
        {
            object obj2;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    obj2 = command.ExecuteScalar();
                }
            }
            return obj2;
        }

        public static string Obj2String(object value)
        {
            if (value == DBNull.Value || value == null)
                return "";
            else return (string)value;
        }

        public static DateTime Obj2DateTime(object value)
        {
            if (value == DBNull.Value || value == null)
                return new DateTime(1970, 1, 1);// "";
            else return (DateTime)value;
        }

        public static object FromDbValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            return value;
        }

        public static object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        public static void LoadDataTableToDBModelTableSR(DataTable dt, string modeltable)
        {
            if (dt.Rows.Count > 0)
            {
                using (SqlBulkCopy bkc = new SqlBulkCopy(connStrSR))
                {
                    bkc.DestinationTableName = modeltable;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string fieldName = dt.Columns[i].ColumnName;
                        bkc.ColumnMappings.Add(fieldName, fieldName);
                    }
                    bkc.WriteToServer(dt);
                }
            }
        }

        public static void LoadDataTableToDBModelTable(DataTable dt, string modeltable)
        {
            if (dt.Rows.Count > 0)
            {
                using (SqlBulkCopy bkc = new SqlBulkCopy(connStr))
                {
                    bkc.DestinationTableName = modeltable;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string fieldName = dt.Columns[i].ColumnName;
                        bkc.ColumnMappings.Add(fieldName, fieldName);
                    }
                    bkc.WriteToServer(dt);
                }
            }
        }

        public static List<T> DataTableToModelList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (value.GetType() == typeof(System.DBNull))
                        {
                            value = null;
                        }
                        pro.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        public static void LoadArrayToDBModelTable<T>(T[] source, string modeltable)
        {
            DataTable dt = ConvertArrayToDataTable<T>(source);
            if (dt.Rows.Count > 0)
            {
                using (SqlBulkCopy bkc = new SqlBulkCopy(connStr))
                {
                    bkc.DestinationTableName = modeltable;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string fieldName = dt.Columns[i].ColumnName;
                        bkc.ColumnMappings.Add(fieldName, fieldName);
                    }
                    bkc.WriteToServer(dt);
                }
            }
        }

        private static DataTable ConvertArrayToDataTable<T>(T[] source)
        {
            DataTable result = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                if (pd.PropertyType.IsGenericType && pd.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    result.Columns.Add(pd.Name, Nullable.GetUnderlyingType(pd.PropertyType));
                else
                    result.Columns.Add(pd.Name, pd.PropertyType);
            }
            foreach (T item in source)
            {
                DataRow row = result.NewRow();
                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
                    row[pd.Name] = pd.GetValue(item) ?? DBNull.Value;
                result.Rows.Add(row);
            }
            return result;
        }

        public static DataTable ConvertIEnumerableToDataTable<T>(IEnumerable<T> array)
        {
            DataTable result = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                if (pd.PropertyType.IsGenericType && pd.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    result.Columns.Add(pd.Name, Nullable.GetUnderlyingType(pd.PropertyType));
                else
                    result.Columns.Add(pd.Name, pd.PropertyType);
            }
            foreach (T item in array)
            {
                DataRow row = result.NewRow();
                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
                    row[pd.Name] = pd.GetValue(item) ?? DBNull.Value;
                result.Rows.Add(row);
            }
            return result;
        }

        public static object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }

        // 数据库有NULL 值的时候，需要将其转化加载到模型属性
        public static object ConvertNullableStringToDbValue(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return DBNull.Value;
            }
            return obj;
        }

        public static object ConvertNullableIntToDbValue(int? obj)
        {
            if (obj == null || obj == 0)
            {
                return DBNull.Value;
            }
            return obj;
        }

    }
}
