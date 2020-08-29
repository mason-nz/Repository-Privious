using HtmlAgilityPack;
using log4net;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.Open.ComponentAPIs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.WxEditor;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.BLL.WxEditor
{

    /// <summary>
    /// 2017-06-22 zlb
    /// 图文组Bll
    /// </summary>
    public class WxArticleGroup
    {
        public static readonly WxArticleGroup Instance = new WxArticleGroup();
        ILog logger = LogManager.GetLogger(typeof(WxArticleGroup));

        /// <summary>
        /// 2017-06-23 zlb
        /// 查询用户对应的图文组同步记录
        /// </summary>
        /// <param name="TitleOrAbstract">标题或摘要</param>
        /// <param name="Pagesize">页数大小</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="Status">0全部；1失败的</param>
        /// <returns>同步微信历史记录集合</returns>
        public Dictionary<string, object> SelectArticleGroupListByIDList(string TitleOrAbstract, int Pagesize, int PageIndex, int Status, out string Message)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("TotalCount", 0);
            Message = "";
            if (TitleOrAbstract == null)
            {
                TitleOrAbstract = "";
            }
            if (Status != 0 && Status != 1)
            {
                Message = "查询类型错误";
            }
            List<HistoryArticleGroup> hagList = new List<HistoryArticleGroup>();
            int userID = Common.UserInfo.GetLoginUserID();
            #region 获取前Pagesize条图文组批次ID
            DataTable dt = XYAuto.ITSC.Chitunion2017.Dal.WxEditor.WxArticleGroup.Instance.SelectGroupIDListByUserid(userID, TitleOrAbstract.Trim(), Pagesize, PageIndex, Status);
            #endregion
            if (dt != null && dt.Rows.Count > 0)
            {
                dic["TotalCount"] = dt.Columns["TotalCount"].Expression;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HistoryArticleGroup hag = new HistoryArticleGroup();
                    hag.WxGRID = Convert.ToInt32(dt.Rows[i]["WxGRID"]);
                    hagList.Add(hag);
                    sb.Append(dt.Rows[i]["WxGRID"] + ",");
                }
                string WxgrIDList = sb.ToString();
                WxgrIDList = WxgrIDList.Substring(0, WxgrIDList.Length - 1);

                DataSet ds = XYAuto.ITSC.Chitunion2017.Dal.WxEditor.WxArticleGroup.Instance.SelectArticleGroupListByIDList(WxgrIDList);
                if (ds != null)
                {
                    for (int i = 0; i < hagList.Count; i++)
                    {
                        #region 获取图文文章信息
                        DataRow[] drArrayArticle = ds.Tables[0].Select("WxGRID=" + hagList[i].WxGRID);
                        if (drArrayArticle != null && drArrayArticle.Count() > 0)
                        {
                            for (int j = 0; j < drArrayArticle.Count(); j++)
                            {
                                hagList[i].TitleList.Add(drArrayArticle[j]["Title"].ToString());
                            }
                            DataRow dr = drArrayArticle.OrderBy(x => x["Orderby"]).ToArray().First();
                            hagList[i].CoverPicUrl = dr["CoverPicUrl"].ToString();
                        }
                        #endregion
                        #region 获取图文同步微信号及状态信息
                        DataRow[] drArrayStatus = ds.Tables[1].Select("WxGRID=" + hagList[i].WxGRID);
                        if (drArrayStatus != null && drArrayStatus.Count() > 0)
                        {
                            for (int j = 0; j < drArrayStatus.Count(); j++)
                            {
                                ArticleSatusInfo asInfo = new ArticleSatusInfo();
                                asInfo.WxName = drArrayStatus[j]["NickName"].ToString();
                                asInfo.HeadImg= drArrayStatus[j]["HeadImg"].ToString(); 
                                asInfo.WxStatus = (ArticleSyncStatus)Convert.ToInt32(drArrayStatus[j]["Status"]);
                                hagList[i].AstatusList.Add(asInfo);
                            }
                            DataRow dr = drArrayStatus.OrderByDescending(x => x["CompleteTime"]).FirstOrDefault();
                            hagList[i].ComplateTime = dr["CompleteTime"].ToString() == "" ? "" : Convert.ToDateTime(dr["CompleteTime"]).ToString("yyyy-MM-dd HH:mm");
                        }
                        #endregion
                    }
                }
            }
            dic.Add("ArticleGroupInfo", hagList);
            return dic;
        }

        public Dictionary<string, object> SelectWxStatusInfoByGroupID(int GroupID, int InputType, out string Message)
        {
            Message = "";
            if (InputType != 1 && InputType != 0)
            {
                Message = "查询类型错误";
                return null;
            }
            int userID = Common.UserInfo.GetLoginUserID();
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            DataTable dtWxInfo = new DataTable();

            if (InputType == 0 && Dal.WxEditor.WxArticleGroup.Instance.SelectInSyncGroupCount(GroupID, userID) <= 0)
            {
                dicAll.Add("ReturnType", 0);
                dtWxInfo = Dal.WxEditor.WxArticleGroup.Instance.SelectWxOAuthInfo(userID);
            }
            else
            {
                dicAll.Add("ReturnType", 1);
                dtWxInfo = Dal.WxEditor.WxArticleGroup.Instance.SelectWxStatusInfoByGroupID(GroupID, userID);
            }

            List<Dictionary<string, object>> listWxInfo = new List<Dictionary<string, object>>();
            dicAll.Add("IsComplete", 1);
            if (dtWxInfo != null && dtWxInfo.Rows.Count > 0)
            {
                for (int i = 0; i < dtWxInfo.Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    DataRow dr = dtWxInfo.Rows[i];
                    dic.Add("WxID", Convert.ToInt32(dr["WxID"]));
                    dic.Add("WxNumber", dr["WxNumber"].ToString());
                    dic.Add("WxName", dr["WxName"].ToString());
                    dic.Add("HeadImg", dr["HeadImg"].ToString());
                    dic.Add("Status", Convert.ToInt32(dr["Status"]));
                    if (Convert.ToInt32(dr["Status"]) == (int)ArticleSyncStatus.同步中 || Convert.ToInt32(dr["Status"]) == (int)ArticleSyncStatus.待同步)
                    {
                        dicAll["IsComplete"] = 0;
                    }
                    listWxInfo.Add(dic);
                }
            }
            dicAll.Add("WxSyncStatus", listWxInfo);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 查询图文组信息
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectGroupInfoByGroupID(int GroupID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            int userID = Common.UserInfo.GetLoginUserID();
            DataTable dt = Dal.WxEditor.WxArticleGroup.Instance.SelectGroupInfoByGroupID(GroupID, userID);
            List<Dictionary<string, object>> ArticleGroups = new List<Dictionary<string, object>>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ArticleID", dr["ArticleID"]);
                    dic.Add("Title", dr["Title"].ToString());
                    dic.Add("CoverPicUrl", dr["CoverPicUrl"].ToString());
                    dic.Add("Orderby", dr["Orderby"]);
                    ArticleGroups.Add(dic);
                }
            }
            dicAll.Add("ArticleGroups", ArticleGroups);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 根据图文ID和用户ID查询图文信息
        /// </summary>
        /// <param name="ArticleID">图文ID</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectArticleInfoByArticleID(int ArticleID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("Title", "");
            dicAll.Add("Author", "");
            dicAll.Add("Abstract", "");
            dicAll.Add("Content", "");
            dicAll.Add("OriginalUrl", "");
            int userID = Common.UserInfo.GetLoginUserID();
            DataTable dt = Dal.WxEditor.WxArticleGroup.Instance.SelectArticleInfoByArticleID(ArticleID, userID);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                dicAll["Title"] = dr["Title"].ToString();
                dicAll["Author"] = dr["Author"].ToString();
                dicAll["Abstract"] = dr["Abstract"].ToString();
                dicAll["Content"] = dr["Content"].ToString();
                dicAll["OriginalUrl"] = dr["OriginalUrl"].ToString();
            }
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-07-01
        /// 根据URL获取单篇文章
        /// </summary>
        /// <param name="ImportUrl">URL</param>
        /// <param name="SelectType">查看类型： 1微信 2通用</param>
        /// <param name="iga">文章信息</param>
        /// <returns></returns>
        public string GetSingleGroups(string ImportUrl, int SelectType, out ImportGroupArticle iga)
        {
            try
            {
                iga = null;
                if (string.IsNullOrWhiteSpace(ImportUrl))
                {
                    return "请输入文章地址";
                }
                if (SelectType == 1)
                {
                    if (!ImportUrl.Substring(0, 26).Contains("mp.weixin.qq.com"))
                    {
                        return "请输入正确的文章地址";
                    }
                }
                //string domainPath = ConfigurationManager.AppSettings["CatchDomain"];
                string domainPath = "http://192.168.121.118:10088";
                string url = string.Format("{0}/api/content/getcontent?url={1}", domainPath, HttpUtility.UrlEncode(ImportUrl));
                var request = HttpWebRequest.Create(url) as HttpWebRequest;
                var response = request.GetResponse() as HttpWebResponse;
                string str = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    str = sr.ReadToEnd();
                }
                response.Close();
                request.Abort();
                iga = JsonConvert.DeserializeObject<ImportGroupArticle>(str);
                if (iga == null || iga.code != "200" || iga.data == null)
                {
                    return "文章不存在请确认";
                }
                return "";
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("抓取错误", ex);
                iga = null;
                return "获取文章失败";
            }
        }
        /// <summary>
        /// zlb 2017-07-01
        /// 微信URL导入文章
        /// </summary>
        /// <param name="ImportUrl">URL</param>
        /// <param name="groupID">返回的图文组ID</param>
        /// <returns></returns>
        public string ImportWxArticle(string ImportUrl, ref int groupID)
        {
            ImportGroupArticle iga = null;
            string message = GetSingleGroups(ImportUrl, 1, out iga);
            if (message != "")
            {
                return message;
            }
            int userID = Common.UserInfo.GetLoginUserID();
            ModifyArticleReqDTO req = new ModifyArticleReqDTO();
            req.ImportType = ArticleImportTypeEnum.Url;
            req.fromWxName = iga.data.fromWxName == null ? "未知" : iga.data.fromWxName;
            req.fromWxNumber = iga.data.fromWxNumber;
            req.UpdateUserID = userID;
            req.fromUrl = ImportUrl.Trim();

            ModifyArticleItem mai = new ModifyArticleItem();
            mai.Abstract = iga.data.Abstract;
            mai.Author = iga.data.Author;
            mai.Title = iga.data.Title;
            mai.OriginalUrl = ImportUrl.Trim();
            mai.Content = iga.data.Content.Replace("data-src=", "src=");
            mai.CoverPicUrl = "/uploadfiles/Article/CoverDefaultPic.png";
            mai.Orderby = 1;
            req.ArticleList = new List<ModifyArticleItem>();
            req.ArticleList.Add(mai);
            bool result;
            DataTable dt = Dal.WxEditor.WxArticleGroup.Instance.SelectArticleIdByUrl(ImportUrl.Trim(), userID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                groupID = Convert.ToInt32(dr["GroupID"]);
                result = Dal.WxEditor.WxArticleGroup.Instance.UpdateGroupArticleByID(Convert.ToInt32(dr["ArticleID"]), groupID, req);

            }
            else
            {
                result = Dal.WxEditor.WxArticleGroup.Instance.ModifyArticle(req, ref groupID);
            }
            if (result)
            {
                return "";
            }
            else
            {
                return "导入失败，稍后再试";
            }
        }
        /// <summary>
        /// zlb 2017-07-01
        /// 根据URL查询文章内容
        /// </summary>
        /// <param name="ImportUrl">URL</param>
        /// <param name="Message">返回错误消息</param>
        /// <returns></returns>
        public string SelectArticleByUrl(string ImportUrl, out string Message)
        {
            ImportGroupArticle iga = null;
            Message = GetSingleGroups(ImportUrl, 2, out iga);
            if (Message != "")
            {
                return "";
            }
            return iga.data.Content;
        }
        /// <summary>
        /// zlb 2017-07-01
        /// 查询图文组文章信息列表
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <returns></returns>
        public Dictionary<string, object> SelectGroupArticlesByGroupID(int GroupID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            List<Dictionary<string, object>> listArticle = new List<Dictionary<string, object>>();
            int userID = Common.UserInfo.GetLoginUserID();
            DataTable dt = Dal.WxEditor.WxArticleGroup.Instance.SelectGroupArticlesByGroupID(GroupID, userID);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, object> dicArticle = new Dictionary<string, object>();
                    DataRow dr = dt.Rows[i];
                    dicArticle.Add("ArticleID", dr["ArticleID"]);
                    dicArticle.Add("Title", dr["Title"].ToString());
                    dicArticle.Add("CoverPicUrl", dr["CoverPicUrl"].ToString());
                    dicArticle.Add("Abstract", dr["Abstract"].ToString());
                    dicArticle.Add("Author", dr["Author"].ToString());
                    dicArticle.Add("Content", dr["Content"].ToString());
                    dicArticle.Add("OriginalUrl", dr["OriginalUrl"].ToString());
                    dicArticle.Add("PCViewUrl", dr["PCViewUrl"].ToString());
                    dicArticle.Add("MobileViewUrl", dr["MobileViewUrl"].ToString());
                    dicArticle.Add("Orderby", dr["Orderby"]);
                    listArticle.Add(dicArticle);
                }
            }
            else
            {
                Dictionary<string, object> dicArticle = new Dictionary<string, object>();
                dicArticle.Add("ArticleID", "");
                dicArticle.Add("Title", "");
                dicArticle.Add("CoverPicUrl", "/uploadfiles/Article/CoverDefaultPic.png");
                dicArticle.Add("Abstract", "");
                dicArticle.Add("Author", "");
                dicArticle.Add("Content", "");
                dicArticle.Add("OriginalUrl", "");
                dicArticle.Add("PCViewUrl", "");
                dicArticle.Add("MobileViewUrl", "");
                dicArticle.Add("Orderby", 1);
                listArticle.Add(dicArticle);

            }
            dicAll.Add("ArticleList", listArticle);
            return dicAll;
        }

        #region Ls

        /// <summary>
        /// 获取所属微信
        /// </summary>
        /// <returns></returns>
        public List<OwnWeixinItemDTO> GetOwnWeixinList()
        {
            var ur = Common.UserInfo.GetUserRole();
            return Dal.WxEditor.WxArticleGroup.Instance.GetOwnWeixinList(ur.UserID);
        }

        /// <summary>
        /// 获取图文组列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public GetWeixinArticleGroupListResDTO GetWeixinArticleGroupList(GetWeixinArticleGroupListReqDTO req)
        {
            var ur = Common.UserInfo.GetUserRole();
            var res = Dal.WxEditor.WxArticleGroup.Instance.GetWeixinArticleGroupList(ur.UserID, req.Key, req.WxName, req.WxNumber, req.BeginDate, req.EndDate, req.PageIndex, req.PageSize);
            foreach (var group in res.List)
            {
                if (group.LastUpdateTime.Year.Equals(DateTime.Now.Year))
                {
                    group.UpdateDate = group.LastUpdateTime.ToString("M月d日 HH:mm");
                }
                else
                {
                    group.UpdateDate = group.LastUpdateTime.ToString("yyyy年M月d日 HH:mm");
                }

                #region 图文
                group.ArticleList = new List<ArticleItem>();
                if (string.IsNullOrEmpty(group.CombinStr))
                    continue;
                var articleArr = group.CombinStr.Split(',');
                foreach (var articleStr in articleArr)
                {
                    if (string.IsNullOrEmpty(articleStr))
                        continue;
                    var arr = articleStr.Split('|');
                    ArticleItem article = new ArticleItem()
                    {
                        ArticleID = int.Parse(arr[0]),
                        Orderby = int.Parse(arr[1]),
                        Title = arr[2],
                        CoverPicUrl = arr[3],
                        Abstract = arr[4]
                    };
                    group.ArticleList.Add(article);
                }
                group.CombinStr = string.Empty;
                #endregion
            }
            return res;
        }

        /// <summary>
        /// 获取图文列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public GetWeixinArticleListResDTO GetWeixinArticleList(GetWeixinArticleGroupListReqDTO req)
        {
            var ur = Common.UserInfo.GetUserRole();
            var res = Dal.WxEditor.WxArticleGroup.Instance.GetWeixinArticleList(ur.UserID, req.Key, req.WxName, req.WxNumber, req.BeginDate, req.EndDate, req.PageIndex, req.PageSize);
            return res;
        }

        /// <summary>
        /// 获取好文推荐列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public GetWeixinArticleListResDTO GetWeixinGoodArticleTJList(GetWeixinArticleGroupListReqDTO req)
        {
            var ur = Common.UserInfo.GetUserRole();
            var res = Dal.WxEditor.WxArticleGroup.Instance.GetWeixinGoodArticleTJList(req.Key, req.PageIndex, req.PageSize);
            return res;
        }

        /// <summary>
        /// 移动图文
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool MoveArticle(MoveArticleReqDTO req)
        {
            if (req.OptType < 1 || req.OptType > 2)
                return false;
            return Dal.WxEditor.WxArticleGroup.Instance.MoveArticle(req.ArticleID, req.OptType);
        }

        /// <summary>
        /// 新增编辑图文
        /// </summary>
        /// <param name="req"></param>
        /// <param name="msg"></param>
        /// <param name="groupID"></param>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public bool ModifyArticle(ModifyArticleReqDTO req, ref string msg, ref int groupID, ref int articleID)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsMedia)
            {
                msg = "角色错误!";
                return false;
            }
            string rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.用户, ur);
            req.UpdateTime = DateTime.Now;
            req.UpdateUserID = ur.UserID;
            req.ImportType = ArticleImportTypeEnum.手工添加;
            foreach (var article in req.ArticleList)
            {
                if (!string.IsNullOrEmpty(article.Title))
                    article.Title = article.Title.Replace(",", "，");
                if (!string.IsNullOrEmpty(article.Abstract))
                    article.Abstract = article.Abstract.Replace(",", "，");
                article.Content = this.ConvertToChituUrl(article.Content);
            }
            return Dal.WxEditor.WxArticleGroup.Instance.ModifyArticle(req, ref groupID);
        }

        /// <summary>
        /// 删除图文组
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteArticleGroup(int groupID, ref string msg)
        {
            return Dal.WxEditor.WxArticleGroup.Instance.DeleteArticleGroup(groupID, ref msg);
        }

        /// <summary>
        /// 删除图文
        /// </summary>
        /// <param name="articleIDs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteArticle(List<int> articleIDs, ref string msg)
        {
            bool res = true;
            foreach (var articleID in articleIDs)
            {
                res = Dal.WxEditor.WxArticleGroup.Instance.DeleteArticle(articleID, ref msg);
                if (msg.Length > 0)
                {
                    res = false;
                    break;
                }
            }
            return res;
        }

        /// <summary>
        /// 获取图文信息
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public ArticleInfo GetArticleInfoByID(int articleID)
        {
            return Dal.WxEditor.WxArticleGroup.Instance.GetArticleInfoByID(articleID);
        }

        /// <summary>
        /// 生成图文静态页
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="optType"></param>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateArticlePage(int articleID, int optType, ref string url, ref string msg)
        {
            string templatePath = WebConfigurationManager.AppSettings["TemplatePath"];
            string host = WebConfigurationManager.AppSettings["ExitAddress"];
            string fileNameAttach = string.Empty;
            if (optType.Equals(1))
            {
                templatePath = templatePath += "\\pc\\templete.html";
            }
            else if (optType.Equals(2))
            {
                templatePath = templatePath += "\\mobile\\templete.html";
                fileNameAttach = "_m";
            }
            else
            {
                msg = "参数错误";
                return false;
            }
            logger.Info(HttpContext.Current.Request.ApplicationPath.ToString());
            logger.Info(templatePath);
            var one = this.GetArticleInfoByID(articleID);
            if (one == null)
            {
                msg = "图文不存在";
                return false;
            }
            if (!File.Exists(templatePath))
            {
                msg = "模板不存在";
                return false;
            }

            string html = FileHelper.ReadFile(templatePath);
            html = html.Replace("@@@标题@@@", one.Title);
            html = html.Replace("@@@日期@@@", one.LastUpdateTime.ToString("yyyy-MM-dd"));
            html = html.Replace("@@@作者@@@", one.Author);
            html = html.Replace("@@@内容@@@", one.Content);
            if (!string.IsNullOrWhiteSpace(one.OriginalUrl))
                html = html.Replace("@@@查看原文@@@", "<a href='" + one.OriginalUrl + "'>查看原文</a>");
            else
                html = html.Replace("@@@查看原文@@@", string.Empty);
            string pagePath = string.Format("{0}\\UploadFiles\\Article\\{1}\\{2}\\{3}\\{4}",
                WebConfigurationManager.AppSettings["UploadFilePath"],
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour);
            if (!Directory.Exists(pagePath))
                Directory.CreateDirectory(pagePath);
            string fileName = string.Format("{0}{1}.html", Guid.NewGuid().ToString(), fileNameAttach);
            string backFilePath = pagePath + "\\" + fileName;
            FileHelper.UpdateFile(html, backFilePath, Encoding.UTF8);
            string frontFilePath = string.Format("/uploadfiles/Article/{0}/{1}", DateTime.Now.ToString("yyyy/M/d/H"), fileName);
            if (optType.Equals(2))//手机返回二维码获取地址
                frontFilePath = host + frontFilePath;
            bool res = Dal.WxEditor.WxArticleGroup.Instance.UpdateArticleViewUrl(one.ArticleID, optType, frontFilePath);
            if (res)
            {
                url = frontFilePath;
                if (optType.Equals(2))//手机返回二维码获取地址
                    url = "/api/Article/GetQRCode?ArticleID=" + one.ArticleID;
            }
            return res;
        }

        /// <summary>
        /// 上传图文组
        /// </summary>
        /// <returns></returns>
        public bool UploadWeixinArticleGroup(UploadWeixinArticleGroupReqDTO req, ref string msg)
        {
            bool res = false;
            int ranking = 0;
            if (req.UploadWxIDs == null || req.UploadWxIDs.Count.Equals(0))
            {
                msg = "没有选择要上传的微信号";
                return false;
            }
            var articleList = Dal.WxEditor.WxArticleGroup.Instance.GetArticleListByGroupID(req.GroupID);
            if (articleList == null || articleList.Count.Equals(0))
            {
                msg = "没有任何图文信息";
                return false;
            }
            foreach (var article in articleList)
            {
                if (string.IsNullOrWhiteSpace(article.Title))
                {
                    msg = "图文标题不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(article.CoverPicUrl))
                {
                    msg = "图文封面不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(article.Content))
                {
                    msg = "图文内容不能为空";
                    return false;
                }
                //if (article.Content.Length >= 20000)
                //{
                   // msg = "图文内容不能超过2W字";
                    //return false;
                //}
            }
            var ur = Common.UserInfo.GetUserRole();
            res = Dal.WxEditor.WxArticleGroup.Instance.CreateUploadRecord(req.GroupID, req.UploadWxIDs, ur.UserID, ref msg, ref ranking);
            if (res)
            {
                Task.Factory.StartNew(() => SyncToWeixin(req.GroupID, req.UploadWxIDs, ranking));
            }
            return res;
        }

        /// <summary>
        ///同步素材到微信
        /// </summary>
        /// <param name="groupID">组ID</param>
        /// <param name="wxList">微信号列表</param>
        /// <param name="ranking">批次</param>
        public void SyncToWeixin(int groupID, List<int> wxIDList, int ranking)
        {

            string physicalDirectory = WebConfigurationManager.AppSettings["UploadFilePath"];
            foreach (int wxID in wxIDList)
            {
                bool success = true;
                string groupMediaID = string.Empty;
                try
                {
                    Dal.WxEditor.WxArticleGroup.Instance.UploadSyncHisStatus(groupID, wxID, ranking, ArticleSyncStatus.同步中);
                    var articleList = Dal.WxEditor.WxArticleGroup.Instance.GetArticleListByGroupID(groupID);
                    #region 遍历微信号 一个一个发
                    //取token
                    string wxToken = GetWxAccessToken(wxID);
                    if (string.IsNullOrEmpty(wxToken))
                    {
                        throw new Exception("没有取到微信Token");
                    }
                    else
                    {
                        #region 封面图片上传 UploadForeverMedia、图文内容图片上传 UploadImg
                        foreach (var article in articleList)
                        {
                            string content = article.Content;
                            string coverPicUrl = physicalDirectory + article.CoverPicUrl;
                            article.ConverPicMediaID = MediaApi.UploadForeverMedia(wxToken, coverPicUrl).media_id;
                            success = this.ConvertToWeixinUrl(wxToken, ref content);
                            if (success)
                                article.Content = content;
                            else
                                throw new Exception("转换微信地址失败");
                        }
                        #endregion

                        #region 上传图文组 UploadNews
                        List<NewsModel> newsList = new List<NewsModel>();
                        foreach (var article in articleList)
                        {
                            NewsModel news = new NewsModel()
                            {
                                title = article.Title,
                                //图文消息的封面图片素材id（必须是永久mediaID）
                                thumb_media_id = article.ConverPicMediaID,
                                author = article.Author,
                                //仅有单图文消息才有摘要，多图文此处为空。如果本字段为没有填写，则默认抓取正文前64个字
                                digest = articleList.Count.Equals(1) ? article.Abstract : string.Empty,
                                show_cover_pic = "0",
                                content = article.Content,
                                content_source_url = article.OriginalUrl
                            };
                            newsList.Add(news);
                        }
                        groupMediaID = MediaApi.UploadNews(wxToken, 10000, newsList.ToArray()).media_id;
                        Dal.WxEditor.WxArticleGroup.Instance.UploadSyncHisStatus(groupID, wxID, ranking, ArticleSyncStatus.同步成功);
                        Dal.WxEditor.WxArticleGroup.Instance.UpdateWeixinMaterialID(groupID, groupMediaID);
                        #endregion
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Dal.WxEditor.WxArticleGroup.Instance.UploadSyncHisStatus(groupID, wxID, ranking, ArticleSyncStatus.同步失败);
                    success = false;
                }
            }
        }

        /// <summary>
        /// 获取授权公众号Token
        /// </summary>
        /// <returns></returns>
        public string GetWxAccessToken(int wxID)
        {
            try
            {
                var wx = Dal.WeixinOAuth.Instance.GetWeixinInfo(wxID);
                if (!string.IsNullOrEmpty(wx.AccessToken) && wx.GetTokenTime >= DateTime.Now.AddHours(0).AddMinutes(-50))
                    return wx.AccessToken;
                string cpAccessToken = BLL.WeixinOAuth.GetComponentAcessToken();
                var res = ComponentApi.ApiAuthorizerToken(cpAccessToken, BLL.WeixinOAuth.component_AppId, wx.AppID, wx.RefreshAccessToken);
                wx.GetTokenTime = DateTime.Now;
                wx.AccessToken = res.authorizer_access_token;
                wx.RefreshAccessToken = res.authorizer_refresh_token;
                BLL.WeixinOAuth.Instance.UpdateTokenInfo(wx.AppID, res.authorizer_access_token, res.authorizer_refresh_token, DateTime.Now);
                return res.authorizer_access_token;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 外链换成内链
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string ConvertToChituUrl(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                List<string> urlList = this.FindImgUrlDict(content);
                #region 下载替换链接
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:54.0) Gecko/20100101 Firefox/54.0");
                client.Headers.Add("Accept", "*/*");

                string physicalDirectory = string.Format("{0}\\UploadFiles\\Article\\{1}\\{2}\\{3}\\{4}",
                    WebConfigurationManager.AppSettings["UploadFilePath"],
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    DateTime.Now.Hour);
                if (!Directory.Exists(physicalDirectory))
                    Directory.CreateDirectory(physicalDirectory);
                foreach (string oldUrl in urlList)
                {
                    string newUrl = oldUrl;
                    if (oldUrl.ToLower().StartsWith("/uploadfiles"))//本地
                        continue;
                    try
                    {
                        string suffix = oldUrl.Split('.').Last().ToLower().Substring(0, 3);
                        if (suffix != "png" && suffix != "jpg")
                        {
                            suffix = "jpg";
                            //throw new Exception("图片类型只能为jpg或png");
                        }
                        byte[] bytes = client.DownloadData(oldUrl);
                        if (bytes.Length >= 1024 * 1024)
                        {
                            throw new Exception("图片大小不能超过1MB");
                        }
                        string fileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), suffix);
                        string physicalFilePath = physicalDirectory + "\\" + fileName;
                        MemoryStream ms = new MemoryStream(bytes);
                        var image = System.Drawing.Image.FromStream(ms);
                        image.Save(physicalFilePath);
                        newUrl = string.Format("/uploadfiles/Article/{0}/{1}", DateTime.Now.ToString("yyyy/M/d/H"), fileName);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                    finally
                    {
                        content = content.Replace(oldUrl, newUrl);
                    }
                }
                #endregion
            }
            return content;
        }

        /// <summary>
        /// 内链换成微信内链
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool ConvertToWeixinUrl(string wxToken, ref string content)
        {
            try
            {
                string physicalDirectory = WebConfigurationManager.AppSettings["UploadFilePath"];
                if (!string.IsNullOrEmpty(content))
                {
                    string newUrl = string.Empty;
                    var urlList = this.FindImgUrlDict(content);
                    foreach (string oldUrl in urlList)
                    {
                        newUrl = MediaApi.UploadImg(wxToken, physicalDirectory + oldUrl).url;
                        content = content.Replace(oldUrl, newUrl);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 找出内容里面的图片链接(jpg png) 以字典形式返回 Value存新地址
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public List<string> FindImgUrlDict(string content)
        {
            var urlList = new List<string>();
            string regStr = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<src>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
            Regex reg = new Regex(regStr, RegexOptions.IgnoreCase);
            var mc = reg.Matches(content);
            HtmlDocument doc = null;
            foreach (var m in mc)
            {
                doc = new HtmlDocument();
                doc.LoadHtml(m.ToString());
                urlList.Add(doc.DocumentNode.SelectSingleNode("/img").Attributes["src"].Value);
            }
            return urlList;
        }

        #endregion
    }
}

