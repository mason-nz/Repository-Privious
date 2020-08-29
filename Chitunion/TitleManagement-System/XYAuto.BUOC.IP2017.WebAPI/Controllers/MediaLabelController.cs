using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.IP2017.BLL.BatchMedia.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.MediaLabel.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.WebAPI.App_Start;
using XYAuto.BUOC.IP2017.WebAPI.Filter;

namespace XYAuto.BUOC.IP2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
    public class MediaLabelController : ApiController
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1298;
                }
                return _currentUserID;
            }
        }
        private static string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        #endregion        
        #region 获取文章摘要关键词
        [HttpGet]
        [ApiLog]
        public Common.JsonResult GetSummaryKeyWord(int mediaType, int articleID, int summarySize, int keyWordSize)
        {
            try
            {
                var ret = BLL.MediaLabel.MediaLabel.Instance.GetSummaryKeyWord(mediaType, articleID, summarySize, keyWordSize);
                return WebAPI.Common.Util.GetJsonDataByResult(ret);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]GetSummaryKeyWord出错：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 根据文章ID切换文章内容接口（标题、原文地址)
        [ApiLog]
        [HttpGet]
        public Common.JsonResult QueryArticle(int articleID, int mediaType)
        {
            try
            {
                var resDto = BLL.ArticleInfo.ArticleInfo.Instance.QueryArticle(articleID, mediaType);
                return WebAPI.Common.Util.GetJsonDataByResult(resDto, resDto == null ? "没有数据" : string.Empty, 0);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"QueryArticle接口出错:{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult("接口出错：", ex.Message, -1);
            }
        }
        #endregion
        #region 标签录入列表筛选条件，常见分类接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult GetCommonlyClass(int mediaType)
        {
            if (!Enum.IsDefined(typeof(Entities.ENUM.ENUM.EnumMediaType), mediaType))
                return WebAPI.Common.Util.GetJsonDataByResult(null, $"媒体类型参数错误：{mediaType}", -1);

            try
            {
                var ret = BLL.DictInfo.DictInfo.Instance.GetDictInfoByMediaType((Entities.ENUM.ENUM.EnumMediaType)mediaType);
                return WebAPI.Common.Util.GetJsonDataByResult(ret);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]GetCommonlyClass：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 标签录入列表接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult InputListMedia([FromUri] ReqInputListMediaDto request)
        {
            string errmsg = string.Empty;
            try
            {
                request.CurrentUserID = currentUserID;
                var ret = BLL.MediaLabel.MediaLabel.Instance.InputListMedia(request, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]InputListMedia：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 打标签媒体或打标签文章渲染
        [HttpGet]
        [ApiLog]
        public Common.JsonResult RenderBatchMedia([FromUri] ReqBatchMediaDto request)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.MediaLabel.MediaLabel.Instance.RenderBatchMedia(request, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]RenderBatchMedia：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 根据标签母IP获取子IP
        [HttpGet]
        [ApiLog]
        public Common.JsonResult GetSubIPByPID(int dictID)
        {
            try
            {
                var ret = BLL.MediaLabel.MediaLabel.Instance.GetSubIPByPID(dictID);
                return WebAPI.Common.Util.GetJsonDataByResult(ret);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]GetSubIPByPID：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 打标签媒体或打标签文章提交
        [HttpPost]
        [ApiLog]
        public Common.JsonResult BatchMediaSubmit([FromBody] ReqBatchMediaSubmitDto request)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.MediaLabel.MediaLabel.Instance.BatchMediaSubmit(request, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]BatchMediaSubmit：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 打标签批次列表接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult BatchListMedia([FromUri] ReqBatchListMediaDto request)
        {
            string errmsg = string.Empty;
            try
            {
                request.CurrentUserID = currentUserID;
                var result = new BLL.Business.Query.V1_2_4.BatchListMediaQuery().GetQueryList(request);
                return WebAPI.Common.Util.GetJsonDataByResult(result);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]BatchListMedia：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 查看文章或媒体已审批次详情接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult ViewBatchMedia(int BatchMediaID)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.MediaLabel.MediaLabel.Instance.ViewBatchMedia(BatchMediaID, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]ViewBatchMedia：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 打标签媒体文章列表查询或领取接口
        [HttpGet]
        [ApiLog]
        public Common.JsonResult ArticleQueryOrRecive([FromUri] ReqArticleQueryOrReciveDto request)
        {
            string errmsg = string.Empty;
            try
            {
                request.CurrentUserID = currentUserID;
                if (request.IsRecive)
                {
                    var ret = BLL.MediaLabel.MediaLabel.Instance.ArticleRecive(request, ref errmsg);
                    return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
                }
                else
                {
                    var ret = BLL.MediaLabel.MediaLabel.Instance.ArticleQuery(request, ref errmsg);
                    return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]ArticleQueryOrRecive：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        #region 根据打标签人提交的批次ID查询所属文章列表
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryArticleListByBactchID([FromUri] ReqArticleListByBactchIDQueryDto request)
        {
            string errmsg = string.Empty;
            try
            {
                var ret = BLL.MediaLabel.MediaLabel.Instance.QueryArticleListByBactchID(request, ref errmsg);
                return errmsg == string.Empty ? WebAPI.Common.Util.GetJsonDataByResult(ret) : WebAPI.Common.Util.GetJsonDataByResult("操作失败", errmsg, -2);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info($"[MediaLabelController]ArticleQueryOrRecive：{ex.Message}");
                return WebAPI.Common.Util.GetJsonDataByResult(null, ex.Message, -1);
            }
        }
        #endregion
        /// ZLB 2017-11-28
        /// 查询打标签的文章数量
        /// </summary>
        /// <param name="BatchMediaID"></param>
        /// <param name="BatchType">类型（1媒体批次 2审核批次）</param>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryArticleCount(int BatchMediaID,int BatchType)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.MediaLabel.MediaLabel.Instance.SelectArticleCount(BatchMediaID, BatchType);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaLabelController]*****QueryArticleCount-> BatchMediaID:"+BatchMediaID+ "BatchType:" + BatchType + "查询文章数量出错:" + ex.Message);
                throw ex;
            }
        }


    }
}
