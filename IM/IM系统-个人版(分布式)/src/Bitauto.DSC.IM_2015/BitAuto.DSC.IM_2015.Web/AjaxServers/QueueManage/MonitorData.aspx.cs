using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.QueueManage
{
    public partial class MonitorData : System.Web.UI.Page
    {
        private string VisitorName
        {
            get
            {
                return HttpContext.Current.Request["VisitorName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["VisitorName"].ToString());
            }
        }
        private string ProvinceID
        {
            get
            {
                return HttpContext.Current.Request["ProvinceID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ProvinceID"].ToString());
            }
        }
        private string CityID
        {
            get
            {
                return HttpContext.Current.Request["CityID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CityID"].ToString());
            }
        }
        private string SourceTypeValue
        {
            get
            {
                return HttpContext.Current.Request["SourceTypeValue"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SourceTypeValue"].ToString());
            }
        }
        private string QueryStarttime
        {
            get
            {
                return HttpContext.Current.Request["QueryStarttime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStarttime"].ToString());
            }
        }
        private string QueryEndTime
        {
            get
            {
                return HttpContext.Current.Request["QueryEndTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryEndTime"].ToString());
            }
        }
        private string UserName
        {
            get
            {
                return HttpContext.Current.Request["UserName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UserName"].ToString());
            }
        }
        private string BusinessGroup
        {
            get
            {
                return HttpContext.Current.Request["BusinessGroup"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["BusinessGroup"].ToString());
            }
        }
 

        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            string strwhere = "";
            if (VisitorName != "")
            {
                strwhere += " AND b.UserName LIKE '%"+StringHelper.SqlFilter(VisitorName)+"%'";
            }
            if (ProvinceID != "" && ProvinceID != "-1")
            {
                strwhere += " AND b.ProvinceID = '"+StringHelper.SqlFilter(ProvinceID)+"'";
            }
            if (CityID != "" && CityID != "-1" && CityID != "城市")
            {
                strwhere += " AND b.CityID ='" + StringHelper.SqlFilter(CityID) + "'";
            }
            if (SourceTypeValue != "" && SourceTypeValue != "-1" )
            {
                strwhere += " AND  b.SourceType='" + StringHelper.SqlFilter(SourceTypeValue) + "'";
            }
            
            DateTime retdate;
            if (QueryStarttime != "")
            {
                if (DateTime.TryParse(QueryStarttime, out retdate))
                {
                    strwhere += " AND  a.CreateTime>='" + retdate.ToString() + "'";
                }
            }
            if (QueryEndTime != "")
            {
                if (DateTime.TryParse(QueryEndTime, out retdate))
                {
                    strwhere += " AND a.CreateTime<='" + retdate.ToString() + "'";
                }
            }
            if (UserName != "" )
            {
                strwhere += " AND d.TrueName LIKE '%" + StringHelper.SqlFilter(UserName) + "%'";  //根据存储过程确认该参数
            }
            if(BusinessGroup != "")
            {
                strwhere += " AND c.BGID in (" + StringHelper.SqlFilter(BusinessGroup) + ")";
            }
            try
            {
                Rt_CSData.DataSource = BLL.Conversations.Instance.GetConversationingCSData(strwhere);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
            }
            Rt_CSData.DataBind();
   
        }

        public string GetSourceTypeName(string SourceTypeValue)
        {
            return BLL.Util.GetSourceTypeName(SourceTypeValue);
        }
    }
}