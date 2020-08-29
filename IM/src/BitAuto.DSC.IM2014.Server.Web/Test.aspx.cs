using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userid = "87a19baf-955f-41a6-8f93-95d099706256";
            Regex reg = new Regex("[A-Za-z0-9]{8}(-[A-Za-z0-9]{4}){3}-[A-Za-z0-9]{12}");
            bool boolguid = reg.IsMatch(userid);

            System.Text.RegularExpressions.Regex.IsMatch(userid, @"^[A-Z0-9]{8}(-[A-Z0-9]{4}){3}-[A-Z0-9]{12}$");
            string st = "http://ncc.sys1.bitauto.com/ReturnVisit/ReturnVisitRecordView.aspx?RVID={0}aa={1}";
            string.Format(st, "111");
            string a = "";
            a = string.Format(st, "111");
        }
    }
}