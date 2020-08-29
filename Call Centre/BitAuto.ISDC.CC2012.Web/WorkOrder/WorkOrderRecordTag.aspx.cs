using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class WorkOrderRecordTag : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int PageSize
        {
            get
            {
                return this.AjaxPager_Record.PageSize;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        Web.CustInfo.CustInfoHelper ch = new Web.CustInfo.CustInfoHelper();

        private void BindData()
        {
            //查询条件 ch.CustID;
            int totalCount = 0;
            DataTable table = BitAuto.ISDC.CC2012.BLL.WOrderInfo.Instance.GetWorkOrderInfoForV1V2Info(ch.CustID, -1, ch.CurrentPage, PageSize, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater_Record.DataSource = table;
            }
            //绑定列表数据
            repeater_Record.DataBind();
            AjaxPager_Record.InitPager(totalCount);
        }
        //链接
        public string GetURL(string orderid)
        {
            string url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(orderid, "", "");
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(orderid, url);
            string a = "<a target='_blank' href='" + url + "' class='linkBlue'>" + orderid + "</a>";
            return a;
        }
        //状态
        public string StatusName(string statusID)
        {
            string name = string.Empty;
            int _id;
            if (int.TryParse(statusID, out _id))
            {
                name = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderStatus), _id);
            }
            return name;
        }
        //内容
        public string GetContent(string content)
        {
            string trimStr = CutStr(content, 60);
            if (trimStr != content)
            {
                trimStr += "...";
            }
            return trimStr;
        }
        //计算长度
        public string CutStr(string str, int len)
        {
            if (str == null || str.Length == 0 || len <= 0)
            {
                return string.Empty;
            }

            int l = str.Length;

            #region 计算长度
            int clen = 0;
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }
            #endregion

            if (clen < l)
            {
                return str.Substring(0, clen) + "...";
            }
            else
            {
                return str;
            }
        }
    }
}