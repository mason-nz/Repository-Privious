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
	/// 业务逻辑类ExamOnlineDetail 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:16 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ExamOnlineDetail
	{
		#region Instance
		public static readonly ExamOnlineDetail Instance = new ExamOnlineDetail();
		#endregion

		#region Contructor
		protected ExamOnlineDetail()
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
		public DataTable GetExamOnlineDetail(QueryExamOnlineDetail query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ExamOnlineDetail.Instance.GetExamOnlineDetail(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ExamOnlineDetail.Instance.GetExamOnlineDetail(new QueryExamOnlineDetail(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ExamOnlineDetail GetExamOnlineDetail(long EOLDID)
		{
			
			return Dal.ExamOnlineDetail.Instance.GetExamOnlineDetail(EOLDID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByEOLDID(long EOLDID)
		{
			QueryExamOnlineDetail query = new QueryExamOnlineDetail();
			query.EOLDID = EOLDID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetExamOnlineDetail(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ExamOnlineDetail model)
		{
			return Dal.ExamOnlineDetail.Instance.Update(sqltran, model);
		}

		#endregion

        #region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long EOLDID)
		{
			
			return Dal.ExamOnlineDetail.Instance.Delete(EOLDID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long EOLDID)
		{
			
			return Dal.ExamOnlineDetail.Instance.Delete(sqltran, EOLDID);
		}
         
		#endregion

        /// <summary>
        /// add by qizq 更新小题分数
        /// </summary>
        /// <param name="EOLID"></param>
        /// <param name="BQID"></param>
        /// <param name="KLQID"></param>
        /// <param name="Score"></param>
        public void UpdateByEOLID(string EOLID, string BQID, string KLQID, string Score)
        {
            Dal.ExamOnlineDetail.Instance.UpdateByEOLID(EOLID,BQID,KLQID,Score);
        }

	}
}

