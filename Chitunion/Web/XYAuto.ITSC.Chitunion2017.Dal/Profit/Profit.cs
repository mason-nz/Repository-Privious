using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Profit
{
    public class Profit : DataBase
    {
        public static readonly Profit Instance = new Profit();
        ///// <summary>
        ///// 获取收益信息列表
        ///// </summary>
        ///// <param name="UserId">内部用户ID</param>
        ///// <param name="RowNum">排序ID</param>
        ///// <param name="TopCount">条数</param>
        ///// <returns></returns>
        //public DataTable GetProfitList(int UserId, int RowNum, int TopCount)
        //{
        //    string strSql = $@"SELECT TOP {TopCount} *
        //                        FROM(SELECT *,ROW_NUMBER() OVER(ORDER BY CreateTime DESC) RowNum
        //                                  FROM(SELECT    SignTime AS CreateTime,
        //                                                        ProfitType = '签到',
        //                                                        CONVERT(VARCHAR(10), SignTime, 23) ProfitDate,
        //                                                        SignPrice ProfitPrice,
        //                                                        '连续签到:'+CONVERT(VARCHAR,SignNumber)+'天') ProfitDescribe,
        //                                                        OrderId = 0
        //                                              FROM      LE_DaySign
        //                                              WHERE     SignUserID = {UserId}
        //                                              UNION ALL
        //                                              SELECT    IR.ReceiveTime AS CreateTime,
        //                                                        ProfitType = '邀请好友',
        //                                                        CONVERT(VARCHAR(10), ReceiveTime, 23) ProfitDate,
        //                                                        RedEvesPrice ProfitPrice,
        //                                                         ('邀请好友：'+WX.nickname) ProfitDescribe ,
        //                                                        OrderId = 0
        //                                              FROM      LE_InviteRecord IR
        //                                                        LEFT JOIN v_LE_WeiXinUser WX ON IR.BeInvitedUserID = WX.UserID
        //                                              WHERE     IR.InviteUserID = {UserId}
        //                                              UNION ALL
        //                                              SELECT    EndTime AS CreateTime,
        //                                                        ProfitType = '订单',
        //                                                        CONVERT(VARCHAR(10), EndTime, 23) ProfitDate,
        //                                                        TotalAmount ProfitPrice,
        //                                                        ('订单名称：'+OrderName) ProfitDescribe ,
        //                                                        RecID OrderId
        //                                              FROM      LE_ADOrderInfo
        //                                              WHERE     UserID = {UserId}
        //                                                        AND Status = {(int)LeOrderStatusEnum.Finished}
        //                                                        AND TotalAmount > 0
        //                                            ) Profit
        //                                ) Profit WHERE  Profit.RowNum>{RowNum} ";
        //    return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        //}
        /// <summary>
        /// 获取收益信息列表
        /// </summary>
        /// <param name="userId">内部用户ID</param>
        /// <param name="startRowNum">开始RowNum</param>
        /// <param name="endRowNum">结束RowNum</param>
        /// <param name="isGetAll">是否查询全部</param>
        /// <returns></returns>
        public DataTable GetProfitList(int userId, int startRowNum, int endRowNum, bool isGetAll)
        {
            string incomeStr = "";
            if (!isGetAll)
            {
                incomeStr = " AND IncomePrice>0";
            }
            //       string strSql = $@"SELECT  * FROM
            //                      (SELECT ROW_NUMBER() OVER ( ORDER BY D.IncomeTime DESC) RowNum,
            //                       CategoryID ProfitType ,
            //                       CONVERT(VARCHAR(10), IncomeTime, 23)  ProfitDate ,
            //                       IncomeTime,
            //                       IncomePrice ProfitPrice ,
            //                       DetailDescription ProfitDescribe,
            //		(CASE CategoryID WHEN {(int)ProfitTypeEnum.订单统计} THEN  (CONVERT(VARCHAR,D.ClickCount) +'次') ELSE  CONVERT(varchar(5), D.IncomeTime, 24) END) TimeOrClick,
            //		W.nickname Nickname,W.headimgurl Headimgurl,D.ClickCount as ReadCount
            //               FROM    dbo.LE_IncomeDetail D
            //LEFT JOIN dbo.v_LE_WeiXinUser W ON D.ClickCount=W.UserID
            //               WHERE  D.UserID={userId} {incomeStr}) D
            //               WHERE RowNum BETWEEN {startRowNum} AND {endRowNum}";
            string strSql = $@"SELECT  *
                                FROM(SELECT    ROW_NUMBER() OVER(ORDER BY IncomeTime DESC) RowNum,
                                                    *
                                          FROM(SELECT    CategoryID ProfitType,
                                                                CONVERT(VARCHAR(10), IncomeTime, 23) ProfitDate,
                                                                IncomeTime,
                                                                IncomePrice ProfitPrice,
                                                                DetailDescription ProfitDescribe,
                                                                (CASE CategoryID
                                                                    WHEN {(int)ProfitTypeEnum.订单统计}
                                                                    THEN(CONVERT(VARCHAR, D.ClickCount)
                                                                           + '次')
                                                                    ELSE CONVERT(VARCHAR(5), D.IncomeTime, 24)
                                                                  END) TimeOrClick,
                                                                W.nickname Nickname,
                                                                W.headimgurl Headimgurl,
                                                                D.ClickCount AS ReadCount
                                                      FROM      dbo.LE_IncomeDetail D
                                                                LEFT JOIN dbo.v_LE_WeiXinUser W ON D.ClickCount = W.UserID
                                                      WHERE     D.UserID = {userId}
                                                      UNION ALL
                                                      SELECT    ProfitType = {(int)ProfitTypeEnum.订单统计},
                                                                CONVERT(VARCHAR(10), AD.CreateTime, 23) ProfitDate,
                                                                AD.CreateTime IncomeTime,
                                                                ProfitPrice = 0,
                                                                AD.OrderName ProfitDescribe,
                                                                TimeOrClick = '0次',
                                                                Nickname = NULL,
                                                                Headimgurl = NULL,
                                                                ReadCount = 0
                                                      FROM      dbo.LE_ADOrderInfo AD WITH(NOLOCK)
                                                      WHERE     AD.UserID =  {userId}
                                                                AND CONVERT(VARCHAR(10), AD.CreateTime, 23) = CONVERT(VARCHAR(10), GETDATE(), 23)
                                                    ) A
                                        ) D
                                WHERE   RowNum BETWEEN {startRowNum} AND {endRowNum};";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        /// <summary>
        /// 获取用户收益总数量
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int GetProfitCount(int UserId, bool isGetAll)
        {
            string incomeStr = "";
            if (!isGetAll)
            {
                incomeStr = " AND IncomePrice>0";
            }
            string strSql = $@"SELECT count(1)
                    FROM dbo.LE_IncomeDetail
                    WHERE  UserID={UserId} {incomeStr}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        public int InsertProfit(int userId, int profitType, string detailDescription, decimal incomPrice, DateTime? dtDate, int insertCount, SqlTransaction trans = null)
        {
            string strDate = "";
            if (dtDate != null)
            {
                strDate = $"  AND CONVERT(VARCHAR(10),IncomeTime,23)='{((DateTime)dtDate).ToString("yyyy-MM-dd")}' ";
            }
            string strSql = $@"DECLARE @IncomeCount INT;
                                SELECT  @IncomeCount = COUNT(1)
                                FROM    LE_IncomeDetail
                                WHERE   UserID = {userId} 
                                        AND CategoryID = {profitType} {strDate}; 


                                IF ( @IncomeCount < {insertCount} )
                                    BEGIN
                                        INSERT  INTO dbo.LE_IncomeDetail
                                                ( IncomeTime ,
                                                  UserID ,
                                                  CategoryID ,
                                                  DetailDescription ,
                                                  IncomePrice ,
                                                  ClickCount
                                                )
                                        VALUES  ( GETDATE() ,
                                                  {userId} ,
                                                  {profitType} ,
                                                  '{StringHelper.SqlFilter(detailDescription)}' ,
                                                  {incomPrice} , -- IncomePrice - decimal
                                                  0  -- ClickCount - int
                                                );
                                        DECLARE @DetailCount INT;
                                        SELECT  @IncomeCount = COUNT(1)
                                        FROM    LE_IncomeStatisticsCategory
                                        WHERE   UserID = {userId}
                                                AND IncomeCategoryID = {profitType};

                                        IF ( @DetailCount > 0 )
                                            BEGIN
                                                UPDATE  LE_IncomeStatisticsCategory
                                                SET     IncomePrice+= {incomPrice}
                                                WHERE   UserID = {userId}
                                                        AND IncomeCategoryID = {profitType};
                                            END;
                                        ELSE
                                            BEGIN
                                                INSERT  INTO dbo.LE_IncomeStatisticsCategory
                                                        ( IncomeCategoryID ,
                                                          IncomePrice ,
                                                          UserID ,
                                                          CreateTime
                                                        )
                                                VALUES  ( {profitType} , -- IncomeCategoryID - int
                                                          {incomPrice} , -- IncomePrice - decimal
                                                          {userId} , -- UserID - int
                                                          GETDATE()  -- CreateTime - datetime
                                                        );
                                            END;
                                    END;";
            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
            return rowcount;
        }

        /// <summary>
        /// 添加用户收益
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="profitType">收益类型</param>
        /// <param name="detailDescription">收益描述</param>
        /// <param name="incomPrice">收益金额</param>
        /// <param name="dtDate">插入时间</param>
        /// <param name="insertCount">插入条数</param>
        /// <returns></returns>
        public string AddProfit(int userId, int profitType, string detailDescription, decimal incomPrice, DateTime? dtDate, int insertCount)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int count = InsertProfit(userId, profitType, detailDescription, incomPrice, dtDate, insertCount, trans);
                        if (count > 0)
                        {
                            Dal.WechatWithdrawals.WechatWithdrawals.Instance.AddWithdrawals(userId, incomPrice, trans);
                        }
                        trans.Commit();
                        return "";
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return ex.Message;
                    }
                }
            }
        }
    }
}
