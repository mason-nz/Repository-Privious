using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类OrderCRMStopCustTask。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-01 12:21:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderCRMStopCustTask : DataBase
    {
        public static readonly OrderCRMStopCustTask Instance = new OrderCRMStopCustTask();
        private const string P_ORDERCRMSTOPCUSTTASK_SELECT = "p_OrderCRMStopCustTask_Select";

        protected OrderCRMStopCustTask() { }

        /// 查询数据，强斐 2016-8-16
        /// <summary>
        /// 查询数据，强斐 2016-8-16
        /// </summary>
        /// <param name="taskIDs"></param>
        /// <returns></returns>
        public DataTable GetListByTaskIDs(string taskIDs)
        {
            string[] array = taskIDs.Split(',');
            string wherein = "'" + string.Join("','", array) + "'";
            string sqlStr = "SELECT * FROM OrderCRMStopCustTask WHERE TaskID in (" + Dal.Util.SqlFilterByInCondition(wherein) + ")";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        /// 得到一个对象实体 强斐 2016-8-16
        /// <summary>
        /// 得到一个对象实体 强斐 2016-8-16
        /// </summary>
        public OrderCRMStopCustTaskInfo GetOrderCRMStopCustTask(string taskID)
        {
            string sqlStr = "SELECT * FROM OrderCRMStopCustTask WHERE TaskID='" + taskID + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new OrderCRMStopCustTaskInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public OrderCRMStopCustTaskInfo GetEntityByRelationID(int relationID)
        {
            string sqlStr = "SELECT * FROM OrderCRMStopCustTask WHERE RelationID='" + relationID + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new OrderCRMStopCustTaskInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
    }
}

