using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.ChituMedia
{
    public class MediaUserDa : DataBase
    {
        #region 单例
        private MediaUserDa() { }

        static MediaUserDa instance = null;
        static readonly object padlock = new object();

        public static MediaUserDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new MediaUserDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 批量启用、禁用

        /// <summary>
        /// 批量启用、禁用
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int UserEnableOrDisable(UserBatchQueryArgs queryArgs)
        {


            string SQL = @"exec('UPDATE dbo.UserInfo SET Status='+@Status+' WHERE UserID IN ('+@UserID+') AND Category='+@Category+' ')";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID", string.Join(",",queryArgs.UserIDList.ToArray())),
                new SqlParameter("@Status", queryArgs.Status),
                new SqlParameter("@Category",(int)((UsereCategory)Enum.Parse(typeof(UsereCategory), queryArgs.ListType)))
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
        }

        #endregion

        #region 获取广告主、媒体主列表

        /// <summary>
        /// 获取广告主、媒体主列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public Tuple<int, List<MediaUserModel>> GetMediaUserList(UserQueryArgs queryArgs, EnumInfo order)
        {
            StringBuilder SQL = new StringBuilder(@"
                                    SELECT * YanFaFROM(SELECT  U.UserID ,
                                            U.UserName ,
                                            U.Mobile ,
                                            D.DictName AS RegisterFromName,
											DT.DictName AS RegisterTypeName,
                                            U.Source AS SourceID ,
                                            U.CreateTime ,
                                            U.Status ,
											(CASE u.Status WHEN 0 THEN '启用' ELSE '禁用' END ) AS StatusName,
                                            UD.Status AS ApproveStatus ,
                                            ( CASE ISNULL(UD.Status,0)
                                                WHEN 0 THEN '未认证'
                                                WHEN 1 THEN '待审核'
                                                WHEN 2 THEN '已认证'
                                                WHEN 3 THEN '认证未通过'
                                              END ) AS ApproveStatusName ,
											UD.ApplyTime,
											UD.AuditTime,
                                            UD.Reason ,
                                            U.Category,
                                            WU.nickname,
											(CASE  WHEN WU.Status=0 THEN '关注中' WHEN WU.Status=-1 THEN '取消关注' ELSE '无' END ) AS AttentionName

                                    FROM    (SELECT *
                                                                FROM
                                                                (
                                                                    SELECT ROW_NUMBER() OVER (PARTITION BY UserName
                                                                                              ORDER BY CreateTime
                                                                                             ) rowNum,
                                                                        *
                                                                    FROM dbo.UserInfo
                                                                    WHERE Status >= 0
                                                                ) AS A
                                                                WHERE rowNum = 1) AS U
                                            LEFT JOIN v_PromotionChannelList AS PC ON PC.DictID=U.PromotionChannelID

											LEFT JOIN v_LE_WeiXinUser AS WU ON wu.UserID=u.UserID
                                            LEFT JOIN dbo.UserDetailInfo AS UD ON U.UserID = UD.UserID
                                            LEFT JOIN dbo.DictInfo AS D ON D.DictId = U.Source AND D.Status=0
											LEFT JOIN dbo.DictInfo AS  DT ON DT.DictId=U.RegisterType
                                    WHERE   U.Category = " + (int)((UsereCategory)Enum.Parse(typeof(UsereCategory), queryArgs.ListType)) + " AND U.Status>=0 ");
            if (!string.IsNullOrEmpty(queryArgs.UserName))
            {
                SQL.Append(" AND U.UserName Like '%" + queryArgs.UserName + "%' ");
            }
            if (queryArgs.ApproveStatus >= 0)
            {

                SQL.Append(" AND ISNULL(UD.Status,0)=" + queryArgs.ApproveStatus + "");
            }
            if (!string.IsNullOrEmpty(queryArgs.Mobile))
            {
                SQL.Append(" AND U.Mobile LIKE '%" + queryArgs.Mobile + "%' ");
            }
            if (queryArgs.AttentionStatus > -2)
            {
                SQL.Append($" AND WU.Status={queryArgs.AttentionStatus} ");
            }
            if (queryArgs.AttentionStatus == -3)
            {
                SQL.Append($" AND WU.Status IS NULL ");
            }
            if (queryArgs.RegisterFrom > 0)
            {
                SQL.Append(" AND U.Source=" + queryArgs.RegisterFrom + " ");
            }
            if (queryArgs.RegisterType > 0)
            {
                SQL.Append(" AND U.RegisterType=" + queryArgs.RegisterType + " ");
            }
            if (queryArgs.Status >= 0)
            {
                SQL.Append(" AND U.Status=" + queryArgs.Status + " ");
            }
            if (queryArgs.Level1 >= 0)
            {
                SQL.Append(" AND PC.Level1=" + queryArgs.Level1 + " ");
            }
            if (!string.IsNullOrEmpty(queryArgs.BeginTime))
            {
                SQL.Append(" AND U.CreateTime>='" + queryArgs.BeginTime + "' ");
            }
            if (!string.IsNullOrEmpty(queryArgs.EndTime))
            {
                SQL.Append(" AND U.CreateTime<='" + Convert.ToDateTime(queryArgs.EndTime).AddDays(1).ToString() + "' ");
            }
            SQL.Append(" ) as A ");
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",queryArgs.PageSize),
                new SqlParameter("@CurPage",queryArgs.PageIndex),
                new SqlParameter("@Order",order==null?"  CreateTime DESC ":order.Description)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);

            return new Tuple<int, List<MediaUserModel>>(totalCount, DataTableToList<MediaUserModel>(data.Tables[0]));

        }
        #endregion

        #region 获取推广渠道

        public List<PromotionChannelModel> GetPromotionChannelList(int Level)
        {
            string SQL = @"SELECT DictID,ChannelName FROM dbo.v_PromotionChannelList WHERE Level=@Level";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@Level",Level)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);


            return DataTableToList<PromotionChannelModel>(data.Tables[0]);

        }
        #endregion

        #region 批量更新密码

        /// <summary>
        /// 批量更新密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int UserResetPassword(UserBatchQueryArgs queryArgs, string pwd)
        {


            string SQL = @"exec('UPDATE dbo.UserInfo SET Pwd='''+@Pwd+''' WHERE UserID IN ('+@UserID+') AND Category='+@Category+' ')";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID", string.Join(",",queryArgs.UserIDList.ToArray())),
                new SqlParameter("@Category",(int)((UsereCategory)Enum.Parse(typeof(UsereCategory), queryArgs.ListType))),
                new SqlParameter("@Pwd",pwd)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
        }

        #endregion

        #region 用户详细信息

        /// <summary>
        /// 根据用户ID获取用户详细信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public MediaUserDetialModel GetMediaUserDetailInfo(int UserID)
        {
            string SQL = @"
                            SELECT  U.UserID ,
                                    U.UserName ,
                                    U.Mobile ,
                                    P.AreaName AS ProvinceName ,
                                    C.AreaName AS CityName ,
                                    UD.Address ,
                                    U.Type ,
                                    UD.TrueName ,
                                    UD.BLicenceURL ,
                                    UD.IDCardFrontURL,
                                    D.DictName AS RegisterFromName,
									DT.DictName AS RegisterTypeName,
                                    UD.Status AS ApproveStatus ,
                                    ( CASE ISNULL(UD.Status,0)
                                        WHEN 0 THEN '未认证'
                                        WHEN 1 THEN '待审核'
                                        WHEN 2 THEN '已认证'
                                        WHEN 3 THEN '认证未通过'
                                      END ) AS ApproveStatusName ,
									UD.ApplyTime,
									UD.AuditTime,
		                            US.TrueName as ApproveUserName,
		                            UD.Reason,
                                    UD.IdentityNo,
                                    U.CreateTime,
									(CASE WHEN U.Source=3006 THEN  CASE WU.Status WHEN 0 THEN '已关注' ELSE '取消关注' END ELSE '' END) AS AttentionName,
                                    WU.nickname
                            FROM    dbo.UserInfo AS U

                                    LEFT JOIN dbo.v_LE_WeiXinUser AS WU ON wu.UserID=u.UserID
                                    LEFT JOIN dbo.UserDetailInfo AS UD ON U.UserID = UD.UserID
                                    LEFT JOIN dbo.AreaInfo AS P ON P.AreaID = UD.ProvinceID
                                    LEFT JOIN dbo.AreaInfo AS C ON C.AreaID = UD.CityID
		                            LEFT JOIN dbo.UserDetailInfo AS US ON US.UserID=UD.AuditUserID
                                    LEFT JOIN dbo.DictInfo AS D ON D.DictId = U.Source
									LEFT JOIN dbo.DictInfo AS  DT ON DT.DictId=U.RegisterType
                            WHERE   U.Status >= 0  AND U.UserID=@UserID ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID", UserID)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToEntity<MediaUserDetialModel>(data.Tables[0]);

        }

        #endregion

        #region 更新用户状态

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int UserCertificationAudit(UserBatchQueryArgs queryArgs, int AuditUserID)
        {
            string UserSQL = @"UPDATE dbo.UserDetailInfo SET Status=@Status,Reason=@Reason,AuditTime=GETDATE(),AuditUserID=@AuditUserID WHERE UserID=@UserID";
            if (queryArgs.Status != 3)
            {
                queryArgs.Reason = string.Empty;
            }
            var sqlParams = new SqlParameter[] {
                            new SqlParameter("@UserID", queryArgs.UserID),
                            new SqlParameter("@Status", queryArgs.Status),
                            new SqlParameter("@AuditUserID", AuditUserID),
                            new SqlParameter("@Reason", queryArgs.Reason)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, UserSQL, sqlParams);

        }
        public UserTokenInfo GetUserToken(int UserID)
        {

            string UserSQL = @"SELECT W.openId,U.TrueName,U.Reason FROM dbo.v_LE_WeiXinUser AS W
                                INNER JOIN dbo.UserDetailInfo AS U ON U.UserID=W.UserID
                                WHERE W.UserID=@UserID";
            var sqlParams = new SqlParameter[] {
                            new SqlParameter("@UserID", UserID)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, UserSQL, sqlParams);
            return DataTableToEntity<UserTokenInfo>(data.Tables[0]);
        }

        #endregion
    }
}
