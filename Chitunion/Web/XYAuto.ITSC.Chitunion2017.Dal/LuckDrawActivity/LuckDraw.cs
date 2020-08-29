using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.LuckDrawActivity;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LuckDrawActivity
{
    /// <summary>
    /// 注释：LuckDraw
    /// 作者：zhanglb
    /// 日期：2018/5/23 16:50:26
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LuckDraw : DataBase
    {
        #region Instance
        public static readonly LuckDraw Instance = new LuckDraw();
        #endregion

        /// <summary>
        /// 获取已抽奖次数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="drawTime"></param>
        /// <returns></returns>
        public int GetDrawRemainder(int userId, DateTime drawTime)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"SELECT COUNT(1) FROM  dbo.HD_LuckDrawRecord WHERE Status=0 AND UserId={userId} AND CONVERT(VARCHAR(10),DrawTime,23) ='{drawTime.ToString("yyyy-MM-dd")}'");
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        public int GetDrawRemainder(int userId)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"SELECT COUNT(1) FROM  dbo.HD_LuckDrawRecord WHERE Status=0 AND UserId={userId}");
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public DataTable GetSumDrawInfo(int prizeId)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT SUM(DrawPrice) SumDrawPrice,COUNT(1) SumCount FROM dbo.HD_LuckDrawRecord WHERE Status=0 AND PrizeId={prizeId} GROUP BY PrizeId");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        ///  获取抽奖活动有效期和奖池金额
        /// </summary>
        /// <returns></returns>

        public DataTable GetActivityInfo()
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT A.ActivityId,BonusBase,DrawNum,Price,StartTime,EndTime FROM  dbo.HD_LuckDrawActivity A WHERE Status=0 ");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        ///  获取奖项列表
        /// </summary>
        /// <param name="activityId">活动Id</param>
        /// <returns></returns>

        public DataTable GetPrizeList(int activityId)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT * FROM dbo.HD_LuckDrawPrize WHERE status=0 and ActivityId={activityId}");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 查询获奖人名单
        /// </summary>
        /// <param name="topCount">行数</param>
        /// <returns></returns>
        public DataTable GetAwardeeList(int topCount)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT TOP {topCount}
                            W.nickname ,
                            U.Mobile ,
                            R.DrawTime ,
                            R.DrawPrice ,
                            R.DrawDescribe
                            FROM dbo.HD_LuckDrawRecord R 
                            LEFT JOIN dbo.v_LE_WeiXinUser W ON R.UserId = W.UserID
                            LEFT JOIN dbo.UserInfo U ON R.UserId = U.UserID
                            WHERE R.Status=0 and R.DrawPrice>0 ORDER BY R.DrawTime DESC");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 查询获奖人名单（假数据）
        /// </summary>
        /// <param name="topCount">行数</param>
        /// <returns></returns>
        public DataTable GetAwardeeMoniList(int topCount)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT TOP {topCount}
       NickName,
	   Mobile,
	   DrawTime,
	   DrawDescribe
FROM HD_LuckDrawRecord_Moni
WHERE Status=0 AND DrawTime BETWEEN '{DateTime.Now.ToString("yyyy-MM-dd 0:0:0")}' AND '{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 0:0:0")}'
AND ActivityId=1
ORDER BY DrawTime");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取用户获奖记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="totalCount">总条数</param>
        /// <returns></returns>
        public DataTable GetAwardRecord(int userId, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder sbSqlSelect = new StringBuilder();
            sbSqlSelect.Append("select t.* YanFaFROM (");
            sbSqlSelect.Append($@"SELECT P.AwardName,
                        R.DrawPrice,
                        R.DrawTime
                FROM    dbo.HD_LuckDrawRecord R
                        LEFT JOIN dbo.HD_LuckDrawPrize P ON P.PrizeId = R.PrizeId
                WHERE  R.status=0 and R.DrawPrice>0 and UserId = {userId}");
            sbSqlSelect.AppendLine(") T");
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",sbSqlSelect.ToString()),
                new SqlParameter("@PageRows",pageSize),
                new SqlParameter("@CurPage",pageIndex),
                new SqlParameter("@Order","DrawTime desc")
            };

            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams).Tables[0];
            totalCount = (int)(sqlParams[0].Value);
            return dt;
        }
        public int InsertAwardRecord(DrawRecord drawRecord, SqlTransaction trans = null)
        {
            string strSql = $@"INSERT INTO dbo.HD_LuckDrawRecord
                                ( ActivityId ,
                                  PrizeId ,
                                  UserId ,
                                  DrawPrice ,
                                  DrawDescribe ,
                                  DrawTime ,
                                  CreateTime ,
                                  Status
                                )
                        VALUES  ( {drawRecord.ActivityId} , -- ActivityId - int
                                  {drawRecord.PrizeId} , -- PrizeId - int
                                  {drawRecord.UserId} , -- UserId - int
                                  {drawRecord.DrawPrice} , -- DrawPrice - decimal
                                  '{drawRecord.DrawDescribe}' , -- DrawDescribe - varchar(100)
                                  GETDATE() , -- DrawTime - datetime
                                  GETDATE() , -- CreateTime - datetime
                                  {drawRecord.Status}  -- Status - int
                                )";
            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
            return rowcount;
        }

        public int UpdateBonusBase(int activityId, SqlTransaction trans = null)
        {
            string strSql = $"UPDATE dbo.HD_LuckDrawActivity SET DrawNum=DrawNum+1 WHERE ActivityId={activityId}";
            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
            return rowcount;
        }
        public string LotteryDraw(DrawRecord drawRecord, int maxDrawCount)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int count = Dal.Profit.Profit.Instance.InsertProfit(drawRecord.UserId, (int)ProfitTypeEnum.签到抽奖, drawRecord.DrawDescribe, drawRecord.DrawPrice, DateTime.Now, maxDrawCount, trans);
                        if (count > 0)
                        {
                            InsertAwardRecord(drawRecord, trans);
                            UpdateBonusBase(drawRecord.ActivityId, trans);
                            Dal.WechatWithdrawals.WechatWithdrawals.Instance.AddWithdrawals(drawRecord.UserId, drawRecord.DrawPrice, trans);
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
