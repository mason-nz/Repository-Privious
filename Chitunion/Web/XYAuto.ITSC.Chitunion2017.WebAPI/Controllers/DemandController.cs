using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class DemandController : ApiController
    {
        #region 需求模块
        /// <summary>
        /// 获取需求列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDemandList([FromUri]GetDemandListReqDTO req)
        {
            string msg = string.Empty;
            GetDemandListResDTO res = BLL.Demand.Instance.GetDemandList(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 获取需求详情
        /// </summary>
        /// <param name="DemandBillNo"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDemandDetail(int DemandBillNo)
        {
            string msg = string.Empty;
            GetDemandDetailResDTO res = BLL.Demand.Instance.GetDemandDetail(DemandBillNo, ref msg);
            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        /// 审核、终止需求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditDemand([FromBody]AuditDemandReqDTO req)
        {
            string msg = string.Empty;
            int nextBillNo = 0;
            bool res = BLL.Demand.Instance.AuditDemand(req, ref msg, ref nextBillNo);
            return Util.GetJsonDataByResult(nextBillNo, msg, res ? 0 : 1);
        }

        /// <summary>
        /// 获取需求可选广告组列表
        /// </summary>
        /// <param name="DemandBillNo"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetWaittingADList(int DemandBillNo)
        {
            string msg = string.Empty;
            List<ADGroupDTO> res = BLL.Demand.Instance.GetWaittingADList(DemandBillNo, ref msg);
            return Util.GetJsonDataByResult(res,msg,res == null ? 1 : 0);
        }

        /// <summary>
        /// 需求关联到广告组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RelateToADGroup([FromBody]AuditDemandReqDTO req)
        {
            string msg = string.Empty;
            bool res = BLL.Demand.Instance.RelateToADGroup(req, ref msg);
            return Util.GetJsonDataByResult(null, msg, res ? 0 : 1);
        }


        #endregion

        #region 效果图模块
        /// <summary>
        /// 效果图左侧 需求-广告组菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MapGetLeft()
        {
            string msg = string.Empty;
            List<MapGetLeftItemDTO> res = BLL.Demand.Instance.MapGetLeft(ref msg);
            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        /// 账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MapGetRightOne()
        {
            string msg = string.Empty;
            MapGetRightOneResDTO res = BLL.Demand.Instance.MapGetRightOne(ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 曝光点击等
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MapGetRightTwo([FromUri] MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            MapGetRightTwoResDTO res = BLL.Demand.Instance.MapGetRightTwo(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }
        /// <summary>
        /// 趋势图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MapGetRightThree([FromUri]MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            List<MapGetRightThreeItemDTO> res = BLL.Demand.Instance.MapGetRightThree(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }
        /// <summary>
        /// 数据排行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MapGetRightFour([FromUri] MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            MapGetRightADListResDTO res = BLL.Demand.Instance.MapGetRightFour(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1: 0);
        }
        /// <summary>
        /// 数据明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MapGetRightFive([FromUri]MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            MapGetRightADListResDTO res = BLL.Demand.Instance.MapGetRightFive(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }
        /// <summary>
        /// 明细导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MapExportToExcel([FromBody]MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            string path = BLL.Demand.Instance.MapExportToExcel(req, ref msg);
            return Util.GetJsonDataByResult(path, msg, string.IsNullOrEmpty(path) ? 1 : 0);
        }


        #endregion
    }
}
