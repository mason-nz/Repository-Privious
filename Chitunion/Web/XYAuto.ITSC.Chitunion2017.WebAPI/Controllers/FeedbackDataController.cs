using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    /// <summary>
    /// 2017-02-26张立彬
    /// 反馈数据类
    /// </summary>
    [CrossSite]
    public class FeedbackDataController : ApiController
    {
        //
        // GET: /FeedbackData/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// 2017-02-26张立彬
        /// 添加或修改反馈数据
        /// </summary>
        /// <param name="feedbackData"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS001BUT20007")]
        public Common.JsonResult InserFeedbackData([FromBody]Entities.FeedbackData feedbackData)
        {
            string strJson = Json.JsonSerializerBySingleData(feedbackData);
            BLL.Loger.Log4Net.Info("[FeedbackDataController]******InserFeedbackData begin...->feedbackData:" + strJson + "");
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            string messageStr = "";
            try
            {
                messageStr = BLL.FeedbackData.Instance.InserFeedbackData(feedbackData);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[FeedbackDataController]*****InserFeedbackData ->feedbackData:" + strJson + ",添加或修改反馈数据出错:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[FeedbackDataController]******InserFeedbackData end->");
            return jr;
        }
        /// <summary>
        /// 2017-02-27张立彬
        /// 根据子单编号和媒体类型查询反馈数据
        /// </summary>
        /// <param name="subOrderID">子单编号</param>
        /// <param name="mediaType">媒体类型</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS001BUT20002")]
        public Common.JsonResult SelectFeedbackData(string subOrderID, int mediaType)
        {
            List<Entities.SelectFeedbackData> listFeedbackData = new List<Entities.SelectFeedbackData>();
            try
            {
                listFeedbackData = BLL.FeedbackData.Instance.SelectFeedbackData(subOrderID, mediaType);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[FeedbackDataController]*****SelectFeedbackData ->subOrderID:" + subOrderID + " ->mediaType:" + mediaType + ",查询反馈数据信息出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(listFeedbackData, "Success");
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS001BUT20007")]
        public Common.JsonResult DeleteFeedbackData(int MediaType, int FeedbackID, string FileUrl)
        {

            BLL.Loger.Log4Net.Info("[FeedbackDataController]******DeleteFeedbackData begin...->MediaType:" + MediaType + "->FeedbackID:" + FeedbackID);
            string messageStr = "";
            try
            {
                messageStr = BLL.FeedbackData.Instance.DeleteFeedbackData(MediaType, FeedbackID, FileUrl);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[FeedbackDataController]*****DeleteFeedbackData ...->MediaType:" + MediaType + "->FeedbackID:" + FeedbackID + ",删除反馈数据出错:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[FeedbackDataController]******DeleteFeedbackData end->");
            return jr;
        }


    }
}
