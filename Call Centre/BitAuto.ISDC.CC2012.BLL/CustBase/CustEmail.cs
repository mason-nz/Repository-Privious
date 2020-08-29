using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类CustEmail 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:13 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustEmail
	{
		#region Instance
		public static readonly CustEmail Instance = new CustEmail();
		#endregion

		#region Contructor
		protected CustEmail()
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
		public DataTable GetCustEmail(QueryCustEmail query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.CustEmail.Instance.GetCustEmail(query,order,currentPage,pageSize,out totalCount);
		}
        /// <summary>
        /// 获取客户下的所有邮件信息
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable GetCustEmail(string custId)
        {
            return Dal.CustEmail.Instance.GetCustEmail(custId);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.CustEmail.Instance.GetCustEmail(new QueryCustEmail(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.CustEmail GetCustEmail(string CustID,string Email)
		{
			
			return Dal.CustEmail.Instance.GetCustEmail(CustID,Email);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByCustIDAndEmail(string CustID,string Email)
		{
			QueryCustEmail query = new QueryCustEmail();
			query.CustID = CustID;
			query.Email = Email;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetCustEmail(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Insert
/// <summary>
		/// 增加一条数据
		/// </summary>
        public void Insert(Entities.CustEmail model)
		{
            Dal.CustEmail.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CustEmail model)
		{
            Dal.CustEmail.Instance.Insert(sqltran,model);
		}

        /// <summary>
        /// 批量插入邮件
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="tels"></param>
        public void Insert(string custId, string emails)
        {
            string[] emailArry = emails.Split(',');
            foreach (string email in emailArry)
            {
                Entities.CustEmail model = new Entities.CustEmail();
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.CustID = custId;
                model.Email = email;
                BLL.CustEmail.Instance.Insert(model);
            }
        }
		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.CustEmail model)
		{
			return Dal.CustEmail.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除客户下，所有邮箱信息
		/// </summary>
		public int Delete(string CustID)
		{
			
			return Dal.CustEmail.Instance.Delete(CustID);
		}

		#endregion

	}
}

