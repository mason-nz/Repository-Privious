using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common
{
    public static class SplitHelper
    {
        /// <summary>
        /// 对keyword进行分词，将分词的结果返回
        /// </summary>
        public static IEnumerable<string> SplitWords(string keyword)
        {
            IList<string> list = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(keyword));
            Token token = null;

            while ((token = tokenStream.Next()) != null)
            {
                // token.TermText()为当前分的词
                string word = token.TermText();
                list.Add(word);
            }

            return list;
        }
        public static string GetKeyWordsSplitBySpace(string keywords)
        {
            PanGuTokenizer ktTokenizer = new PanGuTokenizer();
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }
        /// <summary>
        /// decimal补位操作
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static string AddedDigitsFloat(decimal Num)
        {

            return (Convert.ToInt32(Num * 100) + string.Empty).PadLeft(11, '0');
        }
        public static string AddedRightFloat(decimal Num)
        {

            return (Convert.ToInt32(Num * 100) + string.Empty).PadRight(11, '0');
        }
        /// <summary>
        /// int补位操作
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static string AddedDigitsInt(int Num)
        {

            return (Num + string.Empty).PadLeft(11, '0');
        }

        ///// <summary>
        ///// 根据Value获取枚举列表
        ///// </summary>
        ///// <returns></returns>
        public static EnumInfo GetEnumDescriptionList<T>(string enumName)
        {
            return Enum.GetValues(typeof(T)).OfType<Enum>().Where(m => Convert.ToInt32(m).ToString() == enumName).Select(x => new EnumInfo
            {
                Key = x != null ? Convert.ToInt32(x) : 0,
                Value = x + string.Empty,
                Description = x.GetDescription()
            }).FirstOrDefault();
        }
        ///// <summary>
        ///// 根据Key获取枚举列表
        ///// </summary>
        ///// <returns></returns>
        public static EnumInfo GetEnumKeyDescriptionList<T>(string enumName)
        {
            return Enum.GetValues(typeof(T)).OfType<Enum>().Where(m => m.ToString() == enumName).Select(x => new EnumInfo
            {
                Key = x != null ? Convert.ToInt32(x) : 0,
                Value = x + string.Empty,
                Description = x.GetDescription()
            }).FirstOrDefault();
        }
        /// <summary>
        /// 截取字符串，并取前几条
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SplitText(string text, int length)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return string.Join(" ", text.Split(' ').ToList().Take(length));
            }
            return text;
        }

        //public static string GetDescription(this Enum @this)
        //{
        //    string _description = string.Empty;
        //    FieldInfo _fieldInfo = @this.GetType().GetField(@this.ToString());
        //    DescriptionAttribute[] _attributes = _fieldInfo.GetDescriptAttr();
        //    if (_attributes != null && _attributes.Length > 0)
        //        _description = _attributes[0].Description;
        //    else
        //        _description = @this.ToString();
        //    return _description;
        //}
        //public static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
        //{
        //    if (fieldInfo != null)
        //    {
        //        return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        //    }
        //    return null;
        //}
        //public string GetEnumDescription(Enum enumValue)
        //{
        //    string value = enumValue.ToString();
        //    FieldInfo field = enumValue.GetType().GetField(value);
        //    object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
        //    if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
        //        return value;
        //    DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
        //    return descriptionAttribute.Description;
        //}
        //public static string GetDescription(this Enum value, bool nameInstend = true)
        //{
        //    Type type = value.GetType();
        //    string name = Enum.GetName(type, value);
        //    if (name == null)
        //    {
        //        return null;
        //    }
        //    FieldInfo field = type.GetField(name);
        //    DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        //    if (attribute == null && nameInstend == true)
        //    {
        //        return name;
        //    }
        //    return attribute == null ? null : attribute.Description;
        //}
        //public static string GetDescription(this Enum @this)
        //{
        //    return _concurrentDictionary.GetOrAdd(@this, (key) =>
        //    {
        //        var type = key.GetType();
        //        var field = type.GetField(key.ToString());
        //        //如果field为null则应该是组合位域值，
        //        return field == null ? key.GetDescriptions() : GetDescription(field);
        //    });
        //}
    }
}
