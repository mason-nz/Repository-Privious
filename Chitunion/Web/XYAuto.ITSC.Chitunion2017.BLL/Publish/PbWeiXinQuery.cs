using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish
{
    public class PbWeiXinQuery : PublishInfoQueryClient<RequestPublishQueryDto, ResponseWeiXinDto>
    {
        public PbWeiXinQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseWeiXinDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"
                    SELECT  PB.PubID ,
                            WX.MediaID ,
                            WX.Number ,
                            WX.Name ,
                            WX.HeadIconURL ,
                            WX.TwoCodeURL ,
                            WX.FansCount ,
                            WX.FansCountURL ,
                            PB.[Status] ,
                            PB.BeginTime ,
                            PB.EndTime ,PB.SaleDiscount,
                            PB.PublishStatus ,
                            (
								SELECT TOP 1 PAD.RejectMsg FROM dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
								WHERE PAD.PublishID = PB.PubID AND PAD.PubStatus = 15004 --驳回
								ORDER BY PAD.RecID DESC
							) AS RejectMsg
                            {$}
                    FROM    dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Weixin AS WX WITH ( NOLOCK ) ON WX.MediaID = PB.MediaID AND WX.Status = 0
                    {#}
                    WHERE 1=1 ");

            sbSql.AppendFormat(@" AND PB.MediaType = {0}", (int)MediaType.WeiXin);

            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder("");
            if (RequetQuery.CreateUserId > 0)//普通角色，只能看见自己的
            {
                sbSql.AppendFormat(@" AND WX.CreateUserID ={0}", RequetQuery.CreateUserId);
            }
            else
            {
                sqlSelect.AppendLine(@" ,DC1.DictName AS Source");
                sqlWhere.AppendFormat(@" LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = WX.Source ");
                if (RequetQuery.Source > Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    sbSql.AppendFormat(@" AND DC1.DictId = {0} ", RequetQuery.Source);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Number))
            {
                sbSql.AppendFormat(@" AND WX.Number = '{0}'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Number));
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Name))
            {
                sbSql.AppendFormat(@" AND WX.Name LIKE '{0}%'", XYAuto.Utils.StringHelper.SqlFilter(RequetQuery.Name));
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
            var query = new PublishQuery<ResponseWeiXinDto>()
            {
                OrderBy = " MediaID DESC ",
                StrSql = sbSql.ToString(),
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<ResponseWeiXinDto> GetResult(List<ResponseWeiXinDto> resultList, QueryPageBase<ResponseWeiXinDto> query)
        {
            var mediaIdList = resultList.Select(s => s.MediaID).ToList();//分页中的媒体Id集合
            if (mediaIdList.Count == 0)
                return base.GetResult(resultList, query);
            //用媒体Id列表查询对应的详情
            var publishItemInfo = PublishInfoQuery.Instance.QueryPublishItemInfo(new FrontPublishQuery<PublishDetailInfo>()
            {
                MediaId = mediaIdList,
                MediaType = (int)MediaType.WeiXin
            });
            resultList.ForEach(item => { GetAdPositionList(publishItemInfo, item); });

            return base.GetResult(resultList, query);
        }

        public ResponseWeiXinDto GetAdPositionList(List<PublishDetailInfo> publishDetailList, ResponseWeiXinDto item)
        {
            var saleDiscount = item.SaleDiscount > 0 ? item.SaleDiscount : 1.0m;
            item.FirstName = new List<AdPositionEntity>();
            item.SecondName = new List<AdPositionEntity>();
            item.ThridName = new List<AdPositionEntity>();
            item.FourthName = new List<AdPositionEntity>();
            var yuanChuan = "原创";
            var groupItemInfo = publishDetailList.Where(s => s.MediaID == item.MediaID);
            //找到first 单图文
            var detailInfos = groupItemInfo as PublishDetailInfo[] ?? groupItemInfo.ToArray();
            var firstList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.单图文);
            //找到first 单图文-硬广
            var publishDetailInfos = firstList as PublishDetailInfo[] ?? firstList.ToArray();
            //找到first 单图文-硬广-发布
            var firstAdPosition27002 = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (firstAdPosition27002 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27002.ADPosition2Name + firstAdPosition27002.ADPosition3Name, Price = Math.Round(firstAdPosition27002.Price * saleDiscount, 2) });
            //找到first 单图文-硬广-原创
            var firstAdPosition27002Yc = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (firstAdPosition27002Yc != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27002Yc.ADPosition2Name + yuanChuan, Price = Math.Round(firstAdPosition27002Yc.Price * saleDiscount, 2) });

            //找到first 单图文-软广-发布
            var firstAdPosition27003 = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (firstAdPosition27003 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27003.ADPosition2Name + firstAdPosition27003.ADPosition3Name, Price = Math.Round(firstAdPosition27003.Price * saleDiscount, 2) });
            //找到first 单图文-软广-原创
            var firstAdPosition27003Yc = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (firstAdPosition27003Yc != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27003Yc.ADPosition2Name + yuanChuan, Price = Math.Round(firstAdPosition27003Yc.Price * saleDiscount, 2) });

            //找到second 多图文头条
            var secondList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.多图文头条);
            //找到second 多图文头条-硬广-发布
            var secondAdPosition27002 = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (secondAdPosition27002 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27002.ADPosition2Name + secondAdPosition27002.ADPosition3Name, Price = Math.Round(secondAdPosition27002.Price * saleDiscount, 2) });
            //找到second 多图文头条-硬广-原创
            var secondAdPosition27002Yc = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (secondAdPosition27002Yc != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27002Yc.ADPosition2Name + yuanChuan, Price = Math.Round(secondAdPosition27002Yc.Price * saleDiscount, 2) });

            //找到second 多图文头条-软广
            var secondAdPosition27003 = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (secondAdPosition27003 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27003.ADPosition2Name + secondAdPosition27003.ADPosition3Name, Price = Math.Round(secondAdPosition27003.Price * saleDiscount, 2) });
            //找到second 多图文头条-原创
            var secondAdPosition27003Yc = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (secondAdPosition27003Yc != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27003Yc.ADPosition2Name + yuanChuan, Price = Math.Round(secondAdPosition27003Yc.Price * saleDiscount, 2) });

            //找到third 多图文第二条
            var thirdList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.多图文第二条);
            //找到third 多图文第二条-硬广-发布
            var thirdAdPosition27002 = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (thirdAdPosition27002 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27002.ADPosition2Name + thirdAdPosition27002.ADPosition3Name, Price = Math.Round(thirdAdPosition27002.Price * saleDiscount, 2) });
            //找到third 多图文第二条-硬广-原创
            var thirdAdPosition27002Yc = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (thirdAdPosition27002Yc != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27002Yc.ADPosition2Name + yuanChuan, Price = Math.Round(thirdAdPosition27002Yc.Price * saleDiscount, 2) });

            //找到third 多图文第二条-软广-发布
            var thirdAdPosition27003 = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (thirdAdPosition27003 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27003.ADPosition2Name + thirdAdPosition27003.ADPosition3Name, Price = Math.Round(thirdAdPosition27003.Price * saleDiscount, 2) });
            //找到third 多图文第二条-软广-原创
            var thirdAdPosition27003Yc = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (thirdAdPosition27003Yc != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27003Yc.ADPosition2Name + yuanChuan, Price = Math.Round(thirdAdPosition27003Yc.Price * saleDiscount, 2) });

            //找到 fourth 多图文第3-N条
            var fourthList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.多图文3N条);
            //找到third 多图文第二条-硬广-发布
            var fourthAdPosition27002 = fourthList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (fourthAdPosition27002 != null)
                item.FourthName.Add(new AdPositionEntity() { AdName = fourthAdPosition27002.ADPosition2Name + fourthAdPosition27002.ADPosition3Name, Price = Math.Round(fourthAdPosition27002.Price * saleDiscount, 2) });
            //找到third 多图文第二条-硬广-原创
            var fourthAdPosition27002Yc = fourthList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (fourthAdPosition27002Yc != null)
                item.FourthName.Add(new AdPositionEntity() { AdName = fourthAdPosition27002Yc.ADPosition2Name + yuanChuan, Price = Math.Round(fourthAdPosition27002Yc.Price * saleDiscount, 2) });
            //找到third 多图文第二条-软广
            var fourthAdPosition27003 = fourthList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.发布);
            if (fourthAdPosition27003 != null)
                item.FourthName.Add(new AdPositionEntity() { AdName = fourthAdPosition27003.ADPosition2Name + fourthAdPosition27003.ADPosition3Name, Price = Math.Round(fourthAdPosition27003.Price * saleDiscount, 2) });
            //找到third 多图文第二条-原创
            var fourthAdPosition27003Yc = fourthList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广 && s.ADPosition3 == (int)AdFormality3.原创发布);
            if (fourthAdPosition27003Yc != null)
                item.FourthName.Add(new AdPositionEntity() { AdName = fourthAdPosition27003Yc.ADPosition2Name + yuanChuan, Price = Math.Round(fourthAdPosition27003Yc.Price * saleDiscount, 2) });

            return item;
        }
    }
}