using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.GroupManage
{
    public partial class GroupManageAdd : CC2012.Web.Base.PageBase
    {
        public string BGID
        {
            get
            {
                return Request.QueryString["BGID"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["BGID"].ToString());
            }
        }
        public string TitleString
        {
            get
            {
                if (BGID == "")
                {
                    return "新增分组";
                }
                else
                {
                    return "编辑分组";
                }
            }
        }
        public string CDID = "";
        public string LineIDs = "";

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024BUT500801"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                BindUIData();
                BindGroupData();
            }
        }
        /// 绑定界面上显示的数据
        /// <summary>
        /// 绑定界面上显示的数据
        /// </summary>
        private void BindUIData()
        {
            //外显
            DataTable cdt = BLL.BusinessGroup.Instance.GetCallDisplay(true);
            repeater_call.DataSource = cdt;
            repeater_call.DataBind();
            //业务线
            DataTable ldt = BLL.BusinessGroup.Instance.GetBusinessLine();
            repeater_line.DataSource = ldt;
            repeater_line.DataBind();
        }

        /// 绑定组数据
        /// <summary>
        /// 绑定组数据
        /// </summary>
        private void BindGroupData()
        {
            if (!string.IsNullOrEmpty(BGID))
            {
                int bgid = CommonFunction.ObjectToInteger(BGID);
                Entities.BusinessGroup model = BLL.BusinessGroup.Instance.GetBusinessGroupInfoByBGID(bgid);

                //名称
                txtBgName.Value = model.Name;
                //区域
                if (model.RegionID == 1)
                {
                    rdo_areaid_1.Checked = true;
                }
                else if (model.RegionID == 2)
                {
                    rdo_areaid_2.Checked = true;
                }
                //设置所属业务
                if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.CC) == (int)BusinessTypeEnum.CC)
                {
                    this.ckb_businesstype_1.Checked = true;
                }
                if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.IM) == (int)BusinessTypeEnum.IM)
                {
                    this.ckb_businesstype_2.Checked = true;
                }
                //外显号码(js赋值)
                CDID = model.CDID.ToString();
                //业务线列表(js赋值)
                LineIDs = BLL.BusinessGroup.Instance.GetBusinessLineIDs(bgid);
            }
        }
    }
}