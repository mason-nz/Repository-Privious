using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
 

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage
{
    public partial class AddRemarkForm : System.Web.UI.Page
    {
        public string RecID
        {
            get
            {
                return HttpContext.Current.Request["RecID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString());
            }
        }
        public string CType
        {
            get
            {
                return HttpContext.Current.Request["CType"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CType"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (CType)
                {
                    case "add":
                        GetLoginUserName();
                        break;
                    case "upd": UpdateModel();
                        break;
                    case "sel":
                        GetModel();
                        taRemarkContents.Disabled = true; 
                        saveremarinfBT.Disabled = true; saveremarinfBT.Visible = false;
                        break;
                    default: break;
                }
            }
        }
        public void GetLoginUserName()
        {
            txtAddRemarkUser.Value =BLL.UserMessage.Instance.GetUserNameByUserID(BLL.Util.GetLoginUserID());
        }
        public void UpdateModel()
        {
            int recid;
            if (int.TryParse(RecID, out recid))
            {
                UserMessage model = new UserMessage();
                model = BLL.UserMessage.Instance.GetModel(int.Parse(RecID));
                if (model.RemarkUserID != null)
                {
                    txtAddRemarkUser.Value = BLL.UserMessage.Instance.GetUserNameByUserID(model.RemarkUserID.Value);
                }
                taRemarkContents.InnerText = model.Remarks;
            }
            else
            {
                taRemarkContents.Disabled = true;
                saveremarinfBT.Disabled = true; 
                saveremarinfBT.Visible = false;
            }

        }
        public void GetModel()
        {
            int recid;
            if (int.TryParse(RecID, out recid))
            {
                UserMessage model = new UserMessage();
                model = BLL.UserMessage.Instance.GetModel(int.Parse(RecID));
                if (model.RemarkUserID != null)
                {
                    txtAddRemarkUser.Value = BLL.UserMessage.Instance.GetUserNameByUserID(model.RemarkUserID.Value);
                }
                taRemarkContents.InnerText = model.Remarks;
                taRemarkContents.Disabled = true;
                saveremarinfBT.Disabled = true;
                saveremarinfBT.Visible = false;
            }
            else
            {
                taRemarkContents.Disabled = true;
                saveremarinfBT.Disabled = true;
                saveremarinfBT.Visible = false;
            }

        }
    }
}