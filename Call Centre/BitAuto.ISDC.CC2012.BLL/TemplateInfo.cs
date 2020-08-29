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
	/// ҵ���߼���TemplateInfo ��ժҪ˵����
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
		/// ���ղ�ѯ������ѯ
		/// </summary>
		/// <param name="query">��ѯ����</param>
		/// <param name="order">����</param>
		/// <param name="currentPage">ҳ��,-1����ҳ</param>
		/// <param name="pageSize">ÿҳ��¼��</param>
		/// <param name="totalCount">������</param>
		/// <returns>����</returns>
		public DataTable GetTemplateInfo(QueryTemplateInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.TemplateInfo.Instance.GetTemplateInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.TemplateInfo.Instance.GetTemplateInfo(new QueryTemplateInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.TemplateInfo GetTemplateInfo(int RecID)
		{
			
			return Dal.TemplateInfo.Instance.GetTemplateInfo(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
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
        /// �Ƿ����ͬ���Ƶ�ģ��
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
		/// ����һ������
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
		/// ����һ������
		/// </summary>
		public int Update(Entities.TemplateInfo model)
		{
			return Dal.TemplateInfo.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.TemplateInfo.Instance.Delete(RecID);
		}

		#endregion

        #region ClearUser
        /// <summary>
        /// ����һ��ģ��Ľ�����
        /// </summary>
        public int ClearUser(int RecID)
        {
            return Dal.TemplateInfo.Instance.ClearUser(RecID);
        }
        #endregion

        #region getEmailServers(int)
        /// <summary>
        /// ��ȡ�����ʼ����û�
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

