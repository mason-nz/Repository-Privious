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
    /// ҵ���߼���DealerInfo ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class DealerInfo
    {
        #region Instance
        public static readonly DealerInfo Instance = new DealerInfo();
        #endregion

        #region Contructor
        protected DealerInfo()
        { }
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
        public DataTable GetDealerInfo(QueryDealerInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.DealerInfo.Instance.GetDealerInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.DealerInfo.Instance.GetDealerInfo(new QueryDealerInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.DealerInfo GetDealerInfo(string CustID)
        {

            return Dal.DealerInfo.Instance.GetDealerInfo(CustID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryDealerInfo query = new QueryDealerInfo();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetDealerInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsExistsByCustID(string CustID)
        {
            QueryDealerInfo query = new QueryDealerInfo();
            query.CustID = CustID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetDealerInfo(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.DealerInfo model)
        {
            return Dal.DealerInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.DealerInfo model)
        {
            return Dal.DealerInfo.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int DeleteForSetStatus(string CustID)
        {
            return Dal.DealerInfo.Instance.DeleteForSetStatus(CustID);
        }
        public void Delete(string cbid)
        {
            Dal.DealerInfo.Instance.Delete(cbid);
        }
        #endregion
        /// <summary>
        /// ȡ����ͨ��Ա��Ϣ
        /// </summary>
        /// <param name="membername"></param>
        /// <returns></returns>
        public DataTable GetMemberInfo(string membername, string memberCode, string custId, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.DealerInfo.Instance.GetMemberInfo(membername, memberCode, custId, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ����Ʒ��idȡƷ����Ϣ
        /// </summary>
        /// <param name="BrandIDs"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBrandInfo(string BrandIDs, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.DealerInfo.Instance.GetBrandInfo(BrandIDs, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ����Ʒ��nameȡƷ����Ϣ
        /// </summary>
        /// <param name="BrandIDs"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBrandInfoByName(string brandname, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.DealerInfo.Instance.GetBrandInfoByName(brandname, order, currentPage, pageSize, out totalCount);
        }
    }
}

