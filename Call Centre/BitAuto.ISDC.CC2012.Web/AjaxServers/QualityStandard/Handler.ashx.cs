using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        #region 属性定义
        private string RequestAction
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Action")); }
        }
        private string RequestQS_RID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QS_RID")); }
        }
        private string RequestIsReject
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("IsReject")); }
        }
        private string RequestRemark
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Remark")); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (RequestAction.ToLower())
            {
                case "appeal":
                    //申诉
                    Dispose(QSResultStatus.TobeFirstInstance,QSApprovalType.ScoreAppeal,false, out msg);
                    break;
                case "firstaudit":
                    if (RequestIsReject == "yes")
                    {
                        //初审拒绝
                        Dispose(QSResultStatus.Claimed, QSApprovalType.AppealFirstAduit, true, out msg);
                    }
                    else
                    {
                        //初审通过
                        Dispose(QSResultStatus.TobeReviewInstance, QSApprovalType.AppealFirstAduit, false, out msg);
                    }
                    break;
                case "auditagain":
                    if (RequestIsReject == "yes")
                    {
                        //复审拒绝
                        Dispose(QSResultStatus.Claimed, QSApprovalType.AppealAgainAduit, true, out msg);
                    }
                    else
                    {
                        //复审通过
                        Dispose(QSResultStatus.Claimed, QSApprovalType.AppealAgainAduit, false, out msg);
                    }
                    break;
            }
            context.Response.Write(msg);
        }

        public void Dispose(Entities.QSResultStatus status, Entities.QSApprovalType type, bool IsReject, out string msg)
        {
            msg = string.Empty;
            int rid = 0;
            
            if (int.TryParse(RequestQS_RID, out rid))
            {
               Entities.QS_Result info = BLL.QS_Result.Instance.GetQS_Result(rid);
               if (info != null)
               {
                   int userId = BLL.Util.GetLoginUserID();
                   info.Status = (int)status;
                   info.ModifyTime = DateTime.Now;
                   info.ModifyUserID = userId;
                  
                   Entities.QS_ApprovalHistory model = new QS_ApprovalHistory();
                   model.CreateTime = DateTime.Now;
                   model.CreateUserID = userId;
                   model.QS_RID = rid;
                   model.QS_RTID = info.QS_RTID;
                   model.Remark = RequestRemark;
                   model.Status = (int)status;
                   model.ApprovalType = ((int)type).ToString();
                   if (type != QSApprovalType.ScoreAppeal)
                   {
                       if (IsReject)
                       {
                           model.ApprovalResult = 2;
                           info.StateResult = 2;
                       }
                       else
                       {
                           model.ApprovalResult = 1;
                           info.StateResult = 1;
                       }
                   }
                   model.Type = "1";
                   BLL.QS_Result.Instance.Update(info);
                   BLL.QS_ApprovalHistory.Instance.Insert(model);
                   msg = "success";
               }
               else
               {
                   msg = "不存在此评分";
               }
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