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
    public class WorkOrderLog
    {
        #region Instance
        public static readonly WorkOrderLog Instance = new WorkOrderLog();
        #endregion

        #region Contructor
        protected WorkOrderLog()
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
        public DataTable GetWorkOrderLog(QueryWorkOrderLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WorkOrderLog.Instance.GetWorkOrderLog(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.WorkOrderLog.Instance.GetWorkOrderLog(new QueryWorkOrderLog(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderLog GetWorkOrderLog(int RecID)
        {

            return Dal.WorkOrderLog.Instance.GetWorkOrderLog(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryWorkOrderLog query = new QueryWorkOrderLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderLog(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.WorkOrderLog model)
        {
            return Dal.WorkOrderLog.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderLog model)
        {
            return Dal.WorkOrderLog.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderLog model)
        {
            return Dal.WorkOrderLog.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderLog model)
        {
            return Dal.WorkOrderLog.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.WorkOrderLog.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.WorkOrderLog.Instance.Delete(sqltran, RecID);
        }

        #endregion

        #region SelectByOrderID
        /// <summary>
        /// 根据工单号查询用到的标签
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderLogByOrderID(string orderid)
        {
            return Dal.WorkOrderLog.Instance.GetWorkOrderLogByOrderID(orderid);
        }

        public string GetWorkOrderLogNames(string orderid)
        {
            string tagNames = string.Empty;

            DataTable dtTag = Dal.WorkOrderLog.Instance.GetWorkOrderLogByOrderID(orderid);
            for (int i = 0; i < dtTag.Rows.Count; i++)
            {
                tagNames += dtTag.Rows[i]["TagName"].ToString() + ",";
            }

            return tagNames.TrimEnd(' ');
        }

        #endregion



        internal string GetLogDesc(Dictionary<string, string> logDescDic)
        {
            StringBuilder logDesc = new StringBuilder();
            logDesc.Append("{");

            int i = 0;
            foreach (var item in logDescDic)
            {
                if ((++i) == logDescDic.Count)
                {
                    //最后一个
                    logDesc.Append("\"" + item.Key + "\":\"" + item.Value + "\"");
                }
                else
                {
                    logDesc.Append("\"" + item.Key + "\":\"" + item.Value + "\",");
                }
            }
         
            logDesc.Append("}");
            return logDesc.ToString();
        }

        internal Entities.WorkOrderLog GetWorkOrderLogByReceiverRecID(int ReceiverRecID)
        {
            Entities.WorkOrderLog model = new Entities.WorkOrderLog();
            Entities.QueryWorkOrderLog query = new QueryWorkOrderLog();
            query.ReceiverRecID = ReceiverRecID;

            int totalCount = 0;
            DataTable dt = BLL.WorkOrderLog.Instance.GetWorkOrderLog(query, "", 1, 99999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                model = Dal.WorkOrderLog.Instance.LoadSingleWorkOrderLog(dt.Rows[0]);
            }
            else
            {
                model = null;
            }

            return model;
        }
    }
}
