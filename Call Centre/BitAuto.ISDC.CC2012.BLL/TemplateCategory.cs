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
	/// ҵ���߼���TemplateCategory ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:22 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class TemplateCategory
	{
		#region Instance
		public static readonly TemplateCategory Instance = new TemplateCategory();
		#endregion

		#region Contructor
		protected TemplateCategory()
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
		public DataTable GetTemplateCategory(QueryTemplateCategory query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.TemplateCategory.Instance.GetTemplateCategory(query,order,currentPage,pageSize,out totalCount);
		}
        public DataTable GetTemplateCategoryByTemplateCategoryName(string firstTemplateCategoryName, string secondTemplateCategoryNames)
        {
            return Dal.TemplateCategory.Instance.GetTemplateCategoryByTemplateCategoryName(firstTemplateCategoryName, secondTemplateCategoryNames);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.TemplateCategory.Instance.GetTemplateCategory(new QueryTemplateCategory(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.TemplateCategory GetTemplateCategory(int RecID)
		{
			
			return Dal.TemplateCategory.Instance.GetTemplateCategory(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryTemplateCategory query = new QueryTemplateCategory();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetTemplateCategory(query, string.Empty, 1, 1, out count);
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
        /// <param name="model"></param>
		/// <returns></returns>
		public int  Insert(Entities.TemplateCategory model)
		{
			return Dal.TemplateCategory.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.TemplateCategory model)
		{
			return Dal.TemplateCategory.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.TemplateCategory.Instance.Delete(RecID);
		}

		#endregion

	}
}

