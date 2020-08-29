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
	/// ҵ���߼���SurveyPerson ��ժҪ˵����
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
	public class SurveyPerson
	{
		#region Instance
		public static readonly SurveyPerson Instance = new SurveyPerson();
		#endregion

		#region Contructor
		protected SurveyPerson()
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
		public DataTable GetSurveyPerson(QuerySurveyPerson query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.SurveyPerson.Instance.GetSurveyPerson(query,order,currentPage,pageSize,out totalCount);
		}
        /// <summary>
        /// ���ݵ�����ĿID����ѯ������Ա��Ϣ
        /// </summary>
        /// <param name="spiId"></param>
        /// <returns></returns>
        public DataTable GetSurveyPersonBySPIID(int spiId)
        {
            return Dal.SurveyPerson.Instance.GetSurveyPersonBySPIID(spiId);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.SurveyPerson.Instance.GetSurveyPerson(new QuerySurveyPerson(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.SurveyPerson GetSurveyPerson(int RecID)
		{
			
			return Dal.SurveyPerson.Instance.GetSurveyPerson(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QuerySurveyPerson query = new QuerySurveyPerson();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetSurveyPerson(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.SurveyPerson model)
		{
			return Dal.SurveyPerson.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.SurveyPerson.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.SurveyPerson.Instance.Delete(sqltran, RecID);
		}

         /// <summary>
        /// ɾ�������µĲ�����
        /// </summary>
        /// <param name="spiId"></param>
        /// <returns></returns>
        public int DeleteBySPIID(int spiId)
        {
            return Dal.SurveyPerson.Instance.DeleteBySPIID(spiId);
        }
		#endregion

	}
}

