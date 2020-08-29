using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustCheck.CrmCustCheck
{
    public partial class SecondCarView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID = "";

        CustCheckHelper helper = new CustCheckHelper();
        public string QueryParams { get { return HttpUtility.UrlEncode(helper.QueryParams); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!IsPostBack)
                    {
                        
                    }
                    CustCheckHelper h = new CustCheckHelper();
                    //int taskId = -1;
                    if (!string.IsNullOrEmpty(h.TID))
                    {
                        TID = h.TID;
                        Entities.ProjectTaskInfo task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(h.TID);
                        UCSecondCarView1.Task = task;
                    }
                    else
                    {
                        throw new Exception("无法找到此任务");
                    }
                }
            }
            catch (Exception ex)
            {
                //日志
            }
        }
    }
}