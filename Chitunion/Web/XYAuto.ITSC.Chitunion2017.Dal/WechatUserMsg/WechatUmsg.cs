using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WechatUserMsg
{
    public class WechatUmsg : DataBase
    {
        public static readonly WechatUmsg Instance = new WechatUmsg();
        /// <summary>
        /// 查询没有分发订单的用户opendID
        /// </summary>
        /// <returns></returns>
        public DataSet NoOrderOpenIds()
        {
            string strSql = $@"SELECT    A1.CreateTime ,
                                            A1.UserID
                                  INTO      #TEMP
                                  FROM      LE_ADOrderInfo A1 WITH ( NOLOCK )
                                            JOIN ( SELECT   MAX(RecID) RecID ,
                                                            UserID
                                                   FROM     dbo.LE_ADOrderInfo WITH ( NOLOCK )
                                                   GROUP BY UserID
                                                 ) A2 ON A2.UserID = A1.UserID
                                                         AND A2.RecID = A1.RecID;
                           --SELECT    W.openid ,
                 --                           W.nickname ,
                 --                           W.CreateTime
                 --                 FROM      dbo.LE_WeiXinUser W WITH ( NOLOCK )
                 --                           JOIN dbo.#TEMP O ON W.Status = 0
                 --                                               AND W.UserID = O.UserID
                 --                 WHERE     DATEDIFF(DAY, O.CreateTime, GETDATE()) = 8
                 --                 UNION ALL
                 --                 SELECT    W.openid ,
                 --                           W.nickname ,
                 --                           W.CreateTime
                 --                 FROM      dbo.LE_WeiXinUser W WITH ( NOLOCK )
                 --                 WHERE     W.Status = 0
                 --                           AND W.UserID NOT IN ( SELECT    UserID
                 --                                                 FROM      #TEMP )
                 --                           AND DATEDIFF(DAY, W.CreateTime, GETDATE()) = 8;

                 --                 SELECT    W.openid ,
                 --                           W.nickname ,
                 --                           W.CreateTime
                 --                 FROM      dbo.LE_WeiXinUser W WITH ( NOLOCK )
                 --                           JOIN dbo.#TEMP O ON W.Status = 0
                 --                                               AND W.UserID = O.UserID
                 --                 WHERE     DATEDIFF(DAY, O.CreateTime, GETDATE()) = 5
                 --                 UNION ALL
                 --                 SELECT    W.openid ,
                 --                           W.nickname ,
                 --                           W.CreateTime
                 --                 FROM      dbo.LE_WeiXinUser W WITH ( NOLOCK )
                 --                 WHERE     W.Status = 0
                 --                           AND W.UserID NOT IN ( SELECT    UserID
                 --                                                 FROM      #TEMP )
                 --                           AND DATEDIFF(DAY, W.CreateTime, GETDATE()) = 5;
                                  SELECT    W.openid ,
                                            W.nickname ,
                                            W.CreateTime
                                  FROM      dbo.LE_WeiXinUser W WITH ( NOLOCK )
                                  WHERE     W.Status = 0
                                            AND W.UserID NOT IN ( SELECT    UserID
                                                                  FROM      #TEMP )
                                            AND DATEDIFF(HOUR, W.CreateTime, GETDATE()) <=24;";
            //string strSql = $@"SELECT openid,nickname,CreateTime  FROM dbo.LE_WeiXinUser WHERE Status=0 AND nickname='三分苦咖啡'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// 查询一周内订单收益大于0的用户OpenId
        /// </summary>
        /// <returns></returns>
        public DataTable OrderWeekProfitOpenIds()
        {
            string strSql = $@"SELECT  W.openid ,
                            W.nickname ,
                            SUM(AB.IncomePrice) TotalMoney ,
                            ( CONVERT(VARCHAR(10), DATEADD(D, -7, GETDATE()), 23) + '至'
                              + CONVERT(VARCHAR(10), DATEADD(D, -1, GETDATE()), 23) ) TimeSlot
                    FROM    dbo.LE_IncomeDetail AB WITH ( NOLOCK )
                            JOIN dbo.v_LE_WeiXinUser W WITH ( NOLOCK ) ON AB.UserID = W.UserID
                                                                        AND W.Status = 0
                    WHERE   AB.IncomePrice > 0
                            AND DATEDIFF(DAY, AB.IncomeTime, GETDATE()) BETWEEN 1 AND 7
                    GROUP BY W.openid ,
                            W.nickname;";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public DataTable GetAllWechatUser()
        {
            string strSql = $@"SELECT openid,nickname  FROM dbo.LE_WeiXinUser WHERE Status=0";
            //string strSql = $@"SELECT openid,nickname  FROM dbo.LE_WeiXinUser WHERE Status=0 AND openid='oSuvAwY5EMAN3PEkrw71JilXNiEA'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 查询“1元提现活动”新用户近2天收益数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetWechatUserBy1YuanTx(string startDate, string endDate)
        {
            string strSql = $@"with 
CTE_WxUser as 
( 
	SELECT WeiXinUserID,UserID,unionid,nickname,openid,UserType,CreateTime
	FROM dbo.LE_WeiXinUser
	WHERE unionid IN (
		SELECT a.unionid FROM (
		SELECT 
			   wxu.unionid,
			   (CASE WHEN wxu.CreateTime BETWEEN '{StringHelper.SqlFilter(startDate)} 0:0:0' AND '{StringHelper.SqlFilter(endDate)} 0:0:0' THEN 1
			   ELSE 0 END) AS flag
		FROM dbo.LE_WeiXinUser AS wxu
		JOIN dbo.UserInfo AS ui ON wxu.UserID=ui.UserID
		) AS a
		GROUP BY a.unionid
		HAVING SUM(a.flag)=COUNT(a.flag)
	)
	AND subscribe=1 AND Status=0
)
SELECT wu.*,wi.AppID,wi.Domain,
	   t2.Date,t2.Price
FROM (
	SELECT 
		t.*,
		ROW_NUMBER() OVER (PARTITION BY t.UserID ORDER BY t.Date DESC) AS R
	FROM (
		SELECT  
			   wu.UserID,
			   CONVERT(VARCHAR(10),isc.IncomeTime,120) AS Date,
			   SUM(CASE WHEN isc.IncomeTime >= DATEADD(DAY,-1,CONVERT(VARCHAR(10),GETDATE(),120))
		AND isc.IncomeTime < DATEADD(DAY,1,CONVERT(VARCHAR(10),GETDATE(),120)) THEN isc.IncomePrice ELSE NULL END) AS Price
			   --SUM(isc.IncomePrice) 
		FROM CTE_WxUser AS wu
		LEFT JOIN dbo.LE_IncomeDetail AS isc ON isc.UserID=wu.UserID
		GROUP BY wu.UserID,CONVERT(VARCHAR(10),isc.IncomeTime,120)
		HAVING (CONVERT(VARCHAR(10),isc.IncomeTime,120) BETWEEN DATEADD(DAY,-1,CONVERT(VARCHAR(10),GETDATE(),120))
		AND DATEADD(DAY,1,CONVERT(VARCHAR(10),GETDATE(),120))) OR CONVERT(VARCHAR(10),isc.IncomeTime,120) IS NULL
	) AS t
) AS t2 
JOIN CTE_WxUser AS wu ON wu.UserID = t2.UserID
JOIN LE_WeixinInfo AS wi ON wi.RecID=wu.UserType AND wi.STATUS=0
WHERE t2.R=1";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
    }
}
