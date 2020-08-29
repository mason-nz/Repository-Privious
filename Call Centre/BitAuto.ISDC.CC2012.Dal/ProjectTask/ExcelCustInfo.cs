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
    /// 数据访问类ExcelCustInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExcelCustInfo : DataBase
    {
        #region Instance
        public static readonly ExcelCustInfo Instance = new ExcelCustInfo();
        #endregion

        #region const
        private const string P_EXCELCUSTINFO_SELECT = "p_ExcelCustInfo_Select";
        private const string P_EXCELCUSTINFO_SELECT_BY_ID = "p_ExcelCustInfo_select_by_id";
        private const string P_EXCELCUSTINFO_UPDATE = "p_ExcelCustInfo_update";
        private const string P_EXCELCUSTINFO_INSERT = "dbo.p_ExcelCustInfo_insert";
        private const string P_EXCELCUSTINFO_Delete = "p_ExcelCustInfo_Delete";
        private const string P_EXCELCUSTINFO_Select_TaskStatus = "p_ExcelCustInfo_Select_TaskStatus";
        private const string P_EXCELCUSTINFO_Select_TaskStatus_By_Contion = "P_ExcelCustInfo_Select_TaskStatus_By_Contion";
        private const string P_EXCELCUSTINFO_Delete_By_Contion = "p_ExcelCustInfo_Delete_By_Contion";
        #endregion

        #region Contructor
        protected ExcelCustInfo()
        { }
        #endregion

        #region Select

        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryExcelCustInfo">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>销售网络集合</returns>
        public DataTable GetExcelCustInfo_Manage(QueryExcelCustInfo query, int currentPage, int pageSize, out int totalCount)
        {
            string where = GenerateWhereStr(query);
            string order = " ei.CreateTime desc";

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
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXCELCUSTINFO_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }

        #endregion

        #region Insert

        /// <summary>
        /// 批量插入客户信息
        /// </summary>
        /// <param name="models"></param>
        public void InsertExcelCustInfo_ForBitch(List<Entities.ExcelCustInfo> models)
        {
            List<SqlParameter[]> sqlParametersList = new List<SqlParameter[]>();
            foreach (Entities.ExcelCustInfo model in models)
            {
                SqlParameter[] parameters = {
 					new SqlParameter("@CustName", SqlDbType.VarChar,100),
					new SqlParameter("@TypeName", SqlDbType.VarChar,100),
					new SqlParameter("@ProvinceName", SqlDbType.VarChar,100),
					new SqlParameter("@CityName", SqlDbType.VarChar,100),
					new SqlParameter("@CountyName", SqlDbType.VarChar,100),
					new SqlParameter("@Zipcode", SqlDbType.VarChar,10),
					new SqlParameter("@Address", SqlDbType.VarChar,400),
					new SqlParameter("@BrandName", SqlDbType.VarChar,2000),
					new SqlParameter("@Fax", SqlDbType.VarChar,50),
					new SqlParameter("@OfficeTel", SqlDbType.VarChar,50),
                    new SqlParameter("@CreateUserID",SqlDbType.Int),
                    new SqlParameter("@CarType",SqlDbType.Int),
                    new SqlParameter("@TradeMarketID",SqlDbType.VarChar,50),
                    new SqlParameter("@MonthStock",SqlDbType.Int),
                    new SqlParameter("@MonthSales",SqlDbType.Int),
                    new SqlParameter("@MonthTrade",SqlDbType.Int),
                    new SqlParameter("@ContactName",SqlDbType.VarChar),
                      new SqlParameter("@ID", SqlDbType.Int,8)
                   };

                parameters[0].Value = model.CustName;
                parameters[1].Value = model.TypeName;
                parameters[2].Value = model.ProvinceName;
                parameters[3].Value = model.CityName;
                parameters[4].Value = model.CountyName;
                parameters[5].Value = model.Zipcode;
                parameters[6].Value = model.Address;
                parameters[7].Value = model.BrandName;
                parameters[8].Value = model.Fax;
                parameters[9].Value = model.OfficeTel;
                parameters[10].Value = model.CreateUserID;
                parameters[11].Value = model.CarType;
                parameters[12].Value = model.TradeMarketID;
                parameters[13].Value = model.MonthStock;
                parameters[14].Value = model.MonthSales;
                parameters[15].Value = model.MonthTrade;
                parameters[16].Value = model.ContactName;
                parameters[17].Direction = ParameterDirection.Output;

                sqlParametersList.Add(parameters);
            }
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRINGS))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("InsertExcelInfo");
                try
                {
                    if (sqlParametersList != null && sqlParametersList.Count > 0)
                    {
                        int i = 0;
                        foreach (SqlParameter[] parameters in sqlParametersList)
                        {
                            //return parameters[0].Value.ToString();
                             SqlHelper.ExecuteNonQuery(transaction,CommandType.StoredProcedure, P_EXCELCUSTINFO_INSERT, parameters);
                            int returnID = (int)parameters[17].Value;
                            models[i].ID = returnID;
                            i++;
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                    throw ex;
                }
            }
        }



        #endregion

        #region Updata
        /// <summary>
        /// 修改销售网络信息
        /// </summary>
        /// <param name="model">实体类销售网络信息</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int UpdataExcelCustInfo(Entities.ExcelCustInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@CustName", SqlDbType.VarChar,100),
					new SqlParameter("@TypeName", SqlDbType.VarChar,100),
					new SqlParameter("@ProvinceName", SqlDbType.VarChar,100),
					new SqlParameter("@CityName", SqlDbType.VarChar,100),
					new SqlParameter("@CountyName", SqlDbType.VarChar,100),
					new SqlParameter("@Zipcode", SqlDbType.VarChar,10),
					new SqlParameter("@BrandName", SqlDbType.VarChar,2000),
					new SqlParameter("@OfficeTel", SqlDbType.VarChar,50),
                    new SqlParameter("@Fax",SqlDbType.VarChar,50),
                    new SqlParameter("@Address",SqlDbType.VarChar,400)
                      };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CustName;
            parameters[2].Value = model.TypeName;
            parameters[3].Value = model.ProvinceName;
            parameters[4].Value = model.CityName;
            parameters[5].Value = model.CountyName;
            parameters[6].Value = model.Zipcode;
            parameters[7].Value = model.BrandName;
            parameters[8].Value = model.OfficeTel;
            parameters[9].Value = model.Fax;
            parameters[10].Value = model.Address;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXCELCUSTINFO_UPDATE, parameters);
        }
        #endregion

        #region SelectSingle
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="rid">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        public Entities.ExcelCustInfo GetExcelCustInfo(int rid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@ID", SqlDbType.Int,4)
            };

            parameters[0].Value = rid;
            //绑定存储过程参数

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXCELCUSTINFO_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleExcelCustInfo(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }

        private static Entities.ExcelCustInfo LoadSingleExcelCustInfo(DataRow row)
        {
            Entities.ExcelCustInfo model = new Entities.ExcelCustInfo();
            if (row["ID"] != DBNull.Value)
            {
                model.ID = Convert.ToInt32(row["ID"].ToString());
            }
            if (row["CustName"] != DBNull.Value)
            {
                model.CustName = row["CustName"].ToString();
            }
            if (row["TypeName"] != DBNull.Value)
            {
                model.TypeName = row["TypeName"].ToString();
            }
            if (row["ProvinceName"] != DBNull.Value)
            {
                model.ProvinceName = row["ProvinceName"].ToString();
            }
            if (row["CityName"] != DBNull.Value)
            {
                model.CityName = row["CityName"].ToString();
            }
            if (row["CountyName"] != DBNull.Value)
            {
                model.CountyName = row["CountyName"].ToString();
            }
            if (row["Zipcode"] != DBNull.Value)
            {
                model.Zipcode = row["Zipcode"].ToString();
            }
            if (row["BrandName"] != DBNull.Value)
            {
                model.BrandName = row["BrandName"].ToString();
            }
            if (row["OfficeTel"] != DBNull.Value)
            {
                model.OfficeTel = row["OfficeTel"].ToString();
            }
            if (row["Address"] != DBNull.Value)
            {
                model.Address = row["Address"].ToString();
            }
            if (row["CarType"] != DBNull.Value)
            {
                model.CarType = int.Parse(row["CarType"].ToString());
            }
            if (row["Fax"] != DBNull.Value)
            {
                model.Fax = row["Fax"].ToString();
            }
            if (row["MonthStock"] != DBNull.Value)
            {
                model.MonthStock = int.Parse(row["MonthStock"].ToString());
            }
            if (row["MonthSales"] != DBNull.Value)
            {
                model.MonthSales = int.Parse(row["MonthSales"].ToString());
            }
            if (row["MonthTrade"] != DBNull.Value)
            {
                model.MonthTrade = int.Parse(row["MonthTrade"].ToString());
            }
            if (row["ContactName"] != DBNull.Value)
            {
                model.ContactName = row["ContactName"].ToString();
            }
            if (row["TradeMarketID"] != DBNull.Value)
            {
                model.TradeMarketID = int.Parse(row["TradeMarketID"].ToString());
            }
            return model;
        }
        #endregion

        public void StatNewCustCheckRecordsByUserID(QueryExcelCustInfo query, out int totalCount, out  int noProcessCount, out int processingCount, out  int finishedCount)
        {
            string where = GenerateWhereStr(query);
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					new SqlParameter("@NoProcessCount", SqlDbType.Int, 4),
					new SqlParameter("@ProcessingCount", SqlDbType.Int, 4),
					new SqlParameter("@FinishedCount", SqlDbType.Int, 4)
			};
            parameters[0].Value = where;
            parameters[1].Direction = ParameterDirection.Output;
            parameters[2].Direction = ParameterDirection.Output;
            parameters[3].Direction = ParameterDirection.Output;
            parameters[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ExcelCustInfo_StatCountOfRecords", parameters);

            totalCount = int.Parse(parameters[1].Value.ToString());
            noProcessCount = int.Parse(parameters[2].Value.ToString());
            processingCount = int.Parse(parameters[3].Value.ToString());
            finishedCount = int.Parse(parameters[4].Value.ToString());
        }

        #region Delete
        public void Delete(string ids)
        {
            SqlParameter parameter = new SqlParameter("@ID", SqlDbType.VarChar, 8000);
            parameter.Value = ids;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXCELCUSTINFO_Delete, parameter);
        }

        public void Delete(QueryExcelCustInfo query)
        {

            string where = string.Empty;
            where = GenerateWhereStr(query);

            SqlParameter[] parameters = {
				new SqlParameter("@Where", SqlDbType.VarChar,8000)};
            parameters[0].Value = where;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXCELCUSTINFO_Delete_By_Contion, parameters);
        }
        #endregion

        public bool HasTaskStatusMoreThanAssign(string custIDs)
        {
            bool result = false;
            SqlParameter[] parameters = {
			new SqlParameter("@RelationID", SqlDbType.VarChar,8000)
             };

            parameters[0].Value = custIDs;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXCELCUSTINFO_Select_TaskStatus, parameters))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        public bool HasTaskStatusMoreThanAssign(QueryExcelCustInfo query)
        {
            bool result = false;
            string where = string.Empty;
            where = GenerateWhereStr(query);

            SqlParameter[] parameters = {
				new SqlParameter("@Where", SqlDbType.VarChar,8000)};

            parameters[0].Value = where;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXCELCUSTINFO_Select_TaskStatus_By_Contion, parameters))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        private string GenerateWhereStr(QueryExcelCustInfo query)
        {
            string where = string.Empty;
            if (query.BrandName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.BrandName like '%" + StringHelper.SqlFilter(query.BrandName) + "%'";
            }
            if (query.ProvinceName != Constant.STRING_EMPTY_VALUE)
            {
                query.ProvinceName = StringHelper.SqlFilter(query.ProvinceName).Replace("省", "");
                where += " And ei.ProvinceName like '%" + query.ProvinceName + "%'";
            }
            if (query.CityName != Constant.STRING_EMPTY_VALUE)
            {
                query.CityName = StringHelper.SqlFilter(query.CityName).Replace("市", "");
                where += " And ei.CityName like '%" + query.CityName + "%'";
            }
            if (query.CountyName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.CountyName like '%" + StringHelper.SqlFilter(query.CountyName) + "%'"; ;
            }
            if (query.TypeNames != Constant.STRING_EMPTY_VALUE)
            {
                where += "And ei.TypeName in (select * from dbo.f_split('" + Dal.Util.SqlFilterByInCondition(query.TypeNames) + "',','))";
            }
            if (query.CustName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.ID != Constant.INT_INVALID_VALUE)
            {
                where += " And ei.ID=" + query.ID;
            }
            if (query.TrueName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ui.TrueName like '%" + StringHelper.SqlFilter(query.TrueName) + "%'";
            }

            if (query.LastUpdateTime_StartTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime>='" + query.LastUpdateTime_StartTime.ToString("yyyy-MM-dd") + "'";
            }
            if (query.LastUpdateTime_EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime<'" + query.LastUpdateTime_EndTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (query.CarType != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.CarType in (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ")";
            }
            //add by qizq 添加客户类别2012-6-8
            if (!string.IsNullOrEmpty(query.TypeID))
            {
                where += " and ct.typeid in (" + Dal.Util.SqlFilterByInCondition(query.TypeID.Trim()) + ")";
            }

            if (query.StatusNoManage || query.StatusManaging || query.StatusManageFinsh || query.StatusNoAssign)
            {
                where += "And ( 1=-1 ";
                if (query.StatusNoManage)
                {
                    where += " or tk.TaskStatus=180000 ";
                }
                if (query.StatusManaging)
                {
                    string sqlT = " or ((tk.TaskStatus=180001 or tk.TaskStatus=180002 or tk.TaskStatus=180009) and ({0}))";
                    StringBuilder sbT = new StringBuilder();
                    if (string.IsNullOrEmpty(query.AdditionalStatus) == false)
                    {
                        foreach (string s in query.AdditionalStatus.Split(','))
                        {
                            string ss = s.Trim();
                            if (string.IsNullOrEmpty(ss) == false)
                            {
                                if (sbT.Length > 0) { sbT.Append(" or "); }
                                //if (ss.ToLower() == "as_a")
                                //{
                                //    sbT.Append(string.Format(" tas.AdditionalStatus='{0}' or tas.AdditionalStatus is null ", ss));
                                //}
                                //else
                                //{
                                sbT.Append(string.Format(" tas.AdditionalStatus='{0}' ", ss));
                                //}
                            }
                        }
                    }
                    else { sbT.Append(" 1=1 "); }

                    where += string.Format(sqlT, sbT.ToString());
                }
                if (query.StatusManageFinsh)
                {
                    where += " or (tk.TaskStatus between 180003 and 180008)";
                }
                if (query.StatusNoAssign)
                {
                    where += " or ccte.TID is null";
                }
                where += ")";
            }

            if (query.UserIDAssigned != Constant.INT_INVALID_VALUE)
            {
                if (query.UserIDAssigned == 0)
                {
                    where += " and ccte.UserID IS NOT NULL";
                }
                else
                {
                    where += " and ccte.UserID IS NOT NULL And ccte.UserID=" + query.UserIDAssigned;
                }
            }
            //where += " and tk.Status=0 "; 暂不需要

            if (query.CallRecordsCount != Constant.STRING_INVALID_VALUE)
            {
                where += " and (SELECT Count(*) FROM CallRecordInfo WHERE TaskTypeID=1 AND (TaskID=tk.PTID OR CustID=tk.CRMCustID))=" + StringHelper.SqlFilter(query.CallRecordsCount);
            }
            if (query.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " and tk.PTID='" + StringHelper.SqlFilter(query.PTID)+"'";
            }
            return where;
        }

        public DataTable GetExcelCustInfoByCustName(string custName)
        {
            string sql = string.Format(@"SELECT ce.*,ct.PTID FROM dbo.ExcelCustInfo AS ce
                                         INNER JOIN dbo.ProjectTaskInfo AS ct ON ct.RelationID=ce.ID AND ct.Source=1
                                         WHERE ce.CustName='{0}'", StringHelper.SqlFilter(custName));
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
    }
}

