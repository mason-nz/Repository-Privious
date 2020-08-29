using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish
{
    public class PbWeiBoQuery : PublishInfoQueryClient<RequestPublishQueryDto, ResponseWeiBoDto>
    {
        public PbWeiBoQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseWeiBoDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"
                             SELECT  PB.PubID ,
                                    WB.MediaID ,
                                    WB.Number ,
                                    WB.Name ,
                                    WB.HeadIconURL ,
                                    WB.FansCount ,
                                    WB.FansCountURL ,
                                    PB.[Status] ,
                                    PB.PublishStatus ,
                                    PB.BeginTime ,
                                    PB.EndTime ,PB.SaleDiscount,
                        ");
            sbSql.AppendFormat(@"   ( SELECT TOP 1
												PAD.RejectMsg
									  FROM      dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
									  WHERE     PAD.PublishID = PB.PubID
												AND PAD.PubStatus = {0} --驳回
									  ORDER BY  PAD.RecID DESC
							        ) AS RejectMsg", (int)EnumPublishStatus.Reject);
            sbSql.Append(@"         {$}
                                FROM dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                    INNER JOIN dbo.Media_Weibo AS WB WITH ( NOLOCK ) ON WB.MediaID = PB.MediaID AND WB.Status = 0
                                {#}");

            sbSql.AppendLine("  WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.MediaType = {0}", (int)MediaType.WeiBo);

            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder("");
            if (RequetQuery.CreateUserId > 0)//普通角色，只能看见自己的
            {
                sbSql.AppendFormat(@" AND WB.CreateUserID ={0}", RequetQuery.CreateUserId);
            }
            else
            {
                sqlSelect.AppendLine(@" ,DC1.DictName AS Source");
                sqlWhere.AppendFormat(@" LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = WB.Source ");
                if (RequetQuery.Source != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    sbSql.AppendFormat(@" AND DC1.DictId = {0} ", RequetQuery.Source);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Number))
            {
                sbSql.AppendFormat(@" AND WB.Number = '{0}'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Number));
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Name))
            {
                sbSql.AppendFormat(@" AND WB.Name LIKE '{0}%'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Name));
            }

            if (RequetQuery.Status != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.Status = {0} ", RequetQuery.Status);
            }
            if (RequetQuery.PublishStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.PublishStatus ={0} ", RequetQuery.PublishStatus);
            }
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }

            sbSql.AppendLine(@" ) T");
            sbSql.Replace("{#}", sqlWhere.ToString());//追加sql
            sbSql.Replace("{$}", sqlSelect.ToString());//追加 查询字段
            var query = new PublishQuery<ResponseWeiBoDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MediaID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<ResponseWeiBoDto> GetResult(List<ResponseWeiBoDto> resultList, QueryPageBase<ResponseWeiBoDto> query)
        {
            var mediaIdList = resultList.Select(s => s.MediaID).ToList();//分页中的媒体Id集合
            if (mediaIdList.Count == 0)
                return base.GetResult(resultList, query);
            //用媒体Id列表查询对应的详情
            var publishItemInfo = PublishInfoQuery.Instance.QueryPublishItemInfo(new FrontPublishQuery<PublishDetailInfo>()
            {
                MediaId = mediaIdList,
                MediaType = (int)MediaType.WeiBo
            });
            resultList.ForEach(item => { GetAdPositionList(publishItemInfo, item); });

            return base.GetResult(resultList, query);
        }

        private ResponseWeiBoDto GetAdPositionList(List<PublishDetailInfo> publishDetailList,
           ResponseWeiBoDto item)
        {
            var saleDiscount = item.SaleDiscount > 0 ? item.SaleDiscount : 1.0m;
            item.FirstName = new List<AdPositionEntity>();
            item.SecondName = new List<AdPositionEntity>();
            item.ThridName = new List<AdPositionEntity>();
            var groupItemInfo = publishDetailList.Where(s => s.MediaID == item.MediaID);
            var detailInfos = groupItemInfo as PublishDetailInfo[] ?? groupItemInfo.ToArray();
            //找到first 直发
            var firstList = detailInfos.Where(s => s.ADPosition2 == (int)AdFormality4.直发);
            var publishDetailInfos = firstList as PublishDetailInfo[] ?? firstList.ToArray();
            //找到first 直发-硬广
            var firstAdPosition27002 = publishDetailInfos.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.硬广);
            //找到first 直发-软广
            var firstAdPosition27003 = publishDetailInfos.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.软广);

            if (firstAdPosition27002 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27002.ADPosition1Name, Price = Math.Round(firstAdPosition27002.Price * saleDiscount, 2) });
            if (firstAdPosition27003 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27003.ADPosition1Name, Price = Math.Round(firstAdPosition27003.Price * saleDiscount, 2) });

            //找到second 转发
            var secondList = detailInfos.Where(s => s.ADPosition2 == (int)AdFormality4.转发);
            //找到second 转发-硬广
            var secondAdPosition27002 = secondList.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.硬广);
            //找到second 转发-软广
            var secondAdPosition27003 = secondList.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.软广);

            if (secondAdPosition27002 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27002.ADPosition1Name, Price = Math.Round(secondAdPosition27002.Price * saleDiscount, 2) });
            if (secondAdPosition27003 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27003.ADPosition1Name, Price = Math.Round(secondAdPosition27003.Price * saleDiscount, 2) });

            //找到 third 原创发布
            var thirdList = detailInfos.Where(s => s.ADPosition2 == (int)AdFormality4.原创发布);
            //找到second 转发-硬广
            var thirdAdPosition27002 = thirdList.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.硬广);
            //找到second 转发-软广
            var thirdAdPosition27003 = thirdList.FirstOrDefault(s => s.ADPosition1 == (int)AdTypeMapping.软广);

            if (thirdAdPosition27002 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27002.ADPosition1Name, Price = Math.Round(thirdAdPosition27002.Price * saleDiscount, 2) });
            if (thirdAdPosition27003 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27003.ADPosition1Name, Price = Math.Round(thirdAdPosition27003.Price * saleDiscount, 2) });

            return item;
        }
    }
}