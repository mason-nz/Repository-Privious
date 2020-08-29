using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class List : PageBase
    {
        #region 属性
        private string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }

        private int RequestProjectID
        {
            get { return BLL.Util.GetCurrentRequestQueryInt("ProjectID"); }
        }
        private string RequestStatuss
        {
            get { return HttpContext.Current.Request["statuss"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["statuss"].ToString()); }
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
        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["beginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["beginTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["endTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["endTime"].ToString()); }
        }

        private string ISAutoCall
        {
            get { return HttpContext.Current.Request["ISAutoCall"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ISAutoCall"].ToString()); }
        }
        private string ACStatus
        {
            get { return HttpContext.Current.Request["ACStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ACStatus"].ToString()); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        public bool right_edit; //编辑
        public bool right_delete;   //删除
        public bool right_newTask; //生成 
        public bool right_stopProject;//结束
        public bool right_ExportTask;//导出任务
        public bool right_viewProject;//查看
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD500601"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                right_edit = BLL.Util.CheckRight(userID, "SYS024BUT500602");
                right_delete = BLL.Util.CheckRight(userID, "SYS024BUT500603");
                right_newTask = BLL.Util.CheckRight(userID, "SYS024BUT500604");

                right_viewProject = BLL.Util.CheckRight(userID, "SYS024BUT500606");
                right_stopProject = BLL.Util.CheckRight(userID, "SYS024BUT500607");
                right_ExportTask = BLL.Util.CheckRight(userID, "SYS024BUT500605");

                BindData();
            }
        }

        //绑定数据
        public void BindData()
        {
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();

            if (RequestProjectID > 0)
            {
                query.ProjectID = RequestProjectID;
            }
            else if (!string.IsNullOrEmpty(RequestName))
            {
                query.Name = StringHelper.SqlFilter(RequestName);
            }
            if (RequestStatuss != "")
            {
                query.Statuss = StringHelper.SqlFilter(RequestStatuss);
            }

            if (RequestGroup != "")
            {
                query.BGID = int.Parse(RequestGroup);
            }
            if (RequestCategory != "")
            {
                //query.PCatageID = int.Parse(RequestCategory);
                int outPcatage = -1;
                bool bResult = Int32.TryParse(RequestCategory, out outPcatage);
                if (bResult)
                {
                    query.PCatageID = outPcatage;
                }
            }
            if (RequestCreater != "")
            {
                query.CreateUserID = int.Parse(RequestCreater);
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = StringHelper.SqlFilter(RequestBeginTime);
            }
            if (RequestEndTime != "")
            {
                query.EndTime = StringHelper.SqlFilter(RequestEndTime);
            }
            query.ISAutoCall = ISAutoCall;
            query.ACStatus = ACStatus;

            DataTable dt = BLL.ProjectInfo.Instance.GetProjectInfo(query, "ProjectInfo.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount, userID);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        public string getProjectName(string IsOldData, string Name, string ProjectID, string comCount, string SumCount)
        {
            string str = "";
            if (IsOldData == "1")
            {
                str += Name;
                str += "(" + comCount + "/0)";
            }
            else
            {
                str += "<a href='/ProjectManage/ViewProject.aspx?projectid=";
                str += ProjectID;
                str += "' target='_blank'>";
                str += Name;
                str += "</a>";
                str += "(" + comCount + "/" + SumCount + ")";
            }

            return str;
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
            if (_operator == null)
            {
                _operator = "--";
            }
            return _operator;
        }
        //获取状态
        public string getStatus(string status)
        {
            string _statusStr = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                switch (_status)
                {
                    case 0: _statusStr = "未开始";
                        break;
                    case 1: _statusStr = "进行中";
                        break;
                    case 2: _statusStr = "已结束";
                        break;
                    case -1: _statusStr = "删除";
                        break;
                }
            }
            return _statusStr;
        }
        //外呼状态
        public string getACStatus(string status)
        {
            int a = -1;
            if (int.TryParse(status, out a) && a >= 0)
            {
                return BLL.Util.GetEnumOptText(typeof(Entities.ProjectACStatus), a);
            }
            else
            {
                return "--";
            }
        }
        //得到操作链接
        public string getOperLink(string status, string siid, string source, string NoGenCount, string IsAutoCall, string ACStatus, string notDistributeCount)
        {
            string operLinkStr = string.Empty;

            if (status.Trim() == "1" && notDistributeCount.Trim() != "0")
            {
                operLinkStr += GetDistributeStr(IsAutoCall, ACStatus, notDistributeCount, status, siid);
            }

            //客户回访，其他任务
            if (source == "1" || source == "2" || source == "3" || source == "4")
            {
                operLinkStr +=
                    //编辑
                    rightEdit(siid, status) +
                    //删除
                    rightDelete(status, siid) +
                    //生成任务
                    rightNewTask(status, siid, source, NoGenCount) +
                    //结束项目
                    rightStopProject(status, siid) +
                    //导出
                    rightExport(status, siid, source);
            }
            //C集客 易团购
            else if (source == "6" || source == "7")
            {
                operLinkStr += rightExport(status, siid, source);
            }

            return operLinkStr;
        }
        private string GetDistributeStr(string IsAutoCall, string ACStatus, string notDistributeCount, string status, string siid)
        {
            string rightStr = string.Empty;
            if ((IsAutoCall == "1" && ACStatus != "1") || IsAutoCall != "1") //自动外呼且不在进行中，或是非自动外呼
            {
                if (notDistributeCount.Trim() != "0")   //还有未分配的任务
                {
                    rightStr = "<a href='javascript:void(0)' onclick='DistributeTask(" + siid + ")' >分配</a>&nbsp;";
                }
            }
            return rightStr;
        }
        /// 导出项目的任务
        /// <summary>
        /// 导出项目的任务
        /// </summary>
        /// <param name="siid"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private string rightExport(string status, string siid, string source)
        {
            string rightStr = string.Empty;
            if (status != "0")
            {
                if (right_ExportTask && source == "4")
                {
                    rightStr = "<a href='javascript:void(0)' onclick='ExportData(" + siid + ",1)' >导出</a>&nbsp;";
                }
                else if (right_ExportTask && source == "6")
                {
                    rightStr = "<a href='javascript:void(0)' onclick='ExportData(" + siid + ",2)' >导出</a>&nbsp;";
                }
                else if (right_ExportTask && source == "7")
                {
                    rightStr = "<a href='javascript:void(0)' onclick='ExportData(" + siid + ",3)' >导出</a>&nbsp;";
                }
            }

            return rightStr;
        }
        //编辑按钮权限：有按钮权限 且 状态为0-未开始、1-进行中
        private string rightEdit(string siid, string status)
        {
            string rightStr = string.Empty;

            if (right_edit && (status == "0" || status == "1"))
            {
                rightStr = "<a href='EditProject.aspx?projectid=" + siid + "' target='_blank' name='a_edit'>编辑</a>&nbsp;";
            }
            return rightStr;
        }
        //删除按钮权限：有功能权限 且 状态为0-未完成或者1-未使用
        private string rightDelete(string status, string siid)
        {
            string rightStr = string.Empty;

            if (right_delete && (status == "0"))
            {
                rightStr = "<a href='#' projectID='" + siid + "'   name='a_delete'>删除</a>&nbsp;";
            }
            return rightStr;
        }
        //生成任务：
        private string rightNewTask(string status, string siid, string source, string NoGenCount)
        {
            string rightStr = string.Empty;

            if (right_newTask && (status == "0" || ((status == "1") && int.Parse(NoGenCount) > 0)))
            {
                rightStr = "<a href='javascript:void(0);' onclick='javascript:newTask(" + siid + "," + source + ")' name='aNewTask'>生成任务</a>&nbsp;";
            }

            return rightStr;
        }
        //结束
        private string rightStopProject(string status, string siid)
        {
            string rightStr = string.Empty;

            if (right_stopProject && (status == "1"))
            {
                rightStr = "<a href='#' projectID='" + siid + "'  name='aStopProject'>结束</a>&nbsp;";
            }
            return rightStr;
        }
        //查看
        private string rightViewProject(string status, string siid)
        {
            string rightStr = string.Empty;

            if (right_viewProject)
            {
                rightStr = "<a href='/ProjectManage/ViewProject.aspx?projectid=" + siid + "'  target='_blank' name='aView'>查看</a>&nbsp;";
            }
            return rightStr;
        }
    }
}