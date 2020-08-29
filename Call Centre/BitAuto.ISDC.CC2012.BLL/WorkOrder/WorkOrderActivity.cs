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
    /// 业务逻辑类WorkOrderActivity 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-01-13 04:24:36 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderActivity
    {
        #region Instance
        public static readonly WorkOrderActivity Instance = new WorkOrderActivity();
        #endregion

        #region Contructor
        protected WorkOrderActivity()
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
        public DataTable GetWorkOrderActivity(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderActivity.Instance.GetWorkOrderActivity(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.WorkOrderActivity.Instance.GetWorkOrderActivity(string.Empty, string.Empty, 1, 1000000, out totalCount);
        }
        //根据工单ID返回Guids串
        public string GetGuidsByOrderID(string orderID)
        {
            string guids = string.Empty;
            int count = 0;
            DataTable dt_Activity = BLL.WorkOrderActivity.Instance.GetWorkOrderActivity(" and orderID='" + orderID+"'", "", 1, 1000, out count);
            for (int i = 0; i < dt_Activity.Rows.Count; i++)
            {
                guids += dt_Activity.Rows[i]["ActivityGUID"].ToString() + ",";
            }
            return guids.TrimEnd(',');
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderActivity GetWorkOrderActivity(string OrderID, Guid ActivityGUID)
        {

            return Dal.WorkOrderActivity.Instance.GetWorkOrderActivity(OrderID, ActivityGUID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByOrderIDAndActivityGUID(string OrderID, Guid ActivityGUID)
        {
            //QueryWorkOrderActivity query = new QueryWorkOrderActivity();
            //query.OrderID = OrderID;
            //query.ActivityGUID = ActivityGUID;
            string query = string.Format(" And OrderID='{0}' And ActivityGUID='{1}'",
                StringHelper.SqlFilter(OrderID), StringHelper.SqlFilter(ActivityGUID.ToString()));
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderActivity(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.WorkOrderActivity model)
        {
            Dal.WorkOrderActivity.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.WorkOrderActivity model)
        {
            Dal.WorkOrderActivity.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        //public int Update(Entities.WorkOrderActivity model)
        //{
        //    return Dal.WorkOrderActivity.Instance.Update(model);
        //}

        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        //public int Update(SqlTransaction sqltran, Entities.WorkOrderActivity model)
        //{
        //    return Dal.WorkOrderActivity.Instance.Update(sqltran, model);
        //}

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID, Guid ActivityGUID)
        {

            return Dal.WorkOrderActivity.Instance.Delete(OrderID, ActivityGUID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID, Guid ActivityGUID)
        {

            return Dal.WorkOrderActivity.Instance.Delete(sqltran, OrderID, ActivityGUID);
        }

        #endregion

    }
}

