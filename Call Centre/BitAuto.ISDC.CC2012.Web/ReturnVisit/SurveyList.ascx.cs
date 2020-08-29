using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit
{
    public partial class SurveyList : System.Web.UI.UserControl
    {
        public string requestcustid;
        public string RequestCustID
        {
            get
            {
                return requestcustid;
            }
            set
            {
                requestcustid = value;
            }
        }
        public string requestprojectid;
        public string RequestProjectID
        {
            get
            {
                return requestprojectid;
            }
            set
            {
                requestprojectid = value;
            }
        }

        public string str = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (!string.IsNullOrEmpty(RequestCustID))
                {
                    DataTable dt = null;
                    dt = BLL.ProjectSurveyMapping.Instance.GetSurveyinfoByCustID(RequestCustID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int RowCount = 0;
                            QueryProjectTask_SurveyAnswer query = new QueryProjectTask_SurveyAnswer();
                            query.CustID = RequestCustID;
                            int _siid = 0;
                            if (int.TryParse(dt.Rows[i]["SIID"].ToString(), out _siid))
                            {

                            }
                            query.SIID = _siid;
                            DataTable dtnew = BLL.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(query, "", 1, 1000000, out RowCount);
                            //当前时间不在周期里，并且问卷没有提交过答案
                            DateTime _beginTime = new DateTime();
                            DateTime _endTime = new DateTime();
                            if (DateTime.TryParse(dt.Rows[i]["begindate"].ToString(), out _beginTime))
                            {

                            }
                            if (DateTime.TryParse(dt.Rows[i]["endDate"].ToString(), out _endTime))
                            {

                            }
                            //答过问卷，或者问卷当前时间在问卷周期内
                            if (RowCount > 0 || (System.DateTime.Now >= _beginTime && System.DateTime.Now <= _endTime))
                            {
                                //如果是查看
                                if (dtnew != null && dtnew.Rows.Count > 0)
                                {

                                    if (dtnew.Rows[0]["Status"] != DBNull.Value)
                                    {
                                        if (dtnew.Rows[0]["Status"].ToString() == "1")
                                        {
                                            str += "<a href='../ReturnVisit/SurveyInfo.aspx?SIID=" + dt.Rows[i]["SIID"].ToString() + "&ProJectID=" + dt.Rows[i]["ProjectID"].ToString() + "&CustID=" + RequestCustID + "&Action=view' target='_blank'>" + dt.Rows[i]["Name"].ToString() + "</a>&nbsp;&nbsp;";
                                        }
                                        else
                                        {
                                            str += "<a href='../ReturnVisit/SurveyInfo.aspx?SIID=" + dt.Rows[i]["SIID"].ToString() + "&ProJectID=" + dt.Rows[i]["ProjectID"].ToString() + "&CustID=" + RequestCustID + "' target='_blank'>" + dt.Rows[i]["Name"].ToString() + "</a>&nbsp;&nbsp;";
                                        }
                                    }
                                }
                                else
                                {
                                    str += "<a href='../ReturnVisit/SurveyInfo.aspx?SIID=" + dt.Rows[i]["SIID"].ToString() + "&ProJectID=" + dt.Rows[i]["ProjectID"].ToString() + "&CustID=" + RequestCustID + "' target='_blank'>" + dt.Rows[i]["Name"].ToString() + "</a>&nbsp;&nbsp;";
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}