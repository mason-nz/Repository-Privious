using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.CustCheck;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck
{
    public partial class CheckTaskList : PageBase
    {

        #region 属性
        private string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }
        private string RequestCustName
        {
            get { return HttpContext.Current.Request["custName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["custName"].ToString()); }
        }

        private string RequestStatuss
        {
            get { return HttpContext.Current.Request["status_s"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status_s"].ToString()); }
        }
        private string RequestOperationStatus
        {
            get { return HttpContext.Current.Request["operstatus_s"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["operstatus_s"].ToString()); }
        }
        private string RequestGroup
        {
            get { return HttpContext.Current.Request["group"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["group"].ToString()); }
        }
        private string RequestCategory
        {
            get { return HttpContext.Current.Request["category"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["category"].ToString()); }
        }
        private string RequestCreater
        {
            get { return HttpContext.Current.Request["creater"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["creater"].ToString()); }
        }
        private string RequestUserId
        {
            get { return HttpContext.Current.Request["selUserId"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["selUserId"].ToString()); }
        }
        private string RequestOptUserId
        {
            get { return HttpContext.Current.Request["optUserId"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["optUserId"].ToString()); }
        }

        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["beginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["beginTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["endTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["endTime"].ToString()); }
        }

        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pagesize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pagesize"].ToString());
            }
        }

        private string TaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }

        private string CustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }

        private string CustType
        {
            get { return HttpContext.Current.Request["CustType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustType"].ToString()); }
        }

        private string LastOperStartTime
        {
            get { return HttpContext.Current.Request["LastOperStartTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LastOperStartTime"].ToString()); }
        }
        private string LastOperEndTime
        {
            get { return HttpContext.Current.Request["LastOperEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LastOperEndTime"].ToString()); }
        }
        public string AdditionalStatus
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["AdditionalStatus"]) ? "" :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["AdditionalStatus"].ToString());
            }
        }

        public string CRMBrandIDs
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["CRMBrandIDs"]) ? "" :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["CRMBrandIDs"].ToString());
            }
        }

        public string NoCRMBrand
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["NoCRMBrand"]) ? "" :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["NoCRMBrand"].ToString());
            }
        }

        #endregion


        CustCheckHelper helper = new CustCheckHelper();

        public string QueryParams { get { return HttpUtility.UrlEncode(helper.QueryParams); } }

        public bool right_btn1;   //分配

        /// <summary>
        /// 回收
        /// </summary>
        public bool right_btn2;  //回收

        /// <summary>
        /// 结束
        /// </summary>
        public bool right_btn3;  //结束
        public bool right_btn4;  //导出
        public bool right_btn5;  //处理
        public bool right_btn6;  //审核

        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();

                right_btn1 = BLL.Util.CheckRight(userID, "SYS024BUT130101");
                right_btn2 = BLL.Util.CheckRight(userID, "SYS024BUT130102");
                right_btn3 = BLL.Util.CheckRight(userID, "SYS024BUT130103");
                right_btn4 = BLL.Util.CheckRight(userID, "SYS024BUT130104");
                right_btn5 = BLL.Util.CheckRight(userID, "SYS024BUT130105");
                right_btn6 = BLL.Util.CheckRight(userID, "SYS024BUT130106");

                BindData();
            }
        }

        private void BindData()
        {

            int totalCount = 0;
            if (!int.TryParse(RequestPageSize, out pageSize))
            {
                pageSize = 20;
            }

            Entities.QueryProjectTaskInfo query = BLL.ProjectTaskInfo.Instance.GetQuery(RequestName, RequestCustName, RequestStatuss, AdditionalStatus,
                RequestGroup, RequestCategory, RequestCreater, RequestUserId, RequestBeginTime, RequestEndTime, RequestOptUserId, CRMBrandIDs, NoCRMBrand,
                 TaskID, CustID, RequestOperationStatus, CustType, LastOperStartTime, LastOperEndTime);

            DataTable dt = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(query, BLL.PageCommon.Instance.PageIndex, pageSize, out totalCount, userID);

            RecordCount = totalCount;

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, pageSize, BLL.PageCommon.Instance.PageIndex, 2);
        }

        //获取状态
        public string getStatus(string status, string AdditionalStatus)
        {
            string _statusStr = string.Empty;

            int intval = 0;
            if (int.TryParse(status, out intval))
            {
                if (intval == 180003 || intval == 180004 || intval == 180010)
                {
                    _statusStr = "待审核";
                }
                else
                {
                    _statusStr = BLL.Util.GetEnumOptText(typeof(EnumProjectTaskStatus), intval);
                    if (intval == 180001)
                    {
                        if (AdditionalStatus != "")
                        {
                            _statusStr = _statusStr + "(" + AdditionalStatus.Substring(3, 1) + ")";
                        }
                    }
                }
            }
            return _statusStr;
        }

        //获取操作人
        public string getOperator(string createrID)
        {
            string _operator = string.Empty;
            int _createrID;
            if (int.TryParse(createrID, out _createrID))
            {
                _operator = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_createrID);
            }
            return _operator;
        }

        /// <summary>
        /// 查看链接
        /// </summary>
        /// <param name="status"></param>
        /// <param name="additionalStatus"></param>
        /// <param name="taskid"></param>
        /// <param name="carType"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public string GetViewStr(string status, string additionalStatus, string taskid, string carType, string source)
        {
            string htmlstr = "";

            htmlstr = getview(taskid, carType, source, status);

            return htmlstr;
        }

        //得到操作链接
        public string GetCheckUrl(string status, string additionalStatus, string taskid, string carType, string source, string AssignUserId)
        {
            string htmlstr = "";

            //if ((status == "180012" || status == "180000" || status == "180001") && right_btn1)
            //{
            //    htmlstr += "<a name='aAssign'  taskid='" + taskid + "'    href='#'>分配</a>&nbsp;&nbsp;";
            //}

            if ((status == "180000" || status == "180001") && right_btn5)
            {
                if (userID.ToString() == AssignUserId)
                {
                    //分配给当前人的，才可以点击处理

                    if (source == "2")
                    {
                        //CRM

                        if (carType == "2")
                        {
                            htmlstr += "<a target=_blank' href=\"/CustCheck/CrmCustCheck/SecondCarCheck.aspx?TID=" + taskid + "\">处理</a>&nbsp;&nbsp;";
                        }
                        else
                        {
                            htmlstr += "<a target=_blank'  href=\"/CustCheck/CrmCustCheck/Check.aspx?TID=" + taskid + "\">处理</a>&nbsp;&nbsp;";
                        }
                    }
                    else
                    {
                        if (carType == "2")
                        {
                            htmlstr += "<a target=_blank'  href=\"/CustCheck/NewCustCheck/SecondCarCheck.aspx?TID=" + taskid + "\">处理</a>&nbsp;&nbsp;";
                        }
                        else
                        {
                            htmlstr += "<a target=_blank'  href=\"/CustCheck/NewCustCheck/Check.aspx?TID=" + taskid + "\">处理</a>&nbsp;&nbsp;";
                        }
                    }
                }
            }

            //if ((status == "180000" || status == "180001") && right_btn2)
            //{
            //    htmlstr += "<a name='aReturn' taskid='" + taskid + "'  href='#'>收回</a>&nbsp;&nbsp;";
            //}
            if ((status == "180003" || status == "180004" || status == "180011" || status == "180010") && right_btn6)
            {
                htmlstr += "<a name='aRefuse' target='_blank' href='/CustAudit/View.aspx?TID=" + taskid + "'>审核</a>&nbsp;&nbsp;";
            }
            //if ((status != "180014" && status != "180015" && status != "180016") && right_btn3)
            //{
            //    htmlstr += "<a name='aStop' taskid='" + taskid + "'   href='#'>结束</a>&nbsp;&nbsp;";
            //}
            return htmlstr;
        }

        public string GetCustUrl(string CustName, string CRMCustID, string source, string TaskStatus)
        {
            string html = "";

            if (source == "1")
            {
                //Excel导入
                if ((TaskStatus == "180014" || TaskStatus == "180015" || TaskStatus == "180016") && CRMCustID != "")
                {
                    html = " <a  target='_blank' href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + CRMCustID + "'>" + CustName + "</a>";
                }
                else
                {
                    html = CustName;
                }
            }
            else if (source == "2")
            {
                html = " <a  target='_blank' href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + CRMCustID + "'>" + CustName + "</a>";
            }

            return html;
        }


        /// <summary>
        /// 查看链接
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="carType"></param>
        /// <param name="source"></param>
        /// <param name="htmlstr"></param>
        /// <returns></returns>
        public string getview(string taskid, string carType, string source, string TaskStatus)
        {
            string html = "";

            if (TaskStatus == "180012")
            {
                return taskid;
            }

            #region 查看URL

            string url = "";

            if (source == "2")
            {
                //CRM
                if (carType == "2")
                {
                    url += "/CustCheck/CrmCustCheck/SecondCarView.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=" + this.QueryParams + "&Action=view";
                }
                else
                {
                    url += "/CustCheck/CrmCustCheck/View.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=" + this.QueryParams + "&Action=view";
                }
            }
            else
            {
                //Excel
                if (carType == "2")
                {
                    url += "/CustCheck/NewCustCheck/SecondCarView.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=" + this.QueryParams + "&Action=view";
                }
                else
                {
                    url += "/CustCheck/NewCustCheck/View.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=" + this.QueryParams + "&Action=view";
                }
            }
            #endregion

            html += " <a target='_blank' href='" + url + "'>" + taskid + "</a>";

            return html;
        }
    }
}