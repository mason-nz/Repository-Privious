using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_BusinessScale
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        public HttpContext currentContext;
        #region 属性
        public string RequstAction
        {
            get { return currentContext.Request["Action"] == null ? string.Empty : currentContext.Request["Action"].ToString(); }
        }
        /// <summary>
        /// ID
        /// </summary>
        public string RequestRecID
        {
            get { return currentContext.Request["RecID"] == null ? string.Empty : currentContext.Request["RecID"].ToString(); }
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string RequestTID
        {
            get { return currentContext.Request["TID"] == null ? string.Empty : currentContext.Request["TID"].Trim(); }
        }
        /// <summary>
        /// 月库存数量
        /// </summary>
        public string RequestMonthStock
        {
            get { return currentContext.Request["MonthStock"] == null ? string.Empty : currentContext.Request["MonthStock"]; }
        }
        /// <summary>
        /// 月置换数量
        /// </summary>
        public string RequestMonthSales
        {
            get { return currentContext.Request["MonthSales"] == null ? string.Empty : currentContext.Request["MonthSales"]; }
        }

        /// <summary>
        /// 月交易量
        /// </summary>
        public string RequestMonthTrade
        {
            get { return currentContext.Request["MonthTrade"] == null ? string.Empty : currentContext.Request["MonthTrade"]; }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            currentContext = context;
            string msg = string.Empty;
            switch (RequstAction.ToLower())
            {
                case "add":
                    Add(out msg);
                    break;
                case "edit":
                    Edit(out msg);
                    break;
                case "del":
                    Del(out msg);
                    break;
            }

            context.Response.Write(msg);
        }

        public void Add(out string msg)
        {
            msg = string.Empty;
            Entities.ProjectTask_BusinessScale model = new Entities.ProjectTask_BusinessScale();
            try
            {
                msg = VerifyRequest();
                if (msg.Length < 1)
                {
                    model.PTID = RequestTID;
                    if (!string.IsNullOrEmpty(RequestMonthStock))
                    {
                        model.MonthStock = int.Parse(RequestMonthStock);
                    }
                    if (!string.IsNullOrEmpty(RequestMonthSales))
                    {
                        model.MonthSales = int.Parse(RequestMonthSales);
                    }
                    if (!string.IsNullOrEmpty(RequestMonthTrade))
                    {
                        model.MonthTrade = int.Parse(RequestMonthTrade);
                    }
                    model.CreateUserID = BLL.Util.GetLoginUserID();
                    BLL.ProjectTask_BusinessScale.Instance.Insert(model);
                    msg = "success";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        public void Edit(out string msg)
        {
            msg = string.Empty;
            try
            {
                msg = VerifyRequest();
                if (msg.Length < 1)
                {
                    Entities.ProjectTask_BusinessScale model = BLL.ProjectTask_BusinessScale.Instance.GetProjectTask_BusinessScale(int.Parse(RequestRecID));
                    if (!string.IsNullOrEmpty(RequestMonthStock))
                    {
                        model.MonthStock = int.Parse(RequestMonthStock);
                    }
                    else
                    {
                        model.MonthStock = null;
                    }
                    if (!string.IsNullOrEmpty(RequestMonthSales))
                    {
                        model.MonthSales = int.Parse(RequestMonthSales);
                    }
                    else
                    {
                        model.MonthSales = null;
                    }
                    if (!string.IsNullOrEmpty(RequestMonthTrade))
                    {
                        model.MonthTrade = int.Parse(RequestMonthTrade);
                    }
                    else
                    {
                        model.MonthTrade = null;
                    }
                    model.ModifyUserID = BLL.Util.GetLoginUserID();
                    BLL.ProjectTask_BusinessScale.Instance.Update(model);
                    msg = "success";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        public void Del(out string msg)
        {
            msg = string.Empty;
            int recId = -1;
            if (int.TryParse(RequestRecID, out recId))
            {
                try
                {
                    BLL.ProjectTask_BusinessScale.Instance.Delete(recId);
                    msg = "success";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            else
            {
                msg = "二手车规模ID有误！";
            }
        }

        public string VerifyRequest()
        {
            StringBuilder sbStr = new StringBuilder();
            int id = -1;
            if (RequestMonthSales == "-1" && RequestMonthStock == "-1" && RequestMonthTrade == "-1")
            {
                sbStr.Append("至少选择一项");
            }
            if (string.IsNullOrEmpty(RequestTID) && RequstAction.ToLower().Equals("add"))
            {
                sbStr.Append("任务ID不能为空!<br />");
            }
            if (!string.IsNullOrEmpty(RequestMonthStock))
            {
                if (!int.TryParse(RequestMonthStock, out id))
                {
                    sbStr.Append("月库存量有误!<br />");
                }
            }
            if (!string.IsNullOrEmpty(RequestMonthSales))
            {
                if (!int.TryParse(RequestMonthSales, out id))
                {
                    sbStr.Append("月置换量有误!<br />");
                }
            }
            if (!string.IsNullOrEmpty(RequestRecID))
            {
                if (!int.TryParse(RequestRecID, out id))
                {
                    sbStr.Append("二手车范围ID有误!<br />");
                }
            }
            if (!string.IsNullOrEmpty(RequestMonthTrade))
            {
                if (!int.TryParse(RequestMonthTrade, out id))
                {
                    sbStr.Append("月交易量有误!<br />");
                }
            }

            return sbStr.ToString();
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