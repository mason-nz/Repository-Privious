using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.WorkReport
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            if ("delete".Equals(context.Request["action"], StringComparison.OrdinalIgnoreCase))
            {
                DeleteWorkReport(context);
                return;
            }
            if ("isview".Equals(context.Request["action"], StringComparison.OrdinalIgnoreCase))
            {
                IsViewReport(context);
                return;
            }
            if ("updatestatus".Equals(context.Request["action"], StringComparison.OrdinalIgnoreCase))
            {
                UpdateReportStatus(context);
                return;
            }
        }

        /// 删除工作报告
        /// <summary>
        /// 删除工作报告
        /// </summary>
        /// <param name="context"></param>
        private void DeleteWorkReport(HttpContext context)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(context.Request["recid"]) && !int.TryParse(context.Request["recid"], out id))
            {
                context.Response.Write("{\"success\":0,\"message\":\"参数错误！\"}");
                return;
            }

            try
            {
                BitAuto.YanFa.Crm2009.BLL.WorkReport.Instance.Delete(id);
                context.Response.Write("{\"success\":1, \"message\":\"删除成功！\"}");
            }
            catch (Exception e)
            {
                context.Response.Write("{\"success\":0,\"message\":\"" + e.Message + "\"}");
            }
        }
        /// 检查该工作报告是否已经被查看
        /// <summary>
        /// 检查该工作报告是否已经被查看
        /// </summary>
        /// <param name="context"></param>
        private void IsViewReport(HttpContext context)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(context.Request["recid"]) && !int.TryParse(context.Request["recid"], out id))
            {
                context.Response.Write("{\"success\":0,\"message\":\"参数错误！\"}");
                return;
            }
            try
            {
                context.Response.Write("{\"success\":1, \"message\":\"" + (BitAuto.YanFa.Crm2009.BLL.WorkReport.Instance.IsView(id) ? 1 : 0) + "\"}");
            }
            catch (Exception e)
            {
                context.Response.Write("{\"success\":0,\"message\":\"" + e.Message + "\"}");
            }
        }
        /// 更新工作报告状态
        /// <summary>
        /// 更新工作报告状态
        /// </summary>
        /// <param name="context"></param>
        private void UpdateReportStatus(HttpContext context)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(context.Request["recid"]) && !int.TryParse(context.Request["recid"], out id))
            {
                context.Response.Write("{\"success\":0,\"message\":\"参数错误！\"}");
                return;
            }
            int status = 0;
            if (!string.IsNullOrEmpty(context.Request["status"])
                && !int.TryParse(context.Request["status"], out status))
            {
                context.Response.Write("{\"success\":0,\"message\":\"参数错误！\"}");
                return;
            }
            try
            {
                BitAuto.YanFa.Crm2009.Entities.WorkReport report = BitAuto.YanFa.Crm2009.BLL.WorkReport.Instance.Get(id);
                //撤销 改为 不删除 接收人
                IList<BitAuto.YanFa.Crm2009.Entities.WRRecipient> wRRecipient = YanFa.Crm2009.BLL.WRRecipient.Instance.GetRecipients(id);
                foreach (BitAuto.YanFa.Crm2009.Entities.WRRecipient item in wRRecipient)
                {
                    report.Recipients.Add(item.UserID);
                }

                if (report != null)
                {
                    report.Status = status == 1 ? BitAuto.YanFa.Crm2009.Entities.WorkReportStatus.Draft : BitAuto.YanFa.Crm2009.Entities.WorkReportStatus.Submitted;
                    BitAuto.YanFa.Crm2009.BLL.WorkReport.Instance.Save(report);
                    context.Response.Write("{\"success\":1, \"message\":\"撤回成功！\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":0,\"message\":\"该报告不存在！\"}");
                }
            }
            catch (Exception e)
            {
                context.Response.Write("{\"success\":0,\"message\":\"" + e.Message + "\"}");
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}