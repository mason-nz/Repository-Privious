using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ConverToEntitie<T> where T : class
    {
        public T TargetClass;

        Type type;
        PropertyInfo[] propertyList;

        public ConverToEntitie(T obj)
        {
            this.TargetClass = obj;

            type = typeof(T);
            propertyList = type.GetProperties();

        }

        /// <summary>
        /// 将字符串转换成指定类型的实体类
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public string Conver(string jsonStr)
        {
            string errMsg = "";

            string[] list = jsonStr.Split('&');
            string key = "";
            string val = "";

            foreach (string item in list)
            {
                if (item.Split('=').Length > 1)
                {
                    key = item.Split('=')[0];
                    val = item.Split('=')[1];
                }

                if (key != "" && val != "")
                {
                    errMsg = SetValToProperty(key, HttpUtility.UrlDecode(val));
                }
                if (errMsg != "")
                {
                    break;
                }
            }

            return errMsg;
        }

        /// <summary>
        /// 给属性赋值
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="val">属性值</param>
        /// <returns>错误信息</returns>
        private string SetValToProperty(string key, string val)
        {
            string errMsg = "";

            int intval = 0;
            DateTime timeval;
            Decimal decimalval = 0;
            long longval = 0;
            bool boolval = false;

            PropertyInfo info = type.GetProperty(key);
            if (info != null)
            {
                if (info.PropertyType.FullName.IndexOf("System.Int32") != -1)
                {
                    if (int.TryParse(val, out intval))
                    {
                        info.SetValue(TargetClass, intval, null);
                    }

                }
                else if (info.PropertyType.FullName.IndexOf("System.String") != -1)
                {
                    info.SetValue(TargetClass, val, null);
                }
                else if (info.PropertyType.FullName.IndexOf("System.DateTime") != -1)
                {
                    if (DateTime.TryParse(val, out timeval))
                    {
                        info.SetValue(TargetClass, timeval, null);
                    }

                }
                else if (info.PropertyType.FullName.IndexOf("System.Decimal") != -1)
                {
                    if (Decimal.TryParse(val, out decimalval))
                    {
                        info.SetValue(TargetClass, Decimal.Parse(val), null);
                    }

                }
                else if (info.PropertyType.FullName.IndexOf("System.Int64") != -1)//对应long类型
                {
                    if (long.TryParse(val, out longval))
                    {
                        info.SetValue(TargetClass, long.Parse(val), null);
                    }

                }
                else if (info.PropertyType.FullName.IndexOf("System.Boolean") != -1)//对应long类型
                {
                    if (bool.TryParse(val, out boolval))
                    {
                        info.SetValue(TargetClass, bool.Parse(val), null);
                    }

                }
            }
            return errMsg;
        }



    }
}
