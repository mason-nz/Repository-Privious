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
	/// ���ݷ�����ProjectTask_CSTLinkMan��
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
	public class ProjectTask_CSTLinkMan : DataBase
	{
		#region Instance
		public static readonly ProjectTask_CSTLinkMan Instance = new ProjectTask_CSTLinkMan();
		#endregion

		#region const
		private const string P_PROJECTTASK_CSTLINKMAN_SELECT = "p_ProjectTask_CSTLinkMan_Select";
		private const string P_PROJECTTASK_CSTLINKMAN_INSERT = "p_ProjectTask_CSTLinkMan_Insert";
		private const string P_PROJECTTASK_CSTLINKMAN_UPDATE = "p_ProjectTask_CSTLinkMan_Update";
		private const string P_PROJECTTASK_CSTLINKMAN_DELETE = "p_ProjectTask_CSTLinkMan_Delete";
		#endregion

		#region Contructor
		protected ProjectTask_CSTLinkMan()
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
		public DataTable GetProjectTask_CSTLinkMan(QueryProjectTask_CSTLinkMan query, string order, int currentPage, int pageSize, out int totalCount)
		{
            string where = string.Empty;
            if (query.CSTMemberID != Constant.INT_INVALID_VALUE)
            {
                where += " And CSTMemberID=" + query.CSTMemberID;
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " And RecID=" + query.RecID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTLINKMAN_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
		}

		#endregion

		#region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_CSTLinkMan GetProjectTask_CSTLinkMan(int RecID)
        {
            QueryProjectTask_CSTLinkMan query = new QueryProjectTask_CSTLinkMan();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTLinkMan(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_CSTLinkMan(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_CSTLinkMan GetProjectTask_CSTLinkManModel(int ID)
        {
            QueryProjectTask_CSTLinkMan query = new QueryProjectTask_CSTLinkMan();
            query.CSTMemberID = ID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTLinkMan(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_CSTLinkMan(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        private Entities.ProjectTask_CSTLinkMan LoadSingleProjectTask_CSTLinkMan(DataRow row)
        {
            Entities.ProjectTask_CSTLinkMan model = new Entities.ProjectTask_CSTLinkMan();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["PTID"].ToString() != "")
            {
                model.PTID = row["PTID"].ToString();
            }
            if (row["CSTMemberID"].ToString() != "")
            {
                model.CSTMemberID = int.Parse(row["CSTMemberID"].ToString());
            }
            if (row["OriginalCSTLinkManID"].ToString() != "")
            {
                model.OriginalCSTLinkManID = int.Parse(row["OriginalCSTLinkManID"].ToString());
            }
            model.CSTRecID = row["CSTRecID"].ToString();
            model.Name = row["Name"].ToString();
            model.Department = row["Department"].ToString();
            model.Position = row["Position"].ToString();
            model.Mobile = row["Mobile"].ToString();
            model.Email = row["Email"].ToString();
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
		///  ����һ������
		/// </summary>
		public int Insert(Entities.ProjectTask_CSTLinkMan model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@CSTMemberID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Department", SqlDbType.NVarChar,100),
					new SqlParameter("@Position", SqlDbType.NVarChar,100),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,100),
					new SqlParameter("@Email", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.CSTMemberID;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.Department;
            parameters[5].Value = model.Position;
            parameters[6].Value = model.Mobile;
            parameters[7].Value = model.Email;
            parameters[8].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTLINKMAN_INSERT, parameters);
            return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.ProjectTask_CSTLinkMan model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@CSTMemberID", SqlDbType.Int,4),
					new SqlParameter("@OriginalCSTLinkManID", SqlDbType.Int,4),
					new SqlParameter("@CSTRecID", SqlDbType.VarChar,10),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Department", SqlDbType.NVarChar,100),
					new SqlParameter("@Position", SqlDbType.NVarChar,100),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,100),
					new SqlParameter("@Email", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.CSTMemberID;
            parameters[2].Value = model.OriginalCSTLinkManID;
            parameters[3].Value = model.CSTRecID;
            parameters[4].Value = model.Name;
            parameters[5].Value = model.Department;
            parameters[6].Value = model.Position;
            parameters[7].Value = model.Mobile;
            parameters[8].Value = model.Email;
            parameters[9].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTLINKMAN_UPDATE, parameters);
		}
		#endregion

		#region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CSTLINKMAN_DELETE, parameters);
        }

        /// <summary>
        /// ���ӳ���ͨ��Ա���ɾ����ϵ����Ϣ
        /// </summary>
        /// <param name="cstMemberId"></param>
        /// <returns></returns>
        public int DeleteByCstMemberID(int cstMemberId)
        {
            string sqlStr = "UPDATE ProjectTask_CSTLinkMan SET Status=-1 WHERE CSTMemberID=@CSTMemberID";
            SqlParameter[] parameters = {
					new SqlParameter("@CSTMemberID", SqlDbType.Int,4)};
            parameters[0].Value = cstMemberId;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }

        /// <summary>
        /// ��������ID������CC_CSTLinkMan�У�StatusֵΪ-1
        /// </summary>
        /// <param name="tid">����ID</param>
        public int DeleteByTID(string tid)
        {
            string sqlStr = string.Format("UPDATE ProjectTask_CSTLinkMan SET Status=-1 WHERE PTID='{0}'", StringHelper.SqlFilter(tid));
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }
		#endregion

	}
}

