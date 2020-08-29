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
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class SmartSearchController : BaseApiController
    {
        /// <summary>
        /// 获取智能搜索详情页媒体列表
        /// </summary>
        /// <param name="SmartSearchID"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetSmartSearchMediaList([FromUri]int SmartSearchID)
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -1, Message = "没有权限" };
            }
            var CartInfo = SmartSearchBll.Instance.GetSmartSearchMediaList(SmartSearchID);

            return Util.GetJsonDataByResult(CartInfo);
        }
        /// <summary>
        /// 获取智能搜索列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetSmartSearchList([FromUri]QueryArgs query)
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -1, Message = "没有权限" };
            }
            var CartInfo = SmartSearchBll.Instance.GetSmartSearchList(query, GetUserInfo.UserID);

            return Util.GetJsonDataByResult(CartInfo);
        }
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult AddSmartSearchInfo([FromBody]SmartSearchModel query)
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -1, Message = "没有权限" };
            }
            var CartInfo = SmartSearchBll.Instance.AddSmartSearchInfo(query);
            if (CartInfo <= 0)
            {
                return new JsonResult { Status = -1, Message = "添加失败" };
            }

            dynamic objectDynamic = new ExpandoObject();
            objectDynamic.ReturnID = CartInfo;

            return Util.GetJsonDataByResult(objectDynamic);
        }
        /// <summary>
        /// 根据列表ID获取详情
        /// </summary>
        /// <param name="RecID"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetSmartSearchDetailInfo([FromUri]int RecID)
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -1, Message = "没有权限" };
            }
            var DetailInfo = SmartSearchBll.Instance.GetSmartSearchDetailInfo(RecID);

            return Util.GetJsonDataByResult(DetailInfo);
        }
        /// <summary>
        /// 根据列表ID获取详情
        /// </summary>
        /// <param name="RecID"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetSmartSearchCount()
        {
            if (GetUserInfo.Category != (int)UserCategoryEnum.广告主)
            {
                return new JsonResult { Status = -1, Message = "没有权限" };
            }


            var CountNum = SmartSearchBll.Instance.GetSmartSearchCount(GetUserInfo.UserID);
            dynamic objectDynamic = new ExpandoObject();
            objectDynamic.PromotionCount = CountNum;

            return Util.GetJsonDataByResult(objectDynamic);
        }
        
    }
}
