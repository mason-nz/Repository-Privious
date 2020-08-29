using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;

namespace XYAuto.ITSC.Chitunion2017.Dal.SysRight
{
    public class RoleInfo:DataBase
    {
        #region Instance
        public static readonly RoleInfo Instance = new RoleInfo();
        #endregion

        #region const
        //
        public const string P_ROLEINFO_SELECT = "p_RoleInfo_select";
        public const string P_ROLEINFO_UPDATE = "p_RoleInfo_update";
        public const string P_ROLEINFO_INSERT = "p_RoleInfo_insert";
        public const string P_USERROLE_IS_USE = "P_USERROLE_IS_USE";
        public const string P_ROLEINFO_SELECT_BY_ID = "p_RoleInfo_select_by_id";
        public const string P_ROLEINFO_SELECT_ALL = "P_ROLEINFO_SELECT_ALL";
        public const string P_ROLEINFO_SELECT_BY_SYSID = "P_ROLEINFO_SELECT_BY_SYSID";
        public const string P_ROLEINFO_SELECT_BY_ROLEID = "p_RoleInfo_Select_By_RoleID";
        #endregion

        #region Contructor
        protected RoleInfo()
        {
        }
        #endregion

        #region Select
        ///// <summary>
        ///// 根据角色ID查询
        ///// </summary>
        ///// <param name="RoleID">角色ID</param>
        ///// <returns></returns>
        //public DataTable GetRoleInfoByRoleID(string RoleID)
        //{
        //    DataSet ds;
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@RoleID", SqlDbType.VarChar,50)
        //     };
        //    parameters[0].Value = RoleID;
        //    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_SELECT_BY_ROLEID, parameters);

        //    return ds.Tables[0];
        //}
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>集合</returns>
        public DataTable GetRoleInfo(QueryRoleInfo RoleInfoQuery, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            if (RoleInfoQuery.RecID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " and  RecID=" + RoleInfoQuery.RecID;
            }
            if (RoleInfoQuery.RoleName != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " and  RoleName='" + SqlFilter(RoleInfoQuery.RoleName) + "'";
            }
            if (RoleInfoQuery.SysID != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " and  SysID='" + SqlFilter(RoleInfoQuery.SysID) + "'";
            }
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            //totalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 判断选择的角色是否有用户使用
        /// </summary>
        /// <param name="roleIds">角色id</param>        
        /// <returns>用户表集合</returns>
        public int UserRoleIsUse(string roleIds)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@roleIds", SqlDbType.VarChar,500)
             };
            parameters[0].Value = roleIds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERROLE_IS_USE, parameters);

            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }

        public DataTable GetRoleInfoBySysID(string sysID)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@sysID", SqlDbType.NVarChar,50)
			 };
            parameters[0].Value = SqlFilter(sysID);
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_SELECT_BY_SYSID, parameters);

            return ds.Tables[0];
        }
        ///// <summary>
        ///// 得到全部
        ///// </summary>
        ///// <returns>集合</returns>
        //public DataTable GetRoleInfoAll()
        //{
        //    DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_SELECT_ALL);
        //    return ds.Tables[0];
        //}
        #endregion

        #region SelectByID
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="id">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        public Entities.SysRight.RoleInfo GetRoleInfo(string RoleID)
        {
            DataSet ds;
            //绑定存储过程参数
            SqlParameter[] parameters = {new SqlParameter("@RoleID", SqlDbType.VarChar,50)
                   };

            parameters[0].Value = RoleID;
            //绑定存储过程参数
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleRoleInfo(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }

        private Entities.SysRight.RoleInfo LoadSingleRoleInfo(DataRow row)
        {
            Entities.SysRight.RoleInfo RoleInfo = new Entities.SysRight.RoleInfo();

            if (row["RecID"] != DBNull.Value)
            {
                RoleInfo.RecID = Convert.ToInt32(row["RecID"].ToString());
            }

            if (row["RoleID"] != DBNull.Value)
            {
                RoleInfo.RoleID = row["RoleID"].ToString();
            }

            if (row["SysID"] != DBNull.Value)
            {
                RoleInfo.SysID = row["SysID"].ToString();
            }

            if (row["RoleName"] != DBNull.Value)
            {
                RoleInfo.RoleName = row["RoleName"].ToString();
            }

            if (row["Intro"] != DBNull.Value)
            {
                RoleInfo.Intro = row["Intro"].ToString();
            }

            if (row["Status"] != DBNull.Value)
            {
                RoleInfo.Status = Convert.ToInt32(row["Status"].ToString());
            }

            if (row["CreateTime"] != DBNull.Value)
            {
                RoleInfo.CreateTime = Convert.ToDateTime(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"] != DBNull.Value)
            {
                RoleInfo.CreateUserID = Convert.ToInt32(row["CreateUserID"].ToString());
            }
            //if (row["roletype"] != DBNull.Value)
            //{
            //    RoleInfo.RoleType = Convert.ToInt32(row["roletype"].ToString());
            //}
            return RoleInfo;
        }

        #endregion

        #region Updata
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>成功:1 失败:-1</returns>
        public int UpdataRoleInfo(Entities.SysRight.RoleInfo model)
        {
            SqlParameter[] parameters = {
					
                    new SqlParameter("@RoleID", SqlDbType.VarChar,50),
                    new SqlParameter("@SysID", SqlDbType.VarChar,50),
                    new SqlParameter("@RoleName", SqlDbType.VarChar,100),
                    new SqlParameter("@Intro", SqlDbType.VarChar,500),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int,4)};

            parameters[0].Value = model.RoleID;
            parameters[1].Value = model.SysID;
            parameters[2].Value = model.RoleName;
            parameters[3].Value = model.Intro;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_UPDATE, parameters);
        }
        #endregion

        //#region Delete
        /////// <summary>
        ///////删除
        /////// </summary>
        /////// <param name="id">索引ID</param>
        /////// <returns>成功:1 失败:-1</returns>
        ////public int DeleteRoleInfo(int id)
        ////{
        ////    SqlParameter[] parameters = {new SqlParameter("@roleid", SqlDbType.Int,4)};
        ////    parameters[0].Value = id;
        ////    return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_DELETE, parameters);
        ////}
        //#endregion

        #region Insert
        /// <summary>
        /// 添加详细
        /// </summary>
        /// <param name="model">值对象</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertRoleInfo(Entities.SysRight.RoleInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4),
                    new SqlParameter("@SysID", SqlDbType.VarChar,50),
                    new SqlParameter("@RoleName", SqlDbType.VarChar,100),
                    new SqlParameter("@Intro", SqlDbType.VarChar,500),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SysID;
            parameters[2].Value = model.RoleName;
            parameters[3].Value = model.Intro;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEINFO_INSERT, parameters);

            return int.Parse(parameters[0].Value.ToString());

        }
        #endregion

        //public XYAuto.YanFa.SysRightsManager.Entities.RoleInfo GetRoleInfoByRecID(int addRoleID)
        //{
        //    QueryRoleInfo queryRoleInfo = new QueryRoleInfo();
        //    queryRoleInfo.RecID = addRoleID;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetRoleInfo(queryRoleInfo, 1, int.MaxValue, out count);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        return LoadSingleRoleInfo(dt.Rows[0]);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 通过角色id获取拥有该角色的用户信息
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns>用户信息</returns>
        public DataTable GetUserInfoByRoleId(string roleID)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@roleid",SqlDbType.VarChar,50)
            };
            parameters[0].Value = roleID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_UserRole_UserInfo_Select", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
    }
}
