using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;
using XYAuto.ITSC.Chitunion2017.Entities.UserManage;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.UserManage
{
    public class UserManage : DataBase
    {
        public readonly static UserManage Instance = new UserManage();

        public Tuple<DataTable, DataTable, DataTable> QueryUserBasicInfo(int userID)
        {
            var sbSql = new StringBuilder();

            sbSql.Append($@"
                            SELECT  UI.UserID ,
                                    UI.UserName ,
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
                                    UI.LastUpdateUserID,
                                    UD.ProvinceID ,
                                    UD.CityID,
                                    UD.Address ,
                                    UI.RegisterType
                            FROM    dbo.UserInfo UI
                            LEFT JOIN dbo.UserDetailInfo UD ON UI.UserID=UD.UserID
                            WHERE   UI.UserID = {userID};

                            SELECT  UD.IdentityNo ,
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
                                    UD.Status ,
                                    UD.Reason ,
                                    UI.Type,
                                    UD.Sex
                            FROM    dbo.UserDetailInfo UD
                                    JOIN dbo.UserInfo UI ON UI.UserID = UD.UserID
                            WHERE   UD.UserID = {userID};


                            SELECT  UBK.AccountName ,
                                    UBK.AccountType
                            FROM    dbo.LE_UserBankAccount UBK
                            WHERE   UBK.UserID = {userID}
                                    AND UBK.AccountType = {(int)Entities.UserManage.AccountType.支付宝}; ");

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return new Tuple<DataTable, DataTable, DataTable>(ds.Tables[0], ds.Tables[1], ds.Tables[2]);
        }

        public bool EditUserBasicInfo(int userID, string userName, int provinceID, int cityID, string address, ref string errormsg)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var userInfoAll = Dal.UserManage.UserInfo.Instance.GetModel(userID);
                        //userInfoAll.UserName = userName;
                        //if (!string.IsNullOrEmpty(userName))
                        //    userInfoAll.UserName = userName;
                        userInfoAll.ProvinceID = provinceID;
                        userInfoAll.CityID = cityID;
                        userInfoAll.Address = address;
                        Dal.UserManage.UserInfo.Instance.Update(userInfoAll, trans);
                        if (!UserDetailInfo.Instance.IsExistsByUserID(userID))
                            UserDetailInfo.Instance.Insert(userInfoAll, 0, trans);
                        else
                            UserDetailInfo.Instance.Update(userInfoAll, trans);
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        errormsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool EditUserAuthenticationInfo(int userID, int type, string trueName, string bLicenceURL, string identityNo, string idCardFrontURL, int AuditStatus, ref string errormsg, int AccountType = 0, int status = 0, string AccountName = "", string Mobile = "")
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var userInfoAll = Dal.UserManage.UserInfo.Instance.GetModel(userID);
                        userInfoAll.Type = type;
                        userInfoAll.TrueName = trueName;
                        if (!string.IsNullOrEmpty(Mobile))
                        {
                            userInfoAll.Mobile = Mobile;
                        }
                        if ((int)Entities.UserManage.Type.企业 == type)
                        {
                            userInfoAll.BLicenceURL = bLicenceURL;
                            //清空个人信息
                            userInfoAll.IdentityNo = string.Empty;
                            userInfoAll.IDCardFrontURL = string.Empty;
                        }
                        else if ((int)Entities.UserManage.Type.个人 == type)
                        {
                            userInfoAll.IdentityNo = identityNo;
                            userInfoAll.IDCardFrontURL = idCardFrontURL;
                            //清空企业信息
                            userInfoAll.BLicenceURL = string.Empty;
                        }
                        UserInfo.Instance.Update(userInfoAll, trans);
                        if (!UserDetailInfo.Instance.IsExistsByUserID(userID))
                            UserDetailInfo.Instance.Insert(userInfoAll, status, trans);
                        else
                            UserDetailInfo.Instance.Update(userInfoAll, trans);
                        if (AccountType > 0)
                            Dal.LETask.LeUserBankAccount.Instance.TransUpdate(userID, AccountType, AccountName, trans);

                        UserDetailInfo.Instance.UpdateStatus(userInfoAll.UserID, AuditStatus, trans);
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        errormsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        #region V2.3微信版
        /// <summary>
        /// 根据手机号查询用户信息
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public Tuple<DataTable, DataTable, DataTable> QueryUserInfo(string Mobile, int Category)
        {
            var sbSql = new StringBuilder();

            sbSql.Append($@" 
                            SELECT UI.Mobile
                            FROM    dbo.UserInfo UI
                            WHERE    UI.Mobile = '{StringHelper.SqlFilter(Mobile)}';

                            SELECT  UD.IdentityNo ,
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
                                    UD.Status ,
                                    UD.Reason ,
                                    UI.Type
                            FROM    dbo.UserDetailInfo UD
                                    JOIN dbo.UserInfo UI ON UI.UserID = UD.UserID
                            WHERE   UI.Mobile = '{StringHelper.SqlFilter(Mobile)}' AND Category={Category};

                            SELECT  UBK.AccountName ,
                                    UBK.AccountType
                            FROM    dbo.LE_UserBankAccount UBK
                                    JOIN dbo.UserInfo UI ON UI.UserID = UBK.UserID
                            WHERE    UI.Mobile = '{StringHelper.SqlFilter(Mobile)}'
                                    AND UBK.AccountType = {(int)Entities.UserManage.AccountType.支付宝}; ");

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return new Tuple<DataTable, DataTable, DataTable>(ds.Tables[0], ds.Tables[1], ds.Tables[2]);
        }
        /// <summary>
        /// 判断是提现账号是否存在
        /// </summary>
        /// <param name="AccountType"></param>
        /// <param name="AccountName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool IsSelfAccount(int AccountType, string AccountName, int UserID = 0)
        {
            SqlParameter para = new SqlParameter("@AccountName", SqlDbType.VarChar, 50);
            para.Value = AccountName;
            string sql = $@"
                    SELECT count(1)
                    FROM    LE_UserBankAccount AS UB 
                    WHERE  UB.AccountName = @AccountName AND UB.AccountType = {AccountType}  
                    ";
            if (UserID > 0)
            {
                sql += $" AND UserID = { UserID }";
            }
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, para);
            return obj == null ? false : (Convert.ToInt32(obj) > 0 ? true : false);
        }
        /// <summary>
        /// 查询是否存在其他支付账号
        /// </summary>
        /// <param name="AccountType">账号类型</param>
        /// <param name="AccountName">账号名称</param>
        /// <param name="ListUserID">同一个手机号对应的用户id</param>
        /// <returns></returns>
        public bool IsOtherAccount(int AccountType, string AccountName, List<int> ListUserID)
        {
            SqlParameter para = new SqlParameter("@AccountName", SqlDbType.VarChar, 50);
            para.Value = AccountName;
            string sql = $@"
                    SELECT count(1)
                    FROM    LE_UserBankAccount AS UB 
                    WHERE  UB.AccountName = @AccountName AND UB.AccountType = {AccountType}  AND UB.status =0 ";
            for (int i = 0; i < ListUserID.Count; i++)
            {
                sql += $" AND UserID != { ListUserID[i] }";
            }
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, para);
            return obj == null ? false : (Convert.ToInt32(obj) > 0 ? true : false);
        }
        /// <summary>
        /// 获取审核状态和微信昵称
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataSet GetExamineStatus(int UserId)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT  Status  FROM  dbo.UserDetailInfo WHERE UserID={UserId}
                             SELECT nickname FROM LE_WeiXinUser WHERE UserID={UserId} ");
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
        }
        /// <summary>
        /// 获取审核状态
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int GetUserDetailStatus(int UserId)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT  Status  FROM  dbo.UserDetailInfo WHERE UserID={UserId}");
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 判断提现账号是否存在（非本人的）
        /// </summary>
        /// <param name="AccountType"></param>
        /// <param name="AccountName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool IsAccount(int AccountType, string AccountName, int UserID)
        {
            SqlParameter para = new SqlParameter("@AccountName", SqlDbType.VarChar, 50);
            para.Value = AccountName;
            string sql = $@"
                    SELECT count(1)
                    FROM    LE_UserBankAccount AS UB 
                    WHERE  UB.AccountName = @AccountName AND UB.AccountType = {AccountType} AND UserID != { UserID }";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, para);
            return obj == null ? false : (Convert.ToInt32(obj) > 0 ? true : false);
        }
        /// <summary>
        /// 合并用户基本数据（老的用户覆盖新的）
        /// </summary>
        /// <param name="OldUserId">老用户ID</param>
        /// <param name="NewUserId">新用户ID</param>
        /// <returns></returns>
        public bool CleanUserInfo(int Type, int OldUserId, int NewUserId, string trueName, string bLicenceURL, string identityNo, string idCardFrontURL, int AuditStatus, int AccountType, string AccountName)
        {
            SqlParameter[] paras =  {
                new  SqlParameter("@OldUserId",SqlDbType.Int),
                new  SqlParameter("@NewUserId",SqlDbType.Int),
                new  SqlParameter("@errorSum",SqlDbType.Int),
                new  SqlParameter("@TrueName",SqlDbType.VarChar,200),
                new  SqlParameter("@BLicenceURL",SqlDbType.VarChar,200),
                new  SqlParameter("@IDCardFrontURL",SqlDbType.VarChar,200),
                new  SqlParameter("@IdentityNo",SqlDbType.VarChar,200),
                new  SqlParameter("@AccountName",SqlDbType.VarChar,200),
                new  SqlParameter("@AccountType",SqlDbType.Int),
                new  SqlParameter("@Status",SqlDbType.Int),
                new SqlParameter("@return",SqlDbType.Int),
                new SqlParameter("@Type",SqlDbType.Int),
            };
            paras[0].Value = OldUserId;
            paras[1].Value = NewUserId;
            paras[2].Direction = ParameterDirection.Output;
            paras[3].Value = trueName;
            paras[4].Value = bLicenceURL;
            paras[5].Value = idCardFrontURL;
            paras[6].Value = identityNo;
            paras[7].Value = AccountName;
            paras[8].Value = AccountType;
            paras[9].Value = AuditStatus;
            paras[10].Direction = ParameterDirection.ReturnValue;
            paras[11].Value = Type;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CleanUserInfo", paras);
            int error = (int)(paras[10].Value);
            return error > 0 ? false : true;
        }
        /// <summary>
        /// 合并用户关联数据（老的用户覆盖新的）
        /// </summary>
        /// <param name="OldUserId">老用户ID</param>
        /// <param name="NewUserId">新用户ID</param>
        /// <returns></returns>
        public bool CleanUserRelation(int OldUserId, int NewUserId)
        {
            SqlParameter[] paras =  {
                new  SqlParameter("@OldUserId",SqlDbType.Int),
                new  SqlParameter("@NewUserId",SqlDbType.Int),
                new  SqlParameter("@errorSum",SqlDbType.Int),
                new SqlParameter("@return",SqlDbType.Int)
            };
            paras[0].Value = OldUserId;
            paras[1].Value = NewUserId;
            paras[2].Direction = ParameterDirection.Output;
            paras[3].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CleanUserRelation", paras);
            int error = (int)(paras[3].Value);
            return error > 0 ? false : true;
        }
        #endregion

        /// <summary>
        /// auth:lixiong
        /// desc:pc端用户清洗
        /// </summary>
        /// <param name="oldUserId"></param>
        /// <param name="newUserId"></param>
        /// <param name="provinceId"></param>
        /// <param name="cityId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool CleanUserInfoPc(int oldUserId, int newUserId, int provinceId, int cityId, string address)
        {
            const string storedProcedure = "p_CleanUserInfo_PC";

            var outParam = new SqlParameter("@errorSum", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@OldUserId",oldUserId),
                new SqlParameter("@NewUserId",newUserId),
                new SqlParameter("@ProvinceId",provinceId),
                new SqlParameter("@CityId",cityId),
                new SqlParameter("@Address",address)
            };

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, storedProcedure, sqlParams);

            return Convert.ToInt32(sqlParams[0].Value) == 0;
        }
        #region V2.5微信版
        /// <summary>
        /// 查询个人信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable QueryUserInfo(int UserId)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  W.Nickname ,
                            U.Mobile ,
                            B.AccountName ,
                            B.AccountType ,
                            D.Status,
                            W.Status as IsFollow
                    FROM    dbo.UserInfo U
                            LEFT JOIN dbo.v_LE_WeiXinUser W ON U.UserID = W.UserID
                            LEFT JOIN LE_UserBankAccount B ON U.UserID = B.UserID
                                                              AND B.Status = 0
                            LEFT JOIN dbo.UserDetailInfo D ON U.UserID = D.UserID
                    WHERE   U.Status = 0
                            AND U.UserID = {UserId};  ");

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 查询认证信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable QueryUserDetailInfo(int UserId)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                           
                            SELECT  UD.IdentityNo ,
                                    UD.TrueName ,
                                    UD.BLicenceURL ,
                                    UD.IDCardFrontURL ,
                                    UD.Status ,
                                    UD.Reason ,
                                    UI.Type
                            FROM    dbo.UserDetailInfo UD
                                    JOIN dbo.UserInfo UI ON UI.UserID = UD.UserID
                            WHERE   UD.UserID = {UserId}; ");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 合并用户基本数据（老的用户覆盖新的
        /// </summary>
        /// <param name="OldUserId">老用户ID</param>
        /// <param name="NewUserId">新用户ID</param>
        /// <returns></returns>
        public bool CleanUserInfo(int OldUserId, int NewUserId)
        {
            SqlParameter[] paras =  {
                new  SqlParameter("@OldUserId",SqlDbType.Int),
                new  SqlParameter("@NewUserId",SqlDbType.Int),
                new  SqlParameter("@errorSum",SqlDbType.Int),

            };
            paras[0].Value = OldUserId;
            paras[1].Value = NewUserId;
            paras[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CleanUserInfo_M", paras);
            int error = (int)(paras[2].Value);
            return error > 0 ? false : true;
        }
        #endregion

        #region V2.1.0
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetUserInfo(int userId)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                           SELECT  W.nickname ,
                            W.headimgurl ,
                            U.Mobile ,
                            D.TrueName ,
                            D.Sex ,
                            D.IdentityNo ,
                            B.AccountName ,
                            B.AccountType ,
                            D.Status
                    FROM    dbo.UserInfo U
                            LEFT JOIN dbo.v_LE_WeiXinUser W ON U.UserID = W.UserID
                            LEFT JOIN LE_UserBankAccount B ON U.UserID = B.UserID
                                                              AND B.Status = 0
                            LEFT JOIN dbo.UserDetailInfo D ON U.UserID = D.UserID
                    WHERE   U.Status = 0 AND U.UserID = {userId};  ");

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 根据手机号查个人信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public DataTable GetUserInfoByMobile(string mobile)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                           SELECT  W.nickname ,
                            W.headimgurl ,
                            U.Mobile ,
                            D.TrueName ,
                            D.Sex ,
                            D.IdentityNo ,
                            B.AccountName ,
                            B.AccountType ,
                            D.Status
                    FROM    dbo.UserInfo U
                            LEFT JOIN dbo.v_LE_WeiXinUser W ON U.UserID = W.UserID
                            LEFT JOIN LE_UserBankAccount B ON U.UserID = B.UserID
                                                              AND B.Status = 0
                            LEFT JOIN dbo.UserDetailInfo D ON U.UserID = D.UserID
                    WHERE   U.Status = 0 AND U.Mobile='{StringHelper.SqlFilter(mobile)}' ");

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userInfoAll"></param>
        /// <returns></returns>
        public string AddUserInfo(UserInfoAll userInfoAll)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (!UserDetailInfo.Instance.IsExistsByUserID(userInfoAll.UserID))
                            UserDetailInfo.Instance.Insert(userInfoAll, userInfoAll.Status, trans);
                        else
                            UserDetailInfo.Instance.Update(userInfoAll, trans);
                        Dal.UserManage.UserInfo.Instance.UpdateMobile(userInfoAll.Mobile, userInfoAll.UserID, trans);
                        Dal.LETask.LeUserBankAccount.Instance.TransUpdate(userInfoAll.UserID, userInfoAll.AccountType, userInfoAll.AccountName, trans);
                        trans.Commit();
                        return "";
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return ex.Message;
                    }
                }
            }
        }
        #endregion

        //public int LoginForWeChat(Entities.DTO.ReqLoginDTO req, ref string errormsg)
        //{
        //    using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
        //    {
        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                var userInfo = Dal.UserManage.LE_WeiXinUser.Instance.GetModel(req.openid);
        //                if (userInfo != null)
        //                    return userInfo.UserID;

        //                //int userId = Dal.UserManage.UserInfo.Instance.InsertUser("", "", (int)Entities.UserManage.Type.个人, (int)Entities.Enum.LeTask.RegisterFromEnum.APP, (int)Entities.ChituMedia.UserCategoryEnum.媒体主, (int)Entities.Enum.LeTask.RegisterTypeEnum.H5, 1);
        //                //Dal.UserManage.LE_WeiXinUser.Instance.Insert(new Entities.UserManage.LE_WeiXinUser() { openid=req.openid,nickname=req.nickname}, trans);                        
        //                trans.Commit();
        //                return userId;
        //            }
        //            catch (Exception ex)
        //            {
        //                trans.Rollback();
        //                errormsg = ex.Message;
        //                return -2;
        //            }
        //        }
        //    }
        //}
    }
}
