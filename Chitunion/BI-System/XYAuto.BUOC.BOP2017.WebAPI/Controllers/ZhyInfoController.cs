﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using XYAuto.BUOC.BOP2017.Entities.Dto;
using XYAuto.BUOC.BOP2017.Entities.ZHY;
using XYAuto.BUOC.BOP2017.WebAPI.App_Start;
using XYAuto.BUOC.BOP2017.WebAPI.Common;
using XYAuto.BUOC.BOP2017.WebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.BUOC.BOP2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ZhyInfoController : ApiController
    {
        /// <summary>
        /// zlb 2017-08-18
        /// 根查询广告主/子客列表
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectZhyAdvertiserList(string Mobile = "", int PageIndex = 1, int PageSize = 20)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectZhyAdvertiserList(Mobile, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectZhyAdvertiserList ->Mobile:" + Mobile + ",查询广告主/子客列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询广告运营角色列表
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Mobile">手机号</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectZhyOperaterList(string UserName = "", string Mobile = "", int PageIndex = 1, int PageSize = 20)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectZhyOperaterList(UserName, Mobile, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectZhyOperaterList ->UserName:" + UserName + "->Mobile:" + Mobile + ",查询广告运营角色列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询未绑定子客和广告运营用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectGdtUserAndOperaterList()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectGdtUserAndOperaterList();
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectGdtUserAndOperaterList;查询未绑定的子客信息和广告运营角色用户列表:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询未绑定运营的广告主信息集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectFreeAdvertiserList()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectFreeAdvertiserList();
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectFreeAdvertiserList;查询未绑定运营的广告主信息集合出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-22
        ///查询广告运营所管理的广告主
        /// </summary>
        /// <param name="OperaterId">广告运营ID</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectOptAdvertiserList(int OperaterId)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectOptAdvertiserList(OperaterId);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectOptAdvertiserList;查询广告运营所管理的广告主信息集合出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-18
        ///子客和广告主绑定
        /// </summary>
        /// <param name="ReqDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult BingdingOptIdAndAccIdToAdvId([FromBody]BindingIdReqDTO ReqDTO)
        {
            string strJson = Common.Json.JsonSerializerBySingleData(ReqDTO);
            Loger.Log4Net.Info("[ZhyInfoController]******BingdingOptIdAndAccIdToAdvId begin...->BindingIdReqDTO:" + strJson + "");
            try
            {
                string result = BLL.ZHY.ZhyInfo.Instance.BingdingOptIdAndAccIdToAdvId(ReqDTO);
                JsonResult jr;
                if (result == "")
                {
                    jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
                }
                else
                {
                    jr = Common.Util.GetJsonDataByResult(null, result, -1);
                }
                Loger.Log4Net.Info("[ZhyInfoController]******BingdingOptIdAndAccIdToAdvId end->");
                return jr;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****BingdingOptIdAndAccIdToAdvId ->BindingIdReqDTO:" + strJson + "广告主绑定子客和绑定运营角色出错:" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 运营批量绑定广告主
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult BingdingOptIdToAdvsId([FromBody]AdvIdToOptIdReqDTO ReqDTO)
        {
            string strJson = Common.Json.JsonSerializerBySingleData(ReqDTO);
            Loger.Log4Net.Info("[ZhyInfoController]******BingdingOptIdToAdvsId begin...->AdvIdToOptIdReqDTO:" + strJson + "");
            try
            {
                string result = BLL.ZHY.ZhyInfo.Instance.BingdingOptIdToAdvsId(ReqDTO);
                JsonResult jr;
                if (result == "")
                {
                    jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
                }
                else
                {
                    jr = Common.Util.GetJsonDataByResult(null, result, -1);
                }
                Loger.Log4Net.Info("[ZhyInfoController]******BingdingOptIdToAdvsId end->");
                return jr;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****BingdingOptIdToAdvsId ->AdvIdToOptIdReqDTO:" + strJson + "运营批量绑定广告主出错:" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 解除广告主与子客的绑定
        /// </summary>
        /// <param name="ReqDto">广告主ID</param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult DeleteAdvsiterByAdvId([FromBody]AdvertiserIdReqDTO ReqDTO)
        {
            string strJson = Common.Json.JsonSerializerBySingleData(ReqDTO);
            Loger.Log4Net.Info("[ZhyInfoController]******DeleteAdvsiterByAdvId begin...->AdvertiserIdReqDTO:" + strJson + "");
            try
            {
                string result = BLL.ZHY.ZhyInfo.Instance.DeleteAdvsiterByAdvId(ReqDTO);
                JsonResult jr;
                if (result == "")
                {
                    jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
                }
                else
                {
                    jr = Common.Util.GetJsonDataByResult(null, result, -1);
                }
                Loger.Log4Net.Info("[ZhyInfoController]******AdvIdToOptIdReqDTO end->");
                return jr;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****DeleteAdvsiterByAdvId ->AdvertiserIdReqDTO:" + strJson + "解除广告主与子客的绑定出错:" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询广点通流水明细
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="GdtNum"></param>
        /// <param name="AccountType"></param>
        /// <param name="TradeType"></param>
        /// <param name="StarDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectGdtFlowDetail(int AccountType, int TradeType, string StarDate = "", string EndDate = "", int PageIndex = 1, int PageSize = 20, string UserName = "", string GdtNum = "")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectGdtFlowDetail(UserName, GdtNum, AccountType, TradeType, StarDate, EndDate, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectGdtFlowDetail ->AccountType:" + AccountType + "->TradeType" + TradeType + "->StarDate" + StarDate + "->EndDate" + EndDate + "->UserName" + UserName + "->GdtNum" + GdtNum + ",查询广点通流水明细出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询日结明细
        /// </summary>
        /// <param name="AccountType"></param>
        /// <param name="TradeType"></param>
        /// <param name="StarDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectGdtDateSummaryInfo(int AccountType, int TradeType, string StarDate = "", string EndDate = "", int PageIndex = 1, int PageSize = 20)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectGdtDateSummaryInfo(AccountType, TradeType, StarDate, EndDate, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectGdtDateSummaryInfo ->AccountType:" + AccountType + "->TradeType" + TradeType + "->StarDate" + StarDate + "->EndDate" + EndDate + ",查询日结汇总出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询充值列表
        /// </summary>
        /// <param name="RechargeNumber"></param>
        /// <param name="DemandBillNo"></param>
        /// <param name="ExternalBillNo"></param>
        /// <param name="UserName"></param>
        /// <param name="RechargeStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectGdtRechargeInfo(string RechargeNumber, string DemandBillNo, string ExternalBillNo, string UserName, int RechargeStatus, int PageIndex, int PageSize)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectGdtRechargeInfo(RechargeNumber, DemandBillNo, ExternalBillNo, UserName, RechargeStatus, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectGdtRechargeInfo ->RechargeNumber:" + RechargeNumber + "->DemandBillNo" + DemandBillNo + "->ExternalBillNo" + ExternalBillNo + "->DemandBillNo" + DemandBillNo + "->UserName" + UserName + "->RechargeStatus" + RechargeStatus + ",查询充值列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询未关联的GDT流水单列表
        /// </summary>
        /// <param name="TradeType">84001:充值，84003回划 </param>
        /// <param name="DemandBillNo">需求单号 </param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectGdtInfo(int TradeType, int DemandBillNo)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectGdtInfo(TradeType, DemandBillNo);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectGdtInfo ->TradeType:" + TradeType + ", 查询未关联的GDT流水单列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        ///  zlb 2017-08-21
        ///  充值对账或资金回划绑定GDT流水单
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult BingdingTradeRelation([FromBody]GdtTradeReqDTO ReqDTO)
        {
            string strJson = Common.Json.JsonSerializerBySingleData(ReqDTO);
            Loger.Log4Net.Info("[ZhyInfoController]******BingdingTradeRelation begin...->GdtTradeReqDTO:" + strJson + "");
            try
            {
                string result = BLL.ZHY.ZhyInfo.Instance.BingdingTradeRelation(ReqDTO);
                JsonResult jr;
                if (result == "")
                {
                    jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
                }
                else
                {
                    jr = Common.Util.GetJsonDataByResult(null, result, -1);
                }
                Loger.Log4Net.Info("[ZhyInfoController]******BingdingTradeRelation end->");
                return jr;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****BingdingTradeRelation ->GdtTradeReqDTO:" + strJson + "充值对账或资金回划绑定GDT流水单出错:" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询资金回划列表
        /// </summary>
        /// <param name="RechargeNumber"></param>
        /// <param name="DemandBillNo"></param>
        /// <param name="ExternalBillNo"></param>
        /// <param name="UserName"></param>
        /// <param name="HandleStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectBackAmountInfo(string RechargeNumber, string DemandBillNo, string ExternalBillNo, string UserName, int HandleStatus, int PageIndex, int PageSize)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectBackAmountInfo(RechargeNumber, DemandBillNo, ExternalBillNo, UserName, HandleStatus, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectGdtRechargeInfo ->RechargeNumber:" + RechargeNumber + "->DemandBillNo" + DemandBillNo + "->ExternalBillNo" + ExternalBillNo + "->DemandBillNo" + DemandBillNo + "->UserName" + UserName + "->HandleStatus" + HandleStatus + ",查询充值列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        /// <summary>
        /// zlb 2017-09-30
        /// 查询广告主客户详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectZhyAdvertiserInfo(int UserId)
        {
            ZhyAdvertiserInfo Info;
            try
            {
                Info = BLL.ZHY.ZhyInfo.Instance.SelectZhyAdvertiserInfo(UserId);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectZhyAdvertiserInfo ->UserId:" + UserId + ",查询:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(Info, "Success");
        }
        /// <summary>
        /// zlb 查询需求充值列表
        /// </summary>
        /// <param name="RechargeNumber">充值单号</param>
        /// <param name="DemandBillNo">需求单号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="StarDate">充值请求开始时间</param>
        /// <param name="EndDate">充值请求结束时间</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectDemandBillRecharge(string DemandBillNo="", string RechargeNumber = "", string UserName = "", string StarDate = "", string EndDate = "", int PageIndex = 1, int PageSize = 20)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectDemandBillRecharge(RechargeNumber, DemandBillNo, UserName, StarDate, EndDate, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectDemandBillRecharge ->RechargeNumber:" + RechargeNumber + "->DemandBillNo：" + DemandBillNo + "->DemandBillNo：" + DemandBillNo + "->UserName：" + UserName + "->StarDate：" + StarDate + "->EndDate：" + EndDate + ",查询需求充值列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        /// <summary>
        /// zlb 2017-09-30
        /// 查询异常数据列表
        /// </summary>
        /// <param name="RechargeNumber">充值单号</param>
        /// <param name="DemandBillNo">需求单号</param>
        /// <param name="DemandBillName">需求名称</param>
        /// <param name="ExceptionType">异常类型（0全部 1充值异常 2回划异常）</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectExceptionDataList(string DemandBillNo="", string RechargeNumber = "", string DemandBillName = "", int ExceptionType = 0, int PageIndex = 1, int PageSize = 20)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectExceptionDataList(RechargeNumber, DemandBillNo, DemandBillName, ExceptionType, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectExceptionDataList ->RechargeNumber:" + RechargeNumber + "->DemandBillNo：" + DemandBillNo + "->DemandBillName：" + DemandBillName + "->ExceptionType：" + ExceptionType + ",查询异常数据列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        /// <summary>
        /// zlb 2017-09-30
        /// 查询需求花费记录
        /// </summary>
        /// <param name="DemandBillNo">需求单号</param>
        /// <param name="DemandBillName">需求名称</param>
        /// <param name="StarDate">需求创建开始时间</param>
        /// <param name="EndDate">需求结束开始时间</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SelectDemandSpendList(string DemandBillNo="", string DemandBillName = "", string StarDate = "", string EndDate = "", int PageIndex = 1, int PageSize = 20)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = BLL.ZHY.ZhyInfo.Instance.SelectDemandSpendList(DemandBillNo, DemandBillName, StarDate, EndDate, PageIndex, PageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ZhyInfoController]*****SelectDemandSpendList ->DemandBillName:" + DemandBillName + "->DemandBillNo：" + DemandBillNo + "->StarDate：" + StarDate + "->EndDate：" + EndDate + ",查询需求花费列表出错:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
    }
}