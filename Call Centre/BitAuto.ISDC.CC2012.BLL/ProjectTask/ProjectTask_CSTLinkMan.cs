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
	/// 业务逻辑类ProjectTask_CSTLinkMan 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:29 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_CSTLinkMan
	{
		#region Instance
		public static readonly ProjectTask_CSTLinkMan Instance = new ProjectTask_CSTLinkMan();
		#endregion

		#region Contructor
		protected ProjectTask_CSTLinkMan()
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
		public DataTable GetProjectTask_CSTLinkMan(QueryProjectTask_CSTLinkMan query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(new QueryProjectTask_CSTLinkMan(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_CSTLinkMan GetProjectTask_CSTLinkMan(int RecID)
        {
            return Dal.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(RecID);
        }
        public Entities.ProjectTask_CSTLinkMan GetProjectTask_CSTLinkManModel(int ID)
        {
            return Dal.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkManModel(ID);
        }

		#endregion

		#region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryProjectTask_CSTLinkMan query = new QueryProjectTask_CSTLinkMan();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_CSTLinkMan(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ProjectTask_CSTLinkMan model)
		{
			return Dal.ProjectTask_CSTLinkMan.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.ProjectTask_CSTLinkMan model)
		{
			return Dal.ProjectTask_CSTLinkMan.Instance.Update(model);
		}

		#endregion

		#region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            return Dal.ProjectTask_CSTLinkMan.Instance.Delete(RecID);
        }

        /// <summary>
        /// 根据任务ID，更新CC_CSTLinkMan中，Status值为-1
        /// </summary>
        /// <param name="tid">任务ID</param>
        public int DeleteByTID(string tid)
        {
            return Dal.ProjectTask_CSTLinkMan.Instance.DeleteByTID(tid);
        }

        /// <summary>
        /// 更加车商通会员编号删除联系人信息
        /// </summary>
        /// <param name="cstMemberId"></param>
        /// <returns></returns>
        public int DeleteByCstMemberID(int cstMemberId)
        {
            return Dal.ProjectTask_CSTLinkMan.Instance.Delete(cstMemberId);
        }
		#endregion

	}
}

