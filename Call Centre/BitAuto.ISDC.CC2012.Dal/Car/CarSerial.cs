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
    /// 数据访问类CarSerial。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-12-11 03:57:11 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CarSerial : DataBase
    {
        #region Instance
        public static readonly CarSerial Instance = new CarSerial();
        #endregion

        #region const
        private const string P_CARSERIAL_SELECT = "p_CarSerial_Select";
        private const string P_CARSERIAL_INSERT = "p_CarSerial_Insert";
        private const string P_CARSERIAL_UPDATE = "p_CarSerial_Update";
        private const string P_CARSERIAL_DELETE = "p_CarSerial_Delete";
        #endregion

        #region Contructor
        protected CarSerial()
        { }
        #endregion

        #region Select
        ///// <summary>
        ///// 按照查询条件查询
        ///// </summary>
        ///// <param name="query">查询条件</param>
        ///// <param name="order">排序</param>
        ///// <param name="currentPage">页号,-1不分页</param>
        ///// <param name="pageSize">每页记录数</param>
        ///// <param name="totalCount">总行数</param>
        ///// <returns>集合</returns>
        public DataTable GetCarSerial(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            //string where = string.Empty;

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARSERIAL_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="CSID"></param>
        /// <returns></returns>
        public Entities.CarSerial GetCarSerial(int CSID)
        {
            string sql = string.Format("SELECT * FROM CarSerial WHERE CSID=@CSID");
            SqlParameter[] parameters = { new SqlParameter("@CSID", SqlDbType.Int, 4) { Value = CSID } };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCarSerial(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CarSerial LoadSingleCarSerial(DataRow row)
        {
            Entities.CarSerial model = new Entities.CarSerial();

            if (row["CSID"].ToString() != "")
            {
                model.CSID = int.Parse(row["CSID"].ToString());
            }
            if (row["MasterBrandID"].ToString() != "")
            {
                model.MasterBrandID = int.Parse(row["MasterBrandID"].ToString());
            }
            model.Name = row["Name"].ToString();
            model.OldCbID = row["OldCbID"].ToString();
            model.CsLevel = row["CsLevel"].ToString();
            model.MultiPriceRange = row["MultiPriceRange"].ToString();
            model.CsSaleState = row["CsSaleState"].ToString();
            model.AllSpell = row["AllSpell"].ToString();
            model.CsMultiChar = row["CsMultiChar"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            return model;
        }
        public List<Entities.CarSerial> GetCarSerial(string csids)
        {
            csids = csids.TrimEnd(',');
            List<Entities.CarSerial> list = new List<Entities.CarSerial>();
            string sql = string.Format("SELECT * FROM CarSerial WHERE CSID in (select * from dbo.f_split(@CSID,','))");
            SqlParameter[] parameters = { new SqlParameter("@CSID", SqlDbType.VarChar, 100) { Value = csids } };
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleCarSerial(dr));
                }
            }
            return list;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.CarSerial model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@OldCbID", SqlDbType.VarChar,10),
					new SqlParameter("@CsLevel", SqlDbType.VarChar,200),
					new SqlParameter("@MultiPriceRange", SqlDbType.NVarChar,200),
					new SqlParameter("@CsSaleState", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@CsMultiChar", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
  					new SqlParameter("@BrandID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.MasterBrandID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.OldCbID;
            parameters[4].Value = model.CsLevel;
            parameters[5].Value = model.MultiPriceRange;
            parameters[6].Value = model.CsSaleState;
            parameters[7].Value = model.AllSpell;
            parameters[8].Value = model.CsMultiChar;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.BrandID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARSERIAL_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CarSerial model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@OldCbID", SqlDbType.VarChar,10),
					new SqlParameter("@CsLevel", SqlDbType.VarChar,200),
					new SqlParameter("@MultiPriceRange", SqlDbType.NVarChar,200),
					new SqlParameter("@CsSaleState", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@CsMultiChar", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
  					new SqlParameter("@BrandID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.MasterBrandID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.OldCbID;
            parameters[4].Value = model.CsLevel;
            parameters[5].Value = model.MultiPriceRange;
            parameters[6].Value = model.CsSaleState;
            parameters[7].Value = model.AllSpell;
            parameters[8].Value = model.CsMultiChar;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.BrandID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CARSERIAL_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CarSerial model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@OldCbID", SqlDbType.VarChar,10),
					new SqlParameter("@CsLevel", SqlDbType.VarChar,200),
					new SqlParameter("@MultiPriceRange", SqlDbType.NVarChar,200),
					new SqlParameter("@CsSaleState", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@CsMultiChar", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
  					new SqlParameter("@BrandID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.MasterBrandID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.OldCbID;
            parameters[4].Value = model.CsLevel;
            parameters[5].Value = model.MultiPriceRange;
            parameters[6].Value = model.CsSaleState;
            parameters[7].Value = model.AllSpell;
            parameters[8].Value = model.CsMultiChar;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.BrandID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARSERIAL_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CarSerial model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@OldCbID", SqlDbType.VarChar,10),
					new SqlParameter("@CsLevel", SqlDbType.VarChar,200),
					new SqlParameter("@MultiPriceRange", SqlDbType.NVarChar,200),
					new SqlParameter("@CsSaleState", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@CsMultiChar", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
  					new SqlParameter("@BrandID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.MasterBrandID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.OldCbID;
            parameters[4].Value = model.CsLevel;
            parameters[5].Value = model.MultiPriceRange;
            parameters[6].Value = model.CsSaleState;
            parameters[7].Value = model.AllSpell;
            parameters[8].Value = model.CsMultiChar;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.BrandID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CARSERIAL_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int CSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)};
            parameters[0].Value = CSID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARSERIAL_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int CSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)};
            parameters[0].Value = CSID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CARSERIAL_DELETE, parameters);
        }
        #endregion


        public void DeleteTable()
        {
            string sql = "DELETE FROM CarSerial";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public DataTable GetAllCarSerial()
        {
            string sqlStr = "select CSID,MasterBrandID,Name,BrandID from CarSerial";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetAllListFromCrm2009()
        {
            string sqlText = "SELECT SerialID AS  CSID,MasterBrandID,Name,BrandID FROM dbo. Car_Serial";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlText);
            return ds.Tables[0];

        }
    }
}

