using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.Utils.Data;
using System.Data.SqlClient;

namespace XYAuto.ITSC.Chitunion2017.Dal.SysRight
{
    public class ModuleInfo : DataBase
    {
        #region Instance
        public static readonly ModuleInfo Instance = new ModuleInfo();
        #endregion

        #region const
        //
        public const string P_MODULEINFO_SELECT = "p_moduleinfo_select";
        public const string P_MODULEINFO_UPDATE = "p_moduleinfo_update";
        public const string P_MODULEINFO_INSERT = "p_moduleinfo_insert";
        public const string P_MODULEINFO_SELECT_BY_ROLEID = "P_MODULEINFO_SELECT_BY_ROLEID";
        public const string P_MODULEINFO_SELECT_BY_ModuleNameRelation = "p_ModuleInfo_Select_ModuleNameRelation";
        private const string p_ModuleInfo_Links_IsExist = "p_ModuleInfo_Links_IsExist";
        #endregion

        #region Contructor
        protected ModuleInfo()
        {
        }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="moduleInfoQuery">查询条件</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>集合</returns>
        public DataTable GetModuleInfo(QueryModuleInfo moduleInfoQuery, string orderStr, int currentPage, int pageSize, out int totalCount)
        {
            string where = " and status>=0 ";
            if (moduleInfoQuery.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + moduleInfoQuery.Status.ToString();
            }
            if (moduleInfoQuery.SysID != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " and  SysID='" + SqlFilter(moduleInfoQuery.SysID) + "'";
            }
            if (moduleInfoQuery.ModuleName != null && moduleInfoQuery.ModuleName != string.Empty)
            {
                where += " And ModuleName Like '%" + SqlFilter(moduleInfoQuery.ModuleName) + "%'";
            }
            else if (moduleInfoQuery.ExistModuleName != null && moduleInfoQuery.ExistModuleName != string.Empty)
            {
                where += " And ModuleName = '" + SqlFilter(moduleInfoQuery.ExistModuleName) + "'";
            }
            if (moduleInfoQuery.RecID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " and RecID=" + moduleInfoQuery.RecID.ToString();
            }
            if (moduleInfoQuery.ModuleID != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " and  ModuleID='" + SqlFilter(moduleInfoQuery.ModuleID) + "'";
            }

            if (moduleInfoQuery.ExceptModuleID != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " and  ModuleID!='" + SqlFilter(moduleInfoQuery.ExceptModuleID) + "'";
            }
            if (moduleInfoQuery.PID != Entities.Constants.Constant.STRING_INVALID_VALUE && moduleInfoQuery.PID != "-1")
            {
                where += " and  PID='" + SqlFilter(moduleInfoQuery.PID) + "'";
            }
            if (moduleInfoQuery.PID == "-1")
            {
                where += "and level = 1";
            }
            if (moduleInfoQuery.Level != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                where += " and level<=" + moduleInfoQuery.Level;
            }

            if (moduleInfoQuery.DomainCode != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " and  DomainCode  =" + moduleInfoQuery.DomainCode + " ";
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
            parameters[1].Value = orderStr;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MODULEINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            //totalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());

            return ds.Tables[0];
        }

        /// <summary>
        /// 按照userid查询
        /// </summary>
        /// <param name="userid">userid</param>
        /// <returns>集合</returns>
        public DataTable GetParentModuleInfoByUserID(int userid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@userid", SqlDbType.Int,4)
			 };
            //绑定存储过程参数
            parameters[0].Value = userid;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_MODULE_SELECT_PID_BY_USERID", parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 按照roleid查询
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="sysID"></param>
        /// <returns>集合</returns>
        public DataTable GetModuleInfoByRoleId(string roleId, string sysID)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.VarChar,50),
                    new SqlParameter("@SysID", SqlDbType.VarChar,50)
			 };
            //绑定存储过程参数
            parameters[0].Value = roleId;
            parameters[1].Value = sysID;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MODULEINFO_SELECT_BY_ROLEID, parameters);
            return ds.Tables[0];
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>成功:1 失败:-1</returns>
        public int UpdataModuleInfo(Entities.SysRight.ModuleInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ModuleID", SqlDbType.VarChar,50),
					new SqlParameter("@SysID", SqlDbType.VarChar,50),
					new SqlParameter("@ModuleName", SqlDbType.VarChar,100),
					new SqlParameter("@PID", SqlDbType.VarChar,50),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Intro", SqlDbType.VarChar,1000),
					new SqlParameter("@Url", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@Links", SqlDbType.VarChar,2000),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),
                    new SqlParameter("@DomainID", SqlDbType.Int,4)
					//new SqlParameter("@Blank", SqlDbType.SmallInt,2)
										};
            parameters[0].Value = model.RecID;
            parameters[1].Value = SqlFilter(model.ModuleID);
            parameters[2].Value = SqlFilter(model.SysID);
            parameters[3].Value = SqlFilter(model.ModuleName);
            parameters[4].Value = SqlFilter(model.PID);
            parameters[5].Value = model.Level;
            parameters[6].Value = SqlFilter(model.Intro);
            parameters[7].Value = model.Url;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateTime;
            //parameters[13].Value = model.Blank;
            if (!string.IsNullOrEmpty(model.Links))
            {
                parameters[10].Value = "," + model.Links.Trim(',') + ",";
            }
            else
            {
                parameters[10].Value = model.Links;
            }
            parameters[11].Value = model.OrderNum;
            parameters[12].Value = model.DomainID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MODULEINFO_UPDATE, parameters);

        }
        #endregion

        #region Insert
        /// <summary>
        /// 添加详细
        /// </summary>
        /// <param name="model">值对象</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertModuleInfo(Entities.SysRight.ModuleInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@ModuleID", SqlDbType.VarChar,50),
					new SqlParameter("@SysID", SqlDbType.VarChar,50),
					new SqlParameter("@ModuleName", SqlDbType.VarChar,100),
					new SqlParameter("@PID", SqlDbType.VarChar,50),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Intro", SqlDbType.VarChar,1000),
					new SqlParameter("@Url", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Links", SqlDbType.VarChar,2000),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),
                    new SqlParameter("@DomainID", SqlDbType.Int,4)
					//new SqlParameter("@Blank", SqlDbType.SmallInt,2)
										};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = SqlFilter(model.ModuleID);
            parameters[2].Value = SqlFilter(model.SysID);
            parameters[3].Value = SqlFilter(model.ModuleName);
            parameters[4].Value = SqlFilter(model.PID);
            parameters[5].Value = model.Level;
            parameters[6].Value = SqlFilter(model.Intro);
            parameters[7].Value = model.Url;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateTime;
            //parameters[13].Value = model.Blank;
            if (!string.IsNullOrEmpty(model.Links))
            {
                parameters[10].Value = "," + model.Links.Trim(',') + ",";
            }
            else
            {
                parameters[10].Value = model.Links;
            }
            parameters[11].Value = model.OrderNum;
            parameters[12].Value = model.DomainID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MODULEINFO_INSERT, parameters);

            return int.Parse(parameters[0].Value.ToString());
        }
        #endregion


        #region SelectByID
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="id">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        public Entities.SysRight.ModuleInfo GetModuleInfo(int id)
        {
            int count = 0;
            DataTable dt;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.RecID = id;
            dt = GetModuleInfo(moduleInfo,string.Empty, 1, 1, out count);


            if (dt != null && dt.Rows.Count > 0)
            {
                return LoadSingleSysInfo(dt.Rows[0]);
            }
            return null;
        }
        public Entities.SysRight.ModuleInfo GetModuleInfo(string moduleid)
        {
            int count = 0;
            DataTable dt;
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            moduleInfo.ModuleID = moduleid;
            dt = GetModuleInfo(moduleInfo,string.Empty, 1, 1, out count);


            if (dt != null && dt.Rows.Count > 0)
            {
                return LoadSingleSysInfo(dt.Rows[0]);
            }
            return null;
        }
        private Entities.SysRight.ModuleInfo LoadSingleSysInfo(DataRow row)
        {
            Entities.SysRight.ModuleInfo moduleInfo = new Entities.SysRight.ModuleInfo();

            if (row["RecID"] != DBNull.Value)
            {
                moduleInfo.RecID = Convert.ToInt32(row["RecID"]);
            }

            if (row["SysID"] != DBNull.Value)
            {
                moduleInfo.SysID = row["SysID"].ToString();
            }

            if (row["ModuleID"] != DBNull.Value)
            {
                moduleInfo.ModuleID = row["ModuleID"].ToString();
            }

            if (row["ModuleName"] != DBNull.Value)
            {
                moduleInfo.ModuleName = row["ModuleName"].ToString();
            }

            if (row["PID"] != DBNull.Value)
            {
                moduleInfo.PID = row["PID"].ToString();
            }

            if (row["Level"] != DBNull.Value)
            {
                moduleInfo.Level = Convert.ToInt32(row["Level"]);
            }

            if (row["Url"] != DBNull.Value)
            {
                moduleInfo.Url = row["Url"].ToString();
            }

            if (row["Intro"] != DBNull.Value)
            {
                moduleInfo.Intro = row["Intro"].ToString();
            }

            if (row["CreateTime"] != DBNull.Value)
            {
                moduleInfo.CreateTime = Convert.ToDateTime(row["CreateTime"]);
            }

            if (row["Status"] != DBNull.Value)
            {
                moduleInfo.Status = Convert.ToInt32(row["Status"]);
            }
            if (row["Links"] != DBNull.Value)
            {
                moduleInfo.Links = row["Links"].ToString();
            }
            if (row["OrderNum"] != DBNull.Value)
            {
                moduleInfo.OrderNum = Convert.ToInt32(row["OrderNum"]);
            }
            if (row["DomainID"] != DBNull.Value)
            {
                moduleInfo.DomainID = Convert.ToInt32(row["DomainID"]);
            }
            return moduleInfo;
        }

        #endregion

        public DataTable GetModuleNameRelation(QueryModuleInfo queryModuleInfo)
        {
            string where = "";
            if (queryModuleInfo.SysID != Entities.Constants.Constant.STRING_INVALID_VALUE)
            {
                where += " and  SysID='" + SqlFilter(queryModuleInfo.SysID) + "'";
            }
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@where", SqlDbType.NVarChar,1000)
			 };

            parameters[0].Value = where;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MODULEINFO_SELECT_BY_ModuleNameRelation, parameters);

            //totalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());

            return ds.Tables[0];
        }

        public DataTable GetSysInfoByMaxRecID()
        {
            string where = " And recid = (select max(recid) from ModuleInfo)";
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@where", SqlDbType.NVarChar,1000),
                    new SqlParameter("@pagesize", SqlDbType.Int,4),
                    new SqlParameter("@page", SqlDbType.Int,4)
             };
            parameters[0].Value = where;
            parameters[1].Value = 20;
            parameters[2].Value = 1;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_MODULEINFO_SELECT, parameters);

            //totalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());

            return ds.Tables[0];
        }
        /// <summary>
        /// 判断一个地址是否已存在于某个模块中
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool IsExist(string link, string sysid)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@link", SqlDbType.VarChar,100),
                    new SqlParameter("@sysid", SqlDbType.VarChar,50)

             };
            parameters[0].Value = SqlFilter(link.Trim(','));
            parameters[0].Value = SqlFilter(sysid);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_ModuleInfo_Links_IsExist, parameters);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;

        }
    }
}
