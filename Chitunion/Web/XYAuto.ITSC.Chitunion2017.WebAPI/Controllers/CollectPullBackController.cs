using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    public class CollectPullBackController : ApiController
    {
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Add([FromBody] AddToCollectPullBackDto requestDto)
        {
            var jsonResult = new JsonResult();
            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID();
            var retValue = new CollectPullBackProxy(requestDto).AddToExcute();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Query([FromUri]ReqCollectPullBackQueryDto requestDto)
        {
            var jsonResult = new JsonResult() { Message = "请输入合法的查询类型", Status = 5001 };
            if (!Enum.IsDefined(typeof(CollectPullBackTypeEnum), requestDto.OperateType))
            {
                return jsonResult;
            }
            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID();
            var config = new ConfigEntity();
            if (requestDto.OperateType == (int)CollectPullBackTypeEnum.Collection)
            {
                jsonResult.Result = new CollectionListQuery(config).GetQueryList(requestDto);
            }
            else
            {
                jsonResult.Result = new PullBackListQuery(config).GetQueryList(requestDto);
            }
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            return jsonResult;
        }

        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Remove([FromBody] RemoveCollectPullBackDto requestDto)
        {
            var jsonResult = Util.Verify<RemoveCollectPullBackDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            //requestDto.OperateType = (int)DataStatusEnum.Delete;//移除
            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID();
            var retValue = new CollectPullBackProxy(requestDto).RemoveExcute();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }
    }
}