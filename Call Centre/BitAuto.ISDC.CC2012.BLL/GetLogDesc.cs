using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace BitAuto.ISDC.CC2012.BLL
{
    public static class GetLogDesc
    {
        #region 属性

        //这两个数组可以不被定义
        //作用：日志里面的属性名可以被描述名代替

        public static Hashtable ht_FieldName = new Hashtable(); //属性名和对应的真实描述名

        public static Hashtable ht_FieldType = new Hashtable(); //属性类型
          
        #endregion

        /// <summary>
        /// 编辑时：获取对比过后编辑的信息
        /// </summary>
        /// <param name="oldObj">修改前的对象</param>
        /// <param name="newObj">修改后的对象</param>
        /// <param name="logMsg">返回存在改动的属性日志信息</param>
        public static void getCompareLogInfo(object oldObj, object newObj, out string logMsg)
        {
            logMsg = string.Empty;

            if (oldObj == null)
            {
                return;
            }

            PropertyInfo[] list = oldObj.GetType().GetProperties();

            foreach (PropertyInfo info in list)
            {
                object oldValue = info.GetValue(oldObj, null);
                object newValue = info.GetValue(newObj, null);

                if (info.PropertyType.FullName.IndexOf("System.String") != -1)
                {
                    //获取info该属性的值
                    if (oldValue != null)
                    {
                        oldValue = oldValue.ToString();
                    }
                    else { oldValue = ""; }

                    if (newValue != null)
                    {
                        newValue = newValue.ToString();
                    }
                    else { newValue = ""; }
                }
                else if (info.PropertyType.FullName.IndexOf("System.Int32") != -1)
                {
                    //获取info该属性的值
                    if (oldValue != null)
                    {
                        oldValue = int.Parse(oldValue.ToString());
                    }
                    else { oldValue = ""; }

                    if (newValue != null)
                    {
                        newValue = int.Parse(newValue.ToString());
                    }
                    else { newValue = ""; }
                }
                else if (info.PropertyType.FullName.IndexOf("System.DateTime") != -1)
                {
                    //获取info该属性的值
                    if (oldValue != null)
                    {
                        oldValue = DateTime.Parse(oldValue.ToString());
                    }
                    else { oldValue = ""; }

                    if (newValue != null)
                    {
                        newValue = DateTime.Parse(newValue.ToString());
                    }
                    else { newValue = ""; }
                }
                else if (info.PropertyType.FullName.IndexOf("System.Decimal") != -1)
                {
                    //获取info该属性的值
                    if (oldValue != null)
                    {
                        oldValue = Decimal.Parse(oldValue.ToString());
                    }
                    else { oldValue = ""; }

                    if (newValue != null)
                    {
                        newValue = Decimal.Parse(newValue.ToString());
                    }
                    else { newValue = ""; }
                }

                //对比
                if ((oldValue != null) && (newValue != null) && !oldValue.Equals(newValue))
                {
                    logMsg += "属性 -> " + getRealName(info.Name) + " 从：" + getTrueNameByType(info.Name, oldValue) + " 修改为：" + getTrueNameByType(info.Name, newValue) + "；";
                }
            }
        }

        /// <summary>
        /// 新增时：获取新增的信息
        /// </summary>
        /// <param name="obj">新增的对象</param>
        /// <param name="logMsg">返回新增不为空的属性的日志信息</param>
        public static void getAddLogInfo(object obj, out string logMsg)
        {
            logMsg = string.Empty;

            if (obj == null)
            {
                return;
            }

            PropertyInfo[] list = obj.GetType().GetProperties();

            foreach (PropertyInfo info in list)
            {
                object properValue = info.GetValue(obj, null);
                string newValue = string.Empty;

                if (properValue != null)
                {
                    newValue = properValue.ToString();
                }

                if (newValue != string.Empty || newValue != null)
                {
                    logMsg += "属性 -> " + getRealName(info.Name) + " 值：" + getTrueNameByType(info.Name, newValue) + "；";
                }
            }
        }
         
        /// <summary>
        /// 删除时：获取删除的信息
        /// </summary>
        /// <param name="obj">删除的对象</param>
        /// <param name="logMsg">返回删除不为空的属性的日志信息</param>
        public static void getDeleteLogInfo(object obj, out string logMsg)
        {
            logMsg = string.Empty;

            PropertyInfo[] list = obj.GetType().GetProperties();

            foreach (PropertyInfo info in list)
            {
                object properValue = info.GetValue(obj, null);
                string newValue = string.Empty;

                if (properValue != null)
                {
                    newValue = properValue.ToString();
                }

                if (newValue != string.Empty || newValue != null)
                {
                    logMsg += "属性 -> " + getRealName(info.Name) + " 值：" + getTrueNameByType(info.Name, newValue) + "；";
                }
            }
        }
         
        /// <summary>
        /// 通过属性名找到真实的字段描述名
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>返回真实的字段描述名</returns>
        private static string getRealName(string name)
        {
            string realName = string.Empty;

            realName = ht_FieldName[name] == null ? name : ht_FieldName[name].ToString();

            return realName;
        }

        //根据类型找到对应值
        private static string getTrueNameByType(string name, object value)
        {
            string resultStr = value.ToString(); 

            object o = ht_FieldType[name];

            if (o == null)
            {
                return resultStr;
            }

            if (o.GetType() == typeof(string))
            {
                switch (o.ToString())
                {
                    case "UserID": resultStr = getUserName(value);
                        break;
                }
            }
            else if (o.GetType() == typeof(Hashtable))
            {
                Hashtable item = o as Hashtable;

                resultStr = item[value.ToString()] == null ? value.ToString() : item[value.ToString()].ToString();
            }
             
            return resultStr;
        }
         
        //根据userID得到userName
        private static string getUserName(object value)
        {
            return BLL.Util.GetNameInHRLimitEID(int.Parse(value.ToString()));
        }
    }
}
