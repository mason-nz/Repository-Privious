using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class List : PageBase
    {
        #region 参数

        private string IVRScore
        {
            get
            {
                return Request["IVRScore"] == null ? "" :
                HttpUtility.UrlDecode(Request["IVRScore"].ToString().Trim());
            }
        }

        private string IncomingSource
        {
            get
            {
                return Request["IncomingSource"] == null ? "" :
                HttpUtility.UrlDecode(Request["IncomingSource"].ToString().Trim());
            }
        }

        private string Name
        {
            get
            {
                return Request["Name"] == null ? "" :
                HttpUtility.UrlDecode(Request["Name"].ToString().Trim());
            }
        }

        private string ANI
        {
            get
            {
                return Request["ANI"] == null ? "" :
                HttpUtility.UrlDecode(Request["ANI"].ToString().Trim());
            }
        }

        private string Agent
        {
            get
            {
                return Request["Agent"] == null ? "" :
                HttpUtility.UrlDecode(Request["Agent"].ToString().Trim());
            }
        }

        private string TaskID
        {
            get
            {
                return Request["TaskID"] == null ? "" :
                HttpUtility.UrlDecode(Request["TaskID"].ToString().Trim());
            }
        }

        private string CallID
        {
            get
            {
                return Request["CallID"] == null ? "" :
                HttpUtility.UrlDecode(Request["CallID"].ToString().Trim());
            }
        }

        private string BeginTime
        {
            get
            {
                return Request["BeginTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["BeginTime"].ToString().Trim());
            }
        }

        private string EndTime
        {
            get
            {
                return Request["EndTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["EndTime"].ToString().Trim());
            }
        }

        private string AgentNum
        {
            get
            {
                return Request["AgentNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentNum"].ToString().Trim());
            }
        }
        private string PhoneNum
        {
            get
            {
                return Request["PhoneNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["PhoneNum"].ToString().Trim());
            }
        }

        private string TaskCategory
        {
            get
            {
                return Request["TaskCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["TaskCategory"].ToString().Trim());
            }
        }

        private string SpanTime1
        {
            get
            {
                return Request["SpanTime1"] == null ? "" :
                HttpUtility.UrlDecode(Request["SpanTime1"].ToString().Trim());
            }
        }

        private string SpanTime2
        {
            get
            {
                return Request["SpanTime2"] == null ? "" :
                HttpUtility.UrlDecode(Request["SpanTime2"].ToString().Trim());
            }
        }

        private string AgentGroup
        {
            get
            {
                return Request["AgentGroup"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentGroup"].ToString().Trim());
            }
        }

        public string Category
        {
            get
            {
                return Request["selCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["selCategory"].ToString().Trim());
            }
        }

        /// <summary>
        /// 电话状态（1-呼入，2-呼出）
        /// </summary>
        public string CallStatus
        {
            get
            {
                return Request["CallStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["CallStatus"].ToString().Trim());
            }
        }

        /// <summary>
        /// 业务线
        /// </summary>
        public string selBusinessType
        {
            get
            {
                return Request["selBusinessType"] == null ? "" :
                HttpUtility.UrlDecode(Request["selBusinessType"].ToString().Trim());
            }
        }

        public string OutTypes
        {
            get
            {
                return Request["OutTypes"] == null ? "" :
                HttpUtility.UrlDecode(Request["OutTypes"].ToString().Trim());
            }
        }
        public string ProjectId
        {
            get
            {
                return Request["ProjectId"] == null ? "" :
                HttpUtility.UrlDecode(Request["ProjectId"].ToString().Trim());
            }
        }
        public string IsSuccess
        {
            get
            {
                return Request["IsSuccess"] == null ? "" :
                HttpUtility.UrlDecode(Request["IsSuccess"].ToString().Trim());
            }
        }
        public string FailReason
        {
            get
            {
                return Request["FailReason"] == null ? "" :
                HttpUtility.UrlDecode(Request["FailReason"].ToString().Trim());
            }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;

        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        private void BindData()
        {
            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;

            _loginID = userID;
            #region 调整分组前数据权限
            //判断数据权限，数据权限如果为 2-全部，则查看所有数据
            /*
            Entities.UserDataRigth model_userDataRight = BLL.UserDataRigth.Instance.GetUserDataRigth(userID);
            if (model_userDataRight != null)
            {
                if (model_userDataRight.RightType != 2)//数据权限不为 2-全部
                {
                    _loginID = userID;
                    //判断分组权限，如果权限是2-本组，则能看到本组人创建的信息；如果权限是1-本人，则只能看本人创建的信息 
                    DataTable dt_userGroupDataRight = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userID);
                    string ownGroup = string.Empty;//权限是本组的 组串
                    string oneSelf = string.Empty; //权限是本人的 组串
                    for (int i = 0; i < dt_userGroupDataRight.Rows.Count; i++)
                    {
                        if (dt_userGroupDataRight.Rows[i]["RightType"].ToString() == "2")
                        {
                            ownGroup += dt_userGroupDataRight.Rows[i]["BGID"].ToString() + ",";
                        }
                        if (dt_userGroupDataRight.Rows[i]["RightType"].ToString() == "1")
                        {
                            oneSelf += dt_userGroupDataRight.Rows[i]["BGID"].ToString() + ",";
                        }
                    }
                    _ownGroup = ownGroup.TrimEnd(',');
                    _oneSelf = oneSelf.TrimEnd(',');
                }
            }*/
            #endregion

            Entities.QueryCallRecordInfo query = BLL.CallRecordInfo.Instance.GetQueryModel(
                Name, ANI, Agent, TaskID, CallID, BeginTime, EndTime, AgentNum, PhoneNum, TaskCategory,
                SpanTime1, SpanTime2, AgentGroup, CallStatus, _loginID, _ownGroup, _oneSelf, Category, IVRScore, IncomingSource, selBusinessType, ""
                );
            query.OutTypes = OutTypes;
            int projectid;
            if (int.TryParse(ProjectId, out projectid))
            {
                query.ProjectId = projectid;
            }
            int issuccess;
            if (int.TryParse(IsSuccess, out issuccess))
            {
                query.IsSuccess = issuccess;
            }
            int failreason;
            if (int.TryParse(FailReason, out failreason))
            {
                query.FailReason = failreason;
            }
            string tableEndName = BLL.Util.CalcTableNameByMonth(3, query.BeginTime.Value);
            DataTable dt = BLL.CallRecordInfo.Instance.GetCallRecordInfo(query, "c.CreateTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, tableEndName, out RecordCount);

            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        private Random R = new Random();
        public string GetViewUrl(string TaskID, string BGID, string SCID)
        {
            string url = "";
            url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, BGID, SCID);
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
            return BLL.Util.GenBusinessURLByBGIDAndSCID(BGID, SCID, url, TaskID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID);
        }
    }
}