using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class FrequencyStatisticsDetails : PageBase
    {
        public string ScoreCreater
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ScoreCreater").ToString();
            }
        }
        public string ScoreBeginTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ScoreBeginTime").ToString();
            }
        }
        public string ScoreEndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ScoreEndTime").ToString();
            }
        }

        public int PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD6304"))//"质检统计管理—抽查频次统计"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }
        //绑定数据
        public void BindData()
        {
            Entities.QueryCallRecordInfo query = new Entities.QueryCallRecordInfo();

            query.LoginID = userID;
            if (!string.IsNullOrEmpty(ScoreBeginTime))
            {
                query.ScoreBeginTime = ScoreBeginTime;
            }
            if (!string.IsNullOrEmpty(ScoreEndTime))
            {
                query.ScoreEndTime = ScoreEndTime;
            }
            if (!string.IsNullOrEmpty(ScoreCreater))
            {
                // query.ScoreCreater = int.Parse(ScoreCreater);
                query.QSScoreCreaters = " and b.UserID IN (" + BLL.Util.SqlFilterByInCondition(ScoreCreater) + ")";
            }
            query.QSResultStatus = "20002,20003,20004,20005,20006";
            query.IsFilterNull = 1;
            int RecordCount = 0;

            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultFrequenyStatistics(query, "tCount desc", BLL.PageCommon.Instance.PageIndex, PageSize, tableEndName, out RecordCount);
            repeaterList.DataSource = dt;
            repeaterList.DataBind();

            AjaxPager.PageSize = 10;
            AjaxPager.InitPager(RecordCount);
        }
        public string GetEmNameByEid(int userID)
        {
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userID);
        }
    }
}