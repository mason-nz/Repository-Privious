/********************************************************
*创建人：lixiong
*创建时间：2017/5/12 11:43:11
*说明：广告审核（驳回、待审核）列表查询，分角色、AE只能看见自己的
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1
{
    public class AdWeiXinNotPassQuery : PublishInfoQueryClient<RequestAdQueryDto, RespAdWeiXinAuditPassDto>
    {
        public AdWeiXinNotPassQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespAdWeiXinAuditPassDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.AppendFormat(@"
                    SELECT  PB.PubID ,
                            PB.PubName ,
                            PB.BeginTime ,--有效期
                            PB.EndTime ,
                            PB.Wx_Status ,
                            PB.CreateTime ,
                            PB.IsAppointment ,
                            PB.MediaType ,
                            WX.MediaID ,
		                    WX.ADName,
                            WO.RecID AS WxId,
                            WX.Number ,
                            WX.Name ,
                            WX.HeadIconURL ,
                            UI.UserName,UI.Mobile,
                            VU.SysName AS SubmitUser,
                            WX.[Source] ,
                            WO.IsAreaMedia,
		                    --审核人+审核时间
                            AuditUser = ( SELECT TOP 1
                                                    UDI.TrueName + '|'+CONVERT(VARCHAR(20), PAD.CreateTime, 120)
                                          FROM      dbo.PublishAuditInfo AS PAD WITH ( NOLOCK )
								                    INNER JOIN DBO.UserDetailInfo UDI WITH(NOLOCK) ON UDI.UserID =  PAD.CreateUserID
                                          WHERE     PAD.PublishID = PB.PubID
                                                    AND PAD.PubStatus = {0} --审核操作
                                          ORDER BY  PAD.RecID DESC
                                        ) ,
                            ReferencePrice = (
			                    --参考价
                                               SELECT   CAST(MIN(PD.SalePrice) AS VARCHAR(100)) + ','
                                                        + CAST(MAX(PD.SalePrice) AS VARCHAR(100))
                                               FROM     dbo.Publish_DetailInfo AS PD WITH ( NOLOCK )
                                               WHERE    PD.MediaType = PD.MediaType
                                                        AND PB.PubID = PD.PubID
                                             ) ,
                            DC1.DictName AS Wx_StatusName
                    FROM    dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Weixin AS WX WITH ( NOLOCK ) ON WX.MediaID = PB.MediaID AND WX.Status = 0
                            INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = WX.WxID AND WO.Status = 0
                            LEFT JOIN dbo.UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = PB.CreateUserID
                            --LEFT JOIN dbo.UserDetailInfo AS UDI WITH ( NOLOCK ) ON UDI.UserID = PB.CreateUserID
                            LEFT JOIN DBO.v_UserInfo AS VU WITH(NOLOCK) ON  PB.CreateUserID = VU.UserID
                            LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = PB.Wx_Status
                    ", RequetQuery.Wx_Status.Trim(',').ToSqlFilter());
            sbSql.AppendLine("  WHERE 1=1");
            sbSql.AppendFormat(@" AND PB.IsDel = 0 AND PB.MediaType = {0}", (int)MediaType.WeiXin);
            var sqlWhere = new StringBuilder();
            var sqlSelect = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(RequetQuery.Wx_Status))
            {
                sbSql.AppendFormat(@"  AND PB.Wx_Status IN ({0})", RequetQuery.Wx_Status.Trim(',').ToSqlFilter());
            }
            if (RequetQuery.IsAreaMedia != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@"  AND WO.IsAreaMedia = {0}", RequetQuery.IsAreaMedia);
                var areaWhere = string.Empty;
                if (RequetQuery.AreaProvniceId != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    areaWhere += $" AND MAM.ProvinceID = {RequetQuery.AreaProvniceId }";
                }
                if (RequetQuery.AreaCityId != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    areaWhere += $" AND MAM.CityID = {RequetQuery.AreaCityId }";
                }
                if (!string.IsNullOrWhiteSpace(areaWhere))
                {
                    sbSql.AppendFormat(@" AND EXISTS(
											SELECT 1 FROM dbo.Media_Area_Mapping AS MAM WITH ( NOLOCK )
											WHERE MAM.MediaType = {1} AND MAM.MediaID = MW.MediaID
											AND MAM.RelateType = {2}
                                            {0}
                                            )", areaWhere, (int)MediaType.WeiXin, (int)MediaAreaMappingType.AreaMedia);
                }
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.AdName))
            {
                sbSql.AppendFormat(@"  AND WX.ADName LIKE '%{0}%'", RequetQuery.AdName.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.Keyword))
            {
                sbSql.AppendFormat(@"  AND (WO.WxNumber = '{0}' OR WO.NickName = '{0}')", RequetQuery.Keyword.ToSqlFilter());
            }
            //过期时间
            if (RequetQuery.EndTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat(@" AND PB.EndTime <= DATEADD(DAY,{0},GETDATE()) ", RequetQuery.EndTime);
            }

            #region ROLE

            //当前运营角色需要看到 自己审核操作过的记录
            if (ConfigEntity.RoleTypeEnum == RoleEnum.SupperAdmin ||
                ConfigEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                if (RequetQuery.CreateUserId != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    //通过或者驳回，看到的是自己的审核操作记录
                    sbSql.AppendFormat(@" AND EXISTS(
	                        SELECT 1 FROM DBO.PublishAuditInfo AS PAI WITH(NOLOCK)
	                                WHERE PAI.PublishID = PB.PubID
	                                AND PAI.CreateUserID = {0} --当前角色审核的刊例
	                                AND PAI.MediaType = PB.MediaType
	                                AND PAI.PubStatus = {1} --当前角色审核结果
                        )", RequetQuery.CreateUserId, RequetQuery.Wx_Status.Trim(',').ToSqlFilter());
                }
                else
                {
                    //待审核是全部的数据
                    //sbSql.AppendFormat(@" AND EXISTS(
                    //     SELECT 1 FROM DBO.PublishAuditInfo AS PAI WITH(NOLOCK)
                    //             WHERE PAI.PublishID = PB.PubID
                    //             AND PAI.MediaType = PB.MediaType
                    //             AND PAI.PubStatus = {0} --当前角色审核结果
                    //    )", RequetQuery.Wx_Status.Trim(',').ToSqlFilter());
                }
            }
            else if (ConfigEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //AE角色 - 到角色,媒体主 - 个人
                sbSql.AppendFormat(@" AND PB.CreateUserID IN (SELECT UserID
                                      FROM   dbo.UserRole
                                      WHERE  RoleID = '{0}' AND Status = 0)", RoleInfoMapping.AE);
            }
            else
            {
                sbSql.AppendFormat(" AND PB.CreateUserID = {0}", RequetQuery.CreateUserId);
            }

            #endregion ROLE

            //查询刊例提交人
            if (!string.IsNullOrWhiteSpace(RequetQuery.SubmitUser))
            {
                sbSql.AppendFormat(" AND (UDI.TrueName = '{0}' OR (EXISTS(SELECT 1 FROM DBO.UserInfo AS UIDD WITH(NOLOCK) WHERE UIDD.UserName = '{0}')))",
                   RequetQuery.SubmitUser.ToSqlFilter());
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDateTime))
            {
                sbSql.AppendFormat(@"  AND PB.CreateTime >= CONVERT(VARCHAR(24),'{0}',120) ", RequetQuery.StartDateTime.ToSqlFilter());
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.EndDateTime))
            {
                var date = DateTime.Parse(RequetQuery.EndDateTime.ToSqlFilter()).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                sbSql.AppendFormat(@"  AND PB.CreateTime <= CONVERT(VARCHAR(24),'{0}',120) ", date);
            }

            sbSql.AppendLine(@") T");
            var query = new PublishQuery<RespAdWeiXinAuditPassDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " PubID DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}