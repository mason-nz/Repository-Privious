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

        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        //public int Update(SqlTransaction sqltran, Entities.Conversations model)
        //{
        //    return Dal.Conversations.Instance.Update(sqltran, model);
        //}

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
                query.RightBGIDs = " AND (b.UserID = '" + currentuserid + "' OR e.BGID  IN (" + StringHelper.SqlFilter(bgids) + "))";
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

        public DataSet ExportCSDataForLiaoTianJiLu(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
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

        public void UpdateConversationTag(string strCsid, string strTagId, string strTagName)
        {
            Dal.Conversations.Instance.UpdateConversationTag(strCsid, strTagId, strTagName);
        }
        public DataTable GetConversationTagData(int BGID)
        {
            return Dal.Conversations.Instance.GetConversationTagData(BGID);
        }

        public void GetChildNode(DataTable dt, string NodeID, ref string leafids)
        {

            DataRow[] rowsArr1 = dt.Select("PID=" + NodeID);
            if (rowsArr1 == null || rowsArr1.Length == 0)
            {
                leafids += "," + NodeID;
            }
            else
            {
                foreach (DataRow row in rowsArr1)
                {
                    DataRow[] rowsArr2 = dt.Select("PID=" + row["RecID"].ToString());
                    if (rowsArr2 == null || rowsArr2.Length == 0)
                    {
                        leafids += "," + row["RecID"].ToString();
                    }
                    else
                    {
                        GetChildNode(dt, row["RecID"].ToString(), ref leafids);
                    }
                }
            }
        }
        /// <summary>
        /// 取客服统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns>日统计返回：（daytime：日期），周统计返回（weekb:周开始日期，weeke：周结束日期），月统计返回：（monthb:月开始日期，monthe:月结束日期），（其他：userid，truename：姓名，agentnum：工号，sumonlinetime：总在线时长，SumConversations：总对话量，SumConversationTimeLong：总对话时长，SumFRTimeLong：总的首次回复时长，SumReception：总的接待量，SumAgentDailog：坐席发消息总量，SumNetFriendDailog：网友发消息总量</returns>
        public DataTable S_Agent_Total_Select(QueryUserSatisfactionTotal query, string order, int currentPage, int pageSize, out int totalCount, int logUserID)
        {
            return Dal.Conversations.Instance.S_Agent_Total_Select(query, order, currentPage, pageSize, out totalCount, logUserID);
        }

        /// <summary>
        /// 取客服统计汇总数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns>sumonlinetime：总在线时长，SumConversations：总对话量，SumConversationTimeLong：总对话时长，SumFRTimeLong：总的首次回复时长，SumReception：总的接待量，SumAgentDailog：坐席发消息总量，SumNetFriendDailog：网友发消息总量</returns>
        public DataTable S_Agent_Total_Select(QueryUserSatisfactionTotal query, int logUserID)
        {
            return Dal.Conversations.Instance.S_Agent_Total_Select(query, logUserID);
        }


        /// <summary>
        /// 业务线汇总数据
        /// </summary>
        /// <param name="sourcetype">业务线标识，传-1为所有业务线，如果选择易车汇总，不选二级，则传100</param>
        /// <param name="selectType">1日，2周，3月，4小时</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>日统计返回：（daytime：日期），周统计返回（weekb:周开始日期，weeke：周结束日期），月统计返回：（monthb:月开始日期，monthe:月结束日期）其他：SourceType:业务线标识（通过bll.util下方法GetSourceTypeName取名称）,SumConversation：总对话量,SumEffective:有效对话量,SumNoEffective:无效对话量）</returns>
        public DataTable S_BussinessLine_Total_Select(QueryBussinessLineTotal query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query, order, currentPage, pageSize, out totalCount);

        }

        /// <summary>
        /// 业务线汇总数据合计
        /// </summary>
        /// <param name="sourcetype">业务线标识，传-1为所有业务线，如果选择易车汇总，不选二级，则传100，如果选择二级，则只传二级</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>SumConversation：总对话量,SumEffective:有效对话量,SumNoEffective:无效对话量</returns>
        public DataTable S_BussinessLine_Total_Select(QueryBussinessLineTotal query)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query);
        }


        /// <summary>
        /// 业务线流量统计数据
        /// </summary>
        /// <param name="sourcetype">业务线标识，传-1为所有业务线，如果选择易车汇总，不选二级，则传100</param>
        /// <param name="selectType">1日，2周，3月，4小时</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>日统计返回：（daytime：日期），周统计返回：（weekb:周开始日期，weeke：周结束日期），月统计返回：（monthb:月开始日期，monthe:月结束日期），小时统计返回：（hourtime：小时，格式0,1,2,。。。,hourtimename：小时名称，格式2015-09-09 17时至18时），其他：SourceType:业务线标识（通过bll.util下方法GetSourceTypeName取名称）,SumVisit:页面访问量,SumConversation：总对话量,SumQueueFail:队列放弃量,LoginVisit:登录访客总量，NoLoginVisit：匿名访客总量）</returns>
        public DataTable S_Flow_Total_Select(QueryBussinessLineTotal query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query, order, currentPage, pageSize, out totalCount);

        }

        /// <summary>
        /// 业务线流量统计数据合计
        /// </summary>
        /// <param name="sourcetype">业务线标识，传-1为所有业务线，如果选择易车汇总，不选二级，则传100，如果选择二级，则只传二级</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>SumVisit:页面访问量,SumConversation：总对话量,SumQueueFail:队列放弃量,LoginVisit:登录访客总量，NoLoginVisit：匿名访客总量</returns>
        public DataTable S_Flow_Total_Select(QueryBussinessLineTotal query)
        {
            return Dal.Conversations.Instance.S_BussinessLine_Total_Select(query);
        }

        /// <summary>
        /// 业务线流量汇总数据
        /// </summary>
        /// <param name="sourcetype">业务线标识，传-1为所有业务线，如果选择易车汇总，不选二级，则传100</param>
        /// <param name="selectType">1日，2周，3月，4小时</param>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>日统计返回：（daytime：日期），周统计返回：（weekb:周开始日期，weeke：周结束日期），月统计返回：（monthb:月开始日期，monthe:月结束日期），小时统计返回：（hourtime：小时，格式0,1,2,。。。,hourtimename：小时名称，格式2015-09-09 17时至18时），其他：SourceType:业务线标识（通过bll.util下方法GetSourceTypeName取名称）,SumVisit:页面访问量,SumConversation：总对话量,SumQueueFail:队列放弃量,LoginVisit:登录访客总量，NoLoginVisit：匿名访客总量</returns>
        public DataTable S_FlowForMap_Select(QueryBussinessLineTotal query)
        {

            return Dal.Conversations.Instance.S_FlowForMap_Select(query);
        }

        /// <summary>
        /// 今日概况业务线汇总数据，开始时间，结束时间传今天，selecttype传1，currentPage传1，pagesize传100000
        /// </summary>
        /// <returns>SourceType:业务线标识（通过bll.util下方法GetSourceTypeName取名称）,SumVisit:页面访问量,SumConversation：总对话量,SumQueueFail:队列放弃量,SumReception:接待量，LeaveMessage：留言量，对话中的访客量，以及排队中访客量，这两个字段通过页面绑定后台方法，只能通过web层调用</returns>
        public DataTable Today_Total_Select(QueryBussinessLineTotal query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Conversations.Instance.Today_Total_Select(query, order, currentPage, pageSize, out totalCount);
        }


        /// <summary>
        /// 今日概况业务线汇总数据合计，开始时间，结束时间传今天，selecttype传1，currentPage传1，pagesize传100000
        /// </summary>
        /// <returns>SumVisit:页面访问量,SumConversation：总对话量,SumQueueFail:队列放弃量,SumReception:接待量，LeaveMessage：留言量，对话中的访客量，以及排队中访客量，这两个字段通过页面绑定后台方法，只能通过web层调用</returns>
        public DataTable Today_Total_Select(QueryBussinessLineTotal query)
        {
            return Dal.Conversations.Instance.Today_Total_Select(query);

        }
    }
}

