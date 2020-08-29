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
	/// 业务逻辑类SurveyPerson 的摘要说明。
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
	public class SurveyPerson
	{
		#region Instance
		public static readonly SurveyPerson Instance = new SurveyPerson();
		#endregion

		#region Contructor
		protected SurveyPerson()
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
		public DataTable GetSurveyPerson(QuerySurveyPerson query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyPerson.Instance.GetSurveyPerson(query,order,currentPage,pageSize,out totalCount);
		}
        /// <summary>
        /// 根据调查项目ID，查询参与人员信息
        /// </summary>
        /// <param name="spiId"></param>
        /// <returns></returns>
        public DataTable GetSurveyPersonBySPIID(int spiId)
        {
            return Dal.SurveyPerson.Instance.GetSurveyPersonBySPIID(spiId);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyPerson.Instance.GetSurveyPerson(new QuerySurveyPerson(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.SurveyPerson GetSurveyPerson(int RecID)
		{
			
			return Dal.SurveyPerson.Instance.GetSurveyPerson(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QuerySurveyPerson query = new QuerySurveyPerson();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSurveyPerson(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.SurveyPerson.Instance.Delete(RecID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.SurveyPerson.Instance.Delete(sqltran, RecID);
		}

         /// <summary>
        /// 删除调查下的参与人
        /// </summary>
        /// <param name="spiId"></param>
        /// <returns></returns>
        public int DeleteBySPIID(int spiId)
        {
            return Dal.SurveyPerson.Instance.DeleteBySPIID(spiId);
        }
		#endregion

	}
}

