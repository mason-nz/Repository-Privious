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
    /// ���ݷ�����CarMasterBrand��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-12-11 03:57:10 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CarMasterBrand : DataBase
    {
        #region Instance
        public static readonly CarMasterBrand Instance = new CarMasterBrand();
        #endregion

        #region const
        private const string P_CARMASTERBRAND_SELECT = "p_CarMasterBrand_Select";
        private const string P_CARMASTERBRAND_INSERT = "p_CarMasterBrand_Insert";
        private const string P_CARMASTERBRAND_UPDATE = "p_CarMasterBrand_Update";
        private const string P_CARMASTERBRAND_DELETE = "p_CarMasterBrand_Delete";
        #endregion

        #region Contructor
        protected CarMasterBrand()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetCarMasterBrand(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            //string where = string.Empty;
            string sql = string.Format("SELECT * FROM CarMasterBrand Where 1=1 {0}", where);

            DataSet ds;

            //SqlParameter[] parameters = {
            //        new SqlParameter("@where", SqlDbType.NVarChar, 40000)
            //        };

            //parameters[0].Value = where;
            //parameters[1].Value = order;
            //parameters[2].Value = pageSize;
            //parameters[3].Value = currentPage;
            //parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            //totalCount = (int)(parameters[4].Value);
            totalCount = ds.Tables[0].Rows.Count;
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        /// <param name="MasterBrandID"></param>
        /// <returns></returns>
        public Entities.CarMasterBrand GetCarMasterBrand(int MasterBrandID)
        {
            string sql = string.Format("SELECT * FROM CarMasterBrand WHERE MasterBrandID=@MasterBrandID");
            SqlParameter[] parameters = {
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4)};
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCarMasterBrand(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CarMasterBrand LoadSingleCarMasterBrand(DataRow row)
        {
            Entities.CarMasterBrand model = new Entities.CarMasterBrand();

            if (row["MasterBrandID"].ToString() != "")
            {
                model.MasterBrandID = int.Parse(row["MasterBrandID"].ToString());
            }
            model.Name = row["Name"].ToString();
            model.EName = row["EName"].ToString();
            model.Country = row["Country"].ToString();
            model.AllSpell = row["AllSpell"].ToString();
            model.Spell = row["Spell"].ToString();
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
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public void Insert(Entities.CarMasterBrand model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@EName", SqlDbType.VarChar,200),
					new SqlParameter("@Country", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@Spell", SqlDbType.VarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime)};
            parameters[0].Value = model.MasterBrandID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.EName;
            parameters[3].Value = model.Country;
            parameters[4].Value = model.AllSpell;
            parameters[5].Value = model.Spell;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.ModifyTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARMASTERBRAND_INSERT, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CarMasterBrand model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@EName", SqlDbType.VarChar,200),
					new SqlParameter("@Country", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@Spell", SqlDbType.VarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime)};
            parameters[0].Value = model.MasterBrandID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.EName;
            parameters[3].Value = model.Country;
            parameters[4].Value = model.AllSpell;
            parameters[5].Value = model.Spell;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.ModifyTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CARMASTERBRAND_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.CarMasterBrand model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@EName", SqlDbType.VarChar,200),
					new SqlParameter("@Country", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@Spell", SqlDbType.VarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime)};
            parameters[0].Value = model.MasterBrandID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.EName;
            parameters[3].Value = model.Country;
            parameters[4].Value = model.AllSpell;
            parameters[5].Value = model.Spell;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.ModifyTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARMASTERBRAND_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CarMasterBrand model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@EName", SqlDbType.VarChar,200),
					new SqlParameter("@Country", SqlDbType.NVarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@Spell", SqlDbType.VarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime)};
            parameters[0].Value = model.MasterBrandID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.EName;
            parameters[3].Value = model.Country;
            parameters[4].Value = model.AllSpell;
            parameters[5].Value = model.Spell;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.ModifyTime;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CARMASTERBRAND_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int MasterBrandID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4)};
            parameters[0].Value = MasterBrandID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARMASTERBRAND_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int MasterBrandID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4)};
            parameters[0].Value = MasterBrandID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CARMASTERBRAND_DELETE, parameters);
        }
        #endregion


        public void DeleteTable()
        {
            string sql = "DELETE FROM CarMasterBrand";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public DataTable GetAllListFromCrm2009()
        {
            string sqlText = "SELECT MasterBrandID,Name  FROM Car_MasterBrand";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlText);
            return ds.Tables[0];
        }
    }
}

