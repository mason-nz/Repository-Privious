using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class AddOkPanel : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        public string returnProjectID
        {
            get
            {
                return HttpContext.Current.Request["returnProjectID"]==null?"":
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["returnProjectID"].ToString());
            }
        }
        public string Source
        {
            get
            {
                return HttpContext.Current.Request["Source"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Source"].ToString());
            }
        }
        //public string AddIDcount
        //{
        //    get
        //    {
        //        return HttpContext.Current.Request["AddIDcount"] == null ? "" :
        //            HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["AddIDcount"].ToString());
        //    }
        //}

        public string AddIDcount = "0";

        public string status
        {
            get
            {
                return HttpContext.Current.Request["status"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["status"].ToString());
            }
        }
        public string RecCount
        {
            get
            {
                return HttpContext.Current.Request["RecCount"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["RecCount"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        { 
            int intval = 0;
            if (int.TryParse(returnProjectID, out intval))
            {
                Entities.QueryProjectDataSoure dsQuery = new Entities.QueryProjectDataSoure();
                dsQuery.ProjectID = intval;
                dsQuery.Status = 0;

                int totalCount = 0;
                DataTable dt = BLL.ProjectDataSoure.Instance.GetProjectDataSoure(dsQuery, "", 1, 99999999, out totalCount);
                //
                AddIDcount = dt.Rows.Count.ToString();
            }
        }
    }
}