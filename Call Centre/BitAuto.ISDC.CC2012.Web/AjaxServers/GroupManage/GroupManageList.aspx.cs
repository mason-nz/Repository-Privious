using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.GroupManage
{
    public partial class GroupManageList : PageBase
    {
        public bool AddAuth = false;
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        public string BusinessType
        {
            get
            {
                return Request.QueryString["businesstype"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["businesstype"].ToString());
            }
        }
        public string Status
        {
            get
            {
                return Request.QueryString["status"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["status"].ToString());
            }
        }
        public string Region
        {
            get
            {
                return Request.QueryString["region"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["region"].ToString());
            }
        }

        public int PageIndex
        {
            get
            {
                return CommonFunction.ObjectToInteger(BLL.PageCommon.Instance.PageIndex, 1);
            }
        }

        public string getCurrentPage()
        {
            return BLL.PageCommon.Instance.PageIndex.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD500801"))//"分类管理"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                AddAuth = BLL.Util.CheckRight(userId, "SYS024BUT500801");//"分类管理"功能验证逻辑
                BindData();
            }
        }

        private void BindData()
        {
            QueryBusinessGroup query = new QueryBusinessGroup();
            query.BusinessType = StringHelper.SqlFilter(this.BusinessType);
            query.Status = StringHelper.SqlFilter(this.Status);
            query.Region = StringHelper.SqlFilter(this.Region);

            DataTable dt = BLL.BusinessGroup.Instance.GetBusinessGroup(query, "bg.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);

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
        /// 获取区域名称
        /// <summary>
        /// 获取区域名称
        /// </summary>
        /// <param name="regionid"></param>
        /// <returns></returns>
        public string GetRegionName(string regionid)
        {
            return BLL.Util.GetEnumOptText(typeof(RegionID), CommonFunction.ObjectToInteger(regionid));
        }
        /// 获取状态名称
        /// <summary>
        /// 获取状态名称
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetStatusName(string status)
        {
            return status == "0" ? "在用" : "停用";
        }
        /// 返回操作按钮
        /// <summary>
        /// 返回操作按钮
        /// </summary>
        /// <param name="bgid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetOptionLink(string bgid, string status)
        {
            string link = "";
            if (status == "0")
            {
                //修改
                link += "<a href=\"javascript:void(0);\" onclick=\"On_Modify('" + bgid + "')\">修改</a>&nbsp;&nbsp;";
                //停用
                link += "<a href=\"javascript:void(0);\" onclick=\"On_ChangeStatus('" + bgid + "','1')\">停用</a>";
            }
            else if (status == "1")
            {
                //启用
                link += "<a href=\"javascript:void(0);\" onclick=\"On_ChangeStatus('" + bgid + "','0')\">启用</a>";
            }
            return link;
        }
    }
}