using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.UserManage
{
    public class UserInfo : DataBase
    {
        public readonly static UserInfo Instance = new UserInfo();
        public Entities.UserManage.UserInfoAll GetModel(int userID)
        {
            var sbSql = new StringBuilder();

            sbSql.Append($@"
                            SELECT  UI.UserID ,
                                    UI.UserName ,
                                    UI.Pwd ,
                                    UI.Mobile ,
                                    UI.Type ,
                                    UI.Category ,
                                    UI.Source ,
                                    UI.IsAuthMTZ ,
                                    UI.AuthAEUserID ,
                                    UI.IsAuthAE ,
                                    UI.SysUserID ,
                                    UI.EmployeeNumber ,
                                    UI.Email ,
                                    UI.Status ,
                                    UI.CreateTime ,
                                    UI.CreateUserID ,
                                    UI.LastUpdateTime ,
                                    UI.LastUpdateUserID ,
                                    UD.IdentityNo ,
                                    UD.TrueName ,
                                    UD.BusinessID ,
                                    UD.ProvinceID ,
                                    UD.CityID ,
                                    UD.CounntyID ,
                                    UD.Contact ,
                                    UD.Address ,
                                    UD.BLicenceURL ,
                                    UD.OrganizationURL ,
                                    UD.IDCardFrontURL ,
                                    UD.IDCardBackURL ,
                                    UD.Status UDStatus ,
                                    UBK.AccountName ,
                                    UBK.AccountType
                            FROM    dbo.UserInfo UI
                                    LEFT JOIN dbo.UserDetailInfo UD ON UD.UserID = UI.UserID
                                    LEFT JOIN dbo.LE_UserBankAccount UBK ON UBK.UserID = UI.UserID
                                                                            AND UBK.AccountType = {(int)Entities.UserManage.AccountType.支付宝}
                            WHERE   UI.UserID = {userID}; ");

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToEntity<Entities.UserManage.UserInfoAll>(ds.Tables[0]);
        }

        public bool Update(Entities.UserManage.UserInfoAll model, SqlTransaction trans = null)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                UPDATE  dbo.UserInfo
                                SET     UserName = '{model.UserName}' ,
                                        Mobile = '{model.Mobile}' ,
                                        Pwd = '{model.Pwd}' ,
                                        Type = {model.Type} ,
                                        Category = {model.Category} ,
                                        LastUpdateTime = GETDATE() 
                                WHERE   UserID = {model.UserID};");
            //var parameters = new SqlParameter[]
            //{
            //    new SqlParameter("@Content", materielID)
            //};

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString());

            return rowcount > 0;
        }

        public bool IsExistsByMobileCategory(string mobile, int category)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                SELECT COUNT(1) FROM dbo.UserInfo WHERE Category={category} AND Mobile='{mobile}';");

            var rowcount = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());

            return Convert.ToInt32(rowcount) > 1;
        }
        public bool IsExistsMobile(int userID, int category, string mobile)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                SELECT  COUNT(1)
                                FROM    dbo.UserInfo UI 
                                WHERE   UI.Category = {category} 
                                        AND UI.UserID <> {userID} 
                                        AND UI.Mobile = '{mobile}';");

            var rowcount = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());

            return Convert.ToInt32(rowcount) > 0;
        }
        /// <summary>
        /// 通过手机号获取用户ID
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <param name="Category">用户分类（29001 广告主 29002 媒体主）</param>
        /// <returns></returns>
        public int GetUserIDByMobile(string Mobile, int Category, int UserId = 0)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                SELECT  UI.UserID
                                FROM    dbo.UserInfo UI 
                                WHERE   UI.Category = {Category} AND UI.Status=0
                                        AND UI.Mobile = '{StringHelper.SqlFilter(Mobile)}'");
            if (UserId > 0)
            {
                sbSql.AppendLine($" AND UI.UserID!={UserId}");
            }
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        public int UpdateMobile(string Mobile, int UserID, SqlTransaction trans = null)
        {
            string strSql = $"UPDATE dbo.UserInfo SET Mobile='{StringHelper.SqlFilter(Mobile)}' WHERE  UserID={UserID}";
            var rowcount = 0;
            rowcount = trans == null
                ? SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql)
                : SqlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
            return rowcount;
    
        }

        public bool IsAddedMobile(int UserId, ref string Mobile)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"SELECT mobile  FROM dbo.UserInfo WHERE UserID={UserId}");
            object mobile = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            Mobile = "";
            if (mobile != null)
            {
                Mobile = mobile.ToString();
            }
            return mobile == null ? false : (mobile.ToString() == "" ? false : true);
        }
        /// <summary>
        /// 修改用户类型
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool UpdateType(int Type, int UserID)
        {
            string strSql = $"UPDATE dbo.UserInfo SET Type={Type} WHERE  UserID={UserID}";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql) > 0;
        }
        public int GetUserIdByMobile(string Mobile, int Category)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"SELECT UserID FROM dbo.UserInfo WHERE Status=0   AND Mobile='{Mobile}' AND Category={Category}");
            object userId = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return userId == null ? 0 : Convert.ToInt32(userId);

        }
        /// <summary>
        /// 快捷注册用户
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Type">类型（个人或企业）</param>
        /// <param name="Source">来源</param>
        /// <param name="Category">分类</param>
        /// <param name="RegisterType">注册类型 </param>
        /// <returns></returns>
        public int InsertUser(string Mobile, string UserName, int Type, int Source, int Category, int RegisterType, long PromotionChannelID,string RegisterIp)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
											INSERT INTO dbo.UserInfo
											        ( UserName ,
											          Mobile ,
											          Pwd ,
											          Type ,
											          Source ,
											          IsAuthMTZ ,
											          AuthAEUserID ,
											          IsAuthAE ,
											          SysUserID ,
											          EmployeeNumber ,
											          Status ,
											          CreateTime ,
											          CreateUserID ,
											          LastUpdateTime ,
											          LastUpdateUserID ,
											          Category ,
											          Email ,
											          LockState ,
											          SleepState ,
											          LockType ,
											          SleepStatus ,
											          RegisterType ,
											          CleanStatus ,
											          PromotionChannelID,
                                                        RegisterIp
											        )
											VALUES  ( '{UserName}' , -- UserName - varchar(50)
											          '{Mobile}' , -- Mobile - varchar(20)
											          '' , -- Pwd - varchar(50)
											          {Type} , -- Type - int
											          {Source} , -- Source - int
											          0 , -- IsAuthMTZ - bit
											          0 , -- AuthAEUserID - int
											          0 , -- IsAuthAE - bit
											          0 , -- SysUserID - int
											          '' , -- EmployeeNumber - varchar(20)
											          0 , -- Status - int
											          GETDATE() , -- CreateTime - datetime
											          0 , -- CreateUserID - int
											          GETDATE() , -- LastUpdateTime - datetime
											          0 , -- LastUpdateUserID - int
											          {Category} , -- Category - int
											          '' , -- Email - varchar(50)
											          0 , -- LockState - int
											          0 , -- SleepState - int
											          0 , -- LockType - int
											          0 , -- SleepStatus - int
											          {RegisterType} , -- RegisterType - int
											          0 , -- CleanStatus - int
											          {PromotionChannelID} , -- PromotionChannelID - bigint
                                                        '{RegisterIp}'
											        );select @@identity");
            object userId = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return userId == null ? 0 : Convert.ToInt32(userId);
        }
        #region V2.8
        /// <summary>
        /// 判断用户是否为新用户
        /// 2018-04-09 zlb
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="hour">超出小时数</param>
        /// <returns></returns>
        public bool IsNewUser(int userId, int hour)
        {
            var sql = $@"SELECT COUNT(1) FROM dbo.UserInfo U WHERE  DATEDIFF(HOUR, U.CreateTime, GETDATE()) <= {hour} AND U.UserID= {userId}";
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
            return count > 0;
        }
        public int InsertH5User(string UserName, int Type, int Source, int Category, int RegisterType, long PromotionChannelID, string HeadimgURL, string ChannelUserID)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
											INSERT INTO dbo.UserInfo
											        ( UserName ,
                                                      Mobile,
											          Pwd ,
											          Type ,
											          Source ,
											          IsAuthMTZ ,
											          AuthAEUserID ,
											          IsAuthAE ,
											          SysUserID ,
											          EmployeeNumber ,
											          Status ,
											          CreateTime ,
											          CreateUserID ,
											          LastUpdateTime ,
											          LastUpdateUserID ,
											          Category ,
											          Email ,
											          LockState ,
											          SleepState ,
											          LockType ,
											          SleepStatus ,
											          RegisterType ,
											          CleanStatus ,
											          PromotionChannelID,
                                                      HeadimgURL,
                                                      ChannelUserID
											        )
											VALUES  ( '{UserName}' , -- UserName - varchar(50)
                                                      '', -- 手机号 - varchar(50)
											          '' , -- Pwd - varchar(50)
											          {Type} , -- Type - int
											          {Source} , -- Source - int
											          0 , -- IsAuthMTZ - bit
											          0 , -- AuthAEUserID - int
											          0 , -- IsAuthAE - bit
											          0 , -- SysUserID - int
											          '' , -- EmployeeNumber - varchar(20)
											          0 , -- Status - int
											          GETDATE() , -- CreateTime - datetime
											          0 , -- CreateUserID - int
											          GETDATE() , -- LastUpdateTime - datetime
											          0 , -- LastUpdateUserID - int
											          {Category} , -- Category - int
											          '' , -- Email - varchar(50)
											          0 , -- LockState - int
											          0 , -- SleepState - int
											          0 , -- LockType - int
											          0 , -- SleepStatus - int
											          {RegisterType} , -- RegisterType - int
											          0 , -- CleanStatus - int
											          {PromotionChannelID}, -- PromotionChannelID - bigint
                                                      '{HeadimgURL}',
                                                       '{ChannelUserID}'
											        );select @@identity");
            object userId = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return userId == null ? 0 : Convert.ToInt32(userId);
        }

        public int UpdateH5User(string ChannelUserID, int UserID, string UserName, string HeadimgURL, int Sex)
        {
            string strSql = $@"UPDATE dbo.UserInfo SET  ChannelUserID='{ChannelUserID}',UserName='{UserName}',HeadimgURL='{HeadimgURL}',LastUpdateTime=GETDATE()  
                               WHERE  UserID={UserID} and (UserName!='{UserName}' or HeadimgURL!='{HeadimgURL}')";

            if (Sex == 0 || Sex == 1)
            {
                strSql += $" UPDATE dbo.UserDetailInfo SET   Sex={Sex} WHERE UserID={UserID} and Sex!={Sex}";
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        public int GetUserIdByChannelUID(string ChannelUserID, int Category)
        {
            string strSql = $"SELECT UserID  FROM dbo.UserInfo WHERE  ChannelUserID = '{ChannelUserID}' and status=0 AND Category={Category} ORDER BY LastUpdateTime DESC ";
            object userId = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return userId == null ? 0 : Convert.ToInt32(userId.ToString() == "" ? 0 : userId);
        }
        public string GechannelUserId(int userId, int category)
        {
            string strSql = $"SELECT ChannelUserID   FROM dbo.UserInfo WHERE  UserID = {userId} and status=0 AND Category={category}";
            object channelUserId = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return channelUserId?.ToString() ?? "";
        }
        #endregion
        
    }
}
