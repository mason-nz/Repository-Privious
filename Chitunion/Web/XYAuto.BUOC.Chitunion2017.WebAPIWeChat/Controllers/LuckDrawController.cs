using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    [CrossSite]
    public class LuckDrawController : ApiController
    {
        [HttpGet]
        public Common.JsonResult GetDrawRemainder()
        {
            Dictionary<string, int> dic;
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                dic = XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity.LuckDraw.Instance.GetDrawRemainder(userId);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[LuckDrawController]*****GetDrawRemainder 查询用户今日剩余抽奖次数失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        [HttpGet]
        public Common.JsonResult GetActivityBasicInfo()
        {
            Dictionary<string, object> dic;
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                dic = XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity.LuckDraw.Instance.GetActivityBasicInfo(userId);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[LuckDrawController]*****GetActivityBasicInfo 查询抽奖信息失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        [HttpGet]
        public Common.JsonResult GetAwardRecord(int pageIndex = 1, int pageSize = 20)
        {
            Dictionary<string, object> dic;
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                dic = XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity.LuckDraw.Instance.GetAwardRecord(userId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[LuckDrawController]*****GetAwardRecord 查询用户获奖记录失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        [HttpGet]
        public Common.JsonResult GetAwardeeList()
        {
            List<Dictionary<string, object>> dicList;
            try
            {
                //dicList = XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity.LuckDraw.Instance.GetAwardeeList();
                //Modify=Masj,Date=2018-06-11,Remark=改成假数据了
                dicList = XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity.LuckDraw.Instance.GetAwardeeMoniList();
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[LuckDrawController]*****GetAwardeeList 查询抽奖信息失败:", ex);
                throw;
            }
            return Common.Util.GetJsonDataByResult(dicList, "Success");
        }
        [ApiLog]
        [HttpPost]
        public Common.JsonResult LotteryDraw()
        {
            try
            {
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                int errorMsg = 0;
                var dicDraw = XYAuto.ITSC.Chitunion2017.BLL.LuckDrawActivity.LuckDraw.Instance.LotteryDraw(userId, out errorMsg);
                if (errorMsg < 0)
                {
                    return Common.Util.GetJsonDataByResult(null, errorMsg == -1 ? "完成签到后才可参与抽奖活动哦~" : "抽奖失败！请稍后重试", errorMsg);
                }
                var list = ITSC.Chitunion2017.BLL.LETask.V2_3.LE_Task.Instance.GetDataByPageV2_5(new ITSC.Chitunion2017.Entities.DTO.V2_3.TaskResDTO()
                {
                    UserID = userId,
                    SceneID = -2,
                    PageIndex = -2,
                    PageSize = 15
                });
                dicDraw.Add("List", list);
                return Common.Util.GetJsonDataByResult(dicDraw, "Success", 0);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[LuckDrawController]*****LotteryDraw 抽奖失败:", ex);
                throw;
            }
        }

    }
}
