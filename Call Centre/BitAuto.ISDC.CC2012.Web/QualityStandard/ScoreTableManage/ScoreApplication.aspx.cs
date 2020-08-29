using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.QualityScoring.ScoreTableManage
{
    public partial class ScoreApplication : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string rangeStr = string.Empty;
        bool right_range = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                right_range = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT600205");
                if (right_range)
                {
                    bindGroupData();
                    bindData();
                }
                else
                {
                    Response.Write(@"<script language='javascript'>alert('您无权限访问该页面');javascript:$.closePopupLayer('scoreApplication',false);</script>");
                }
            }
        }

        private void bindGroupData()
        {
            DataTable dt = BLL.BusinessGroup.Instance.GetInUseBusinessGroup(BLL.Util.GetLoginUserID());//只显示在用的组
            repeaterScoreApplicationList.DataSource = dt;
            repeaterScoreApplicationList.DataBind();
        }

        private void bindData()
        {
            DataTable dt = BLL.QS_RulesRange.Instance.GetAllList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rangeStr += dt.Rows[i]["BGID"].ToString() + "$" + dt.Rows[i]["QS_RTID"].ToString() + "|";
            }
            rangeStr = rangeStr.TrimEnd('|');
        }
    }
}