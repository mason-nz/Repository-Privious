using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;
using System.Data;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class YTGActivity : DataBase
    {
        private YTGActivity() { }
        private static YTGActivity _instance = null;
        public static YTGActivity Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGActivity();
                }
                return _instance;
            }
        }

        /// 终止项目
        /// <summary>
        /// 终止项目
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        public int EndProjectForYTGActivity(string activityid)
        {
            //--0-未开始，1-进行中，2-已结束，-1-删除
            string sql = @"UPDATE ProjectInfo
                                    SET Status=2
                                    WHERE DemandID ='" + StringHelper.SqlFilter(activityid) + "'";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);
        }
        public DataTable GetProjectForYTGActivity(string activityid)
        {
            string sql = "SELECT ProjectID,Name FROM projectinfo WHERE DemandID='" + StringHelper.SqlFilter(activityid) + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql).Tables[0];
            return dt;
        }
        /// 终止任务
        /// <summary>
        /// 终止任务
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        public int EndTaskForYTGActivity(string activityid, string remark)
        {
            //插入日志
            //--生成=1,分配=2,保存=3,回收=4,提交=5,撤销=6
            string log = @"INSERT INTO YTGActivityTaskLog(TaskID,OperationStatus,TaskStatus,Remark,CreateTime)
                                    SELECT TaskID,6,5,'" + StringHelper.SqlFilter(remark) + @"',GETDATE() FROM YTGActivityTask
                                    WHERE ActivityID='" + StringHelper.SqlFilter(activityid) + @"'
                                    AND Status<>4";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, log);
            //撤销任务
            //--待分配=1 待处理=2 处理中=3 已处理=4 已撤销=5
            string sql = @"UPDATE dbo.YTGActivityTask
                                    SET Status=5
                                    WHERE ActivityID='" + StringHelper.SqlFilter(activityid) + @"'
                                    AND Status<>4";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);
        }
        /// 获取到期未关闭的活动
        /// <summary>
        /// 获取到期未关闭的活动
        /// </summary>
        /// <returns></returns>
        public DataTable GetMaturityNoCloseActivity(DateTime d)
        {
            string sql = @"SELECT * FROM YTGActivity
                                    WHERE SignEndTime<='" + d.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                    AND Status=1";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 获取最大报名人ID
        /// <summary>
        /// 获取最大报名人ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxSingUserID()
        {
            string sql = @"SELECT ISNULL(MAX(SignID),0) FROM YTGActivityTask";
            return CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }
        /// 根据城市获取省
        /// <summary>
        /// 根据城市获取省
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public int GetAreaParentID(int aid)
        {
            string sql = "SELECT PID FROM dbo.AreaInfo WHERE AreaID=" + aid;
            return CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(ConnectionStrings_CRM, CommandType.Text, sql));
        }
        /// 根据车款获取品牌和车型
        /// <summary>
        /// 根据车款获取品牌和车型
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public string[] GetCarInfo(int carid)
        {
            string[] array = new string[3];
            string sql = @"SELECT  a.CarID ,
                                    a.CarName ,
                                    b.SerialID ,
                                    b.BrandID ,
                                    b.MasterBrandID
                            FROM    Car_Car a
                                    INNER JOIN Car_Serial b ON a.CsID = b.SerialID
                            WHERE   a.CarID = '" + carid + "'";
            using (SqlDataReader sr = SqlHelper.ExecuteReader(ConnectionStrings_CRM, CommandType.Text, sql))
            {
                if (sr.Read())
                {
                    array[0] = sr["MasterBrandID"].ToString();
                    array[1] = sr["SerialID"].ToString();
                    array[2] = sr["CarID"].ToString();
                }
            }
            return array;
        }
        /// 获取活动数据
        /// <summary>
        /// 获取活动数据
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetYTGActivityByStauts(int status)
        {
            string sql = @"SELECT  a.* ,
                                            ISNULL(b.maxSignID, 0) AS maxSignID
                                    FROM    dbo.YTGActivity a
                                            LEFT JOIN ( SELECT  ActivityID ,
                                                                MAX(SignID) maxSignID
                                                        FROM    dbo.YTGActivityTask
                                                        GROUP BY ActivityID
                                                      ) b ON b.ActivityID = a.ActivityID
                                    WHERE   a.Status =" + status;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
    }
}
