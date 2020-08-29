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
	/// 业务逻辑类SurveyProjectInfo 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:18 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class SurveyProjectInfo
	{
		#region Instance
		public static readonly SurveyProjectInfo Instance = new SurveyProjectInfo();
		#endregion

		#region Contructor
		protected SurveyProjectInfo()
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
		public DataTable GetSurveyProjectInfo(QuerySurveyProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// 获取项目所有创建人员
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllCreateUserID()
        {
            return Dal.SurveyProjectInfo.Instance.GetAllCreateUserID(BLL.Util.GetLoginUserID());
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyProjectInfo.Instance.GetSurveyProjectInfo(new QuerySurveyProjectInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.SurveyProjectInfo GetSurveyProjectInfo(int SPIID)
		{
			return Dal.SurveyProjectInfo.Instance.GetSurveyProjectInfo(SPIID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsBySPIID(int SPIID)
		{
			QuerySurveyProjectInfo query = new QuerySurveyProjectInfo();
			query.SPIID = SPIID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSurveyProjectInfo(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int SPIID)
		{
			
			return Dal.SurveyProjectInfo.Instance.Delete(SPIID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int SPIID)
		{
			
			return Dal.SurveyProjectInfo.Instance.Delete(sqltran, SPIID);
		}

		#endregion

	}
}

