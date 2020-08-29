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
	/// ҵ���߼���KLQAnswer ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-08-21 10:19:08 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class KLQAnswer
	{
		#region Instance
		public static readonly KLQAnswer Instance = new KLQAnswer();
		#endregion

		#region Contructor
		protected KLQAnswer()
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
		public DataTable GetKLQAnswer(QueryKLQAnswer query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.KLQAnswer.Instance.GetKLQAnswer(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.KLQAnswer.Instance.GetKLQAnswer(new QueryKLQAnswer(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.KLQAnswer GetKLQAnswer(long KLQID,int KLAOID)
		{
			
			return Dal.KLQAnswer.Instance.GetKLQAnswer(KLQID,KLAOID);
		}
        /// <summary>
        /// ��ȡ�����µĴ�
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public DataTable GetKLQAnswerByKLQID(long KLQID)
        {
            return Dal.KLQAnswer.Instance.GetKLQAnswerByKLQID(KLQID);
        }
		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByKLQIDAndKLAOID(long KLQID,int KLAOID)
		{
			QueryKLQAnswer query = new QueryKLQAnswer();
			query.KLQID = KLQID;
			query.KLAOID = KLAOID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetKLQAnswer(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.KLQAnswer model)
		{
			Dal.KLQAnswer.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.KLQAnswer model)
		{
			Dal.KLQAnswer.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.KLQAnswer model)
		{
			return Dal.KLQAnswer.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLQAnswer model)
		{
			return Dal.KLQAnswer.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long KLQID,int KLAOID)
		{
			
			return Dal.KLQAnswer.Instance.Delete(KLQID,KLAOID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long KLQID,int KLAOID)
		{
			
			return Dal.KLQAnswer.Instance.Delete(sqltran, KLQID,KLAOID);
		}

        /// <summary>
        /// ��������IDɾ�������µĴ�
        /// </summary>
        /// <param name="sqltran"></param>
        /// <param name="KLQAID"></param>
        /// <returns></returns>
        public int Delete(SqlTransaction sqltran, long KLQAID)
        {
            return Dal.KLQAnswer.Instance.Delete(sqltran, KLQAID);
        }
		#endregion

	}
}

