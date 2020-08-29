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
	/// 数据访问类ProjectTask_DMSMember_Brand。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_DMSMember_Brand : DataBase
	{
		#region Instance
		public static readonly ProjectTask_DMSMember_Brand Instance = new ProjectTask_DMSMember_Brand();
		#endregion

		#region const
        private const string P_PROJECTTASK_DMSMEMBER_BRAND_SELECT = "p_ProjectTask_DMSMember_Brand_Select";
        public const string P_PROJECTTASK_DMSMEMBER_BRAND_SELECT_BY_ID = "p_ProjectTask_DMSMember_Brand_select_by_id";
		private const string P_PROJECTTASK_DMSMEMBER_BRAND_INSERT = "p_ProjectTask_DMSMember_Brand_Insert";
		private const string P_PROJECTTASK_DMSMEMBER_BRAND_UPDATE = "p_ProjectTask_DMSMember_Brand_Update";
		private const string P_PROJECTTASK_DMSMEMBER_BRAND_DELETE = "p_ProjectTask_DMSMember_Brand_Delete";
        public const string P_CC_DMSMember_Brand_UpdataByMemberID = "p_ProjectTask_DMSMember_Brand_UpdateByMemberID";
		#endregion

		#region Contructor
		protected ProjectTask_DMSMember_Brand()
		{}
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
		public DataTable GetProjectTask_DMSMember_Brand(QueryProjectTask_DMSMember_Brand query, int currentPage, int pageSize, out int totalCount)
		{
            string where = "";
            string order = "";

            DataSet ds;
            SqlParameter[] parameters = {
                     new SqlParameter("@where", SqlDbType.VarChar,8000),
			new SqlParameter("@order", SqlDbType.NVarChar,100),
			new SqlParameter("@pagesize", SqlDbType.Int,4),
			new SqlParameter("@page", SqlDbType.Int,4),
			new SqlParameter("@totalRecorder", SqlDbType.Int,4)
             };

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_BRAND_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
		}

		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.ProjectTask_DMSMember_Brand model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@MemberID", SqlDbType.Int,4),
					new SqlParameter("@BrandID", SqlDbType.Int,4),
					new SqlParameter("@SerialID", SqlDbType.VarChar,50),
					new SqlParameter("@Type", SqlDbType.Char,10),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.MemberID;
            parameters[1].Value = model.BrandID;
            parameters[2].Value = model.SerialID;
            parameters[3].Value = model.Type;
            parameters[4].Value = model.CreateTime;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_BRAND_INSERT, parameters);

            return Convert.ToInt32(parameters[0].Value.ToString());
		}
		
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ProjectTask_DMSMember_Brand model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@MemberID", SqlDbType.Int,4),
					new SqlParameter("@BrandID", SqlDbType.Int,4),
					new SqlParameter("@SerialID", SqlDbType.VarChar,50),
					new SqlParameter("@Type", SqlDbType.Char,10),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.MemberID;
            parameters[1].Value = model.BrandID;
            parameters[2].Value = model.SerialID;
            parameters[3].Value = model.Type;
            parameters[4].Value = model.CreateTime;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_BRAND_UPDATE, parameters);
		}
		
		#endregion

        #region SelectSingle
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="rid">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        public Entities.ProjectTask_DMSMember_Brand GetProjectTask_DMSMember_Brand(int rid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@Rid", SqlDbType.Int,4)
                    };

            parameters[0].Value = rid;
            //绑定存储过程参数

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_BRAND_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleProjectTask_DMSMember_Brand(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }

        private static Entities.ProjectTask_DMSMember_Brand LoadSingleProjectTask_DMSMember_Brand(DataRow row)
        {
            Entities.ProjectTask_DMSMember_Brand model = new Entities.ProjectTask_DMSMember_Brand();

            if (row["MemberID"] != DBNull.Value)
            {
                model.MemberID = Convert.ToInt32(row["MemberID"].ToString());
            }

            if (row["BrandID"] != DBNull.Value)
            {
                model.BrandID = Convert.ToInt32(row["BrandID"].ToString());
            }

            if (row["SerialID"] != DBNull.Value)
            {
                model.SerialID = row["SerialID"].ToString();
            }

            if (row["Type"] != DBNull.Value)
            {
                model.Type = row["Type"].ToString();
            }

            if (row["CreateTime"] != DBNull.Value)
            {
                model.CreateTime = Convert.ToDateTime(row["CreateTime"].ToString());
            }
            return model;
        }
        #endregion

        /// <summary>
        /// 更新会员与子品牌对应关系
        /// </summary>
        public void UpdateMemberSerial(int memberId, string brandIds, string serialIds, int type)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MemberID", SqlDbType.Int),
					new SqlParameter("@BrandID", SqlDbType.VarChar,4000),
					new SqlParameter("@SerialID", SqlDbType.VarChar,4000),
					new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)
            };
            parameters[0].Value = memberId;
            parameters[1].Value = brandIds;
            parameters[2].Value = serialIds;
            parameters[3].Value = type;
            parameters[4].Value = DateTime.Now;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DMSMEMBER_BRAND_UPDATE, parameters);
        }

        internal DataTable GetProjectTask_DMSMember_MainBrand(int memberId)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@MemberID", SqlDbType.VarChar,8000)
            };

            parameters[0].Value = memberId;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_ProjectTask_DMSMember_Brand_SelectMainBrand", parameters);
            return ds.Tables[0];
        }

        internal DataTable GetProjectTask_DMSMember_SerialBrand(int memberId)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@MemberID", SqlDbType.VarChar,8000)
            };

            parameters[0].Value = memberId;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_ProjectTask_DMSMember_Brand_SELECTSerialBrand", parameters);
            return ds.Tables[0];
        }

        public void DeleteByMemberID(int memberID)
        {
            string sql = string.Format("Delete FROM ProjectTask_DMSMember_Brand Where MemberID={0}", memberID);
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

	}
}

