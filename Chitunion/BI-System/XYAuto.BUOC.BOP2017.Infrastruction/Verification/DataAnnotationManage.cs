using System;
using System.Collections.Generic;
using System.Reflection;

namespace XYAuto.BUOC.BOP2017.Infrastruction.Verification
{
    public class DataAnnotationManage
    {
        public Dictionary<string, string> DisplaySelfAttribute<T>(T item) where T : class, new()
        {
            var parDic = new Dictionary<string, string>();

            var tp = typeof(T);
            if (item == null)
            {
                parDic.Add(tp.ToString(), "请输入参数");
                return parDic;
            }
            //得到属性上的标记属性信息即列名
            foreach (PropertyInfo pi in tp.GetProperties())
            {
                var objAttributes = pi.GetCustomAttributes(typeof(NecessaryAttribute), true);
                if (objAttributes.Length <= 0) continue;
                var cnAttribute = objAttributes[0] as NecessaryAttribute;
                if (cnAttribute == null) continue;
                //todo
                var value = pi.GetValue(item, null) == null ? string.Empty : pi.GetValue(item, null).ToString();
                if (string.IsNullOrEmpty(value))
                {
                    var errorMsg = string.IsNullOrEmpty(cnAttribute.Message) ? "请输入参数:" + cnAttribute.MtName : string.Format(cnAttribute.Message, cnAttribute.MtName);
                    parDic.Add(cnAttribute.MtName, errorMsg);
                }
                else
                {
                    if (!cnAttribute.IsValidateThanAt) continue;
                    if (value.ToFloat() <= cnAttribute.ThanMaxValue)
                    {
                        var errorMsg = string.IsNullOrEmpty(cnAttribute.Message) ? "请输入参数:" + cnAttribute.MtName : string.Format(cnAttribute.Message, cnAttribute.MtName, cnAttribute.ThanMaxValue);
                        parDic.Add(cnAttribute.MtName, errorMsg);
                    }
                }
            }
            return parDic;
        }
    }

    public static class ConvertToInt
    {
        public static int ToInt(this string strParam, int defaultValue = 0)
        {
            int outPutInt;
            if (!int.TryParse(strParam, out outPutInt))
                outPutInt = defaultValue;
            return outPutInt;
        }

        public static decimal ToDecimal(this string strParam, int defaultValue = 0)
        {
            decimal outPutDecimal;
            if (!decimal.TryParse(strParam, out outPutDecimal))
                outPutDecimal = defaultValue;
            return outPutDecimal;
        }

        public static float ToFloat(this string strParam, int defaultValue = 0)
        {
            float outPutInt;
            if (!float.TryParse(strParam, out outPutInt))
                outPutInt = defaultValue;
            return outPutInt;
        }

        public static bool ToBoolean(this string strParams, bool defaultValue = false)
        {
            bool outPutBool;
            if (!bool.TryParse(strParams, out outPutBool))
                outPutBool = defaultValue;
            return outPutBool;
        }

        public static bool ToBoolean(this int strParams, bool defaultValue = false)
        {
            try
            {
                return Convert.ToBoolean(strParams);
            }
            catch
            {
                return defaultValue;
            }
        }
    }

    //类名以Attribute结尾,继承于Attribute
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]//指明作用对象
    public class NecessaryAttribute : Attribute
    {
        //至少保护一个构造函数
        public NecessaryAttribute()
        {
        }

        /// <summary>
        /// 必须字段
        /// </summary>
        public string MtName
        {
            get;
            set;
        }

        public bool IsValidateThanAt { get; set; }
        public int ThanMaxValue { get; set; }
        public string Message { get; set; }
    }
}