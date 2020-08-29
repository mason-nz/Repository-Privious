/********************************************************
*创建人：hant
*创建时间：2017/12/18 14:19:31 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.TaskInfo
{
     public class AppInfo :DataBase
    {
        #region
        public static readonly AppInfo Instance = new AppInfo();
        #endregion

        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="appid">主键ID</param>
        /// <returns></returns>
        public Entities.Task.AppInfo GeAppInfo(int appid)
        {
            var sql = @"
                   SELECT [AppID]
                          ,[AppName]
                          ,[AppKey]
                          ,[Introducce]
                          ,[ChannelID]
                          ,[CallBackUrl]
                          ,[LinkName]
                          ,[LinkTel]
                          ,[UserID]
                          ,[ValidDate]
                          ,[Status]
                          ,[CreateTime] 
	                      FROM [Chitunion_DataSystem2017].[dbo].[AppInfo]
	                      WHERE AppID=@AppID";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AppID",appid)
            };
            var data = SqlHelper.ExecuteDataset(Chitunion_DataSystem2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToEntity<Entities.Task.AppInfo>(data.Tables[0]);
        }

        public Entities.Task.AppInfo GeAppInfo(int appid,string appkey)
        {
            var sql = @"
                   SELECT [AppID]
                          ,[AppName]
                          ,[AppKey]
                          ,[Introducce]
                          ,[ChannelID]
                          ,[CallBackUrl]
                          ,[LinkName]
                          ,[LinkTel]
                          ,[UserID]
                          ,[ValidDate]
                          ,[Status]
                          ,[CreateTime] 
	                      FROM [Chitunion_DataSystem2017].[dbo].[AppInfo]
	                      WHERE AppID=@AppID AND AppKey=@AppKey";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AppID",appid),
                new SqlParameter("@AppKey",appkey)
            };
            var data = SqlHelper.ExecuteDataset(Chitunion_DataSystem2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToEntity<Entities.Task.AppInfo>(data.Tables[0]);

        }

        /// <summary>
        /// 更新AppKey
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public bool UpdateAppKey(int appid,string appkey)
        {
            var sql = @"UPDATE  [Chitunion_DataSystem2017].[dbo].[AppInfo] SET [AppKey]=NEWID()
	                    WHERE AppID=@AppID AND AppKey=@AppKey";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AppID",appid),
                new SqlParameter("@AppKey",appkey)
            };
            int count = SqlHelper.ExecuteNonQuery(Chitunion_DataSystem2017, CommandType.Text, sql, parameters.ToArray());
            return count > 0 ? true : false;

        }

        public int GeChannelIDByUserId(int UserId)
        {
            var sql = @"
                   SELECT  [ChannelID]
	                      FROM [Chitunion_DataSystem2017].[dbo].[AppInfo]
	                      WHERE UserID=@UserID";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserID",UserId)
            };
            var data = SqlHelper.ExecuteScalar(Chitunion_DataSystem2017, CommandType.Text, sql, parameters.ToArray());
            return Convert.IsDBNull(data) ? 0 : Convert.ToInt32(data);
        }

        public string GetChannelNameByChannelId(int ChannelId)
        {
            var sql = @"
                   SELECT  [AppName]
	                      FROM [Chitunion_DataSystem2017].[dbo].[AppInfo]
	                      WHERE ChannelId=@ChannelId";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ChannelId",ChannelId)
            };
            var data = SqlHelper.ExecuteScalar(Chitunion_DataSystem2017, CommandType.Text, sql, parameters.ToArray());
            return data.ToString();
        }

        public string GetAppKeyByAppID(int AppId)
        {
            var sql = @"
                   SELECT  [AppKey]
	                      FROM [Chitunion_DataSystem2017].[dbo].[AppInfo]
	                      WHERE AppID=@AppId";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@AppId",AppId)
            };
            var data = SqlHelper.ExecuteScalar(Chitunion_DataSystem2017, CommandType.Text, sql, parameters.ToArray());
            return data.ToString();
        }

    }
}
