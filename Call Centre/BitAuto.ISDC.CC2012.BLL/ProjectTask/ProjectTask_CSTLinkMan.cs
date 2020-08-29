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
	/// ҵ���߼���ProjectTask_CSTLinkMan ��ժҪ˵����
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
		/// ���ղ�ѯ������ѯ
		/// </summary>
		/// <param name="query">��ѯ����</param>
		/// <param name="order">����</param>
		/// <param name="currentPage">ҳ��,-1����ҳ</param>
		/// <param name="pageSize">ÿҳ��¼��</param>
		/// <param name="totalCount">������</param>
		/// <returns>����</returns>
		public DataTable GetProjectTask_CSTLinkMan(QueryProjectTask_CSTLinkMan query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(new QueryProjectTask_CSTLinkMan(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
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
        /// �Ƿ���ڸü�¼
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
		/// ����һ������
		/// </summary>
		public int  Insert(Entities.ProjectTask_CSTLinkMan model)
		{
			return Dal.ProjectTask_CSTLinkMan.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ProjectTask_CSTLinkMan model)
		{
			return Dal.ProjectTask_CSTLinkMan.Instance.Update(model);
		}

		#endregion

		#region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {
            return Dal.ProjectTask_CSTLinkMan.Instance.Delete(RecID);
        }

        /// <summary>
        /// ��������ID������CC_CSTLinkMan�У�StatusֵΪ-1
        /// </summary>
        /// <param name="tid">����ID</param>
        public int DeleteByTID(string tid)
        {
            return Dal.ProjectTask_CSTLinkMan.Instance.DeleteByTID(tid);
        }

        /// <summary>
        /// ���ӳ���ͨ��Ա���ɾ����ϵ����Ϣ
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

