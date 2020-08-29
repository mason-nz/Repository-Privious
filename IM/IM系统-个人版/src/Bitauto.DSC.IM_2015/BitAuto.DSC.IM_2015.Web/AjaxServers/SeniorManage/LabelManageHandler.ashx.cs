using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
{
    /// <summary>
    /// LabelManageHandler 的摘要说明
    /// </summary>
    public class LabelManageHandler : IHttpHandler, IRequiresSessionState
    {
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? string.Empty : HttpContext.Current.Request["Action"].ToString();
            }
        }
        public string RequestDirect
        {
            get
            {
                return HttpContext.Current.Request["Direct"] == null ? string.Empty : HttpContext.Current.Request["Direct"].ToString();
            }
        }
        public int CurrentUserId;
        public void ProcessRequest(HttpContext context)
        {
            CurrentUserId = BLL.Util.GetLoginUserID();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {                
                case "moveupordown":
                    MoveUpOrDown(out msg);
                    break;
                case "save2dbgrouplabel":
                    Save2DBGroupLabel(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        private void Save2DBGroupLabel(out string msg)
        {
            msg = string.Empty;
            try
            {
                Entities.QueryGroupLabel query = BLL.Util.BindQuery<Entities.QueryGroupLabel>(HttpContext.Current);

                BLL.GroupLabel.Instance.SaveDataBatch(query.BGID, query.LTIDS, CurrentUserId);

                msg = "{result:'yes',msg:'保存成功!'}";
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'保存失败>" + ex.Message + "'}";
            }
        }

        private void MoveUpOrDown(out string msg)
        {
            msg = string.Empty;
            int type = RequestDirect == "up" ? 1 : -1;
            try
            {
                Entities.LabelTable model = BLL.Util.BindQuery<Entities.LabelTable>(HttpContext.Current);

                Entities.LabelTable info = BLL.LabelTable.Instance.GetLabelTable(model.LTID);

                if (info != null)
                {
                    info.ModifyTime = DateTime.Now;
                    info.ModifyUserID = CurrentUserId;

                    if (BLL.LabelTable.Instance.MoveUpOrDown(model.LTID, Convert.ToInt32(info.SortNum), type))
                    {
                        msg = "{result:'yes',msg:'移动成功!'}";
                    }
                    else
                    {
                        msg = "{result:'no',msg:'移动失败!'}";
                    }
                }
                else
                {
                    msg = "{result:'no',msg:'移动失败标签不存在!'}";
                }

            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
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