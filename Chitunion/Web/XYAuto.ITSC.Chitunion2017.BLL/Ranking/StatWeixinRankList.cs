/********************************************************
*创建人：lixiong
*创建时间：2017/7/10 16:51:23
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Ranking;

namespace XYAuto.ITSC.Chitunion2017.BLL.Ranking
{
    public class StatWeixinRankList
    {
        #region Instance

        public static readonly StatWeixinRankList Instance = new StatWeixinRankList();

        #endregion Instance

        public Tuple<List<RespStatWeixinRankListDto>, RespStatWeixinRankItemDto> GetRankList(RankingQuery<RespStatWeixinRankListDto> query)
        {
            query.PageSize = query.PageSize > 100 ? 100 : query.PageSize;

            return Dal.Ranking.StatWeixinRankList.Instance.GetRankList(query);
        }

        public List<CommonlyClassDto> GetRankingCategoryList(int userId)
        {
            return Dal.Ranking.StatWeixinRankList.Instance.GetRankingCategoryList(userId);
        }

        public List<CommonlyClassDto> GetCategoryList(int mediaType)
        {
            return Dal.Ranking.StatWeixinRankList.Instance.GetCategoryList(mediaType);
        }
    }
}