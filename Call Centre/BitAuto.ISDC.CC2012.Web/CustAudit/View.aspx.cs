using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.CustCheck;

namespace BitAuto.ISDC.CC2012.Web.CustAudit
{
    public partial class View : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CustCheckHelper h = new CustCheckHelper();
                    //int taskId = -1;
                    if (!string.IsNullOrEmpty(h.TID))
                    {
                        TID = h.TID;
                        Entities.ProjectTaskInfo task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(h.TID);
                        this.UCCust1.Task = task;
                        if (task != null)
                        {
                            switch (task.TaskStatus)
                            {
                                //case (int)Entities.EnumProjectTaskStatus.SubmitManageSuccess:
                                //case (int)Entities.EnumProjectTaskStatus.SubmitManageFail:
                                //case (int)Entities.EnumProjectTaskStatus.DelSuccess:
                                //case (int)Entities.EnumProjectTaskStatus.DelFail:
                                case (int)Entities.EnumProjectTaskStatus.StayReview:
                                case (int)Entities.EnumProjectTaskStatus.Finshed:
                                    this.btnConfirm.Visible = this.btnRefuse.Visible = this.btnCallBack.Visible = false;
                                    break;
                                case (int)Entities.EnumProjectTaskStatus.SubmitFinsih:
                                case (int)Entities.EnumProjectTaskStatus.DelFinsh:
                                    this.btnConfirm.Visible = this.btnRefuse.Visible=this.btnCallBack.Visible = true;
                                    break;
                                default:
                                    break;
                            }
                            //if (task.TaskStatus == (int)Entities.EnumProjectTaskStatus.SubmitManageFail)
                            //{
                            //    this.btnReject.Visible = true;
                            //}
                        }
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