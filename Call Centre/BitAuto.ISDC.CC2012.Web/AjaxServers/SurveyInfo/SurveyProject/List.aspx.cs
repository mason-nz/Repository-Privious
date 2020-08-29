using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyProject
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性定义
        private string ProjectName
        {
            get { return BLL.Util.GetCurrentRequestStr("ProjectName"); }
        }
        private string BGID
        {
            get { return BLL.Util.GetCurrentRequestStr("BGID"); }
        }
        private string SCID
        {
            get { return BLL.Util.GetCurrentRequestStr("SCID"); }
        }
        private string BusniessGroup
        {
            get { return BLL.Util.GetCurrentRequestStr("BusniessGroup"); }
        }
        private string SurveyStatus
        {
            get { return BLL.Util.GetCurrentRequestStr("SurveyStatus"); }
        }
        private string BeginTime
        {
            get { return BLL.Util.GetCurrentRequestStr("BeginTime"); }
        }
        private string EndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndTime"); }
        }
        private string CreateUserID
        {
            get { return BLL.Util.GetCurrentRequestStr("CreateUserID"); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD3607"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                SurveyProjectBind();
            }
        }

        private void SurveyProjectBind()
        {
            Entities.QuerySurveyProjectInfo query = new Entities.QuerySurveyProjectInfo();
            if (!string.IsNullOrEmpty(ProjectName))
            {
                query.Name = ProjectName;
            }
            int bgId = 0;
            if (int.TryParse(BGID, out bgId)&&bgId>0)
            {
                query.BGID = bgId;
            }
            int scId = 0;
            if (int.TryParse(SCID, out scId) && scId > 0)
            {
                query.SCID = scId;
            }
            if (!string.IsNullOrEmpty(BusniessGroup))
            {
                query.BusinessGroup = BusniessGroup;
            }
            if (!string.IsNullOrEmpty(SurveyStatus))
            {
                query.StatusStr = SurveyStatus;
            }
            DateTime beginTime = Entities.Constants.Constant.DATE_INVALID_VALUE;
            DateTime endTime = Entities.Constants.Constant.DATE_INVALID_VALUE;
            if (DateTime.TryParse(BeginTime, out beginTime))
            {
                query.SurveyStartTime = beginTime;
            }
            if (DateTime.TryParse(EndTime, out endTime))
            {
                query.SurveyEndTime = endTime.Add(new TimeSpan(23,59,59));
            }
            int userId = 0;
            if (int.TryParse(CreateUserID, out userId)&&userId>0)
            {
                query.CreateUserID = userId;
            }
            query.LoginUserID = BLL.Util.GetLoginUserID();

            int totalCount = 0;
            DataTable dt = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query,"spi.CreateTime desc", BLL.PageCommon.Instance.PageIndex, BLL.PageCommon.Instance.PageSize, out totalCount);
            rptSurveyProject.DataSource = dt;
            rptSurveyProject.DataBind();

            AjaxPager_Project.PageSize = 10;
            AjaxPager_Project.InitPager(totalCount);
        }

        /// <summary>
        /// 显示项目状态
        /// </summary>
        public string ShowSurveyProjectStatusStr(string beginTimeStr,string endtimeStr,string status)
        {
            string StatusStr = string.Empty;
            DateTime beginTime = DateTime.Parse(beginTimeStr);
            DateTime endTime = DateTime.Parse(endtimeStr);
            if (int.Parse(status) > 0)
            {
                StatusStr = "已完成";
            }
            else
            {
                if (beginTime >= DateTime.Now)
                {
                    StatusStr = "未开始";
                }
                else if (beginTime <= DateTime.Now && endTime > DateTime.Now)
                {
                    StatusStr = "进行中";
                }
                else if (endTime < DateTime.Now)
                {
                    StatusStr = "已结束";
                }
            }

            return StatusStr;
        }

        //显示创建人员名称
        public string ShowCreateUserName(string createUserId)
        {
            string userName = string.Empty;
            int userId = -1;
            if (int.TryParse(createUserId, out userId))
            {
                userName=BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userId);
            }

            return userName;
        }

        //预计参加调查人员数目
        public string ShowEstimatePersonNum(string spiIdStr)
        {
            string num = string.Empty;
            int spiId = 0;
            if (int.TryParse(spiIdStr, out spiId))
            {
               DataTable dt = BLL.SurveyPerson.Instance.GetSurveyPersonBySPIID(spiId);
               if (dt != null)
               {
                   num = dt.Rows.Count.ToString();
               }
            }

            return num;
        }

        //实际参加调查人员数目
        public string ShowTruePersonNum(string spiIdStr)
        {
            string num = string.Empty;
            int spiId = 0;
            if (int.TryParse(spiIdStr, out spiId))
            {
                num = BLL.SurveyAnswer.Instance.GetAnswerUserCountBySPIID(spiId).ToString();
            }
            return num;
        }

        public string ShowButtonHtml(string beginTimeStr, string endtimeStr, string status, string spiIdStr)
        {
            string ButtonHtml = string.Empty;
            DateTime beginTime = DateTime.Parse(beginTimeStr);
            DateTime endTime = DateTime.Parse(endtimeStr);
            if (beginTime >= DateTime.Now)
            {
                ButtonHtml = "<a href=\"/SurveyInfo/SurveyProject/Dispose.aspx?SPIID=" + spiIdStr + "\" target=\"_blank\">编辑</a>&nbsp;&nbsp;<a href=\"javascript:void(0)\" onclick=\"DeleteSurveyProjectInfo(" + spiIdStr + ")\">删除</a>";
            }
            else if (beginTime <= DateTime.Now && endTime > DateTime.Now)
            {
                ButtonHtml = "<a href=\"javascript:void(0)\" onclick=\"CompleteSurveyProjectInfo(" + spiIdStr + ")\">完成</a>&nbsp;&nbsp;<a href=\"/SurveyInfo/SurveyProject/Display.aspx?SPIID=" + spiIdStr + "\" target=\"_blank\">查看</a>&nbsp;&nbsp;<a href=\"SurveyStatResult.aspx?SPIID=" + spiIdStr + "\" target=\"_blank\">查看统计</a>";
            }
            else if (endTime < DateTime.Now)
            {
                ButtonHtml = "<a href=\"/SurveyInfo/SurveyProject/Display.aspx?SPIID=" + spiIdStr + "\" target=\"_blank\">查看</a>&nbsp;&nbsp;<a href=\"SurveyStatResult.aspx?SPIID=" + spiIdStr + "\" target=\"_blank\">查看统计</a>";
                ButtonHtml += "&nbsp;&nbsp;<a class='hrefExport' href=\"/SurveyInfo/SurveyProject/ExportExcel.aspx?SPIID=" + spiIdStr + "\">导出详细</a>";
            }

            return ButtonHtml;
        }
    }
}