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
	/// 业务逻辑类TemplateInfo 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:23 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class TemplateInfo
	{
		#region Instance
		public static readonly TemplateInfo Instance = new TemplateInfo();
		#endregion

		#region Contructor
		protected TemplateInfo()
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
		public DataTable GetTemplateInfo(QueryTemplateInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.TemplateInfo.Instance.GetTemplateInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.TemplateInfo.Instance.GetTemplateInfo(new QueryTemplateInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.TemplateInfo GetTemplateInfo(int RecID)
		{
			
			return Dal.TemplateInfo.Instance.GetTemplateInfo(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryTemplateInfo query = new QueryTemplateInfo();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetTemplateInfo(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        /// <summary>
        /// 是否存在同名称的模版
        /// </summary>
        /// <param name="tcId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsExist(int tcId, string title)
        {
            return Dal.TemplateInfo.Instance.IsExist(tcId, title);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="tcId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsExistNotThisRecID(int recId, int tcId, string title)
        {
            return Dal.TemplateInfo.Instance.IsExistNotThisRecID(recId, tcId, title);
        }
        #endregion

        #region Insert
        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Insert(Entities.TemplateInfo model)
		{
			return Dal.TemplateInfo.Instance.Insert(model);
		}

        public void InsertUser_Template(string sqlStr)
        {
            
        }


		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.TemplateInfo model)
		{
			return Dal.TemplateInfo.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.TemplateInfo.Instance.Delete(RecID);
		}

		#endregion

        #region ClearUser
        /// <summary>
        /// 清理一个模板的接收人
        /// </summary>
        public int ClearUser(int RecID)
        {
            return Dal.TemplateInfo.Instance.ClearUser(RecID);
        }
        #endregion

        #region getEmailServers(int)
        /// <summary>
        /// 获取接受邮件的用户
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public DataTable getEmailServers(int TemplateID)
        {
            return Dal.TemplateInfo.Instance.getEmailServers(TemplateID);
        }
        #endregion
	}
}

