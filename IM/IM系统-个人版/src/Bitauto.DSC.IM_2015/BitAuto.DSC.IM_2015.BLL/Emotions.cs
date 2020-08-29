using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
	//----------------------------------------------------------------------------------
	/// <summary>
	/// ҵ���߼���Emotions ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:02 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class Emotions
	{
		#region Instance
		public static readonly Emotions Instance = new Emotions();
		#endregion

		#region Contructor
		protected Emotions()
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
		public DataTable GetEmotions(QueryEmotions query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.Emotions.Instance.GetEmotions(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.Emotions.Instance.GetEmotions(new QueryEmotions(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.Emotions GetEmotions(int RecID)
		{
			
			return Dal.Emotions.Instance.GetEmotions(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryEmotions query = new QueryEmotions();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetEmotions(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.Emotions model)
		{
			return Dal.Emotions.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.Emotions model)
		{
			return Dal.Emotions.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.Emotions model)
		{
			return Dal.Emotions.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.Emotions model)
		{
			return Dal.Emotions.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.Emotions.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.Emotions.Instance.Delete(sqltran, RecID);
		}

		#endregion

        public DataTable GetEmotionInfoByECategory(string strWhere)
        {
            return Dal.Emotions.Instance.GetEmotionInfoByECategory(strWhere);
        }

	}
}

