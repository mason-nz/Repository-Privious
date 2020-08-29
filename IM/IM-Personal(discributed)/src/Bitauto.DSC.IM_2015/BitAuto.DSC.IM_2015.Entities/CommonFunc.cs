using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BitAuto.DSC.IM_2015.Entities
{
    public class CommonFunc
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
            else return obj.ToString();
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
        public static long ObjectToLong(object obj)
        {
            return ObjectToLong(obj, 0);
        }
        public static long ObjectToLong(object obj, long dvalue)
        {
            if (obj == null)
                return dvalue;
            long a = 0;
            if (long.TryParse(obj.ToString(), out a))
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
        /// 转枚举
        /// <summary>
        /// 转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ObjectToEnum<T>(object obj)
        {
            return (T)System.Enum.Parse(typeof(T), ObjectToString(obj));
        }
        /// boo转int
        /// <summary>
        /// boo转int
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int BoolToInt(bool b)
        {
            if (b) return 1;
            else return 0;
        }
        /// int转bool
        /// <summary>
        /// int转bool
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IntToBool(int b)
        {
            return (b == 1);
        }
        #endregion

        /// 拆分字符串
        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> StringToList(string s)
        {
            List<string> t = new List<string>();
            if (!string.IsNullOrEmpty(s))
            {
                string[] array = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                t = new List<string>(array);
            }
            return t;
        }

        #region xml文件读取和保存
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
        /// 获取所有节点下的匹配的内容
        /// <summary>
        /// 获取所有节点下的匹配的内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<T, S> GetAllNodeContentByFile<T, S>(string path, string nodePath, string keyNm = "key", string valueNm = "value")
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
