using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.APP
{
    /// <summary>
    /// 注释：AppPushMsgSwitchLog
    /// 作者：lix
    /// 日期：2018/5/24 14:03:47
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class AppPushMsgSwitchLog : DataBase
    {
        #region Instance

        public static readonly AppPushMsgSwitchLog Instance = new AppPushMsgSwitchLog();

        #endregion Instance


        public int Insert(Entities.APP.AppPushMsgSwitchLog entity)
        {
            var sql = $@"
                IF(NOT EXISTS(SELECT 1 FROM dbo.AppPushMsgSwitchLog WITH(NOLOCK) WHERE DeviceId = '{entity.DeviceId}' AND DATEDIFF(DAY,GETDATE(), PushShowTime) = 0))
				                BEGIN
					                INSERT INTO DBO.AppPushMsgSwitchLog
					                        ( DeviceId, IsOpen, PushShowTime,Platform )
					                VALUES  ( N'{entity.DeviceId}', -- DeviceId - nvarchar(200)
					                          '{entity.IsOpen}', -- IsOpen - bit
					                          GETDATE() , -- PushShowTime - datetime
                                                {entity.Platform}
					                          )
			                    END
                ";

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public int InsertFisrt(Entities.APP.AppPushMsgSwitchLog entity)
        {
            var sql = $@"
                IF(NOT EXISTS(SELECT 1 FROM dbo.AppPushMsgSwitchLog WITH(NOLOCK) WHERE DeviceId = '{entity.DeviceId}'))
			                   BEGIN
					                INSERT INTO DBO.AppPushMsgSwitchLog
					                        ( DeviceId, IsOpen, PushShowTime,Platform )
					                VALUES  ( N'{entity.DeviceId}', -- DeviceId - nvarchar(200)
					                          '{entity.IsOpen}', -- IsOpen - bit
					                          GETDATE() , -- PushShowTime - datetime
                                                {entity.Platform}
					                          )
			                   END
                ";

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public bool IsExist(string deviceId, int days,int platform)
        {
            var sql = $@"
			IF(EXISTS(SELECT 1 FROM dbo.AppPushMsgSwitchLog WITH(NOLOCK) WHERE DeviceId = '{deviceId}' AND Platform={platform} AND DATEDIFF(DAY, PushShowTime,GETDATE()) BETWEEN 0 AND {days}))
			BEGIN
				SELECT 1;
			END
			ELSE
			BEGIN
				SELECT 0;
			END
            ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj != null && Convert.ToInt32(obj) > 0;
        }

        public bool IsClosed(string deviceId, int days, int platform)
        {
            var sql = $@"
			
                SELECT TOP 1
                        IsClosed
                FROM    dbo.AppPushMsgSwitchLog WITH ( NOLOCK )
                WHERE   DeviceId = '{deviceId}' AND Platform={platform}
                        AND DATEDIFF(DAY, PushShowTime, GETDATE()) BETWEEN 0 AND {days}
                ORDER BY RecId DESC
            ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj != null && Convert.ToInt32(obj) > 0;
        }

        public void UpdateClosed(string deviceId, int platform)
        {
            var sql = $@"
                
                    UPDATE  dbo.AppPushMsgSwitchLog
                    SET     IsClosed = 1
                    WHERE   RecId = ( SELECT TOP 1
                                                A.RecId
                                      FROM      dbo.AppPushMsgSwitchLog AS A
                                      WHERE     A.DeviceId = '{deviceId}' AND A.Platform = {platform}
                                      ORDER BY  A.RecId DESC
                                    );
                ";

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}
