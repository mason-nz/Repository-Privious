using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.StatManage
{
    public partial class ExportAppealStat : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region
        private string RequestUserID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("UserID")); }
        }
        private string RequestGroupID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("GroupID")); }
        }
        private string RequestBeginTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BeginTime")); }
        }
        private string RequestEndTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("EndTime")); }
        }
        public string RequestRecordBeginTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("RecordBeginTime")); }
        }
        public string RequestRecordEndTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("RecordEndTime")); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD6302"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindAppealData();
                }
            }
        }

        private void BindAppealData()
        {
            string where = string.Empty;
            int userId = 0;
            if (int.TryParse(RequestUserID, out userId))
            {
                where += " And cri.CreateUserID=" + userId;
            }
            int groupId = 0;
            if (int.TryParse(RequestGroupID, out groupId) & groupId > 0)
            {
                where += " AND cri.BGID=" + groupId;
            }
            DateTime beginTime;
            DateTime endTime;
            if (DateTime.TryParse(RequestBeginTime, out beginTime))
            {
                where += " And qsr.CreateTime>='" + beginTime.ToString() + "'";
            }
            if (DateTime.TryParse(RequestEndTime, out endTime))
            {
                where += " And qsr.CreateTime>='" + endTime.ToString() + "'";
            }
            DateTime recordBeginTime;
            DateTime recordEndTime;
            if (DateTime.TryParse(RequestRecordBeginTime, out recordBeginTime))
            {
                where += " And cri.CreateTime>='" + recordBeginTime.ToString() + "'";
            }
            if (DateTime.TryParse(RequestRecordEndTime, out recordEndTime))
            {
                where += " And cri.CreateTime<='" + recordEndTime.Add(new TimeSpan(23, 59, 59)).ToString() + "'";
            }

            #region 数据权限判断
            int loginUserId = BLL.Util.GetLoginUserID();

            string whereDataRight = "";
            whereDataRight = BLL.UserGroupDataRigth.Instance.GetSqlRightstr("cri", "BGID", "CreateUserID", loginUserId);

            where += whereDataRight;
            #endregion

            int totalCount = 0;
            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultAppealStat(where, "", 1, -1, tableEndName, out totalCount);

            DataTable dtNew = new DataTable();
            DataColumn dcEmployeeName = new DataColumn("坐席");
            DataColumn dcSumCount = new DataColumn("申诉总量");
            DataColumn dcSuccessSumCount = new DataColumn("申诉成功量");
            DataColumn dcRate = new DataColumn("申诉成功率");
            dtNew.Columns.Add(dcEmployeeName);
            dtNew.Columns.Add(dcSumCount);
            dtNew.Columns.Add(dcSuccessSumCount);
            dtNew.Columns.Add(dcRate);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    //坐席名称
                    drNew[0] = dr["TrueName"].ToString();
                    //申诉总量
                    drNew[1] = dr["SumCount"].ToString();
                    //申诉成功量
                    drNew[2] = dr["appealCount"].ToString();
                    //申诉成功率
                    if (dr["SumCount"].ToString() != "")
                    {
                        drNew[3] = (float.Parse(dr["appealCount"].ToString()) * 100 / float.Parse(dr["SumCount"].ToString())).ToString("0.00") + "%";
                    }
                    else
                    {
                        drNew[3] = "0%";
                    }
                    dtNew.Rows.Add(drNew);
                }
            }

            BLL.Util.ExportToCSV("坐席申诉统计", dtNew);
        }
    }
}