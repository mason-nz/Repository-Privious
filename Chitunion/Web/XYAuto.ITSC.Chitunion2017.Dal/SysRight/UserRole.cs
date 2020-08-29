using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.SysRight
{
    public class UserRole:DataBase
    {
        #region Instance
        public static readonly UserRole Instance = new UserRole();
        #endregion

        #region Contructor
        protected UserRole()
        {
        }
        #endregion

        #region const
        public const string P_USERROLE_SELECT = "p_UserRole_select";
        public const string P_USERROLE_UPDATE = "p_UserRole_update";
        public const string P_USERROLE_DELETE_BY_USERID = "p_UserRole_delete_by_userId";
        /// <summary>
        /// 添加，如果已存在返回-1
        /// </summary>
        private const string p_UserRole_Insert = "p_UserRole_Insert";
        /// <summary>
        /// 删除
        /// </summary>
        private const string p_UserRole_Delete = "p_UserRole_Delete";

        private const string p_UserRole_IsSysRole = "p_UserRole_IsSysRole";

        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryUserRole">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>用户表集合</returns>
        public DataTable GetUserRole(string where, int currentPage, int pageSize, out int totalCount)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@where", SqlDbType.NVarChar,1000),
                    new SqlParameter("@order", SqlDbType.NVarChar,1000),
                    new SqlParameter("@page", SqlDbType.Int,4),
                    new SqlParameter("@pagesize", SqlDbType.Int,4),
                    new SqlParameter("@TotalRecorder", SqlDbType.Int,4)
			 };
            parameters[0].Value = where;
            parameters[1].Value = string.Empty;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERROLE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            //totalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());

            return ds.Tables[0];
        }

        #endregion

        #region Insert
        /// <summary>
        /// 添加详细
        /// </summary>
        /// <param name="UserRole">值对象</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertUserRole(Entities.SysRight.UserRole UserRole)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4),            
                    new SqlParameter("@userid", SqlDbType.Int,4),                    
                    new SqlParameter("@roleid", SqlDbType.VarChar,50),
                    new SqlParameter("@SysID", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime)
                    };

            //绑定存储过程参数
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = UserRole.UserID;
            parameters[2].Value = SqlFilter(UserRole.RoleID);
            parameters[3].Value = SqlFilter(UserRole.SysID);
            parameters[4].Value = UserRole.Status;
            parameters[5].Value = UserRole.CreateTime;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_UserRole_Insert, parameters);
            return Convert.ToInt32(parameters[0].Value);
        }
        #endregion

        #region Updata
        /// <summary>
        /// 修改详细
        /// </summary>
        /// <param name="UserRole">值对象</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int UpdataUserRole(Entities.SysRight.UserRole UserRole)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@userid", SqlDbType.Int,4),
                    new SqlParameter("@roleid", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime)
                    };

            //绑定存储过程参数
            parameters[0].Value = UserRole.UserID;
            parameters[1].Value = UserRole.RoleID;
            parameters[2].Value = UserRole.CreateTime;


            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERROLE_UPDATE, parameters);
        }
        #endregion

        public void Delete(int RecID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4)
			 };
            parameters[0].Value = RecID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_UserRole_Delete, parameters);

        }
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="UserID"></param>
        public void DeleteByuserID(int UserID)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@UserID",SqlDbType.Int,4)
            };
            parameters[0].Value = UserID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERROLE_DELETE_BY_USERID, parameters);
        }
        /// <summary>
        /// 判断用户是否系统角色
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool IsSysRole(int UserID)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@UserID",SqlDbType.Int,4)
            };
            parameters[0].Value = UserID;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_UserRole_IsSysRole, parameters).Tables[0];
            if (dt != null && Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 拼接数据权限
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="tablenameUserID">UserID所属的表名称，或表别名</param>
        /// <param name="UserIDFileName">UserID字段名称</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="msg"></param>
        /// <returns>返回Sql字符串</returns>
        public string GetSqlRightStr(Entities.EnumResourceType resourceType, string tablenameUserID,string UserIDFileName, int UserID,int PubID,out string msg)
        {

            #region 根据UserID、资源类型判断是否有资源权限
            string sqlstr = string.Format("SELECT t1.RoleID,t1.RoleName FROM dbo.RoleInfo t1 JOIN dbo.UserRole t2 ON t2.RoleID = t1.RoleID WHERE t2.UserID={0}", UserID);
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr).Tables[0];
            string roleids = string.Empty;
            msg = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    roleids += row["RoleID"].ToString() + ",";
                }
                if (roleids.EndsWith(","))
                    roleids = roleids.Substring(0, roleids.Length - 1);
            }
            else
            {
                msg = "没有资源权限";
                return " AND (1=2) ";
            }

            #region 李硕自用 V1_0
            switch (resourceType)
            {
                case Entities.EnumResourceType.媒体存在验证:
                    //超级管理员、运营、AE在自己的池子里面验证 媒体主只在用户下
                    if (roleids.Contains("SYS001RL00001") || roleids.Contains("SYS001RL00004") || roleids.Contains("SYS001RL00005"))
                        return string.Format(" And ({0}{1} In ( Select Distinct(UserID) From UserRole where RoleID in ('SYS001RL00001','SYS001RL00004','SYS001RL00005') ))", string.IsNullOrEmpty(tablenameUserID) ? "" : tablenameUserID + ".", UserIDFileName, UserID);
                    else if (roleids.Contains("SYS001RL00003"))
                        return string.Format(" And ({0}{1} = {2})", string.IsNullOrEmpty(tablenameUserID) ? "" : tablenameUserID + ".", UserIDFileName, UserID);
                    else
                        return " And (1=2) ";
                case Entities.EnumResourceType.添加刊例:
                    //超级管理员、运营可以给任何媒体加刊例，AE、媒体主只能在自己媒体下加刊例
                    if (roleids.Contains("SYS001RL00001") || roleids.Contains("SYS001RL00004"))
                        return " And (1=1) ";
                    else if (roleids.Contains("SYS001RL00003") || roleids.Contains("SYS001RL00005"))
                        return string.Format(" And ({0}{1} = {2})", string.IsNullOrEmpty(tablenameUserID) ? "" : tablenameUserID + ".", UserIDFileName, UserID);
                    else
                        return " And (1=2) ";
                case Entities.EnumResourceType.修改刊例:
                    if (roleids.Contains("SYS001RL00001") || roleids.Contains("SYS001RL00004"))
                        return " And (1=1) ";
                    else if (roleids.Contains("SYS001RL00003") || roleids.Contains("SYS001RL00005"))
                    {//需要看对应媒体的创建人
                        int mediaCreateUserID = Dal.PublishInfo.Instance.GetMediaCreateUserIDByPubID(PubID);
                        if (mediaCreateUserID > 0 && mediaCreateUserID.Equals(UserID))//媒体创建者
                            return " And (1=1) ";
                        else
                            return " And (1=2) ";
                    }
                    else
                        return " And (1=2) ";
            }
            #endregion

            //运营和超级管理员看全部
            if (roleids.Contains("SYS001RL00001") || roleids.Contains("SYS001RL00004"))
            {
                //msg = "运营和超级管理员看全部";
                return " AND (1=1) ";
            }
            //广告主、媒体主、AE、运营、超级
            switch (resourceType)
            {
                case XYAuto.ITSC.Chitunion2017.Entities.EnumResourceType.MediaORPublish:
                    //广告主无权限
                    if (roleids.Contains("SYS001RL00002"))
                    {
                        msg = "广告主无媒体刊例数据权限";
                        return " AND (1=2) ";
                    }
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumResourceType.ADOrderInfo:
                    //媒体主无权限
                    if (roleids.Contains("SYS001RL00003"))
                    {
                        msg = "媒体主无项目数据权限";
                        return " AND (1=2) ";
                    }
                    break;
                case XYAuto.ITSC.Chitunion2017.Entities.EnumResourceType.SubADInfo:
                    break;
                default:
                    break;
            }
            #endregion

            StringBuilder where = new StringBuilder();

            if (resourceType == Entities.EnumResourceType.MediaORPublish)
            {
                //取本人权限
                //媒体主只能看自己的、AE只能看自己的
                where.Append(" And (" + tablenameUserID + "." + UserIDFileName + "='" + UserID + "'"); 
            }
            else if (resourceType == Entities.EnumResourceType.ADOrderInfo || resourceType == Entities.EnumResourceType.SubADInfo)
            {
                if (roleids.Contains("SYS001RL00005"))
                {
                    //AE可以看到自己创建的或者自己负责的客户的项目
                    where.Append(" And (" + tablenameUserID + "." + UserIDFileName + "='" + UserID + "'");
                    where.Append(" OR " + tablenameUserID + "." + UserIDFileName + " IN ('" + GetUserIDS_AuthAEUserID(UserID) + "')");   
                }
                else if (roleids.Contains("SYS001RL00002"))
                {
                    //广告主可以看自己的
                    where.Append(" And (" + tablenameUserID + "." + UserIDFileName + "='" + UserID + "'");
                }
            }
            //else if (resourceType == Entities.EnumResourceType.SubADInfo)
            //{
            //    if (roleids.Contains("SYS001RL00005"))
            //    {
            //        //AE可以看到自己创建的或者自己负责的客户的
            //        where.Append(" And (" + tablenameUserID + "." + UserIDFileName + "='" + UserID + "'");
            //        where.Append(" OR " + tablenameUserID + "." + UserIDFileName + " IN ('" + GetUserIDS_AuthAEUserID(UserID) + "')");
            //    }
            //    else if (roleids.Contains("SYS001RL00002"))
            //    {
            //        //广告主可以看自己的
            //        where.Append(" And (" + tablenameUserID + "." + UserIDFileName + "='" + UserID + "'");
            //    }
            //}
          
            where.Append(") ");
            return where.ToString();
        }

        #region 查看AE负责的客户
        public string GetUserIDS_AuthAEUserID(int userID)
        {
            string sqlstr = string.Format(@"SELECT  LEFT(aa, LEN(aa) - 1)
FROM    ( SELECT    ( SELECT    CONVERT(VARCHAR(20), UserID) + ','
                      FROM      dbo.UserInfo
                      WHERE     AuthAEUserID = {0}
                    FOR
                      XML PATH('')
                    ) AS aa
        ) rr",userID);

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr).Tables[0];
            if (dt != null && !string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
            {
                return dt.Rows[0][0].ToString();
            }
            return "";
        }
        #endregion
    }
}
