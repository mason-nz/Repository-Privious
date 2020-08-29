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
	/// 数据访问类ConsultSecondCar。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:11 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConsultSecondCar : DataBase
	{
		#region Instance
		public static readonly ConsultSecondCar Instance = new ConsultSecondCar();
		#endregion

		#region const
		private const string P_CONSULTSECONDCAR_SELECT = "p_ConsultSecondCar_Select";
		private const string P_CONSULTSECONDCAR_INSERT = "p_ConsultSecondCar_Insert";
		private const string P_CONSULTSECONDCAR_UPDATE = "p_ConsultSecondCar_Update";
		private const string P_CONSULTSECONDCAR_DELETE = "p_ConsultSecondCar_Delete";
		#endregion

		#region Contructor
		protected ConsultSecondCar()
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
		public DataTable GetConsultSecondCar(QueryConsultSecondCar query, string order, int currentPage, int pageSize, out int totalCount)
		{
			string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID.ToString();
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

			ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSECONDCAR_SELECT, parameters);
			totalCount = (int)(parameters[4].Value);
			return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ConsultSecondCar GetConsultSecondCar(int RecID)
		{
			QueryConsultSecondCar query = new QueryConsultSecondCar();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConsultSecondCar(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleConsultSecondCar(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ConsultSecondCar LoadSingleConsultSecondCar(DataRow row)
		{
			Entities.ConsultSecondCar model=new Entities.ConsultSecondCar();

				if(row["RecID"].ToString()!="")
				{
					model.RecID=int.Parse(row["RecID"].ToString());
				}
				model.CustID=row["CustID"].ToString();
				if(row["QuestionType"].ToString()!="")
				{
					model.QuestionType=int.Parse(row["QuestionType"].ToString());
				}
				if(row["CarBrandId"].ToString()!="")
				{
					model.CarBrandId=int.Parse(row["CarBrandId"].ToString());
				}
				if(row["CarSerialId"].ToString()!="")
				{
					model.CarSerialId=int.Parse(row["CarSerialId"].ToString());
				}
				model.CarName=row["CarName"].ToString();
				if(row["SaleCarBrandId"].ToString()!="")
				{
					model.SaleCarBrandId=int.Parse(row["SaleCarBrandId"].ToString());
				}
				if(row["SaleCarSerialId"].ToString()!="")
				{
					model.SaleCarSerialId=int.Parse(row["SaleCarSerialId"].ToString());
				}
				model.SaleCarName=row["SaleCarName"].ToString();
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
		public int Insert(Entities.ConsultSecondCar model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@QuestionType", SqlDbType.Int,4),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleCarBrandId", SqlDbType.Int,4),
					new SqlParameter("@SaleCarSerialId", SqlDbType.Int,4),
					new SqlParameter("@SaleCarName", SqlDbType.NVarChar,200),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.QuestionType;
			parameters[3].Value = model.CarBrandId;
			parameters[4].Value = model.CarSerialId;
			parameters[5].Value = model.CarName;
			parameters[6].Value = model.SaleCarBrandId;
			parameters[7].Value = model.SaleCarSerialId;
			parameters[8].Value = model.SaleCarName;
			parameters[9].Value = model.CallRecord;
			parameters[10].Value = model.CreateTime;
			parameters[11].Value = model.CreateUserID;

			SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSECONDCAR_INSERT,parameters);
			return (int)parameters[0].Value;
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ConsultSecondCar model)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@QuestionType", SqlDbType.Int,4),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleCarBrandId", SqlDbType.Int,4),
					new SqlParameter("@SaleCarSerialId", SqlDbType.Int,4),
					new SqlParameter("@SaleCarName", SqlDbType.NVarChar,200),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
			parameters[0].Value = model.RecID;
			parameters[1].Value = model.CustID;
			parameters[2].Value = model.QuestionType;
			parameters[3].Value = model.CarBrandId;
			parameters[4].Value = model.CarSerialId;
			parameters[5].Value = model.CarName;
			parameters[6].Value = model.SaleCarBrandId;
			parameters[7].Value = model.SaleCarSerialId;
			parameters[8].Value = model.SaleCarName;
			parameters[9].Value = model.CallRecord;
			parameters[10].Value = model.CreateTime;
			parameters[11].Value = model.CreateUserID;

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSECONDCAR_UPDATE,parameters);
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

			return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTSECONDCAR_DELETE,parameters);
		}
		#endregion

	}
}

