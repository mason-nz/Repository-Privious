using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage
{
    public class FpWeiXinQuery : PublishInfoQueryClient<RequestFrontPublishQueryDto, ResponseFrontWeiXinDto>
    {
        public FpWeiXinQuery(ConfigEntity configEntity)
            : base(configEntity)
        {
        }

        protected override PublishQuery<ResponseFrontWeiXinDto> GetQueryParams()
        {
            #region sql

            var sbSql = new StringBuilder("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@" SELECT  A.PubID,WX.MediaID ,
                            WX.Number ,
                            WX.Name ,
                            WX.HeadIconURL ,
                            WX.TwoCodeURL ,
                            WX.FansCount ,
                            WX.FansCountURL ,
                            WX.[Status] ,WX.IsAuth,
                            A.PublishStatus,A.SaleDiscount,
                            IWX.UpdateCount,IWX.ReferReadCount,
							(
								SELECT TOP 1 ISNULL(PD.Price,0)
								FROM dbo.Publish_DetailInfo AS PD WITH(NOLOCK) WHERE PD.MediaType ={0} AND WX.MediaID = PD.MediaID
								AND PD.ADPosition1 = {1}--原创+发布
							) AS Price--如果有价格就代表可原创
                                $$
                    FROM  dbo.Publish_BasicInfo AS A WITH (NOLOCK)
                            INNER JOIN dbo.Media_Weixin AS WX WITH (NOLOCK) ON WX.MediaID = A.MediaID AND WX.Status = 0
                            LEFT JOIN dbo.Interaction_Weixin AS IWX WITH (NOLOCK) ON IWX.MediaID = A.MediaID
                                ##
                WHERE 1=1 ", (int)MediaType.WeiXin, (int)AdFormality4.原创发布);

            sbSql.AppendFormat(@" AND A.MediaType = {0} AND A.PublishStatus = {1}  AND
                        GETDATE() BETWEEN A.BeginTime AND A.EndTime", (int)MediaType.WeiXin, (int)EnumPublishStatus.OnSold);
            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder("");
            if (RequetQuery.CategoryID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WX.CategoryID = {0}", RequetQuery.CategoryID);
            }

            if (RequetQuery.SearchMediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WX.MediaID = {0}", RequetQuery.SearchMediaId);
                RequetQuery.MediaName = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))//
            {
                sbSql.AppendFormat(" AND (WX.Number LIKE '%{0}%' OR WX.Name LIKE '%{0}%')", Utils.StringHelper.SqlFilter(RequetQuery.MediaName));
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.FansCount) && !string.IsNullOrWhiteSpace(RequetQuery.FansCount.Replace("-", "")))
            {
                var spFansCount = RequetQuery.FansCount.Split('-');
                if (spFansCount[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND WX.FansCount >= {0}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND WX.FansCount >= {0} AND WX.FansCount < {1}", spFansCount[0].ToInt() * RequetQuery.FansCountUnit, spFansCount[1].ToInt() * RequetQuery.FansCountUnit);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Price) && !string.IsNullOrWhiteSpace(RequetQuery.Price.Replace("-", "")))
            {
                var spPrice = RequetQuery.Price.Split('-');

                sbSql.Append(" AND WX.MediaID IN ( ");
                sbSql.Append(" SELECT MediaID FROM DBO.Publish_DetailInfo AS PD WITH(NOLOCK) WHERE PD.MediaType = A.MediaType");
                if (spPrice[1].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    //代表是最大，没上限
                    sbSql.AppendFormat(@" AND PD.Price * A.SaleDiscount >= {0}", spPrice[0].ToInt() * RequetQuery.PriceUnit);
                }
                else
                {
                    sbSql.AppendFormat(@" AND PD.Price * A.SaleDiscount >= {0} AND PD.Price * A.SaleDiscount < {1}", spPrice[0].ToInt() * RequetQuery.PriceUnit, spPrice[1].ToInt() * RequetQuery.PriceUnit);
                }
                sbSql.Append(" ) ");
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.CoverageArea) && !string.IsNullOrWhiteSpace(RequetQuery.CoverageArea.Replace("-", "")))
            {
                sbSql.Append(@"
                        AND EXISTS
				        (
					        SELECT MAM.MediaID FROM  dbo.Media_Area_Mapping AS MAM WITH(NOLOCK)
					        WHERE MAM.MediaID = A.MediaID AND MAM.MediaType = A.MediaType
					    ");
                var spCoverageArea = RequetQuery.CoverageArea.Split('-');
                sbSql.AppendFormat(@" AND MAM.ProvinceID = {0} ", spCoverageArea[0]);
                if (!CityMapping.IsMunicipality(spCoverageArea[0].ToInt()))//直辖市不用查CityID
                {
                    if (!string.IsNullOrWhiteSpace(spCoverageArea[1]) && spCoverageArea[1].ToInt() > 0)
                        sbSql.AppendFormat(@" AND MAM.CityID = {0}", spCoverageArea[1]);
                }
                sbSql.Append(@" ) ");
            }

            if (RequetQuery.IsAuth != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WX.IsAuth = {0}", RequetQuery.IsAuth);
            }
            if (RequetQuery.LevelType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WX.LevelType = {0}", RequetQuery.LevelType);
            }
            if (RequetQuery.OrderRemark != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND WX.OrderRemark LIKE '{0}%'", RequetQuery.OrderRemark);
            }
            if (RequetQuery.FansSex != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                if (RequetQuery.FansSex == 0)
                {
                    sbSql.AppendFormat(@" AND WX.FansMalePer > 50.00");
                }
                if (RequetQuery.FansSex == 1)
                    sbSql.AppendFormat(@" AND WX.FansFemalePer >50.00");
            }

            var orderByReference = string.Empty;//如果没有参考报价排序 优先级大于普通排序

            //参考报价排序
            if (RequetQuery.OrderByReference != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                var dic = new Dictionary<int, string>()
                {
                    { 9001,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},ASC",(int)AdTypeMapping.硬广,(int)AdFormality3.发布)},//硬广发布asc
                    { 9002,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},DESC",(int)AdTypeMapping.硬广,(int)AdFormality3.发布)},//硬广发布desc
                    { 9003,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},ASC",(int)AdTypeMapping.硬广,(int)AdFormality3.原创发布)},//硬广原创asc
                    { 9004,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},DESC",(int)AdTypeMapping.硬广,(int)AdFormality3.原创发布)},//硬广原创desc
                    { 9005,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},ASC",(int)AdTypeMapping.软广,(int)AdFormality3.发布)},//软广发布asc
                    { 9006,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},DESC",(int)AdTypeMapping.软广,(int)AdFormality3.发布)},//软广发布desc
                    { 9007,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},ASC",(int)AdTypeMapping.软广,(int)AdFormality3.原创发布)},//软广原创asc
                    { 9008,string.Format(" AND PD.ADPosition2={0} AND PD.ADPosition3={1},DESC",(int)AdTypeMapping.软广,(int)AdFormality3.原创发布)},//软广原创desc
                };
                var value = dic.FirstOrDefault(s => s.Key == RequetQuery.OrderByReference);

                if (!string.IsNullOrWhiteSpace(value.Value))
                {
                    var sp = value.Value.Split(',');
                    var ascDescOrderBy = sp[1];
                    sqlSelect.AppendFormat(@",(
			                    SELECT TOP 1 PD.Price FROM DBO.Publish_DetailInfo AS PD WITH(NOLOCK)
			                    WHERE PD.MediaID =WX.MediaID AND PD.MediaType = A.MediaType {0}
			                    ORDER BY PD.Price {1}
		                    ) AS PriceOrder", sp[0], ascDescOrderBy);
                    orderByReference = " PriceOrder " + ascDescOrderBy;
                }
            }

            #endregion sql

            sbSql.AppendLine(@" ) T");

            sbSql.Replace("##", sqlWhere.ToString());
            sbSql.Replace("$$", sqlSelect.ToString());
            var query = new PublishQuery<ResponseFrontWeiXinDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = string.IsNullOrWhiteSpace(orderByReference) ? GetOrderBy() : orderByReference,//默认按照入库时间倒序排列（待确定是媒体时间还是刊例时间）
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        private string GetOrderBy()
        {
            var orderByStr = " MediaID DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount DESC"},
                {1002," FansCount ASC"},
                {2001," ReferReadCount DESC"},
                {2002," ReferReadCount ASC"},
                {3001," UpdateCount DESC"},
                {3002," UpdateCount ASC"}
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == RequetQuery.OrderBy);
            return value.Value ?? orderByStr;
        }

        protected override BaseResponseEntity<ResponseFrontWeiXinDto> GetResult(List<ResponseFrontWeiXinDto> resultList, QueryPageBase<ResponseFrontWeiXinDto> query)
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

        private ResponseFrontWeiXinDto GetAdPositionList(List<PublishDetailInfo> publishDetailList, ResponseFrontWeiXinDto item)
        {
            var saleDiscount = item.SaleDiscount > 0 ? item.SaleDiscount : 1.0m;
            item.FirstName = new List<AdPositionEntity>();
            item.SecondName = new List<AdPositionEntity>();
            item.ThridName = new List<AdPositionEntity>();
            var groupItemInfo = publishDetailList.Where(s => s.MediaID == item.MediaID);
            //找到first 单图文
            var detailInfos = groupItemInfo as PublishDetailInfo[] ?? groupItemInfo.ToArray();
            var firstList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.单图文);
            //找到first 单图文-硬广
            var publishDetailInfos = firstList as PublishDetailInfo[] ?? firstList.ToArray();
            var firstAdPosition27002 = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广);
            //找到first 单图文-软广
            var firstAdPosition27003 = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广);
            if (firstAdPosition27002 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27002.ADPosition2Name, Price = Math.Round(firstAdPosition27002.Price * saleDiscount, 2) });
            if (firstAdPosition27003 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27003.ADPosition2Name, Price = Math.Round(firstAdPosition27003.Price * saleDiscount, 2) });

            //找到second 多图文头条
            var secondList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.多图文头条);
            //找到second 多图文头条-硬广
            var secondAdPosition27002 = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广);
            //找到second 多图文头条-软广
            var secondAdPosition27003 = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广);

            if (secondAdPosition27002 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27002.ADPosition2Name, Price = Math.Round(secondAdPosition27002.Price * saleDiscount, 2) });
            if (secondAdPosition27003 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27003.ADPosition2Name, Price = Math.Round(secondAdPosition27003.Price * saleDiscount, 2) });

            //找到third 多图文第二条
            var thirdList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.多图文第二条);
            //找到third 多图文第二条-硬广
            var thirdAdPosition27002 = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广);
            //找到third 多图文第二条-软广
            var thirdAdPosition27003 = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广);

            if (thirdAdPosition27002 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27002.ADPosition2Name, Price = Math.Round(thirdAdPosition27002.Price * saleDiscount, 2) });
            if (thirdAdPosition27003 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27003.ADPosition2Name, Price = Math.Round(thirdAdPosition27003.Price * saleDiscount, 2) });

            return item;
        }
    }
}