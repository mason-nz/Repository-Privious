using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    /// <summary>
    /// zlb 2017-06-22
    /// 微信编辑器
    /// </summary>
    [CrossSite]
    public class WeChatEditorController : ApiController
    {
        #region 图文组信息
        /// <summary>
        /// 2017-06-23 张立彬
        /// 查询同步微信历史记录图文组
        /// </summary>
        /// <param name="TitleOrAbstract">标题或摘要</param>
        /// <param name="Status">0全部；1失败的</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectArticleGroupListByIDList(string TitleOrAbstract = "", int Pagesize = 20, int PageIndex = 1, int Status = 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                string Message = "";
                dic = BLL.WxEditor.WxArticleGroup.Instance.SelectArticleGroupListByIDList(TitleOrAbstract, Pagesize, PageIndex, Status, out Message);
                if (Message != "")
                {
                    return Common.Util.GetJsonDataByResult(null, Message, -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****SelectArticleGroupListByIDList ->TitleOrAbstract:" + TitleOrAbstract + ",查询同步微信历史记录失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success", 0);
        }

        /// <summary>
        /// zlb 2017-06-26
        /// 查询图文信息
        /// </summary>
        /// <param name="ArticleID">图文ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectArticleInfoByArticleID(int ArticleID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.WxEditor.WxArticleGroup.Instance.SelectArticleInfoByArticleID(ArticleID);
                return Common.Util.GetJsonDataByResult(dic, "Success", 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****SelectArticleInfoByArticleID ->ArticleID:" + ArticleID + ",查询图文信息失败:" + ex.Message);
                throw ex;
            }
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("Author", "张大伟");
            //dic.Add("Abstract", "读书的重要性");
            //dic.Add("Content", "1234567，山里的和尚叫小七");
            //dic.Add("OriginalUrl", "http://j.chitunion.com/userInfo/toUserMediaList");
            //return Common.Util.GetJsonDataByResult(dic, "Success", 0);
        }

        /// <summary>
        /// zlb 2017-06-26
        /// 查询图文组图文信息列表
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectGroupInfoByGroupID(int GroupID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.WxEditor.WxArticleGroup.Instance.SelectGroupInfoByGroupID(GroupID);
                return Common.Util.GetJsonDataByResult(dic, "Success", 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****SelectWxStatusInfoByGroupID ->GroupID:" + GroupID + ",查询微信通同步状态信息或授权微信号信息记录失败:" + ex.Message);
                throw ex;
            }
            //List<object> list = new List<object>();
            //for (int i = 1; i <= 2; i++)
            //{
            //    Dictionary<string, object> dic = new Dictionary<string, object>();
            //    dic.Add("ArticleID", i);
            //    dic.Add("Title", "标题" + i);
            //    dic.Add("CoverPicUrl", i + "$" + Guid.NewGuid().ToString() + ".jpg");
            //    dic.Add("Orderby", i);
            //    list.Add(dic);
            //}
            //return Common.Util.GetJsonDataByResult(list, "Success", 0);
        }

        /// <summary>
        /// zlb 2017-06-26
        /// 查询微信通同步状态信息或授权微信号信息
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <param name="InputType">查询类型（0:查询所有微信账号；1：查询同步账号状态）</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectWxStatusInfoByGroupID(int GroupID, int InputType)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                string Message = "";
                dic = BLL.WxEditor.WxArticleGroup.Instance.SelectWxStatusInfoByGroupID(GroupID, InputType, out Message);
                if (Message != "")
                {
                    return Common.Util.GetJsonDataByResult(null, Message, -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****SelectWxStatusInfoByGroupID ->GroupID:" + GroupID + ",查询微信通同步状态信息或授权微信号信息记录失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success", 0);
            //Dictionary<string, object> dicAll = new Dictionary<string, object>();
            //List<object> list = new List<object>();
            //if (InputType == 0)
            //{
            //    dicAll.Add("ReturnType", 0);
            //    for (int i = 0; i < 2; i++)
            //    {
            //        Dictionary<string, object> dic = new Dictionary<string, object>();
            //        dic.Add("WxID", i + 1);
            //        dic.Add("WxNumber", "zicheng" + (i + 1));
            //        dic.Add("WxName", "三只老鼠" + (i + 1));
            //        dic.Add("Status", 0);
            //        list.Add(dic);
            //    }
            //    dicAll.Add("WxSyncStatus", list);
            //    return Common.Util.GetJsonDataByResult(dicAll, "Success", 0);
            //}
            //else
            //{
            //    dicAll.Add("ReturnType", 1);
            //    for (int i = 0; i < 2; i++)
            //    {
            //        Dictionary<string, object> dic = new Dictionary<string, object>();
            //        dic.Add("WxID", i + 1);
            //        dic.Add("WxNumber", "zicheng" + (i + 1));
            //        dic.Add("WxName", "三只老鼠" + (i + 1));
            //        dic.Add("Status", 54002);
            //        list.Add(dic);
            //    }
            //    dicAll.Add("WxSyncStatus", list);
            //    return Common.Util.GetJsonDataByResult(dicAll, "Success", 0);
            //}

        }
        /// <summary>
        /// zlb 2017-07-01
        /// 查询图文组文章信息列表
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]

        public Common.JsonResult SelectGroupArticlesByGroupID(int GroupID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.WxEditor.WxArticleGroup.Instance.SelectGroupArticlesByGroupID(GroupID);
                return Common.Util.GetJsonDataByResult(dic, "Success", 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****SelectGroupArticlesByGroupID ->GroupID:" + GroupID + ",查询图文组文章信息列表录失败:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-07-01
        /// 根据微信URL导入文章
        /// </summary>
        /// <param name="ImportUrl">URL</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult ImportWxArticle(string ImportUrl)
        {
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******ImportWxArticle begin...->ImportUrl:" + ImportUrl + "");
            string messageStr = "";
            int GroupID = 0;
            try
            {
                messageStr = BLL.WxEditor.WxArticleGroup.Instance.ImportWxArticle(ImportUrl, ref GroupID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****ImportWxArticle ->ImportUrl:" + ImportUrl + ",根据微信URL导入文章错误:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(GroupID, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(GroupID, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******ImportWxArticle end->");
            return jr;
        }
        /// <summary>
        ///  zlb 2017-07-01
        /// 根据URL查询文章内容
        /// </summary>
        /// <param name="ImportUrl">文章URL</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectArticleByUrl(string ImportUrl)
        {
            try
            {
                string Message = "";
                string ArticleContent = BLL.WxEditor.WxArticleGroup.Instance.SelectArticleByUrl(ImportUrl, out Message);
                if (Message == "")
                {
                    return Common.Util.GetJsonDataByResult(ArticleContent, "Success", 0);
                }
                else
                {
                    return Common.Util.GetJsonDataByResult("", Message, -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****SelectArticleByUrl ->ImportUrl:" + ImportUrl + ", 根据URL查询文章内容失败:" + ex.Message);
                throw ex;
            }

        }
        #endregion

        #region 操作图片信息
        /// <summary>
        /// zlb 2017-06-26
        /// 查询用户下的微信图片列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="PicName">图片名称</param>
        /// <param name="WxID">微信ID；0:查询本地上传 大于0：微信的 小于0：全部的</param>
        /// <returns></returns>01
        /// 
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SelectPictrues(int PageIndex, int PageSize, string PicName = "", int WxID = -1)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.WxEditor.WxPictureMaterial.Instance.SelectPictrues(PageIndex, PageSize, PicName, WxID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****SelectPictrues ->WxID:" + WxID + "->PicName:" + PicName + ", 查询微信图片列表失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success", 0);
        }

        /// <summary>
        /// zlb 2017-06-26
        /// 批量删除微信图片
        /// </summary>
        /// <param name="PicIDs">图片ID集合</param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult DeletePictruesByPicIDs([FromBody]DeletePictrueReqDTO req)
        {
            string strJson = Json.JsonSerializerBySingleData(req);
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******DeletePictruesByPicIDs begin...->DeletePictrueReqDTO:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.WxEditor.WxPictureMaterial.Instance.DeletePictruesByPicIDs(req);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****DeletePictruesByPicIDs ->DeletePictrueReqDTO:" + strJson + ",批量删除微信图片失败:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******DeletePictruesByPicIDs end->");
            return jr;
        }

        /// <summary>
        /// zlb 2017-06-26
        /// 修改微信图片名称
        /// </summary>
        /// <param name="req">图片信息类</param
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult UpdatePictruesByPicID([FromBody]UpdatePictrueNameReqDTO req)
        {

            string strJson = Json.JsonSerializerBySingleData(req);
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******UpdatePictruesByPicID begin...->UpdatePictrueNameReqDTO:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.WxEditor.WxPictureMaterial.Instance.UpdatePictruesByPicID(req);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****UpdatePictruesByPicID ->UpdatePictrueNameReqDTO:" + strJson + ",修改微信图片名称失败:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******UpdatePictruesByPicID end->");
            return jr; ;
        }

        /// <summary>
        /// zlb 2017-06-26
        /// 批量上传微信图片
        /// </summary>
        /// <param name="PicIDs">图片ID集合</param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult InsertPictrues([FromBody]InsertPictruesReqDTO req)
        {
            string strJson = Json.JsonSerializerBySingleData(req);
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******InsertPictrues begin...->InsertPictruesReqDTO:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.WxEditor.WxPictureMaterial.Instance.InsertPictrues(req);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[WeChatEditorController]*****InsertPictrues ->InsertPictruesReqDTO:" + strJson + ",批量添加微信图片失败:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[WeChatEditorController]******InsertPictrues end->");
            return jr;
        }
        #endregion
    }
}
