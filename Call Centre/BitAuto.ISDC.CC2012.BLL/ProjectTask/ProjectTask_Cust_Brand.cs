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
	/// 业务逻辑类ProjectTask_Cust_Brand 的摘要说明。
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
	public class ProjectTask_Cust_Brand
	{
		#region Instance
		public static readonly ProjectTask_Cust_Brand Instance = new ProjectTask_Cust_Brand();
		#endregion

		#region Contructor
		protected ProjectTask_Cust_Brand()
		{}
		#endregion

		#region Select
        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryProjectTask_Cust_Brand">查询值对象，用来存放查询条件</param>     
        /// <param name="currentPage">页号,-1不分页</param>   
        /// <param name="order"> </param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>销售网络集合</returns>
        public DataTable GetProjectTask_Cust_Brand(QueryProjectTask_Cust_Brand queryProjectTask_Cust_Brand,string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_Cust_Brand.Instance.GetProjectTask_Cust_Brand(queryProjectTask_Cust_Brand, order, currentPage, pageSize, out totalCount);
        }
        #endregion

        /// <summary>
        /// 根据任务ID，删除信息
        /// </summary>
        /// <param name="tid">任务ID</param>
        public void DeleteByTID(string tid)
        {
            Dal.ProjectTask_Cust_Brand.Instance.DeleteByTID(tid);
        }

	}
}

