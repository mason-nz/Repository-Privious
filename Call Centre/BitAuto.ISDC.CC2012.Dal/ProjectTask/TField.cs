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
    /// 数据访问类TField。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-20 03:24:42 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TField : DataBase
    {
        #region Instance
        public static readonly TField Instance = new TField();
        #endregion

        #region const
        private const string P_TFIELD_SELECT = "p_TField_Select";
        private const string P_TFIELD_INSERT = "p_TField_Insert";
        private const string P_TFIELD_UPDATE = "p_TField_Update";
        private const string P_TFIELD_DELETE = "p_TField_Delete";
        #endregion

        #region Contructor
        protected TField()
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
        public DataTable GetTField(QueryTField query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.TTCode != Constant.STRING_INVALID_VALUE)
            {
                where += " and TTCode='" + StringHelper.SqlFilter(query.TTCode) + "'";
            }

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TFIELD_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.TField GetTField(int RecID)
        {
            QueryTField query = new QueryTField();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTField(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleTField(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.TField LoadSingleTField(DataRow row)
        {
            Entities.TField model = new Entities.TField();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.TFCode = row["TFCode"].ToString();
            model.TFDesName = row["TFDesName"].ToString();
            model.TFName = row["TFName"].ToString();
            model.TTCode = row["TTCode"].ToString();
            if (row["TTypeID"].ToString() != "")
            {
                model.TTypeID = int.Parse(row["TTypeID"].ToString());
            }
            if (row["TFLen"].ToString() != "")
            {
                model.TFLen = int.Parse(row["TFLen"].ToString());
            }
            model.TFDes = row["TFDes"].ToString();
            if (row["TFInportIsNull"].ToString() != "")
            {
                model.TFInportIsNull = int.Parse(row["TFInportIsNull"].ToString());
            }
            if (row["TFIsNull"].ToString() != "")
            {
                model.TFIsNull = int.Parse(row["TFIsNull"].ToString());
            }
            if (row["TFSortIndex"].ToString() != "")
            {
                model.TFSortIndex = int.Parse(row["TFSortIndex"].ToString());
            }
            model.TFCssName = row["TFCssName"].ToString();
            if (row["TFIsExportShow"].ToString() != "")
            {
                model.TFIsExportShow = int.Parse(row["TFIsExportShow"].ToString());
            }
            if (row["TFIsListShow"].ToString() != "")
            {
                model.TFIsListShow = int.Parse(row["TFIsListShow"].ToString());
            }
            model.TFShowCode = row["TFShowCode"].ToString();
            model.TFValue = row["TFValue"].ToString();
            model.TFGenSubField = row["TFGenSubField"].ToString();
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
        public int Insert(Entities.TField model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TFCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TFDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TFName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTypeID", SqlDbType.Int,4),
					new SqlParameter("@TFLen", SqlDbType.Int,4),
					new SqlParameter("@TFDes", SqlDbType.NVarChar,100),
					new SqlParameter("@TFInportIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFSortIndex", SqlDbType.Int,4),
					new SqlParameter("@TFCssName", SqlDbType.NVarChar,200),
					new SqlParameter("@TFIsExportShow", SqlDbType.Int,4),
					new SqlParameter("@TFIsListShow", SqlDbType.Int,4),
					new SqlParameter("@TFShowCode", SqlDbType.NVarChar,10),
					new SqlParameter("@TFValue", SqlDbType.NVarChar,4000),
					new SqlParameter("@TFGenSubField", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TFCode;
            parameters[2].Value = model.TFDesName;
            parameters[3].Value = model.TFName;
            parameters[4].Value = model.TTCode;
            parameters[5].Value = model.TTypeID;
            parameters[6].Value = model.TFLen;
            parameters[7].Value = model.TFDes;
            parameters[8].Value = model.TFInportIsNull;
            parameters[9].Value = model.TFIsNull;
            parameters[10].Value = model.TFSortIndex;
            parameters[11].Value = model.TFCssName;
            parameters[12].Value = model.TFIsExportShow;
            parameters[13].Value = model.TFIsListShow;
            parameters[14].Value = model.TFShowCode;
            parameters[15].Value = model.TFValue;
            parameters[16].Value = model.TFGenSubField;
            parameters[17].Value = model.Status;
            parameters[18].Value = model.CreateTime;
            parameters[19].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TFIELD_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TField model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TFCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TFDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TFName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTypeID", SqlDbType.Int,4),
					new SqlParameter("@TFLen", SqlDbType.Int,4),
					new SqlParameter("@TFDes", SqlDbType.NVarChar,100),
					new SqlParameter("@TFInportIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFSortIndex", SqlDbType.Int,4),
					new SqlParameter("@TFCssName", SqlDbType.NVarChar,200),
					new SqlParameter("@TFIsExportShow", SqlDbType.Int,4),
					new SqlParameter("@TFIsListShow", SqlDbType.Int,4),
					new SqlParameter("@TFShowCode", SqlDbType.NVarChar,10),
					new SqlParameter("@TFValue", SqlDbType.NVarChar,4000),
					new SqlParameter("@TFGenSubField", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TFCode;
            parameters[2].Value = model.TFDesName;
            parameters[3].Value = model.TFName;
            parameters[4].Value = model.TTCode;
            parameters[5].Value = model.TTypeID;
            parameters[6].Value = model.TFLen;
            parameters[7].Value = model.TFDes;
            parameters[8].Value = model.TFInportIsNull;
            parameters[9].Value = model.TFIsNull;
            parameters[10].Value = model.TFSortIndex;
            parameters[11].Value = model.TFCssName;
            parameters[12].Value = model.TFIsExportShow;
            parameters[13].Value = model.TFIsListShow;
            parameters[14].Value = model.TFShowCode;
            parameters[15].Value = model.TFValue;
            parameters[16].Value = model.TFGenSubField;
            parameters[17].Value = model.Status;
            parameters[18].Value = model.CreateTime;
            parameters[19].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TFIELD_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.TField model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TFCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TFDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TFName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTypeID", SqlDbType.Int,4),
					new SqlParameter("@TFLen", SqlDbType.Int,4),
					new SqlParameter("@TFDes", SqlDbType.NVarChar,100),
					new SqlParameter("@TFInportIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFSortIndex", SqlDbType.Int,4),
					new SqlParameter("@TFCssName", SqlDbType.NVarChar,200),
					new SqlParameter("@TFIsExportShow", SqlDbType.Int,4),
					new SqlParameter("@TFIsListShow", SqlDbType.Int,4),
					new SqlParameter("@TFShowCode", SqlDbType.NVarChar,10),
					new SqlParameter("@TFValue", SqlDbType.NVarChar,4000),
					new SqlParameter("@TFGenSubField", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TFCode;
            parameters[2].Value = model.TFDesName;
            parameters[3].Value = model.TFName;
            parameters[4].Value = model.TTCode;
            parameters[5].Value = model.TTypeID;
            parameters[6].Value = model.TFLen;
            parameters[7].Value = model.TFDes;
            parameters[8].Value = model.TFInportIsNull;
            parameters[9].Value = model.TFIsNull;
            parameters[10].Value = model.TFSortIndex;
            parameters[11].Value = model.TFCssName;
            parameters[12].Value = model.TFIsExportShow;
            parameters[13].Value = model.TFIsListShow;
            parameters[14].Value = model.TFShowCode;
            parameters[15].Value = model.TFValue;
            parameters[16].Value = model.TFGenSubField;
            parameters[17].Value = model.Status;
            parameters[18].Value = model.CreateTime;
            parameters[19].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TFIELD_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TField model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TFCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TFDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TFName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTypeID", SqlDbType.Int,4),
					new SqlParameter("@TFLen", SqlDbType.Int,4),
					new SqlParameter("@TFDes", SqlDbType.NVarChar,100),
					new SqlParameter("@TFInportIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFIsNull", SqlDbType.Int,4),
					new SqlParameter("@TFSortIndex", SqlDbType.Int,4),
					new SqlParameter("@TFCssName", SqlDbType.NVarChar,200),
					new SqlParameter("@TFIsExportShow", SqlDbType.Int,4),
					new SqlParameter("@TFIsListShow", SqlDbType.Int,4),
					new SqlParameter("@TFShowCode", SqlDbType.NVarChar,10),
					new SqlParameter("@TFValue", SqlDbType.NVarChar,4000),
					new SqlParameter("@TFGenSubField", SqlDbType.NVarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TFCode;
            parameters[2].Value = model.TFDesName;
            parameters[3].Value = model.TFName;
            parameters[4].Value = model.TTCode;
            parameters[5].Value = model.TTypeID;
            parameters[6].Value = model.TFLen;
            parameters[7].Value = model.TFDes;
            parameters[8].Value = model.TFInportIsNull;
            parameters[9].Value = model.TFIsNull;
            parameters[10].Value = model.TFSortIndex;
            parameters[11].Value = model.TFCssName;
            parameters[12].Value = model.TFIsExportShow;
            parameters[13].Value = model.TFIsListShow;
            parameters[14].Value = model.TFShowCode;
            parameters[15].Value = model.TFValue;
            parameters[16].Value = model.TFGenSubField;
            parameters[17].Value = model.Status;
            parameters[18].Value = model.CreateTime;
            parameters[19].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TFIELD_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TFIELD_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TFIELD_DELETE, parameters);
        }
        #endregion


        public int GetMaxID()
        {
            int maxid = 0;
            string sqlStr = "SELECT MAX( RecID) maxid FROM TField ";
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (obj != null && obj.ToString() != "")
            {
                maxid = int.Parse(obj.ToString());
            }

            return maxid;
        }

        public List<Entities.TField> GetTFieldListByTTCode(string ttcode)
        {
            List<Entities.TField> list = new List<Entities.TField>();

            Entities.QueryTField query = new QueryTField();
            query.TTCode = ttcode;

            int totalCount = 0;
            DataTable dt = GetTField(query, "TFSortIndex", 1, 9999, out totalCount);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Entities.TField model = LoadSingleTField(dr);

                    list.Add(model);
                }
            }

            return list;
        }
        public DataTable GetTFieldTableByTTCode(string ttcode)
        {
            Entities.QueryTField query = new QueryTField();
            query.TTCode = ttcode;

            int totalCount = 0;
            DataTable dt = GetTField(query, "", 1, 9999, out totalCount);

            return dt;
        }

        /// <summary>
        /// 根据ttCode得到表名，再找到该表已“Tempf”开头的列名
        /// </summary>
        /// <param name="tableName">ttCode</param>
        /// <returns></returns>
        public DataTable GetTemptColumnNameByTableName(string ttCode)
        {

            SqlParameter[] par = { new SqlParameter("@ttcode", ttCode) };

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_TField_GetTempColumnName", par);

            //            string sqlStr = @"SELECT  name
            //                                    FROM    syscolumns
            //                                    WHERE   id = ( SELECT   MAX(id)
            //                                                   FROM     sysobjects
            //                                                   WHERE    xtype = 'u'
            //                                                                AND name = (SELECT TTName FROM dbo.TTable WHERE TTCode='" + Utils.StringHelper.SqlFilter(ttCode) + "'))" +
            //                                                "AND sys.syscolumns.name LIKE 'Tempf%'";
            //            DataSet ds = new DataSet();
            //            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            return ds.Tables[0];
        }

        /// <summary>
        /// 根据TFName获取字段名和是否导出的字段
        /// </summary>
        /// <param name="TFName"></param>
        /// <returns></returns>
        public DataTable GetTFieldTableByTFName(string TFName)
        {
            DataTable dt = new DataTable();
            string sqlStr = @"SELECT Top 1 TFDesName,TFInportIsNull FROM dbo.TField WHERE TFName='" + Utils.StringHelper.SqlFilter(TFName) + "'";

            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
            return dt;
        }


        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="Leng"></param>
        /// <returns></returns>
        public int AddColumn(string tableName, string fieldName, string typeID)
        {
            string typeName = "";

            if (typeID == "1")
            {
                typeName = "nvarchar(20)";
            }
            if (typeID == "3")
            {
                typeName = "int";
            }
            if (typeID == "4")
            {
                typeName = "datetime";
            }

            string sqlStr = "alter table " + tableName + " ADD " + fieldName + "  " + typeName;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sqlStr);
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="Leng"></param>
        /// <returns></returns>
        public int DelColumn(string tableName, string fieldName)
        {

            string sqlStr = "ALTER TABLE " + tableName + " DROP COLUMN " + fieldName + "";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sqlStr);
        }
    }
}

