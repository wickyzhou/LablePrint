using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ui.Helper
{
    public static class ModelTypeHelper
    {
        public static void PropertyMapper<T1, T2>(T1 source, T2 destination)
        {
            try
            {
                var destinationTypes = destination.GetType();
                var sourceTypes = source.GetType();
                foreach (PropertyInfo sp in sourceTypes.GetProperties())
                {
                    foreach (PropertyInfo dp in destinationTypes.GetProperties())
                    {
                        if (sp.Name == dp.Name)
                        {
                            sp.SetValue(source, dp.GetValue(destination, null), null);//获得d对象属性的值复制给s对象的属性
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T Clone<T>(this T obj) where T : class
        {
            if (obj is string || obj.GetType().IsValueType)
                return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    field.SetValue(retval, Clone(field.GetValue(obj)));
                }
                catch { }
            }
            return (T)retval;
        }

    }
}
