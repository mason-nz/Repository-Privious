﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage
{
    public partial class ContentsDetailForm : System.Web.UI.Page
    {
        private string RecID
        {
            get
            {
                return HttpContext.Current.Request["RecID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (string.IsNullOrEmpty(RecID))
                {
                }
                else
                {

                    int recid;
                    if (int.TryParse(RecID, out recid))
                    {
                        int RecordCount = 0;
                        QueryUserMessage query = new QueryUserMessage();
                        query.RecID = recid;
                        query.UserID = BLL.Util.GetLoginUserID();
                        DataTable dt = BLL.UserMessage.Instance.GetUserMessage(query, "um.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
                        if (dt != null && dt.Rows.Count>0)
                        {
                            taContents.Value = dt.Rows[0]["Content"].ToString();
                        }
                    }

                }
            }
        }
    }
}