using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xy.ImageFastDFS;
using XYAuto.ITSC.Chitunion2017.HBaseDataSync.WangyiDun;
using XYAuto.Utils.Config;
using ZXing;
using XYAuto.ITSC.Chitunion2017.HBaseDataSync.Entities;
using System.Threading;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync
{
    public class Util
    {
        protected static int FilterADImgHeight = int.Parse(ConfigurationUtil.GetAppSettingValue("FilterADImgHeight").ToString());//剔除规则:高度小于200的图片，删除所在标签
        protected static string FilterADWords = ConfigurationUtil.GetAppSettingValue("FilterADWords");//剔除广告关键词信息
        protected static string FilterADContentWords = ConfigurationManager.AppSettings["FilterADContentWords"];//剔除文章内容中的关键词信息
        protected static string CleanImgURLPrefix = ConfigurationUtil.GetAppSettingValue("CleanImgURLPrefix");//文章中替换图片url后的域名
        protected static string FilterArticleTitleWords = ConfigurationUtil.GetAppSettingValue("FilterArticleTitleWords");//剔除文章标题关键词信息
        protected static int ArticleOverContentLen = int.Parse(ConfigurationUtil.GetAppSettingValue("ArticleOverContentLen").ToString());//文章内容最小文本长度限制

        public readonly static Util Instance = new Util();
        private static Dictionary<String, ImageFormat> _imageFormats;

        public static Dictionary<String, ImageFormat> ImageFormats
        {
            get
            {
                return _imageFormats ?? (_imageFormats = GetImageFormats());
            }
        }

        /// <summary>
        /// 根据文章的HTML内容，剔除广告部分
        /// 剔除规则：
        /// 1、根据文章内容，找到缺少的nbsp;空格标签，替换完整;
        /// 2、二维码图片，删除所在标签；
        /// 3、高度小于200的图片，删除所在标签；
        /// 4、所有带链接的（账号内其他文章链接）、小程序的标签，删除所在标签；
        /// 5、删除关键词相关的标签；
        /// </summary>
        /// <param name="content">微信公众号内容</param>
        /// <param name="title">文章标题</param>
        /// <param name="SN">微信文章的唯一标识</param>
        /// <param name="type">文章类型（1-微信公众号，2-今日头条）</param>
        /// <param name="articleUrl">文章URL</param>
        /// <returns></returns>
        public string FilterADByContent(string content, string title, string sn, int type, string articleUrl, Entities.ArticleInfo article)
        {
            string result = content;
            string[] words = FilterADContentWords.Split(',');
            result = Regex.Replace(result, @"(?<![&])nbsp;", "&nbsp;");//根据文章内容，找到缺少的nbsp;空格标签，替换完整;

            var doc = new HtmlDocument();
            doc.LoadHtml(result.Trim());

            switch (type)
            {
                case 1://微信公众号
                    if (IsHasVideo(sn, doc) || IsHasAudio(sn, doc))
                    //IsHasWords(sn, content, words) || IsHasWords(sn, title, words))
                    {
                        if (IsOverContentLen(sn, doc, 400))//若文章文本内容小于400长度，剔除此文章
                        {
                            return string.Empty;
                        }
                        else//否则，则删除视频、音频相关标签，保留文章文本内容
                        {
                            FilterCustomTag(sn, doc, "//iframe[contains(@data-src,'v.qq.com')]|//iframe[contains(@src,'v.qq.com')]");
                            FilterCustomTag(sn, doc, "//mpvoice|//qqmusic");
                        }
                    }
                    if (!IsHasImageByContent(sn, doc))
                    {
                        return string.Empty;
                    }
                    FilterImage(sn, doc, type, article);
                    FilterATag(sn, doc);
                    FilterCustomTag(sn, doc, "//table");
                    FilterCustomTag(sn, doc, "//script");
                    FilterCustomTag(sn, doc, "//iframe");
                    FilterCustomTag(sn, doc, "//span");
                    FilterKeyWords(sn, doc);
                    FilterKeyWordsByRegex(sn, doc);
                    FilterOverLengthTagByRegex(sn, doc);
                    FilterStyleImage(sn, doc);
                    FilterMiniprogramString(sn, doc);//删除html中包含小程序字样的标签
                    if (IsOverContentLen(sn, doc, ArticleOverContentLen))
                    {
                        return string.Empty;
                    }
                    //临时注释（调用网易易盾的接口，监测文本是否符合规范）
                    if (!CheckContent(sn, doc.DocumentNode.InnerText, articleUrl))
                    {
                        return string.Empty;
                    }
                    break;
                case 2://今日头条
                    //if (IsHasWords(sn, content, words) || IsHasWords(sn, title, words))
                    //{
                    //    return string.Empty;
                    //}
                    if (IsHasVideo(sn, doc) || IsHasAudio(sn, doc))
                    {
                        if (IsOverContentLen(sn, doc, 400))//若文章文本内容小于400长度，剔除此文章
                        {
                            return string.Empty;
                        }
                        else//否则，则删除视频、音频相关标签，保留文章文本内容
                        {
                            FilterCustomTag(sn, doc, "//iframe[contains(@data-src,'v.qq.com')]|//iframe[contains(@src,'v.qq.com')]");
                            FilterCustomTag(sn, doc, "//mpvoice|//qqmusic");
                        }
                    }
                    if (!IsHasImageByContent(sn, doc))
                    {
                        return string.Empty;
                    }
                    //临时注释
                    //if (!CheckContent(sn, doc.DocumentNode.InnerText, articleUrl))
                    //{
                    //    return string.Empty;
                    //}
                    FilterImage(sn, doc, type);
                    FilterCustomTag(sn, doc, "//table");
                    FilterCustomTag(sn, doc, "//script");
                    FilterCustomTag(sn, doc, "//iframe");
                    FilterCustomTag(sn, doc, "//span");
                    FilterKeyWords(sn, doc);
                    FilterKeyWordsByRegex(sn, doc);
                    FilterOverLengthTagByRegex(sn, doc);
                    FilterStyleImage(sn, doc);
                    if (IsOverContentLen(sn, doc, ArticleOverContentLen))
                    {
                        return string.Empty;
                    }
                    break;
                default:
                    break;
            }
            StringBuilder sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        private void FilterMiniprogramString(string sn, HtmlDocument doc)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//*[text()='小程序']");
            if (list != null && list.Count() > 0)
            {
                int count = 0;
                for (int j = 0; j < list.Count(); j++)
                {
                    RemoveSpanTagByNode(list.ToList()[j]);
                    count++;
                }
                if (count > 0)
                {
                    BLL.Loger.Log4Net.Info($"文章ID为{sn}中,找到匹配的微信小程序标签，共有{count}个，已全部剔除所在span标签，直到不是span为止");
                }
            }
        }

        private void RemoveSpanTagByNode(HtmlNode currentNode)
        {
            if (currentNode != null)
            {
                if (currentNode.ParentNode != null &&
                    currentNode.ParentNode.Name.ToLower().Trim() == "span")
                {
                    RemoveSpanTagByNode(currentNode.ParentNode);
                    if (currentNode != null)
                    {
                        currentNode.Remove();
                    }
                }
                if (currentNode.Name.ToLower().Trim() == "span")
                {
                    currentNode.Remove();
                }
            }
        }

        /// <summary>
        /// 调用网易易盾的接口，监测是否符合规范
        /// </summary>
        /// <param name="sn">文章主键ID</param>
        /// <param name="innerText">文章内容</param>
        /// <returns></returns>
        private bool CheckContent(string sn, string innerText, string articleUrl)
        {
            if (!string.IsNullOrWhiteSpace(innerText))
            {
                TextResult tr = TextCheckApi.Instance.Check(sn, innerText, articleUrl);
                if (tr != null && tr.code == 200 &&
                    //(tr.result.action == 0 || (tr.result.action == 1 && CheckTextLevel(tr.result.labels))))
                    (tr.result.action == 0))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检验文本，是否符合具体规范，若label=200，600，level=1，需要过滤掉
        /// </summary>
        /// <param name="labels"></param>
        /// <returns>符合规范返回True，否则返回False</returns>
        private bool CheckTextLevel(List<Label> labels)
        {
            if (labels != null && labels.Count > 0)
            {
                int c = 0;
                foreach (Label lbl in labels)
                {
                    if (lbl.level == 1 && (lbl.label == 200 || lbl.label == 600))
                    {
                        c++;
                    }
                }
                if (c > 0 && c == labels.Count)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckImgContent(string sn, string imgUrl, string articleUrl)
        {
            bool flag = false;
            if (!string.IsNullOrWhiteSpace(imgUrl))
            {
                ImgResult ir = ImageCheckApi.Instance.Check(sn, imgUrl, articleUrl);
                if (ir != null && ir.code == 200 && ir.result != null && ir.result.Count > 0
                    && ir.result[0].labels.Count > 0)
                {
                    flag = true;
                    foreach (var item in ir.result[0].labels)
                    {
                        if (item.level != 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            return flag;
        }


        private bool IsHasWords(string sn, string content, string[] words)
        {
            if (words != null && words.Length > 0)
            {
                foreach (string word in words)
                {
                    if (string.IsNullOrWhiteSpace(word) == false && content.Contains(word.Trim()))
                    {
                        BLL.Loger.Log4Net.Info($"文章ID为{sn}中,内容有“{word}”这个关键词");
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 文章内容中，是否包含视频
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HTML内容</param>
        /// <returns>包含则返回true，否则返回false</returns>
        private bool IsHasVideo(string sn, HtmlDocument doc)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//iframe[contains(@data-src,'v.qq.com')]|//iframe[contains(@src,'v.qq.com')]");
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个视频iframe标签");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 文章内容中，是否包含音频
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HTML内容</param>
        /// <returns>包含则返回true，否则返回false</returns>
        private bool IsHasAudio(string sn, HtmlDocument doc)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//mpvoice|//qqmusic");
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个音频标签");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetRecIDsByDataTable(DataTable dt, string colunmName)
        {
            try
            {
                string str = string.Empty;
                if (dt != null)
                {
                    int id = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        int.TryParse(dr[colunmName].ToString(), out id);
                        str += id + ",";
                    }
                }
                return str.TrimEnd(',');
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("GetRecIDsByDataTable-Error", ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// 剔除关键词
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HtmlDocument</param>
        private void FilterKeyWords(string sn, HtmlDocument doc)
        {
            string expStr = GetFilterKeyWordsXpathExpression(FilterADWords);
            if (!string.IsNullOrWhiteSpace(expStr))
            {
                IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes(expStr);
                if (list != null && list.Count() > 0)
                {
                    BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个包含有特定关键词的标签");
                    for (int j = 0; j < list.Count(); j++)
                    {
                        //list.ToList()[j].Remove();
                        DeleteNodeByNode(list.ToList()[j]);
                    }
                    BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个包含有特定关键词的标签，已全部剔除");
                }
            }
        }

        private static void DeleteNodeByNode(HtmlNode node)
        {
            //HtmlNode parentNode = list.ToList()[    j].ParentNode;
            if (node != null && !string.IsNullOrWhiteSpace(node.Name) && node.Name.ToLower().Trim() == "p")
            {
                node.Remove();
            }
            else
            {
                //list.ToList()[j].Remove();
                DeleteNodeByNode(node.ParentNode);
            }
        }

        /// <summary>
        /// 根据配置文件中的filterADWords配置，生成Xpath查询表达式
        /// </summary>
        /// <param name="filterADWords">关键词列表，如：“未经授权不得转载;支持原创 长按赞赏;”等</param>
        /// <returns>生成Xpath表达式</returns>
        private string GetFilterKeyWordsXpathExpression(string filterADWords)
        {
            string expression = string.Empty;
            string[] array = filterADWords.Split(';');
            if (array.Length > 0)
            {
                foreach (string item in array)
                {
                    string[] subArray = item.Split(' ');
                    if (subArray.Length > 0 && subArray.Length == 1)
                    {
                        expression += $"//p/descendant-or-self::*[contains(text(),'{subArray[0]}')]|";
                    }
                    else if (subArray.Length > 0 && subArray.Length > 1)
                    {
                        expression += "//p/descendant-or-self::*[";
                        foreach (string subItem in subArray)
                        {
                            expression += $"contains(text(),'{subItem}') and ";
                        }
                        expression = expression.Substring(0, expression.Length - 5);
                        expression += "]|";
                    }
                }
                expression = expression.TrimEnd('|');
            }
            return expression;
        }

        /// <summary>
        /// 若文章内容中，汉字长度小于800，则返回True，剔除文章
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="articleOverContentLen">验证文本长度</param>
        /// <returns></returns>
        private bool IsOverContentLen(string sn, HtmlDocument doc, int articleOverContentLen)
        {
            if (doc != null && BLL.Util.GetLength(doc.DocumentNode.InnerText) < articleOverContentLen)
            {
                BLL.Loger.Log4Net.Info($"文章ID={sn}，文章内容文字长度小于{articleOverContentLen}，已剔除");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据正则，获取指定标签中InnerText内容，来判断长度是否满足需求，进行剔除操作
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="doc"></param>
        public void FilterOverLengthTagByRegex(string sn, HtmlDocument doc)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//*");
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个标签");
                Regex rgTitleInnerText = new Regex("更多(.){0,2}(视频|阅读|文章)");//且小于10个字,
                foreach (HtmlNode item in list)
                {
                    if ((rgTitleInnerText.IsMatch(item.InnerText) &&
                        BLL.Util.GetLength(item.InnerText) < 20) ||
                        ((item.InnerText.Contains("相关链接") || item.InnerText.Contains("相关文章") || item.InnerText.Contains("相关阅读"))
                         && BLL.Util.GetLength(item.InnerText) < 30))
                    {
                        item.Remove();
                        BLL.Loger.Log4Net.Info($"文章ID为{sn}中,标签中有不满足规则的InnerText内容，已剔除");
                    }
                }
            }
        }

        /// <summary>
        /// 剔除A标签（里面有href属性的）
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HtmlDocument</param>
        private void FilterATag(string sn, HtmlDocument doc)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//a[@href]");
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个a标签(有href属性的)");
                for (int j = 0; j < list.Count(); j++)
                {
                    RemovePTagByNode(list, j);
                }
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个a标签(有href属性的)，已全部剔除");
            }
        }

        /// <summary>
        /// 剔除文本中，根据正则表达式匹配出来的标签的上级P标签
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="doc"></param>
        private void FilterKeyWordsByRegex(string sn, HtmlDocument doc)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//*[text()!='']");
            if (list != null && list.Count() > 0)
            {
                int count = 0;
                for (int j = 0; j < list.Count(); j++)
                {
                    string text = list.ToList()[j].InnerText;
                    if (Regex.IsMatch(text, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?") ||
                        Regex.IsMatch(text, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)"))
                    {
                        RemovePTagByNode(list, j);
                        count++;
                    }
                }
                if (count > 0)
                {
                    BLL.Loger.Log4Net.Info($"文章ID为{sn}中,在所有标签中，查找有正则匹配URL或Email的，共有{count}个，已全部剔除父级P标签");
                }
            }
        }

        /// <summary>
        /// 若当前标签上一级为p标签，则删除掉
        /// </summary>
        /// <param name="list">html节点list</param>
        /// <param name="j">当前索引值</param>
        private static void RemovePTagByNode(IEnumerable<HtmlNode> list, int j)
        {
            HtmlNode parentNode = list.ToList()[j].ParentNode;
            if (parentNode != null && !string.IsNullOrWhiteSpace(parentNode.Name) && parentNode.Name.ToLower().Trim() == "p")
            {
                parentNode.Remove();
            }
            else
            {
                list.ToList()[j].Remove();
            }
        }

        /// <summary>
        /// 剔除table标签
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HtmlDocument</param>
        /// <param name="customTag">html标签名称</param>
        private void FilterCustomTag(string sn, HtmlDocument doc, string customTag)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes(customTag);
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个[{customTag}]标签");
                foreach (HtmlNode item in list)
                {
                    if (customTag == "//span")
                    {
                        Regex rgSpanInnerText = new Regex("by(.*)网(.*)");
                        if (rgSpanInnerText.IsMatch(item.InnerText))
                        {
                            BLL.Loger.Log4Net.Info($"文章ID为{sn}中,有span标签内容【{item.InnerText}】被匹配，需要清除InnerText");
                            HtmlNode newSpan = HtmlNode.CreateNode("<span></span>");
                            foreach (var attrSpan in item.Attributes)
                            {
                                newSpan.SetAttributeValue(attrSpan.Name, attrSpan.Value);
                            }
                            item.ParentNode.ReplaceChild(newSpan, item);
                        }
                        continue;
                    }
                    item.Remove();
                }
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个[{customTag}]标签，已全部剔除");
            }
        }

        /// <summary>
        /// 根据文章标题，检查是否可用
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <returns>若检测通过，则返回文章标题内容，若不通过则返回空字符串</returns>
        public string FilterArticleTitle(string title)
        {
            string[] tags = FilterArticleTitleWords.Split(';');
            foreach (string tag in tags)
            {
                if (title.Contains(tag))
                {
                    return string.Empty;
                }
            }
            return title;
        }

        /// <summary>
        /// 判断文章内容是否包含img标签
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HtmlDocument</param>
        /// <returns></returns>
        private bool IsHasImageByContent(string sn, HtmlDocument doc)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//img[@data-src|@src]");
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个img标签");
                return true;
            }
            else
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,没有img标签");
                return false;
            }
        }

        /// <summary>
        /// 文档中，替换标签属性【style】中存的url内容
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HtmlDocument</param>
        private void FilterStyleImage(string sn, HtmlDocument doc)
        {
            Regex rUrl = new Regex(@"((ht|f)tps?):\/\/[\w\-]+(\.[\w\-]+)+([\w\-\.,@?^=%&:\/~\+#]*[\w\-\@?^=%&\/~\+#])?");
            int replaceCount = 0;
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//*[contains(@style,\"url\")]");
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个包含图片的标签，稍后要图片本地化");
                for (int j = 0; j < list.Count(); j++)
                {
                    string val = list.ToList()[j].Attributes["style"].Value;
                    MatchCollection matchs = rUrl.Matches(val);
                    for (int i = 0; i < matchs.Count; i++)
                    {
                        string url = matchs[i].Value;
                        if (url.EndsWith("&quot"))
                        {
                            url = url.Substring(0, url.Length - 5);
                        }
                        Image img = GetImageByURL(url);
                        if (img != null)
                        {
                            string qrCode = ReadQrCodeText(img);
                            if (!string.IsNullOrWhiteSpace(qrCode))
                            {
                                BLL.Loger.Log4Net.Info($"样式style中，背景图片[{url}]中有二维码，内容为[{qrCode}]，已经替换为字符串空，文章ID为：{sn}");
                                val = val.Replace(url, string.Empty);
                                replaceCount++;
                            }
                            else
                            {
                                string imageUrl = CleanImg(img, url);//图片本地化
                                if (!string.IsNullOrWhiteSpace(imageUrl))
                                {
                                    val = val.Replace(url, CleanImgURLPrefix + imageUrl);//替换原有图片内容
                                    replaceCount++;
                                    BLL.Loger.Log4Net.Info($"文章ID为{sn}中,原来URL为：{url}的图片，替换为：{CleanImgURLPrefix + imageUrl}，操作成功");
                                }
                            }
                        }
                    }
                    list.ToList()[j].Attributes["style"].Value = val;//写回document对象中去
                }
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个包含图片的标签，图片本地化完成，共替换{replaceCount}个。");
            }
        }


        /// <summary>
        /// 剔除图片标签相关
        /// </summary>
        /// <param name="sn">文章唯一标识</param>
        /// <param name="doc">HtmlDocument</param>
        private void FilterImage(string sn, HtmlDocument doc, int type, Entities.ArticleInfo article = null)
        {
            IEnumerable<HtmlNode> list = doc.DocumentNode.SelectNodes("//img[@data-src|@src]");
            int headImgCount = 0;//清洗头图个数，最大2个；
            if (list != null && list.Count() > 0)
            {
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个img标签");
                int imgHasQrCount = 0;//统计一篇文章中，移除图片的数量
                int imgOverHeightCount = 0;//统计一篇文章中，高度小于200的数量
                int imgCheckCount = 0;//统计一篇文章中，调用网易易盾接口，不符合规则的图片数量
                for (int j = 0; j < list.Count(); j++)
                {
                    string imgUrl = string.Empty;
                    if (list.ToList()[j].Attributes.Contains("data-src"))
                    {
                        imgUrl = list.ToList()[j].GetAttributeValue("data-src", "");
                    }
                    else if (list.ToList()[j].Attributes.Contains("src"))
                    {
                        imgUrl = list.ToList()[j].GetAttributeValue("src", "");
                    }
                    if (list.ToList()[j].Attributes.Contains("height"))
                    {
                        list.ToList()[j].Attributes.Remove("height");
                    }

                    if (list.ToList()[j].Attributes.Contains("style"))
                    {
                        string imgStyle = list.ToList()[j].Attributes["style"].Value.ToLower();
                        if (imgStyle.Contains("height"))
                        {
                            while (imgStyle.IndexOf("height") >= 0)
                            {
                                string temp = imgStyle.Substring(imgStyle.IndexOf("height"));
                                if (temp.IndexOf(';') > 0)
                                {
                                    temp = temp.Substring(0, temp.IndexOf(';') + 1);
                                }
                                imgStyle = imgStyle.Replace(temp, "");
                            }
                        }
                        list.ToList()[j].Attributes["style"].Value = imgStyle;
                    }
                    if (imgUrl.StartsWith("//"))
                    {
                        imgUrl = "http:" + imgUrl;
                    }

                    Image img = GetImageByURL(imgUrl);
                    if (img != null)
                    {
                        string qrCode = ReadQrCodeText(img);
                        if (!string.IsNullOrWhiteSpace(qrCode))
                        {
                            BLL.Loger.Log4Net.Info($"此处图片[{imgUrl}]中有二维码，需要移除,文章ID为：{sn}");
                            //list.ToList()[j].Remove();
                            RemovePTagByNode(list, j);
                            imgHasQrCount++;
                        }
                        else if (img.Height <= FilterADImgHeight)
                        {
                            BLL.Loger.Log4Net.Info($"此处图片[{imgUrl}]中高度小于{FilterADImgHeight}，实际高度为{img.Height}，需要移除,文章ID为：{sn}");
                            //list.ToList()[j].Remove();
                            RemovePTagByNode(list, j);
                            imgOverHeightCount++;
                        }
                        else
                        {
                            string imageUrl = CleanImg(img, imgUrl);
                            if (!string.IsNullOrWhiteSpace(imageUrl))
                            {
                                //if (!CheckImgContent(sn, CleanImgURLPrefix + imageUrl))
                                //{
                                //    BLL.Loger.Log4Net.Info($"此处图片[{CleanImgURLPrefix + imageUrl}]中不符合网易易盾要求，需要移除,文章ID为：{sn}");
                                //    RemovePTagByNode(list, j);
                                //    imgCheckCount++;
                                //}
                                //else
                                {
                                    imageUrl = CleanImgURLPrefix + imageUrl;
                                    if (list.ToList()[j].Attributes.Contains("src"))
                                    {
                                        list.ToList()[j].Attributes["src"].Value = imageUrl;
                                    }
                                    else
                                    {
                                        list.ToList()[j].Attributes.Add("src", imageUrl);
                                    }
                                }

                                if (j > 0 && article != null &&

                                    img.Height >= 200 && img.Width >= 300 && headImgCount < 3)
                                {
                                    //临时注释
                                    //if (!Util.Instance.CheckImgContent(sn, imgUrl, article.Url))
                                    //{
                                    //    BLL.Loger.Log4Net.Info($"当前线程[{Thread.CurrentThread.ManagedThreadId}],微信文章数据,SN={sn}，新的2个头图不符合规则，已剔除");
                                    //    //list.ToList()[j].Remove();
                                    //    imgCheckCount++;
                                    //    continue;
                                    //}


                                    if (headImgCount == 1)
                                    {
                                        article.HeadImgNew2 = Util.Instance.GetUrlByCutForCustom(imgUrl, imageUrl);
                                    }

                                    else if (headImgCount == 2)
                                    {
                                        article.HeadImgNew3 = Util.Instance.GetUrlByCutForCustom(imgUrl, imageUrl);
                                    }
                                    headImgCount++;
                                }
                            }
                        }
                    }
                }
                BLL.Loger.Log4Net.Info($"文章ID为{sn}中,共有{list.Count()}个img标签，其中移除二维码图片的标签共有{imgHasQrCount}个,高度小于{FilterADImgHeight}的图片有{imgOverHeightCount}个，网易检测不符合规则的图片有{imgCheckCount}个。");
            }
        }

        /// <summary>
        /// 清洗图片（替换url，改为自己的连接）
        /// </summary>
        /// <param name="img">img对象</param>
        /// <param name="imgURL">图片URL地址</param>
        private string CleanImg(Image img, string imgUrl)
        {
            string imageUrl = string.Empty;
            string extName = string.Empty;
            ImageFormat imageFormat = GetImageExtension(img, out extName);
            if (!string.IsNullOrWhiteSpace(extName))
            {
                IFastDFSAdapter _iFastDFSAdapter = new StreamFastDFSAdapter(ImageToByteArray(img, imageFormat, imgUrl), extName.TrimStart('.'));
                imageUrl = _iFastDFSAdapter.Upload();
            }
            return imageUrl;
        }

        public string CleanToBmpImg(Image image, string SuffixName)
        {
            string imageUrl = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {

                if (SuffixName == "JPEG")
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (SuffixName == "JPG")
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (SuffixName == "PNG")
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (SuffixName == "BMP")
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (SuffixName == "GIF")
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (SuffixName == "ICON")
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                else
                {
                    SuffixName = "BMP";
                    image.Save(ms, ImageFormat.Bmp);
                }
                image.Dispose();

                IFastDFSAdapter _iFastDFSAdapter = new StreamFastDFSAdapter(ms.ToArray(), SuffixName);
                imageUrl = _iFastDFSAdapter.Upload();
                return imageUrl;
            }
        }

        /// <summary>
        /// 根据URL，清洗图片，改成本地化url
        /// </summary>
        /// <param name="url">图片URL地址</param>
        /// <returns>返回本地化url</returns>
        public string CleanImg(string url)
        {
            string imageUrl = string.Empty;
            if (!string.IsNullOrWhiteSpace(url))
            {
                BLL.Loger.Log4Net.Info("本地化图片，URL[" + url + "]---------开始");
                Image image = GetImageByURL(url);
                if (image != null)
                {
                    imageUrl = CleanImg(image, url);
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        imageUrl = CleanImgURLPrefix + imageUrl;
                    }
                }
                BLL.Loger.Log4Net.Info("本地化图片，URL[" + url + "]---------结束，转换后为[" + imageUrl + "]");
            }
            return imageUrl;
        }

        public byte[] ImageToByteArray(Image image, ImageFormat imageFormat, string imgUrl)
        {
            if (imageFormat == ImageFormat.Gif)
            {
                WebClient Client = new WebClient();
                return Client.DownloadData(imgUrl);
            }
            else
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, imageFormat);
                return ms.ToArray();
            }
        }

        public ImageFormat GetImageExtension(Image image, out string extName)
        {
            extName = string.Empty;
            if (ImageFormats != null && ImageFormats.Count > 0)
            {
                foreach (var pair in ImageFormats)
                {
                    if (pair.Value.Guid == image.RawFormat.Guid)
                    {
                        extName = pair.Key.ToString().Trim();
                        return pair.Value;
                    }
                }
            }
            return null;
        }

        //public string GetImageExtensionName(Image p_Image)
        //{
        //    Type Type = typeof(ImageFormat);
        //    System.Reflection.PropertyInfo[] _ImageFormatList = Type.GetProperties(BindingFlags.Static | BindingFlags.Public);
        //    for (int i = 0; i != _ImageFormatList.Length; i++)
        //    {
        //        ImageFormat _FormatClass = (ImageFormat)_ImageFormatList[i].GetValue(null, null);
        //        if (_FormatClass.Guid.Equals(p_Image.RawFormat.Guid))
        //        {
        //            return _ImageFormatList[i].Name;
        //        }
        //    }
        //    return string.Empty;
        //}

        private static Dictionary<String, ImageFormat> GetImageFormats()
        {
            var dic = new Dictionary<String, ImageFormat>();
            var properties = typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in properties)
            {
                var format = property.GetValue(null, null) as ImageFormat;
                if (format == null) continue;
                dic.Add(("." + (property.Name.ToLower() == "jpeg" ? "jpg" : property.Name.ToLower())).ToLower(), format);
            }
            return dic;
        }

        ///// <summary>
        ///// 获取图片文件的高度
        ///// </summary>
        ///// <param name="stream">文件流</param>
        ///// <returns>返回图片文件的高度，若查不到或异常，返回NULL</returns>
        //private int? GetImgHeight(Image stream)
        //{
        //    try
        //    {
        //        Image img = Image.FromStream(stream);
        //        int height = img.Height;
        //        //stream.Dispose();
        //        return height;
        //    }
        //    catch (Exception ex)
        //    {
        //        BLL.Loger.Log4Net.Error("GetImgHeight-Error", ex);
        //        return null;
        //    }
        //}

        /// <summary>
        /// 读取二维码内容文本 
        /// </summary>
        /// <param name="stream">Image对象</param>
        /// <returns>二维码内容文本，若找不到返回字符串空</returns>
        public string ReadQrCodeText(Image img)
        {
            try
            {
                BarcodeReader reader = new BarcodeReader();
                reader.Options.CharacterSet = "UTF-8";
                Bitmap map = new Bitmap(img);
                ZXing.Result result = reader.Decode(map);
                //stream.Dispose();
                //map.Dispose();
                return result == null ? "" : result.Text;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"读取二维码内容文本出错", ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// 根据图片URL，获取的Image对象
        /// </summary>
        /// <param name="imgUrl">图片URL</param>
        /// <returns>返回Image对象</returns>
        public Image GetImageByURL(string imgUrl)
        {
            try
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imgUrl);
                System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
                Image img = Image.FromStream(res.GetResponseStream());
                res.Dispose();
                return img;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("GetImageByURL-Error", ex);
                return null;
            }
        }

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="initImage">原图Image对象</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        /// <returns>返回裁剪后的Image对象</returns>
        private Image CutForCustom(Image initImage, int maxWidth, int maxHeight)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            //System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);
            System.Drawing.Image templateImage;

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                //initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                return initImage;
            }
            else
            {
                //模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;
                //原图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;

                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    //按模版大小生成最终图片
                    templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);

                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    //EncoderParameters ep = new EncoderParameters(1);
                    //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                    //保存缩略图
                    //templateImage.Save(fileSaveUrl, ici, ep);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateG.Dispose();
                    //templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();

                }
            }

            //释放资源
            initImage.Dispose();
            return templateImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgUrl">原始图片URL</param>
        /// <param name="oldimgUrl">原始图片本地化后的URL</param>
        /// <param name="maxWidth">裁剪宽度（像素）</param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public string GetUrlByCutForCustom(string imgUrl, string oldimgUrl, int maxWidth = 300, int maxHeight = 200)
        {
            int cutImgWidth = 0;
            int cutImgHeight = 0;
            string msg = string.Empty;
            string cutImageUrl = string.Empty;
            try
            {
                Uri imageUrl = new Uri(imgUrl);
                if (imageUrl != null)
                {
                    string url = imageUrl.AbsoluteUri;
                    if (imageUrl.Host == "imgcdn.chitunion.com")
                    {
                        url = url.Replace("imgcdn.chitunion.com", "192.168.105.9");
                    }

                    Image image = BLL.Util.GetImageByURL(url);
                    if (image != null)
                    {
                        string extName = string.Empty;
                        ImageFormat imagef = GetImageExtension(image, out extName);

                        if (image.Width <= maxWidth && image.Height <= maxHeight)
                        {
                            cutImgWidth = image.Width;
                            cutImgHeight = image.Height;
                            //if (!(imageUrl.Host.Contains("qcdqcdn") ||
                            //    imageUrl.Host.Contains("chitunion")))
                            //{
                            //    cutImageUrl = CleanImg(imgUrl);
                            //    if (!string.IsNullOrWhiteSpace(cutImageUrl))
                            //    {
                            //        cutImageUrl = CleanImgURLPrefix + cutImageUrl;
                            //    }
                            //}
                            //else
                            //{
                            //    cutImageUrl = imgUrl;
                            //}
                            cutImageUrl = oldimgUrl;
                        }
                        else
                        {
                            Image newImg = CutForCustom(image, maxWidth, maxHeight);
                            if (newImg != null)
                            {
                                cutImgWidth = newImg.Width;
                                cutImgHeight = newImg.Height;
                                cutImageUrl = CleanToBmpImg(newImg, extName.TrimStart('.').ToUpper());
                                if (!string.IsNullOrWhiteSpace(cutImageUrl))
                                {
                                    cutImageUrl = CleanImgURLPrefix + cutImageUrl;
                                }
                            }
                            else
                            {
                                msg = "图片裁剪失败";
                            }
                            newImg.Dispose();
                        }
                    }
                    else
                    {
                        msg = "OriginHeadImageUrl参数转换Image对象失败";
                    }
                    image.Dispose();
                }
                else
                {
                    msg = "OriginHeadImageUrl参数不是合法的URL";
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("CutHeadImage出错", ex);
                return string.Empty;
            }
            return cutImageUrl;
        }
    }
}
