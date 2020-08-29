using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //用户银行账户表
    public partial class LeUserBankAccount : DataBase
    {


        public static readonly LeUserBankAccount Instance = new LeUserBankAccount();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeUserBankAccount entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_UserBankAccount(");
            strSql.Append("UserID,AccountName,CreateTime,Status,AccountType");
            strSql.Append(") values (");
            strSql.Append("@UserID,@AccountName,@CreateTime,@Status,@AccountType");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserID",entity.UserID),
                        new SqlParameter("@AccountName",entity.AccountName),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Account",entity.AccountType),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public List<Entities.LETask.LeUserBankAccount> GetInfo(int userId)
        {
            var sql = $@"
                    SELECT  UB.RecID ,
                            UB.UserID ,
                            UB.AccountName ,
                            UB.CreateTime ,
                            UB.Status ,
                            UB.AccountType,
		                    DC.DictName AS AccountTypeName
                    FROM    LE_UserBankAccount AS UB WITH ( NOLOCK )
                    LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = UB.AccountType
                    WHERE  UB.UserID = {userId}
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.LETask.LeUserBankAccount>(obj.Tables[0]);
        }


        public List<Entities.LETask.LeUserBankAccount> GetInfo(string accountName,
            UserBankAccountTypeEnum accountTypeEnum, int userId)
        {
            var sql = $@"
                    SELECT  UB.RecID ,
                            UB.UserID ,
                            UB.AccountName ,
                            UB.CreateTime ,
                            UB.Status ,
                            UB.AccountType,
		                    DC.DictName AS AccountTypeName
                    FROM    LE_UserBankAccount AS UB WITH ( NOLOCK )
                    LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = UB.AccountType

                    WHERE  UB.AccountName = '{accountName}' AND UB.AccountType = {(int)accountTypeEnum}
                    ";
            if (userId > 0)
            {
                sql += $"  AND UB.UserID ={userId}";
            }
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.LETask.LeUserBankAccount>(obj.Tables[0]);
        }

        public int Update(Entities.LETask.LeUserBankAccount entity)
        {
            var sql = $@"

                    DECLARE @UserId INT = {entity.UserID} ,
                            @AccountType INT  = {entity.AccountType},
                            @AccountName VARCHAR(50) = '{entity.AccountName}'
                    IF ( EXISTS ( SELECT    1 FROM      dbo.LE_UserBankAccount AS UB WITH ( NOLOCK )
                                  WHERE     UB.UserID = @UserId
                                            AND UB.AccountType = @AccountType ) )
                        BEGIN
	                    --存在，编辑
                            UPDATE  dbo.LE_UserBankAccount
                            SET     AccountName = @AccountName
                            WHERE   UserID = @UserId
                                    AND AccountType = @AccountType
                        END
                    ELSE
                        BEGIN
	                    --添加
                            INSERT  INTO dbo.LE_UserBankAccount
                                    ( UserID ,
                                      AccountName ,
                                      CreateTime ,
                                      Status ,
                                      AccountType
	                                )
                            VALUES  ( @UserId , -- UserID - int
                                      @AccountName , -- AccountName - varchar(50)
                                      GETDATE() , -- CreateTime - datetime
                                      0 , -- Status - int
                                      @AccountType  -- AccountType - int
	                                )
                        END
                    ";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);

        }
        public int TransUpdate(int UserID, int AccountType, string AccountName, SqlTransaction trans = null)
        {
            var sql = $@"

                    DECLARE @UserId INT = {UserID} ,
                            @AccountType INT  = {AccountType},
                            @AccountName VARCHAR(50) = '{AccountName}'
                    IF ( EXISTS ( SELECT    1 FROM      dbo.LE_UserBankAccount AS UB WITH ( NOLOCK )
                                  WHERE     UB.UserID = @UserId
                                            AND UB.AccountType = @AccountType ) )
                        BEGIN
	                    --存在，编辑
                            UPDATE  dbo.LE_UserBankAccount
                            SET     AccountName = @AccountName
                            WHERE   UserID = @UserId
                                    AND AccountType = @AccountType
                        END
                    ELSE
                        BEGIN
	                    --添加
                            INSERT  INTO dbo.LE_UserBankAccount
                                    ( UserID ,
                                      AccountName ,
                                      CreateTime ,
                                      Status ,
                                      AccountType
	                                )
                            VALUES  ( @UserId , -- UserID - int
                                      @AccountName , -- AccountName - varchar(50)
                                      GETDATE() , -- CreateTime - datetime
                                      0 , -- Status - int
                                      @AccountType  -- AccountType - int
	                                )
                        END
                    ";

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
            return rowcount;
           
    }
    }
}

