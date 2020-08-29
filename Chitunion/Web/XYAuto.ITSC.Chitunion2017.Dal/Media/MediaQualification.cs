/********************************************************
*创建人：lixiong
*创建时间：2017/5/12 18:25:18
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaQualification : DataBase
    {
        #region Instance

        public static readonly MediaQualification Instance = new MediaQualification();

        #endregion Instance

        public Entities.Media.MediaQualification GetInfo(int userId)
        {
            string sql = @"SELECT  MQ.* ,
                                    UR.Status AS AuditStatus
                            FROM    [dbo].[Media_Qualification] AS MQ WITH ( NOLOCK )
                                    LEFT JOIN dbo.UserDetailInfo AS UR WITH ( NOLOCK ) ON UR.UserID = MQ.CreateUserID
                            WHERE   MQ.Status = 0
                                    ";
            var paras = new List<SqlParameter>();

            if (userId > 0)
            {
                sql += " AND MQ.CreateUserID =" + userId;
            }

            sql += " ORDER BY MQ.RecID DESC";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaQualification>(data.Tables[0]);
        }

        public Entities.Media.MediaQualification GetUserDetailInfo(int userId)
        {
            string sql = @"
                            SELECT TrueName AS EnterpriseName,
                                   BLicenceURL AS BusinessLicense,
                                   IDCardFrontURL ,
                                   IDCardBackURL ,
                                   --OrganizationURL AS AgentContractFrontURL,
                                   Status AS AuditStatus
                                    FROM DBO.UserDetailInfo WITH(NOLOCK)
                                   WHERE UserID = @UserID ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@UserID",userId)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaQualification>(data.Tables[0]);
        }

        public Entities.Media.MediaQualification GetEntity(int mediaId = 0, int userId = 0,
            int mediaType = (int)MediaType.WeiXin)
        {
            string sql = @"SELECT MQ.* ,
                                    UR.Status AS AuditStatus
                            FROM    [dbo].[Media_Qualification] AS MQ WITH ( NOLOCK )
                                    LEFT JOIN dbo.UserDetailInfo AS UR WITH ( NOLOCK ) ON UR.UserID = MQ.CreateUserID
                                    WHERE MQ.Status = 0 AND MQ.MediaType = @MediaType";
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaType", (int)mediaType) };

            if (mediaId > 0)
            {
                sql += " AND MQ.MediaID = @MediaID ";
                paras.Add(new SqlParameter("@MediaID", mediaId));
            }
            if (userId > 0)
            {
                sql += " AND MQ.CreateUserID =" + userId;
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaQualification>(data.Tables[0]);
        }

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Insert(Entities.Media.MediaQualification entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Media_Qualification(");
            strSql.Append("MediaID,EnterpriseName,QualificationOne,QualificationTwo,BusinessLicense,CreateTime,CreateUserID,Status");
            strSql.Append(",IDCardFrontURL,IDCardBackURL,AgentContractFrontURL,AgentContractBackURL,MediaRelations,OperatingType,MediaType");
            strSql.Append(") values (");
            strSql.Append("@MediaID,@EnterpriseName,@QualificationOne,@QualificationTwo,@BusinessLicense,getdate(),@CreateUserID,@Status");
            strSql.Append(",@IDCardFrontURL,@IDCardBackURL,@AgentContractFrontURL,@AgentContractBackURL,@MediaRelations,@OperatingType,@MediaType");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@EnterpriseName",entity.EnterpriseName),
                        new SqlParameter("@QualificationOne",entity.QualificationOne),
                        new SqlParameter("@QualificationTwo",entity.QualificationTwo),
                        new SqlParameter("@BusinessLicense",entity.BusinessLicense),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@Status",entity.Status),
                         new SqlParameter("@IDCardFrontURL",entity.IDCardFrontURL),
                          new SqlParameter("@IDCardBackURL",entity.IDCardBackURL),
                           new SqlParameter("@AgentContractFrontURL",entity.AgentContractFrontURL),
                            new SqlParameter("@AgentContractBackURL",entity.AgentContractBackURL),
                             new SqlParameter("@MediaRelations",entity.MediaRelations),
                              new SqlParameter("@OperatingType",entity.OperatingType),
                               new SqlParameter("@MediaType",entity.MediaType),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Media.MediaQualification entity, int mediaUserId)
        {
            var strSql = new StringBuilder();
            strSql.Append("UPDATE  dbo.Media_Qualification ");
            strSql.Append(@"SET     EnterpriseName = @EnterpriseName ,
                                    QualificationOne = @QualificationOne,
                                    QualificationTwo = @QualificationTwo,
                                    BusinessLicense = @BusinessLicense,
                                    CreateUserID = @CreateUserID,
                                   [IDCardFrontURL] = @IDCardFrontURL
                                  ,[IDCardBackURL] = @IDCardBackURL
                                  ,[AgentContractFrontURL] = @AgentContractFrontURL
                                  ,[AgentContractBackURL] = @AgentContractBackURL
                                  ,[MediaRelations] = @MediaRelations
                                  ,[OperatingType] = @OperatingType
                                    ,[MediaType] = @MediaType
                            WHERE   MediaID = @MediaID ");
            //strSql.Append("UPDATE DBO.UserDetailInfo SET BLicenceURL = @BLicenceURL,Status = @Status ");
            //strSql.Append("WHERE UserID = @UserID ");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@EnterpriseName",entity.EnterpriseName),
                        new SqlParameter("@QualificationOne",entity.QualificationOne),
                        new SqlParameter("@QualificationTwo",entity.QualificationTwo),
                        new SqlParameter("@BusinessLicense",entity.BusinessLicense),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@IDCardBackURL",entity.IDCardBackURL),
                        new SqlParameter("@IDCardFrontURL",entity.IDCardFrontURL),
                        new SqlParameter("@AgentContractFrontURL",entity.AgentContractFrontURL),
                        new SqlParameter("@AgentContractBackURL",entity.AgentContractBackURL),
                        new SqlParameter("@MediaRelations",entity.MediaRelations),
                        new SqlParameter("@OperatingType",entity.OperatingType),
                        new SqlParameter("@MediaType",entity.MediaType)
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public int UpdateUserDetail(Entities.Media.MediaQualification entity,
            int mediaUserId)
        {
            var strSql = new StringBuilder();
            strSql.AppendFormat(@"
                    DECLARE @UserID INT = {0}
                    IF(EXISTS(SELECT 1 FROM DBO.UserDetailInfo WITH(NOLOCK) WHERE UserID= @UserID))
                    BEGIN
                        --修改UserInfo
                        UPDATE DBO.UserInfo SET [Type] = {4} WHERE UserID = @UserID
                        --修改UserDetailInfo
                        UPDATE dbo.UserDetailInfo SET BLicenceURL = '{1}',Status = {2}
                                                      ,IDCardFrontURL = '{5}',IDCardBackURL='{6}'
                                                      ,TrueName='{7}',LastUpdateUserID={8}
                        WHERE UserID = @UserID

                    END
                    ELSE
                    BEGIN
	                    INSERT INTO	dbo.UserDetailInfo
	                            ( UserID ,TrueName , BusinessID ,  ProvinceID , CityID , CounntyID , Contact ,  Address ,
	                              BLicenceURL , IDCardFrontURL ,  IDCardBackURL , CreateTime , CreateUserID , LastUpdateTime , LastUpdateUserID ,
	                              OrganizationURL,[Status]
	                            )
	                    SELECT UserID,{7},'',-2,-2,-2,'','','{1}','{5}','{6}',GETDATE(),{3},GETDATE(),{3},'',{2}
	                    FROM DBO.UserInfo WITH(NOLOCK) WHERE UserID = @UserID
                    END
                            ", mediaUserId, entity.BusinessLicense, (int)UserDetailInfoStatusEnum.AuditPass,
                            entity.CreateUserID, entity.OperatingType, entity.IDCardFrontURL, entity.IDCardBackURL,
                            entity.EnterpriseName, entity.CreateUserID);
            var parameters = new SqlParameter[]{
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public GetAppQualifucationResDTO GetQualificationInfo(int mediaID, int mediaType, int createUserID)
        {
            GetAppQualifucationResDTO res = new GetAppQualifucationResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID", mediaID),
                new SqlParameter("@MediaType",mediaType),
                new SqlParameter("@CreateUserID",createUserID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetQualificationInfo", parameters);
            List<Entities.Media.MediaQualification> qList = DataTableToList<Entities.Media.MediaQualification>(ds.Tables[0]);
            DataTable userInfoDt = ds.Tables[1];
            DataTable userDetailInfoDt = ds.Tables[2];
            if (qList == null || qList.Count.Equals(0))
            {
                #region 无资质

                res.OperatingType = Convert.ToInt32(userInfoDt.Rows[0]["Type"] == DBNull.Value ? -2 : userInfoDt.Rows[0]["Type"]);
                res.OperatingTypeName = userInfoDt.Rows[0]["TypeName"].ToString();
                if (userDetailInfoDt != null && userDetailInfoDt.Rows.Count > 0)
                {
                    res.EnterpriseName = userDetailInfoDt.Rows[0]["TrueName"].ToString();
                    res.BusinessLicense = userDetailInfoDt.Rows[0]["BLicenceURL"].ToString();
                    if (res.OperatingType.Equals(1002))
                    {//个人
                        res.Q1 = userDetailInfoDt.Rows[0]["IDCardFrontURL"].ToString();
                        res.Q2 = userDetailInfoDt.Rows[0]["IDCardBackURL"].ToString();
                    }
                }
                res.CanEdit = true;

                #endregion 无资质
            }
            else
            {
                #region 有资质

                var myItem = qList.FirstOrDefault(q => q.MediaID.Equals(mediaID));
                var recentlyItem = qList[0];
                //有个人项 媒体关系取资质表
                if (myItem != null)
                {
                    res.MediaRelations = myItem.MediaRelations;
                    res.MediaRelationsName = myItem.MediaRelationsName;
                }
                if (recentlyItem.AuditStatus.Equals(43001) || recentlyItem.AuditStatus.Equals(43002))
                {
                    res.CanEdit = false;
                }
                else
                {
                    res.CanEdit = true;
                }
                res.OperatingType = recentlyItem.OperatingType;
                res.OperatingTypeName = recentlyItem.OperatingTypeName;
                res.EnterpriseName = recentlyItem.EnterpriseName;
                res.BusinessLicense = recentlyItem.BusinessLicense;
                if (recentlyItem.OperatingType.Equals(1002))
                {//个人
                    res.Q1 = recentlyItem.IDCardFrontURL;
                    res.Q2 = recentlyItem.IDCardBackURL;
                }
                else if (myItem != null)
                {
                    res.Q1 = myItem.AgentContractFrontURL;
                    res.Q2 = myItem.AgentContractBackURL;
                }

                //if (qList.Count(q => q.AuditStatus.Equals(43002)) > 0)
                //{
                //    #region 有审核过的

                //    res.CanEdit = false;
                //    res.OperatingType = Convert.ToInt32(userInfoDt.Rows[0]["Type"] == DBNull.Value ? -2 : userInfoDt.Rows[0]["Type"]);
                //    res.OperatingTypeName = userInfoDt.Rows[0]["TypeName"].ToString();
                //    if (userDetailInfoDt != null && userDetailInfoDt.Rows.Count > 0)
                //    {
                //        res.EnterpriseName = userDetailInfoDt.Rows[0]["TrueName"].ToString();
                //        res.BusinessLicense = userDetailInfoDt.Rows[0]["BLicenceURL"].ToString();
                //        if (res.OperatingType.Equals(1002))
                //        {//个人
                //            res.Q1 = userDetailInfoDt.Rows[0]["IDCardFrontURL"].ToString();
                //            res.Q2 = userDetailInfoDt.Rows[0]["IDCardBackURL"].ToString();
                //        }
                //        else if (myItem != null)
                //        {
                //            res.Q1 = myItem.AgentContractFrontURL;
                //            res.Q2 = myItem.AgentContractBackURL;
                //        }
                //    }

                //    #endregion 有审核过的
                //}
                //else if (qList.Count(q => q.AuditStatus.Equals(43001)) > 0)
                //{
                //    #region 有待审核的

                //    res.CanEdit = false;
                //    var recentlyItem = qList.Where(q => q.AuditStatus.Equals(43001)).OrderByDescending(q => q.CreateTime).First();
                //    res.OperatingType = recentlyItem.OperatingType;
                //    res.OperatingTypeName = recentlyItem.OperatingTypeName;
                //    res.EnterpriseName = recentlyItem.EnterpriseName;
                //    res.BusinessLicense = recentlyItem.BusinessLicense;
                //    if (recentlyItem.OperatingType.Equals(1002))
                //    {//个人
                //        res.Q1 = recentlyItem.IDCardFrontURL;
                //        res.Q2 = recentlyItem.IDCardBackURL;
                //    }
                //    else if (myItem != null)
                //    {
                //        res.Q1 = myItem.AgentContractFrontURL;
                //        res.Q2 = myItem.AgentContractBackURL;
                //    }

                //    #endregion 有待审核的
                //}
                //else
                //{
                //    #region 全部为被驳回的

                //    res.CanEdit = true;
                //    res.OperatingType = Convert.ToInt32(userInfoDt.Rows[0]["Type"] == DBNull.Value ? -2 : userInfoDt.Rows[0]["Type"]);
                //    res.OperatingTypeName = userInfoDt.Rows[0]["TypeName"].ToString();
                //    if (userDetailInfoDt != null && userDetailInfoDt.Rows.Count > 0)
                //    {
                //        res.EnterpriseName = userDetailInfoDt.Rows[0]["TrueName"].ToString();
                //        res.BusinessLicense = userDetailInfoDt.Rows[0]["BLicenceURL"].ToString();
                //        if (res.OperatingType.Equals(1002))
                //        {//个人
                //            res.Q1 = userDetailInfoDt.Rows[0]["IDCardFrontURL"].ToString();
                //            res.Q2 = userDetailInfoDt.Rows[0]["IDCardBackURL"].ToString();
                //        }
                //        else if (myItem != null)
                //        {
                //            res.Q1 = myItem.AgentContractFrontURL;
                //            res.Q2 = myItem.AgentContractBackURL;
                //        }
                //    }

                //    #endregion 全部为被驳回的
                //}

                #endregion 有资质
            }
            return res;
        }
    }
}