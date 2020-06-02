using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Ui.View.Test
{
    public class StudentInfoWithValidation : IDataErrorInfo
 {
    #region 构造方法
    public StudentInfoWithValidation()
   {
        StudentName = "Tom";
        Score = 90;
   }
    public StudentInfoWithValidation(string m_StudentName, double m_Score)
    {
        StudentName = m_StudentName;
         Score = m_Score;
   }
    #endregion
 
     #region 属性
    public string StudentName
    {
       get; set;
   }
    public double Score
   {
       get; set;
  }
   #endregion

   #region 实现IDataErrorInfo接口的成员
   public string Error
   {
     get 
     {
          return null;
       }
    }

    public string this[string columnName]
   {
        get
     {
          string result = null;
 
       switch (columnName)
          {
             case "StudentName":
                 // 设置StudentName属性的验证规则
                   int len = StudentName.Length;
                      if (len < 2 || len > 10)
                 {
                              result = "StudentName length must between 2 and 10";
                          }
                    break;
                    case "Score":
                 // 设置Score属性的验证规则
                    if (Score < 0 || Score > 100)
                          {
                           result = "Score must between 0 and 100";
                           }
                      break;
               }
 
          return result;
       }
  }
    #endregion
 }
}
