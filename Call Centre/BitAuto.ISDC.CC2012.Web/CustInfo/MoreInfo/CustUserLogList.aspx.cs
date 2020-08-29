using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class CustUserLogList : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
                return this.AjaxPager_CUMLog.PageSize;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        { 
            CustInfoHelper ch = new CustInfoHelper();
            this.custID = ch.CustID;

            //查询
            int totalCount = 0;
            string where = "and CustID='" + this.CustID + "'";
            DataTable table = BitAuto.YanFa.Crm2009.BLL.CustUserMappingLog.Instance.GetCustUserMappingLog(where, "BeginTime DESC", ch.CurrentPage, PageSize, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater.DataSource = table;
            }
            //绑定列表数据
            repeater.DataBind();
            //分页控件
            AjaxPager_CUMLog.PageSize = 5;
            AjaxPager_CUMLog.InitPager(totalCount);  
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