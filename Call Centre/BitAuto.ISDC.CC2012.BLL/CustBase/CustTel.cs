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
	/// ҵ���߼���CustTel ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:15 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class CustTel
	{
		#region Instance
		public static readonly CustTel Instance = new CustTel();
		#endregion

		#region Contructor
		protected CustTel()
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
		public DataTable GetCustTel(QueryCustTel query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.CustTel.Instance.GetCustTel(query,order,currentPage,pageSize,out totalCount);
		}

        /// <summary>
        /// ��ȡ�ͻ��µ����е绰
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable GetCustTel(string custId)
        {
            return Dal.CustTel.Instance.GetCustTel(custId);
        }
		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.CustTel.Instance.GetCustTel(new QueryCustTel(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.CustTel GetCustTel(string CustID,string Tel)
		{
			return Dal.CustTel.Instance.GetCustTel(CustID,Tel);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByCustIDAndTel(string CustID,string Tel)
		{
			QueryCustTel query = new QueryCustTel();
			query.CustID = CustID;
			query.Tel = Tel;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetCustTel(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.CustTel model)
		{
			Dal.CustTel.Instance.Insert(model);
		}

	/// <summary>
		/// ����һ������
		/// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CustTel model)
		{
            Dal.CustTel.Instance.Insert(sqltran,model);
		}

        /// <summary>
        /// ��������绰
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="tels"></param>
        public void Insert(string custId, string tels)
        {
            string[] telArry = tels.Split(',');
            foreach (string tel in telArry)
            {
                Entities.CustTel model = new Entities.CustTel();
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.CustID = custId;
                model.Tel = tel;
                BLL.CustTel.Instance.Insert(model);
            }
        }
		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
		public int Update(Entities.CustTel model)
		{
			return Dal.CustTel.Instance.Update(model);
		}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(string CustID)
		{
			
			return Dal.CustTel.Instance.Delete(CustID);
		}

		#endregion

	}
}

