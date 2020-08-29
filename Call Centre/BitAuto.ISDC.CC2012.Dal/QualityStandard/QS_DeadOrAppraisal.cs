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
	/// ���ݷ�����QS_DeadOrAppraisal��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:35 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_DeadOrAppraisal : DataBase
	{
		#region Instance
		public static readonly QS_DeadOrAppraisal Instance = new QS_DeadOrAppraisal();
		#endregion

		#region const
		private const string P_QS_DEADORAPPRAISAL_SELECT = "p_QS_DeadOrAppraisal_Select";
		private const string P_QS_DEADORAPPRAISAL_INSERT = "p_QS_DeadOrAppraisal_Insert";
		private const string P_QS_DEADORAPPRAISAL_UPDATE = "p_QS_DeadOrAppraisal_Update";
		private const string P_QS_DEADORAPPRAISAL_DELETE = "p_QS_DeadOrAppraisal_Delete";
		#endregion

		#region Contructor
		protected QS_DeadOrAppraisal()
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
		public DataTable GetQS_DeadOrAppraisal(QueryQS_DeadOrAppraisal query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.QS_DAID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_DAID=" + query.QS_DAID;
            }
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RTID=" + query.QS_RTID;
            }
            if (query.DeadItemName != Constant.STRING_INVALID_VALUE)
            {
                where += " And DeadItemName  like '%" + StringHelper.SqlFilter(query.DeadItemName) + "%'";
            }
           
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + query.Status;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And CreateUserID=" + query.CreateUserID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_DEADORAPPRAISAL_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.QS_DeadOrAppraisal GetQS_DeadOrAppraisal(int QS_DAID)
		{
			QueryQS_DeadOrAppraisal query = new QueryQS_DeadOrAppraisal();
			query.QS_DAID = QS_DAID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_DeadOrAppraisal(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleQS_DeadOrAppraisal(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.QS_DeadOrAppraisal LoadSingleQS_DeadOrAppraisal(DataRow row)
		{
			Entities.QS_DeadOrAppraisal model=new Entities.QS_DeadOrAppraisal();

				if(row["QS_DAID"].ToString()!="")
				{
					model.QS_DAID=int.Parse(row["QS_DAID"].ToString());
				}
				if(row["QS_RTID"].ToString()!="")
				{
					model.QS_RTID=int.Parse(row["QS_RTID"].ToString());
				}
				model.DeadItemName=row["DeadItemName"].ToString();
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["CreateUserID"].ToString()!="")
				{
					model.CreateUserID=int.Parse(row["CreateUserID"].ToString());
				}
				if(row["LastModifyTime"].ToString()!="")
				{
					model.LastModifyTime=DateTime.Parse(row["LastModifyTime"].ToString());
				}
				if(row["LastModifyUserID"].ToString()!="")
				{
					model.LastModifyUserID=int.Parse(row["LastModifyUserID"].ToString());
				}
				if(row["Sort"].ToString()!="")
				{
					model.Sort=int.Parse(row["Sort"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(Entities.QS_DeadOrAppraisal model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_DAID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@DeadItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.DeadItemName;
			parameters[3].Value = model.Status;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;
			parameters[6].Value = model.LastModifyTime;
			parameters[7].Value = model.LastModifyUserID;
			parameters[8].Value = model.Sort;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_DEADORAPPRAISAL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.QS_DeadOrAppraisal model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_DAID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@DeadItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.DeadItemName;
			parameters[3].Value = model.Status;
			parameters[4].Value = model.CreateTime;
			parameters[5].Value = model.CreateUserID;
			parameters[6].Value = model.LastModifyTime;
			parameters[7].Value = model.LastModifyUserID;
			parameters[8].Value = model.Sort;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_DEADORAPPRAISAL_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.QS_DeadOrAppraisal model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_DAID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@DeadItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),				 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_DAID;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.DeadItemName;
			parameters[3].Value = model.Status;
            parameters[4].Value = model.LastModifyTime;
            parameters[5].Value = model.LastModifyUserID;
            parameters[6].Value = model.Sort;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_DEADORAPPRAISAL_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_DeadOrAppraisal model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_DAID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@DeadItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
				 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_DAID;
			parameters[1].Value = model.QS_RTID;
			parameters[2].Value = model.DeadItemName;
			parameters[3].Value = model.Status;
		 
			parameters[4].Value = model.LastModifyTime;
			parameters[5].Value = model.LastModifyUserID;
			parameters[6].Value = model.Sort;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_DEADORAPPRAISAL_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int QS_DAID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_DAID", SqlDbType.Int,4)};
			parameters[0].Value = QS_DAID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_DEADORAPPRAISAL_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_DAID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_DAID", SqlDbType.Int,4)};
			parameters[0].Value = QS_DAID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_DEADORAPPRAISAL_DELETE,parameters);
		}
		#endregion

	}
}

