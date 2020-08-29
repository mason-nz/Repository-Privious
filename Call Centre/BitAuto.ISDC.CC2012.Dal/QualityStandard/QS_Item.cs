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
	/// ���ݷ�����QS_Item��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-04-25 09:42:36 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class QS_Item : DataBase
	{
		#region Instance
		public static readonly QS_Item Instance = new QS_Item();
		#endregion

		#region const
		private const string P_QS_ITEM_SELECT = "p_QS_Item_Select";
		private const string P_QS_ITEM_INSERT = "p_QS_Item_Insert";
		private const string P_QS_ITEM_UPDATE = "p_QS_Item_Update";
		private const string P_QS_ITEM_DELETE = "p_QS_Item_Delete";
		#endregion

		#region Contructor
		protected QS_Item()
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
		public DataTable GetQS_Item(QueryQS_Item query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.QS_IID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_IID=" + query.QS_IID; 
            }
            if (query.QS_CID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_CID=" + query.QS_CID;
            }
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RTID=" + query.QS_RTID;
            }
            if (query.ItemName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ItemName like '%" + StringHelper.SqlFilter(query.ItemName)+"%'";
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_ITEM_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.QS_Item GetQS_Item(int QS_IID)
		{
			QueryQS_Item query = new QueryQS_Item();
			query.QS_IID = QS_IID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetQS_Item(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleQS_Item(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.QS_Item LoadSingleQS_Item(DataRow row)
		{
			Entities.QS_Item model=new Entities.QS_Item();

				if(row["QS_IID"].ToString()!="")
				{
					model.QS_IID=int.Parse(row["QS_IID"].ToString());
				}
				if(row["QS_CID"].ToString()!="")
				{
					model.QS_CID=int.Parse(row["QS_CID"].ToString());
				}
				if(row["QS_RTID"].ToString()!="")
				{
					model.QS_RTID=int.Parse(row["QS_RTID"].ToString());
				}
				model.ItemName=row["ItemName"].ToString();
				if(row["ScoreType"].ToString()!="")
				{
					model.ScoreType=int.Parse(row["ScoreType"].ToString());
				}
				if(row["Score"].ToString()!="")
				{
					model.Score=decimal.Parse(row["Score"].ToString());
				}
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
		public int Insert(Entities.QS_Item model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.QS_CID;
			parameters[2].Value = model.QS_RTID;
			parameters[3].Value = model.ItemName;
			parameters[4].Value = model.ScoreType;
			parameters[5].Value = model.Score;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.CreateTime;
			parameters[8].Value = model.CreateUserID;
			parameters[9].Value = model.LastModifyTime;
			parameters[10].Value = model.LastModifyUserID;
			parameters[11].Value = model.Sort;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_ITEM_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.QS_Item model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.QS_CID;
			parameters[2].Value = model.QS_RTID;
			parameters[3].Value = model.ItemName;
			parameters[4].Value = model.ScoreType;
			parameters[5].Value = model.Score;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.CreateTime;
			parameters[8].Value = model.CreateUserID;
			parameters[9].Value = model.LastModifyTime;
			parameters[10].Value = model.LastModifyUserID;
			parameters[11].Value = model.Sort;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_ITEM_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.QS_Item model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@Status", SqlDbType.Int,4),
					 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_IID;
			parameters[1].Value = model.QS_CID;
			parameters[2].Value = model.QS_RTID;
			parameters[3].Value = model.ItemName;
			parameters[4].Value = model.ScoreType;
			parameters[5].Value = model.Score;
			parameters[6].Value = model.Status;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.Sort;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_ITEM_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.QS_Item model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ItemName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@Status", SqlDbType.Int,4),
				 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
			parameters[0].Value = model.QS_IID;
			parameters[1].Value = model.QS_CID;
			parameters[2].Value = model.QS_RTID;
			parameters[3].Value = model.ItemName;
			parameters[4].Value = model.ScoreType;
			parameters[5].Value = model.Score;
			parameters[6].Value = model.Status;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.Sort;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_ITEM_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int QS_IID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_IID", SqlDbType.Int,4)};
			parameters[0].Value = QS_IID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_ITEM_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int QS_IID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@QS_IID", SqlDbType.Int,4)};
			parameters[0].Value = QS_IID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_ITEM_DELETE,parameters);
		}
		#endregion

	}
}

