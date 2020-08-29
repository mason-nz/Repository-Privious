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
	/// ҵ���߼���CustContactInfo ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2014-12-23 06:17:00 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustContactInfo
	{
		#region Instance
		public static readonly CustContactInfo Instance = new CustContactInfo();
		#endregion

		#region Contructor
		protected CustContactInfo()
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
		public DataTable GetCustContactInfo(QueryCustContactInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.CustContactInfo.Instance.GetCustContactInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.CustContactInfo.Instance.GetCustContactInfo(new QueryCustContactInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.CustContactInfo GetCustContactInfo(int RecID)
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return Dal.CustContactInfo.Instance.GetCustContactInfo(RecID);
		}

        /// <summary>
        /// ���ݸ����û�ID��ȡʵ��
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public Entities.CustContactInfo GetCustContactInfoByCustID(string CustID)
        {
            return Dal.CustContactInfo.Instance.GetCustContactInfoByCustID(CustID);
        }

		#endregion

		#region IsExists

		#endregion

		#region Insert
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(Entities.CustContactInfo model)
		{
			Dal.CustContactInfo.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Insert(SqlTransaction sqltran, Entities.CustContactInfo model)
		{
			Dal.CustContactInfo.Instance.Insert(sqltran, model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.CustContactInfo model)
		{
			return Dal.CustContactInfo.Instance.Update(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(SqlTransaction sqltran, Entities.CustContactInfo model)
		{
			return Dal.CustContactInfo.Instance.Update(sqltran, model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(int RecID)
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return Dal.CustContactInfo.Instance.Delete(RecID);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(SqlTransaction sqltran,int RecID)
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return Dal.CustContactInfo.Instance.Delete(sqltran,RecID);
		}

		#endregion

	}
}

