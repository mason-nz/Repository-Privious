using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Common;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Filter;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Controllers
{
    [CrossSite]
    public class MediaUserController : ApiController
    {
        /// <summary>
        /// 批量启用、禁用SYS008BUT200102
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS008BUT200102")]
        public JsonResult UserEnableOrDisable([FromBody]UserBatchQueryArgs queryArgs)
        {
            var ResultNum = MediaUserBll.Instance.UserEnableOrDisable(queryArgs);
            return Util.GetJsonDataByResult(null, ResultNum <= 0 ? "操作失败" : "OK", ResultNum <= 0 ? -1 : 0);

        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaUserList([FromUri]UserQueryArgs queryArgs)
        {
            var resultList = MediaUserBll.Instance.GetMediaUserList(queryArgs);
            return Util.GetJsonDataByResult(resultList);

        }
        /// <summary>
        /// 根据层级获取推广列表
        /// </summary>
        /// <param name="Level"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetPromotionChannelList([FromUri]int Level)
        {
            var resultList = MediaUserBll.Instance.GetPromotionChannelList(Level);
            return Util.GetJsonDataByResult(resultList);
        }

        /// <summary>
        /// 批量更新密码	
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS008BUT200103")]
        public JsonResult UserResetPassword([FromBody]UserBatchQueryArgs queryArgs)
        {
            var ResultNum = MediaUserBll.Instance.UserResetPassword(queryArgs);
            return Util.GetJsonDataByResult(null, ResultNum <= 0 ? "操作失败" : "OK", ResultNum <= 0 ? -1 : 0);

        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaUserDetailInfo([FromUri]int UserID)
        {
            var query = MediaUserBll.Instance.GetMediaUserDetailInfo(UserID);

            return Util.GetJsonDataByResult(query);

        }
        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS008BUT200101")]
        public JsonResult UserCertificationAudit([FromBody]UserBatchQueryArgs queryArgs)
        {
            var ResultNum = MediaUserBll.Instance.UserCertificationAudit(queryArgs);
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"返回ID：{ResultNum}");
            if (ResultNum > 0)
            {
                MediaUserBll.Instance.WeiXinAudit(queryArgs);
            }
            return Util.GetJsonDataByResult(null, ResultNum <= 0 ? "操作失败" : "OK", ResultNum <= 0 ? -1 : 0);

        }
    }
}
