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
	/// ҵ���߼���MakeUpExamInfo ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-09-03 02:04:21 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class MakeUpExamInfo
	{
		#region Instance
		public static readonly MakeUpExamInfo Instance = new MakeUpExamInfo();
		#endregion

		#region Contructor
		protected MakeUpExamInfo()
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
		public DataTable GetMakeUpExamInfo(QueryMakeUpExamInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfo(new QueryMakeUpExamInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public Entities.MakeUpExamInfo GetMakeUpExamInfo(int MEIID)
		{
			
			return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfo(MEIID);
		}


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.MakeUpExamInfo GetMakeUpExamInfoByEIID(int EIID)
        {

            return Dal.MakeUpExamInfo.Instance.GetMakeUpExamInfoByEIID(EIID);
        }

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByMEIID(int MEIID)
		{
			QueryMakeUpExamInfo query = new QueryMakeUpExamInfo();
			query.MEIID = MEIID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetMakeUpExamInfo(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.MakeUpExamInfo model)
		{
			return Dal.MakeUpExamInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int EIID)
		{
			
			return Dal.MakeUpExamInfo.Instance.Delete(EIID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int EIID)
		{
			
			return Dal.MakeUpExamInfo.Instance.Delete(sqltran, EIID);
		}

		#endregion

	}
}

