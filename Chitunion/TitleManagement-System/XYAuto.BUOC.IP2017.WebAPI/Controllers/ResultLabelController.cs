using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.BUOC.IP2017.WebAPI.App_Start;
using XYAuto.BUOC.IP2017.WebAPI.Filter;

namespace XYAuto.BUOC.IP2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
    public class ResultLabelController : ApiController
    {
        /// <summary>
        /// zlb 2017-10-26
        /// 查询结果媒体标签
        /// </summary>
        /// <param name="MediaResultID">结果id</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryResultMediaLabel(int MediaResultID)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ResultLabel.ResultLabel.Instance.QueryResultMediaLabel(MediaResultID);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ResultLabelController]*****QueryResultMediaLabel ->MediaResultID:" + MediaResultID + ", 查询结果媒体标签接口出错:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 查询结果品牌标签
        /// </summary>
        /// <param name="MediaResultID">结果id</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryResultBrandLabel(int MediaResultID)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ResultLabel.ResultLabel.Instance.QueryResultBrandLabel(MediaResultID);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ResultLabelController]*****QueryResultBrandLabel ->MediaResultID:" + MediaResultID + ", 查询结果媒体标签接口出错:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询结果品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryResultBrandList([FromUri] ReqsAuditBrandDto ReqDto)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ResultLabel.ResultLabel.Instance.QueryResultBrandList(ReqDto);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ResultLabelController]*****QueryResultBrandList ->查询结果品牌列表:" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询结果媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult QueryResultMediaList([FromUri] ReqsAuditMediaDto ReqDto)
        {
            try
            {
                Dictionary<string, object> dicAll = BLL.ResultLabel.ResultLabel.Instance.QueryResultMediaList(ReqDto);
                return Common.Util.GetJsonDataByResult(dicAll, "Success");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ResultLabelController]*****QueryResultMediaList ->查询结果媒体列表:" + ex.Message);
                throw ex;
            }
        }
        [HttpPost]
        [ApiLog]
        public Common.JsonResult DeleteMediaLabel([FromBody]ReqResultIdDto DTO)
        {
            string strJson = Common.Json.JsonSerializerBySingleData(DTO);
            BLL.Loger.Log4Net.Info("[ResultLabelController]******DeleteMediaLabel begin...->DTO:" + strJson + "");

            string messageStr = "";
            try
            {
                messageStr = BLL.ResultLabel.ResultLabel.Instance.DeleteMediaLabel(DTO);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[ResultLabelController]*****DeleteMediaLabel ->DTO:" + strJson + ",删除失败 :" + ex.Message);
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
            BLL.Loger.Log4Net.Info("[ResultLabelController]******DeleteMediaLabel end->");
            return jr;
        }
    }
}
