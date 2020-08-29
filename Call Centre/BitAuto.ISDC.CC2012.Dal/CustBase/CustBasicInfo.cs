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
    /// 数据访问类CustBasicInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:12 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustBasicInfo : DataBase
    {
        #region Instance
        public static readonly CustBasicInfo Instance = new CustBasicInfo();
        #endregion

        #region const
        private const string P_CUSTBASICINFO_SELECT = "p_CustBasicInfo_Select";
        private const string P_CUSTBASICINFO_INSERT = "p_CustBasicInfo_Insert";
        private const string P_CUSTBASICINFO_UPDATE = "p_CustBasicInfo_Update";
        private const string P_CUSTBASICINFO_DELETE = "p_CustBasicInfo_Delete";
        private const string P_CUSTBASICINFO_EXCELEXPORT = "p_CustBasicInfo_ExcelExport"; //导出到Excel用的
        private const string P_CUSTBASICINFO_EXPORT = "p_CustBasicInfo_export";
        #endregion

        #region Contructor
        protected CustBasicInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 根据电话获取客户信息
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfosByTel(string tel)
        {
            string sqlStr = "SELECT * FROM CustBasicInfo AS cbi JOIN CustTel AS tel ON cbi.CustID=tel.CustID WHERE Tel=@Tel And Status=0";
            SqlParameter parameter = new SqlParameter("@Tel", tel);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }
        /// <summary>
        /// 根据电话和联系人姓名（模糊匹配）获取客户信息
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfosByTelAndName(string tel, string custName)
        {
            string sqlStr = "SELECT * FROM CustBasicInfo AS cbi JOIN CustTel AS tel ON cbi.CustID=tel.CustID WHERE Tel=@Tel And Status=0 And cbi.CustName like '%" + StringHelper.SqlFilter(custName) + "%'";
            SqlParameter parameter = new SqlParameter("@Tel", tel);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustBasicInfo GetCustBasicInfo(string CustID)
        {
            string sqlStr = "SELECT * FROM CustBasicInfo WHERE CustID=@CustID And Status=0";
            SqlParameter parameter = new SqlParameter("CustID", CustID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCustBasicInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据客户名称和电话，查询客户信息
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="custName"></param>
        /// <returns></returns>
        public Entities.CustBasicInfo GetCustBasicInfo(string tel, string custName)
        {
            Entities.CustBasicInfo info = null;

            string sqlStr = "SELECT * FROM CustBasicInfo AS cbi JOIN CustTel AS tel ON cbi.CustID=tel.CustID WHERE Tel=@Tel And Status=0 And cbi.CustName='" + StringHelper.SqlFilter(custName) + "'";
            SqlParameter parameter = new SqlParameter("@Tel", tel);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                info = LoadSingleCustBasicInfo(ds.Tables[0].Rows[0]);
            }

            return info;
        }

        private Entities.CustBasicInfo LoadSingleCustBasicInfo(DataRow row)
        {
            Entities.CustBasicInfo model = new Entities.CustBasicInfo();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            model.CustName = row["CustName"].ToString();
            if (row["Sex"].ToString() != "")
            {
                model.Sex = int.Parse(row["Sex"].ToString());
            }
            if (row["CustCategoryID"].ToString() != "")
            {
                model.CustCategoryID = int.Parse(row["CustCategoryID"].ToString());
            }
            if (row["ProvinceID"].ToString() != "")
            {
                model.ProvinceID = int.Parse(row["ProvinceID"].ToString());
            }
            if (row["CityID"].ToString() != "")
            {
                model.CityID = int.Parse(row["CityID"].ToString());
            }
            if (row["CountyID"].ToString() != "")
            {
                model.CountyID = int.Parse(row["CountyID"].ToString());
            }
            if (row["AreaID"].ToString() != "")
            {
                model.AreaID = row["AreaID"].ToString(); //int.Parse(row["AreaID"].ToString());
            }
            model.Address = row["Address"].ToString();
            if (row["DataSource"].ToString() != "")
            {
                model.DataSource = int.Parse(row["DataSource"].ToString());
            }
            if (row["CallTime"].ToString() != "")
            {
                model.CallTime = int.Parse(row["CallTime"].ToString());
            }
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
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region 增删改
        /// <summary>
        ///  增加一条数据,返回CustID
        /// </summary>
        public string Insert(Entities.CustBasicInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CustName", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@CustCategoryID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@AreaID", SqlDbType.VarChar,20),
					new SqlParameter("@Address", SqlDbType.NVarChar,200),
					new SqlParameter("@DataSource", SqlDbType.Int,4),
					new SqlParameter("@CallTime", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustName;
            parameters[2].Value = model.Sex;
            parameters[3].Value = model.CustCategoryID;
            parameters[4].Value = model.ProvinceID;
            parameters[5].Value = model.CityID;
            parameters[6].Value = model.CountyID;
            parameters[7].Value = model.AreaID;
            parameters[8].Value = model.Address;
            parameters[9].Value = model.DataSource;
            parameters[10].Value = model.CallTime;
            parameters[11].Value = model.Status;
            parameters[12].Value = model.CreateTime;
            parameters[13].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTBASICINFO_INSERT, parameters);
            return parameters[0].Value.ToString();
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CustBasicInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CustName", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.Int,4),
					new SqlParameter("@CustCategoryID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@AreaID", SqlDbType.VarChar,20),
					new SqlParameter("@Address", SqlDbType.NVarChar,200),
					new SqlParameter("@DataSource", SqlDbType.Int,4),
					new SqlParameter("@CallTime", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),					
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.CustName;
            parameters[2].Value = model.Sex;
            parameters[3].Value = model.CustCategoryID;
            parameters[4].Value = model.ProvinceID;
            parameters[5].Value = model.CityID;
            parameters[6].Value = model.CountyID;
            parameters[7].Value = model.AreaID;
            parameters[8].Value = model.Address;
            parameters[9].Value = model.DataSource;
            parameters[10].Value = model.CallTime;
            parameters[11].Value = model.Status;
            parameters[12].Value = model.ModifyTime;
            parameters[13].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTBASICINFO_UPDATE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string custID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20)};
            parameters[0].Value = custID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTBASICINFO_DELETE, parameters);
        }
        #endregion

        /// 根据电话号码查询历史客户（新版） 强斐 2016-4-7
        /// <summary>
        /// 根据电话号码查询历史客户（新版） 强斐 2016-4-7
        /// 按照时间排序，取最新的客户
        /// </summary>
        /// <param name="tels"></param>
        /// <param name="taskid"></param>
        /// <param name="retList"></param>
        public void GetCallRecordORGIHistory_New(string tels, string taskid, out List<string[]> retList)
        {
            string wheretels = "";
            foreach (string item in tels.Split(','))
            {
                wheretels += "'" + item + "',";
            }

            if (wheretels.EndsWith(","))
            {
                wheretels = wheretels.Substring(0, wheretels.Length - 1);
                wheretels = "IN(" + wheretels + ") ";
            }

            string sql = @"SELECT a.CustID,b.Tel
                                    FROM dbo.CustBasicInfo a 
                                    INNER JOIN dbo.CustTel b ON (a.CustID=b.CustID)
                                    WHERE a.Status=0 
                                    AND b.Tel " + wheretels + @"
                                    ORDER BY a.CreateTime DESC,b.CreateTime DESC";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];

            retList = new List<string[]>();
            string[] array = new string[3];
            if (dt.Rows.Count > 0)
            {
                array[0] = "2";//显示个人用户查看页                    
                array[1] = dt.Rows[0]["Tel"].ToString();
                array[2] = dt.Rows[0]["CustID"].ToString();
                retList.Add(array);
                return;
            }
            else
            {
                array[0] = "1";
                array[1] = "";
                array[2] = "";
                retList.Add(array);
                return;
            }
        }
        /// 根据电话和姓名取值（为IM系统）
        /// <summary>
        /// 根据电话和姓名取值（为IM系统）
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfoForIM(string tel)
        {
            string sql = @"
                                SELECT Top 1 CustID ,
                                CASE Sex
                                WHEN 1 THEN '男'
                                WHEN 2 THEN '女'
                                ELSE '未知'
                                END AS sex ,
                                ProvinceID ,
                                CityID
                                FROM dbo.CustBasicInfo
                                WHERE CustID IN ( SELECT CustID  FROM dbo.CustTel WHERE Tel = '" + SqlFilter(tel) + @"' )
                                ORDER BY dbo.CustBasicInfo.CreateTime DESC";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 根据手机号码查询最新的客户ID
        /// <summary>
        /// 根据手机号码查询最新的客户ID
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public string GetMaxNewCustBasicInfoByTel(params string[] tels)
        {
            string where = "";
            foreach (string tel in tels)
            {
                where += "'" + StringHelper.SqlFilter(tel) + "',";
            }
            where = where.Substring(0, where.Length - 1);
            string sql = @"SELECT TOP 1 a.CustID
                                    FROM dbo.CustBasicInfo a 
                                    INNER JOIN dbo.CustTel b ON (a.CustID=b.CustID)
                                    WHERE a.Status=0
                                    AND b.Tel IN (" + where + @")
                                    ORDER BY a.CreateTime DESC,b.CreateTime DESC";
            return CommonFunction.ObjectToString(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }

        #region 话务分月关联查询
        /// 查询个人用户信息
        /// <summary>
        /// 查询个人用户信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryCallInfo"></param>
        /// <param name="queryDealerInfo"></param>
        /// <param name="queryCustHistoryInfo"></param>
        /// <param name="outField"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfo(QueryCustBasicInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWhereStr(query);
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)};
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTBASICINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private string GetWhereStr(QueryCustBasicInfo query)
        {
            string where = "";
            if (!string.IsNullOrEmpty(query.CustName))
            {
                where += " AND cb.CustName LIKE '" + SqlFilter(query.CustName) + "%'";
            }
            if (!string.IsNullOrEmpty(query.Sexs))
            {
                where += " AND cb.Sex IN (" + Dal.Util.SqlFilterByInCondition(query.Sexs) + ")";
            }
            if (!string.IsNullOrEmpty(query.CustTel))
            {
                where += " AND cb.CustID IN (SELECT CustID FROM dbo.CustTel WHERE Tel='" + SqlFilter(query.CustTel) + "')";
            }
            if (query.ProvinceID > 0)
            {
                where += " AND cb.ProvinceID=" + query.ProvinceID;
            }
            if (query.CityID > 0)
            {
                where += " AND cb.CityID=" + query.CityID;
            }
            if (query.CountyID > 0)
            {
                where += " AND cb.CountyID=" + query.CountyID;
            }
            return where;
        }
        #endregion
    }
}

