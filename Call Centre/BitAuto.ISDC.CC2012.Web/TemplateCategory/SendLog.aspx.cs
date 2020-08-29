using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    public partial class SendLog : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        int page;
        protected void Page_Load(object sender, EventArgs e)
        {
            bindRepter(); 
            
        }

        protected void typeChange(object sender,EventArgs e)
        {
            bindRepter();
        }

        //列表数据绑定
        protected void bindRepter()
        {
            //Response.Write(RB_EMail.Checked.ToString());
            if (Request["page"] != null)
            {
                page = Convert.ToInt32(Request["page"]);
            }
            else
            {
                page = 1;
            }

            int pageCount = 0;
            if (RB_EMail.Checked)
            {//邮件发送日志数据绑定     
                QuerySendEmailLog query = new QuerySendEmailLog();
                Rpt_LogList.DataSource = BLL.SendEmailLog.Instance.GetSendEmailLog(query,"SendTime desc",page,10,out pageCount);
                
            }
            else
            {//短信发送日志数据绑定
                QuerySendSMSLog query = new QuerySendSMSLog();
                Rpt_LogList.DataSource = BLL.SendSMSLog.Instance.GetSendSMSLog(query, "SendTime desc", page, 10, out pageCount);
            }
            Rpt_LogList.DataBind();
            Ltr_page.Text = PageCommon.Instance.LinkString(pageCount);
        }

    }
}