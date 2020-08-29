using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ApplyRange
{
    public partial class ApplyRangeList : PageBase
    {
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD6201"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                BindData();
            }
        }

        private void BindData()
        {
            DataTable dt = BLL.QS_RulesRange.Instance.GetQS_RulesRange(new Entities.QueryQS_RulesRange(), "", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 1);
            }
        }
        /// 返回业务分类
        /// <summary>
        /// 返回业务分类
        /// </summary>
        /// <param name="businesstype"></param>
        /// <returns></returns>
        public string GetBusinessType(string businesstype)
        {
            return BLL.Util.GetMutilEnumDataNames(CommonFunction.ObjectToInteger(businesstype), typeof(BusinessTypeEnum));
        }
        /// 返回操作按钮
        /// <summary>
        /// 返回操作按钮
        /// </summary>
        /// <param name="bgid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetOptionLink(string bgid, string btype)
        {
            string link = "";
            //编辑
            link += "<a href=\"javascript:void(0);\" onclick=\"On_Modify('" + bgid + "','" + btype + "')\">编辑</a>";
            return link;
        }
        /// 权限控制
        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="btype"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetQSRTName(string bgid, string btype, string name, string type, string rtid)
        {
            //ly:录音  hh:会话
            if (((btype == "1" || btype == "3") && type == "ly")
                ||
                ((btype == "2" || btype == "3") && type == "hh"))
            {
                //有权限
                if (name == "")
                {
                    name = "无";
                    rtid = "-1";
                }
                else
                {
                    name = "<a href=\"/QualityStandard/ScoreTableManage/ScoreTableView.aspx?QS_RTID=" + rtid + "\" target=\"_blank\">" + name + "</a>";
                }
                return "<span id=\"span_" + type + "_" + bgid + "\">" + name + "</span>" +
                        "<select id=\"select_" + type + "_" + bgid + "\" style=\"line-height: 22px; display:none;\"></select>" +
                        "<input id=\"hidden_" + type + "_" + bgid + "\" type='hidden' value='" + rtid + "'>";
            }
            else
            {
                //无权限
                return "-";
            }
        }
    }
}