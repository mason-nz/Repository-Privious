using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using System.Data.SqlClient;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CarBrand : DataBase
    {
        public static readonly CarBrand Instance = new CarBrand();

        #region const
        private const string P_CARBRAND_SELECT = "p_CarBrand_Select";
        private const string P_CARBRAND_INSERT = "p_CarBrand_Insert";
        private const string P_CARBRAND_UPDATE = "p_CarBrand_Update";
        private const string P_CARBRAND_DELETE = "p_CarBrand_Delete";
        #endregion

        protected CarBrand()
        {
        }

        /// <summary>
        /// 得到主品牌信息
        /// </summary>
        /// <param name="brandNameList">主品牌名称列表，用逗号分隔</param>
        /// <returns>ids, names. 用逗号分隔</returns>
        public string[] GetCarBrandByNames(string brandNameList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in brandNameList.Split(','))
            {
                sb.Append("'" + SqlFilter(s) + "',");
            }

            string sqlText = "SELECT * FROM Crm2009.dbo.Car_Brand WHERE [Name] IN ({0})";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, string.Format(sqlText, sb.ToString() + "'-1'"));
            DataTable dt = ds.Tables[0];

            StringBuilder ids = new StringBuilder();
            StringBuilder names = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Append(dr["BrandID"].ToString() + ",");
                names.Append(dr["Name"].ToString() + ",");
            }

            string[] r = { ids.ToString().Trim(','), names.ToString().Trim(',') };
            return r;
        }

        /// <summary>
        /// 得到主品牌信息
        /// </summary>
        /// <param name="brandNameList">主品牌名称列表，用逗号分隔</param>
        /// <returns>id数组</returns>
        public List<int> GetCarBrandIDsByNames(string brandNameList)
        {
            List<int> r = new List<int>();

            StringBuilder sb = new StringBuilder();
            foreach (string s in brandNameList.Split(','))
            {
                sb.Append("'" + SqlFilter(s) + "',");
            }
            string sqlText = "SELECT * FROM Crm2009.dbo.Car_Brand WHERE [Name] IN ({0})";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, string.Format(sqlText, sb.ToString() + "'-1'"));
            DataTable dt = ds.Tables[0];


            int i = -1;
            foreach (DataRow dr in dt.Rows)
            {
                if (int.TryParse(dr["BrandID"].ToString(), out i))
                {
                    r.Add(i);
                }
            }

            return r;
        }

        public DataTable GetAllCarBrandFormCrm2009()
        {
            string sqlText = "SELECT * FROM Car_Brand ";
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlText);
            return ds.Tables[0];
        }

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.CarBrand model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BrandID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Country", SqlDbType.NVarChar,200),
					new SqlParameter("@NewCountry", SqlDbType.VarChar,200),
					new SqlParameter("@AllSpell", SqlDbType.VarChar,200),
					new SqlParameter("@Spell", SqlDbType.VarChar,200),
					new SqlParameter("@BrandSEOName", SqlDbType.VarChar,200),
					new SqlParameter("@MasterBrandID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime)};
            parameters[0].Value = model.BrandID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Country;
            parameters[3].Value = model.NewCountry;
            parameters[4].Value = model.AllSpell;
            parameters[5].Value = model.Spell;
            parameters[6].Value = model.BrandSEOName;
            parameters[7].Value = model.MasterBrandID;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.ModifyTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CARBRAND_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
         #endregion

        public void DeleteTable()
        {
            string sql = "DELETE FROM CarBrand";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public DataTable GetAllList()
        {
            string sql = string.Format("SELECT * FROM CarBrand ");

            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return ds.Tables[0];
        }
    }
}
