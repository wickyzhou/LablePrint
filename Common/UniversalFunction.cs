using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class UniversalFunction
    {


        public static bool ConvertStringToNullableInt(string inputValue, out int? outValue)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                outValue = null;
                return true;
            }
            else
            {
                bool r = int.TryParse(inputValue, out int v1);
                outValue = v1;
                return r;
            }
        }

        public static string SetString(object obj)
        {

            if (obj.Equals(DBNull.Value) || obj == null)
            {
                return null;
            }
            return obj.ToString();
        }


        public static string ToHexString(byte[] bytes)

        {
            string hexString = "0x";
            if (bytes != null)
            {
                System.Text.StringBuilder strB = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString += strB.ToString();
            }
            return hexString;
        }


        #region 生成And条件表达式


        public static Expression<Func<T, bool>> AndCondition<T>(string name, string value, Expression<Func<T, bool>> condition = null)
        {

            ParameterExpression p1 = Expression.Parameter(typeof(T), "p");

            GetMethodNameAndValue(value, out string methodName, out string methodValue);
            MethodCallExpression m1 = GetMethodExpression(methodName, name, methodValue, p1);
            return condition == null ? Expression.Lambda<Func<T, bool>>(m1, p1) : condition.And(Expression.Lambda<Func<T, bool>>(m1, p1));
        }

        private static void GetMethodNameAndValue(string inValue, out string methodName, out string outValue)
        {
            methodName = "Contains";
            outValue = inValue;
            if (inValue.StartsWith("%"))
            {
                methodName = "EndsWith";
                outValue = inValue.Replace("%", "");
            }
            else if (inValue.EndsWith("%"))
            {
                methodName = "StartsWith";
                outValue = inValue.Replace("%", "");
            }
            else if (inValue.StartsWith("="))
            {
                methodName = "Equals";
                outValue = inValue.Substring(1);
            }
        }

        private static MethodCallExpression GetMethodExpression(string methodName, string propertyName, string propertyValue, ParameterExpression parameterExpression)
        {
            var propertyExpression = Expression.Property(parameterExpression, propertyName);
            MethodInfo method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            return Expression.Call(propertyExpression, method, someValue);
        }
        #endregion


        /// <summary>
        /// 根据实体创建table,添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataTable ModelToTable<T>(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
                    return null;
            DataTable dt = new DataTable(typeof(T).Name);
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            }

            foreach (T model in modelList)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo p in typeof(T).GetProperties())
                {
                    dr[p.Name] = p.GetValue(model, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }


        /// <summary>
        /// 获取字符串长度。与string.Length不同的是，该方法将中文作 2 个字符计算。
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (str == null || str.Length == 0) { return 0; }

            int l = str.Length;
            int realLen = l;

            #region 计算长度
            int clen = 0;//当前长度
            while (clen < l)
            {
                //每遇到一个中文，则将实际长度加一。
                if ((int)str[clen] > 128) { realLen++; }
                clen++;
            }
            #endregion

            return realLen;
        }

    }
}
