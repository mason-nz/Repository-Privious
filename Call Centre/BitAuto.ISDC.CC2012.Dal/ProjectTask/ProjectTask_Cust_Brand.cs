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
	/// ���ݷ�����ProjectTask_Cust_Brand��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_Cust_Brand : DataBase
	{
		#region Instance
		public static readonly ProjectTask_Cust_Brand Instance = new ProjectTask_Cust_Brand();
		#endregion

		#region const
		private const string P_PROJECTTASK_CUST_BRAND_SELECT = "p_ProjectTask_Cust_Brand_Select";
        public const string P_PROJECTTASK_CUST_BRAND_SELECT_BY_ID = "p_ProjectTask_Cust_Brand_Select_By_ID";
        public const string P_PROJECTTASK_CUST_Brand_UpdataByTID = "P_ProjectTask_Cust_Brand_UpdataByTID";
		private const string P_PROJECTTASK_CUST_BRAND_INSERT = "p_ProjectTask_Cust_Brand_Insert";
		#endregion

		#region Contructor
		protected ProjectTask_Cust_Brand()
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
		public DataTable GetProjectTask_Cust_Brand(QueryProjectTask_Cust_Brand query, string order, int currentPage, int pageSize, out int totalCount)
		{
            string where = "";

            if (query.PTID!=Constant.STRING_INVALID_VALUE)
            {
                where += " and ProjectTask_Cust_Brand.PTID ='" + StringHelper.SqlFilter(query.PTID) + "'";
            }
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
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_BRAND_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ProjectTask_Cust_Brand GetProjectTask_Cust_Brand(string PTID,int BrandID)
		{
			QueryProjectTask_Cust_Brand query = new QueryProjectTask_Cust_Brand();
			query.PTID = PTID;
			query.BrandID = BrandID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetProjectTask_Cust_Brand(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleProjectTask_Cust_Brand(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ProjectTask_Cust_Brand LoadSingleProjectTask_Cust_Brand(DataRow row)
		{
			Entities.ProjectTask_Cust_Brand model=new Entities.ProjectTask_Cust_Brand();

				model.PTID=row["PTID"].ToString();
				if(row["BrandID"].ToString()!="")
				{
					model.BrandID=int.Parse(row["BrandID"].ToString());
				}
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(Entities.ProjectTask_Cust_Brand model)
		{
            SqlParameter[] parameters = {
				new SqlParameter("@PTID", SqlDbType.VarChar),
				new SqlParameter("@BrandID", SqlDbType.Int,4),
				new SqlParameter("@CreateTime", SqlDbType.DateTime)
            };
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.BrandID;
            parameters[2].Value = model.CreateTime;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_BRAND_INSERT, parameters);

            return Convert.ToInt32(parameters[0].Value.ToString());
		}
		#endregion

        /// <summary>
        /// ���¿ͻ�����ӪƷ��
        /// ���ͻ���������ӪƷ��ɾ���������Ӧ��ӪƷ��
        /// </summary>
        public void UpdataBrandInfoByTID(string tID, List<int> brandIDs)
        {
            StringBuilder sb = new StringBuilder();
            if (brandIDs != null)
            {
                foreach (int id in brandIDs)
                {
                    sb.Append(id + ",");
                }
            }
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@BrandIDs", SqlDbType.VarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = tID;
            parameters[1].Value = sb.ToString().Trim(',');
            parameters[2].Value = DateTime.Now;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_Brand_UpdataByTID, parameters);
        }

        #region SelectSingle
        /// <summary>
        /// ����ID��ѯ����������һ����¼
        /// </summary>
        /// <param name="rid">����ID</param>
        /// <returns>����������һ��ֵ����</returns>
        public Entities.ProjectTask_Cust_Brand GetCC_Cust_Brand(int rid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@Rid", SqlDbType.Int,4)
                    };

            parameters[0].Value = rid;
            //�󶨴洢���̲���

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_BRAND_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleCC_Cust_Brand(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }

        private static Entities.ProjectTask_Cust_Brand LoadSingleCC_Cust_Brand(DataRow row)
        {
            Entities.ProjectTask_Cust_Brand model = new Entities.ProjectTask_Cust_Brand();
            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["BrandID"] != DBNull.Value)
            {
                model.BrandID = Convert.ToInt32(row["BrandID"].ToString());
            }

            if (row["CreateTime"] != DBNull.Value)
            {
                model.CreateTime = Convert.ToDateTime(row["CreateTime"].ToString());
            }

            return model;
        }
        #endregion

        /// <summary>
        /// ���ݿͻ�����ID��ɾ���ͻ���ӪƷ����Ϣ
        /// </summary>
        /// <param name="tid"></param>
        public void DeleteByTID(string tid)
        {
            string sql = string.Format("Delete FROM ProjectTask_Cust_Brand Where PTID='{0}'", StringHelper.SqlFilter(tid));
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

	}
}

