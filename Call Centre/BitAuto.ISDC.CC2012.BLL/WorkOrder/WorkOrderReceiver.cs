using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.BLL.WorkOrder;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class WorkOrderReceiver
    {
        #region Instance
        public static readonly WorkOrderReceiver Instance = new WorkOrderReceiver();
        #endregion

        #region Contructor
        protected WorkOrderReceiver()
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
        public DataTable GetWorkOrderReceiver(QueryWorkOrderReceiver query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderReceiver.Instance.GetWorkOrderReceiver(query, order, currentPage, pageSize, out totalCount);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderReceiver GetWorkOrderReceiver(int RecID)
        {
            return Dal.WorkOrderReceiver.Instance.GetWorkOrderReceiver(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryWorkOrderReceiver query = new QueryWorkOrderReceiver();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderReceiver(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.WorkOrderReceiver model)
        {
            return Dal.WorkOrderReceiver.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderReceiver model)
        {
            return Dal.WorkOrderReceiver.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderReceiver model)
        {
            return Dal.WorkOrderReceiver.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderReceiver model)
        {
            return Dal.WorkOrderReceiver.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.WorkOrderReceiver.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.WorkOrderReceiver.Instance.Delete(sqltran, RecID);
        }

        #endregion

        /// 同步话务数据到工单回复表
        /// <summary>
        /// 同步话务数据到工单回复表
        /// </summary>
        /// <returns></returns>
        public int SyncCallDataForService()
        {
            return Dal.WorkOrderReceiver.Instance.SyncCallDataForService();
        }
    }
}
