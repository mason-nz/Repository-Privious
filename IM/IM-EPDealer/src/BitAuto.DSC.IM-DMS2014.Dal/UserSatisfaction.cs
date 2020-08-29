using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Dal
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ���ݷ�����UserSatisfaction��
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:05 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class UserSatisfaction : DataBase
	{
		#region Instance
		public static readonly UserSatisfaction Instance = new UserSatisfaction();
		#endregion

		#region const
		private const string P_USERSATISFACTION_SELECT = "p_UserSatisfaction_Select";
		private const string P_USERSATISFACTION_INSERT = "p_UserSatisfaction_Insert";
		private const string P_USERSATISFACTION_UPDATE = "p_UserSatisfaction_Update";
		private const string P_USERSATISFACTION_DELETE = "p_UserSatisfaction_Delete";
		#endregion

		#region Contructor
		protected UserSatisfaction()
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
		public DataTable GetUserSatisfaction(QueryUserSatisfaction query, string order, int currentPage, int pageSize, out int totalCount)
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.UserSatisfaction GetUserSatisfaction(int RecID)
		{
			QueryUserSatisfaction query = new QueryUserSatisfaction();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserSatisfaction(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleUserSatisfaction(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.UserSatisfaction LoadSingleUserSatisfaction(DataRow row)
		{
			Entities.UserSatisfaction model=new Entities.UserSatisfaction();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				if(row["CSID"].ToString()!="")
				{
					model.CSID=int.Parse(row["CSID"].ToString());
				}
				if(row["PerSatisfaction"].ToString()!="")
				{
					model.PerSatisfaction=int.Parse(row["PerSatisfaction"].ToString());
				}
				if(row["ProSatisfaction"].ToString()!="")
				{
					model.ProSatisfaction=int.Parse(row["ProSatisfaction"].ToString());
				}
				model.Contents=row["Contents"].ToString();
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
		public int Insert(Entities.UserSatisfaction model)
		{
			SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.PerSatisfaction;
			parameters[3].Value = model.ProSatisfaction;
			parameters[4].Value = model.Contents;
			parameters[5].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.UserSatisfaction model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.PerSatisfaction;
			parameters[3].Value = model.ProSatisfaction;
			parameters[4].Value = model.Contents;
			parameters[5].Value = model.CreateTime;

			SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERSATISFACTION_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(Entities.UserSatisfaction model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.PerSatisfaction;
			parameters[3].Value = model.ProSatisfaction;
			parameters[4].Value = model.Contents;
			parameters[5].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_UPDATE,parameters);
		}
		/// <summary>
		///  ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserSatisfaction model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CSID;
			parameters[2].Value = model.PerSatisfaction;
			parameters[3].Value = model.ProSatisfaction;
			parameters[4].Value = model.Contents;
			parameters[5].Value = model.CreateTime;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERSATISFACTION_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_DELETE,parameters);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERSATISFACTION_DELETE,parameters);
		}
		#endregion


        //<summary>
        //�ж϶�ָ���Ự������������Ƿ����
        //</summary>
        //<param name="CSID">�ỰID</param>
        //<returns></returns>
        public bool SatisfactionExists(int CSID)
        {
            String strSql = "select count(1) from UserSatisfaction where CSID=@CSID";
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)
			};
            parameters[0].Value = CSID;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);

            int objNum;
            if (obj != null && int.TryParse(obj.ToString(), out objNum))
            {
                if (objNum > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

	}
}

