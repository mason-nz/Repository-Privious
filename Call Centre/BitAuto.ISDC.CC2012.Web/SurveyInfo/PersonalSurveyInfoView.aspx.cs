using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo
{
    public partial class PersonalSurveyInfoView : Base.PageBase
    {
        //项目名称
        public string SurveyName = "";
        //调查时间
        public string SurveyTime = "";
        //调查对象名称
        public string UserName = "";
        private string siid = "";
        /// <summary>
        /// 取调查问卷ID
        /// </summary>
        public string RequestSIID
        {
            get
            {
                return siid;
            }
            set
            {
                siid = value;
            }
        }
        /// <summary>
        /// 取调查项目ID
        /// </summary>
        public string RequestSPIID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["SPIID"]))
                {
                    return "";
                }
                else
                {
                    return Request["SPIID"];
                }
            }
        }
        //调查人
        public string RequestUserID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["UserID"]))
                {
                    return "";
                }
                else
                {
                    return Request["UserID"];
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                bool flag = true;
                //判断当前登录人是否有查看当前问卷的权限
                flag = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT3301");
                if (flag == false)
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('您没有查看该问卷的权限');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
                else
                {

                    if (!string.IsNullOrEmpty(RequestSPIID))
                    {
                        //取项目信息
                        Entities.SurveyProjectInfo Model = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(Convert.ToInt32(RequestSPIID));
                        if (Model != null)
                        {
                            //取试卷id
                            RequestSIID = Model.SIID.ToString();

                            UCSurveyInfoEditID.RequestSPIID = RequestSPIID;
                            UCSurveyInfoEditID.RequestSIID = RequestSIID;
                            UCSurveyInfoEditID.RequestUserID = RequestUserID;

                            SurveyName = Model.Name;
                            if (!string.IsNullOrEmpty(RequestUserID))
                            {
                                UserName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(RequestUserID));
                            }

                            if (!string.IsNullOrEmpty(RequestSPIID) && !string.IsNullOrEmpty(RequestSIID) && !string.IsNullOrEmpty(RequestUserID))
                            {
                                Entities.QuerySurveyAnswer query = new Entities.QuerySurveyAnswer();
                                query.SPIID = Convert.ToInt32(RequestSPIID);
                                query.SIID = Convert.ToInt32(RequestSIID);
                                query.CreateUserID = Convert.ToInt32(RequestUserID);
                                int allcount = 0;
                                DataTable dt = BLL.SurveyAnswer.Instance.GetSurveyAnswer(query, "", 1, 100000, out allcount);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["CreateTime"] != DBNull.Value)
                                    {
                                        if (dt.Rows[0]["CreateTime"].ToString() != "")
                                        {
                                            SurveyTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Response.Write(@"<script language='javascript'>javascript:alert('调查问卷不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");


                            }

                        }
                        else
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('调查问卷不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                        }
                    }
                }
            }
        }
    }
}