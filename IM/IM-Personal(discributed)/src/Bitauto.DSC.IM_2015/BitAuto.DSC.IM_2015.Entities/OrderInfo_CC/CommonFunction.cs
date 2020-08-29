using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;

namespace BitAuto.DSC.IM_2015.Entities
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
                return dvalue;
            else return obj.ToString().Trim();
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
                return dvalue;
            if (DateTime.TryParse(obj.ToString(), out a))
                return a;
            else return dvalue;
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
                return dvalue;
            int a = 0;
            if (int.TryParse(obj.ToString(), out a))
                return a;
            else return dvalue;
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
                return dvalue;
            decimal a = 0;
            if (decimal.TryParse(obj.ToString(), out a))
                return a;
            else return dvalue;
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
                return dvalue;
            double a = 0;
            if (double.TryParse(obj.ToString(), out a))
                return a;
            else return dvalue;
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
        public static string ListToString(List<string> list, string a)
        {
            if (list == null) return "";
            return string.Join(a, list.ToArray());
        }
        #endregion

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

        /// 时间转换
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetDateTimeStr(DateTime t)
        {
            return t.ToString("yyyy-MM-dd HH:mm:ss");
        }

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
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(path, settings);
            xDom.Load(reader);
            reader.Close();
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
            //解析字符串
            XmlDocument xmlDoc = ReadXmlDocument(path);
            if (nodePath == null || nodePath == "") nodePath = "/root";
            XmlNode RootNode = xmlDoc.SelectSingleNode(nodePath);
            //递归取数
            SetDictionaryValue(ref cpTypeDic, RootNode, keyNm, valueNm, false);
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
            //获取文件中的Dictionary
            Dictionary<T, S> old_dic = GetAllNodeContentByFile<T, S>(path, keyNm, valueNm, null);
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
        #endregion

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
    }
}
