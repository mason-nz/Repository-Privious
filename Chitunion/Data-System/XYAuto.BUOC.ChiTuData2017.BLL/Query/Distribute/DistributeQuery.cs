/********************************************************
*创建人：lixiong
*创建时间：2017/9/12 13:53:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute
{
    public class DistributeQuery
        : PublishInfoQueryClient<RequestDistributeQueryDto, RespDistributeListDto>
    {
        public DistributeQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespDistributeListDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            StringBuilder jjrSql = new StringBuilder();
            #region 全网域
            sbSql.Append(" SELECT MaterielID,Url, AssembleTime,BrowsePageAvg,DistributeDate,DistributeType,DistributeUserId,ForwardNumber, InquiryNumber, JumpProportion, OnLineAvgTime, PV, SessionNumber,[Source], TelConnectNumber, TotalDistribute, UV,AssembleUser, DistributeUser,Title,DistributeTypeName YanFaFROM ( ");

            sbSql.AppendFormat("   SELECT MESOURCE.MaterielID,MESOURCE.MaterielUrl AS Url, MESOURCE.CreateTime AS AssembleTime,BrowsePageAvg,DistributeDate,DistributeType,DistributeUserId,ForwardNumber, InquiryNumber, JumpProportion, OnLineAvgTime, PV, SessionNumber,[Source], TelConnectNumber, TotalDistribute, UV, AssembleUser,DistributeUser, MESOURCE.Title, DistributeTypeName FROM Chitunion_OP2017.dbo.MaterielExtend MESOURCE INNER JOIN ");
            sbSql.AppendFormat(@" (SELECT
						ME.MaterielID,BrowsePageAvg,DistributeDate,DistributeType,DistributeUserId,
						ForwardNumber,InquiryNumber,JumpProportion,
				OnLineAvgTime,PV,SessionNumber,QuanWang.[Source],TelConnectNumber,TotalDistribute,UV,
		                UI.SysName AS AssembleUser ,
		                UI1.SysName AS DistributeUser ,
		                DI.DictName AS DistributeTypeName
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK) INNER JOIN (
					SELECT
	                T.DistributeDate ,
	                T.DistributeUserId,73001 AS DistributeType,D.PV,UV,OnLineAvgTime,JumpProportion,
					BrowsePageAvg,InquiryNumber,SessionNumber, TelConnectNumber,
					D.MaterielId,D.Source,D.TotalDistribute,ForwardNumber
	                FROM
	                (
	                --全网域分发+统计
	                SELECT  CH.DistributeDate ,
			                MC1.MaterielID ,
			                MC1.CreateUserID AS DistributeUserId
	                FROM    Chitunion_OP2017.dbo.MaterielChannel AS MC1
			                INNER JOIN 
							( SELECT MC.MaterielID ,
								     MIN(MC.CreateTime) AS DistributeDate
						       FROM   Chitunion_OP2017.dbo.MaterielChannel AS MC WITH (NOLOCK)
						       INNER JOIN  Chitunion_OP2017.DBO.DictInfo DC  WITH ( NOLOCK ) ON MC.ChannelType = DC.DictId
							   WHERE DictType = {0}
						       GROUP BY MC.MaterielID
					            ) AS CH ON MC1.MaterielID = CH.MaterielID AND MC1.CreateTime = CH.DistributeDate
	                GROUP BY MC1.MaterielID,CH.DistributeDate,MC1.CreateUserID
	                )AS T
	                INNER JOIN ( SELECT SUM(CASE  WHEN DD.PV <=-1 THEN 0 ELSE DD.PV END) AS PV ,
						                SUM(CASE  WHEN DD.UV <=-1 THEN 0 ELSE DD.UV END) AS UV ,
						                SUM(CASE  WHEN DD.OnLineAvgTime <=-1 THEN 0 ELSE DD.OnLineAvgTime END) AS OnLineAvgTime,
						                SUM(CASE  WHEN DD.JumpProportion <=-1 THEN 0 ELSE DD.JumpProportion END)AS JumpProportion,
						                SUM(CASE  WHEN DD.BrowsePageAvg <=-1 THEN 0 ELSE DD.BrowsePageAvg END) AS BrowsePageAvg ,
						                SUM(CASE  WHEN DD.InquiryNumber <=-1 THEN 0 ELSE DD.InquiryNumber END) AS InquiryNumber,
						                SUM(CASE  WHEN DD.SessionNumber <=-1 THEN 0 ELSE DD.SessionNumber END) AS SessionNumber,
						                SUM(CASE  WHEN DD.TelConnectNumber <=-1 THEN 0 ELSE DD.TelConnectNumber END)AS TelConnectNumber,
						                DD.MaterielId ,
						                DD.Source,
						                COUNT(0) AS TotalDistribute
				                FROM    dbo.Materiel_DistributeDetailed AS DD WITH (NOLOCK)
								WHERE  DD.DistributeUrl IS NOT NULL
				                GROUP BY DD.MaterielId,DD.Source
				                ) AS D ON D.MaterielId = T.MaterielID AND D.Source = {0} --全网域()
							LEFT JOIN 
							(SELECT MDS.MaterielId, SUM(CASE  WHEN ForwardNumber <=-1 THEN 0 ELSE ForwardNumber END)AS ForwardNumber FROM 
							[dbo].[Materiel_DetailedStatistics] MDS WITH ( NOLOCK )  
							INNER JOIN   dbo.Materiel_DistributeDetailed MDD  WITH ( NOLOCK ) ON MDS.MaterielId = MDD.MaterielId
                            AND MDS.DistributeId = MDD.DistributeId
							WHERE  MDD.Source = {0}
							GROUP BY MDS.MaterielId
							) MD ON D.MaterielId = MD.MaterielId) AS QuanWang ON ME.MaterielID = QuanWang.MaterielId
							LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK) ON DI.DictId = QuanWang.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = QuanWang.DistributeUserId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001", (int)DistributeTypeEnum.QuanWangYu);
            #endregion

            #region 经纪人
            jjrSql.AppendFormat(@" UNION 
                        SELECT
                        ME.MaterielID,
                        BrowsePageAvg, DistributeDate, DistributeType, DistributeUserId,
                        ForwardNumber, InquiryNumber, JumpProportion,
                OnLineAvgTime, PV, SessionNumber, JingJiRen.[Source], TelConnectNumber, TotalDistribute, UV,
                        UI.SysName AS AssembleUser,
                        UI1.SysName AS DistributeUser,
                        DI.DictName AS DistributeTypeName
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK) INNER JOIN(
                    SELECT
                    T.DistributeDate,
                    T.DistributeUserId, 73002 AS DistributeType, D.PV, UV, OnLineAvgTime, JumpProportion,
                    BrowsePageAvg, InquiryNumber, SessionNumber, TelConnectNumber,
                    D.MaterielId, D.Source, D.TotalDistribute, ForwardNumber
                    FROM
                    (
                        SELECT  CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId AS DistributeUserId
                        FROM    dbo.Materiel_DistributeQingNiaoAgent AS DNAT1 WITH(NOLOCK)
                                INNER JOIN(SELECT DNAT.MaterielId,
                                                    MIN(DNAT.DistributeDate) AS DistributeDate
                                             FROM   dbo.Materiel_DistributeQingNiaoAgent AS DNAT WITH(NOLOCK)
                                             GROUP BY DNAT.MaterielId
                                           ) AS CH ON CH.MaterielId = DNAT1.MaterielId  AND CH.DistributeDate = DNAT1.DistributeDate
                        WHERE DNAT1.[Type] = 1
                        GROUP BY CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId
                    )AS T
                    INNER JOIN(SELECT SUM(CASE  WHEN DD.PV <= -1 THEN 0 ELSE DD.PV END) AS PV,
                                        SUM(CASE  WHEN DD.UV <= -1 THEN 0 ELSE DD.UV END) AS UV,
                                        SUM(CASE  WHEN DD.OnLineAvgTime <= -1 THEN 0 ELSE DD.OnLineAvgTime END) AS OnLineAvgTime,
                                        SUM(CASE  WHEN DD.JumpProportion <= -1 THEN 0 ELSE DD.JumpProportion END)AS JumpProportion,
                                        SUM(CASE  WHEN DD.BrowsePageAvg <= -1 THEN 0 ELSE DD.BrowsePageAvg END) AS BrowsePageAvg,
                                        SUM(CASE  WHEN DD.InquiryNumber <= -1 THEN 0 ELSE DD.InquiryNumber END) AS InquiryNumber,
                                        SUM(CASE  WHEN DD.SessionNumber <= -1 THEN 0 ELSE DD.SessionNumber END) AS SessionNumber,
                                        SUM(CASE  WHEN DD.TelConnectNumber <= -1 THEN 0 ELSE DD.TelConnectNumber END)AS TelConnectNumber,
                                        DD.MaterielId,
                                        DD.Source,
                                        COUNT(0) AS TotalDistribute
                                FROM    dbo.Materiel_DistributeDetailed AS DD WITH(NOLOCK)
                                WHERE  DD.DistributeUrl IS NOT NULL
                                GROUP BY DD.MaterielId, DD.Source
                                ) AS D ON D.MaterielId = T.MaterielId AND D.Source = {0}--经纪人
                                LEFT JOIN
                            (SELECT MDS.MaterielId, SUM(CASE  WHEN ForwardNumber <= -1 THEN 0 ELSE ForwardNumber END)AS ForwardNumber
                            FROM[dbo].[Materiel_DetailedStatistics] MDS WITH(NOLOCK)
                            INNER JOIN   dbo.Materiel_DistributeDetailed MDD ON MDS.MaterielId = MDD.MaterielId
                            AND MDS.DistributeId = MDD.DistributeId
                            WHERE  MDD.Source = {0}
                            GROUP BY MDS.MaterielId
							) MD ON D.MaterielId = MD.MaterielId ) AS JingJiRen ON ME.MaterielID = JingJiRen.MaterielId
                            LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK)ON DI.DictId = JingJiRen.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = JingJiRen.DistributeUserId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001", (int)DistributeTypeEnum.QingNiaoAgent);
            #endregion

            #region 查询条件

            var sbSqlWhere = new StringBuilder();

            StringBuilder wlSqlWhere = new StringBuilder();

            sbSqlWhere.AppendFormat(@" AND ME.CreateTime>= '{0}' AND ME.CreateTime< '{1}'", RequetQuery.StartDate, Convert.ToDateTime(RequetQuery.EndDate).AddDays(1).ToString("yyyy-MM-dd"));
            wlSqlWhere.AppendFormat(sbSqlWhere.ToString());
            //全网域,经纪人(分发类型)
            if (RequetQuery.DistributeType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.AppendFormat($@" AND DistributeType = {RequetQuery.DistributeType}");
            }
            //分发操作人(QuanWangYu,QingNiaoAgent)
            if (!string.IsNullOrWhiteSpace(RequetQuery.DistributeUser))
            {
                var type = IsMatchDistributeType(RequetQuery.DistributeUser);
                if (type > 0)
                {
                    sbSqlWhere.AppendFormat($@" AND DistributeType = {type}");
                }
                else
                {
                    sbSqlWhere.AppendFormat(@" AND ( UI1.UserName LIKE '%{0}%'
                                                          OR UI1.SysName LIKE '%{0}%'
                                                          OR UI1.Mobile LIKE '%{0}%'
                                                        )", RequetQuery.DistributeUser.ToSqlFilter());
                }
            }
            //组装操作人c)	组装操作人：目前组装的物料全部为赤兔系统人工封装，可通过输入赤兔系统的用户名/手机号查询
            if (!string.IsNullOrWhiteSpace(RequetQuery.AssembleUser))
            {
                sbSqlWhere.AppendFormat(@" AND (UI.UserName LIKE '%{0}%' OR UI.SysName LIKE '%{0}%' OR UI.Mobile LIKE '%{0}%')",
                              RequetQuery.AssembleUser.ToSqlFilter());
                wlSqlWhere.AppendFormat(@" AND (UI.UserName LIKE '%{0}%' OR UI.SysName LIKE '%{0}%' OR UI.Mobile LIKE '%{0}%')",
                              RequetQuery.AssembleUser.ToSqlFilter());
            }

            //标题/URL模糊搜索
            if (!string.IsNullOrWhiteSpace(RequetQuery.MaterielName))
            {
                sbSqlWhere.AppendFormat(@" AND (ME.Title LIKE '%{0}%' OR ME.HeadContentURL LIKE '%{0}%')", RequetQuery.MaterielName.ToSqlFilter());
                wlSqlWhere.AppendFormat(@" AND (ME.Title LIKE '%{0}%' OR ME.HeadContentURL LIKE '%{0}%')", RequetQuery.MaterielName.ToSqlFilter());
            }
            //车型
            if (RequetQuery.CarSerialId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.AppendFormat(@" AND ME.SerialID = {0}", RequetQuery.CarSerialId);
                wlSqlWhere.AppendFormat(@" AND ME.SerialID = {0}", RequetQuery.CarSerialId);
            }
            //子品牌
            if (RequetQuery.BrandId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.AppendFormat(@"
                        AND ME.SerialID IN (
	                        SELECT CS.SerialID FROM BaseData2017.DBO.CarSerial AS CS WITH(NOLOCK)
	                        WHERE CS.BrandID = {0}
                        )", RequetQuery.BrandId);

                wlSqlWhere.AppendFormat(@"
                        AND ME.SerialID IN (
	                        SELECT CS.SerialID FROM BaseData2017.DBO.CarSerial AS CS WITH(NOLOCK)
	                        WHERE CS.BrandID = {0}
                        )", RequetQuery.BrandId);
            }
            //品牌模糊查询
            if (!string.IsNullOrWhiteSpace(RequetQuery.CarSerialName))
            {
                sbSqlWhere.AppendFormat(@"
                        AND ME.SerialID IN (
	                        SELECT CS.SerialID FROM BaseData2017.DBO.CarSerial AS CS WITH(NOLOCK)
                            LEFT JOIN BaseData2017.dbo.CarBrand AS CB WITH ( NOLOCK ) ON CB.BrandID = CS.BrandID
	                        WHERE CS.ShowName LIKE '%{0}%' OR CS.Name LIKE '%{0}%' OR CB.Name LIKE '%{0}%'
                        )", RequetQuery.CarSerialName.ToSqlFilter());

                wlSqlWhere.AppendFormat(@"
                        AND ME.SerialID IN (
	                        SELECT CS.SerialID FROM BaseData2017.DBO.CarSerial AS CS WITH(NOLOCK)
                            LEFT JOIN BaseData2017.dbo.CarBrand AS CB WITH ( NOLOCK ) ON CB.BrandID = CS.BrandID
	                        WHERE CS.ShowName LIKE '%{0}%' OR CS.Name LIKE '%{0}%' OR CB.Name LIKE '%{0}%'
                        )", RequetQuery.CarSerialName.ToSqlFilter());
            }
            //渠道场景
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.AppendFormat(@" AND ME.SceneID = {0}", RequetQuery.ChannelId);

                wlSqlWhere.AppendFormat(@" AND ME.SceneID = {0}", RequetQuery.ChannelId);
            }
            //ip 子ip
            if (RequetQuery.Ip != Entities.Constants.Constant.INT_INVALID_VALUE
                || RequetQuery.ChildIp != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.AppendFormat(@" AND EXISTS( ");

                wlSqlWhere.AppendFormat(@" AND EXISTS( ");
                var ipSql = $@" SELECT 1 FROM Chitunion_OP2017.DBO.MaterielIpLableInfo AS MLP WITH(NOLOCK)
                                WHERE MLP.MaterielID = ME.MaterielID ";
                if (RequetQuery.Ip != Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    ipSql += $" AND MLP.TitleID = {RequetQuery.Ip} AND MLP.Type = 6001";
                }
                else
                {
                    ipSql += $" AND MLP.TitleID = {RequetQuery.ChildIp} AND MLP.Type = 6002";
                }
                sbSqlWhere.AppendFormat(ipSql);
                sbSqlWhere.AppendFormat(@" ) ");

                wlSqlWhere.AppendFormat(ipSql);
                wlSqlWhere.AppendFormat(@" ) ");
            }
            #endregion

            RequetQuery.SqlWhere = sbSqlWhere.ToString() + "#" + wlSqlWhere.ToString();
            sbSql.AppendFormat(sbSqlWhere.ToString()).AppendFormat(jjrSql.ToString()).AppendFormat(sbSqlWhere.ToString());
            sbSql.AppendLine(@") UNIONALL ON MESOURCE.MaterielID = UNIONALL.MaterielID");
            sbSql.AppendFormat(" WHERE MESOURCE.Status = 0 AND MESOURCE.ArticleFrom <> 69001 AND MESOURCE.CreateTime>= '{0}' AND MESOURCE.CreateTime<'{1}'", RequetQuery.StartDate, Convert.ToDateTime(RequetQuery.EndDate).AddDays(1).ToString("yyyy-MM-dd"));
            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDistributeListDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = GetOrderBy(),
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespDistributeListDto> GetResult(List<RespDistributeListDto> resultList,
            QueryPageBase<RespDistributeListDto> query)
        {
            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }

            resultList.ForEach(s =>
            {
                s.BrowsePageAvg = DistributeProfile.GetAvg(Convert.ToDouble(s.BrowsePageAvg), s.TotalDistribute);

                if (s.DistributeType == (int)DistributeTypeEnum.QuanWangYu)
                {
                    s.OnLineAvgTimeFormt = DistributeProfile.GetOnLineAvgTimeFormt(DistributeProfile.GetAvg(s.OnLineAvgTime, s.TotalDistribute));
                    s.DistributeUrl = s.Url;
                    s.JumpProportion = -1;//赤兔没有跳出率（因为这里是统计查询累加，赤兔类型的入库是0.00，查询的时候也是过滤了-1，没有办法直接返回-1）
                }
                else
                {
                    //经纪人默认没有分发人
                    s.OnLineAvgTimeFormt = "一";
                    s.DistributeUser = DistributeTypeEnum.QingNiaoAgent.GetEnumDesc();
                    s.JumpProportion = DistributeProfile.GetJumpProportionAvg(s.JumpProportion, s.TotalDistribute);
                }
            });
            var resp = base.GetResult(resultList, query);
            resp.Extend = new DistributeProvider().GetDistributeTotals(RequetQuery.StartDate, RequetQuery.EndDate, RequetQuery.SqlWhere);

            return resp;
        }

        private int IsMatchDistributeType(string key)
        {
            var dic = new Dictionary<string, int>()
            {
                //{ DistributeTypeEnum.QuanWangYu.ToString(), (int)DistributeTypeEnum.QuanWangYu},
                { DistributeTypeEnum.QingNiaoAgent.ToString(), (int)DistributeTypeEnum.QingNiaoAgent}
            };

            return dic.FirstOrDefault(s => s.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;
        }

        private string GetOrderBy()
        {
            var orderByStr = " DistributeDate DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001,"   PV ASC "},
                {1002,"   PV DESC"},
                {1011,"   UV ASC "},
                {1012,"   UV DESC"},
                {1021,"   OnLineAvgTime ASC "},
                {1022,"   OnLineAvgTime DESC"},
                {1031,"   JumpProportion ASC "},
                {1032,"   JumpProportion DESC"},
                {1041,"   BrowsePageAvg ASC "},
                {1042,"   BrowsePageAvg DESC"},
                {1051,"   InquiryNumber ASC "},
                {1052,"   InquiryNumber DESC"},
                {1061,"   SessionNumber ASC "},
                {1062,"   SessionNumber DESC"},
                {1071,"   TelConnectNumber ASC "},
                {1072,"   TelConnectNumber DESC"},
                {1081,"   ForwardNumber ASC "},
                {1082,"   ForwardNumber DESC"},
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == RequetQuery.OrderBy);
            return value.Value ?? orderByStr;
        }
    }
}