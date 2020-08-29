using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类Conversations 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class Conversations
    {
        #region Instance
        public static readonly Conversations Instance = new Conversations();
        #endregion

        #region Contructor
        protected Conversations()
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
        public DataTable GetConversations(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConversations(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 按照查询条件查询客服统计
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataSet GetConverStatistics(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConverStatistics(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable CheckConversationOrderCustInfo(string csid)
        {
            return Dal.Conversations.Instance.CheckConversationOrderCustInfo(csid);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.Conversations.Instance.GetConversations(new QueryConversations(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.Conversations GetConversations(int CSID)
        {

            return Dal.Conversations.Instance.GetConversations(CSID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByCSID(int CSID)
        {
            QueryConversations query = new QueryConversations();
            query.CSID = CSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetConversations(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Entities.Conversations GetConversationsByCsId(int CSID)
        {
            Entities.Conversations cs = null;
            QueryConversations query = new QueryConversations();
            query.CSID = CSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetConversations(query, string.Empty, 1, 1, out count);
            if (dt.Rows.Count > 0)
            {
                cs = new Entities.Conversations()
                {
                    CSID = CSID,
                    AgentStartTime = Convert.ToDateTime(dt.Rows[0]["AgentStartTime"]),
                    BGID = Convert.ToInt32(dt.Rows[0]["BGID"]),
                    CreateTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"]),
                    EndTime = Convert.ToDateTime(dt.Rows[0]["EndTime"]),
                    OrderID = dt.Rows[0]["OrderID"].ToString(),
                    LastClientTime = Convert.ToDateTime(dt.Rows[0]["LastClientTime"]),
                    Status = Convert.ToInt32(dt.Rows[0]["Status"]),
                    UserID = Convert.ToInt32(dt.Rows[0]["UserID"]),
                    UserName = dt.Rows[0]["UserName"].ToString(),
                    VisitID = Convert.ToInt32(dt.Rows[0]["VisitID"])
                };

            }
            return cs;
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Conversations model)
        {
            return Dal.Conversations.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.Conversations model)
        {
            return Dal.Conversations.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.Conversations model)
        {
            return Dal.Conversations.Instance.Update(model);
        }

        public int CallBackUpdate(Entities.Conversations model)
        {
            return Dal.Conversations.Instance.CallBackUpdate(model);
        }
        public int UpdateConversationReplyTime(DateTime dt, int csid)
        {
            return Dal.Conversations.Instance.UpdateConversationReplyTime(dt, csid);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.Conversations model)
        {
            return Dal.Conversations.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int CSID)
        {

            return Dal.Conversations.Instance.Delete(CSID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int CSID)
        {

            return Dal.Conversations.Instance.Delete(sqltran, CSID);
        }

        #endregion


        #region  毕帆  2014-10-31

        //<summary>
        //判断会话是否存在
        //</summary>
        //<param name="CSID">会话ID</param>
        //<returns></returns>
        public bool Exists(int CSID)
        {
            return Dal.Conversations.Instance.Exists(CSID);
        }
        /// <summary>
        /// 根据指定条件查询会话数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCSData(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int currentuserid = BLL.Util.GetLoginUserID();
            query.UserID = currentuserid;
            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(currentuserid);
            string bgids = Constant.STRING_INVALID_VALUE;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(bgids))
            {
                query.RightBGIDs = bgids.Substring(1);
            }
            string region = BLL.BaseData.Instance.GetAgentRegionByUserID(currentuserid.ToString());
            int intreatype;
            if (int.TryParse(region, out intreatype))
            {
                query.AreaType = intreatype;
            }
            return Dal.Conversations.Instance.GetCSData(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 根据VisitID获取客户信息
        /// exec p_GetMemberInfoByVisitID ' and VisitID=''001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetMemberInfoByVisitID(string strWhere)
        {
            return Dal.Conversations.Instance.GetMemberInfoByVisitID(strWhere);
        }
        /// <summary>
        /// 根据VisitID获取会话相关信息
        /// exec p_CSRelateInfoByVisitID ' and a.VisitID=''001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetCSRelateInfoByCSID(string strWhere)
        {
            return Dal.Conversations.Instance.GetCSRelateInfoByCSID(strWhere);
        }
        /// <summary>
        /// 根据OrderID查询工单相关信息
        /// exec p_GetWorkOrderInfoByOrderID ' and a.OrderID = ''WO20130000000001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoByOrderID(string strWhere)
        {
            return Dal.Conversations.Instance.GetWorkOrderInfoByOrderID(strWhere);
        }

        /// <summary>
        /// 根据指定条件查询历史聊天数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetConversationHistoryData(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int currentuserid = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(currentuserid);
            string bgids = Constant.STRING_INVALID_VALUE;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(bgids))
            {
                bgids = bgids.Substring(1);
            }
            string region = BLL.BaseData.Instance.GetAgentRegionByUserID(currentuserid.ToString());
            int intreatype;
            if (int.TryParse(region, out intreatype))
            {
                query.AreaType = intreatype;
            }

            //数据权限控制(当前登录人+当前登录人的管辖分组ids)
            if (bgids != Constant.STRING_INVALID_VALUE)
            {
                query.RightBGIDs = " AND (b.UserID = '" + currentuserid + "' OR e.BGID  IN (" + bgids + "))";
            }
            else
            {
                query.RightBGIDs = " AND b.UserID = '" + currentuserid + "'";
            }



            return Dal.Conversations.Instance.GetConversationHistoryData(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetConversationingCSData(string strWhere)
        {
            return Dal.Conversations.Instance.GetConversationingCSData(strWhere);
        }
        #endregion

        public DataTable ExportCSDataForLiaoTianJiLu(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(BLL.Util.GetLoginUserID());
            string bgids = Constant.STRING_INVALID_VALUE;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(bgids))
            {
                query.RightBGIDs = bgids.Substring(1);
            }

            return Dal.Conversations.Instance.ExportCSDataForLiaoTianJiLu(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetConversationHistoryDataForCC(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConversationHistoryDataForCC(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetConversationHistoryDataNew(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.GetConversationHistoryDataNew(query, order, currentPage, pageSize, out totalCount);
        }

    }
}

