using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM2014.Entities;
using BitAuto.DSC.IM2014.Entities.Constants;

namespace BitAuto.DSC.IM2014.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// 业务逻辑类AgentInfo 的摘要说明。
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-03-05 10:05:57 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class AgentInfo
	{
		#region Instance
		public static readonly AgentInfo Instance = new AgentInfo();
		#endregion

		#region Contructor
		protected AgentInfo()
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
		public DataTable GetAgentInfo(QueryAgentInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.AgentInfo.Instance.GetAgentInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.AgentInfo.Instance.GetAgentInfo(new QueryAgentInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Entities.AgentInfo GetAgentInfo(string AgentID)
		{
			
			return Dal.AgentInfo.Instance.GetAgentInfo(AgentID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool IsExistsByAgentID(string AgentID)
		{
			QueryAgentInfo query = new QueryAgentInfo();
			query.AgentID = AgentID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetAgentInfo(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.AgentInfo model)
		{
			Dal.AgentInfo.Instance.Insert(model);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.AgentInfo model)
		{
			Dal.AgentInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(Entities.AgentInfo model)
		{
			return Dal.AgentInfo.Instance.Update(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.AgentInfo model)
		{
			return Dal.AgentInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(int AgentID)
		{
			
			return Dal.AgentInfo.Instance.Delete(AgentID);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public int Delete(SqlTransaction sqltran, int AgentID)
		{
			
			return Dal.AgentInfo.Instance.Delete(sqltran, AgentID);
		}

		#endregion

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsExistAgentIMID(string username)
        {
            return Dal.AgentInfo.Instance.IsExistAgentIMID(username);
        }
        /// <summary>
        /// 根据域帐号判断IMID是否存在
        /// </summary>
        /// <param name="domainaccount"></param>
        /// <returns></returns>
        public bool IsExistAgentIMIDByDomainAccount(string domainaccount, out string agentid,out string username)
        {
            QueryAgentInfo query = new QueryAgentInfo();
            query.DomainAccount = domainaccount;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAgentInfo(query, string.Empty, 1, 1, out count);
            agentid = "";
            username = "";
            if (count > 0)
            {
                agentid = Convert.ToString(dt.Rows[0]["AgentID"]);
                username = dt.Rows[0]["username"].ToString();
                if (agentid != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据坐席ID获取在聊网友数上限值
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public int GetAgentMaxDialogCount(string agentid)
        {
            Entities.AgentInfo agent = Dal.AgentInfo.Instance.GetAgentInfo(agentid);
            if (agent != null)
            {
                return Convert.ToInt32(agent.MaxDialogCount);
            }
            return 0;
        }
        /// <summary>
        /// 根据域名从CC取数据初始化AgentInfo
        /// </summary>
        /// <param name="domainaccount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool InitAgentInfo(string domainaccount,out string username,out string agentid,out string msg)
        {
            return Dal.AgentInfo.Instance.InitAgentInfo(domainaccount,out username,out agentid, out msg);
        }
	}
}

