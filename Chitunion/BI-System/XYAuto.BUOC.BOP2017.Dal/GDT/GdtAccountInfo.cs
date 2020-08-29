using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.GDT;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    //广点通直客账户表
    public class GdtAccountInfo : DataBase
    {
        public static readonly GdtAccountInfo Instance = new GdtAccountInfo();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GDT.GdtAccountInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_AccountInfo(");
            strSql.Append("AccountId,DailyBudget,SystemStatus,RejectMessage,CorporationName,ContactPerson,ContactPersonTelephone,PullTime");
            strSql.Append(") values (");
            strSql.Append("@AccountId,@DailyBudget,@SystemStatus,@RejectMessage,@CorporationName,@ContactPerson,@ContactPersonTelephone,@PullTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@DailyBudget",entity.DailyBudget),
                        new SqlParameter("@SystemStatus",entity.SystemStatus),
                        new SqlParameter("@RejectMessage",entity.RejectMessage),
                        new SqlParameter("@CorporationName",entity.CorporationName),
                        new SqlParameter("@ContactPerson",entity.ContactPerson),
                        new SqlParameter("@ContactPersonTelephone",entity.ContactPersonTelephone),
                        new SqlParameter("@PullTime",entity.PullTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int InsertUserInfo(UserInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into dbo.UserInfo(");
            strSql.Append("SysUserID,EmployeeNumber,Status,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,Category,Email,UserName,Mobile,Pwd,Type,Source,IsAuthMTZ,AuthAEUserID,IsAuthAE");
            strSql.Append(") values (");
            strSql.Append("@SysUserID,@EmployeeNumber,@Status,getdate(),@CreateUserID,getdate(),@LastUpdateUserID,@Category,@Email,@UserName,@Mobile,@Pwd,@Type,@Source,@IsAuthMTZ,@AuthAEUserID,@IsAuthAE");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@SysUserID",entity.SysUserID),
                        new SqlParameter("@EmployeeNumber",entity.EmployeeNumber),
                        new SqlParameter("@Status",entity.Status),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        //new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        new SqlParameter("@Category",entity.Category),
                        new SqlParameter("@Email",entity.Email),
                        new SqlParameter("@UserName",entity.UserName),
                        new SqlParameter("@Mobile",entity.Mobile),
                        new SqlParameter("@Pwd",entity.Pwd),
                        new SqlParameter("@Type",entity.Type),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@IsAuthMTZ",entity.IsAuthMTZ),
                        new SqlParameter("@AuthAEUserID",entity.AuthAEUserID),
                        new SqlParameter("@IsAuthAE",entity.IsAuthAE),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS_ITSC, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public void InsertUserRole(int userId, string roleId, string trueName, string contactPerson, int organizeId)
        {
            var sql = @"
                    BEGIN TRANSACTION tran_bank;
                    DECLARE @tran_error INT;
                    SET @tran_error = 0;
                    BEGIN TRY
                ";

            var usrRoleSql = string.Format(@"
                        IF NOT EXISTS(SELECT 1 FROM Chitunion2017.DBO.UserRole WITH(NOLOCK) WHERE UserID = {0} AND SysID = '{1}'
                                        AND RoleID = '{2}' AND Status = 0)
                        BEGIN
                            INSERT Chitunion2017.DBO.UserRole
                                    (UserID,
                                      RoleID,
                                      SysID,
                                      Status,
                                      CreateTime,
                                      CreateUserID
                                    )
                            VALUES({0},'{2}','{1}',0,getdate(),0 )
                            SET @tran_error = @tran_error + @@error;
                        END
                ", userId, "SYS001", roleId);

            #region userDetailSql

            var userDetailSql = string.Format(@"

                        IF EXISTS(SELECT 1 FROM Chitunion2017.DBO.UserDetailInfo WITH(NOLOCK) WHERE UserID = {0})
                        BEGIN
	                        UPDATE Chitunion2017.DBO.UserDetailInfo SET TrueName = '{1}',Contact = '{2}' WHERE UserID = {0}
                            SET @tran_error = @tran_error + @@error;
                        END
                        ELSE
                        BEGIN
	                        INSERT Chitunion2017.DBO.UserDetailInfo
	                                ( UserID ,
	                                  TrueName ,
	                                  BusinessID ,
	                                  ProvinceID ,
	                                  CityID ,
	                                  CounntyID ,
	                                  Contact ,
	                                  Address ,
	                                  BLicenceURL ,
	                                  IDCardFrontURL ,
	                                  IDCardBackURL ,
	                                  CreateTime ,
	                                  CreateUserID ,
	                                  LastUpdateTime ,
	                                  LastUpdateUserID ,
	                                  OrganizationURL ,
	                                  Status
	                                )
	                        VALUES  ( {0} , -- UserID - int
	                                  '{1}' , -- TrueName - varchar(200)
	                                  0 , -- BusinessID - int
	                                  0 , -- ProvinceID - int
	                                  0 , -- CityID - int
	                                  0 , -- CounntyID - int
	                                  '{2}' , -- Contact - varchar(50)
	                                  '' , -- Address - varchar(200)
	                                  '' , -- BLicenceURL - varchar(200)
	                                  '' , -- IDCardFrontURL - varchar(200)
	                                  '' , -- IDCardBackURL - varchar(200)
	                                  GETDATE() , -- CreateTime - datetime
	                                  0 , -- CreateUserID - int
	                                  GETDATE() , -- LastUpdateTime - datetime
	                                  0 , -- LastUpdateUserID - int
	                                  '' , -- OrganizationURL - varchar(200)
	                                  0  -- Status - int
	                                )
                            SET @tran_error = @tran_error + @@error;
                        END

            ", userId, trueName, contactPerson);

            #endregion userDetailSql

            var sqlUserOrganize = string.Format(@"
                        IF NOT EXISTS(SELECT 1 FROM DBO.[GDT_UserOrganize] WITH(NOLOCK) WHERE OrganizeId = {0})
                        BEGIN
	                        INSERT INTO [dbo].[GDT_UserOrganize]
                                   ([UserId]
                                   ,[OrganizeId]
                                   ,[CreateTime])
		                           VALUES ({1},{0},GETDATE())
                            SET @tran_error = @tran_error + @@error;
                        END
                        ", organizeId, userId);

            sql += usrRoleSql + userDetailSql + sqlUserOrganize;

            sql += @"

                    END TRY
                BEGIN CATCH
                    print '出现异常，错误编号：' + convert(varchar, error_number()) + '， 错误消息：' + error_message();
                    SET @tran_error = @tran_error + 1;
                END CATCH
                IF ( @tran_error > 0 )
                    BEGIN
                        --执行出错，回滚事务
                        ROLLBACK TRAN;
                        SELECT 0;
                        PRINT '失败';
                    END
                ELSE
                    BEGIN
                        --没有异常，提交事务
                        COMMIT TRAN;
                         SELECT 1;
                        PRINT '成功';
                    END
            ";

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public Entities.GDT.UserInfo GetUserInfo(string mobile, string roleId = "")
        {
            var sql = @"SELECT * FROM Chitunion2017.DBO.UserInfo WITH(NOLOCK) WHERE Mobile = @Mobile AND Status = 0";
            var paras = new List<SqlParameter>() { new SqlParameter("@Mobile", mobile) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ITSC, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.GDT.UserInfo>(data.Tables[0]);
        }

        public Entities.GDT.UserInfo VerifyOfRole(string mobile, string roleId)
        {
            var sql = @"
                        SELECT  UI.*
                        FROM    Chitunion2017.dbo.UserInfo AS UI WITH ( NOLOCK )
                        WHERE   EXISTS ( SELECT 1
                                         FROM   Chitunion2017.dbo.UserRole AS UR WITH ( NOLOCK )
                                         WHERE  UR.RoleID = @RoleID
                                                AND UR.UserID = UI.UserID )
                                AND UI.Mobile = @Mobile
                        ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@Mobile", mobile),
                new SqlParameter("@RoleID", roleId)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ITSC, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.GDT.UserInfo>(data.Tables[0]);
        }

        public Entities.GDT.UserInfo VerifyOfOrganizeId(string mobile, int organizeId)
        {
            var sql = @"
                        SELECT  UI.*,UO.OrganizeId
                        FROM    Chitunion2017.dbo.UserInfo AS UI WITH ( NOLOCK )
		                        LEFT JOIN DBO.GDT_UserOrganize AS UO WITH(NOLOCK) ON UI.UserID = UO.UserId
                        WHERE   Status = 0
                        ";
            var paras = new List<SqlParameter>();
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                sql += " AND UI.Mobile = @Mobile ";
                paras.Add(new SqlParameter("@Mobile", mobile));
            }
            if (organizeId > 0)
            {
                sql += " AND UO.OrganizeId = @OrganizeId ";
                paras.Add(new SqlParameter("@OrganizeId", organizeId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.GDT.UserInfo>(data.Tables[0]);
        }

        public Entities.GDT.UserInfo VerifyOfOrganizeId(int organizeId)
        {
            var sql = @"
                        SELECT  UO.*
                        FROM    DBO.GDT_UserOrganize AS UO WITH(NOLOCK)
                        WHERE   UO.OrganizeId = @OrganizeId
                        ";
            var paras = new List<SqlParameter> { new SqlParameter("@OrganizeId", organizeId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.GDT.UserInfo>(data.Tables[0]);
        }

        public List<Entities.GDT.GdtAccountInfo> GetList(int accountId, bool isActive = true)
        {
            var sql = @"SELECT A.* FROM DBO.GDT_AccountInfo AS A WITH(NOLOCK) WHERE 1 = 1";
            if (accountId > 0)
            {
                sql += $" AND A.AccountId = {accountId}";
            }
            if (isActive)
            {
                sql += $" AND A.SystemStatus = 80001";
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.GDT.GdtAccountInfo>(data.Tables[0]);
        }

        public int InsertByGdtServer(Entities.GDT.GdtAccountInfo entity)
        {
            var sql = string.Format(@"
                    IF NOT EXISTS( SELECT 1 FROM DBO.[GDT_AccountInfo] WHERE AccountId = {0})
                    BEGIN
                        INSERT INTO [dbo].[GDT_AccountInfo]
                                   ([AccountId]
                                   ,[DailyBudget]
                                   ,[SystemStatus]
                                   ,[RejectMessage]
                                   ,[CorporationName]
                                   ,[ContactPerson]
                                   ,[ContactPersonTelephone]
                                   ,[PullTime])
                             VALUES
                                   ({0}
                                   ,{1}
                                   ,{2}
                                   ,'{3}'
                                   ,'{4}'
                                   ,'{5}'
                                   ,'{6}'
                                   ,getdate())
		                           ;select SCOPE_IDENTITY()
                    END
                        ", entity.AccountId, entity.DailyBudget, entity.SystemStatus, entity.RejectMessage, entity.CorporationName,
                        entity.ContactPerson, entity.ContactPersonTelephone);
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
    }
}