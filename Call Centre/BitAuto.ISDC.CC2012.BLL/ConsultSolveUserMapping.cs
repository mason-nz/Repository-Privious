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
	/// ҵ���߼���ConsultSolveUserMapping ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:12 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class ConsultSolveUserMapping
	{
		#region Instance
		public static readonly ConsultSolveUserMapping Instance = new ConsultSolveUserMapping();
		#endregion

		#region Contructor
		protected ConsultSolveUserMapping()
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
		public DataTable GetConsultSolveUserMapping(QueryConsultSolveUserMapping query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.ConsultSolveUserMapping.Instance.GetConsultSolveUserMapping(query,order,currentPage,pageSize,out totalCount);
		}
        /// <summary>
        /// ����ģ��������Ʋ�ѯ������Ա
        /// </summary>
        /// <param name="templateCategoryName"></param>
        /// <returns></returns>
        public DataTable GetEmailConsultSolveUserByTemplateCategoryName(string firstTemplateCategoryName, string secondTemplateCategoryNames)
        {
            return Dal.ConsultSolveUserMapping.Instance.GetEmailConsultSolveUserByTemplateCategoryName(firstTemplateCategoryName, secondTemplateCategoryNames);
        }

        /// <summary>
        /// ����ģ��ID��ѯ�����Ϣ
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public DataTable GetConsultSolveUserByTemplateID(int templateId)
        {
            return Dal.ConsultSolveUserMapping.Instance.GetConsultSolveUserByTemplateID(templateId);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.ConsultSolveUserMapping.Instance.GetConsultSolveUserMapping(new QueryConsultSolveUserMapping(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.ConsultSolveUserMapping GetConsultSolveUserMapping(int RecID)
		{
			
			return Dal.ConsultSolveUserMapping.Instance.GetConsultSolveUserMapping(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryConsultSolveUserMapping query = new QueryConsultSolveUserMapping();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetConsultSolveUserMapping(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.ConsultSolveUserMapping model)
		{
			return Dal.ConsultSolveUserMapping.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.ConsultSolveUserMapping model)
		{
			return Dal.ConsultSolveUserMapping.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.ConsultSolveUserMapping.Instance.Delete(RecID);
		}

		#endregion

	}
}

