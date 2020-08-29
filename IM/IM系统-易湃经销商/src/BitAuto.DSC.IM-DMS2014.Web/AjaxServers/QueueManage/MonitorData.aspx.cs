using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.QueueManage
{
    public partial class MonitorData : System.Web.UI.Page
    {
        private string MemberName
        {
            get
            {
                return HttpContext.Current.Request["MemberName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MemberName"].ToString());
            }
        }
        private string District
        {
            get
            {
                return HttpContext.Current.Request["District"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["District"].ToString());
            }
        }
        private string CityGroup
        {
            get
            {
                return HttpContext.Current.Request["CityGroup"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CityGroup"].ToString());
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
        private string CountyID
        {
            get
            {
                return HttpContext.Current.Request["CountyID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CountyID"].ToString());
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
            if (MemberName != "")
            {
                strwhere += " AND d.MemberName like '%"+StringHelper.SqlFilter(MemberName)+"%'";
            }
            if (District != "" && District != "-1")
            {
                strwhere += " AND  d.District='" + StringHelper.SqlFilter(District) + "'";
            }
            if (CityGroup != "" && CityGroup != "-1") 
            {
                strwhere += " AND  d.CityGroup='" + StringHelper.SqlFilter(CityGroup) + "'";
            }
            if (ProvinceID != "" && ProvinceID != "-1")
            {
                strwhere += " AND  d.ProvinceID='" + StringHelper.SqlFilter(ProvinceID) + "'";
            }
            if (CityID != "" && CityID != "-1" && CityID != "城市")
            {
                strwhere += " AND  d.CityID='" + StringHelper.SqlFilter(CityID) + "'";
            }
            if (CountyID != "" && CountyID != "-1" && CountyID != "区县")
            {
                strwhere += " AND  d.CountyID='" + StringHelper.SqlFilter(CountyID) + "'";
            }
            if (UserName != "" )
            {
                strwhere += " AND b.TrueName like '%" + StringHelper.SqlFilter(UserName) + "%'";  //根据存储过程确认该参数
            }
            DateTime retdate;
            if (QueryStarttime != "")
            {
                if (DateTime.TryParse(QueryStarttime, out retdate))
                {
                strwhere += " AND  a.CreateTime>='" + StringHelper.SqlFilter(Convert.ToDateTime(QueryStarttime).ToString()) + "'";
                }
            }
            if (QueryEndTime != "")
            {
                if (DateTime.TryParse(QueryEndTime, out retdate))
                {
                    strwhere += " AND  a.CreateTime<'" + StringHelper.SqlFilter(Convert.ToDateTime(QueryEndTime).AddDays(1).ToString()) + "'";
                }
            }

            Rt_CSData.DataSource = BLL.Conversations.Instance.GetConversationingCSData(strwhere);
            Rt_CSData.DataBind();
 
        }
    }
}