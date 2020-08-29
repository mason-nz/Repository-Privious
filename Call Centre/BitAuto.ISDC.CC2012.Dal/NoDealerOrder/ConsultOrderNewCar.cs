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
	/// 数据访问类ConsultOrderNewCar。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-21 10:33:30 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConsultOrderNewCar : DataBase
	{
		#region Instance
		public static readonly ConsultOrderNewCar Instance = new ConsultOrderNewCar();
		#endregion

		#region const
		private const string P_CONSULTORDERNEWCAR_SELECT = "p_ConsultOrderNewCar_Select";
		private const string P_CONSULTORDERNEWCAR_INSERT = "p_ConsultOrderNewCar_Insert";
		private const string P_CONSULTORDERNEWCAR_UPDATE = "p_ConsultOrderNewCar_Update";
		private const string P_CONSULTORDERNEWCAR_DELETE = "p_ConsultOrderNewCar_Delete";
		#endregion

		#region Contructor
		protected ConsultOrderNewCar()
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
		public DataTable GetConsultOrderNewCar(QueryConsultOrderNewCar query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID;
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERNEWCAR_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ConsultOrderNewCar GetConsultOrderNewCar(int RecID)
		{
			QueryConsultOrderNewCar query = new QueryConsultOrderNewCar();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConsultOrderNewCar(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleConsultOrderNewCar(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ConsultOrderNewCar LoadSingleConsultOrderNewCar(DataRow row)
		{
			Entities.ConsultOrderNewCar model=new Entities.ConsultOrderNewCar();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.CustID=row["CustID"].ToString();
				if(row["CarBrandId"].ToString()!="")
				{
					model.CarBrandId=int.Parse(row["CarBrandId"].ToString());
				}
				if(row["CarSerialId"].ToString()!="")
				{
					model.CarSerialId=int.Parse(row["CarSerialId"].ToString());
				}
				if(row["CarNameID"].ToString()!="")
				{
					model.CarNameID=int.Parse(row["CarNameID"].ToString());
				}
				model.CarColor=row["CarColor"].ToString();
				model.DealerName=row["DealerName"].ToString();
				model.DealerCode=row["DealerCode"].ToString();
				model.OrderRemark=row["OrderRemark"].ToString();
				model.CallRecord=row["CallRecord"].ToString();
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["CreateUserID"].ToString()!="")
				{
					model.CreateUserID=int.Parse(row["CreateUserID"].ToString());
				}
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.ConsultOrderNewCar model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarNameID", SqlDbType.Int,4),
					new SqlParameter("@CarColor", SqlDbType.VarChar,16),
					new SqlParameter("@DealerName", SqlDbType.VarChar,200),
					new SqlParameter("@DealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CarBrandId;
			parameters[3].Value = model.CarSerialId;
			parameters[4].Value = model.CarNameID;
			parameters[5].Value = model.CarColor;
			parameters[6].Value = model.DealerName;
			parameters[7].Value = model.DealerCode;
			parameters[8].Value = model.OrderRemark;
			parameters[9].Value = model.CallRecord;
			parameters[10].Value = model.CreateTime;
			parameters[11].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERNEWCAR_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(SqlTransaction sqltran, Entities.ConsultOrderNewCar model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarNameID", SqlDbType.Int,4),
					new SqlParameter("@CarColor", SqlDbType.VarChar,16),
					new SqlParameter("@DealerName", SqlDbType.VarChar,200),
					new SqlParameter("@DealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CarBrandId;
			parameters[3].Value = model.CarSerialId;
			parameters[4].Value = model.CarNameID;
			parameters[5].Value = model.CarColor;
			parameters[6].Value = model.DealerName;
			parameters[7].Value = model.DealerCode;
			parameters[8].Value = model.OrderRemark;
			parameters[9].Value = model.CallRecord;
			parameters[10].Value = model.CreateTime;
			parameters[11].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_CONSULTORDERNEWCAR_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ConsultOrderNewCar model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarNameID", SqlDbType.Int,4),
					new SqlParameter("@CarColor", SqlDbType.VarChar,16),
					new SqlParameter("@DealerName", SqlDbType.VarChar,200),
					new SqlParameter("@DealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CarBrandId;
			parameters[3].Value = model.CarSerialId;
			parameters[4].Value = model.CarNameID;
			parameters[5].Value = model.CarColor;
			parameters[6].Value = model.DealerName;
			parameters[7].Value = model.DealerCode;
			parameters[8].Value = model.OrderRemark;
			parameters[9].Value = model.CallRecord;
			parameters[10].Value = model.CreateTime;
			parameters[11].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERNEWCAR_UPDATE,parameters);
		}
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ConsultOrderNewCar model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarNameID", SqlDbType.Int,4),
					new SqlParameter("@CarColor", SqlDbType.VarChar,16),
					new SqlParameter("@DealerName", SqlDbType.VarChar,200),
					new SqlParameter("@DealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.CarBrandId;
			parameters[3].Value = model.CarSerialId;
			parameters[4].Value = model.CarNameID;
			parameters[5].Value = model.CarColor;
			parameters[6].Value = model.DealerName;
			parameters[7].Value = model.DealerCode;
			parameters[8].Value = model.OrderRemark;
			parameters[9].Value = model.CallRecord;
			parameters[10].Value = model.CreateTime;
			parameters[11].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERNEWCAR_UPDATE,parameters);
		}
		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERNEWCAR_DELETE,parameters);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
			parameters[0].Value = RecID;

			return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERNEWCAR_DELETE,parameters);
		}
		#endregion

	}
}

