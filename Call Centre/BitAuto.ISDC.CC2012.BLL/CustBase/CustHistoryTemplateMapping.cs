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
	/// ҵ���߼���CustHistoryTemplateMapping ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-09 02:39:28 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustHistoryTemplateMapping
	{
		#region Instance
		public static readonly CustHistoryTemplateMapping Instance = new CustHistoryTemplateMapping();
		#endregion

		#region Contructor
		protected CustHistoryTemplateMapping()
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
		public DataTable GetCustHistoryTemplateMapping(QueryCustHistoryTemplateMapping query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.CustHistoryTemplateMapping.Instance.GetCustHistoryTemplateMapping(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.CustHistoryTemplateMapping.Instance.GetCustHistoryTemplateMapping(new QueryCustHistoryTemplateMapping(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.CustHistoryTemplateMapping GetCustHistoryTemplateMapping(long RecID)
		{
			
			return Dal.CustHistoryTemplateMapping.Instance.GetCustHistoryTemplateMapping(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(long RecID)
		{
			QueryCustHistoryTemplateMapping query = new QueryCustHistoryTemplateMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetCustHistoryTemplateMapping(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.CustHistoryTemplateMapping model)
		{
			return Dal.CustHistoryTemplateMapping.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.CustHistoryTemplateMapping model)
		{
			return Dal.CustHistoryTemplateMapping.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long RecID)
		{
			
			return Dal.CustHistoryTemplateMapping.Instance.Delete(RecID);
		}

		#endregion

	}
}

