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
	/// ҵ���߼���ProjectTask_DMSMember_Brand ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2013-02-20 10:39:31 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ProjectTask_DMSMember_Brand
	{
		#region Instance
		public static readonly ProjectTask_DMSMember_Brand Instance = new ProjectTask_DMSMember_Brand();
		#endregion

		#region Contructor
		protected ProjectTask_DMSMember_Brand()
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
        public DataTable GetProjectTask_DMSMember_Brand(QueryProjectTask_DMSMember_Brand query, int currentPage, int pageSize, out int totalCount)
        {
			return Dal.ProjectTask_DMSMember_Brand.Instance.GetProjectTask_DMSMember_Brand(query,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ProjectTask_DMSMember_Brand.Instance.GetProjectTask_DMSMember_Brand(new QueryProjectTask_DMSMember_Brand(),1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByMemberIDAndBrandIDAndSerialIDAndType(int MemberID,int BrandID,string SerialID,string Type)
		{
			QueryProjectTask_DMSMember_Brand query = new QueryProjectTask_DMSMember_Brand();
			query.MemberID = MemberID;
			query.BrandID = BrandID;
			query.SerialID = SerialID;
			query.Type = Type;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetProjectTask_DMSMember_Brand(query, 1, 1, out count);
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
		public void Insert(Entities.ProjectTask_DMSMember_Brand model)
		{
			Dal.ProjectTask_DMSMember_Brand.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ProjectTask_DMSMember_Brand model)
		{
			return Dal.ProjectTask_DMSMember_Brand.Instance.Update(model);
		}


		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
        public void DeleteByMemberID(int memberID)
        {

            Dal.ProjectTask_DMSMember_Brand.Instance.DeleteByMemberID(memberID);
		}

		#endregion

	}
}

