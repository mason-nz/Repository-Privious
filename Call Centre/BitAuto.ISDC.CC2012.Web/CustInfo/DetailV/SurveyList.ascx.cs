using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class SurveyList : System.Web.UI.UserControl
    {
        public string requesttaskid;
        public string RequestTaskID
        {
            get
            {
                return requesttaskid;
            }
            set
            {
                requesttaskid = value;
            }
        }

        public string str = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(RequestTaskID))
                {
                    DataTable dt = null;
                    dt = BLL.ProjectSurveyMapping.Instance.GetSurveyinfoByTaskID(RequestTaskID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int RowCount = 0;
                            QueryProjectTask_SurveyAnswer query = new QueryProjectTask_SurveyAnswer();
                            query.PTID = RequestTaskID;
                            int _siid = 0;
                            if (int.TryParse(dt.Rows[i]["SIID"].ToString(), out _siid))
                            {

                            }
                            query.SIID = _siid;
                            DataTable dtnew = BLL.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(query, "", 1, 1000000, out RowCount);
                            //当前时间不在周期里，并且问卷没有提交过答案
                            DateTime _beginTime=new DateTime();
                            DateTime _endTime=new DateTime();
                            if(DateTime.TryParse(dt.Rows[i]["begindate"].ToString(),out _beginTime))
                            {

                            }
                            if(DateTime.TryParse(dt.Rows[i]["endDate"].ToString(),out _endTime))
                            {

                            }
                            //答过问卷，或者问卷当前时间在问卷周期内
                            if (RowCount>0 || (System.DateTime.Now >= _beginTime && System.DateTime.Now <= _endTime))
                            {
                                //如果是查看
                                if (Request["Action"] != null && Request["Action"] == "view")
                                {
                                    str += "<a href='../../CustInfo/DetailV/SurveyInfo.aspx?SIID=" + dt.Rows[i]["SIID"].ToString() + "&ProJectID=" + dt.Rows[i]["ProjectID"].ToString() + "&TaskID=" + dt.Rows[i]["PTID"].ToString() + "&Action=view' target='_blank'>" + dt.Rows[i]["Name"].ToString() + "</a>&nbsp;&nbsp;";
                                }
                                else
                                {
                                    str += "<a href='../../CustInfo/DetailV/SurveyInfo.aspx?SIID=" + dt.Rows[i]["SIID"].ToString() + "&ProJectID=" + dt.Rows[i]["ProjectID"].ToString() + "&TaskID=" + dt.Rows[i]["PTID"].ToString() + "' target='_blank'>" + dt.Rows[i]["Name"].ToString() + "</a>&nbsp;&nbsp;";
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}