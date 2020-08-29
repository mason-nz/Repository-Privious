using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.YanFa.Crm2009.Entities;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class CustUserList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string custID;
        /// <summary>
        /// 
        /// </summary>
        public string CustID { get { return custID; } }

        public int PageSize
        {
            get
            {
                return this.AjaxPager_CUM.PageSize;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                //增加 负责员工 页面权限
                //int userId = BLL.Util.GetLoginUserID();
                //if (BLL.Util.CheckRight(userId, "SYS024BUT2203"))
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

        private void BindData()
        {
            CustInfoHelper ch = new CustInfoHelper();
            this.custID = ch.CustID;

            //查询
            int totalCount = 0;
            BitAuto.YanFa.Crm2009.Entities.QueryCustUserMapping qcum = new BitAuto.YanFa.Crm2009.Entities.QueryCustUserMapping();
            qcum.CustID = ch.CustID;
            DataTable table = BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.GetCustUserMapping(qcum, "CreateTime DESC", ch.CurrentPage, PageSize, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                this.repeater_CUM.DataSource = table;
            }
            //绑定列表数据
            this.repeater_CUM.DataBind();
            //分页控件 
            AjaxPager_CUM.PageSize = 5;
            AjaxPager_CUM.InitPager(totalCount);
        }

        public string GetBussinessName(string userId)
        {
            string strResult = string.Empty;
            int businessId = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetBusinessLine(int.Parse(userId));
            strResult = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.ShowBusinessName(businessId.ToString());
            
            return strResult;
        }

        /// <summary>
        /// 获取负责会员
        /// </summary>
        /// <param name="id">人员编号</param>
        /// <returns></returns>
        public string GetManageMember(string userid)
        {
            string strResult = string.Empty;
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.MemberUserMapping.Instance.GetList(" member.Status>=0 AND mum.UserID ='" + userid + "' AND member.CustID='" + CustID + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    strResult += row["Abbr"] + "、";
                }
                strResult = strResult.TrimEnd('、');
            }
            return strResult;
        }

    }
}
