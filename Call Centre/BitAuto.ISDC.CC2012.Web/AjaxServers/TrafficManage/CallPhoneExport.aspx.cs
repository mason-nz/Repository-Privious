using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class CallPhoneExport : PageBase
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

        public string Category
        {
            get
            {
                return Request["selCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["selCategory"].ToString().Trim());
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

        /// <summary>
        /// 电话状态（1-呼入，2-呼出）
        /// </summary>
        private string CallStatus
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

        private string RequestBrowser
        {
            get
            {

                return HttpContext.Current.Request["Browser"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Browser"].ToString());
            }
        }
        public int RecordCount;
        public int userID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //增加“话务管理--去电记录”导出功能验证逻辑
            userID = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userID, "SYS024BUT4021"))
            {
                BindData();
            }
            else
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }

        private void BindData()
        {
            bool r;
            string info;
            DateTime st = CommonFunction.ObjectToDateTime(BeginTime);
            DateTime et = CommonFunction.ObjectToDateTime(EndTime);
            r = BLL.Util.CheckForSelectCallRecordORIG(st, et, out info);
            if (r == false)
            {
                return;
            }

            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;

            _loginID = userID;
            #region 调整分组前数据权限
            /*
            //判断数据权限，数据权限如果为 2-全部，则查看所有数据
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
            }
             */
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
            DataTable dt = BLL.CallRecordInfo.Instance.GetCallRecordInfo(query, "c.CreateTime desc", 1, -1, tableEndName, out RecordCount);
            ExportData(dt);
        }

        private void ExportData(DataTable dt)
        {
            if (dt != null)
            {
                dt.Columns.Add("OutBoundTypeName");

                dt.Columns["TASKID"].SetOrdinal(0);
                dt.Columns["TASKTYPENAME"].SetOrdinal(1);
                dt.Columns["AGENTNAME"].SetOrdinal(2);
                dt.Columns["AGENTNUM"].SetOrdinal(3);
                dt.Columns["PHONENUM"].SetOrdinal(4);
                dt.Columns["BEGINTIME"].SetOrdinal(5);
                dt.Columns["ENDTIME"].SetOrdinal(6);
                dt.Columns["TALLTIME"].SetOrdinal(7);
                dt.Columns["OutBoundTypeName"].SetOrdinal(8);

                foreach (DataRow dr in dt.Rows)
                {
                    dr["OutBoundTypeName"] = dr["OutBoundType"].ToString() == "1" ? "页面" : dr["OutBoundType"].ToString() == "2" ? "客户端" : dr["OutBoundType"].ToString() == "4" ? "自动" : "无";
                }

                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() != "TASKID"
                        && dt.Columns[i].ColumnName.ToUpper() != "TASKTYPENAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "AGENTNAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "AGENTNUM"
                        && dt.Columns[i].ColumnName.ToUpper() != "PHONENUM"
                        && dt.Columns[i].ColumnName.ToUpper() != "BEGINTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "ENDTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "TALLTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "OUTBOUNDTYPENAME"
                        )
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName);
                    }
                    else
                    {
                        #region 修改列名

                        switch (dt.Columns[i].ColumnName)
                        {
                            case "AgentNum": dt.Columns[i].ColumnName = "工号"; break;
                            case "AgentName": dt.Columns[i].ColumnName = "坐席"; break;
                            case "PhoneNum": dt.Columns[i].ColumnName = "被叫号码"; break;
                            case "BeginTime": dt.Columns[i].ColumnName = "开始时间"; break;
                            case "EndTime": dt.Columns[i].ColumnName = "结束时间"; break;
                            case "TallTime": dt.Columns[i].ColumnName = "通话时长(秒)"; break;
                            case "TaskTypeName": dt.Columns[i].ColumnName = "任务分类"; break;
                            case "TaskID": dt.Columns[i].ColumnName = "任务ID"; break;
                            case "OutBoundTypeName": dt.Columns[i].ColumnName = "呼叫类别"; break;
                        }
                        #endregion
                    }
                }


                BLL.Util.ExportToCSV("去电记录" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }
    }
}