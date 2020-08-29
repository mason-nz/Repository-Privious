

using System.Data.SqlClient;
/**
*
*创建人：lixiong
*创建时间：2018/5/10 15:23:20
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.LE
{
    public sealed class LeUserBankAccountImpl : RepositoryImpl<LE_UserBankAccount>, ILeUserBankAccount
    {
        public int TransUpdate(int UserID, int AccountType, string AccountName)
        {
            var sql = $@"
                    DECLARE @UserId INT = @UserIdApp ,
                            @AccountType INT  = @AccountTypeApp,
                            @AccountName VARCHAR(50) =@AccountNameApp
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

            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserIdApp",UserID),
                new SqlParameter("@AccountTypeApp",AccountType),
                new SqlParameter("@AccountNameApp",AccountName) };

            return context.Database.ExecuteSqlCommand(sql, sqlParams);

        }
    }
}
