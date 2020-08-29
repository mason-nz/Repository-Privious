using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Ranking;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    /// <summary>
    /// V1.1.7 榜单，玛丽相关
    /// Auth:lixiong
    /// </summary>
    [CrossSite]
    public class RankingController : ApiController
    {
        /// <summary>
        /// 榜单查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Query([FromUri] RankingQueryDto requestDto)
        {
            var jsonResult = new JsonResult() { Status = 0, Message = "success" };
            if (requestDto == null)
            {
                requestDto = new RankingQueryDto()
                {
                    PageSize = 100
                };
            }

            var userId = ConfigurationUtil.GetAppSettingValue("RankListQueryByUserId").ToInt();

            if (userId <= 0)
            {
                jsonResult.Message = "请配置指定用户id";
                return jsonResult;
            }
            var tup = BLL.Ranking.StatWeixinRankList.Instance.GetRankList(new RankingQuery<RespStatWeixinRankListDto>()
            {
                CategoryId = requestDto.CategoryId,
                CreateUserId = userId,
                KeyWord = requestDto.Keyword,
                PageSize = requestDto.PageSize
            });
            jsonResult.Result = new
            {
                List = tup.Item1,
                Info = tup.Item2
            };
            return jsonResult;
        }

        /// <summary>
        /// 榜单分类查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult CategoryList()
        {
            var jsonResult = new JsonResult() { Status = 0, Message = "success" };
            var userId = ConfigurationUtil.GetAppSettingValue("RankListQueryByUserId").ToInt();

            if (userId <= 0)
            {
                jsonResult.Message = "请配置指定用户id";
                return jsonResult;
            }
            jsonResult.Result = BLL.Ranking.StatWeixinRankList.Instance.GetRankingCategoryList(userId);
            return jsonResult;
        }
    }
}