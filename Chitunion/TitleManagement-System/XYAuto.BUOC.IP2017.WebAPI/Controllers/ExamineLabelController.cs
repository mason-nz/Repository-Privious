using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.WebAPI.App_Start;
using XYAuto.BUOC.IP2017.WebAPI.Filter;

namespace XYAuto.BUOC.IP2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
    public class ExamineLabelController : ApiController
    {

        #region 媒体信息
        /// <summary>
        /// zlb 2017-10-20
        /// 查询待审媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryPendingAuditMediaList([FromUri] ReqsAuditMediaDto ReqDto)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineMediaLabel.Instance.QueryPendingAuditMediaList(ReqDto);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryPendingAuditMediaList ->查询待审核媒体列表:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询已审媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryAuditedMediaList([FromUri] ReqsAuditMediaDto ReqDto)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineMediaLabel.Instance.QueryAuditedMediaList(ReqDto);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryAuditedMediaList ->查询已审核媒体列表:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-23
        /// 根据审核批次ID查询媒体待审标签和已审标签
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryAuditMediaLabel(int BatchAuditID)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineMediaLabel.Instance.QueryAuditMediaLabel(BatchAuditID);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryAuditMediaLabel ->BatchAuditID:" + BatchAuditID + ",查询媒体待审标签和已审标签接口出错:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-24
        /// 根据批次ID查询媒体待审标签或最终结果标签
        /// </summary>
        /// <param name="BatchID">批次ID</param>
        /// <param name="SelectType">查询类型（1审核 2修改）</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryPendingAuditMediaLabel(int BatchID, int SelectType)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineMediaLabel.Instance.QueryPendingAuditMediaLabel(BatchID, SelectType);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryPendingAuditMediaLabel ->BatchID:" + BatchID + ",查询媒体待审标签或最终结果标签接口出错:" + ex.Message);
                throw ex;
            }
        }
        #endregion
        #region 品牌信息
        /// <summary>
        /// zlb 2017-10-20
        /// 查询待审品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryPendingAuditBrandList([FromUri] ReqsAuditBrandDto ReqDto)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineBrandLabel.Instance.QueryPendingAuditBrandList(ReqDto);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryPendingAuditBrandList ->查询待审核品牌列表:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询已审品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryAuditedBrandList([FromUri] ReqsAuditBrandDto ReqDto)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineBrandLabel.Instance.QueryAuditedBrandList(ReqDto);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryAuditedBrandList ->查询已审核品牌列表:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-23
        /// 根据审核批次ID查询品牌待审标签和已审标签 
        /// </summary>
        /// <param name="BatchAuditID"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryAuditBrandLabel(int BatchAuditID)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineBrandLabel.Instance.QueryAuditBrandLabel(BatchAuditID);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryAuditBrandLabel ->BatchAuditID:" + BatchAuditID + ",查询品牌待审标签和已审标签接口出错:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-24
        /// 根据批次ID查询品牌待审标签或最终结果标签
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <param name="SelectType">查询类型（1审核 2修改）</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryPendingAuditBrandLabel(int BatchID, int SelectType)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ExamineLabel.ExmaineBrandLabel.Instance.QueryPendingAuditBrandLabel(BatchID, SelectType);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****QueryPendingAuditBrandLabel ->BatchID:" + BatchID + "->SelectType:" + SelectType + ", 查询品牌待审标签或最终结果标签接口出错:" + ex.Message);
                throw ex;
            }
        }
        #endregion
        #region 审核
        /// <summary>
        /// zlb 2017-10-26
        /// 审核标签或修改标签
        /// </summary>
        /// <param name="auditLabel"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult ExamineOrUpdateLabel([FromBody]AuditLabel auditLabel)
        {
            string strJson = Common.Json.JsonSerializerBySingleData(auditLabel);
            BLL.Loger.Log4Net.Info("[ExamineLabelController]******ExamineOrUpdateLabel begin...->AuditLabel:" + strJson + "");

            string messageStr = "";
            try
            {
                messageStr = BLL.ExamineLabel.ExamineLableOperate.Instance.ExamineOrUpdateLabel(auditLabel);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****ExamineOrUpdateLabel ->AuditLabel:" + strJson + ",审核或修改标签失败 :" + ex.Message);
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
            BLL.Loger.Log4Net.Info("[ExamineLabelController]******ExamineOrUpdateLabel end->");
            return jr;
        }
        /// <summary>
        /// 修改审核状态为审核中或待审核
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult UpdateMediaStatus([FromBody]ReqsMediaStatusDto ReqDTO)
        {
            string strJson = Common.Json.JsonSerializerBySingleData(ReqDTO);
            BLL.Loger.Log4Net.Info("[ExamineLabelController]******UpdateMediaStatus begin...->ReqDTO:" + strJson + "");
            string messageStr = "";
            int ArticleCount = 0;
            try
            {

                messageStr = BLL.ExamineLabel.ExamineLableOperate.Instance.UpdateMediaStatus(ReqDTO, out ArticleCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ExamineLabelController]*****UpdateMediaStatus ->ReqDTO:" + strJson + ",修改审核状态为审核中或待审核失败 :" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(ArticleCount, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(ArticleCount, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[ExamineLabelController]******UpdateMediaStatus end->");
            return jr;
        }
        #endregion

    }
}
