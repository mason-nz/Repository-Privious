using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.UserManage
{
    public class UserDetailInfo : DataBase
    {
        public readonly static UserDetailInfo Instance = new UserDetailInfo();
        public bool Update(Entities.UserManage.UserInfoAll model, SqlTransaction trans = null)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                UPDATE  dbo.UserDetailInfo
                                SET     TrueName = '{model.TrueName}' ,
                                        ProvinceID = {model.ProvinceID} ,
                                        CityID = {model.CityID} , 
                                        Address = '{model.Address}' ,
                                        BLicenceURL = '{model.BLicenceURL}' ,
                                        OrganizationURL = '{model.OrganizationURL}' ,
                                        IDCardFrontURL = '{model.IDCardFrontURL}' ,
                                        IDCardBackURL = '{model.IDCardBackURL}' ,
                                        IdentityNo = '{model.IdentityNo}' ,
                                        LastUpdateTime = GETDATE(),
                                        sex={model.Sex}
                                WHERE   UserID = {model.UserID};");

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString());

            return rowcount > 0;
        }
        public bool Update(Entities.UserManage.UserInfoAll model, int Status)
        {
            string AuditTime = Status > 1 ? "GETDATE()" : "NULL";
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                UPDATE  dbo.UserDetailInfo
                                SET     TrueName = '{model.TrueName}' ,
                                        ProvinceID = {model.ProvinceID} ,
                                        CityID = {model.CityID} , 
                                        Address = '{model.Address}' ,
                                        BLicenceURL = '{model.BLicenceURL}' ,
                                        OrganizationURL = '{model.OrganizationURL}' ,
                                        IDCardFrontURL = '{model.IDCardFrontURL}' ,
                                        IDCardBackURL = '{model.IDCardBackURL}' ,
                                        IdentityNo = '{model.IdentityNo}' ,
                                        LastUpdateTime = GETDATE(),
                                        Status={Status},
                                        ApplyTime=GETDATE(),
                                        AuditTime={AuditTime}
                                WHERE   UserID = {model.UserID};");
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return rowcount > 0;
        }
        public bool UpdateStatus(int userID, int status, SqlTransaction trans = null)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                UPDATE  dbo.UserDetailInfo
                                SET     Status = {status} ,
                                        ApplyTime = GETDATE() ,
                                        LastUpdateTime = GETDATE()
                                WHERE   UserID = {userID};");

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString());

            return rowcount > 0;
        }
        public int Insert(Entities.UserManage.UserInfoAll model, int status, SqlTransaction trans = null)
        {
            string AuditTime = status > 1 ? "GETDATE()" : "NULL";
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                INSERT dbo.UserDetailInfo
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
                                          OrganizationURL ,
                                          Status ,
                                          IdentityNo,
                                          ApplyTime,
                                          AuditTime,
                                          Sex
                                        )
                                VALUES  ( {model.UserID} , -- UserID - int
                                          '{model.TrueName}' , -- TrueName - varchar(200)
                                          {model.BusinessID} , -- BusinessID - int
                                          {model.ProvinceID} , -- ProvinceID - int
                                          {model.CityID} , -- CityID - int
                                          {model.CounntyID} , -- CounntyID - int
                                          '{model.Contact}' , -- Contact - varchar(50)
                                          '{model.Address}' , -- Address - varchar(200)
                                          '{model.BLicenceURL}' , -- BLicenceURL - varchar(200)
                                          '{model.IDCardFrontURL}' , -- IDCardFrontURL - varchar(200)
                                          '{model.IDCardBackURL}' , -- IDCardBackURL - varchar(200)
                                           GETDATE() , -- CreateTime - datetime
                                          {model.CreateUserID} , -- CreateUserID - int
                                          '{model.OrganizationURL}' , -- OrganizationURL - varchar(200)
                                          {status} ,
                                           '{model.IdentityNo}',
                                           GETDATE(),
                                           {AuditTime},
                                           {model.Sex}
                                        );");

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString());

            return rowcount;
        }
        public int Delete(int userID, SqlTransaction trans = null)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"DELETE FROM dbo.UserDetailInfo WHERE UserID={userID};");

            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sbSql.ToString());

            return rowcount;
        }
        public bool IsExistsByUserID(int userID)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                SELECT  COUNT(1)
                                FROM    dbo.UserDetailInfo
                                WHERE   UserID = {userID};");

            var rowcount = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());

            return Convert.ToInt32(rowcount) > 0;
        }
        #region 判断身份证号是否非本人重复
        public bool IsExistsIdentityNo(int userID, int category, string identityNo)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                SELECT  COUNT(1)
                                FROM    dbo.UserDetailInfo UD
                                        JOIN dbo.UserInfo UI ON UI.UserID = UD.UserID
                                WHERE   UI.Category = {category} 
                                        AND UD.UserID <> {userID} 
                                        AND UD.IdentityNo = '{identityNo}';");

            var rowcount = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());

            return Convert.ToInt32(rowcount) > 0;
        }
        #endregion
        #region V2.3微信版
        /// <summary>
        /// 判断身份证号是否非本人重复
        /// </summary>
        /// <param name="listUserId">同一个手机号的用户ID</param>
        /// <param name="category">用户分类</param>
        /// <param name="identityNo">身份证号</param>
        /// <returns></returns>
        public bool IsExistsIdentityNo(List<int> listUserId, int category, string identityNo)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"
                                SELECT  COUNT(1)
                                FROM    dbo.UserDetailInfo UD
                                        JOIN dbo.UserInfo UI ON UI.UserID = UD.UserID
                                WHERE   UI.Category = {category} 
                                        AND UD.IdentityNo = '{identityNo}' AND UD.status=0");
            for (int i = 0; i < listUserId.Count(); i++)
            {
                sbSql.AppendLine($" AND UD.UserID <> { listUserId[i]}");

            }
            var rowcount = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());

            return Convert.ToInt32(rowcount) > 0;
        }
        #endregion
    }
}
