using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit
{
    public partial class SurveyInfo : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性

        public string RequestSIID
        {
            get { return HttpContext.Current.Request["SIID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SIID"].ToString()); }
        }
        public string RequestProjectID
        {
            get { return HttpContext.Current.Request["ProjectID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProjectID"].ToString()); }
        }
        //客户ID
        public string RequestCustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }

        #endregion
        //str += "<a href='SurveyInfo.aspx?SIID='" + dt.Rows[i]["SIID"].ToString() + "'+'&ProJectID='" + dt.Rows[i]["ProjectID"].ToString() + "'+'&TaskID='" + dt.Rows[i]["PTID"].ToString() + "' target='_blank'>" + dt.Rows[i]["Name"].ToString() + "</a>&nbsp;&nbsp;";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                //int _projectid;
                //if (int.TryParse(RequestProjectID, out _projectid))
                //{
                //后续需要判断任务状态，不是所有状态都可以答问卷,任务在处理中，未处理加载可编辑问卷，其他状态加载只能查看
                if (string.IsNullOrEmpty(RequestCustID))
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('回访客户不存在');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
                //else if (string.IsNullOrEmpty(RequestSIID))
                //{
                //    Response.Write("<script language='javascript'>alert('核实问卷不存在！');closePage();</script>");
                //}
                else
                {
                    if (!string.IsNullOrEmpty(RequestSIID))
                    {
                        //Entities.ProjectTaskInfo Taskinfo = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(RequestCustID);

                        Entities.QueryProjectTask_SurveyAnswer querey = new Entities.QueryProjectTask_SurveyAnswer();
                        querey.CustID = RequestCustID;

                        int _siid;
                        if (int.TryParse(RequestSIID, out _siid))
                        {

                        }
                        querey.SIID = _siid;
                        int rowcount = 0;
                        DataTable dt = BLL.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(querey, "", 1, 1000000, out rowcount);


                        if (Request["Action"] != null && Request["Action"] == "view")
                        {
                            UCSurveyInfoEdit1.Visible = false;
                            UCSurveyInfoView1.Visible = true;
                            UCSurveyInfoView1.RequestSIID = RequestSIID;
                            UCSurveyInfoView1.RequestCustID = RequestCustID;
                            btnsave.Visible = false;
                            btnsub.Visible = false;
                        }
                        else if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["status"] != DBNull.Value && dt.Rows[0]["status"].ToString() == "1")
                            {
                                UCSurveyInfoEdit1.Visible = false;
                                UCSurveyInfoView1.Visible = true;
                                UCSurveyInfoView1.RequestSIID = RequestSIID;
                                UCSurveyInfoView1.RequestCustID = RequestCustID;
                                btnsave.Visible = false;
                                btnsub.Visible = false;
                            }
                            else
                            {
                                UCSurveyInfoEdit1.Visible = true;
                                UCSurveyInfoView1.Visible = false;
                                UCSurveyInfoEdit1.RequestSIID = RequestSIID;
                                UCSurveyInfoEdit1.RequestCustID = RequestCustID;
                                btnsave.Visible = true;
                                btnsub.Visible = true;
                            }
                        }
                        else
                        {

                            UCSurveyInfoEdit1.Visible = true;
                            UCSurveyInfoView1.Visible = false;
                            UCSurveyInfoEdit1.RequestSIID = RequestSIID;
                            UCSurveyInfoEdit1.RequestCustID = RequestCustID;
                            btnsave.Visible = true;
                            btnsub.Visible = true;

                        }
                    }


                }
                //}
                //else
                //{
                //    Response.Write("<script language='javascript'>alert('核实项目id数据格式不正确！');closePage();</script>");
                //}
            }
        }
    }
}