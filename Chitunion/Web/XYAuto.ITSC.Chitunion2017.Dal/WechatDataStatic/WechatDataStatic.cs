using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WechatDataStatic
{
    public class WechatDataStatic : DataBase
    {
        public static readonly WechatDataStatic Instance = new WechatDataStatic();
        public DataTable StaticInviteDataForDay()
        {
            string strSql = $@"SELECT   CONVERT(VARCHAR(10), D.CreateTime, 23) 日期 ,
                WX.nickname 产生邀请行为的用户 ,
                COUNT(CASE WHEN D.ShareResult != 1 THEN NULL
                           ELSE 1
                      END) 成功邀请次数,
                ( SELECT    COUNT(1)
                  FROM      dbo.LE_InviteRecord
                  WHERE     InviteUserID = D.CreateUserID AND   CONVERT(VARCHAR(10),InviteTime,23)=CONVERT(VARCHAR(10), DATEADD(D,-1,GETDATE())  ,23)
                ) 该用户成功邀请好友数量 ,
                ( SELECT    COUNT(CASE WHEN I.RedEvesPrice <= 0 THEN NULL
                                       ELSE I.RedEvesPrice
                                  END) RedCount
                  FROM      dbo.LE_InviteRecord I
                  WHERE     InviteUserID = D.CreateUserID AND CONVERT(VARCHAR(10),I.ReceiveTime,23)=CONVERT(VARCHAR(10), DATEADD(D,-1,GETDATE())  ,23)
                ) 该用户领取红包数量 ,
                ( SELECT    ISNULL(SUM(I.RedEvesPrice),0)  RedEvesPrice
                  FROM      dbo.LE_InviteRecord I
                  WHERE     InviteUserID = D.CreateUserID AND CONVERT(VARCHAR(10),I.ReceiveTime,23)=CONVERT(VARCHAR(10), DATEADD(D,-1,GETDATE())  ,23)
                ) 该用户领取红包金额
       FROM     LE_ShareDetail D
                LEFT JOIN (SELECT *
                            FROM
                            (
                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                          ORDER BY CreateTime DESC
                                                         ) rowNum,
                                    *
                                FROM dbo.LE_WeiXinUser 
                                WHERE Status >= 0
                            ) AS A
                            WHERE A.rowNum = 1) WX ON D.CreateUserID = WX.UserID
       WHERE    D.Type ={(int)ShareTypeEnum.邀请}  AND CONVERT(VARCHAR(10),D.CreateTime,23) =CONVERT(VARCHAR(10), DATEADD(D,-1,GETDATE())  ,23)
       GROUP BY D.CreateUserID ,
                CONVERT(VARCHAR(10), D.CreateTime, 23) ,
                WX.nickname;";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public DataTable StaticSignDataForDay()
        {
            string strSql = $@"
                            SELECT  CONVERT(VARCHAR(10), D.SignTime, 23) 日期 ,
                                    W.nickname 签到用户 ,
									U.UserName 用户名,
                                    D.SignNumber 第几天 ,
                                    D.SignPrice 领取奖励金额
                            FROM    dbo.LE_DaySign D
                                    LEFT JOIN (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime DESC
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                                WHERE Status >= 0
                                            ) AS A
                                            WHERE A.rowNum = 1) W ON W.UserID = D.SignUserID
									LEFT JOIN dbo.UserInfo AS U ON U.UserID=W.UserID
                            WHERE   CONVERT(VARCHAR(10), D.SignTime, 23) = CONVERT(VARCHAR(10), DATEADD(D,
                                                                                          -1, GETDATE()), 23);";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public DataTable StaticDistributeForDay()
        {
            int channelId = (int)LeOrderChannelTypeEnum.微信;
            string strSql = $@"  SELECT   CONVERT(VARCHAR(10),GETDATE(),23) 日期,U.nickname AS '产生分发行为用户' ,--用户
        A.OrderTotal AS '领取订单数量' ,--订单数量
        B.CPCTotal AS '点击数量' ,--点击数量
        B.CPLTotal AS '产生线索量' ,--线索数量
        A.TotalAmount AS '获得分发奖励金额'--金额
FROM    ( SELECT    UserID ,
                    COUNT(0) AS OrderTotal ,
                    SUM(TotalAmount) AS TotalAmount
          FROM      dbo.LE_ADOrderInfo 
          WHERE     StatisticsStatus = 1
                    --AND ChannelID = {channelId}
                    AND CreateTime = CAST(DATEADD(DAY,-1,GETDATE()) AS DATE)
          GROUP BY  UserID
        ) AS A
        INNER JOIN ( SELECT OI.UserID ,
                            SUM(CPCShowCount) AS CPCTotal ,
                            SUM(CPLShowCount) AS CPLTotal
                     FROM   LE_ADOrderInfo AS OI
                            JOIN dbo.LE_AccountBalance AS AB ON OI.RecID = AB.OrderID
                     WHERE  StatisticsStatus = 1
                            --AND ChannelID ={channelId}
                            AND AB.StatisticsTime = CAST(DATEADD(DAY,-1,GETDATE()) AS DATE)
                     GROUP BY OI.UserID
                   ) AS B ON B.UserID = A.UserID
        INNER JOIN (SELECT *
                        FROM
                        (
                            SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                      ORDER BY CreateTime DESC
                                                     ) rowNum,
                                *
                            FROM dbo.LE_WeiXinUser
                            WHERE Status >= 0
                        ) AS A
                        WHERE A.rowNum = 1) AS U ON U.UserID = A.UserID;";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public DataTable StaticDataSumForWeek()
        {
            int channelId = (int)LeOrderChannelTypeEnum.微信;
            string strSql = $@"
           						DECLARE @BeginTime DATE ,
							@EndTime DATE;
						SET @BeginTime = DATEADD(DAY, 3,
												 ( DATEADD(WEEK, -1,
														   DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()),
																   0)) ));

						SET @EndTime = DATEADD(DAY, 7, @BeginTime); 


						SELECT  DateDB.Date AS '日期' ,
								NewUser.新关注用户总数 ,
								H5User.通过H5上二维码关注的新用户 ,
								YQHY.通过邀请好友页面上二维码新关注的用户 ,
								HYXW.产生邀请好友行为总用户数 ,
								QDUser.产生签到行为总用户数 ,
								QDPrice.签到奖励总金额 ,
								FenFUser.产生分发行为总用户数 ,
								DingDanCount.产生订单总数量 ,
								XianSuoCount.产生线索总数量 ,
								JiangLi.分发奖励发放总金额
						FROM    dbo.fn_GetDateByCycle(@BeginTime, DATEADD(DAY, -1, @EndTime)) AS DateDB
								LEFT JOIN ( SELECT  CreateTime ,
													COUNT(0) AS '新关注用户总数'
											FROM    (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime DESC
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                                WHERE Status >= 0
                                            ) AS A
                                            WHERE A.rowNum = 1) as WX
											WHERE   CreateTime BETWEEN @BeginTime AND @EndTime
											GROUP BY CreateTime
										  ) AS NewUser ON NewUser.CreateTime = DateDB.Date
								LEFT JOIN ( SELECT  CreateTime ,
													COUNT(0) AS '通过H5上二维码关注的新用户'
											FROM    (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime DESC
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                                WHERE Status >= 0
                                            ) AS A
                                            WHERE A.rowNum = 1) as WX
											WHERE   Inviter = '撒币活动'
													AND CreateTime BETWEEN @BeginTime AND @EndTime
											GROUP BY CreateTime
										  ) AS H5User ON H5User.CreateTime = DateDB.Date
								LEFT JOIN ( SELECT  CreateTime ,
													COUNT(0) AS '通过邀请好友页面上二维码新关注的用户'
											FROM    (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime DESC
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                                WHERE Status >= 0
                                            ) AS A
                                            WHERE A.rowNum = 1) as WX
											WHERE   Source = 101
													AND CreateTime BETWEEN @BeginTime AND @EndTime
											GROUP BY CreateTime
										  ) AS YQHY ON YQHY.CreateTime = DateDB.Date
								LEFT JOIN ( SELECT  CreateTime ,
													COUNT(0) AS '产生邀请好友行为总用户数'
											FROM    dbo.LE_ShareDetail
											WHERE   ShareResult = 1
													AND Type = 202002
													AND CreateTime BETWEEN @BeginTime AND @EndTime
											GROUP BY CreateTime
										  ) AS HYXW ON HYXW.CreateTime = DateDB.Date--ShareUserTotal, --产生邀请好友行为总用户数,
								LEFT JOIN ( SELECT  SignTime ,
													COUNT(0) AS '产生签到行为总用户数'
											FROM    dbo.LE_DaySign
											WHERE   SignTime BETWEEN @BeginTime AND @EndTime
											GROUP BY SignTime
										  ) AS QDUser ON QDUser.SignTime = DateDB.Date
								LEFT JOIN ( SELECT  SignTime ,
													SUM(SignPrice) AS '签到奖励总金额'
											FROM    dbo.LE_DaySign
											WHERE   SignTime BETWEEN @BeginTime AND @EndTime
											GROUP BY SignTime
										  ) AS QDPrice ON QDPrice.SignTime = DateDB.Date
								LEFT JOIN ( SELECT  CreateTime ,
													COUNT(0) AS '产生分发行为总用户数'
											FROM    dbo.LE_ADOrderInfo
											WHERE   1=1
                                                    --ChannelID = 103001
													AND CreateTime BETWEEN @BeginTime AND @EndTime
											GROUP BY CreateTime
										  ) AS FenFUser ON FenFUser.CreateTime = DateDB.Date
								LEFT JOIN ( SELECT  CreateTime ,
													COUNT(0) AS '产生订单总数量'
											FROM    dbo.LE_ADOrderInfo
											WHERE   1=1
                                                    --ChannelID = 103001
													AND CreateTime BETWEEN @BeginTime AND @EndTime
											GROUP BY CreateTime
										  ) AS DingDanCount ON DingDanCount.CreateTime = DateDB.Date
								LEFT JOIN ( SELECT  SUM(B.CPCShowCount) AS '产生线索总数量' ,
													B.StatisticsTime
											FROM    dbo.LE_AccountBalance AS B
													INNER JOIN dbo.LE_ADOrderInfo AS A ON A.RecID = B.OrderID
											WHERE   B.CPCShowCount > 0
													--AND A.ChannelID = 103001
													AND StatisticsTime BETWEEN @BeginTime AND @EndTime
											GROUP BY B.StatisticsTime
										  ) AS XianSuoCount ON XianSuoCount.StatisticsTime = DateDB.Date
								LEFT JOIN ( SELECT  SUM(TotalAmount) AS '分发奖励发放总金额' ,
													CreateTime
											FROM    dbo.LE_ADOrderInfo
											WHERE   1=1
                                                    --ChannelID = 103001
													AND CreateTime BETWEEN @BeginTime AND @EndTime
											GROUP BY CreateTime
										  ) AS JiangLi ON JiangLi.CreateTime = DateDB.Date
						ORDER BY DateDB.Date;";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 微信用户数据统计
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinUserDay()
        {
            string SelectSQL = @"
                                WITH    t AS ( SELECT   '微信（H5）' AS '产品端' ,
                                                        CAST(DATEADD(DAY, -1,
                                                              GETDATE()) AS DATE) AS '时间' ,
                                                        ( SELECT
                                                              COUNT(0)
                                                          FROM
                                                              dbo.UserInfo
                                                          WHERE
                                                              Source = 3006 AND Status>=0
                                                              AND CreateTime < CONVERT(NVARCHAR(20), GETDATE(), 23)
                                                        ) AS '累计注册用户' ,
                                                        ( SELECT COUNT(0)
                                                            FROM
                                                            (
                                                                SELECT COUNT(UserID) AS Num
                                                                FROM dbo.LE_WeiXinVisvit_Log
                                                                WHERE Type = 1
                                                                      AND 1 = DATEDIFF(DAY,
                                                                                  LastUpdateTime,
                                                                                  GETDATE()
                                                                                      )
                                                                GROUP BY UserID
                                                            ) AS TJ
                                                        ) AS '登录用户（有效访问）' ,
                                                        ( SELECT
                                                              COUNT(0)
                                                          FROM
                                                              dbo.LE_ADOrderInfo
                                                          WHERE 1=1
                                                              --ChannelID = 101003  
                                                              AND 1 = DATEDIFF(DAY,
                                                              CreateTime,
                                                              GETDATE())
                                                        ) AS '分发订单量' ,
                                                        ( SELECT
                                                              COUNT(0)
                                                          FROM
                                                              ( SELECT
                                                              TaskID
                                                              FROM
                                                              dbo.LE_ADOrderInfo
                                                              WHERE 1=1
                                                              --ChannelID = 101003
                                                              AND 1 = DATEDIFF(DAY,
                                                              CreateTime,
                                                              GETDATE())
                                                              GROUP BY TaskID
                                                              ) AS A
                                                        ) AS '分发任务量' ,
                                                        ( SELECT
                                                              COUNT(0)
                                                          FROM
                                                              ( SELECT
                                                              UserID
                                                              FROM
                                                              dbo.LE_ADOrderInfo
                                                              WHERE  1=1
                                                              --ChannelID = 101003
                                                              AND 1 = DATEDIFF(DAY,
                                                              CreateTime,
                                                              GETDATE())
                                                              GROUP BY UserID
                                                              ) AS A
                                                        ) AS '分发人数' ,
                                                        ( SELECT
                                                              ISNULL(SUM(LE_AccountBalance.TotalMoney),
                                                              0)
                                                          FROM
                                                              dbo.LE_ADOrderInfo
                                                              INNER JOIN dbo.LE_AccountBalance ON LE_ADOrderInfo.RecID = dbo.LE_AccountBalance.OrderID
                                                          WHERE 1=1
                                                              --LE_ADOrderInfo.ChannelID = 101003
                                                              AND 1 = DATEDIFF(DAY,
                                                              StatisticsTime,
                                                              GETDATE())
                                                        ) AS '分发花费'
                                             )
                                    SELECT  t.产品端 ,
                                            t.时间 ,
                                            t.累计注册用户 ,
                                            t.[登录用户（有效访问）] ,
											t.分发订单量,
                                            t.分发任务量 ,
                                            t.分发人数 ,
                                            CASE t.分发人数
                                              WHEN 0 THEN 0
                                              ELSE ROUND(CAST(t.分发订单量 AS FLOAT)
                                                         / CAST(t.分发人数 AS FLOAT),
                                                         2)
                                            END AS '人均分发量' ,
                                            t.分发花费 ,
                                            CASE t.分发人数
                                              WHEN 0 THEN 0
                                              ELSE ROUND(CAST(t.分发花费 AS FLOAT)
                                                         / CAST(t.分发人数 AS FLOAT),
                                                         2)
                                            END AS '人均成本'
                                    FROM    t;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }
        /// <summary>
        /// 微信渠道数据统计
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinChannelDay()
        {
            string SelectSQL = @"
                                IF OBJECT_ID('tempdb..#ChannelTJ') IS NOT NULL
                                    DROP TABLE #ChannelTJ;
                                WITH P
                                AS (SELECT *,
                                        CAST(ChannelName AS NVARCHAR(200)) AS Path
                                    FROM LE_PromotionChannel_Dict WITH (NOLOCK)
                                    WHERE DictID in(101003,101004,101005)
                                    UNION ALL
                                    SELECT A.*,
                                        CAST(B.Path + '-' + CAST(A.ChannelName AS NVARCHAR(100)) AS NVARCHAR(200)) AS Path
                                    FROM LE_PromotionChannel_Dict A,
                                        P B
                                    WHERE A.ParentID = B.DictID
                                   )
                                SELECT *
                                INTO #ChannelTJ
                                FROM
                                (
                                    SELECT *
                                    FROM P WITH (NOLOCK)
                                    UNION ALL
                                    SELECT *,
                                        CAST(ChannelName AS NVARCHAR(200)) AS Path
                                    FROM dbo.LE_PromotionChannel_Dict WITH (NOLOCK)
                                    WHERE DictID = 0
                                ) AS L
                                WHERE ChannelCode <> ''
                                      OR DictID = 0;
                                SELECT '微信（H5）' AS '产品端',
                                    PW.DictID,
                                    CAST(DATEADD(DAY, -1, GETDATE()) AS DATE) AS '时间',
                                    dbo.fn_SplitSubString(Path, '-', 1) AS '一级菜单',
                                    dbo.fn_SplitSubString(Path, '-', 2) AS '二级菜单',
                                    dbo.fn_SplitSubString(Path, '-', 3) AS '三级菜单',
                                    dbo.fn_SplitSubString(Path, '-', 4) AS '四级菜单',
                                    dbo.fn_SplitSubString(Path, '-', 5) AS '五级菜单',
                                    ISNULL(PW.ChannelCode, 0) AS '渠道编码（多级）',
                                    ISNULL(A.累计注册用户, 0) AS '累计注册用户',
                                    ISNULL(B.新增注册用户, 0) AS '新增注册用户',
                                    ISNULL(DingDan.分发订单量, 0) AS '分发订单量',
                                    ISNULL(C.分发任务量, 0) AS '分发任务量',
                                    ISNULL(D.分发人数, 0) AS '分发人数',
                                    0 AS '访问pv',
                                    0 AS '访问uv',
                                    ISNULL(M.回流pv, 0) AS '回流pv',
                                    ISNULL(M.回流uv, 0) AS '回流uv',
                                    ISNULL(PvUv.基础物料pv, 0) AS '基础物料pv',
                                    ISNULL(PvUv.基础物料uv, 0) AS '基础物料uv',
                                    ISNULL(M.基础物料订单, 0) AS '基础物料订单',
                                    0 AS '注册成本',
                                    0 AS '粉丝成本',
                                    0 AS '花费'
                                FROM #ChannelTJ AS PW
                                    LEFT JOIN
                                     (
                                         SELECT COUNT(0) AS '累计注册用户',
                                             PromotionChannelID
                                         FROM dbo.UserInfo WITH (NOLOCK)
                                         WHERE Source IN ( 3006, 3007 )
                                               AND CreateTime < CONVERT(NVARCHAR(20), GETDATE(), 23)
                                         GROUP BY PromotionChannelID
                                     ) AS A
                                        ON PW.DictID = A.PromotionChannelID
                                    LEFT JOIN
                                     (
                                         SELECT COUNT(0) AS '新增注册用户',
                                             PromotionChannelID
                                         FROM dbo.UserInfo WITH (NOLOCK)
                                         WHERE Source IN ( 3006, 3007 )
                                               AND 1 = DATEDIFF(DAY,
                                                           CreateTime,
                                                           GETDATE()
                                                               )
                                         GROUP BY PromotionChannelID
                                     ) AS B
                                        ON PW.DictID = B.PromotionChannelID
                                    LEFT JOIN
                                     (
                                         SELECT SUM(C.pv) AS '基础物料pv',
                                             SUM(C.uv) AS '基础物料uv',
                                             PromotionChannelID
                                         FROM dbo.LE_ADOrderInfo AS A WITH (NOLOCK)
                                             JOIN
                                              (
                                                  SELECT click_val,
                                                      SUM(pv) AS pv,
                                                      SUM(uv) AS uv
                                                  FROM Chitunion_DataSystem2017..chitu_click_stat WITH (NOLOCK)
                                                  GROUP BY click_val
                                              ) AS C
                                                 ON A.OrderCoding = C.click_val
                                         WHERE 1=1
                                               --ChannelID = 101003
                                               AND A.OrderCoding <> ''
                                               AND 1 = DATEDIFF(DAY,
                                                           CreateTime,
                                                           GETDATE()
                                                               )
                                         GROUP BY PromotionChannelID
                                     ) AS PvUv
                                        ON PW.DictID = PvUv.PromotionChannelID
                                    LEFT JOIN
                                     (
                                         SELECT COUNT(0) AS '分发订单量',
                                             PromotionChannelID
                                         FROM dbo.LE_ADOrderInfo WITH (NOLOCK)
                                         WHERE 1=1
                                               --ChannelID = 101003
                                               AND 1 = DATEDIFF(DAY,
                                                           CreateTime,
                                                           GETDATE()
                                                               )
                                         GROUP BY PromotionChannelID
                                     ) AS DingDan
                                        ON PW.DictID = DingDan.PromotionChannelID
                                    LEFT JOIN
                                     (
                                         SELECT COUNT(0) AS '分发任务量',
                                             RW.PromotionChannelID
                                         FROM
                                         (
                                             SELECT TaskID,
                                                 PromotionChannelID
                                             FROM dbo.LE_ADOrderInfo WITH (NOLOCK)
                                             WHERE 1=1
                                                   --ChannelID = 101003
                                                   AND 1 = DATEDIFF(DAY,
                                                               CreateTime,
                                                               GETDATE()
                                                                   )
                                             GROUP BY TaskID,
                                                 PromotionChannelID
                                         ) AS RW
                                         GROUP BY RW.PromotionChannelID
                                     ) AS C
                                        ON C.PromotionChannelID = PW.DictID
                                    LEFT JOIN
                                     (
                                         SELECT COUNT(0) AS '分发人数',
                                             YH.PromotionChannelID
                                         FROM
                                         (
                                             SELECT UserID,
                                                 PromotionChannelID
                                             FROM dbo.LE_ADOrderInfo WITH (NOLOCK)
                                             WHERE 1=1
                                                   --ChannelID = 101003
                                                   AND 1 = DATEDIFF(DAY,
                                                               CreateTime,
                                                               GETDATE()
                                                                   )
                                             GROUP BY UserID,
                                                 PromotionChannelID
                                         ) AS YH
                                         GROUP BY YH.PromotionChannelID
                                     ) AS D
                                        ON D.PromotionChannelID = PW.DictID
                                    LEFT JOIN
                                     (
                                         SELECT SUM(chitu.pv) AS '回流pv',
                                             SUM(chitu.uv) AS '回流uv',
                                             SUM(chitu.orders) AS '基础物料订单',
                                             PromotionChannelID
                                         FROM dbo.LE_ADOrderInfo WITH (NOLOCK)
                                             INNER JOIN Chitunion_DataSystem2017..chitu_channel_stat AS chitu WITH (NOLOCK)
                                                 ON chitu.channel = dbo.LE_ADOrderInfo.OrderCoding
                                         WHERE 1=1
                                               --LE_ADOrderInfo.ChannelID = 101003
                                               AND OrderCoding <> ''
                                               AND 1 = DATEDIFF(DAY,
                                                           chitu.dt,
                                                           GETDATE()
                                                               )
                                         GROUP BY PromotionChannelID
                                     ) AS M
                                        ON PW.DictID = M.PromotionChannelID;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }
        /// <summary>
        /// 微信抢单赚钱统计数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinOrderDay()
        {
            string SelectSQL = @"
                                SELECT  '微信（H5）' AS '产品端' ,
                                        CAST(DATEADD(DAY, -1, GETDATE()) AS DATE) AS '时间' ,
                                        0 AS '选择分类页pv' ,
                                        0 AS '选择分类页uv' ,
                                        0 AS '分类按钮点击pv' ,
                                        0 AS '分类按钮点击uv' ,
                                        0 AS '任务列表页pv' ,
                                        0 AS '任务列表页uv' ,
                                        ISNULL(( SELECT SUM(organic_pv)
                                                 FROM   Chitunion_DataSystem2017..chitu_material_stat
                                                 WHERE  1 = DATEDIFF(DAY, dt, GETDATE())
                                               ), 0) AS '物料详情页pv（预览）' ,
                                        ISNULL((SELECT SUM(organic_uv)
                                                 FROM   Chitunion_DataSystem2017..chitu_material_stat
                                                 WHERE  1 = DATEDIFF(DAY, dt, GETDATE())
                                               ), 0) AS '物料详情页uv（预览）' ,
										ISNULL(( SELECT COUNT(0)
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202001
                                                        AND ShareResult = 1
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                               ), 0) AS '物料详情页分享成功pv' ,
                                        ISNULL((SELECT COUNT(0) FROM( SELECT CreateUserID
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202001
                                                        AND ShareResult = 1
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                                 GROUP BY CreateUserID) AS C
                                               ), 0) AS '物料详情页分享成功uv' ,
                                        ISNULL(( SELECT COUNT(0)
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202001
                                                        AND ShareResult = 0
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                               ), 0) AS '物料详情页分享失败pv（取消分享）' ,
                                        ISNULL(( SELECT COUNT(0) FROM( SELECT CreateUserID
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202001
                                                        AND ShareResult = 0
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                                 GROUP BY CreateUserID)AS S
                                               ), 0) AS '物料详情页分享失败uv（取消分享）' ,
                                        ISNULL(( SELECT COUNT(0)
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202002
                                                        AND ShareResult = 1
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                               ), 0) AS '分享成功pv' ,
                                        ISNULL((SELECT COUNT(0) FROM( SELECT CreateUserID
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202002
                                                        AND ShareResult = 1
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                                 GROUP BY CreateUserID) AS C
                                               ), 0) AS '分享成功uv' ,
                                        ISNULL(( SELECT COUNT(0)
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202002
                                                        AND ShareResult = 0
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                               ), 0) AS '分享失败pv（取消分享）' ,
                                        ISNULL(( SELECT COUNT(0) FROM( SELECT CreateUserID
                                                 FROM   dbo.LE_ShareDetail
                                                 WHERE  Type = 202002
                                                        AND ShareResult = 0
                                                        AND 1 = DATEDIFF(DAY, CreateTime, GETDATE())
                                                 GROUP BY CreateUserID)AS S
                                               ), 0) AS '分享失败uv（取消分享）' ,
                                        SUM(chitu.pv) AS '回流pv' ,
                                        SUM(chitu.uv) AS '回流uv'
                                FROM    dbo.LE_ADOrderInfo
                                        INNER  JOIN Chitunion_DataSystem2017..chitu_channel_stat AS chitu ON chitu.channel = dbo.LE_ADOrderInfo.OrderCoding
                                WHERE   1=1
                                        --LE_ADOrderInfo.ChannelID = 101003 
                                        AND OrderCoding<>''
                                        AND 1 = DATEDIFF(DAY, chitu.dt, GETDATE());";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 赤兔联盟微信号推广分渠道数据需求—渠道效果数据表
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinChannelResultDay()
        {
            //string SelectSQL = @"
            //                    --默认当天
            //                    DECLARE @TheDay DATE= DATEADD(DAY, -1,
            //                                                  GETDATE());

            //                    SELECT  CONVERT(NVARCHAR(20), @TheDay, 23) AS '日期' ,
            //                            BaseDB.Inviter AS '渠道名称' ,
            //                            ISNULL(NewUser.新关注用户数, 0) '新关注用户数' ,
            //                            ISNULL(OldUser.老关注用户数, 0) '老关注用户数' ,
            //                            ISNULL(AuthorizeUser.注册人数, 0) '注册人数' ,
            //                            ISNULL(Ding.日分发订单量, 0) '日分发订单量' ,
            //                            ISNULL(Task.日分发任务量, 0) '日分发任务量' ,
            //                            ISNULL(FenFa.有效分发用户数, 0) AS '有效分发用户数'
            //                    FROM    ( SELECT Inviter
            //                                FROM
            //                                (
            //                                    SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                              ORDER BY CreateTime DESC
            //                                                             ) rowNum,
            //                                        *
            //                                    FROM dbo.LE_WeiXinUser
            //                                    WHERE Status >= 0
            //                                ) AS A
            //                                WHERE A.rowNum = 1 AND A.Source=3 GROUP BY A.Inviter
            //                            ) AS BaseDB
            //                            LEFT JOIN ( SELECT  Inviter ,
            //                                                COUNT(*) AS '新关注用户数'
            //                                        FROM
            //                                        (
            //                                            SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                                      ORDER BY CreateTime DESC
            //                                                                     ) rowNum,
            //                                                *
            //                                            FROM dbo.LE_WeiXinUser
            //                                            WHERE Status >= 0
            //                                        ) AS A
            //                                        WHERE rowNum = 1 and Source = 3 AND Status=0
            //                                                AND 0 = DATEDIFF(DAY,
            //                                                  CreateTime,
            //                                                  subscribe_time)
            //                                                AND 0 = DATEDIFF(DAY,
            //                                                  CreateTime,
            //                                                  @TheDay)
            //                                        GROUP BY Inviter
            //                                      ) AS NewUser ON NewUser.Inviter = BaseDB.Inviter
            //                            LEFT JOIN ( SELECT  Inviter ,
            //                                                COUNT(*) '老关注用户数'
            //                                         FROM
            //                                        (
            //                                            SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                                      ORDER BY CreateTime DESC
            //                                                                     ) rowNum,
            //                                                *
            //                                            FROM dbo.LE_WeiXinUser
            //                                            WHERE Status >= 0
            //                                        ) AS A
            //                                        WHERE rowNum = 1 and Source = 3 AND Status=0
            //                                                AND 0 < DATEDIFF(DAY,
            //                                                  CreateTime,
            //                                                  subscribe_time)
            //                                                AND 0 = DATEDIFF(DAY,
            //                                                  subscribe_time,
            //                                                  @TheDay)
            //                                        GROUP BY Inviter
            //                                      ) AS OldUser ON OldUser.Inviter = BaseDB.Inviter
            //                            LEFT JOIN ( SELECT  Inviter ,
            //                                                COUNT(*) '注册人数'
            //                                         FROM
            //                                        (
            //                                            SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                                      ORDER BY CreateTime DESC
            //                                                                     ) rowNum,
            //                                                *
            //                                            FROM dbo.LE_WeiXinUser
            //                                            WHERE Status >= 0
            //                                        ) AS A
            //                                        WHERE rowNum = 1 and   Source = 3
            //                                                AND 0 = DATEDIFF(DAY,
            //                                                  AuthorizeTime,
            //                                                  @TheDay)
            //                                        GROUP BY Inviter
            //                                      ) AS AuthorizeUser ON AuthorizeUser.Inviter = BaseDB.Inviter
            //                            LEFT JOIN ( SELECT  U.Inviter ,
            //                                                COUNT(0) AS '日分发订单量'
            //                                        FROM    (SELECT *
            //                                                    FROM
            //                                                    (
            //                                                        SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                                                  ORDER BY CreateTime DESC
            //                                                                                 ) rowNum,
            //                                                            *
            //                                                        FROM dbo.LE_WeiXinUser
            //                                                        WHERE Status >= 0
            //                                                    ) AS A
            //                                                    WHERE rowNum = 1 )
            //                                                AS U
            //                                                JOIN dbo.LE_ADOrderInfo
            //                                                AS D ON U.UserID = D.UserID
            //                                        WHERE   Source = 3
            //                                                AND 0 = DATEDIFF(DAY,
            //                                                  D.CreateTime,
            //                                                  @TheDay)
            //                                        GROUP BY U.Inviter
            //                                      ) AS Ding ON Ding.Inviter = BaseDB.Inviter
            //                            LEFT JOIN ( SELECT  A.Inviter ,
            //                                                COUNT(0) AS '日分发任务量'
            //                                        FROM    ( SELECT
            //                                                  U.Inviter ,
            //                                                  D.TaskID
            //                                                  FROM
            //                                                  (SELECT *
            //                                                    FROM
            //                                                    (
            //                                                        SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                                                  ORDER BY CreateTime DESC
            //                                                                                 ) rowNum,
            //                                                            *
            //                                                        FROM dbo.LE_WeiXinUser
            //                                                        WHERE Status >= 0
            //                                                    ) AS A
            //                                                    WHERE rowNum = 1 )
            //                                                  AS U
            //                                                  JOIN dbo.LE_ADOrderInfo
            //                                                  AS D ON U.UserID = D.UserID
            //                                                  WHERE
            //                                                  Source = 3
            //                                                  AND 0 = DATEDIFF(DAY,
            //                                                  D.CreateTime,
            //                                                  @TheDay)
            //                                                  GROUP BY U.Inviter ,
            //                                                  D.TaskID
            //                                                ) AS A
            //                                        GROUP BY A.Inviter
            //                                      ) AS Task ON Task.Inviter = BaseDB.Inviter
            //                            LEFT JOIN ( SELECT  B.Inviter ,
            //                                                COUNT(0) AS '有效分发用户数'
            //                                        FROM    ( SELECT
            //                                                  U.Inviter ,
            //                                                  U.UserID
            //                                                  FROM
            //                                                  (SELECT *
            //                                                    FROM
            //                                                    (
            //                                                        SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                                                  ORDER BY CreateTime DESC
            //                                                                                 ) rowNum,
            //                                                            *
            //                                                        FROM dbo.LE_WeiXinUser
            //                                                        WHERE Status >= 0
            //                                                    ) AS A
            //                                                    WHERE rowNum = 1 )
            //                                                  AS U
            //                                                  JOIN dbo.LE_ADOrderInfo
            //                                                  AS D ON U.UserID = D.UserID
            //                                                  WHERE
            //                                                  Source = 3
            //                                                  AND 0 = DATEDIFF(DAY,
            //                                                  U.CreateTime,
            //                                                  subscribe_time)
            //                                                  AND U.AuthorizeTime IS NOT NULL
            //                                                  AND 0 = DATEDIFF(DAY,
            //                                                  U.CreateTime,
            //                                                  @TheDay) AND 0 = DATEDIFF(DAY,
            //                                                  D.CreateTime,
            //                                                  @TheDay)
            //                                                  GROUP BY U.Inviter ,
            //                                                  U.UserID
            //                                                ) AS B
            //                                        GROUP BY B.Inviter
            //                                      ) AS FenFa ON FenFa.Inviter = BaseDB.Inviter;";
            // 张立彬修改
            string SelectSQL = @" 
                          DECLARE @TheDay DATE= DATEADD(DAY, -1, GETDATE());

                        SELECT CONVERT(NVARCHAR(20), @TheDay, 23) AS '日期',
                            BaseDB.Inviter AS '渠道名称',
                            ISNULL(NewUser.新关注用户数, 0) '新关注用户数',
                            ISNULL(OldUser.老关注用户数, 0) '老关注用户数',
                            ISNULL(AuthorizeUser.注册人数, 0) '注册人数',
                            ISNULL(Ding.日分发订单量, 0) '日分发订单量',
                            ISNULL(Task.日分发任务量, 0) '日分发任务量',
                            ISNULL(FenFa.有效分发用户数, 0) AS '有效分发用户数'
                        FROM
                            (
                                SELECT Inviter
                                FROM
                                (
                                    SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                              ORDER BY CreateTime 
                                                             ) rowNum,
                                        *
                                    FROM dbo.LE_WeiXinUser
                                ) AS A
                                WHERE A.rowNum = 1
                                      AND A.Source = 3
                                      AND Status = 0
                                GROUP BY A.Inviter
                            ) AS BaseDB
                            LEFT JOIN
                            (
                                SELECT Inviter,
                                    COUNT(*) AS '新关注用户数'
                                FROM
                                (
                                    SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                              ORDER BY CreateTime 
                                                             ) rowNum,
                                        *
                                    FROM dbo.LE_WeiXinUser
                                ) AS A
                                WHERE rowNum = 1
                                      AND Source = 3
                                      AND Status = 0
                                      AND 0 = DATEDIFF(DAY,
                                                  CreateTime,
                                                  subscribe_time
                                                      )
                                      AND 0 = DATEDIFF(DAY,
                                                  CreateTime,
                                                  @TheDay
                                                      )
                                GROUP BY Inviter
                            ) AS NewUser
                                ON NewUser.Inviter = BaseDB.Inviter
                            LEFT JOIN
                            (
                                SELECT Inviter,
                                    COUNT(*) '老关注用户数'
                                FROM
                                (
                                    SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                              ORDER BY CreateTime 
                                                             ) rowNum,
                                        *
                                    FROM dbo.LE_WeiXinUser
                                ) AS A
                                WHERE rowNum = 1
                                      AND Source = 3
                                      AND Status = 0
                                      AND 0 < DATEDIFF(DAY,
                                                  CreateTime,
                                                  subscribe_time
                                                      )
                                      AND 0 = DATEDIFF(
                                                  DAY,
                                                  subscribe_time,
                                                  @TheDay
                                                      )
                                GROUP BY Inviter
                            ) AS OldUser
                                ON OldUser.Inviter = BaseDB.Inviter
                            LEFT JOIN
                            (
                                SELECT Inviter,
                                    COUNT(*) '注册人数'
                                FROM
                                (
                                    SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                              ORDER BY CreateTime
                                                             ) rowNum,
                                        *
                                    FROM dbo.LE_WeiXinUser
                                ) AS A
                                WHERE rowNum = 1
                                      AND A.Source = 3
                                      AND 0 = DATEDIFF(DAY,
                                                  A.CreateTime,
                                                  @TheDay
                                                      )
                                GROUP BY Inviter
                            ) AS AuthorizeUser
                                ON AuthorizeUser.Inviter = BaseDB.Inviter
                            LEFT JOIN
                            (
                                SELECT U.Inviter,
                                    COUNT(0) AS '日分发订单量'
                                FROM
                                    (
                                        SELECT *
                                        FROM
                                        (
                                            SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                      ORDER BY CreateTime DESC
                                                                     ) rowNum,
                                                *
                                            FROM dbo.LE_WeiXinUser
                                            WHERE Status >= 0
                                        ) AS A
                                        WHERE rowNum = 1
                                    ) AS U
                                    JOIN dbo.LE_ADOrderInfo AS D
                                        ON U.UserID = D.UserID
                                WHERE Source = 3
                                      AND 0 = DATEDIFF(DAY,
                                                  D.CreateTime,
                                                  @TheDay
                                                      )
                                GROUP BY U.Inviter
                            ) AS Ding
                                ON Ding.Inviter = BaseDB.Inviter
                            LEFT JOIN
                            (
                                SELECT A.Inviter,
                                    COUNT(0) AS '日分发任务量'
                                FROM
                                (
                                    SELECT U.Inviter,
                                        D.TaskID
                                    FROM
                                        (
                                            SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime DESC
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                                WHERE Status >= 0
                                            ) AS A
                                            WHERE rowNum = 1
                                        ) AS U
                                        JOIN dbo.LE_ADOrderInfo AS D
                                            ON U.UserID = D.UserID
                                    WHERE Source = 3
                                          AND 0 = DATEDIFF(
                                                      DAY,
                                                      D.CreateTime,
                                                      @TheDay
                                                          )
                                    GROUP BY U.Inviter,
                                        D.TaskID
                                ) AS A
                                GROUP BY A.Inviter
                            ) AS Task
                                ON Task.Inviter = BaseDB.Inviter
                            LEFT JOIN
                            (
                                SELECT B.Inviter,
                                    COUNT(0) AS '有效分发用户数'
                                FROM
                                (
                                    SELECT U.Inviter,
                                        U.UserID
                                    FROM
                                    (
                                        SELECT *
                                        FROM
                                        (
                                            SELECT ROW_NUMBER() OVER (PARTITION BY W.UserID
                                                                      ORDER BY W.CreateTime
                                                                     ) rowNum,
                                                W.*
                                            FROM dbo.LE_WeiXinUser W
                                                JOIN dbo.LE_ADOrderInfo D
                                                    ON W.UserID = D.UserID
                                                       AND 0 = DATEDIFF(
                                                                   DAY,
                                                                   D.CreateTime,
                                                                   @TheDay
                                                                       )
                                        ) AS A
                                        WHERE rowNum = 1
                                    ) AS U
                                    WHERE Source = 3
                                          AND 0 = DATEDIFF(
                                                      DAY,
                                                      CreateTime,
                                                      @TheDay
                                                          )
                                    GROUP BY U.Inviter,
                                        U.UserID
                                ) AS B
                                GROUP BY B.Inviter
                            ) AS FenFa
                                ON FenFa.Inviter = BaseDB.Inviter;";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }
        /// <summary>
        /// 赤兔联盟微信号推广分渠道数据需求—新关注粉丝明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinNewAttentionDay()
        {
            string SelectSQL = @"
                                 --默认当天
                                DECLARE @TheDay DATE= DATEADD(DAY,
                                            -1, GETDATE());
                                SELECT    @TheDay AS '日期' ,
                                          Inviter AS '渠道名称' ,
										  U.UserName AS '用户名',
                                          nickname AS '首次关注微信用户昵称',
										  (CASE WHEN U.Source=3006 THEN  CASE W.Status WHEN 0 THEN '已关注' ELSE '取消关注' END ELSE '' END) AS '当前关注状态'
                                FROM      (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                               -- WHERE Status >= 0
                                            ) AS A
                                            WHERE rowNum = 1 ) AS W
								LEFT JOIN dbo.UserInfo AS U ON U.UserID=W.UserID
                                WHERE     W.Source = 3
                                          AND 0 = DATEDIFF(DAY,
                                            W.CreateTime,
                                            subscribe_time)
                                          AND 0 = DATEDIFF(DAY,
                                            W.CreateTime,
                                            @TheDay)
                                ORDER BY  渠道名称;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }
        /// <summary>
        /// 赤兔联盟微信号推广分渠道数据需求—老关注粉丝明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinOldAttentionDay()
        {
            string SelectSQL = @"
                                 --默认当天
                                DECLARE @TheDay DATE= DATEADD(DAY,
                                            -1, GETDATE());
                                SELECT    @TheDay AS '日期' ,
                                          Inviter AS '渠道名称' ,
										  U.UserName AS '用户名',
                                          nickname AS '首次关注微信用户昵称',
										  (CASE WHEN U.Source=3006 THEN  CASE W.Status WHEN 0 THEN '已关注' ELSE '取消关注' END ELSE '' END) AS '当前关注状态'
                                FROM      (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime 
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                               -- WHERE Status >= 0
                                            ) AS A
                                            WHERE rowNum = 1 ) AS W
								LEFT JOIN dbo.UserInfo AS U ON U.UserID=W.UserID
                                WHERE     W.Source = 3
                                          AND 0 < DATEDIFF(DAY,
                                            W.CreateTime,
                                            subscribe_time)
                                          AND 0 = DATEDIFF(DAY,
                                            W.subscribe_time,
                                            @TheDay)
                                ORDER BY  渠道名称;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }
        /// <summary>
        /// 赤兔联盟微信号推广分渠道数据需求—注册用户明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinRegisterUserDay()
        {
            //    string SelectSQL = @"
            //                         --默认当天
            //                        DECLARE @TheDay DATE= DATEADD(DAY, -1,
            //                                                      GETDATE());
            //                        SELECT  @TheDay AS '日期' ,
            //                                Inviter AS '渠道名称' ,
            //		U.UserName AS '用户名',
            //                                nickname AS '注册用户微信昵称',
            //                                (CASE WHEN U.Source=3006 THEN  CASE W.Status WHEN 0 THEN '已关注' ELSE '取消关注' END ELSE '' END) AS '当前关注状态'
            //                        FROM    (SELECT *
            //                                    FROM
            //                                    (
            //                                        SELECT ROW_NUMBER() OVER (PARTITION BY UserID
            //                                                                  ORDER BY CreateTime DESC
            //                                                                 ) rowNum,
            //                                            *
            //                                        FROM dbo.LE_WeiXinUser
            //                                        WHERE Status >= 0
            //                                    ) AS A
            //                                    WHERE rowNum = 1 ) AS W
            //LEFT JOIN dbo.UserInfo AS U ON U.UserID=W.UserID
            //                        WHERE   W.Source = 3
            //                                AND 0 = DATEDIFF(DAY, W.CreateTime,
            //                                                 @TheDay) and 0 = DATEDIFF(DAY, U.CreateTime,
            //                                                 @TheDay)
            //                        ORDER BY 渠道名称;";
            //zhanglb 修改
            string SelectSQL = @"
                                    --默认当天
                                DECLARE @TheDay DATE= DATEADD(DAY, -1,
                                                              GETDATE());
                                SELECT  @TheDay AS '日期' ,
                                        Inviter AS '渠道名称' ,
										U.UserName AS '用户名',
                                        nickname AS '注册用户微信昵称',
                                        (CASE WHEN U.Source=3006 THEN  CASE W.Status WHEN 0 THEN '已关注' ELSE '取消关注' END ELSE '' END) AS '当前关注状态'
                                FROM    (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                            ) AS A
                                            WHERE rowNum = 1 ) AS W
								LEFT JOIN dbo.UserInfo AS U ON U.UserID=W.UserID
                                WHERE   W.Source = 3
                                        AND 0 = DATEDIFF(DAY, W.CreateTime,
                                                         @TheDay) 
                                ORDER BY 渠道名称;";


            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 物料分发（周报） 
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinDispenseForWeek()
        {
            string SelectSQL = @"  
--------------------------2018-03-08--------------导出当前日期,前一个自然周,统计数据--------------
                                            set datefirst 1
                                            DECLARE @currendDate DATE
                                            SET @currendDate=GETDATE()
                                            DECLARE @currentWeekDay INT
                                            select @currentWeekDay=DATEPART(weekday, @currendDate) 
                                            PRINT @currentWeekDay

                                            DECLARE @statDate DATETIME
                                            DECLARE @endDate DATETIME
                                            --SET @statDate=DATEADD(DAY,-6-@currentWeekDay,@currendDate) 
                                            SET @statDate=CONVERT(VARCHAR(10),DATEADD(DAY,-7,GETDATE()), 120)+' 0:0:0'
                                            SET @endDate=CONVERT(VARCHAR(10),DATEADD(DAY,-1,GETDATE()), 120)+' 23:59:59'
                                            --PRINT @statDate
                                            --PRINT @endDate


                                            --DECLARE @抓取文章数 INT
                                            --DECLARE @机洗入库文章数 INT


                                            SELECT a.Date, SUM(a.ct) AS [抓取文章数] INTO #temp1 FROM (
                                            SELECT 
	                                            CONVERT(VARCHAR(10),CreateTime, 120) AS Date,
	                                            COUNT(*) AS ct 
                                            FROM BaseData2017.dbo.Weixin_ArticleInfo WITH(NOLOCK)
                                            WHERE CreateTime BETWEEN @statDate AND @endDate
                                            GROUP BY CONVERT(VARCHAR(10),CreateTime, 120)
                                            UNION ALL
                                            SELECT 
                                            CONVERT(VARCHAR(10), CreateTime, 120) AS Date,
                                            COUNT(*) AS ct 
                                            FROM BaseData2017.dbo.ArticleCrawler WITH(NOLOCK)
                                            WHERE CreateTime BETWEEN @statDate AND @endDate
                                            GROUP BY CONVERT(VARCHAR(10),CreateTime, 120)
                                            ) AS a
                                            GROUP BY a.Date

                                            --SELECT @抓取文章数


                                            SELECT b.Date,SUM(b.ct) AS [机洗入库文章数] INTO #temp2 FROM (
                                                    SELECT 
			                                            CONVERT(VARCHAR(10), ai.CreateTime, 120) AS Date,
			                                            COUNT(DISTINCT ai.RecID) AS ct
		                                            FROM BaseData2017.dbo.ArticleInfo AS ai WITH(NOLOCK)
		                                            --JOIN MaterielExtend AS me ON me.ArticleID=ai.RecID AND me.ArticleFrom=69002
		                                            WHERE ai.Resource IN (1) AND ai.XyAttr=1
		                                            AND ai.DataId IN (
			                                            SELECT Number FROM Chitunion_OP2017.dbo.ChannelAccount WHERE Status=0 AND ChannelID=1
		                                            )
		                                            AND ai.CreateTime BETWEEN @statDate AND @endDate
		                                            GROUP BY CONVERT(VARCHAR(10),ai.CreateTime, 120)
		
		                                            UNION ALL
		
		                                            SELECT
			                                            CONVERT(VARCHAR(10), ai.CreateTime, 120) AS Date,
			                                            COUNT(DISTINCT ai.RecID) AS ct
		                                            FROM BaseData2017.dbo.ArticleInfo AS ai WITH(NOLOCK)
		                                            JOIN (
			                                            SELECT ac.Source,ac.Source_ID FROM BaseData2017.dbo.ArticleCrawler AS ac WITH (NOLOCK)
			                                            GROUP BY ac.Source,ac.Source_ID
			                                            HAVING ac.Source_ID IS NOT NULL AND ac.Source IS NOT NULL
		                                            ) AS s ON s.Source_ID=ai.Resource
		                                            --JOIN MaterielExtend AS me ON me.ArticleID=ai.RecID AND me.ArticleFrom=69002
		                                            WHERE --ai.Resource IN (7,8,9,10,11,12,13,17) AND 
		                                            ai.XyAttr=1
		                                            AND ai.CreateTime BETWEEN @statDate AND @endDate
		                                            GROUP BY CONVERT(VARCHAR(10),ai.CreateTime, 120)
		                                            ) AS b
                                            GROUP BY b.Date
                                            --SELECT CONVERT(VARCHAR(10),@statDate)+'至'+CONVERT(VARCHAR(10),DATEADD(DAY,7,@statDate)) AS [时间周期],
                                            --@抓取文章数 AS [抓取文章数],@机洗入库文章数 AS [机洗入库文章数]


                                            --SELECT TOP 100 channel,pv,uv FROM Chitunion_DataSystem2017.dbo.chitu_channel_stat

                                             -- 将开始时间赋值给临时变量
                                                --DECLARE @StartTime DATETIME
	                                            --DECLARE @EndTime DATETIME
	                                            --SELECT @StartTime=MIN(BeginTime),@EndTime=MAX(EndTime) FROM Chitunion2017.dbo.LE_TaskInfo;
                                             --   SET @StartTime = '2018-01-02'
	                                            --SET @EndTime=DATEADD(DAY,7,@statDate);

	                                            DECLARE @TempTime DATETIME
	                                            SET @TempTime=@statDate
    
                                                --创建字段为年、月、日的临时表#TMP
                                                CREATE TABLE #TMP
                                                ( 
                                                    MyDate DATE
                                                )
    
                                                --将给时间段内的日期插入临时表#TMP
                                                WHILE(@TempTime BETWEEN @statDate AND @endDate)
                                                    BEGIN
                                                        INSERT INTO #TMP VALUES(
                                                             @TempTime
                                                        )
                                                        SET @TempTime = DATEADD(DAY,1,@TempTime)
                                                    END

	                                            SELECT T1.*,T2.赤兔任务库存量,T3.被领取的任务数,T3.订单数,T3.赤兔联盟分发带来的pv,T3.赤兔联盟分发带来的uv, ISNULL(T3.[线索量（不排重）],0) AS [线索量（不排重）] 
	                                            , ISNULL(T4.[无渠道参数_线索量（不排重）],0) AS [无渠道参数_线索量（不排重）] 
	                                            INTO #temp3
	                                            FROM (

	                                            SELECT  TMP.MyDate '日期' ,
			                                            COUNT(DISTINCT ME.MaterielID) '封装物料数' ,
			                                            COUNT(DISTINCT MC1.MaterielID) '推送物料数-汽车大全' ,
			                                            COUNT(DISTINCT MC2.MaterielID) '推送物料数-赤兔联盟' ,--,COUNT(DISTINCT TA.RecID) '赤兔任务库存量'
														(SELECT COUNT(DISTINCT MC3.MaterielID) FROM Chitunion_OP2017.dbo.MaterielChannel MC3 WHERE MC3.ChannelType = 73003001 AND CONVERT(DATE,MC3.CreateTime)=TMP.MyDate) '推送物料数-汽车大全（独立）' ,
														(SELECT COUNT(DISTINCT MC4.MaterielID) FROM Chitunion_OP2017.dbo.MaterielChannel MC4 WHERE MC4.ChannelType = 73004001 AND CONVERT(DATE,MC4.CreateTime)=TMP.MyDate) '推送物料数-赤兔联盟（独立）' 
			                                            --COUNT(DISTINCT MC4.MaterielID) '推送物料数-赤兔联盟（独立）'
	                                            FROM     #TMP TMP 
												        LEFT JOIN (SELECT MaterielID,CONVERT(VARCHAR(10),CreateTime,120) AS CreateDate FROM Chitunion_OP2017.dbo.MaterielExtend WHERE Status=0) ME ON ME.CreateDate=TMP.MyDate
			                                            LEFT JOIN Chitunion_OP2017.dbo.MaterielChannel MC1 ON MC1.MaterielID = ME.MaterielID
																                                              AND MC1.ChannelType = 73003001
			                                            LEFT JOIN Chitunion_OP2017.dbo.MaterielChannel MC2 ON MC2.MaterielID = ME.MaterielID
																                                              AND MC2.ChannelType = 73004001
			                                            LEFT JOIN Chitunion2017.dbo.LE_TaskInfo TA ON TA.MaterialID = ME.MaterielID
			                                            LEFT JOIN Chitunion2017.dbo.LE_ADOrderInfo AD ON AD.TaskID = TA.RecID
			                                            LEFT JOIN Chitunion_DataSystem2017.dbo.chitu_channel_stat st ON st.channel = AD.OrderCoding
	                                            GROUP BY TMP.MyDate
	                                            )T1 
	                                            LEFT JOIN 

	                                            (
	                                            SELECT  TMP.MyDate '日期' ,
			                                            (SELECT COUNT(1) FROM Chitunion2017.dbo.LE_TaskInfo WHERE TMP.MyDate BETWEEN BeginTime AND EndTime) '赤兔任务库存量'
	                                            FROM     #TMP TMP
	                                            )T2 ON T1.日期=T2.日期
	                                            LEFT JOIN
	                                            (
	                                            SELECT  TMP.MyDate '日期' ,
		                                            --COUNT(DISTINCT TA.RecID) '赤兔任务库存量' ,
                                                    COUNT(DISTINCT AD.TaskID) '被领取的任务数' ,
                                                    COUNT(DISTINCT AD.RecID) '订单数' ,
                                                    SUM(st.pv) '赤兔联盟分发带来的pv' ,
                                                    SUM(st.uv) '赤兔联盟分发带来的uv' ,
                                                    SUM(st.orders) '线索量（不排重）'
	                                            FROM    #TMP TMP LEFT JOIN (SELECT TaskID,RecID,CONVERT(VARCHAR(10),CreateTime,120) AS CreateDate,OrderCoding FROM Chitunion2017.dbo.LE_ADOrderInfo) AD ON AD.CreateDate=TMP.MyDate
			                                            LEFT JOIN Chitunion2017.dbo.LE_TaskInfo TA ON TA.RecID=AD.TaskID
			                                            LEFT JOIN Chitunion_DataSystem2017.dbo.chitu_channel_stat st ON st.channel = AD.OrderCoding AND AD.OrderCoding!=''
	                                            WHERE   1=1 --ME.Status = 0
	                                            GROUP BY TMP.MyDate 


	 



	                                            --SELECT * FROM Chitunion_DataSystem2017.dbo.chitu_channel_stat st
	                                            --WHERE st.dt BETWEEN '2018-03-22' AND '2018-03-29'
	                                            --AND st.channel=''
	                                            --AND st.orders>0

	                                            )T3 ON T3.日期 = T1.日期
	                                            LEFT JOIN
	                                            (
		                                            SELECT  TMP.MyDate '日期' ,
		                                            --COUNT(DISTINCT TA.RecID) '赤兔任务库存量' ,
                                              --      COUNT(DISTINCT AD.TaskID) '被领取的任务数' ,
                                              --      COUNT(DISTINCT AD.RecID) '订单数' ,
                                              --      SUM(st.pv) '赤兔联盟分发带来的pv' ,
                                              --      SUM(st.uv) '赤兔联盟分发带来的uv' ,
                                              --      SUM(st.orders) '线索量（不排重）',
		                                            SUM(st2.orders) '无渠道参数_线索量（不排重）'
	                                            FROM    #TMP TMP 
			                                            --LEFT JOIN (SELECT TaskID,RecID,CONVERT(VARCHAR(10),CreateTime,120) AS CreateDate,OrderCoding FROM Chitunion2017.dbo.LE_ADOrderInfo) AD ON AD.CreateDate=TMP.MyDate
			                                            --LEFT JOIN Chitunion2017.dbo.LE_TaskInfo TA ON TA.RecID=AD.TaskID
			                                            --LEFT JOIN Chitunion_DataSystem2017.dbo.chitu_channel_stat st ON st.channel = AD.OrderCoding
			                                            LEFT JOIN Chitunion_DataSystem2017.dbo.chitu_channel_stat st2 ON st2.dt=TMP.MyDate  AND st2.channel = ''
	                                            WHERE   1=1	--AND TMP.MyDate='2018-03-22'
	                                            --ME.Status = 0
	                                            --AND st.dt=
	                                            GROUP BY TMP.MyDate
		
	                                            )T4 ON T4.日期 = T1.日期
                                            --
                                            --	SELECT * FROM #TMP
                                            --	SELECT * FROM #temp1
                                            --SELECT * FROM #temp2
                                            --SELECT * FROM #temp3

                                            SELECT a.Date,a.抓取文章数 ,
	                                               b.机洗入库文章数,
	                                               c.封装物料数,
	                                               c.[推送物料数-汽车大全],
	                                               c.[推送物料数-赤兔联盟],
												   c.[推送物料数-汽车大全（独立）],
												   c.[推送物料数-赤兔联盟（独立）],
	                                               c.[赤兔任务库存量],
	                                               c.被领取的任务数,
	                                               c.订单数,
	                                               c.赤兔联盟分发带来的pv,
	                                               c.赤兔联盟分发带来的uv,
	                                               c.[线索量（不排重）],
	                                               c.[无渠道参数_线索量（不排重）]
                                            FROM #temp1 AS a
                                            LEFT JOIN #temp2 AS b ON a.Date=b.Date
                                            LEFT JOIN #temp3 AS c ON c.[日期]=b.Date
                                            ORDER BY a.Date DESC


                                            DROP TABLE #TMP
                                            DROP TABLE #temp1
                                            DROP TABLE #temp2
                                            DROP TABLE #temp3
";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 类目选择的分类数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinCategorySelectDay()
        {
            string SelectSQL = @"
                                SELECT '微信' AS '产品',
                                    CONVERT(NVARCHAR(20), DATEADD(DAY, -1, GETDATE()), 23) AS '时间',
                                    F.NumLine AS '选择分类数量',
                                    ISNULL(N.选择的人数, 0) AS '选择的人数'
                                FROM dbo.fn_GetNumber(5, 28) AS F
                                    LEFT JOIN
                                     (
                                         SELECT A.CountSum,
                                             COUNT(0) '选择的人数'
                                         FROM
                                         (
                                             SELECT UserID,
                                                 COUNT(0) CountSum
                                             FROM dbo.LE_WXUserScene
                                             WHERE Status = 0
                                                   AND 0< DATEDIFF(DAY,
                                                               CreateTime,
                                                               GETDATE()
                                                                   )
                                             GROUP BY UserID
                                         ) AS A
                                         GROUP BY A.CountSum
                                     ) AS N
                                        ON F.NumLine = N.CountSum;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }
        /// <summary>
        /// 类目被选择数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinGetCategoryUserDay()
        {
            string SelectSQL = @"
                                SELECT '微信' AS '产品',
                                    CONVERT(NVARCHAR(20), DATEADD(DAY, -1, GETDATE()), 23) AS '时间',
                                    D.SceneName AS '任务分类',
                                    UserCount AS '被选中人数'
                                FROM
                                    (
                                        SELECT SceneID,
                                            COUNT(0) UserCount
                                        FROM dbo.LE_WXUserScene
                                        WHERE Status = 0
                                              AND 1<= DATEDIFF(DAY, CreateTime, GETDATE())
                                        GROUP BY SceneID
                                    ) AS A
                                    INNER JOIN Chitunion_OP2017.dbo.DictScene AS D
                                        ON A.SceneID = D.SceneID
                                ORDER BY A.SceneID;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 分发用户明细数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataDistributionDay()
        {
            string SelectSQL = @"
                                SELECT  '微信' AS '产品' ,
                                        RecID AS '订单ID' ,
                                        TaskID AS '任务ID' ,
                                        DC.DictName AS '平台' ,
                                        P.ChannelName AS '渠道名称' ,
										p.ChannelCode AS '渠道编码',
                                        U.UserName AS '创建人' ,
                                        AD.BeginTime AS '订单生成时间' ,
                                        AD.EndTime AS '订单结束时间' ,
                                        ISNULL(T.PV, 0) AS '回流实际pv' ,
                                        ISNULL(T.UV, 0) AS '回流实际uv' ,
                                        ISNULL(T.IP, 0) AS '结算IP' ,
                                        ISNULL(B_IP.Black_IP, 0) AS '防作弊命中ip' ,
                                        AD.CPCUnitPrice AS '点击单价' ,
                                        AD.TotalAmount AS '实际收益' ,
                                        ISNULL(T.OrderNum, 0) AS '线索数量' ,
                                        AD.CPLUnitPrice AS '线索单价' ,
                                        D.DictName AS '订单类型'
                                FROM    dbo.LE_ADOrderInfo AS AD
                                        JOIN dbo.DictInfo AS D ON D.DictId = AD.OrderType
                                        LEFT JOIN dbo.DictInfo AS DC ON DC.DictId = AD.ChannelID
                                        LEFT JOIN dbo.LE_PromotionChannel_Dict AS P ON P.DictID = AD.PromotionChannelID
                                        LEFT JOIN dbo.UserInfo AS U ON U.UserID = AD.UserID
                                        LEFT JOIN ( SELECT  SUM(PVCount) PV ,
                                                            SUM(UVCount) UV ,
                                                            SUM(CPCCount) IP ,
                                                            SUM(CPLCount) OrderNum ,
                                                            OrderID
                                                    FROM    dbo.LE_AccountBalance
                                                    WHERE   0 < DATEDIFF(DAY, StatisticsTime, GETDATE())
                                                    GROUP BY OrderID
                                                  ) AS T ON T.OrderID = AD.RecID
                                        LEFT JOIN ( SELECT  COUNT(0) AS Black_IP ,
                                                            channel
                                                    FROM    dbo.LE_chitu_blackip_stat
                                                    WHERE   0 < DATEDIFF(DAY, dt, GETDATE())
                                                    GROUP BY channel
                                                  ) AS B_IP ON AD.OrderCoding = B_IP.channel
                                WHERE   1 = DATEDIFF(DAY, EndTime, GETDATE()) and  AD.OrderCoding<>''
                                        --AND AD.ChannelID = 101003
                                        AND AD.Status = 193002;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 物料领取明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataMaterialCollectingDay()
        {
            string SelectSQL = @"
                                SELECT AD.EndTime AS '订单结束时间',
                                    AD.BeginTime AS '订单创建时间',
                                    W.ArticleID AS '文章ID',
                                    W.FinalScore AS '文章评分',
                                    T.CreateTime AS '推送时间',
                                    T.RecID AS '任务ID',
									U.UserName AS '清洗人用户名',
                                    T.MaterialID AS '物料ID',
                                    T.TaskName AS '物料标题',
                                    T.MaterialUrl AS '物料链接',
                                    D.SceneName AS '分类',
                                    P.ChannelName AS '渠道名称',
									P.ChannelCode AS '渠道编码',
                                    AD.UserCount AS '分发人数',
                                    '' AS '物料曝光量',
                                    AD.PVCount AS '回流总PV',
                                    AD.UVCount AS '回流总UV',
                                    '' AS '详情页分类广告pv',
                                    '' AS '详情页分类广告uv',
                                    '' AS '详情，页广告订单',
                                    M.organic_pv AS '物料预览PV',
                                    M.organic_uv AS '物料预览UV',
                                    FX.pv AS '立即分享pv',
                                    FX.uv AS '立即分享uv',
                                    '' AS '回流pv（微信）',
                                    '' AS '回流uv（微信）',
                                    DJ.pv AS '详情页底部列表引流pv',
                                    DJ.uv AS '详情页底部列表引流uv',
                                    '' AS '回流pv（其他）',
                                    '' AS '回流uv（其他）',
                                    XJ.pv AS '询价入口pv',
                                    XJ.uv AS '询价入口uv',
                                    '' AS '询价入口订单'
                                FROM
                                    (
                                        SELECT EndTime,LE_AD.BeginTime,
                                            PromotionChannelID,
                                            TaskID,
                                            COUNT(0) AS UserCount,
                                            SUM(LE_AB.PVCount) AS PVCount,
                                            SUM(LE_AB.UVCount) AS UVCount
                                        FROM dbo.LE_ADOrderInfo AS LE_AD
                                            LEFT JOIN
                                             (
                                                 SELECT OrderID,
                                                     SUM(PVCount) AS PVCount,
                                                     SUM(UVCount) AS UVCount
                                                 FROM dbo.LE_AccountBalance
                                                 GROUP BY OrderID
                                             ) AS LE_AB
                                                ON LE_AD.RecID = LE_AB.OrderID
                                        WHERE LE_AD.Status = 193002
                                              --AND LE_AD.ChannelID = 101003
                                              AND 1 = DATEDIFF(DAY, EndTime, GETDATE())
                                        GROUP BY EndTime,LE_AD.BeginTime,
                                            PromotionChannelID,
                                            TaskID
                                    ) AS AD
                                    LEFT JOIN dbo.LE_TaskInfo AS T
                                        ON T.RecID = AD.TaskID
                                    LEFT JOIN Chitunion_OP2017..MaterielExtend AS ME
                                        ON ME.MaterielID = T.MaterialID
                                    LEFT JOIN Chitunion_OP2017..AccountArticle AS AA
                                        ON AA.ArticleID = ME.ArticleID
                                    LEFT JOIN dbo.UserInfo AS U
                                        ON U.UserID = AA.ReceiveCleanMan
                                    LEFT JOIN Chitunion_OP2017.dbo.DictScene AS D
                                        ON D.SceneID = T.CategoryID
                                    LEFT JOIN
                                    (
                                        SELECT material_id,
                                            SUM(organic_pv) AS organic_pv,
                                            SUM(organic_uv) AS organic_uv
                                        FROM Chitunion_DataSystem2017..chitu_material_stat
                                        GROUP BY material_id
                                    ) AS M
                                        ON CAST(T.MaterialID AS VARCHAR(100)) = M.material_id
                                    LEFT JOIN
                                    (
                                        SELECT material_id,
                                            SUM(pv) AS pv,
                                            SUM(uv) AS uv
                                        FROM Chitunion_DataSystem2017..chitu_click_stat
                                        WHERE click_site = 'fenxiang_button'
                                        GROUP BY material_id
                                    ) AS FX
                                        ON FX.material_id = T.MaterialID
                                    LEFT JOIN
                                    (
                                        SELECT material_id,
                                            SUM(pv) AS pv,
                                            SUM(uv) AS uv
                                        FROM Chitunion_DataSystem2017..chitu_click_stat
                                        WHERE click_site = 'cttask_list'
                                        GROUP BY material_id
                                    ) AS DJ
                                        ON DJ.material_id = T.MaterialID
                                    LEFT JOIN
                                    (
                                        SELECT material_id,
                                            SUM(pv) AS pv,
                                            SUM(uv) AS uv
                                        FROM Chitunion_DataSystem2017..chitu_click_stat
                                        WHERE click_site = 'xunjia'
                                        GROUP BY material_id
                                    ) AS XJ
                                        ON XJ.material_id = T.MaterialID
                                    LEFT JOIN dbo.LE_PromotionChannel_Dict AS P
                                        ON P.DictID = AD.PromotionChannelID
                                    LEFT JOIN
                                    (
                                        SELECT ME.MaterielID,
                                            ME.ArticleID,
                                            AA.FinalScore
                                        FROM Chitunion_OP2017..MaterielExtend ME
                                            LEFT JOIN Chitunion_OP2017..AccountArticle AA
                                                ON AA.ArticleID = ME.ArticleID
                                    ) AS W
                                        ON W.MaterielID = T.MaterialID
                                ORDER BY T.MaterialID;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 物料详情
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataMaterialDetailDay()
        {
            string SelectSQL = @"
                                SELECT CONVERT(NVARCHAR(20), DATEADD(DAY, -1, GETDATE()), 23) AS '时间',
                                    D.SceneName AS '任务分类',
                                    SUM(M.organic_pv) AS '详情页pv',
                                    SUM(M.organic_uv) AS '详情页uv'
                                FROM dbo.LE_TaskInfo AS T
                                    LEFT JOIN Chitunion_OP2017.dbo.DictScene AS D
                                        ON D.SceneID = T.CategoryID
                                    LEFT JOIN Chitunion_DataSystem2017..chitu_material_stat AS M
                                        ON M.material_id = CAST(T.MaterialID AS VARCHAR(50))
                                WHERE T.Status = 194001
                                      AND 1 = DATEDIFF(DAY, M.dt, GETDATE())
                                GROUP BY D.SceneName
                                ORDER BY D.SceneName;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 抢单赚钱登录用户明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticUserLoginLogDay()
        {
            string SelectSQL = @"
                                SELECT CAST(W.LastUpdateTime AS DATE) AS '登陆时间',
                                    U.UserName AS '用户名'
                                FROM LE_WeiXinVisvit_Log AS W
                                    JOIN dbo.UserInfo AS U
                                        ON W.UserID = U.UserID
                                WHERE 1 = DATEDIFF(DAY, W.LastUpdateTime, GETDATE()) AND W.Url LIKE '%index.html%'
                                GROUP BY CAST(W.LastUpdateTime AS DATE),
                                    U.UserName;";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 邀请有礼用户明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticInviteRecordDay()
        {
            string SelectSQL = @"
                                SELECT CAST(IR.InviteTime AS DATE) AS '日期',U.UserName AS '邀请人',T.UserName AS '被邀请人', (CASE WX.Status
                                                 WHEN 0 THEN
                                                     '已关注'
                                                 ELSE
                                                     '取消关注'
                                             END) AS '被邀请人状态'  FROM LE_InviteRecord AS IR
                                LEFT JOIN dbo.UserInfo AS U ON IR.InviteUserID=U.UserID
                                LEFT JOIN dbo.UserInfo AS  T ON IR.BeInvitedUserID=T.UserID
                                LEFT JOIN (SELECT *
                                            FROM
                                            (
                                                SELECT ROW_NUMBER() OVER (PARTITION BY UserID
                                                                          ORDER BY CreateTime DESC
                                                                         ) rowNum,
                                                    *
                                                FROM dbo.LE_WeiXinUser
                                                WHERE Status >= 0
                                            ) AS A
                                            WHERE rowNum = 1 ) AS WX ON WX.UserID=T.UserID 
                                WHERE 1=DATEDIFF(DAY,IR.InviteTime,GETDATE()) AND  1=DATEDIFF(DAY,T.CreateTime,GETDATE()) ORDER BY IR.InviteUserID ";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 分类文章数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticCategoryArticleDay()
        {
            string SelectSQL = @"
                                SELECT  CAST(DATEADD(DAY,-1,GETDATE()) AS DATE) AS '日期',RecID AS '任务ID' ,
                                        D.SceneName AS '分类',
                                        T.MaterialID AS '物料ID',
                                        T.TaskName AS '文章标题',
                                        T.MaterialUrl AS '文章链接',
		                                ISNULL(CS.organic_pv,0) AS '阅读UV',
		                                ISNULL(CS.organic_uv,0) AS '阅读PV'
                                FROM    dbo.LE_TaskInfo AS T
                                        LEFT JOIN Chitunion_OP2017.dbo.DictScene AS D ON D.SceneID = T.CategoryID
                                        LEFT JOIN ( SELECT  *
                                                    FROM    Chitunion_DataSystem2017..chitu_material_stat
                                                    WHERE   1 = DATEDIFF(DAY, dt, GETDATE())
                                                  ) AS CS ON CS.material_id = CAST(T.MaterialID AS VARCHAR(50))
                                WHERE   0 <= DATEDIFF(DAY, DATEADD(DAY, -1, GETDATE()), T.EndTime); ";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }
        /// <summary>
        /// 个人信息页面数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticUserInfoDay()
        {
            string SelectSQL = @"
                                SELECT CAST(DATEADD(DAY, -1, GETDATE()) AS DATE) AS '日期',
                                    0 AS '页面PV',
                                    0 AS '页面UV',
                                    (
                                        SELECT COUNT(0)
                                        FROM dbo.UserInfo AS U
                                            LEFT JOIN dbo.UserDetailInfo AS UD
                                                ON UD.UserID = U.UserID
                                            LEFT JOIN dbo.LE_UserBankAccount AS UB
                                                ON UB.UserID = U.UserID
                                        WHERE UD.Status = 2
                                              AND U.Mobile IS NOT NULL
                                              AND UB.UserID IS NOT NULL
                                              AND 1 < = DATEDIFF(DAY, U.CreateTime, GETDATE())
                                    ) AS '累计完善信息人数',
                                    (
                                        SELECT COUNT(0)
                                        FROM dbo.UserInfo AS U
                                            LEFT JOIN dbo.UserDetailInfo AS UD
                                                ON UD.UserID = U.UserID
                                            LEFT JOIN dbo.LE_UserBankAccount AS UB
                                                ON UB.UserID = U.UserID
                                        WHERE UD.Status = 2
                                              AND U.Mobile IS NOT NULL
                                              AND UB.UserID IS NOT NULL
                                              AND 1 = DATEDIFF(DAY, U.CreateTime, GETDATE())
                                    ) AS '完善信息人数';";

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SelectSQL).Tables[0];
        }

        /// <summary>
        /// 经纪人周数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticBrokerForWeek()
        {
            string SQL = @"
                            CREATE TABLE #Temp_ViewStatMaterile
                            (
                                dt DATE,
                                推送给汽车大全的物料数 INT
                                    DEFAULT 0,
                                推送给经纪人的物料量 INT
                                    DEFAULT 0, --分发给经纪人的物料数
                                物料覆盖人数 INT
                                    DEFAULT 0, --推送覆盖到的经纪人数
                                ShareCount INT
                                    DEFAULT 0, --转发次数（1次）
                                分发量 INT
                                    DEFAULT 0, --经纪人一次转发物料量
                                分发人数 INT
                                    DEFAULT 0, --产生一次转发的经纪人数
                                转发物料数 INT
                                    DEFAULT 0, --产生转发的物料数
                                PV BIGINT
                                    DEFAULT 0,
                                UV BIGINT
                                    DEFAULT 0,
                                Orders INT
                                    DEFAULT 0, --订单
                                Phones INT
                                    DEFAULT 0  --独立手机号
                            );

                            TRUNCATE TABLE #Temp_ViewStatMaterile;

                            DECLARE @BeginDate DATE, @EndDate DATE;

                            SET @BeginDate = DATEADD(DAY, -7, GETDATE());

                            SET @EndDate = GETDATE();


                            --SELECT  @EndDate;

                            DECLARE @i INT = 0,
                                @forOffset INT = 0;
                            SET @forOffset = DATEDIFF(DAY, @BeginDate, @EndDate);
                            --SELECT @forOffset



                            WHILE @i < @forOffset
                            BEGIN
                                INSERT INTO #Temp_ViewStatMaterile
                                (   dt,
                                    推送给汽车大全的物料数,
                                    推送给经纪人的物料量,
                                    物料覆盖人数,
                                    ShareCount,
                                    分发量,
                                    分发人数,
                                    转发物料数,
                                    PV,
                                    UV,
                                    Orders,
                                    Phones
                                )
                                VALUES
                                (   DATEADD(DAY, @i, @BeginDate), -- dt - date
                                    0,                            -- 推送给汽车大全的物料数 - int
                                    0,                            -- 推送给经纪人的物料量 - int
                                    0,                            -- 物料覆盖人数 - int
                                    0,                            --转发次数（1次）
                                    0,                            -- 分发量 - int
                                    0,                            -- 分发人数 - int
                                    0,                            -- 转发物料数 - int
                                    0,                            -- PV - bigint
                                    0,                            -- UV - bigint
                                    0,                            -- Orders - int
                                    0                             -- Phones - int
                                );

                                SET @i = @i + 1;
                            END;

                            --SELECT * FROM #Temp_ViewStatMaterile

                            SELECT TP.dt,
                                T1.MaterielCount AS 推送给汽车大全的物料数,
                                T2.MaterielCount AS 推送给经纪人的物料量,
                                T3.broker_idCount AS 物料覆盖人数,
                                T4.ShareCount AS [转发次数（1次）],
                                T4.MaterielCount AS 分发量,
                                T5.broker_idCount2 AS 分发人数,
                                T6.MaterielCount AS 转发物料数,
                                T7.PV,
                                T7.UV,
                                T7.Orders,
                                T7.Phones
                            FROM #Temp_ViewStatMaterile AS TP
                                --推送给汽车大全的物料数
                                LEFT JOIN
                                 (
                                     SELECT CONVERT(VARCHAR(10), CreateTime, 120) AS dt,
                                         COUNT(MaterielID) AS MaterielCount
                                     FROM Chitunion_OP2017..MaterielChannel
                                     WHERE ChannelType = 73003001
                                           AND CreateTime >= @BeginDate
                                           AND CreateTime < @EndDate
                                     GROUP BY CONVERT(VARCHAR(10), CreateTime, 120)
                                 ) AS T1
                                    ON T1.dt = TP.dt
                                --分发给经纪人的物料数(@推送给经纪人的物料量)
                                LEFT JOIN
                                 (
                                     SELECT CONVERT(VARCHAR(10), create_time, 120) AS dt,
                                         COUNT(DISTINCT materiel_id) AS MaterielCount
                                     FROM Chitunion_DataSystem2017..chitu_fact_broker_materiel_rela_stat
                                     WHERE create_time >= @BeginDate
                                           AND create_time < @EndDate
                                     GROUP BY CONVERT(VARCHAR(10), create_time, 120)
                                 ) AS T2
                                    ON T2.dt = TP.dt
                                --推送覆盖到的经纪人数(@物料覆盖人数)
                                LEFT JOIN
                                 (
                                     SELECT CONVERT(VARCHAR(10), create_time, 120) AS dt,
                                         COUNT(DISTINCT broker_id) AS broker_idCount
                                     FROM Chitunion_DataSystem2017..chitu_fact_broker_materiel_rela_stat
                                     WHERE create_time >= @BeginDate
                                           AND create_time < @EndDate
                                     GROUP BY CONVERT(VARCHAR(10), create_time, 120)
                                 ) AS T3
                                    ON T3.dt = TP.dt
                                --经纪人一次转发物料量(@分发量)
                                LEFT JOIN
                                 (
                                     SELECT CONVERT(VARCHAR(10), create_time, 120) AS dt,
                                         COUNT(DISTINCT materiel_id) AS MaterielCount,
                                         COUNT(*) AS ShareCount
                                     FROM Chitunion_DataSystem2017..chitu_fact_share_log_stat
                                     WHERE create_time >= @BeginDate
                                           AND create_time < @EndDate
                                           AND level = 1
                                     GROUP BY CONVERT(VARCHAR(10), create_time, 120)
                                 ) AS T4
                                    ON T4.dt = TP.dt
                                --产生一次转发的经纪人数(@分发人数)
                                LEFT JOIN
                                 (
                                     SELECT CONVERT(VARCHAR(10), create_time, 120) AS dt,
                                         COUNT(DISTINCT broker_id) AS broker_idCount2
                                     FROM Chitunion_DataSystem2017..chitu_fact_share_log_stat
                                     WHERE create_time >= @BeginDate
                                           AND create_time < @EndDate
                                           AND level = 1
                                     GROUP BY CONVERT(VARCHAR(10), create_time, 120)
                                 ) AS T5
                                    ON T5.dt = TP.dt
                                --产生转发的物料数(@转发物料数)
                                LEFT JOIN
                                 (
                                     SELECT CONVERT(VARCHAR(10), create_time, 120) AS dt,
                                         COUNT(DISTINCT materiel_id) AS MaterielCount
                                     FROM Chitunion_DataSystem2017..chitu_fact_share_log_stat
                                     WHERE create_time >= @BeginDate
                                           AND create_time < @EndDate
                                     GROUP BY CONVERT(VARCHAR(10), create_time, 120)
                                 ) AS T6
                                    ON T6.dt = TP.dt
                                --经纪人统计物料明细(PV,UV,Orders,Phones)
                                LEFT JOIN
                                 (
                                     SELECT Date,
                                         SUM(PV) AS PV,
                                         SUM(UV) AS UV,
                                         SUM(Orders) Orders,
                                         SUM(Phones) Phones
                                     FROM Chitunion_DataSystem2017..Materiel_StatisticalData
                                     WHERE [Date] >= @BeginDate
                                           AND [Date] < @EndDate
                                     GROUP BY Date
                                 ) AS T7
                                    ON T7.Date = TP.dt
                            ORDER BY TP.dt;

                            DROP TABLE #Temp_ViewStatMaterile;";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL).Tables[0];
        }
    }
}
