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
    public class ContentDistributeController : ApiController
    {

        [HttpGet]
        public Common.JsonResult GetContentDistributeList(int Status=0, int PageIndex = 1, int PageSize = 20, string Name = "")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {

                dic = ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front.ContentDistribute.Instance.GetContentDistributeList(Name, Status, PageIndex, PageSize);
            }
            catch (Exception ex)
            {

                Loger.Log4Net.Info("[ContentDistributeController]*****GetContentDistributeList ->Name:" + Name + " ->PageIndex:" + PageIndex + ",查询内容分发列表失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        [HttpPost]
        public Common.JsonResult AddContentDistributeInfo([FromBody] ReqDistributeDto Dto)
        {
            string strJson = XYAuto.ITSC.Chitunion2017.BLL.Json.JsonSerializerBySingleData(Dto);
            Loger.Log4Net.Info("[ContentDistributeController]******AddContentDistributeInfo begin...->ReqDistributeDto:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front.ContentDistribute.Instance.AddContentDistributeInfo(Dto);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ContentDistributeController]*****AddContentDistributeInfo ->ReqDistributeDto:" + strJson + ",添加内容分发推广计划失败:" + ex.Message);
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
            Loger.Log4Net.Info("[ContentDistributeController]******AddContentDistributeInfo end->");
            return jr;
        }
        [HttpGet]
        public Common.JsonResult GetContentDistributeDetailInfo(int RecID)
        {
            RespDistributeDto req = new RespDistributeDto();
            try
            {

                req = ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front.ContentDistribute.Instance.GetContentDistributeDetailInfo(RecID);
            }
            catch (Exception ex)
            {

                Loger.Log4Net.Info("[ContentDistributeController]*****GetContentDistributeDetailInfo ->RecID:" + RecID + ",内容分发列表详情失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(req, "Success");
        }
    }
}
