using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.QuestionnaireSurvey
{
    public partial class TakingAnSurvey :PageBase
    {
        #region 属性

        public string RequestSPIID
        {
            get { return HttpContext.Current.Request["SPIID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SPIID"].ToString()); }
        }
        public string _siid;
        public string RequestSIID
        {
            get { return _siid; }
            set { _siid = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                int _spiid;
                if (int.TryParse(RequestSPIID, out _spiid))
                {
                    Entities.SurveyProjectInfo model = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(_spiid);
                    if (model != null)
                    {
                        RequestSIID = model.SIID.ToString();
                        UCSurveyInfoShow1.SIID = model.SIID.ToString();
                        spanProjectName.InnerHtml = model.Name;
                        spanProjectDesc.InnerHtml = model.Description;
                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>alert('没有找到该问卷项目！');try {
        window.external.MethodScript('/browsercontrol/closepage');
    } catch (e) {
        window.opener = null; window.open('', '_self'); window.close();
    }</script>");
                    }
                }
            }
        }

    }
}