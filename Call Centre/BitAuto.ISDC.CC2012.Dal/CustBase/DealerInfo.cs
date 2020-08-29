using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类DealerInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class DealerInfo : DataBase
    {
        #region Instance
        public static readonly DealerInfo Instance = new DealerInfo();
        #endregion

        #region const
        private const string P_DEALERINFO_SELECT = "p_DealerInfo_Select";
        private const string P_DEALERINFO_INSERT = "p_DealerInfo_Insert";
        private const string P_DEALERINFO_UPDATE = "p_DealerInfo_Update";
        private const string P_DEALERINFO_DELETE = "p_DealerInfo_Delete";
        #endregion

        #region Contructor
        protected DealerInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetDealerInfo(QueryDealerInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query != null && query.RecID != -2)
            {

                where += " and RecID=" + query.RecID;
            }
            if (query != null && !string.IsNullOrEmpty(query.CustID))
            {
                where += " and CustID='" + Utils.StringHelper.SqlFilter(query.CustID) + "'";
            }

            where += " and status=0";

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.DealerInfo GetDealerInfo(string CustID)
        {
            QueryDealerInfo query = new QueryDealerInfo();
            query.CustID = CustID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetDealerInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleDealerInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.DealerInfo LoadSingleDealerInfo(DataRow row)
        {
            Entities.DealerInfo model = new Entities.DealerInfo();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            model.MemberCode = row["MemberCode"].ToString();
            model.Name = row["Name"].ToString();
            if (row["CityScope"].ToString() != "")
            {
                model.CityScope = int.Parse(row["CityScope"].ToString());
            }
            if (row["MemberType"].ToString() != "")
            {
                model.MemberType = int.Parse(row["MemberType"].ToString());
            }
            if (row["CarType"].ToString() != "")
            {
                model.CarType = int.Parse(row["CarType"].ToString());
            }
            if (row["MemberStatus"].ToString() != "")
            {
                model.MemberStatus = int.Parse(row["MemberStatus"].ToString());
            }
            model.Remark = row["Remark"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.DealerInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@MemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@Name", SqlDbType.NVarChar,300),
					new SqlParameter("@CityScope", SqlDbType.Int,4),
					new SqlParameter("@MemberType", SqlDbType.Int,4),
					new SqlParameter("@CarType", SqlDbType.Int,4),
					new SqlParameter("@MemberStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.MemberCode;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.CityScope;
            parameters[5].Value = model.MemberType;
            parameters[6].Value = model.CarType;
            parameters[7].Value = model.MemberStatus;
            parameters[8].Value = model.Remark;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.DealerInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@MemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@Name", SqlDbType.NVarChar,300),
					new SqlParameter("@CityScope", SqlDbType.Int,4),
					new SqlParameter("@MemberType", SqlDbType.Int,4),
					new SqlParameter("@CarType", SqlDbType.Int,4),
					new SqlParameter("@MemberStatus", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.MemberCode;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.CityScope;
            parameters[5].Value = model.MemberType;
            parameters[6].Value = model.CarType;
            parameters[7].Value = model.MemberStatus;
            parameters[8].Value = model.Remark;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERINFO_UPDATE, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int DeleteForSetStatus(string CustID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20)};
            parameters[0].Value = CustID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_DEALERINFO_DELETE, parameters);
        }
        public void Delete(string cbid)
        {
            string sql = "DELETE FROM dbo.DealerInfo WHERE CustID='" + SqlFilter(cbid) + "'";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        #endregion

        /// <summary>
        /// 取车易通会员信息
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public DataTable GetMemberInfo(string memberName, string memberCode, string custId, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = " AND CRM2009.dbo.DMSMember.status=0";
            if (!string.IsNullOrEmpty(memberCode))
            {
                where += " AND CRM2009.dbo.DMSMember.MemberCode = '" + Utils.StringHelper.SqlFilter(memberCode) + "'";
            }
            if (!string.IsNullOrEmpty(memberName))
            {
                where += " AND CRM2009.dbo.DMSMember.name LIKE '%" + Utils.StringHelper.SqlFilter(memberName) + "%'";
            }
            if (!string.IsNullOrEmpty(custId))
            {
                where += " AND CRM2009.dbo.DMSMember.CustID ='" + Utils.StringHelper.SqlFilter(custId) + "'";
            }

            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@where", SqlDbType.NVarChar,1000),
                new SqlParameter("@order", SqlDbType.NVarChar,20),
                new SqlParameter("@pagesize", SqlDbType.Int,4),
                new SqlParameter("@indexpage", SqlDbType.Int,4),
                new SqlParameter("@totalRecorder", SqlDbType.Int,4)
            };
            parameters[4].Direction = ParameterDirection.Output;
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Dearinfo_select", parameters);
            totalCount = int.Parse(parameters[4].Value.ToString());
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据品牌id取品牌信息
        /// </summary>
        /// <param name="BrandIDs"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBrandInfo(string BrandIDs, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            if (BrandIDs != Constant.STRING_INVALID_VALUE && BrandIDs.Trim() != string.Empty)
            {
                string ids = "";
                string[] array = BrandIDs.Split(',');
                if (array.Length > 0)
                {
                    foreach (string s in array)
                    {
                        string ss = s.Trim();
                        if (string.IsNullOrEmpty(ss) == false)
                        {
                            ids += "'" + SqlFilter(ss) + "',";
                        }
                    }
                    if (string.IsNullOrEmpty(ids) == false)
                    {
                        where += " and  a.BrandID in (" + ids.Trim(',') + ")";
                    }
                }
            }

            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@where", SqlDbType.NVarChar,1000),
                new SqlParameter("@order", SqlDbType.NVarChar,20),
                new SqlParameter("@pagesize", SqlDbType.Int,4),
                new SqlParameter("@page", SqlDbType.Int,4),
                new SqlParameter("@totalRecorder", SqlDbType.Int,4)
            };
            parameters[4].Direction = ParameterDirection.Output;
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, "p_Car_Brand_Select", parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }
        /// <summary>
        /// 根据品牌name取品牌信息
        /// </summary>
        /// <param name="BrandIDs"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBrandInfoByName(string brandname, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            if (brandname != Constant.STRING_INVALID_VALUE && brandname != string.Empty)
            {
                where += " and  (a.Name LIKE '%" + SqlFilter(brandname) + "%' or c.CustomBrandName LIKE '%" + SqlFilter(brandname) + "%')";
            }

            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@where", SqlDbType.NVarChar,1000),
                new SqlParameter("@order", SqlDbType.NVarChar,20),
                new SqlParameter("@pagesize", SqlDbType.Int,4),
                new SqlParameter("@page", SqlDbType.Int,4),
                new SqlParameter("@totalRecorder", SqlDbType.Int,4)
            };
            parameters[4].Direction = ParameterDirection.Output;
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, "p_Car_Brand_Select", parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }
    }
}

