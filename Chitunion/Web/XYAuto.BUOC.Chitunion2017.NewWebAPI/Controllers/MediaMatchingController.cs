using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    public class MediaMatchingController : BaseApiController
    {
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        [ApiLog]
        [HttpGet]
        /// <summary>
        /// 智能匹配列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public JsonResult GetMediaMatchingList([FromUri] MediaQueryArgs queryArgs)
        {
            var queryResult = MediaBll.Instance.GetMediaMatchingList(queryArgs);

            return Util.GetJsonDataByResult(queryResult);
        }

        /// <summary>
        /// 购物车列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [HttpGet]
        [ApiLog]
        public JsonResult GetCartInfoList()
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -200, Message = "权限不足" };
            }
            var queryResult = CartBll.Instance.GetCartInfoList(GetUserInfo.UserID);

            return Util.GetJsonDataByResult(queryResult);
        }
        /// <summary>
        /// 购物车删除
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [HttpPost]
        [ApiLog]
        public JsonResult DelCartInfo([FromBody]DeleteCartInfoDto queryArgs)
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -200, Message = "权限不足" };
            }
            var queryResult = CartBll.Instance.DeleteCartInfo(queryArgs);

            return Util.GetJsonDataByResult(null, queryResult <= 0 ? "删除失败" : "OK", queryResult <= 0 ? -1 : 0);
        }

        /// <summary>
        /// 购物车批量添加
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [HttpPost]
        [ApiLog]
        public JsonResult AddCartInfo([FromBody]ReqCartInfoDto queryArgs)
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -200, Message = "权限不足" };
            }
            //获取当前用户购物车数量
            int cartCount = CartBll.Instance.GetCartCount(GetUserInfo.UserID);
            if ((cartCount + queryArgs.CartInfoList.Count) > 300)
            {
                return Util.GetJsonDataByResult(null, "购物车最多只能添加300", -200);

            }
            var queryResult = CartBll.Instance.AddCartInfo(queryArgs.CartInfoList, GetUserInfo.UserID);

            return Util.GetJsonDataByResult(null);
        }
        /// <summary>
        /// 获取当前购物车数量
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [HttpGet]
        [ApiLog]
        public JsonResult GetCartCount()
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -200, Message = "权限不足" };
            }
            //获取当前用户购物车数量
            int cartCount = CartBll.Instance.GetCartCount(GetUserInfo.UserID);

            dynamic objectDynamic = new ExpandoObject();
            objectDynamic.CartCount = cartCount;

            return Util.GetJsonDataByResult(objectDynamic);
        }
        /// <summary>
        /// 获取购物车媒体列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [HttpGet]
        [ApiLog]
        public JsonResult GetCartMediaList()
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -1, Message = "权限不足" };
            }
            var CartInfo = CartBll.Instance.GetCartMediaList(GetUserInfo.UserID);

            return Util.GetJsonDataByResult(CartInfo);
        }
        [HttpGet]
        [ApiLog]
        public JsonResult GetTaskListInfo([FromUri]QueryTaskargs queryArgs)
        {
            var query = CartBll.Instance.TaskListInfo(queryArgs);
            return Util.GetJsonDataByResult(query);
        }
    }
}
