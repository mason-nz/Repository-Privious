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
	/// 数据访问类ProjectTask_Cust_Contact。
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
	public class ProjectTask_Cust_Contact : DataBase
	{
		#region Instance
		public static readonly ProjectTask_Cust_Contact Instance = new ProjectTask_Cust_Contact();
		#endregion

		#region const
		private const string P_PROJECTTASK_CUST_CONTACT_SELECT = "p_ProjectTask_Cust_Contact_Select";
        public const string P_PROJECTTASK_CUST_CONTACT_SELECT_BY_ID = "p_ProjectTask_Cust_Contact_select_by_id";
		private const string P_PROJECTTASK_CUST_CONTACT_INSERT = "p_ProjectTask_Cust_Contact_Insert";
		private const string P_PROJECTTASK_CUST_CONTACT_UPDATE = "p_ProjectTask_Cust_Contact_Update";
		private const string P_PROJECTTASK_CUST_CONTACT_DELETE = "p_ProjectTask_Cust_Contact_Delete";
		#endregion

		#region Contructor
		protected ProjectTask_Cust_Contact()
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
		public DataTable GetProjectTask_Cust_Contact(QueryProjectTask_Cust_Contact query, string order, int currentPage, int pageSize, out int totalCount)
		{
            StringBuilder where = new StringBuilder();
            if (query.PTID!=Constant.STRING_INVALID_VALUE)
            {
                where.Append(string.Format(" and contact.PTID = '{0}'", StringHelper.SqlFilter(query.PTID)));
            }

            if (query.ID != Constant.INT_INVALID_VALUE)
            {
                where.Append(string.Format(" and contact.ID = '{0}'", query.ID));
            }

            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@where", SqlDbType.VarChar,8000),
			    new SqlParameter("@order", SqlDbType.NVarChar,100),
			    new SqlParameter("@pagesize", SqlDbType.Int,4),
			    new SqlParameter("@page", SqlDbType.Int,4),
			    new SqlParameter("@totalRecorder", SqlDbType.Int,4)
             };

            parameters[0].Value = where.ToString();
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_CONTACT_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ProjectTask_Cust_Contact GetProjectTask_Cust_Contact(int ID)
		{
			QueryProjectTask_Cust_Contact query = new QueryProjectTask_Cust_Contact();
			query.ID = ID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetProjectTask_Cust_Contact(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return LoadSingleProjectTask_Cust_Contact(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private Entities.ProjectTask_Cust_Contact LoadSingleProjectTask_Cust_Contact(DataRow row)
		{
			Entities.ProjectTask_Cust_Contact model=new Entities.ProjectTask_Cust_Contact();

				if(row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				model.PTID=row["PTID"].ToString();
				if(row["OriginalContactID"].ToString()!="")
				{
					model.OriginalContactID=int.Parse(row["OriginalContactID"].ToString());
				}
				if(row["PID"].ToString()!="")
				{
					model.PID=int.Parse(row["PID"].ToString());
				}
				model.CName=row["CName"].ToString();
				model.EName=row["EName"].ToString();
				model.Sex=row["Sex"].ToString();
				model.DepartMent=row["DepartMent"].ToString();
				if(row["OfficeTypeCode"].ToString()!="")
				{
					model.OfficeTypeCode=int.Parse(row["OfficeTypeCode"].ToString());
				}
				model.Title=row["Title"].ToString();
				model.OfficeTel=row["OfficeTel"].ToString();
				model.Phone=row["Phone"].ToString();
				model.Email=row["Email"].ToString();
				model.Fax=row["Fax"].ToString();
				model.Remarks=row["Remarks"].ToString();
				model.Address=row["Address"].ToString();
				model.ZipCode=row["ZipCode"].ToString();
				model.MSN=row["MSN"].ToString();
				model.Birthday=row["Birthday"].ToString();
				if(row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["CreateUserID"].ToString()!="")
				{
					model.CreateUserID=int.Parse(row["CreateUserID"].ToString());
				}
				if(row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["ModifyUserID"].ToString()!="")
				{
					model.ModifyUserID=int.Parse(row["ModifyUserID"].ToString());
				}
				model.Hobby=row["Hobby"].ToString();
				model.Responsible=row["Responsible"].ToString();
				return model;
		}
		#endregion

		#region Insert
		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Insert(Entities.ProjectTask_Cust_Contact model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@OriginalContactID", SqlDbType.Int,4),
					new SqlParameter("@PID", SqlDbType.Int,4),
					new SqlParameter("@CName", SqlDbType.VarChar,50),
					new SqlParameter("@EName", SqlDbType.VarChar,50),
					new SqlParameter("@Sex", SqlDbType.Char,2),
					new SqlParameter("@DepartMent", SqlDbType.VarChar,100),
					new SqlParameter("@OfficeTypeCode", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.VarChar,100),
					new SqlParameter("@OfficeTel", SqlDbType.VarChar,500),
					new SqlParameter("@Phone", SqlDbType.VarChar,100),
					new SqlParameter("@Email", SqlDbType.VarChar,100),
					new SqlParameter("@Fax", SqlDbType.VarChar,50),
					new SqlParameter("@Remarks", SqlDbType.VarChar,1000),
					new SqlParameter("@Address", SqlDbType.VarChar,500),
					new SqlParameter("@ZipCode", SqlDbType.VarChar,50),
					new SqlParameter("@MSN", SqlDbType.VarChar,50),
					new SqlParameter("@Birthday", SqlDbType.VarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@Hobby", SqlDbType.VarChar,500),
					new SqlParameter("@Responsible", SqlDbType.VarChar,500),
                    new SqlParameter("@ID",SqlDbType.Int)
            };
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.OriginalContactID;
            parameters[2].Value = model.PID;
            parameters[3].Value = model.CName;
            parameters[4].Value = model.EName;
            parameters[5].Value = model.Sex;
            parameters[6].Value = model.DepartMent;
            parameters[7].Value = model.OfficeTypeCode;
            parameters[8].Value = model.Title;
            parameters[9].Value = model.OfficeTel;
            parameters[10].Value = model.Phone;
            parameters[11].Value = model.Email;
            parameters[12].Value = model.Fax;
            parameters[13].Value = model.Remarks;
            parameters[14].Value = model.Address;
            parameters[15].Value = model.ZipCode;
            parameters[16].Value = model.MSN;
            parameters[17].Value = model.Birthday;
            parameters[18].Value = model.Status;
            parameters[19].Value = model.CreateUserID;
            parameters[20].Value = DateTime.Now;
            parameters[21].Value = null;
            parameters[22].Value = DateTime.Now;
            parameters[23].Value = model.Hobby;
            parameters[24].Value = model.Responsible;
            parameters[25].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_CONTACT_INSERT, parameters);

            return Convert.ToInt32(parameters[25].Value.ToString());
		}
		#endregion

		#region Update
		/// <summary>
		///  更新一条数据
		/// </summary>
		public int Update(Entities.ProjectTask_Cust_Contact model)
		{
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PID", SqlDbType.Int,4),
					new SqlParameter("@CName", SqlDbType.VarChar,50),
					new SqlParameter("@EName", SqlDbType.VarChar,50),
					new SqlParameter("@Sex", SqlDbType.Char,2),
					new SqlParameter("@DepartMent", SqlDbType.VarChar,100),
					new SqlParameter("@OfficeTypeCode", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.VarChar,100),
					new SqlParameter("@OfficeTel", SqlDbType.VarChar,500),
					new SqlParameter("@Phone", SqlDbType.VarChar,100),
					new SqlParameter("@Email", SqlDbType.VarChar,100),
					new SqlParameter("@Fax", SqlDbType.VarChar,50),
					new SqlParameter("@Remarks", SqlDbType.VarChar,1000),
					new SqlParameter("@Address", SqlDbType.VarChar,500),
					new SqlParameter("@ZipCode", SqlDbType.VarChar,50),
					new SqlParameter("@MSN", SqlDbType.VarChar,50),
					new SqlParameter("@Birthday", SqlDbType.VarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@Hobby", SqlDbType.VarChar,500),
					new SqlParameter("@Responsible", SqlDbType.VarChar,500)
                                        };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.PID;
            parameters[2].Value = model.CName;
            parameters[3].Value = model.EName;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.DepartMent;
            parameters[6].Value = model.OfficeTypeCode;
            parameters[7].Value = model.Title;
            parameters[8].Value = model.OfficeTel;
            parameters[9].Value = model.Phone;
            parameters[10].Value = model.Email;
            parameters[11].Value = model.Fax;
            parameters[12].Value = model.Remarks;
            parameters[13].Value = model.Address;
            parameters[14].Value = model.ZipCode;
            parameters[15].Value = model.MSN;
            parameters[16].Value = model.Birthday;
            parameters[17].Value = model.Status;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CreateTime;
            parameters[20].Value = model.ModifyUserID;
            parameters[21].Value = model.ModifyTime;
            parameters[22].Value = model.Hobby;
            parameters[23].Value = model.Responsible;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_CONTACT_UPDATE, parameters);
		}
		#endregion

        public void Delete(int id)
        {
            SqlParameter[] parameters = {
			    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = id;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_CUST_CONTACT_DELETE, parameters);
        }

        public void DeleteContactByTID(string tid)
        {
            string sql = string.Format("Update ProjectTask_Cust_Contact Set Status=-1 Where Status=0 And PTID='{0}'", StringHelper.SqlFilter(tid));
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

	}
}

