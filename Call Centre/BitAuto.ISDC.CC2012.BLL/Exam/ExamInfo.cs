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
	/// 业务逻辑类ExamInfo 的摘要说明。
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
	public class ExamInfo
	{
		#region Instance
		public static readonly ExamInfo Instance = new ExamInfo();
		#endregion

		#region Contructor
		protected ExamInfo()
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
		public DataTable GetExamInfo(QueryExamInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ExamInfo.Instance.GetExamInfo(query,order,currentPage,pageSize,out totalCount);
		}

        public DataTable GetExamInfo2(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = BLL.Util.GetLoginUserID();
            return Dal.ExamInfo.Instance.GetExamInfo2(query, order, currentPage, pageSize, out totalCount, userid);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ExamInfo.Instance.GetExamInfo(new QueryExamInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.ExamInfo GetExamInfo(long EIID)
		{
			
			return Dal.ExamInfo.Instance.GetExamInfo(EIID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByEIID(long EIID)
		{
			QueryExamInfo query = new QueryExamInfo();
			query.EIID = EIID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetExamInfo(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.ExamInfo model)
		{
			return Dal.ExamInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(long EIID)
		{
			
			return Dal.ExamInfo.Instance.Delete(EIID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, long EIID)
		{
			
			return Dal.ExamInfo.Instance.Delete(sqltran, EIID);
		}

		#endregion

        #region 获取所有创建人
        /// <summary>
        /// 获取所有创建人
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCreateUsers()
        {
            return Dal.ExamInfo.Instance.GetAllCreateUsers();
        }
        #endregion

        /// <summary>
        /// 根据项目ID获取成绩
        /// </summary>
        /// <param name="eiid"></param>
        /// <returns></returns>
        public DataTable GetScoreListByEIID(string eiid)
        {
            return Dal.ExamInfo.Instance.GetScoreListByEIID(eiid);
        }
        /// <summary>
        /// 获取试卷被考试项目使用的次数
        /// </summary>
        /// <param name="epid"></param>
        /// <returns></returns>
        public int GetExamPaperUsedCount(long epid)
        {
            return Dal.ExamInfo.Instance.GetExamPaperUsedCount(epid);
        }
    }
}

