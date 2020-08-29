using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
namespace BitAuto.ISDC.CC2012.Web.SurveyInfo
{
    public partial class SurveyInfoView : Base.PageBase
    {
        public string SurveyName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                if (!string.IsNullOrEmpty(Request["SIID"]))
                {


                    Entities.SurveyInfo Model = BLL.SurveyInfo.Instance.GetSurveyInfo(Convert.ToInt32(Request["SIID"]));

                    if (Model != null)
                    {
                        bool flag = true;
                        int userId=BLL.Util.GetLoginUserID();
                        //判断当前登录人是否有查看当前问卷的权限
                        flag = BLL.SurveyInfo.Instance.HaveRight(Model.BGID.ToString(),userId , "SYS024BUT5008");
                        if (Model.CreateUserID == userId)
                        {
                            flag = true;
                        }
                        if (flag == false)
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('您没有查看该问卷的权限！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");

                        }
                        else
                        {
                            SurveyName = Model.Name;
                            SurveyInfoViewID.SIID = Request["SIID"];

                        }
                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>javascript:alert('问卷不存在！');try {
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