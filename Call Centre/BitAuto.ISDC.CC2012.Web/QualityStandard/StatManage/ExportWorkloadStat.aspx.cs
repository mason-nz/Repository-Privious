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
    public partial class ExportWorkloadStat : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 参数
        private string RequestTrueName
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("TrueName")); }
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
        private string RequestCreateUserID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("CreateUserID")); }
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
                if (!BLL.Util.CheckRight(userId, "SYS024MOD6303"))
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
            if (int.TryParse(RequestCreateUserID, out userId) && userId > 0)
            {
                where += " And qsr.CreateUserID=" + userId;
            }
            int groupId = 0;
            if (int.TryParse(RequestGroupID, out groupId) & groupId > 0)
            {
                where += " And cri.CreateUserID IN (SELECT userid FROM UserGroupDataRigth WHERE BGID=" + groupId + " )";
            }
            DateTime beginTime;
            DateTime endTime;
            if (DateTime.TryParse(RequestBeginTime, out beginTime))
            {
                where += " And qsr.CreateTime>='" + beginTime.ToString() + "'";
            }
            if (DateTime.TryParse(RequestEndTime, out endTime))
            {
                //where += " And qsr.CreateTime>='" + endTime.ToString() + "'";
                where += " And qsr.CreateTime<='" + endTime.Add(new TimeSpan(23, 59, 59)).ToString() + "'";
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
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultWorkloadStat(where, "", 1, -1, tableEndName, out totalCount);

            DataTable dtNew = new DataTable();
            DataColumn dcEmployeeName = new DataColumn("评分人");
            DataColumn dcSumCount = new DataColumn("评分次数");
            DataColumn dcTotalTallTime = new DataColumn("质检录音总时长");
            DataColumn dcAppealCount = new DataColumn("被申诉总量");
            DataColumn dcSuccessAppealCount = new DataColumn("被申诉成功量");
            DataColumn dcRate = new DataColumn("被申诉成功率");
            dtNew.Columns.Add(dcEmployeeName);
            dtNew.Columns.Add(dcSumCount);
            dtNew.Columns.Add(dcTotalTallTime);
            dtNew.Columns.Add(dcAppealCount);
            dtNew.Columns.Add(dcSuccessAppealCount);
            dtNew.Columns.Add(dcRate);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    //坐席名称
                    drNew[0] = dr["TrueName"].ToString();
                    drNew[1] = dr["SumCount"].ToString();
                    drNew[2] = dr["TotalTallTime"].ToString();
                    //申诉总量
                    drNew[3] = dr["AppealCount"].ToString();
                    //申诉成功量
                    drNew[4] = dr["AppealSuccessCount"].ToString();

                    //申诉成功率
                    if (dr["AppealCount"].ToString() != "0")
                    {
                        drNew[5] = (float.Parse(dr["AppealSuccessCount"].ToString()) * 100 / float.Parse(dr["AppealCount"].ToString())).ToString("0.00") + "%";
                    }
                    else
                    {
                        drNew[5] = "0%";
                    }
                    dtNew.Rows.Add(drNew);
                }
            }

            BLL.Util.ExportToCSV("工作量统计", dtNew);
        }
    }
}