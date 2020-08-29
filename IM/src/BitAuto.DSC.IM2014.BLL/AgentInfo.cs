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
	/// ҵ���߼���AgentInfo ��ժҪ˵����
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
		/// ���ղ�ѯ������ѯ
		/// </summary>
		/// <param name="query">��ѯ����</param>
		/// <param name="order">����</param>
		/// <param name="currentPage">ҳ��,-1����ҳ</param>
		/// <param name="pageSize">ÿҳ��¼��</param>
		/// <param name="totalCount">������</param>
		/// <returns>����</returns>
		public DataTable GetAgentInfo(QueryAgentInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.AgentInfo.Instance.GetAgentInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.AgentInfo.Instance.GetAgentInfo(new QueryAgentInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.AgentInfo GetAgentInfo(string AgentID)
		{
			
			return Dal.AgentInfo.Instance.GetAgentInfo(AgentID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
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
		/// ����һ������
		/// </summary>
		public void Insert(Entities.AgentInfo model)
		{
			Dal.AgentInfo.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.AgentInfo model)
		{
			Dal.AgentInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.AgentInfo model)
		{
			return Dal.AgentInfo.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.AgentInfo model)
		{
			return Dal.AgentInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int AgentID)
		{
			
			return Dal.AgentInfo.Instance.Delete(AgentID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int AgentID)
		{
			
			return Dal.AgentInfo.Instance.Delete(sqltran, AgentID);
		}

		#endregion

        /// <summary>
        /// �ж��û��Ƿ����
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsExistAgentIMID(string username)
        {
            return Dal.AgentInfo.Instance.IsExistAgentIMID(username);
        }
        /// <summary>
        /// �������ʺ��ж�IMID�Ƿ����
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
        /// ������ϯID��ȡ��������������ֵ
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
        /// ����������CCȡ���ݳ�ʼ��AgentInfo
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

