using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
{
    public partial class LabelConfigList : System.Web.UI.Page
    {
        private string RequestBGID
        {
            get { return HttpContext.Current.Request["BGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"].ToString()); }
        }
        public int MinLTID = 0;
        public int MaxLTID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }        

        private void BindData()
        {
            DataTable dt = null;
             int region ;
             if (int.TryParse(BLL.BaseData.Instance.GetAgentRegionByUserID(BLL.Util.GetLoginUserID().ToString()), out region))
             {
                 //数据权限：所属区域
                 dt = BLL.LabelTable.Instance.GetLabelTableByBGID(Convert.ToInt32(RequestBGID), region);
                 if (dt.Rows.Count > 0)
                 {
                     MinLTID = CommonFunc.ObjectToInteger(dt.Rows[0]["LTID"]);
                     MaxLTID = CommonFunc.ObjectToInteger(dt.Rows[dt.Rows.Count - 1]["LTID"]);
                 }
                 repeaterConfig.DataSource = dt;
                 repeaterConfig.DataBind();
             }
             else
             {
                 Response.Write("获取您的所属区域时出现数据异常，请联系管理员！");
                 Response.End();
             }
            
        }
    }
}