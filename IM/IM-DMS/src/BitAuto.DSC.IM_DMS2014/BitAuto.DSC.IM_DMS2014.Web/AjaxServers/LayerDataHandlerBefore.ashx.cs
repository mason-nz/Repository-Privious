using System;
using System.Collections.Generic;
using System.Web;
using BitAuto.DSC.IM_DMS2014.Entities;
using System.Data;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;
using System.Web.SessionState;
using Newtonsoft.Json;
using BitAuto.DSC.IM_DMS2014.Core;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers
{
    /// <summary>
    /// LayerDataHandlerBefore 的摘要说明
    /// </summary>
    public class LayerDataHandlerBefore : IHttpHandler, IRequiresSessionState
    {

        private string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }

        #region 满意度
        //会话ID
        private string CSID
        {
            get
            {
                return HttpContext.Current.Request["CSID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CSID"].ToString());
            }
        }
        //服务满意度
        private string PerSatisfaction
        {
            get
            {
                return HttpContext.Current.Request["PerSatisfaction"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["PerSatisfaction"].ToString());
            }
        }
        //产品满意度
        private string ProSatisfaction
        {
            get
            {
                return HttpContext.Current.Request["ProSatisfaction"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ProSatisfaction"].ToString());
            }
        }
        //满意度明细信息
        private string SatisfactionDetails
        {
            get
            {
                return HttpContext.Current.Request["DSatisfaction"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["DSatisfaction"].ToString());
            }
        }
        #endregion

        #region 城市数据  District
        private string District
        {
            get
            {
                return HttpContext.Current.Request["District"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["District"].ToString());
            }
        }
        #endregion

        #region 客户信息
        private string VisitID
        {
            get
            {
                return HttpContext.Current.Request["VisitID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["VisitID"].ToString());
            }
        }
        #endregion

        #region 工单信息
        private string OrderID
        {
            get
            {
                return HttpContext.Current.Request["OrderID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["OrderID"].ToString());
            }
        }
        #endregion

        #region 表情信息
        private string ECategory
        {
            get
            {
                return HttpContext.Current.Request["ECategory"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ECategory"].ToString());
            }
        }
        #endregion

        #region 留言信息
        private string Meesage_Type
        {
            get
            {
                return HttpContext.Current.Request["Meesage_Type"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Meesage_Type"].ToString());
            }
        }
        private string Meesage_Contents
        {
            get
            {
                return HttpContext.Current.Request["Meesage_Contents"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Meesage_Contents"].ToString());
            }
        }
        private string Meesage_Name
        {
            get
            {
                return HttpContext.Current.Request["Meesage_Name"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Meesage_Name"].ToString());
            }
        }
        private string Meesage_Phone
        {
            get
            {
                return HttpContext.Current.Request["Meesage_Phone"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Meesage_Phone"].ToString());
            }
        }
        private string Meesage_Email
        {
            get
            {
                return HttpContext.Current.Request["Meesage_Email"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Meesage_Email"].ToString());
            }
        }
        #endregion


        #region 更新留言信息
        private string RemarkRecID
        {
            get
            {
                return HttpContext.Current.Request["RemarkRecID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["RemarkRecID"].ToString());
            }
        }
        private string RemarkDetails
        {
            get
            {
                return HttpContext.Current.Request["RemarkDetails"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["RemarkDetails"].ToString());
            }
        }
        private string MessageOrderID
        {
            get
            {
                return HttpContext.Current.Request["RemarkOrderID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["RemarkOrderID"].ToString());
            }
        }
        private string MessageStatus
        {
            get
            {
                return HttpContext.Current.Request["MessageStatus"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MessageStatus"].ToString());
            }
        }
        #endregion
        private string QueryStarttime
        {
            get
            {
                return HttpContext.Current.Request["QueryStarttime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStarttime"].ToString());
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string msg = "";

            CheckParams(out msg);

            if (msg == "")
            {
                switch (Action)
                {
                    case "addsatisfaction":
                        AddSatisfaction(out msg);
                        break;
                    case "getemotioninfobyecategory":
                        GetEmotionInfoByECategory(out msg);
                        break;
                    case "addonlinemessage":
                        AddOnlineMessage(out msg);
                        break;
                }
            }
            context.Response.Write(msg);
        }
        private void GetNewMessagesData(out string msg)
        {
            msg = string.Empty;

            int RecordCount;
            QueryConversations query = new QueryConversations();
            query.CSID = CSID == "" ? Constant.INT_INVALID_VALUE : int.Parse(CSID);
            query.QueryStarttime = QueryStarttime == "" ? Constant.DATE_INVALID_VALUE : Convert.ToDateTime(QueryStarttime);

            DataTable dt = BLL.Conversations.Instance.GetConversationHistoryData(query, "c.CreateTime asc", 1, 10000, out RecordCount);

            DataTable newTable = new DataTable();
            DataColumn col1 = new DataColumn("newName", typeof(string));
            DataColumn col2 = new DataColumn("Content", typeof(string));
            DataColumn col3 = new DataColumn("lastdate", typeof(string));
            newTable.Columns.Add(col1);
            newTable.Columns.Add(col2);
            newTable.Columns.Add(col3);

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = newTable.NewRow();

                switch (row["Sender"].ToString())
                {
                    case "1": newRow["newName"] = "客服" + row["AgentNum"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                    case "2": newRow["newName"] = row["MemberName"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                    default: newRow["newName"] = "系统消息    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                }
                newRow["Content"] = row["Content"];
                newRow["lastdate"] = row["newcreatetime"] == null ? "" : Convert.ToDateTime(row["newcreatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss.fff");
                newTable.Rows.Add(newRow);
            }

            msg = JavaScriptConvert.SerializeObject(newTable, new DataTableConverter());

        }
        private void AddRemarkInfo(out string msg)
        {
            QueryUserMessage query = new QueryUserMessage();
            query.RecID = RemarkRecID == "" ? Constant.INT_INVALID_VALUE : int.Parse(RemarkRecID);
            query.Remarks = RemarkDetails == "" ? Constant.STRING_INVALID_VALUE : RemarkDetails;
            if (query.Remarks != Constant.STRING_INVALID_VALUE)
            {
                query.RemarksTime = DateTime.Now;
                query.RemarkUserID = BLL.Util.GetLoginUserID();
            }

            query.OrderID = MessageOrderID == "" ? Constant.STRING_INVALID_VALUE : MessageOrderID;

            query.Status = MessageStatus == "" ? Constant.INT_INVALID_VALUE : int.Parse(MessageStatus);
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                query.LastModifyTime = DateTime.Now;
                query.LastModifyUserID = BLL.Util.GetLoginUserID();
            }

            int num = BLL.UserMessage.Instance.UpdateUserMessageInfoByRecID(query);
            if (num > 0)
            {
                msg = "success";
            }
            else
            {
                msg = "留言提交失败，请稍后再试！";
            }

        }

        private void AddOnlineMessage(out string msg)
        {
            UserMessage model = new UserMessage();
            model.VisitID = VisitID;
            model.TypeID = Meesage_Type == "" ? -1 : int.Parse(Meesage_Type);
            model.Content = Meesage_Contents;
            model.UserName = Meesage_Name;
            model.Email = Meesage_Email;
            model.Phone = Meesage_Phone;
            model.CreateTime = DateTime.Now;

            int num = BLL.UserMessage.Instance.InsertUserMessage(model);
            if (num > 0)
            {
                msg = "success";
            }
            else
            {
                msg = "留言提交失败，请稍后再试！";
            }

        }

        private void GetEmotionInfoByECategory(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BLL.Emotions.Instance.GetEmotionInfoByECategory(" and ECategory='" + StringHelper.SqlFilter(ECategory) + "'");
            if (dt.Rows.Count > 0)
            {
                msg += "{root:[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                msg += "{EUrl:'" + dt.Rows[i]["EUrl"].ToString() + "',EText:'" + dt.Rows[i]["EText"].ToString() + "'},";
            }
            if (dt.Rows.Count > 0)
            {
                msg = msg.TrimEnd(',') + "]}";
            }
        }

        private void GetOrderInfoByOrderID(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BLL.Conversations.Instance.GetWorkOrderInfoByOrderID(" and a.OrderID='" + StringHelper.SqlFilter(OrderID) + "'");
            if (dt.Rows.Count > 0)
            {
                msg += "{root:[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strstatus = string.Empty;
                if (dt.Rows[i]["Status"] == null)
                {
                    strstatus = "";
                }
                else
                {
                    switch (dt.Rows[i]["Status"].ToString())
                    {
                        case "0": strstatus = "可用"; break;
                        case "1": strstatus = "待审核"; break;
                        case "2": strstatus = "待处理"; break;
                        case "3": strstatus = "处理中"; break;
                        case "4": strstatus = "已处理"; break;
                        case "5": strstatus = "已完成"; break;
                        case "6": strstatus = "已关闭"; break;
                        default: strstatus = ""; break;
                    }
                }
                msg += "{OrderID:'" + (dt.Rows[i]["OrderID"] == null ? "" : dt.Rows[i]["OrderID"].ToString()) + "'"
                     + ",Title:'" + (dt.Rows[i]["Title"] == null ? "" : dt.Rows[i]["Title"].ToString()) + "'"
                     + ",CreateTime:'" + (dt.Rows[i]["CreateTime"] == null ? "" : Convert.ToDateTime(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")) + "'"
                     + ",TrueName:'" + (dt.Rows[i]["TrueName"] == null ? "0" : dt.Rows[i]["TrueName"].ToString()) + "'"
                    + ",Status:'" + strstatus + "'},";
            }
            if (dt.Rows.Count > 0)
            {
                msg = msg.TrimEnd(',') + "]}";
            }
        }

        private void GetCSRelateInfoByCSID(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BLL.Conversations.Instance.GetCSRelateInfoByCSID(" and a.CSID='" + StringHelper.SqlFilter(CSID) + "'");
            if (dt.Rows.Count > 0)
            {
                msg += "{root:[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += "{CreateTime:'" + (dt.Rows[i]["CreateTime"] == null ? "" : Convert.ToDateTime(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")) + "'"
                     + ",AgentStartTime:'" + (dt.Rows[i]["AgentStartTime"] == null ? "" : Convert.ToDateTime(dt.Rows[i]["AgentStartTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")) + "'"
                     + ",EndTime:'" + (dt.Rows[i]["EndTime"] == null ? "" : Convert.ToDateTime(dt.Rows[i]["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")) + "'"
                     + ",Duration:'" + (dt.Rows[i]["Duration"] == null ? "0" : dt.Rows[i]["Duration"].ToString()) + "秒'"
                     + ",AgentNum:'" + (dt.Rows[i]["TrueName"] == null ? "" : dt.Rows[i]["TrueName"].ToString()) + " [工号：" + (dt.Rows[i]["AgentNum"] == null ? "" : dt.Rows[i]["AgentNum"].ToString()) + "]'"
                     + ",PerSatisfaction:'" + (dt.Rows[i]["PerSatisfaction"] == null ? "" : dt.Rows[i]["PerSatisfaction"].ToString()) + "'"
                     + ",ProSatisfaction:'" + (dt.Rows[i]["ProSatisfaction"] == null ? "" : dt.Rows[i]["ProSatisfaction"].ToString()) + "'"
                     + ",Contents:'" + (dt.Rows[i]["Contents"] == null ? "" : dt.Rows[i]["Contents"].ToString()) + "'"
                    //  + ",VisitRefer:'" + dt.Rows[i]["VisitRefer"].ToString() + "'"
                    + ",UserReferTitle:'" + (dt.Rows[i]["UserReferTitle"] == null ? "" : dt.Rows[i]["UserReferTitle"].ToString()) + "'},";
            }
            if (dt.Rows.Count > 0)
            {
                msg = msg.TrimEnd(',') + "]}";
            }
        }

        private void GetMemberInfoByVisitID(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BLL.Conversations.Instance.GetMemberInfoByVisitID(" and a.VisitID='" + StringHelper.SqlFilter(VisitID) + "'");
            if (dt.Rows.Count > 0)
            {
                msg += "{root:[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += "{MemberName:'" + dt.Rows[i]["MemberName"].ToString()
                     + "',LoginID:'" + dt.Rows[i]["LoginID"].ToString() + "'"
                     + ",DistrictName:'" + dt.Rows[i]["DistrictName"].ToString() + "'"
                     + ",CityGroupName:'" + dt.Rows[i]["CityGroupName"].ToString() + "'"
                     + ",ContractName:'" + dt.Rows[i]["ContractName"].ToString() + "'"
                     + ",ContractJob:'" + dt.Rows[i]["ContractJob"].ToString() + "'"
                     + ",Address:'" + dt.Rows[i]["Address"].ToString() + "'"
                     + ",ContractEmail:'" + dt.Rows[i]["ContractEmail"].ToString() + "'"
                     + ",MemberCode:'" + dt.Rows[i]["MemberCode"].ToString() + "'"
                     + ",ContractPhone:'" + dt.Rows[i]["ContractPhone"].ToString() + "'},";
            }
            if (dt.Rows.Count > 0)
            {
                msg = msg.TrimEnd(',') + "]}";
            }
        }

        /// <summary>
        /// 添加满意度数据
        /// </summary>
        /// <param name="msg"></param>
        private void AddSatisfaction(out string msg)
        {
            msg = string.Empty;
            int newcsid, newpersatisfaction, newprosatisfaction;
            if (int.TryParse(CSID, out newcsid) && int.TryParse(ProSatisfaction, out newprosatisfaction) && int.TryParse(PerSatisfaction, out newpersatisfaction))
            {
                if (CSIDExist(newcsid) && !BLL.UserSatisfaction.Instance.SatisfactionExists(newcsid))
                {
                    UserSatisfaction model = new UserSatisfaction();
                    model.CSID = newcsid;
                    model.ProSatisfaction = newprosatisfaction;
                    model.PerSatisfaction = newpersatisfaction;
                    model.Contents = SatisfactionDetails;
                    model.CreateTime = DateTime.Now;

                    if (BLL.UserSatisfaction.Instance.Insert(model) > 0)
                    {
                        msg = "success";
                    }
                    else
                    {
                        msg = "满意度提交失败，请稍后再试！";
                    }
                }
                else
                {
                    msg = "您的评价我们已经记录，谢谢您对我们的支持！";
                }
            }
        }
        /// <summary>
        /// 验证参数的合法性
        /// </summary>
        /// <param name="msg"></param>
        private void CheckParams(out string msg)
        {
            msg = "";

            if (Action == "")
            {
                msg += "参数不正确";
            }

            #region 添加满意度
            if (Action == "AddSatisfaction")
            {
                if (PerSatisfaction == "")
                {
                    msg += "请选择服务满意度";
                }

                if (ProSatisfaction == "")
                {
                    msg += "请选择产品满意度";
                }
            }
            #endregion
        }
        /// <summary>
        /// 判断会话id是否存在
        /// </summary>
        /// <param name="CsId"></param>
        /// <returns></returns>
        private bool CSIDExist(int CsId)
        {
            return BLL.Conversations.Instance.Exists(CsId);
        }

        //private void GetCityDataByDistrict(out string msg)
        //{
        //    msg = string.Empty;

        //    DataTable dt = BLL.BaseData.Instance.GetCityGroupByDistrict(District);
        //    if (dt.Rows.Count > 0)
        //    {
        //        msg += "{root:[";
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        msg += "{name:'" + dt.Rows[i]["CityGroupName"].ToString() + "',value:'" + dt.Rows[i]["CityGroup"].ToString() + "'},";
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        msg = msg.TrimEnd(',') + "]}";
        //    }
        //}

        private void GetDistrictData(out string msg)
        {
            msg = string.Empty;

            DataTable dt = BLL.BaseData.Instance.GetAllDistrict();
            if (dt.Rows.Count > 0)
            {
                msg += "{root:[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += "{name:'" + dt.Rows[i]["DistrictName"].ToString() + "',value:'" + dt.Rows[i]["District"].ToString() + "'},";
            }
            if (dt.Rows.Count > 0)
            {
                msg = msg.TrimEnd(',') + "]}";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}