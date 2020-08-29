using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V_2_0;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class MediaPromotionController : ApiController
    {
        [HttpGet]
        public Common.JsonResult GetMediaPromotionList(int Status=0, int PageIndex = 1, int PageSize = 20, string Name = "")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {

                dic = ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front.MediaPromotion.Instance.GetMediaPromotionList(Name, Status, PageIndex, PageSize);
            }
            catch (Exception ex)
            {

                Loger.Log4Net.Info("[MediaPromotionController]*****GetMediaPromotionList ->Name:" + Name + " ->PageIndex:" + PageIndex + ",查询媒体智投列表失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        [HttpPost]
        public Common.JsonResult AddMediaPromotionInfo([FromBody]ReqPromotionDto Dto)
        {
            string strJson = XYAuto.ITSC.Chitunion2017.BLL.Json.JsonSerializerBySingleData(Dto);
            Loger.Log4Net.Info("[MediaPromotionController]******AddMediaPromotionInfo begin...->ReqPromotionDto:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front.MediaPromotion.Instance.AddMediaPromotionInfo(Dto);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[MediaPromotionController]*****AddMediaPromotionInfo ->ReqPromotionDto:" + strJson + ",添加媒体智投推广计划失败:" + ex.Message);
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
            Loger.Log4Net.Info("[MediaPromotionController]******AddMediaPromotionInfo end->");
            return jr;
        }
        [HttpGet]
        public Common.JsonResult GetMediaPromotionInfo(int RecID)
        {
            RespPromotionDto req = new RespPromotionDto();
            try
            {

                req = ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front.MediaPromotion.Instance.GetMediaPromotionInfo(RecID);
            }
            catch (Exception ex)
            {

                Loger.Log4Net.Info("[MediaPromotionController]*****GetMediaPromotionInfo ->RecID:" + RecID + ",内容媒体智投详情失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(req, "Success");
        }
    }
}
