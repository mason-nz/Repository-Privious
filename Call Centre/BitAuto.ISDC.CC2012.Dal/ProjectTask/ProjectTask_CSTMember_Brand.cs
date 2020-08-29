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
	/// ���ݷ�����ProjectTask_CSTMember_Brand��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_CSTMember_Brand : DataBase
	{
		#region Instance
		public static readonly ProjectTask_CSTMember_Brand Instance = new ProjectTask_CSTMember_Brand();
		#endregion

		#region const
		private const string P_PROJECTTASK_CSTMEMBER_BRAND_SELECT = "p_ProjectTask_CSTMember_Brand_Select";
		private const string P_PROJECTTASK_CSTMEMBER_BRAND_INSERT = "p_ProjectTask_CSTMember_Brand_Insert";
		private const string P_PROJECTTASK_CSTMEMBER_BRAND_UPDATE = "p_ProjectTask_CSTMember_Brand_Update";
		private const string P_PROJECTTASK_CSTMEMBER_BRAND_DELETE = "p_ProjectTask_CSTMember_Brand_Delete";
		#endregion

		#region Contructor
		protected ProjectTask_CSTMember_Brand()
		{}
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
		public DataTable GetProjectTask_CSTMember_Brand(QueryProjectTask_CSTMember_Brand query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_BRAND_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

        public Entities.ProjectTask_CSTMember_Brand GetProjectTask_CSTMember_Brand1(int ID)
        {
            string where = string.Empty;
            Entities.ProjectTask_CSTMember_Brand model_Brand = null;
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@MemberID", SqlDbType.Int) 
					};

            parameters[0].Value = ID;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_rojectTask_CSTMember_Brand_SelectMainBrand", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                model_Brand = new Entities.ProjectTask_CSTMember_Brand();
                model_Brand.BrandID = int.Parse(ds.Tables[0].Rows[0]["BrandID"].ToString());
                model_Brand.CreateTime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
                model_Brand.CSTMemberID = int.Parse(ds.Tables[0].Rows[0]["CSTMemberID"].ToString());
                model_Brand.CreateUserID = int.Parse(ds.Tables[0].Rows[0]["CreateUserID"].ToString());
            }
            return model_Brand;
        }
		#endregion

		#region GetModel
        /// <summary>
        /// ���ݳ���ͨ��Ա��Ϣ��ID������ӪƷ����Ϣ
        /// </summary>
        /// <param name="memberId">����ͨ��Ա��ϢID</param>
        /// <returns></returns>
        public DataTable GetProjectTask_CSTMember_MainBrand(int memberId)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@MemberID", SqlDbType.VarChar,8000)
            };

            parameters[0].Value = memberId;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_rojectTask_CSTMember_Brand_SELECTMainBrand", parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// ���ݳ���ͨ��Ա��Ϣ��ID������ӪƷ����Ϣ
        /// </summary>
        /// <param name="memberId">����ͨ��Ա��ϢID</param>
        /// <returns></returns>
        public List<Entities.ProjectTask_CSTMember_Brand> GetProjectTask_CSTMember_Brand(int memberId)
        {
            List<Entities.ProjectTask_CSTMember_Brand> list = new List<Entities.ProjectTask_CSTMember_Brand>();
            SqlParameter[] parameters = {
                new SqlParameter("@MemberID", SqlDbType.VarChar,8000)
            };

            parameters[0].Value = memberId;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_projectTask_CSTMember_Brand_SELECTMainBrand", parameters);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Entities.ProjectTask_CSTMember_Brand brandInfo = LoadSingleProjectTask_CSTMember_Brand(dr);
                list.Add(brandInfo);
            }
            return list;
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_CSTMember_Brand GetProjectTask_CSTMember_Brand(int CSTMemberID, int BrandID)
        {
            QueryProjectTask_CSTMember_Brand query = new QueryProjectTask_CSTMember_Brand();
            query.CSTMemberID = CSTMemberID;
            query.BrandID = BrandID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTMember_Brand(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_CSTMember_Brand(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ProjectTask_CSTMember_Brand LoadSingleProjectTask_CSTMember_Brand(DataRow row)
        {
            Entities.ProjectTask_CSTMember_Brand model = new Entities.ProjectTask_CSTMember_Brand();

            if (row["CSTMemberID"].ToString() != "")
            {
                model.CSTMemberID = int.Parse(row["CSTMemberID"].ToString());
            }
            if (row["BrandID"].ToString() != "")
            {
                model.BrandID = int.Parse(row["BrandID"].ToString());
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
		///  ����һ������
		/// </summary>
		public void Insert(Entities.ProjectTask_CSTMember_Brand model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@CSTMemberID", SqlDbType.Int,4),
					new SqlParameter("@BrandID", SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CSTMemberID;
            parameters[1].Value = model.BrandID;
            parameters[2].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_BRAND_INSERT, parameters);
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.ProjectTask_CSTMember_Brand model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@CSTMemberID", SqlDbType.Int,4),
					new SqlParameter("@BrandID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CSTMemberID;
            parameters[1].Value = model.BrandID;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_BRAND_UPDATE, parameters);
		}
		#endregion

		#region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int CSTMemberID, int BrandID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSTMemberID", SqlDbType.Int,4),
					new SqlParameter("@BrandID", SqlDbType.Int,4)};
            parameters[0].Value = CSTMemberID;
            parameters[1].Value = BrandID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTMEMBER_BRAND_DELETE, parameters);
        }

        /// <summary>
        /// ���ݳ���ͨ��Ա��ɾ����ӪƷ����Ϣ
        /// </summary>
        /// <param name="CSTMemberID"></param>
        /// <returns></returns>
        public int Delete(int CSTMemberID)
        {
            string sqlStr = "DELETE FROM ProjectTask_CSTMember_Brand WHERE CstMemberID=@CSTMemberID";
            SqlParameter[] parameters = {
					new SqlParameter("@CSTMemberID", SqlDbType.Int,4)};
            parameters[0].Value = CSTMemberID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }

        /// <summary>
        /// ɾ����Ա�µ���ӪƷ��
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            string sqlStr = string.Format("DELETE FROM ProjectTask_CSTMember_Brand WHERE CSTMemberID IN (SELECT CSTMemberID FROM ProjectTask_CSTMember WHERE Status=0 AND PTID='{0}')", StringHelper.SqlFilter(tid));
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }
		#endregion

        #region IsExist
        /// <summary>
        /// �Ƿ���ڴ���ӪƷ��
        /// </summary>
        /// <param name="cstMemberId"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public bool IsExist(int cstMemberId, int brandId)
        {
            string sqlStr = "SELECT * FROM  ProjectTask_CSTMember_Brand where CstMemberID=@CstMemberID and BrandID=@BrandID";
            SqlParameter[] parameter ={
            new SqlParameter("@CstMemberID",SqlDbType.Int),
            new SqlParameter("@BrandID",SqlDbType.Int)
            };
            parameter[0].Value = cstMemberId;
            parameter[1].Value = brandId;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
	}
}

