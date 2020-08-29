using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类EmployeeAgent 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:02 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class EmployeeAgent
    {
        #region Instance
        public static readonly EmployeeAgent Instance = new EmployeeAgent();
        #endregion

        #region Contructor
        protected EmployeeAgent()
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
        public DataTable GetEmployeeAgent(QueryEmployeeAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(query, order, currentPage, pageSize, out totalCount);
        }
        public string GetAgentNum(string strUserId)
        {
            return Dal.EmployeeAgent.Instance.GetAgentNum(strUserId);
        }
        /// <summary>
        /// 根据客户UserID查询最大排队数，会话数
        /// </summary>
        /// <param name="userid">客服UserID</param>
        /// <param name="MaxQueue">客服最大同时排队量</param>
        /// <param name="MaxDialogueN">客服最大同时会话量</param>
        /// <returns>true:客服表配置参数，false:缺省配置</returns>
        public bool GetMaxQueueDialogue(int userid, out int MaxQueue, out int MaxDialogueN)
        {
            MaxQueue = 0;
            MaxDialogueN = 0;
            string sMaxQueue = "", sMaxDialogueN = "";
            sMaxQueue = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("MaxQueue");
            sMaxDialogueN = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("MaxDialogueN");

            MaxQueue = Convert.ToInt32(sMaxQueue);
            MaxDialogueN = Convert.ToInt32(sMaxDialogueN);
            QueryEmployeeAgent model = new QueryEmployeeAgent();
            model.UserID = userid;
            int itemp = 0;
            DataTable dt = Dal.EmployeeAgent.Instance.GetEmployeeAgent(model, "", 1, 100, out itemp);
            if (dt == null)
            {
                return false;
            }
            if (int.TryParse(dt.Rows[0]["MaxQueue"].ToString(), out itemp))
            {
                MaxQueue = itemp;
            }

            if (int.TryParse(dt.Rows[0]["MaxDialogueN"].ToString(), out itemp))
            {
                MaxDialogueN = itemp;
            }

            return true;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(new QueryEmployeeAgent(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.EmployeeAgent GetEmployeeAgent(int RecID)
        {

            return Dal.EmployeeAgent.Instance.GetEmployeeAgent(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryEmployeeAgent query = new QueryEmployeeAgent();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetEmployeeAgent(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.EmployeeAgent model)
        {
            return Dal.EmployeeAgent.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.EmployeeAgent.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.EmployeeAgent.Instance.Delete(sqltran, RecID);
        }

        #endregion

    }
}

