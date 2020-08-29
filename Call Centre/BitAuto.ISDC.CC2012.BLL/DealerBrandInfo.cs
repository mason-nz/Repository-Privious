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
	/// ҵ���߼���DealerBrandInfo ��ժҪ˵����
	/// </summary>
	/// <author>
	/// masj
	/// </author>
	/// <history>
	/// 2012-07-27 10:30:16 Created
	/// </history>
	/// <version>
	/// 1.0
	/// </version>
	//----------------------------------------------------------------------------------
	public class DealerBrandInfo
	{
		#region Instance
		public static readonly DealerBrandInfo Instance = new DealerBrandInfo();
		#endregion

		#region Contructor
		protected DealerBrandInfo()
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
		public DataTable GetDealerBrandInfo(QueryDealerBrandInfo query, string order, int currentPage, int pageSize, out int totalCount)
		{
			return Dal.DealerBrandInfo.Instance.GetDealerBrandInfo(query,order,currentPage,pageSize,out totalCount);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataTable GetAllList()
		{
			int totalCount=0;
			return Dal.DealerBrandInfo.Instance.GetDealerBrandInfo(new QueryDealerBrandInfo(),string.Empty,1,1000000,out totalCount);
		}

		#endregion

		#region GetModel
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Entities.DealerBrandInfo GetDealerBrandInfo(string CustID,int DealerID,int BrandID)
		{
			
			return Dal.DealerBrandInfo.Instance.GetDealerBrandInfo(CustID,DealerID,BrandID);
		}

		#endregion

		#region IsExists
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool IsExistsByCustIDAndDealerIDAndBrandID(string CustID,int DealerID,int BrandID)
		{
			QueryDealerBrandInfo query = new QueryDealerBrandInfo();
			query.CustID = CustID;
			query.DealerID = DealerID;
			query.BrandID = BrandID;
			DataTable dt = new DataTable();
			int count = 0;
			dt = GetDealerBrandInfo(query, string.Empty, 1, 1, out count);
			if (count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


        public bool IsExistsByCustIDAndDealerIDAndBrandID(string CustID)
        {
            QueryDealerBrandInfo query = new QueryDealerBrandInfo();
            query.CustID = CustID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetDealerBrandInfo(query, string.Empty, 1, 1, out count);
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
		public void Insert(Entities.DealerBrandInfo model)
		{
			Dal.DealerBrandInfo.Instance.Insert(model);
		}

		#endregion

		#region Update
		/// <summary>
		/// ����һ������
		/// </summary>
        //public int Update(Entities.DealerBrandInfo model)
        //{
        //    return Dal.DealerBrandInfo.Instance.Update(model);
        //}

		#endregion

		#region Delete
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public int Delete(string CustID)
		{
			
			return Dal.DealerBrandInfo.Instance.Delete(CustID);
		}

		#endregion

	}
}

