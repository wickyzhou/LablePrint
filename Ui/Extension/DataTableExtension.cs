using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Ui.Extension
{
    public static class DataTableExtension
    {
        public static DataTable ListToDataTable<T>(this IEnumerable<T> data, string tableName)
        {
            DataTable table = new DataTable(tableName);

            //special handling for value types and string
            if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
            {

                DataColumn dc = new DataColumn("Value");
                table.Columns.Add(dc);
                foreach (T item in data)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = item;
                    table.Rows.Add(dr);
                }
            }
            else
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in properties)
                {
                    if (TypeDescriptor.GetProperties(prop.PropertyType).Count > 1 && !prop.PropertyType.IsValueType)
                    {
                        var p1 = TypeDescriptor.GetProperties(prop.PropertyType).Find("Id", true);
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? p1.PropertyType);
                    }
                    else
                    {
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                }
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        try
                        {
                            if (TypeDescriptor.GetProperties(prop.PropertyType).Count > 1 && !prop.PropertyType.IsValueType)
                            {
                               var item1= prop.GetValue(item);
                                var p1 = TypeDescriptor.GetProperties(prop.PropertyType).Find("Id", true);
                                row[prop.Name] = p1.GetValue(item1) ?? DBNull.Value; ;
                            }
                            else
                            {
                                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                            }
                        }
                        catch(Exception ex)
                        {
                            //throw new Exception(ex.Message);
                            row[prop.Name] = DBNull.Value;
                        }


                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
    }
}
