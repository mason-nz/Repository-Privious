using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.Common;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Service.Wechat;
using XYAuto.ChiTu2018.Service.Wechat.Dto;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 注释：邀请好友控制器
    /// 作者：zhanglb
    /// 日期：2018/5/14 19:57:02
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class InviteController : ApiController
    {
        /// <summary>
        /// 邀请好友领取红包接口
        /// </summary>
        /// <param name="dto">实体类</param>>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult ReceiveRedEves([FromBody] ReqInviteRecIdDto dto)
        {

            string messageStr;
            decimal redEvesPrice;
            try
            {
                messageStr = LeInviteRecordService.Instance.ReceiveRedEves(dto, out redEvesPrice);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[InviteController]*****ReceiveRedEves ->WxInviteIdReqDTO:" + JsonConvert.SerializeObject(dto) + ",邀请好友领取红包失败", ex);
                throw;
            }
            var jr = messageStr != "" ? Util.GetJsonDataByResult(null, messageStr, -1) : Util.GetJsonDataByResult(redEvesPrice);
            return jr;
        }

        /// <summary>
        /// 查询邀请好友列表
        /// </summary>
        /// <param name="topCount">查询条数</param>
        /// <param name="recId">主键ID</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBeInvitedList(int topCount = 10, int recId = 0)
        {
            Dictionary<string, object> dic;
            try
            {
                dic = LeInviteRecordService.Instance.GetBeInvitedList(topCount, recId);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[InviteController]*****GetBeInvitedList ->TopCount:" + topCount + " ->RecID:" + recId + ",查询邀请好友列表失败", ex);
                throw;
            }
            return Util.GetJsonDataByResult(dic);
        }

    }
}
