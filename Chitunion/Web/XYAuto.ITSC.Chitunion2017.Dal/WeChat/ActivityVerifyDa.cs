/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017.Dal.WeChat
* 类 名 称 ：ActivityVerifyDa
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/12 17:39:40
********************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.WeChat;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WeChat
{
    public class ActivityVerifyDa : DataBase
    {
        public static readonly ActivityVerifyDa Instance = new ActivityVerifyDa();
        /// <summary>
        /// 根据微信用户表判断是否新用户 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true：新用户  false：老用户</returns>
        public bool VerifyUser(VerifyUserModel model)
        {
            string SQL = @"
                            DECLARE @unionid VARCHAR(100);

                            SELECT TOP 1
                                    @unionid = unionid
                            FROM    dbo.LE_WeiXinUser
                            WHERE   UserID = @UserID;
                            SELECT  ( CASE WHEN UD.CountNum = U.UserCount THEN 1 --新用户
                                           ELSE 0 --老用户
                                      END )
                            FROM    ( SELECT    COUNT(0) AS UserCount ,
                                                UserID
                                      FROM      dbo.LE_WeiXinUser
                                      WHERE     CreateTime BETWEEN @BeginTime AND @EndTime
                                                AND unionid = @unionid --and UserType IS NOT NULL
                                      GROUP BY  UserID
                                    ) AS U
                                    LEFT JOIN ( SELECT  COUNT(0) AS CountNum ,
                                                        UserID
                                                FROM    dbo.LE_WeiXinUser
                                                WHERE   unionid = @unionid --and UserType IS NOT NULL
                                                GROUP BY UserID
                                              ) AS UD ON UD.UserID = U.UserID;";

            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",model.UserID),
                new SqlParameter("@BeginTime",model.BeginTime),
                new SqlParameter("@EndTime",model.EndTime)
            };
            var returnValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return ConvertHelper.GetBoolean(returnValue ?? 0);


        }
        /// <summary>
        /// 根据收益明细判断是否新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool VerifyIncomeDetail(VerifyUserModel model)
        {
            string SQL = @"
                        SELECT (CASE
                                    WHEN COUNT(0) = 0 THEN
                                        1
                                    ELSE
                                        0
                                END) AS Result
                        FROM dbo.LE_IncomeDetail
                        WHERE CategoryID = @CategoryID
                              AND UserID = @UserID";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",model.UserID),
                new SqlParameter("@CategoryID",model.Source),
            };
            var returnValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return ConvertHelper.GetBoolean(returnValue);
        }

        /// <summary>
        /// 根据提现明细判断是否新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool VerifyWithdrawals(VerifyUserModel model)
        {
            string SQL = @"
                        SELECT (CASE
                                    WHEN COUNT(0) = 0 THEN
                                        1
                                    ELSE
                                        0
                                END) AS Result
                        FROM dbo.LE_WithdrawalsDetail
                        WHERE IsActive = 1
                              AND PayeeID = @PayeeID AND ApplySource = @ApplySource";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@PayeeID",model.UserID),
                new SqlParameter("@ApplySource",model.Source)
            };
            var returnValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return ConvertHelper.GetBoolean(returnValue);

        }

        /// <summary>
        /// 根据提现明细判断是否 提现成功（包括：驳回，通过）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool VerifyWithdrawalsSuccess(VerifyUserModel model)
        {
            string SQL = @"
                        SELECT COUNT(0) AS Result
                        FROM dbo.LE_WithdrawalsDetail
                        WHERE IsActive = 1
                              AND PayeeID = @PayeeID AND ApplySource = @ApplySource AND AuditStatus != 197003";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@PayeeID",model.UserID),
                new SqlParameter("@ApplySource",model.Source)
            };
            var returnValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return ConvertHelper.GetBoolean(returnValue);

        }

        /// <summary>
        /// 获取订单数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int GetOrderNum(VerifyUserModel model)
        {
            string SQL = @"
                        SELECT COUNT(0) AS Result
                        FROM dbo.LE_ADOrderInfo
                        WHERE UserID = @UserID;";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",model.UserID)
            };
            var returnValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return ConvertHelper.GetInteger(returnValue);
        }
        /// <summary>
        /// 根据openId获取UserId
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public int GetUserIdByOpenId(string openId)
        {
            string SQL = @"SELECT UserID FROM dbo.LE_WeiXinUser WHERE openid=@openid";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@openid",openId)
            };
            var returnValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return ConvertHelper.GetInteger(returnValue);
        }
    }
}
