using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ui.Helper
{
    public static class TypeConvertHelper
    {

        public static DataTable ConvertIEnumerableToDataTable<T>(IEnumerable<T> list, long timeTickes)
        {
            DataTable result = new DataTable();
            if (list.Count() > 0)
            {
                PropertyInfo[] propertys = list.ElementAt(0).GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    //获取类型
                    Type colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (pi.Name == "TimeTicks")
                            tempList.Add(timeTickes);
                        else
                        {
                            object obj = pi.GetValue(list.ElementAt(i), null);
                            tempList.Add(obj);
                        }

                    }

                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }
}
