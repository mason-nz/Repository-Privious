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
	/// ҵ���߼���KLAnswerOption ��ժҪ˵����
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
	public class KLAnswerOption
	{
		#region Instance
		public static readonly KLAnswerOption Instance = new KLAnswerOption();
		#endregion

		#region Contructor
		protected KLAnswerOption()
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
		public DataTable GetKLAnswerOption(QueryKLAnswerOption query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.KLAnswerOption.Instance.GetKLAnswerOption(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.KLAnswerOption.Instance.GetKLAnswerOption(new QueryKLAnswerOption(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.KLAnswerOption GetKLAnswerOption(long KLAOID)
		{
			
			return Dal.KLAnswerOption.Instance.GetKLAnswerOption(KLAOID);
		}
         /// <summary>
        /// ��ȡ�����µ�ѡ��
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public DataTable GetKLAnswerOptionByKLQID(long KLQID)
        {
            return Dal.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(KLQID);
        }

        
          /// <summary>
        /// ��ȡ�����µ�ѡ��
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public DataTable GetKLAnswerOptionByKLQID(long KLQID, string BQID)
        {
            return Dal.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(KLQID,BQID);
        }
		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByKLAOID(long KLAOID)
		{
			QueryKLAnswerOption query = new QueryKLAnswerOption();
			query.KLAOID = KLAOID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetKLAnswerOption(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.KLAnswerOption model)
		{
			return Dal.KLAnswerOption.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.KLAnswerOption model)
		{
			return Dal.KLAnswerOption.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.KLAnswerOption model)
		{
			return Dal.KLAnswerOption.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.KLAnswerOption model)
		{
			return Dal.KLAnswerOption.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(long KLAOID)
		{
			
			return Dal.KLAnswerOption.Instance.Delete(KLAOID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, long KLAOID)
		{
			
			return Dal.KLAnswerOption.Instance.Delete(sqltran, KLAOID);
		}

		#endregion

	}
}

