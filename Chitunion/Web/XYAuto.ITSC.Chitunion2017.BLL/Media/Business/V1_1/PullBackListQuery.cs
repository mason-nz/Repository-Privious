using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    public class PullBackListQuery : PublishInfoQueryClient<ReqCollectPullBackQueryDto, ResponsePullBackDto>
    {
        public PullBackListQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<ResponsePullBackDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@"
                        SELECT  WO.WxNumber AS Number ,
                                WO.NickName AS Name ,
                                WO.FansCount ,
                                WO.HeadImg AS HeadIconURL ,
                                MW.MediaID ,
                                MW.Status,
                                MW.PublishStatus --上下架状态
                        FROM    dbo.Media_CollectionBlacklist AS MCB WITH ( NOLOCK )
                                INNER JOIN dbo.Media_Weixin AS MW WITH ( NOLOCK ) ON MW.MediaID = MCB.MediaID AND MW.Status = 0
                                INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = MW.WxID AND WO.Status = 0
						WHERE MCB.MediaType = {0} AND MCB.RelationType = {1} AND MCB.Status = {2}",
                        (int)MediaType.WeiXin, (int)CollectPullBackTypeEnum.PullBack, (int)DataStatusEnum.Active);

            if (RequetQuery.CreateUserId > 0)
            {
                sbSql.AppendFormat(" AND MCB.CreateUserID = {0}", RequetQuery.CreateUserId);
            }

            sbSql.AppendLine(@" ) T");

            var query = new PublishQuery<ResponsePullBackDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MediaID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}