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
    /// 业务逻辑类DealerInfo 的摘要说明。
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
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetDealerInfo(QueryDealerInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.DealerInfo.Instance.GetDealerInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.DealerInfo.Instance.GetDealerInfo(new QueryDealerInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.DealerInfo GetDealerInfo(string CustID)
        {

            return Dal.DealerInfo.Instance.GetDealerInfo(CustID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
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
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.DealerInfo model)
        {
            return Dal.DealerInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.DealerInfo model)
        {
            return Dal.DealerInfo.Instance.Update(model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
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
        /// 取车易通会员信息
        /// </summary>
        /// <param name="membername"></param>
        /// <returns></returns>
        public DataTable GetMemberInfo(string membername, string memberCode, string custId, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.DealerInfo.Instance.GetMemberInfo(membername, memberCode, custId, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 根据品牌id取品牌信息
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
        /// 根据品牌name取品牌信息
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

