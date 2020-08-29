using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;
using System.Data.SqlClient;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Dal.SysRight
{
    /// <summary>
    /// 系统域名数据访问类
    /// </summary>
    public class DomainInfo : DataBase
    {
        #region Instance
        public static readonly DomainInfo Instance = new DomainInfo();
        #endregion

        #region Contructor
        protected DomainInfo()
        {
        }
        #endregion

        #region const
        public const string P_DOMAININFO_SELECT = "p_DomainInfo_select";
        public const string P_DOMAININFO_UPDATE = "p_DomainInfo_update";
        public const string P_DOMAININFO_INSERT = "p_DomainInfo_insert";
        public const string P_DOMAININFO_DELETE = "p_DomainInfo_delete";
        public const string P_DOMAININFO_SELECT_BY_SYSID = "p_DomainInfo_Select_by_SysID";
        #endregion

        #region Select
        ///// <summary>
        ///// 按照查询条件查询  分页
        ///// </summary>
        ///// <param name="queryDomainInfo">查询值对象，用来存放查询条件</param>        
        ///// <param name="currentPage">页号,-1不分页</param>
        ///// <param name="totalCount">总行数</param>
        ///// <param name="pageSize">每页记录数</param>
        ///// <returns>用户表集合</returns>
        //public DataTable GetDomainInfo(QueryDomainInfo queryDomainInfo, int currentPage, int pageSize, out int totalCount)
        //{
        //    string where = "";
        //    if (queryDomainInfo.RecID != Constant.INT_INVALID_VALUE)
        //    {
        //        where += " And RecID =  " + queryDomainInfo.RecID + " ";
        //    }
        //    if (queryDomainInfo.DomainCode != Constant.STRING_INVALID_VALUE)
        //    {
        //        where += " And DomainCode =  '" + StringHelper.SqlFilter(queryDomainInfo.DomainCode) + "' ";
        //    }
        //    if (queryDomainInfo.Name != Constant.STRING_INVALID_VALUE)
        //    {
        //        where += " And Name Like '%" + StringHelper.SqlFilter(queryDomainInfo.Name) + "%'";
        //    }
        //    if (queryDomainInfo.Domain != Constant.STRING_INVALID_VALUE)
        //    {
        //        where += " And Domain Like '%" + StringHelper.SqlFilter(queryDomainInfo.Domain) + "%'";
        //    }
        //    if (queryDomainInfo.ExistsName != Constant.STRING_INVALID_VALUE)
        //    {
        //        where += " And Name = '" + StringHelper.SqlFilter(queryDomainInfo.ExistsName) + "'";
        //    }
        //    if (queryDomainInfo.ExistsDomain != Constant.STRING_INVALID_VALUE)
        //    {
        //        where += " And Domain = '" + StringHelper.SqlFilter(queryDomainInfo.ExistsDomain) + "'";
        //    }
        //    if (queryDomainInfo.IsDefault != Constant.INT_INVALID_VALUE)
        //    {
        //        where += " And IsDefault = " + queryDomainInfo.IsDefault + "  ";
        //    }
        //    if (queryDomainInfo.SysID != Constant.STRING_INVALID_VALUE)
        //    {
        //        where += " And SysID =  '" + StringHelper.SqlFilter(queryDomainInfo.SysID) + "' ";
        //    }
        //    DataSet ds;
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@where", SqlDbType.NVarChar,1000),
        //            new SqlParameter("@pagesize", SqlDbType.Int,4),
        //            new SqlParameter("@page", SqlDbType.Int,4)
        //     };
        //    parameters[0].Value = where;
        //    parameters[1].Value = pageSize;
        //    parameters[2].Value = currentPage;

        //    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DOMAININFO_SELECT, parameters);

        //    totalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());

        //    return ds.Tables[0];
        //}

        /// <summary>
        /// 获取某系统的 系统域名
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public DataTable GetDomainInfoBySysID(string sysID)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@sysID", SqlDbType.VarChar,50) 
             };
            parameters[0].Value = sysID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DOMAININFO_SELECT_BY_SYSID, parameters);

            return ds.Tables[0];
        }
        #endregion

        //#region Insert

        ///// <summary>
        ///// 添加详细
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns>成功:索引值;失败:-1</returns>
        //public int InsertDomainInfo(Entities.DomainInfo model)
        //{
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@RecID", SqlDbType.Int,4),
        //            new SqlParameter("@Name", SqlDbType.VarChar,500),
        //            new SqlParameter("@Domain", SqlDbType.VarChar,500),
        //            new SqlParameter("@CreateUserID", SqlDbType.Int,4), 
        //            new SqlParameter("@CreateTime", SqlDbType.DateTime),
        //            new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4), 
        //            new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
        //            new SqlParameter("@SysID", SqlDbType.VarChar,50),
        //            new SqlParameter("@IsDefault", SqlDbType.Int,4),
        //            new SqlParameter("@DomainCode", SqlDbType.VarChar,50 )
        //                                };
        //    parameters[0].Direction = ParameterDirection.Output;
        //    parameters[1].Value = model.Name;
        //    parameters[2].Value = model.Domain;
        //    parameters[3].Value = model.CreateUserID;
        //    parameters[4].Value = model.CreateTime;
        //    parameters[5].Value = model.LastUpdateUserID;
        //    parameters[6].Value = model.LastUpdateTime;
        //    parameters[7].Value = model.SysID;
        //    parameters[8].Value = model.IsDefault;
        //    parameters[9].Value = model.DomainCode;
        //    return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DOMAININFO_INSERT, parameters);
        //}

        //#endregion

        //#region Update
        ///// <summary>
        ///// 修改详细
        ///// </summary>
        ///// <param name="model">值对象</param>
        ///// <returns>成功:索引值;失败:-1</returns>
        //public int UpdateDomainInfo(Entities.DomainInfo model)
        //{
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@RecID", SqlDbType.Int,4),
        //            new SqlParameter("@Name", SqlDbType.VarChar,500),
        //            new SqlParameter("@Domain", SqlDbType.VarChar,500), 
        //            new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4), 
        //            new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
        //            new SqlParameter("@SysID", SqlDbType.VarChar,50),
        //            new SqlParameter("@IsDefault", SqlDbType.Int,4),
        //            new SqlParameter("@DomainCode", SqlDbType.VarChar,50 )
        //                                };
        //    parameters[0].Value = model.RecID;
        //    parameters[1].Value = model.Name;
        //    parameters[2].Value = model.Domain;
        //    parameters[3].Value = model.LastUpdateUserID;
        //    parameters[4].Value = model.LastUpdateTime;
        //    parameters[5].Value = model.SysID;
        //    parameters[6].Value = model.IsDefault;
        //    parameters[7].Value = model.DomainCode;
        //    return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DOMAININFO_UPDATE, parameters);
        //}
        //#endregion

        //#region delete
        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="redID">redID</param>
        ///// <returns>成功:索引值;失败:-1</returns>
        //public int DeleteDomainInfo(int redID)
        //{
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@RecID", SqlDbType.Int,4) };
        //    parameters[0].Value = redID;
        //    return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DOMAININFO_DELETE, parameters);
        //}
        //#endregion

        //#region SelectByID
        ///// <summary>
        ///// 按照ID查询符合条件的一条记录
        ///// </summary>
        ///// <param name="id">索引ID</param>
        ///// <returns>符合条件的一个值对象</returns>
        //public Entities.DomainInfo GetDomainInfo(int id)
        //{
        //    int count = 0;
        //    DataTable dt;
        //    QueryDomainInfo DomainInfo = new QueryDomainInfo();
        //    DomainInfo.RecID = id;
        //    dt = GetDomainInfo(DomainInfo, 1, 1, out count);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        return LoadSingleDomainInfo(dt.Rows[0]);
        //    }
        //    return null;
        //}
        //public Entities.DomainInfo GetDomainInfo(string domainCode)
        //{
        //    int count = 0;
        //    DataTable dt;
        //    QueryDomainInfo DomainInfo = new QueryDomainInfo();
        //    DomainInfo.DomainCode = domainCode;
        //    dt = GetDomainInfo(DomainInfo, 1, 1, out count);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        return LoadSingleDomainInfo(dt.Rows[0]);
        //    }
        //    return null;
        //}
        //private Entities.DomainInfo LoadSingleDomainInfo(DataRow row)
        //{
        //    Entities.DomainInfo DomainInfo = new Entities.DomainInfo();

        //    if (row["RecID"] != DBNull.Value)
        //    {
        //        DomainInfo.RecID = Convert.ToInt32(row["RecID"].ToString());
        //    }

        //    if (row["Name"] != DBNull.Value)
        //    {
        //        DomainInfo.Name = row["Name"].ToString();
        //    }

        //    if (row["Domain"] != DBNull.Value)
        //    {
        //        DomainInfo.Domain = row["Domain"].ToString();
        //    }

        //    if (row["CreateTime"] != DBNull.Value)
        //    {
        //        DomainInfo.CreateTime = Convert.ToDateTime(row["CreateTime"].ToString());
        //    }

        //    if (row["CreateUserID"] != DBNull.Value)
        //    {
        //        DomainInfo.CreateUserID = Convert.ToInt32(row["CreateUserID"].ToString());
        //    }

        //    if (row["LastUpdateTime"] != DBNull.Value)
        //    {
        //        DomainInfo.LastUpdateTime = Convert.ToDateTime(row["LastUpdateTime"].ToString());
        //    }

        //    if (row["LastUpdateUserID"] != DBNull.Value)
        //    {
        //        DomainInfo.LastUpdateUserID = Convert.ToInt32(row["LastUpdateUserID"].ToString());
        //    }


        //    if (row["SysID"] != DBNull.Value)
        //    {
        //        DomainInfo.SysID = row["SysID"].ToString();
        //    }

        //    if (row["IsDefault"] != DBNull.Value)
        //    {
        //        DomainInfo.IsDefault = Convert.ToInt32(row["IsDefault"].ToString());
        //    }

        //    if (row["DomainCode"] != DBNull.Value)
        //    {
        //        DomainInfo.DomainCode = row["DomainCode"].ToString();
        //    }
        //    return DomainInfo;
        //}

        //#endregion
    }
}
