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
	/// ҵ���߼���GroupLabel ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-10-29 10:21:03 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class GroupLabel
	{
		#region Instance
		public static readonly GroupLabel Instance = new GroupLabel();
		#endregion

		#region Contructor
		protected GroupLabel()
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
		public DataTable GetGroupLabel(QueryGroupLabel query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.GroupLabel.Instance.GetGroupLabel(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.GroupLabel.Instance.GetGroupLabel(new QueryGroupLabel(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.GroupLabel GetGroupLabel(int RecID)
		{
			
			return Dal.GroupLabel.Instance.GetGroupLabel(RecID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByRecID(int RecID)
		{
			QueryGroupLabel query = new QueryGroupLabel();
			query.RecID = RecID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetGroupLabel(query, string.Empty, 1, 1, out count);
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
		public int  Insert(Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Insert(SqlTransaction sqltran, Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.GroupLabel model)
		{
			return Dal.GroupLabel.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			
			return Dal.GroupLabel.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran, int RecID)
		{
			
			return Dal.GroupLabel.Instance.Delete(sqltran, RecID);
		}

		#endregion

        public DataTable GetLabelConfig(string where)
        {
            return Dal.GroupLabel.Instance.GetLabelConfig(where);
        }

        /// <summary>
        /// ��������ҵ����-��ǩ�м������
        /// </summary>
        /// <param name="bgid"></param>
        /// <param name="ltids"></param>
        /// <param name="userid"></param>
        public void SaveDataBatch(int bgid, string ltids, int userid)
        {
            Dal.GroupLabel.Instance.SaveDataBatch(bgid, ltids, userid);
        }
	}
}

