using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using JsonResult = XYAuto.ITSC.Chitunion2017.WebAPI.Common.JsonResult;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    /// <summary>
    /// 推荐到首页相关
    /// Auth:lixiong
    /// </summary>
    [CrossSite]
    public class RecommendController : ApiController
    {
        /// <summary>
        /// 推荐到首页查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Query([FromUri] RecommendSearchDto requestDto)
        {
            if (requestDto != null)
            {
                if (requestDto.BusinessType == (int)MediaType.APP)
                {
                    requestDto.CategoryId = 1;//大于0就可以、app没有分类
                }
            }
            var jsonResult = Util.Verify<RecommendSearchDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new RecommendQueryProxy(requestDto).GetQuery();
            return jsonResult;
        }

        /// <summary>
        /// 添加到推荐首页操作
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Add([FromBody] AddRecommendDto requestDto)
        {
            var jsonResult = new JsonResult();
            if (requestDto == null)
                return jsonResult;
            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID();
            var retValue = new RecommendQueryProxy(requestDto, null).AddToRecommend();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        /// <summary>
        /// 编辑信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Update([FromBody] UpdateRecommendDto requestDto)
        {
            var jsonResult = new JsonResult();
            var retValue = new RecommendQueryProxy(null, requestDto).UpdateRecommend();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult DeleteInfo([FromBody]DeleteRecommendDto requestDto)
        {
            var jsonResult = Util.Verify<DeleteRecommendDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var retValue = new RecommendQueryProxy(null, null).DeleteRecommend(requestDto.RecId);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = jsonResult.Message ?? "删除成功";
            return jsonResult;
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Publish([FromBody]PublishRecommendDto requestDto)
        {
            var jsonResult = Util.Verify<PublishRecommendDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var retValue = new RecommendQueryProxy(null, null).UpdatePublishState(requestDto.BusinessType);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = jsonResult.Message ?? "发布成功";
            return jsonResult;
        }
    }
}