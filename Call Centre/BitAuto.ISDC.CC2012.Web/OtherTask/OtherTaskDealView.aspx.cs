using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.OtherTask
{
    public partial class OtherTaskDealView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string _otherTaskID;
        /// <summary>
        /// 其他任务ID
        /// </summary>
        public string OtherTaskID
        {
            get
            {
                if (string.IsNullOrEmpty(_otherTaskID))
                {
                    _otherTaskID = HttpUtility.UrlDecode((Request["OtherTaskID"] + "").Trim());
                }
                return _otherTaskID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(OtherTaskID))
                {
                    Entities.OtherTaskInfo model = BLL.OtherTaskInfo.Instance.GetOtherTaskInfo(OtherTaskID);
                    if (model != null)
                    {
                        OtherTaskEdit1.RequestTaskID = OtherTaskID;
                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>javascript:alert('该任务不存在。');try {
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