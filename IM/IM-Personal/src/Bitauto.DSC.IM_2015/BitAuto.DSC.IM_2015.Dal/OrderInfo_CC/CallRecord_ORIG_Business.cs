using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data.SqlClient;
using BitAuto.Utils.Data;

namespace BitAuto.DSC.IM_2015.Dal
{
    public class CallRecord_ORIG_Business : DataBase
    {
        #region Instance
        public static readonly CallRecord_ORIG_Business Instance = new CallRecord_ORIG_Business();
        #endregion

        #region const
        private const string P_CALLRECORD_ORIG_BUSINESS_SELECT = "p_CallRecord_ORIG_Business_Select";
        private const string P_CALLRECORD_ORIG_BUSINESS_INSERT = "p_CallRecord_ORIG_Business_Insert";
        private const string P_CALLRECORD_ORIG_BUSINESS_UPDATE = "p_CallRecord_ORIG_Business_Update";
        private const string P_CALLRECORD_ORIG_BUSINESS_DELETE = "p_CallRecord_ORIG_Business_Delete";
        #endregion

        #region Contructor
        protected CallRecord_ORIG_Business()
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
        public DataTable GetCallRecord_ORIG_Business(QueryCallRecord_ORIG_Business query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region MyRegion

            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += " And  CallID=" + query.CallID;
            }

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " And  RecID=" + query.RecID;
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " And  BGID=" + query.BGID;
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " And  SCID=" + query.SCID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And  CreateUserID=" + query.CreateUserID;
            }
            if (query.BusinessID != Constant.STRING_INVALID_VALUE)
            {
                where += " And  BusinessID='" + query.BusinessID + "'";
            }

            #endregion
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

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CallRecord_ORIG_Business GetCallRecord_ORIG_Business(long RecID)
        {
            QueryCallRecord_ORIG_Business query = new QueryCallRecord_ORIG_Business();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCallRecord_ORIG_Business(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCallRecord_ORIG_Business(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CallRecord_ORIG_Business LoadSingleCallRecord_ORIG_Business(DataRow row)
        {
            Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            if (row["CallID"].ToString() != "")
            {
                model.CallID = Int64.Parse(row["CallID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.BusinessID = row["BusinessID"].ToString();
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            return model;
        }


        public bool IsExistsByCallID(Int64 callid)
        {
            string sql = string.Format("SELECT * FROM CallRecord_ORIG_Business WHERE callid={0}", callid);
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG_Business model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.BusinessID;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(ConnectionStrings_CC, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_INSERT, parameters);
            return int.Parse(parameters[0].Value.ToString());
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.CallRecord_ORIG_Business model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.BusinessID;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Business model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.BusinessID;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(ConnectionStrings_CC, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_UPDATE, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CallRecord_ORIG_Business model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.BusinessID;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_UPDATE, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(ConnectionStrings_CC, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_DELETE, parameters);
        }
        #endregion

        public DataTable getListBySourceAndCallID(int source, Int64 callID)
        {
            DataSet ds = null;
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@Source", SqlDbType.Int),
					new SqlParameter("@CallID", SqlDbType.BigInt,8)};
            parameters[0].Value = source;
            parameters[1].Value = callID;

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.StoredProcedure, "p_CallRecord_ORIG_BusinessURL_GetListBySourceAndCallID", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public Entities.CallRecord_ORIG_Business GetByCallID(Int64 CallID)
        {
            QueryCallRecord_ORIG_Business query = new QueryCallRecord_ORIG_Business();
            query.CallID = CallID;
            int totalCount = 0;
            DataTable dt = GetCallRecord_ORIG_Business(query, "", 1, 9999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                return LoadSingleCallRecord_ORIG_Business(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public DataTable GetAllURL()
        {
            string sqlStr = "SELECT * FROM dbo.CallRecord_ORIG_BusinessURL";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr);
            return ds.Tables[0];
        }

        /// <summary>
        /// 增加一个URL
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="webBaseUrl"></param>
        /// <returns></returns>
        public int AddBusinessUrl(int BGID, int SCID, string webBaseUrl)
        {
            string sqlStr = @"INSERT dbo.CallRecord_ORIG_BusinessURL
                                            ( BGID ,
                                              SCID ,
                                              Source ,
                                              CarType ,
                                              BusinessDetailURL ,
                                              CreateTime
                                            )
                                    VALUES ( @BGID , -- BGID - int
                                              @SCID,
                                              NULL ,
                                              NULL , 
                                             @webBaseUrl,
                                              GETDATE()
                                            ) ";
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
                       new SqlParameter("@webBaseUrl", SqlDbType.VarChar,2000)       
                                        };
            parameters[0].Value = BGID;
            parameters[1].Value = SCID;
            parameters[2].Value = webBaseUrl;
            return SqlHelper.ExecuteNonQuery(ConnectionStrings_CC, CommandType.Text, sqlStr, parameters);
        }

        public int DeleteBusinessUrl(int BGID, int SCID)
        {
            string sqlStr = "delete CallRecord_ORIG_BusinessURL where BGID=@BGID and SCID=@SCID";
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4)};
            parameters[0].Value = BGID;
            parameters[1].Value = SCID;
            return SqlHelper.ExecuteNonQuery(ConnectionStrings_CC, CommandType.Text, sqlStr, parameters);
        }
        /// <summary>
        /// 根据业务组，分类取url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public DataTable GetBusinessUrl(int BGID, int SCID)
        {
            string sqlStr = "select * from CallRecord_ORIG_BusinessURL where BGID=@BGID and SCID=@SCID";
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4)};
            parameters[0].Value = BGID;
            parameters[1].Value = SCID;
            return SqlHelper.ExecuteDataset(ConnectionStrings_CC, CommandType.Text, sqlStr, parameters).Tables[0];
        }
    }
}
