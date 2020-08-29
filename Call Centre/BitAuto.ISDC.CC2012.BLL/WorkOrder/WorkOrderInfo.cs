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
    /// 业务逻辑类WorkOrderInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderInfo
    {
        #region Instance
        public static readonly WorkOrderInfo Instance = new WorkOrderInfo();
        #endregion

        #region Contructor
        protected WorkOrderInfo()
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
        public DataTable GetWorkOrderInfo(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfo(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetWorkOrderInfoForDemandInfo(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoForDemandInfo(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 按照查询条件查询(包含数据权限)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetWorkOrderInfoForList(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoForList(query, order, currentPage, pageSize, out totalCount, BLL.Loger.Log4Net);
        }
        /// <summary>
        /// 查询员工负责的客户下的工单
        /// </summary>
        /// <param name="query"></param>
        /// <param name="userId"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoByUserID(QueryWorkOrderInfo query, int userId, string departmentId, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoByUserID(query, userId, departmentId, order, currentPage, pageSize, out totalCount, BLL.Loger.Log4Net);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoForExport(QueryWorkOrderInfo query, int workCategory, int userId, string order)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfoForExport(query, workCategory, userId, order);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfo(new QueryWorkOrderInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderInfo GetWorkOrderInfo(string OrderID)
        {

            return Dal.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByOrderID(string OrderID)
        {
            QueryWorkOrderInfo query = new QueryWorkOrderInfo();
            query.OrderID = OrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderInfo(query, string.Empty, 1, 1, out count);
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
        public string Insert(Entities.WorkOrderInfo model)
        {
            //工单表增加两个字段，BGID和SCID 为了减少修改地方直接在Insert方法中添加 lxw 13-10-11
            int loginID = Util.GetLoginUserID();
            model.BGID = SurveyCategory.Instance.GetSelfBGIDByUserID(loginID);
            model.SCID = SurveyCategory.Instance.GetSelfSCIDByUserID(loginID);

            return Dal.WorkOrderInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertWorkOrderInfo(Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Insert(SqlTransaction sqltran, Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderInfo model)
        {
            return Dal.WorkOrderInfo.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID)
        {

            return Dal.WorkOrderInfo.Instance.Delete(OrderID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {

            return Dal.WorkOrderInfo.Instance.Delete(sqltran, OrderID);
        }

        #endregion

        /// <summary>
        /// 根据 表获取总数量
        /// </summary> 
        /// <returns></returns>
        public int GetMax()
        {
            return Dal.WorkOrderInfo.Instance.GetMax();
        }

        public DataTable GetProcessOrderUserID(string orderID)
        {
            return Dal.WorkOrderInfo.Instance.GetProcessOrderUserID(orderID);
        }

        /// <summary>
        /// 获取所有工单的创建人
        /// </summary>
        /// <returns></returns>
        public DataTable GetCreateUser(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.GetCreateUser(where, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 根据工单ID取客户ID
        /// </summary>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>
        public string GetCustIDByWorkOrderTel(Entities.WorkOrderInfo model)
        {
            if (model == null) return "";
            return Dal.WorkOrderInfo.Instance.GetCustIDByWorkOrderID(model.OrderID);
            //return BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(model.ContactTel);
        }

        /// <summary>
        /// 查询某部门及其子部门
        /// </summary>
        /// <param name="departID"></param>
        /// <returns></returns>
        public DataTable GetChildDepartMent(string[] DepartIDs)
        {
            return Dal.WorkOrderInfo.Instance.GetChildDepartMent(DepartIDs);
        }

        /// <summary>
        /// 根据工单ID获取录音URL
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderRecordUrl_OrderID(string orderid)
        {
            return Dal.WorkOrderInfo.Instance.GetWorkOrderRecordUrl_OrderID(orderid);
        }

        /// <summary>
        /// 惠买车工单数据
        /// </summary>
        /// <param name="query">WorkCategory(1:个人2:经销商)</param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable WorkOrderInfoExportHMC(QueryWorkOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderInfo.Instance.WorkOrderInfoExportHMC(query, order, currentPage, pageSize, out totalCount);
        }

        public bool HasConversation(string OrderID)
        {
            return Dal.WorkOrderInfo.Instance.HasConversation(OrderID);
        }
    }
}

