using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类UserSatisfaction 的摘要说明。
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
	public class UserSatisfaction
	{
		#region Instance
		public static readonly UserSatisfaction Instance = new UserSatisfaction();
		#endregion

		#region Contructor
		protected UserSatisfaction()
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
		public DataTable GetUserSatisfaction(QueryUserSatisfaction query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.UserSatisfaction.Instance.GetUserSatisfaction(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.UserSatisfaction.Instance.GetUserSatisfaction(new QueryUserSatisfaction(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.UserSatisfaction GetUserSatisfaction(int RecID)
		{
			
			return Dal.UserSatisfaction.Instance.GetUserSatisfaction(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryUserSatisfaction query = new QueryUserSatisfaction();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetUserSatisfaction(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.UserSatisfaction model)
		{
			return Dal.UserSatisfaction.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.UserSatisfaction.Instance.Delete(RecID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.UserSatisfaction.Instance.Delete(sqltran, RecID);
		}

		#endregion


        //<summary>
        //判断对指定会话的满意度评价是否存在
        //</summary>
        //<param name="CSID">会话ID</param>
        //<returns></returns>
        public bool SatisfactionExists(int CSID)
        {
            return Dal.UserSatisfaction.Instance.SatisfactionExists(CSID);
        }

        /// <summary>
        /// 取满意度统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount">总条数</param>
        /// <param name="logUserID">当前登录人</param>
        /// <returns>日统计返回：（daytime：日期），周统计返回（weekb:周开始日期，weeke：周结束日期），月统计返回：（monthb:月开始日期，monthe:月结束日期），（其他：userid，truename：姓名，agentnum：工号，sumduihua：总对话量，chanping：参评数，profcmy：产品非常满意数，promy：产品满意数，proyb：产品一般，probmy：产品不满意数，profcbmy：产品非常不满意数，perfcmy：服务非常满意，permy：服务满意数，peryb：服务一般，perbmy：服务不满意，perfcbmy：服务非常不满意）</returns>
        public DataTable UserSatisfaction_Total_Select(QueryUserSatisfactionTotal query, string order, int currentPage, int pageSize, out int totalCount, int logUserID)
        {
            return Dal.UserSatisfaction.Instance.UserSatisfaction_Total_Select(query, order, currentPage, pageSize, out totalCount, logUserID);
        }

        /// <summary>
        /// 取满意度统计数据汇总数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns>返回值：sumduihua：总对话量，chanping：参评数，profcmy：产品非常满意数，promy：产品满意数，proyb：产品一般，probmy：产品不满意数，profcbmy：产品非常不满意数，perfcmy：服务非常满意，permy：服务满意数，peryb：服务一般，perbmy：服务不满意，perfcbmy：服务非常不满意</returns>
        public DataTable UserSatisfaction_Total_Select(QueryUserSatisfactionTotal query, int logUserID)
        {
            return Dal.UserSatisfaction.Instance.UserSatisfaction_Total_Select(query,logUserID);
        }


        

	}
}

