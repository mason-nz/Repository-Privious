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
	/// 业务逻辑类ExamPerson 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:19 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ExamPerson
	{
		#region Instance
		public static readonly ExamPerson Instance = new ExamPerson();
		#endregion

		#region Contructor
		protected ExamPerson()
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
		public DataTable GetExamPerson(QueryExamPerson query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ExamPerson.Instance.GetExamPerson(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ExamPerson.Instance.GetExamPerson(new QueryExamPerson(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ExamPerson GetExamPerson(long EPID)
		{
			
			return Dal.ExamPerson.Instance.GetExamPerson(EPID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByEPID(long EPID)
		{
			QueryExamPerson query = new QueryExamPerson();
			query.EPID = EPID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetExamPerson(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ExamPerson model)
		{
			return Dal.ExamPerson.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ExamPerson model)
		{
			return Dal.ExamPerson.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ExamPerson model)
		{
			return Dal.ExamPerson.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ExamPerson model)
		{
			return Dal.ExamPerson.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long EIID)
		{
			
			return Dal.ExamPerson.Instance.Delete(EIID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long EPID)
		{
			
			return Dal.ExamPerson.Instance.Delete(sqltran, EPID);
		}

		#endregion

	}
}

