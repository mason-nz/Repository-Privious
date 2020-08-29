using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Linq;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 公共类方法
    /// <summary>
    /// 公共类方法
    /// qiangfei
    /// 2014-5-27
    /// </summary>
    public static class CommonFunction
    {
        #region 转换
        /// 转字符串
        /// <summary>
        /// 转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToString(object obj)
        {
            return ObjectToString(obj, "");
        }
        public static string ObjectToString(object obj, string dvalue)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return dvalue;
            }
            else
            {
                return obj.ToString().Trim();
            }
        }
        /// 转时间
        /// <summary>
        /// 转时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return ObjectToDateTime(obj, new DateTime());
        }
        public static DateTime ObjectToDateTime(object obj, DateTime dvalue)
        {
            DateTime a = new DateTime();
            if (obj == null)
            {
                return dvalue;
            }
            if (DateTime.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return dvalue;
            }
        }
        /// 转数字
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjectToInteger(object obj)
        {
            return ObjectToInteger(obj, 0);
        }
        public static int ObjectToInteger(object obj, int dvalue)
        {
            if (obj == null)
            {
                return dvalue;
            }
            int a = 0;
            if (int.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return dvalue;
            }
        }
        /// 转数字
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ObjectToDecimal(object obj)
        {
            return ObjectToDecimal(obj, 0);
        }
        public static decimal ObjectToDecimal(object obj, decimal dvalue)
        {
            if (obj == null)
            {
                return dvalue;
            }
            decimal a = 0;
            if (decimal.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return dvalue;
            }
        }
        /// 转数字
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ObjectToDouble(object obj)
        {
            return ObjectToDouble(obj, 0);
        }
        public static double ObjectToDouble(object obj, double dvalue)
        {
            if (obj == null)
            {
                return dvalue;
            }
            double a = 0;
            if (double.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return dvalue;
            }
        }
        /// 转数字
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ObjectToLong(object obj)
        {
            return ObjectToLong(obj, 0);
        }
        public static long ObjectToLong(object obj, long dvalue)
        {
            if (obj == null)
            {
                return dvalue;
            }
            long a = 0;
            if (long.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return dvalue;
            }
        }

        /// bool和int互转
        /// <summary>
        /// bool和int互转
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int? BoolToInt(bool? b)
        {
            if (b == null)
            {
                return null;
            }
            if (b.Value)
            {
                //1是0否
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// bool和int互转
        /// <summary>
        /// bool和int互转
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool? IntToBool(int? b)
        {
            if (b == null)
            {
                return null;
            }
            return b.Value == 1;
        }

        /// 转时间
        /// <summary>
        /// 转时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ObjectToDateTimeOrNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            DateTime a = new DateTime();
            if (DateTime.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return null;
            }
        }
        /// 转数字
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ObjectToIntegerOrNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            int a = 0;
            if (int.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return null;
            }
        }
        /// 转数字
        /// <summary>
        /// 转数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long? ObjectToLongOrNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            long a = 0;
            if (long.TryParse(obj.ToString(), out a))
            {
                return a;
            }
            else
            {
                return null;
            }
        }

        /// 时间转换字符串
        /// <summary>
        /// 时间转换字符串
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetDateTimeStr(DateTime t)
        {
            return t.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// 时间转换字符串
        /// <summary>
        /// 时间转换字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDateTimeStrForPage(string value)
        {
            string f = "yyyy-MM-dd HH:mm:ss";
            return GetDateTimeStrForPage(value, f);
        }
        /// 时间转换字符串
        /// <summary>
        /// 时间转换字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static string GetDateTimeStrForPage(string value, string f)
        {
            DateTime a;
            //兼容默认值和时间戳默认值
            if (value != null && DateTime.TryParse(value, out a) && a.Date > new DateTime(1970, 1, 1))
            {
                return a.ToString(f);
            }
            else
            {
                return "";
            }
        }

        public static string DefineObjectToString(object obj)
        {
            string m = "";
            foreach (var p in obj.GetType().GetProperties())
            {
                if (p.Name.StartsWith("IsModify_") || p.Name.StartsWith("ValueOrDefault_"))
                {
                    continue;
                }
                m += p.Name + "：" + p.GetValue(obj, null) + " ";
            }
            return m;
        }
        #endregion

        #region List
        /// 增加元素
        /// <summary>
        /// 增加元素
        /// </summary>
        /// <param name="list"></param>
        /// <param name="a"></param>
        public static void AddToList<T>(List<T> list, T a)
        {
            if (list != null && !list.Contains(a))
            {
                list.Add(a);
            }
        }
        /// list转换字符串
        /// <summary>
        /// list转换字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string ListToString<T>(List<T> list, string spe)
        {
            string info = "";
            if (list != null && list.Count > 0)
            {
                foreach (T k in list)
                {
                    info += k.ToString() + spe;
                }
                info = info.Substring(0, info.Length - spe.Length);
            }
            return info;
        }
        /// 表转List
        /// <summary>
        /// 表转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt, string columnName, string orderstr, string filter)
        {
            List<T> listValue = new List<T>();
            if (dt == null) return listValue;
            foreach (DataRow dr in dt.Select(filter, orderstr))
            {
                if (dr[columnName].ToString() == "") continue;
                T obj = (T)Convert.ChangeType(dr[columnName], typeof(T));
                if (!listValue.Contains(obj))
                {
                    listValue.Add(obj);
                }
            }
            return listValue;
        }
        #endregion

        #region 文件
        /// 读取xml文件，忽略注释
        /// <summary>
        /// 读取xml文件，忽略注释
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XmlDocument ReadXmlDocument(string path)
        {
            XmlDocument xDom = new XmlDocument();
            XmlReader reader = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            try
            {
                reader = XmlReader.Create(path, settings);
                xDom.Load(reader);
            }
            catch
            {
                //文件格式异常，忽略
            }
            if (reader != null)
            {
                //释放文件
                reader.Close();
            }
            return xDom;
        }
        /// 获取xml中对应节点
        /// <summary>
        /// 获取xml中对应节点
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public static XmlNode ReadXmlSelectSingleNode(string path, string nodePath)
        {
            XmlDocument xmlDoc = ReadXmlDocument(path);
            if (nodePath == null || nodePath == "") nodePath = "/root";
            XmlNode RootNode = xmlDoc.SelectSingleNode(nodePath);
            return RootNode;
        }
        /// 获取所有节点下的匹配的内容
        /// <summary>
        /// 获取所有节点下的匹配的内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<T, S> GetAllNodeContentByFile<T, S>(string path, string keyNm, string valueNm, string nodePath)
        {
            Dictionary<T, S> cpTypeDic = new Dictionary<T, S>();
            try
            {
                //解析字符串
                XmlDocument xmlDoc = ReadXmlDocument(path);
                if (nodePath == null || nodePath == "") nodePath = "/root";
                XmlNode RootNode = xmlDoc.SelectSingleNode(nodePath);
                //递归取数
                SetDictionaryValue(ref cpTypeDic, RootNode, keyNm, valueNm, false);
            }
            catch
            {
            }
            return cpTypeDic;
        }
        /// 递归从文件取数
        /// <summary>
        /// 递归从文件取数
        /// </summary>
        /// <param name="cpTypeDic"></param>
        /// <param name="xmlNode"></param>
        /// <param name="keyNm"></param>
        /// <param name="valueNm"></param>
        /// <param name="isLastNode">是否只取最后一级节点的内容</param>
        private static void SetDictionaryValue<T, S>(ref Dictionary<T, S> cpTypeDic, XmlNode xmlNode, string keyNm, string valueNm, bool isLastNode)
        {
            T key = default(T);
            S value = default(S);
            if (xmlNode == null) return;
            bool isright = false;
            if (xmlNode.Attributes[keyNm] != null)
            {
                key = (T)Convert.ChangeType(xmlNode.Attributes[keyNm].Value, typeof(T));
                isright = true;
            }
            if (xmlNode.Attributes[valueNm] != null)
            {
                value = (S)Convert.ChangeType(xmlNode.Attributes[valueNm].Value, typeof(S));
            }
            if (xmlNode.HasChildNodes)
            {
                if (!isLastNode)
                {
                    if (isright && !cpTypeDic.ContainsKey(key))
                    {
                        cpTypeDic.Add(key, value);
                    }
                }
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    SetDictionaryValue(ref cpTypeDic, childNode, keyNm, valueNm, isLastNode);
                }
            }
            else
            {
                if (isright && !cpTypeDic.ContainsKey(key))
                {
                    cpTypeDic.Add(key, value);
                }
            }
        }
        /// 保存Dictionary到xml文件
        /// <summary>
        /// 保存Dictionary到xml文件
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="groupname"></param>
        /// <param name="path"></param>
        public static void SaveDictionaryToFile<T, S>(Dictionary<T, S> dic, string keyNm, string valueNm, string path)
        {
            if (dic == null || dic.Count == 0) return;
            Dictionary<T, S> old_dic = new Dictionary<T, S>();
            //获取文件中的Dictionary
            old_dic = GetAllNodeContentByFile<T, S>(path, keyNm, valueNm, null);
            //合并
            foreach (T key in dic.Keys)
            {
                old_dic[key] = dic[key];
            }
            //保存
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine("<root>");
            foreach (T key in old_dic.Keys)
            {
                sb.AppendLine("\t\t<item " + keyNm + "=\"" + key.ToString() + "\" "
                    + valueNm + "=\"" + old_dic[key].ToString().Replace(">", "&gt;").Replace("<", "&lt;") + "\"/>");
            }
            sb.AppendLine("</root>");
            //写入文件
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create, FileAccess.Write)))
            {
                sw.Write(sb.ToString());
            }
        }
        /// 保存Dictionary到xml文件
        /// <summary>
        /// 保存Dictionary到xml文件
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="groupname"></param>
        /// <param name="path"></param>
        public static void SaveDictionaryToFile(Dictionary<string, Dictionary<string, string>> Listdic, string path)
        {
            if (Listdic == null || Listdic.Count == 0) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine("<!--程序自动生成-->");
            sb.AppendLine("<root>");
            int i = 0;
            foreach (string groupname in Listdic.Keys)
            {
                sb.AppendLine("\t<" + groupname + ">");
                foreach (string key in Listdic[groupname].Keys)
                {
                    sb.AppendLine("\t\t<item key=\"" + key + "\" value=\"" + Listdic[groupname][key].Replace(">", "&gt;").Replace("<", "&lt;") + "\"/>");
                }
                sb.AppendLine("\t</" + groupname + ">");
                i++;
            }
            sb.AppendLine("</root>");
            //写入文件
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create, FileAccess.Write)))
            {
                sw.Write(sb.ToString());
            }
        }
        #endregion
    }
}
