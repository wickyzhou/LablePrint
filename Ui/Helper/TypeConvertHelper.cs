using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// 将数字格式的时间戳转换为日期格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime? ConvertTimeStampToDateTime(string timeStamp)
        {
            if (string.IsNullOrEmpty(timeStamp) || timeStamp.Equals("0"))
                return null;

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(Regex.Replace(timeStamp, @"\s", "") + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 将日期转换为N为小数的时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToTimeStamp(DateTime time, int scale = 0)
        {
            TimeSpan cha = (time - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)));
            double t = (double)cha.TotalSeconds;
            return ((long)(Math.Round(t, scale) * Math.Pow(10, scale))).ToString();
        }

    }
}
