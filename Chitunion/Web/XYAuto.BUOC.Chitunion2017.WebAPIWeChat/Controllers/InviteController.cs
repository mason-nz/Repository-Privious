using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatInvite;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    [CrossSite]
    public class InviteController : ApiController
    {
        ///// <summary>.,
        ///// 好友关注接口
        ///// </summary>
        ///// <param name="DTO">微信用户信息</param>>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult FriendFollow()
        //{

        //    Loger.Log4Net.Info("[InviteController]******FriendFollow begin->");

        //    try
        //    {
        //        XYAuto.ITSC.Chitunion2017.BLL.WechatInvite.WechatInvite.Instance.FriendFollow();
        //    }
        //    catch (Exception ex)
        //    {
        //        Loger.Log4Net.Info("[InviteController]*****FriendFollow ->WxInviteIdReqDTO:好友关注接口失败:" + ex.Message);
        //    }
        //    Loger.Log4Net.Info("[InviteController]******FriendFollow end->");
        //    return Common.Util.GetJsonDataByResult(null, "Success", 0);
        //}
        /// <summary>
        /// 邀请好友领取红包接口
        /// </summary>
        /// <param name="DTO">微信用户信息</param>>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReceiveRedEves([FromBody] WxInviteIdReqDTO DTO)
        {
            string strJson = XYAuto.ITSC.Chitunion2017.BLL.Json.JsonSerializerBySingleData(DTO);
            Loger.Log4Net.Info("[InviteController]******ReceiveRedEves begin...->WxInviteIdReqDTO:" + strJson + "");
            string messageStr = "";
            decimal RedEvesPrice = 0;
            try
            {
                messageStr = XYAuto.ITSC.Chitunion2017.BLL.WechatInvite.WechatInvite.Instance.ReceiveRedEves(DTO, out RedEvesPrice);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[InviteController]*****ReceiveRedEves ->WxInviteIdReqDTO:" + strJson + ",邀请好友领取红包失败:" + ex.Message);
                throw ex;
            }
            Common.JsonResult jr;
            if (messageStr != "")
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(RedEvesPrice, "Success", 0);
            }
            Loger.Log4Net.Info("[InviteController]******ReceiveRedEves end->");
            return jr;
        }
        /// <summary>
        ///查询邀请好友列表
        /// </summary>
        /// <param name="WxInviteOpenId">邀请人微信用户ID</param>
        /// <param name="RecID">排序ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetBeInvitedList(int TopCount=10, int RecID = 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = XYAuto.ITSC.Chitunion2017.BLL.WechatInvite.WechatInvite.Instance.GetBeInvitedList(TopCount, RecID);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[InviteController]*****GetBeInvitedList ->TopCount:" + TopCount + " ->RecID:" + RecID + ",查询邀请好友列表失败:" + ex.Message);
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
    }
}
