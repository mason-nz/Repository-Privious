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
	/// ���ݷ�����SendSMSLog��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:20 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class SendSMSLog : DataBase
	{
		#region Instance
		public static readonly SendSMSLog Instance = new SendSMSLog();
		#endregion

		#region const
		private const string P_SENDSMSLOG_SELECT = "p_SendSMSLog_Select";
		private const string P_SENDSMSLOG_INSERT = "p_SendSMSLog_Insert";
		private const string P_SENDSMSLOG_UPDATE = "p_SendSMSLog_Update";
		private const string P_SENDSMSLOG_DELETE = "p_SendSMSLog_Delete";
		#endregion

		#region Contructor
		protected SendSMSLog()
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
		public DataTable GetSendSMSLog(QuerySendSMSLog query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

			DataSet ds;

			SqlParameter[] parameters = {
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

			parameters[0].Value = order;
			parameters[1].Value = pageSize;
			parameters[2].Value = currentPage;
			parameters[3].Direction = ParameterDirection.Output;

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SENDSMSLOG_SELECT, parameters);
			totalCount = (int)(parameters[3].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.SendSMSLog GetSendSMSLog(int RecID)
		{
			QuerySendSMSLog query = new QuerySendSMSLog();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSendSMSLog(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleSendSMSLog(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.SendSMSLog LoadSingleSendSMSLog(DataRow row)
		{
			Entities.SendSMSLog model=new Entities.SendSMSLog();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["TemplateID"].ToString()!="")
				{
					model.TemplateID=int.Parse(row["TemplateID"].ToString());
				}
				model.CustID=row["CustID"].ToString();
				model.Mobile=row["Mobile"].ToString();
				if(row["SendTime"].ToString()!="")
				{
					model.SendTime=DateTime.Parse(row["SendTime"].ToString());
				}
				model.SendContent=row["SendContent"].ToString();
				if(row["CreateUserID"].ToString()!="")
				{
					model.CreateUserID=int.Parse(row["CreateUserID"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(Entities.SendSMSLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Mobile", SqlDbType.VarChar,11),
					new SqlParameter("@SendTime", SqlDbType.DateTime),
					new SqlParameter("@SendContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.CustID;
			parameters[3].Value = model.Mobile;
			parameters[4].Value = model.SendTime;
			parameters[5].Value = model.SendContent;
			parameters[6].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SENDSMSLOG_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.SendSMSLog model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Mobile", SqlDbType.VarChar,11),
					new SqlParameter("@SendTime", SqlDbType.DateTime),
					new SqlParameter("@SendContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.TemplateID;
			parameters[2].Value = model.CustID;
			parameters[3].Value = model.Mobile;
			parameters[4].Value = model.SendTime;
			parameters[5].Value = model.SendContent;
			parameters[6].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SENDSMSLOG_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SENDSMSLOG_DELETE,parameters);
		}
		#endregion

	}
}

