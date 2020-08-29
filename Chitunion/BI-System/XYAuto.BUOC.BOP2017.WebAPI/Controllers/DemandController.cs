using System;
using System.Collections.Generic;
using System.Web.Http;
using XYAuto.BUOC.BOP2017.BLL.Demand.Resolver;
using XYAuto.BUOC.BOP2017.BLL.GDT;
using XYAuto.BUOC.BOP2017.Entities.Dto;
using XYAuto.BUOC.BOP2017.WebAPI.App_Start;
using XYAuto.BUOC.BOP2017.WebAPI.Common;
using XYAuto.BUOC.BOP2017.WebAPI.Filter;

namespace XYAuto.BUOC.BOP2017.WebAPI.Controllers
{
    [CrossSite]
    public class DemandController : BaseApiController
    {
        #region 需求模块

        /// <summary>
        /// 获取需求列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDemandList([FromUri]GetDemandListReqDTO req)
        {
            string msg = string.Empty;

            var ur = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRoleIDs(GetUserInfo.UserID);
            if (ur.IndexOf("SYS005RL00021", StringComparison.Ordinal) > -1)
            {
                //广告运营
                req.UserId = GetUserInfo.UserID;
            }
            GetDemandListResDTO res = BLL.Demand.Demand.Instance.GetDemandList(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 获取需求详情
        /// </summary>
        /// <param name="DemandBillNo"></param>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [ApiLog]
        [HttpGet]
        public JsonResult GetDemandDetail(int DemandBillNo)
        {
            string msg = string.Empty;
            GetDemandDetailResDTO res = BLL.Demand.Demand.Instance.GetDemandDetail(DemandBillNo, ref msg);
            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        /// 审核、终止需求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS005BUT3001")]
        public JsonResult AuditDemand([FromBody]AuditDemandReqDto req)
        {
            string msg = string.Empty;
            int nextBillNo = 0;
            bool res = BLL.Demand.Demand.Instance.AuditDemand(req, ref msg, ref nextBillNo);
            return Util.GetJsonDataByResult(nextBillNo, msg, res ? 0 : 1);
        }

        /// <summary>
        /// 获取需求可选广告组列表
        /// </summary>
        /// <param name="DemandBillNo"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetWaittingADList(int DemandBillNo)
        {
            string msg = string.Empty;
            List<ADGroupDTO> res = BLL.Demand.Demand.Instance.GetWaittingADList(DemandBillNo, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 需求关联到广告组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult RelateToAdGroup([FromBody]AuditDemandReqDto req)
        {
            string msg = string.Empty;
            bool res = BLL.Demand.Demand.Instance.RelateToADGroup(req, ref msg);
            return Util.GetJsonDataByResult(null, msg, res ? 0 : 1);
        }

        #endregion 需求模块

        #region 效果图模块

        /// <summary>
        /// 效果图左侧 需求-广告组菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MapGetLeft()
        {
            string msg = string.Empty;
            List<MapGetLeftItemDTO> res = BLL.Demand.Demand.Instance.MapGetLeft(ref msg);
            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        /// 账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MapGetRightOne()
        {
            string msg = string.Empty;
            MapGetRightOneResDTO res = BLL.Demand.Demand.Instance.MapGetRightOne(ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 曝光点击等
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MapGetRightTwo([FromUri] MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            MapGetRightTwoResDTO res = BLL.Demand.Demand.Instance.MapGetRightTwo(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 趋势图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MapGetRightThree([FromUri]MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            List<MapGetRightThreeItemDTO> res = BLL.Demand.Demand.Instance.MapGetRightThree(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 数据排行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MapGetRightFour([FromUri] MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            MapGetRightADListResDTO res = BLL.Demand.Demand.Instance.MapGetRightFour(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 数据明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MapGetRightFive([FromUri]MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            MapGetRightADListResDTO res = BLL.Demand.Demand.Instance.MapGetRightFive(req, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// 明细导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult MapExportToExcel([FromBody]MapGetRightReqDTO req)
        {
            string msg = string.Empty;
            string path = BLL.Demand.Demand.Instance.MapExportToExcel(req, ref msg);
            return Util.GetJsonDataByResult(path, msg, string.IsNullOrEmpty(path) ? 1 : 0);
        }

        #endregion 效果图模块

        #region 清洗数据

        [HttpPost]
        public JsonResult CleanData()
        {
            DemandJsonAnalysis.Instance.CleanData();
            return new JsonResult();
        }

        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ImportUserOrganize([FromBody]ImportUserOrganizeDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null || string.IsNullOrWhiteSpace(request.FileName))
            {
                jsonResult.Message = "请输入导入的文件名（带后缀名）";
                jsonResult.Status = -1;
                return jsonResult;
            }
            var retValue = new LogicTransferProvider().ImportUser(request.FileName);
            return jsonResult.GetReturn(retValue);
        }

        public class ImportUserOrganizeDto
        {
            public string FileName { get; set; }
        }

        #endregion 清洗数据
    }
}