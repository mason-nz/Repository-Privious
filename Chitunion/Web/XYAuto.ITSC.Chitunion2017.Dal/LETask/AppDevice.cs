using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //设备信息表
    public partial class AppDevice : DataBase
    {


        public static readonly AppDevice Instance = new AppDevice();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.AppDevice entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into App_Device(");
            strSql.Append("UserID,EMEI,EMSI,AndroidId,Network,AppVersion,Channel,SystemVersion,PhoneModel,ScreenResolution,ActivationTime,Carrier,AllowLocationInfo,AllowNoticeInfo,ReleatedInstallAppInfo,CreateTime");
            strSql.Append(",IsAllowMsgNotice,Platform) values (");
            strSql.Append("@UserID,@EMEI,@EMSI,@AndroidId,@Network,@AppVersion,@Channel,@SystemVersion,@PhoneModel,@ScreenResolution,@ActivationTime,@Carrier,@AllowLocationInfo,@AllowNoticeInfo,@ReleatedInstallAppInfo,@CreateTime");
            strSql.Append(",@IsAllowMsgNotice,@Platform) ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserID",entity.UserID),
                        new SqlParameter("@EMEI",entity.EMEI),
                        new SqlParameter("@EMSI",entity.EMSI),
                        new SqlParameter("@AndroidId",entity.AndroidId),
                        new SqlParameter("@Network",entity.Network),
                        new SqlParameter("@AppVersion",entity.AppVersion),
                        new SqlParameter("@Channel",entity.Channel),
                        new SqlParameter("@SystemVersion",entity.SystemVersion),
                        new SqlParameter("@PhoneModel",entity.PhoneModel),
                        new SqlParameter("@ScreenResolution",entity.ScreenResolution),
                        new SqlParameter("@ActivationTime",entity.ActivationTime),
                        new SqlParameter("@Carrier",entity.Carrier),
                        new SqlParameter("@AllowLocationInfo",entity.AllowLocationInfo),
                        new SqlParameter("@AllowNoticeInfo",entity.AllowNoticeInfo),
                        new SqlParameter("@ReleatedInstallAppInfo",entity.ReleatedInstallAppInfo),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                           new SqlParameter("@IsAllowMsgNotice",entity.IsAllowMsgNotice),
                           new SqlParameter("@Platform",entity.Platform),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public Entities.LETask.AppDevice IsExistIsAllowMsgNotice(string emei,int platform)
        {
            var sql = @"SELECT TOP 1 * FROM DBO.App_Device WITH(NOLOCK)
			   WHERE EMEI = @EMEI AND Platform =@Platform
			   ORDER BY RecID DESC";
            var parameters = new SqlParameter[]
            {
                   new SqlParameter("@EMEI",emei),
                   new SqlParameter("@Platform",platform),
            };
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return DataTableToEntity<Entities.LETask.AppDevice>(obj.Tables[0]);
        }

        public bool IsExist(int userId)
        {
            var sql = $@"SELECT COUNT(*) FROM dbo.App_Device WITH(NOLOCK) WHERE UserID = {userId}";

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj != null && Convert.ToInt32(obj) > 0;
        }

        public bool UpdateIsAllowMsgNotice(string emei, bool isOpend, int platform)
        {
            var sql = @"
                     UPDATE   dbo.App_Device
                                   SET      IsAllowMsgNotice = @IsAllowMsgNotice
                                   WHERE    RecID = ( SELECT TOP 1 RecID
                                                      FROM      dbo.App_Device WITH ( NOLOCK )
                                                      WHERE     EMEI = @EMEI AND Platform = @Platform
                                                      ORDER BY  RecID DESC
                                                    );
                    ";
            var parameters = new SqlParameter[]
          {
                   new SqlParameter("@EMEI",emei),
              new SqlParameter("@IsAllowMsgNotice",isOpend),
              new SqlParameter("@Platform",platform),
          };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters) > 0;
        }
    }
}

