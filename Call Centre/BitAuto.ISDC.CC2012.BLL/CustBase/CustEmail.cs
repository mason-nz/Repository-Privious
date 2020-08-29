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
	/// ҵ���߼���CustEmail ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:13 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustEmail
	{
		#region Instance
		public static readonly CustEmail Instance = new CustEmail();
		#endregion

		#region Contructor
		protected CustEmail()
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
		public DataTable GetCustEmail(QueryCustEmail query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.CustEmail.Instance.GetCustEmail(query,order,currentPage,pageSize,out totalCount);
		}
        /// <summary>
        /// ��ȡ�ͻ��µ������ʼ���Ϣ
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable GetCustEmail(string custId)
        {
            return Dal.CustEmail.Instance.GetCustEmail(custId);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.CustEmail.Instance.GetCustEmail(new QueryCustEmail(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.CustEmail GetCustEmail(string CustID,string Email)
		{
			
			return Dal.CustEmail.Instance.GetCustEmail(CustID,Email);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByCustIDAndEmail(string CustID,string Email)
		{
			QueryCustEmail query = new QueryCustEmail();
			query.CustID = CustID;
			query.Email = Email;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetCustEmail(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.CustEmail model)
		{
            Dal.CustEmail.Instance.Insert(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CustEmail model)
		{
            Dal.CustEmail.Instance.Insert(sqltran,model);
		}

        /// <summary>
        /// ���������ʼ�
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="tels"></param>
        public void Insert(string custId, string emails)
        {
            string[] emailArry = emails.Split(',');
            foreach (string email in emailArry)
            {
                Entities.CustEmail model = new Entities.CustEmail();
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.CustID = custId;
                model.Email = email;
                BLL.CustEmail.Instance.Insert(model);
            }
        }
		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.CustEmail model)
		{
			return Dal.CustEmail.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ���ͻ��£�����������Ϣ
		/// </summary>
		public int Delete(string CustID)
		{
			
			return Dal.CustEmail.Instance.Delete(CustID);
		}

		#endregion

	}
}

