using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class ContactInfoList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int PageSize
        {
            get
            {
                return this.AjaxPager_Contact.PageSize;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();

                //增加 客户联系人的页面权限
                //int userId = BLL.Util.GetLoginUserID();
                //if (BLL.Util.CheckRight(userId, "SYS024BUT2201"))
                //{
                //    BindData();
                //}
                //else
                //{
                //    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                //    Response.End();
                //}
            }
        }

        CustInfoHelper ch = new CustInfoHelper();

        private void BindData()
        {
            //查询
            int totalCount = 0;
            BitAuto.YanFa.Crm2009.Entities.QueryContactInfo qci = new BitAuto.YanFa.Crm2009.Entities.QueryContactInfo();
            qci.CustID = ch.CustID;
            DataTable table = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfo(qci, "ModifyTime DESC", ch.CurrentPage, PageSize, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater_Contact.DataSource = table;
            }
            //绑定列表数据
            repeater_Contact.DataBind();

            //分页控件 
            AjaxPager_Contact.PageSize = 5;
            AjaxPager_Contact.InitPager(totalCount); 
        }
        /// <summary>
        /// 根据contactID 获取负责会员名 add lxw 12.12.12
        /// </summary>
        /// <returns>string</returns>
        public string ShowManageMember(string contactid)
        {
            string strResult = string.Empty;
            if (!string.IsNullOrEmpty(contactid))
            {
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetList("MCM.ContactID =" + contactid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        strResult += row["Abbr"] + "、";
                    }
                    strResult = strResult.TrimEnd('、');
                    if (strResult.Length > 16)
                    {
                        strResult = strResult.Substring(0, 16) + "...";
                    }
                }
            }
            return strResult;
        }
    }
}
