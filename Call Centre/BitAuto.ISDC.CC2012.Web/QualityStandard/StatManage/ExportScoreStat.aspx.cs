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
    public partial class ExportScoreStat : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 参数
        private string RequestUserID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("UserID")); }
        }
        private string RequestGroupID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("GroupID")); }
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
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024MOD6301"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
            else
            {
                BindScoreData();
            }
        }

        private void BindScoreData()
        {
            QueryQS_ScoreStat query = new QueryQS_ScoreStat();
            query.RequestUserID = RequestUserID;
            query.RequestGroupID = RequestGroupID;
          
            query.RequestRecordBeginTime = RequestRecordBeginTime;
            query.RequestRecordEndTime = RequestRecordEndTime;

            int totalCount = 0;
            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultScoreStat(query, "", 1, -1, tableEndName, out totalCount);

            DataTable dtNew = new DataTable();
            DataColumn dcEmployeeName = new DataColumn("坐席");
            DataColumn dcRulesTable = new DataColumn("评分表");
            DataColumn dcSumCount = new DataColumn("质检次数");
            DataColumn dcStatCount = new DataColumn("合格量");
            DataColumn dcRate = new DataColumn("合格率");
            DataColumn dcAvg = new DataColumn("平均分");
            dtNew.Columns.Add(dcEmployeeName);
            dtNew.Columns.Add(dcRulesTable);
            dtNew.Columns.Add(dcSumCount);
            dtNew.Columns.Add(dcStatCount);
            dtNew.Columns.Add(dcRate);
            dtNew.Columns.Add(dcAvg);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    //坐席名称
                    drNew[0] = dr["TrueName"].ToString();
                    //评分表名称
                    drNew[1] = dr["Name"].ToString();
                    //质检次数
                    drNew[2] = dr["SumCount"].ToString();

                    //质检评分
                    if (dr["ScoreType"].ToString() != "2")
                    {
                        //float result = float.Parse(dr["statCount"].ToString()) / float.Parse(dr["sumCount"].ToString());
                       drNew[3] =  "-";
                        drNew[4] = "-";
                        if (dr["totScore"].ToString() != null && dr["totScore"] != null && dr["sumCount"].ToString() != "0")
                        {
                            drNew[5] =  (float.Parse(dr["totScore"].ToString())/ float.Parse(dr["sumCount"].ToString())).ToString("0.00");
                        }
                        else
                        {
                            drNew[5] = "0.00";
                        }                         
                    }
                    else
                    {
                        drNew[3] = dr["statCount"].ToString();
                        float result = float.Parse(dr["statCount"].ToString()) * 100 / float.Parse(dr["sumCount"].ToString());
                        drNew[4] = result.ToString("0.00") + "%";
                        drNew[5] = "-";
                    }                                         
                    
                    dtNew.Rows.Add(drNew);
                }
            }

            BLL.Util.ExportToCSV("坐席成绩统计", dtNew);
        }
    }
}