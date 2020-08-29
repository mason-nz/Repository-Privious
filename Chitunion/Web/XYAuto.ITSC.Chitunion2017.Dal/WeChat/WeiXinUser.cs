/********************************************************
*创建人：hant
*创建时间：2018/1/17 13:23:16 
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
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WeChat
{
    public class WeiXinUser : DataBase
    {
        public static readonly WeiXinUser Instance = new WeiXinUser();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.WeChat.WeiXinUser entity)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append(@"INSERT INTO [dbo].[LE_WeiXinUser]
                                   ([subscribe]
                                   ,[openid]
                                   ,[nickname]
                                   ,[sex]
                                   ,[city]
                                   ,[country]
                                   ,[province]
                                   ,[language]
                                   ,[headimgurl]
                                   ,[subscribe_time]
                                   ,[unionid]
                                   ,[remark]
                                   ,[groupid]
                                   ,[tagid_list]
                                   ,[UserID]
                                   ,[CreateTime]
                                   ,[LastUpdateTime]
                                   ,[QRcode]
                                   ,[Inviter]
                                   ,[InvitationQR]
                                   ,[Status])
                              VALUES
                                   (@subscribe
                                   ,@openid
                                   ,@nickname
                                   ,@sex
                                   ,@city
                                   ,@country
                                   ,@province
                                   ,@language
                                   ,@headimgurl
                                   ,@subscribe_time
                                   ,@unionid
                                   ,@remark
                                   ,@groupid
                                   ,@tagid_list
                                   ,@UserID
                                   ,@CreateTime
                                   ,@LastUpdateTime
                                   ,@QRcode
                                   ,@Inviter
                                    ,@InvitationQR
                                   ,@Status)");
                strSql.Append(";select SCOPE_IDENTITY()");
                var parameters = new SqlParameter[]{
                        new SqlParameter("@subscribe",entity.subscribe),
                        new SqlParameter("@openid",entity.openid),
                        new SqlParameter("@nickname",entity.nickname),
                        new SqlParameter("@sex",entity.sex),
                        new SqlParameter("@city",entity.city),
                        new SqlParameter("@country",entity.country),
                        new SqlParameter("@province",entity.province),
                        new SqlParameter("@language",entity.language),
                        new SqlParameter("@headimgurl",entity.headimgurl),
                        new SqlParameter("@subscribe_time",entity.subscribe_time),
                        new SqlParameter("@unionid",entity.unionid),
                        new SqlParameter("@remark",entity.remark),
                        new SqlParameter("@groupid",entity.groupid),
                        new SqlParameter("@tagid_list",entity.tagid_list),
                        new SqlParameter("@UserID",entity.UserID),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@QRcode",entity.QRcode),
                        new SqlParameter("@Inviter",entity.Inviter),
                         new SqlParameter("@InvitationQR",entity.InvitationQR),
                        new SqlParameter("@Status",entity.Status)
                        };


                var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
                return obj == null ? 0 : Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Insert WeiXinUser]:" + ex.Message);
                return 0;

            }

        }

        public void Update(Entities.WeChat.WeiXinUser entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Chitunion2017].[dbo].[LE_WeiXinUser]
                               SET [subscribe] = @subscribe
                                  ,[nickname] = @nickname
                                  ,[sex] = @sex
                                  ,[city] = @city
                                  ,[country] = @country
                                  ,[province] = @province
                                  ,[language] = @language
                                  ,[headimgurl] = @headimgurl
                                  ,[subscribe_time] = @subscribe_time
                                  ,[unionid] = @unionid
                                  ,[remark] = @remark
                                  ,[groupid] = @groupid                                  
                                  ,[UserID] = @UserID
                                  ,[LastUpdateTime] = @LastUpdateTime
                                  ,[Status] = @Status
                                  ,[Source] = @Source
                                  ,[Inviter] = @Inviter
                             WHERE [openid]=@openid");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@subscribe",entity.subscribe),
                        new SqlParameter("@nickname",entity.nickname),
                        new SqlParameter("@sex",entity.sex),
                        new SqlParameter("@city",entity.city),
                        new SqlParameter("@country",entity.country),
                        new SqlParameter("@province",entity.province),
                        new SqlParameter("@language",entity.language),
                        new SqlParameter("@headimgurl",entity.headimgurl),
                        new SqlParameter("@subscribe_time",entity.subscribe_time),
                        new SqlParameter("@unionid",entity.unionid),
                        new SqlParameter("@remark",entity.remark),
                        new SqlParameter("@groupid",entity.groupid),
                        new SqlParameter("@UserID",entity.UserID),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@openid",entity.openid),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@Inviter",entity.Inviter),
                        };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);

        }

        public void UpdateSync(Entities.WeChat.WeiXinUser entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Chitunion2017].[dbo].[LE_WeiXinUser]
                               SET [subscribe] = @subscribe
                                  ,[nickname] = @nickname
                                  ,[sex] = @sex
                                  ,[city] = @city
                                  ,[country] = @country
                                  ,[province] = @province
                                  ,[language] = @language
                                  ,[headimgurl] = @headimgurl
                                  ,[subscribe_time] = @subscribe_time
                                  ,[remark] = @remark
                                  ,[groupid] = @groupid
                                  ,[tagid_list] = @tagid_list
                                  ,[LastUpdateTime] = @LastUpdateTime
                                  ,[Status] = 0
                             WHERE [openid]=@openid");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@subscribe",entity.subscribe),
                        new SqlParameter("@nickname",entity.nickname),
                        new SqlParameter("@sex",entity.sex),
                        new SqlParameter("@city",entity.city),
                        new SqlParameter("@country",entity.country),
                        new SqlParameter("@province",entity.province),
                        new SqlParameter("@language",entity.language),
                        new SqlParameter("@headimgurl",entity.headimgurl),
                        new SqlParameter("@subscribe_time",entity.subscribe_time),
                        new SqlParameter("@remark",entity.remark),
                        new SqlParameter("@groupid",entity.groupid),
                        new SqlParameter("@tagid_list",entity.tagid_list),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@openid",entity.openid),
                        };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);

        }

        /// <summary>
        /// 根据openid 更新 unionid
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="unionid"></param>
        /// <returns></returns>
        public bool UpdateUnionID(string openid, string unionid)
        {
            var sql = @"UPDATE [Chitunion2017].[dbo].[LE_WeiXinUser] SET [unionid]=@unionid,LastUpdateTime = GETDATE() WHERE openid =@openid";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@unionid",unionid),
                        new SqlParameter("@openid",openid)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? false : Convert.ToInt32(obj) > 0;
        }

        /// <summary>
        /// 事务插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsetWeiXinAndUserInfo(Entities.WeChat.WeiXinUser entity)
        {
            bool result = false;
            int UserId = 0;
            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRINGS))
            {
                cnn.Open();
                SqlTransaction trans = cnn.BeginTransaction();
                SqlCommand cm = cnn.CreateCommand();
                cm.Transaction = trans;
                string sql = string.Empty;
                try
                {
                    #region 判断是否有UserId
                    var isExist = @"SELECT UserId FROM [Chitunion2017].[dbo].[LE_WeiXinUser] WHERE  openid = '" + entity.openid + "' ";
                    cm.CommandText = isExist;
                    UserId = Convert.ToInt32(cm.ExecuteScalar());
                    #endregion

                    if (UserId == 0)
                    {
                        if (entity.unionid.Length > 0)
                        {
                            #region 插入UserInfo
                            sql = @"  INSERT  [Chitunion2017].dbo.UserInfo(  UserName  ,Mobile  ,Pwd  ,Type  ,
                                    Source  ,IsAuthMTZ  ,AuthAEUserID  ,IsAuthAE  ,SysUserID  ,EmployeeNumber  ,Status  ,
                                    CreateTime  , CreateUserID  ,
                                    LastUpdateTime  ,LastUpdateUserID  , Category  ,Email  , LockState  ,SleepState  ,LockType  ,SleepStatus)
                              VALUES
                                   ('" + entity.unionid + "'  ,' '  ,'6d06306a3afb5c3d2c0a01c018110fc5'  ,1002  ,3006  ,0  ,null  ,0  ,null  ,null  ,0  ,GETDATE()  , 0  ,GETDATE()  ,0  , 29002  ,' '  , 0  ,0  ,null  ,null) ;select SCOPE_IDENTITY()";

                            #endregion

                            cm.CommandText = sql;

                            UserId = Convert.ToInt32(cm.ExecuteScalar());

                            #region 插入用户信息
                            var insert = @"INSERT INTO [dbo].[LE_WeiXinUser]
                                   ([subscribe],[openid],[nickname],[sex],[city],[country]
                                   ,[province],[language],[headimgurl],[subscribe_time]
                                   ,[unionid],[remark],[groupid],[tagid_list],[UserID],[CreateTime],[LastUpdateTime],[QRcode]
      ,[Inviter],[InvitationQR],[Status])
                              VALUES
                                   (" + entity.subscribe + ",'" + entity.openid + "','" + entity.nickname + "'," + entity.sex + ",'" + entity.city + "','" + entity.country + "','" + entity.province + "','" + entity.language + "','" + entity.headimgurl + "','" + entity.subscribe_time + "','" + entity.unionid + "','" + entity.remark + "'," + entity.groupid + ",'" + entity.tagid_list + "'," + UserId + ",'" + entity.CreateTime + "','" + entity.LastUpdateTime + "','" + entity.QRcode + "','" + entity.Inviter + "','" + entity.InvitationQR + "'," + entity.Status + ")";
                            cm.CommandText = insert;
                            cm.ExecuteScalar();

                            trans.Commit();
                            result = true;
                            #endregion
                        }
                        else // 根据OpenId 插入数据
                        {
                            var openid = "SELECT COUNT(0) FROM [dbo].[LE_WeiXinUser] WHERE [openid] = '" + entity.openid + "'";
                            cm.CommandText = openid;
                            UserId = Convert.ToInt32(cm.ExecuteScalar());

                            #region 更新
                            if (UserId > 0)
                            {
                                var update = "UPDATE [dbo].[LE_WeiXinUser] SET [LastUpdateTime] = GETDATE() ,[Status] = 1 WHERE [openid]= '" + entity.openid + "'";
                                cm.CommandText = update;
                                cm.ExecuteScalar();
                                cm.ExecuteScalar();
                                trans.Commit();
                                result = true;
                            }
                            #endregion
                            else
                            {
                                #region 插入用户信息
                                var insert = @"INSERT INTO [dbo].[LE_WeiXinUser]
                                   ([subscribe],[openid],[nickname],[sex],[city],[country]
                                   ,[province],[language],[headimgurl],[subscribe_time]
                                   ,[remark],[groupid],[tagid_list],[CreateTime],[LastUpdateTime],[QRcode]
      ,[Inviter],[InvitationQR],[Status])
                              VALUES
                                   (" + entity.subscribe + ",'" + entity.openid + "','" + entity.nickname + "'," + entity.sex + ",'" + entity.city + "','" + entity.country + "','" + entity.province + "','" + entity.language + "','" + entity.headimgurl + "','" + entity.subscribe_time + "','" + entity.remark + "'," + entity.groupid + ",'" + entity.tagid_list + "','" + entity.CreateTime + "','" + entity.LastUpdateTime + "','" + entity.QRcode + "','" + entity.Inviter + "','" + entity.InvitationQR + "'," + entity.Status + ")";
                                cm.CommandText = insert;
                                cm.ExecuteScalar();

                                trans.Commit();
                                result = true;
                                #endregion
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    cnn.Close();
                    trans.Dispose();
                    cnn.Dispose();
                }
                return result;

            }
        }


        public bool WeiXinUserAuthorization(Entities.WeChat.WeiXinUser entity)
        {

            bool result = false;
            int UserId = 0;
            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRINGS))
            {
                cnn.Open();
                SqlTransaction trans = cnn.BeginTransaction();
                SqlCommand cm = cnn.CreateCommand();
                cm.Transaction = trans;
                string sql = string.Empty;
                try
                {
                    #region 判断是否有关注
                    var isExist = @"SELECT COUNT(0) FROM [Chitunion2017].[dbo].[LE_WeiXinUser] WHERE  openid = '" + entity.openid + "' ";
                    cm.CommandText = isExist;
                    UserId = Convert.ToInt32(cm.ExecuteScalar());
                    #endregion

                    if (UserId == 0)
                    {
                        if (entity.unionid.Length > 0)
                        {
                            #region 插入UserInfo
                            sql = @"  INSERT  [Chitunion2017].dbo.UserInfo(  UserName  ,Mobile  ,Pwd  ,Type  ,
                                    Source  ,IsAuthMTZ  ,AuthAEUserID  ,IsAuthAE  ,SysUserID  ,EmployeeNumber  ,Status  ,
                                    CreateTime  , CreateUserID  ,
                                    LastUpdateTime  ,LastUpdateUserID  , Category  ,Email  , LockState  ,SleepState  ,LockType  ,SleepStatus)
                              VALUES
                                   ('" + entity.unionid + "'  ,' '  ,'6d06306a3afb5c3d2c0a01c018110fc5'  ,1002  ,3006  ,0  ,null  ,0  ,null  ,null  ,0  ,GETDATE()  , 0  ,GETDATE()  ,0  , 29002  ,' '  , 0  ,0  ,null  ,null) ;select SCOPE_IDENTITY()";

                            #endregion

                            cm.CommandText = sql;

                            UserId = Convert.ToInt32(cm.ExecuteScalar());

                            #region 插入用户信息
                            var insert = @"INSERT INTO [dbo].[LE_WeiXinUser]
                                   ([subscribe],[openid],[nickname],[sex],[city],[country]
                                   ,[province],[language],[headimgurl],[subscribe_time]
                                   ,[unionid],[remark],[groupid],[tagid_list],[UserID],[CreateTime],[LastUpdateTime],[AuthorizeTime],[QRcode]
      ,[Inviter],[InvitationQR],[Status])
                              VALUES
                                   (" + entity.subscribe + ",'" + entity.openid + "','" + entity.nickname + "'," + entity.sex + ",'" + entity.city + "','" + entity.country + "','" + entity.province + "','" + entity.language + "','" + entity.headimgurl + "','" + entity.subscribe_time + "','" + entity.unionid + "','" + entity.remark + "'," + entity.groupid + ",'" + entity.tagid_list + "'," + UserId + ",'" + entity.CreateTime + "','" + entity.LastUpdateTime + "',GETDATE(),'" + entity.QRcode + "','" + entity.Inviter + "','" + entity.InvitationQR + "'," + entity.Status + ")";
                            cm.CommandText = insert;
                            cm.ExecuteScalar();

                            trans.Commit();
                            result = true;
                            #endregion
                        }

                    }
                    else //有openid
                    {
                        var isExistUserId = @"SELECT UserId FROM [Chitunion2017].[dbo].[LE_WeiXinUser] WHERE  openid = '" + entity.openid + "' ";
                        cm.CommandText = isExistUserId;
                        UserId = Convert.ToInt32(cm.ExecuteScalar());

                        if (UserId == 0)
                        {
                            #region 插入UserInfo
                            var sqluser = @"  INSERT  [Chitunion2017].dbo.UserInfo(  UserName  ,Mobile  ,Pwd  ,Type  ,
                                    Source  ,IsAuthMTZ  ,AuthAEUserID  ,IsAuthAE  ,SysUserID  ,EmployeeNumber  ,Status  ,
                                    CreateTime  , CreateUserID  ,
                                    LastUpdateTime  ,LastUpdateUserID  , Category  ,Email  , LockState  ,SleepState  ,LockType  ,SleepStatus)
                              VALUES
                                   ('" + entity.unionid + "'  ,' '  ,'6d06306a3afb5c3d2c0a01c018110fc5'  ,1002  ,3006  ,0  ,null  ,0  ,null  ,null  ,0  ,GETDATE()  , 0  ,GETDATE()  ,0  , 29002  ,' '  , 0  ,0  ,null  ,null) ;select SCOPE_IDENTITY()";

                            #endregion

                            cm.CommandText = sqluser;

                            UserId = Convert.ToInt32(cm.ExecuteScalar());

                            #region 更新 UserId
                            var update = @" UPDATE [dbo].[LE_WeiXinUser]
                                           SET [unionid] = '" + entity.unionid + "' ,[UserID] = " + UserId + " WHERE [openid] = '" + entity.openid + "'";

                            #endregion
                            cm.CommandText = update;
                            cm.ExecuteScalar();

                            trans.Commit();
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    cnn.Close();
                    trans.Dispose();
                    cnn.Dispose();
                }
                return result;

            }
        }

        public bool WeiXinUserOperation(Entities.WeChat.WeiXinUser entity, UserCategoryEnum categoryId = UserCategoryEnum.媒体主)
        {
            Loger.Log4Net.Info($" WeiXinUserOperation DAL  enter...openid:{entity.openid}");
            bool result = false;
            int UserId = 0;

            if (entity.Source == 102)
            {
                entity.Source = 3;
            }
            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRINGS))
            {
                cnn.Open();
                SqlTransaction trans = cnn.BeginTransaction();
                SqlCommand cm = cnn.CreateCommand();
                cm.Transaction = trans;
                string sql = string.Empty;
                int mtzUserId = 0;
                int ggzUserId = 0;
                try
                {
                    #region UserInfo

                    //var user = $"SELECT W.UserID FROM Chitunion2017.dbo.v_LE_WeiXinUser W JOIN Chitunion2017.dbo.UserInfo U ON W.UserID = U.UserID WHERE W.unionid = '{entity.unionid}' AND U.Category ={(int)categoryId}";
                    //var user = $"SELECT W.UserID FROM Chitunion2017.dbo.v_LE_WeiXinUser W JOIN Chitunion2017.dbo.UserInfo U ON W.UserID = U.UserID WHERE W.openid = '{entity.openid}' AND U.Category ={(int)categoryId}";
                    var user = $@"SELECT TOP 1 W.UserID
FROM[Chitunion2017].[dbo].v_LE_WeiXinUser W 
JOIN Chitunion2017.dbo.UserInfo U ON W.UserID = U.UserID
        WHERE W.unionid ='{entity.unionid}'
              AND U.Category ={(int)categoryId}
                  ORDER BY W.CreateTime";
                    cm.CommandText = user;
                    UserId = Convert.ToInt32(cm.ExecuteScalar());
                    #endregion


                    var sqlWhere = categoryId == UserCategoryEnum.媒体主 ? " WHERE [UserID] = " + UserId + $" And openid='{entity.openid}'"
                          : " WHERE [AdvertiserUserId] = " + UserId + $" And openid='{entity.openid}'";

                    if (UserId > 0)
                    {
                        Loger.Log4Net.Info($" WeiXinUserOperation  enter...openid:{entity.openid}，UserId > 0");
                        var isWeiXinUser = @"SELECT COUNT(0) FROM [dbo].[LE_WeiXinUser] " + sqlWhere;
                        cm.CommandText = isWeiXinUser;
                        int iswx = Convert.ToInt32(cm.ExecuteScalar());
                        if (iswx > 0)
                        {
                            var update = @"UPDATE [dbo].[LE_WeiXinUser]
   SET [subscribe] = " + entity.subscribe + ",[nickname] = '" + entity.nickname + "',[sex] = " + entity.sex + ",[city] = '" + entity.city + "',[country] = '" + entity.country + "',[province] = '" + entity.province + "',[language] = '" + entity.language + "',[headimgurl] ='" + entity.headimgurl + "',[remark] = '" + entity.remark + "',[groupid] = " + entity.groupid + " ,[tagid_list] = '" + entity.tagid_list + "',[LastUpdateTime] = GETDATE(),[InvitationQR]='" + entity.InvitationQR + "',[Status] = 0 ";
                            update += sqlWhere;
                            cm.CommandText = update;
                            cm.ExecuteScalar();
                            trans.Commit();
                            result = true;
                        }
                        else
                        {
                            #region 插入用户信息

                            mtzUserId = categoryId == UserCategoryEnum.媒体主 ? UserId : mtzUserId;
                            ggzUserId = categoryId == UserCategoryEnum.广告主 ? UserId : ggzUserId;

                            var insert = @"INSERT INTO [dbo].[LE_WeiXinUser]
                                   ([subscribe],[openid],[nickname],[sex],[city],[country]
                                   ,[province],[language],[headimgurl],[subscribe_time]
                                   ,[unionid],[remark],[groupid],[tagid_list],[UserID],[CreateTime],[LastUpdateTime],[AuthorizeTime],[QRcode]
      ,[Inviter],[InvitationQR],[Status],[Source],[AdvertiserUserId],[UserType])
                              VALUES
                                   (" + entity.subscribe + ",'" + entity.openid + "','" + entity.nickname + "'," + entity.sex + ",'" + entity.city + "','" + entity.country + "','" + entity.province + "','" + entity.language + "','" + entity.headimgurl + "','" + entity.subscribe_time + "','" + entity.unionid + "','" + entity.remark + "'," + entity.groupid + ",'" + entity.tagid_list + "',"
                                   + mtzUserId + ",'" + entity.CreateTime + "','" + entity.LastUpdateTime + "',GETDATE(),'" + entity.QRcode + "','" + entity.Inviter + "','" + entity.InvitationQR + "'," + entity.Status + ","
                                   + entity.Source + "," + ggzUserId + "," + entity.UserType + ")";
                            cm.CommandText = insert;
                            cm.ExecuteScalar();

                            trans.Commit();
                            result = true;
                            #endregion
                        }
                    }
                    else
                    {

                        {
                            Loger.Log4Net.Info($" WeiXinUserOperation DAL  enter...openid:{entity.openid}，插入UserInfo");
                            #region 插入UserInfo
                            var sqluser = @"  INSERT  [Chitunion2017].dbo.UserInfo(  UserName  ,Mobile  ,Pwd  ,Type  ,
                                    Source  ,IsAuthMTZ  ,AuthAEUserID  ,IsAuthAE  ,SysUserID  ,EmployeeNumber  ,Status  ,
                                    CreateTime  , CreateUserID  ,
                                    LastUpdateTime  ,LastUpdateUserID  , Category  ,Email  , LockState  ,SleepState  ,LockType  ,SleepStatus,

                                    RegisterType,PromotionChannelID,RegisterIp)
                              VALUES
                                   ('" + entity.unionid + "'  ,''  ,''  ,1002  ," +
                                          "" + entity.RegisterFrom + "  ,0  ,null  ,0  ,null  ,null  ,0  ,GETDATE()  , 0  ,GETDATE()  ,0  , " + ((int)categoryId) + "  ,''  , 0  ,0  ,null  ,null," +

                                          entity.RegisterType + "," + entity.PromotionChannelID + ",'" + entity.RegisterIp + "') ;select SCOPE_IDENTITY()";

                            #endregion

                            cm.CommandText = sqluser;

                            UserId = Convert.ToInt32(cm.ExecuteScalar());

                            mtzUserId = categoryId == UserCategoryEnum.媒体主 ? UserId : mtzUserId;
                            ggzUserId = categoryId == UserCategoryEnum.广告主 ? UserId : ggzUserId;

                            #region 插入用户信息

                            var isExist = @"SELECT COUNT(0) FROM [Chitunion2017].[dbo].[LE_WeiXinUser] WHERE  [openid] = '" + entity.openid + "'";
                            cm.CommandText = isExist;
                            int num = Convert.ToInt32(cm.ExecuteScalar());

                            if (num > 0)//更新
                            {
                                var update = @"UPDATE [dbo].[LE_WeiXinUser]
   SET [subscribe] = " + entity.subscribe + ",[nickname] = '" + entity.nickname + "',[sex] = " + entity.sex +
                                             ",[city] = '" + entity.city + "',[country] = '" + entity.country +
                                             "',[province] = '" + entity.province + "',[language] = '" + entity.language +
                                             "',[headimgurl] ='" + entity.headimgurl + "',[remark] = '" + entity.remark +
                                             "',[groupid] = " + entity.groupid + " ,[tagid_list] = '" +
                                             entity.tagid_list + "',[LastUpdateTime] = GETDATE(),[Status] = 0 ";
                                if (categoryId == UserCategoryEnum.媒体主)
                                {
                                    update += $" ,[UserID] = {mtzUserId}";
                                }
                                else if (categoryId == UserCategoryEnum.广告主)
                                {
                                    update += $" ,[AdvertiserUserId] = {ggzUserId}";
                                }
                                update += " WHERE [openid] = '" + entity.openid + "'";

                                cm.CommandText = update;
                                cm.ExecuteScalar();
                                trans.Commit();
                                result = true;
                            }
                            else
                            {
                                var insert = @"INSERT INTO [dbo].[LE_WeiXinUser]
                                   ([subscribe],[openid],[nickname],[sex],[city],[country]
                                   ,[province],[language],[headimgurl],[subscribe_time]
                                   ,[unionid],[remark],[groupid],[tagid_list],[UserID],[CreateTime],[LastUpdateTime],[AuthorizeTime],[QRcode]
      ,[Inviter],[InvitationQR],[Status],[Source],[AdvertiserUserId],[UserType])
                              VALUES
                                   (" + entity.subscribe + ",'" + entity.openid + "','" + entity.nickname + "'," + entity.sex + ",'" + entity.city + "','" + entity.country + "','" + entity.province + "','" + entity.language + "','" + entity.headimgurl + "','" + entity.subscribe_time + "','" + entity.unionid + "','" + entity.remark + "'," + entity.groupid + ",'" + entity.tagid_list + "',"
                              + mtzUserId + ",'" + entity.CreateTime + "','" + entity.LastUpdateTime + "',GETDATE(),'" + entity.QRcode + "','" +
                              entity.Inviter + "','" + entity.InvitationQR + "'," + entity.Status + "," + entity.Source
                              + "," + ggzUserId + "," + entity.UserType + ")";
                                cm.CommandText = insert;
                                cm.ExecuteScalar();

                                trans.Commit();
                                result = true;
                            }

                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {

                    Loger.Log4Net.Info(" WeiXinUserOperation DAL ex" + ex.ToString());
                    trans.Rollback();
                }
                finally
                {
                    cnn.Close();
                    trans.Dispose();
                    cnn.Dispose();
                }
                Loger.Log4Net.Info($" WeiXinUserOperation DAL  end...openid:{entity.openid}");
                return result;
            }
        }

        /// <summary>
        /// 根据UnionID判断用户
        /// </summary>
        /// <param name="unionid"></param>
        /// <returns></returns>
        public bool IsExist(string unionid)
        {
            var sql = @"SELECT COUNT(0)
                      FROM [Chitunion2017].[dbo].[LE_WeiXinUser]
                      WHERE [Status]=0 AND [unionid] =@unionid";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@unionid",unionid)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? false : Convert.ToInt32(obj) > 0;
        }

        public bool IsExistOpneId(string OpneId, UserCategoryEnum userCategoryEnum = UserCategoryEnum.媒体主)
        {
            var sql = @"SELECT COUNT(0)
                      FROM [Chitunion2017].[dbo].[LE_WeiXinUser]
                      WHERE [openid] =@OpneId";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@OpneId",OpneId)
            };
            if (userCategoryEnum == UserCategoryEnum.媒体主)
            {
                sql += " AND [UserID] >0 ";
            }
            else if (userCategoryEnum == UserCategoryEnum.广告主)
            {
                sql += " AND [AdvertiserUserId] >0 ";
            }
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? false : Convert.ToInt32(obj) > 0;
        }

        public bool IsExistOpneIdAndUnionId(string OpneId, string UnionId)
        {
            var sql = @"SELECT COUNT(0)
                      FROM [Chitunion2017].[dbo].[LE_WeiXinUser]
                      WHERE [openid] =@OpneId AND  [unionid] = @UnionId";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@OpneId",OpneId),
                        new SqlParameter("@UnionId",UnionId)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? false : Convert.ToInt32(obj) > 0;
        }

        /// <summary>
        /// 根据OPENID获取UNIONID 和 USERID
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public Entities.DTO.V2_3.WXUserInfoRspDTO GetUnionAndUserId(string openid)
        {
            var sql = @"SELECT openid,[unionid]
                      ,[UserID],InvitationQR
                  FROM [Chitunion2017].[dbo].[LE_WeiXinUser]
                  WHERE [openid]=@OPENID";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@OPENID",openid)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return DataTableToEntity<Entities.DTO.V2_3.WXUserInfoRspDTO>(ds.Tables[0]);
        }

        public Entities.WeChat.WeiXinUser GetUserInfo(string openid)
        {
            var sql = @"SELECT [WeiXinUserID]
                              ,[subscribe]
                              ,[openid]
                              ,[nickname]
                              ,[sex]
                              ,[city]
                              ,[country]
                              ,[province]
                              ,[language]
                              ,[headimgurl]
                              ,[subscribe_time]
                              ,[unionid]
                              ,[remark]
                              ,[groupid]
                              ,[tagid_list]
                              ,[UserID]
                              ,[CreateTime]
                              ,[LastUpdateTime]
                              ,[AuthorizeTime]
                              ,[QRcode]
                              ,[Inviter]
                              ,[InvitationQR]
                              ,[Status],AdvertiserUserId
                          FROM [Chitunion2017].[dbo].[LE_WeiXinUser]
                  WHERE [openid]=@OPENID";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@OPENID",openid)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return DataTableToEntity<Entities.WeChat.WeiXinUser>(ds.Tables[0]);
        }


        public Entities.WeChat.WeiXinUser GetUserInfoByUserId(int userId, UserCategoryEnum userCategoryEnum)
        {
            var sql = @"SELECT [WeiXinUserID]
                              ,[subscribe]
                              ,[openid]
                              ,[nickname]
                              ,[sex]
                              ,[city]
                              ,[country]
                              ,[province]
                              ,[language]
                              ,[headimgurl]
                              ,[subscribe_time]
                              ,[unionid]
                              ,[remark]
                              ,[groupid]
                              ,[tagid_list]
                              ,[UserID]
                              ,[CreateTime]
                              ,[LastUpdateTime]
                              ,[AuthorizeTime]
                              ,[QRcode]
                              ,[Inviter]
                              ,[InvitationQR]
                              ,[Status],[AdvertiserUserId]
                          FROM [Chitunion2017].[dbo].[LE_WeiXinUser]
                  WHERE 1 = 1 ";
            if (userCategoryEnum == UserCategoryEnum.媒体主)
            {
                sql += " AND [UserID]=@UserId";
            }
            else if (userCategoryEnum == UserCategoryEnum.广告主)
            {
                sql += " AND [AdvertiserUserId]=@UserId";
            }
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserId",userId)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return DataTableToEntity<Entities.WeChat.WeiXinUser>(ds.Tables[0]);
        }


        public void UpdateStatusByOpneId(int status, DateTime updatetime, string openid)
        {
            int subscribe = 1;
            if (status == -1)
                subscribe = 0;
            var sql = $@"UPDATE [Chitunion2017].[dbo].[LE_WeiXinUser] SET [Status]=@Status,[subscribe]={subscribe},[LastUpdateTime] = @LastUpdateTime WHERE openid=@openid";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Status",status),
                        new SqlParameter("@LastUpdateTime",updatetime),
                        new SqlParameter("@openid",openid)
            };
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
        //public void UpdateStatusByOpneId(int status, DateTime updatetime, string openid)
        //{
        //    var sql = @"UPDATE [Chitunion2017].[dbo].[LE_WeiXinUser] SET [Status]=@Status,[LastUpdateTime] = @LastUpdateTime WHERE openid=@openid";
        //    var parameters = new SqlParameter[]{
        //                new SqlParameter("@Status",status),
        //                new SqlParameter("@LastUpdateTime",updatetime),
        //                new SqlParameter("@openid",openid)
        //    };
        //    SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        //}
        public void UpdateStatusByOpneId(int status, DateTime updatetime, DateTime subscribe_time, string openid)
        {
            var sql = @"UPDATE [Chitunion2017].[dbo].[LE_WeiXinUser] SET [Status]=@Status,[LastUpdateTime] = @LastUpdateTime,[subscribe_time] = @subscribe_time WHERE openid=@openid";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Status",status),
                        new SqlParameter("@LastUpdateTime",updatetime),
                        new SqlParameter("@subscribe_time",subscribe_time),
                        new SqlParameter("@openid",openid)
            };
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public void UpdateStatusByOpneIdAndUnionId(int status, DateTime updatetime, string openid, string unionid)
        {
            var sql = @"UPDATE [Chitunion2017].[dbo].[LE_WeiXinUser] SET [Status]=@Status,[LastUpdateTime] = @LastUpdateTime WHERE openid=@openid AND [unionid]=@unionid";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Status",status),
                        new SqlParameter("@LastUpdateTime",updatetime),
                        new SqlParameter("@openid",openid),
                        new SqlParameter("@unionid",unionid)
            };
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }


        public bool GetUserSence(int userid)
        {
            var sql = @"SELECT COUNT(0)
                          FROM [dbo].[LE_WXUserScene]
                          WHERE [Status]=0 AND [UserID]=@UserID";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserID",userid),
            };
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            return count > 0;
        }

        public List<Entities.WeChat.WeiXinUser> GetUsers()
        {
            var sql = @"SELECT [WeiXinUserID]
                              ,[subscribe]
                              ,[openid]
                              ,[nickname]
                              ,[sex]
                              ,[city]
                              ,[country]
                              ,[province]
                              ,[language]
                              ,[headimgurl]
                              ,[subscribe_time]
                              ,[unionid]
                              ,[remark]
                              ,[groupid]
                              ,[tagid_list]
                              ,[UserID]
                              ,[CreateTime]
                              ,[LastUpdateTime]
                              ,[AuthorizeTime]
                              ,[QRcode]
                              ,[Inviter]
                              ,[InvitationQR]
                              ,[Status],AdvertiserUserId
                          FROM [Chitunion2017].[dbo].[LE_WeiXinUser]";

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.WeChat.WeiXinUser>(ds.Tables[0]);
        }

        public int ExecuteNonQuery(string strSql)
        {
            if (strSql != "")
            {
                return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            }
            return 0;
        }


    }
}
