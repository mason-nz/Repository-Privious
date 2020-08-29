using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;
using System.Data;
using System.Data.SqlClient;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class UserDetailInfo : DataBase
    {
        #region Instance
        public static readonly UserDetailInfo Instance = new UserDetailInfo();
        #endregion

        #region 根据userid获取用户名、真实姓名、手机号码
        /// <summary>
        /// 根据userid获取用户名、真实姓名、手机号码
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public Dictionary<string, object> GetUserInfoByUserID(int userid)
        {
            string sqlstr = string.Format("SELECT ui.UserName,ui.Mobile,ud.TrueName FROM dbo.UserInfo ui INNER JOIN dbo.UserDetailInfo ud ON ui.UserID=ud.UserID WHERE ui.UserID={0}",userid);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("UserName", "");
            dic.Add("Mobile", "");
            dic.Add("TrueName", "");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                dic["UserName"] = ds.Tables[0].Rows[0]["UserName"];
                dic["Mobile"] = ds.Tables[0].Rows[0]["Mobile"];
                dic["TrueName"] = ds.Tables[0].Rows[0]["TrueName"];
            }
            return dic;
        }
        #endregion

        public XYAuto.ITSC.Chitunion2017.Entities.UserManage.UserInfo GetUserInfo(int userId)
        {
            var sql = $@"
                    SELECT  UI.UserID ,
                            UI.UserName ,
                            UI.Mobile ,
                            UI.Pwd ,
                            UI.Type ,
                            UI.Category ,
                            UI.Source ,
                            UI.Email ,
                            UI.Status,
                            UI.LockState ,
                               UI.SleepState ,
                               UI.LockType ,
                               UI.SleepStatus 
                    FROM    dbo.UserInfo AS UI WITH ( NOLOCK )
                    WHERE UI.UserID = {userId}
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<XYAuto.ITSC.Chitunion2017.Entities.UserManage.UserInfo>(obj.Tables[0]);
        }

        public int UpdateUserPwd(int userId,string pwd)
        {
            var sql = $@"
                    UPDATE DBO.UserInfo SET Pwd = @Pwd WHERE UserID = @UserID
                    ";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Pwd",pwd),
                        new SqlParameter("@UserID",userId)
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int FirstUpdateUserInfo(int userId, string mobile,int provinceId,int cityId,string address)
        {
            var sql = $@"
                    
                    DECLARE @Mobile VARCHAR(20) = '{mobile}' ,
                        @UserId INT = {userId},
                        @ProvinceId INT = {provinceId} ,
                        @CityId INT = {cityId} ,
                        @Address VARCHAR(200) = '{address}';

                    UPDATE  dbo.UserInfo SET     Mobile = @Mobile WHERE   UserID = @UserId;
                    IF ( EXISTS ( SELECT    1
                                  FROM      dbo.UserDetailInfo AS UD WITH ( NOLOCK )
                                  WHERE     UD.UserID = @UserId ) )
                        BEGIN
                            DECLARE @UpdateStrSql VARCHAR(MAX) = '';
                            IF ( @ProvinceId != -2 )
                                BEGIN
                                    SET @UpdateStrSql += 'UPDATE UserDetailInfo SET ';
                                    SET @UpdateStrSql += ' ProvinceID =' + CAST(@ProvinceId AS VARCHAR(20));
                                END;
                            IF ( @CityId != -2 )
                                BEGIN
                                    SET @UpdateStrSql += ' ,CityID =' + CAST(@CityId AS VARCHAR(20));
                                END;
                            IF ( @Address <> '' )
                                BEGIN
                                    SET @UpdateStrSql += ' ,Address =' + @Address;
                                END;

                            IF ( @UpdateStrSql <> '' )
                                BEGIN
                                    SET @UpdateStrSql = '  WHERE  UserID =' +  CAST(@UserId AS VARCHAR(20));
                                    EXEC(@UpdateStrSql);
                                END;
                        END;
                    ELSE
                        BEGIN
                            INSERT  dbo.UserDetailInfo
                                    ( UserID ,
                                      TrueName ,
                                      BusinessID ,
                                      ProvinceID ,
                                      CityID ,
                                      Address ,
                                      CounntyID ,
                                      BLicenceURL ,
                                      IDCardFrontURL ,
                                      CreateTime ,
                                      CreateUserID ,
                                      Status ,
                                      IdentityNo
                                    )
                            VALUES  ( @UserId ,
                                      '' ,
                                      -2 ,
                                      @ProvinceId ,
                                      @CityId ,
                                      @Address ,
                                      -2 ,
                                      '' ,
                                      '' ,
                                      GETDATE() ,
                                      @UserId ,
                                      0 ,
                                      ''
                                    );
                        END
                    ";

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}
