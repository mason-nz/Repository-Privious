using System;
using System.Collections.Generic;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Service.App.LuckDrawActivity;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    /// <summary>
    /// 抽奖
    /// </summary>
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    [CrossSite]
    public class LuckDrawController : ApiController
    {
        /// <summary>
        /// 查询用户今日剩余抽奖次数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDrawRemainder()
        {
            Dictionary<string, int> dic;
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                dic = AppLuckDrawService.Instance.GetDrawRemainder(userId);
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[LuckDrawController]*****GetDrawRemainder 查询用户今日剩余抽奖次数失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// 获取活动基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetActivityBasicInfo()
        {
            Dictionary<string, object> dic;
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                dic = AppLuckDrawService.Instance.GetActivityBasicInfo(userId);
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[LuckDrawController]*****GetActivityBasicInfo 查询抽奖信息失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// 查询用户获奖记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAwardRecord(int pageIndex = 1, int pageSize = 20)
        {
            Dictionary<string, object> dic;
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                dic = AppLuckDrawService.Instance.GetAwardRecord(userId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[LuckDrawController]*****GetAwardRecord 查询用户获奖记录失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }

        /// <summary>
        /// 查询抽奖信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAwardeeList()
        {
            List<Dictionary<string, object>> dicList;
            try
            {
                //dicList = XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity.LuckDraw.Instance.GetAwardeeList();
                //Modify=Masj,Date=2018-06-11,Remark=改成假数据了
                dicList = AppLuckDrawService.Instance.GetAwardeeMoniList();
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[LuckDrawController]*****GetAwardeeList 查询抽奖信息失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dicList, "Success");
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult LotteryDraw()
        {
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                int errorMsg = 0;
                var dicDraw = AppLuckDrawService.Instance.LotteryDraw(userId, out errorMsg);
                if (errorMsg < 0)
                {
                    return Common.Util.GetJsonDataByResult(null, errorMsg == -1 ? "完成签到后才可参与抽奖活动哦~" : "抽奖失败！请稍后重试", errorMsg);
                }
                dicDraw = AppLuckDrawService.Instance.GetTaskList(dicDraw);
                return Common.Util.GetJsonDataByResult(dicDraw, "Success", 0);
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("[LuckDrawController]*****LotteryDraw 抽奖失败:", ex);
                throw;
            }
        }
    }
}
