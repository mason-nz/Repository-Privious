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
	/// ҵ���߼���SurveyProjectInfo ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-10-24 10:32:18 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class SurveyProjectInfo
	{
		#region Instance
		public static readonly SurveyProjectInfo Instance = new SurveyProjectInfo();
		#endregion

		#region Contructor
		protected SurveyProjectInfo()
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
		public DataTable GetSurveyProjectInfo(QuerySurveyProjectInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// ��ȡ��Ŀ���д�����Ա
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllCreateUserID()
        {
            return Dal.SurveyProjectInfo.Instance.GetAllCreateUserID(BLL.Util.GetLoginUserID());
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyProjectInfo.Instance.GetSurveyProjectInfo(new QuerySurveyProjectInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.SurveyProjectInfo GetSurveyProjectInfo(int SPIID)
		{
			return Dal.SurveyProjectInfo.Instance.GetSurveyProjectInfo(SPIID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsBySPIID(int SPIID)
		{
			QuerySurveyProjectInfo query = new QuerySurveyProjectInfo();
			query.SPIID = SPIID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSurveyProjectInfo(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyProjectInfo model)
		{
			return Dal.SurveyProjectInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int SPIID)
		{
			
			return Dal.SurveyProjectInfo.Instance.Delete(SPIID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int SPIID)
		{
			
			return Dal.SurveyProjectInfo.Instance.Delete(sqltran, SPIID);
		}

		#endregion

	}
}

