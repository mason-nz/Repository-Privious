using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_BusinessScale
{
    public partial class Dispose : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID
        {
            get { return Request["TID"] + ""; }
        }
        public string RecID
        {
            get { return Request["RecID"] + ""; }
        }
        public string Action
        {
            get { return Request["Action"] + ""; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string[]> monthStock = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthStock));
            foreach (string[] s in monthStock)
            {
                this.sltMonthStock.Items.Add(new ListItem(s[0], s[1]));
            }
            this.sltMonthStock.Items.Insert(0, new ListItem("请选择", "-1"));
            this.sltMonthStock.SelectedIndex = 0;

            List<string[]> monthSales = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthSales));
            foreach (string[] s in monthSales)
            {
                this.sltMonthSales.Items.Add(new ListItem(s[0], s[1]));
            }
            this.sltMonthSales.Items.Insert(0, new ListItem("请选择", "-1"));
            this.sltMonthSales.SelectedIndex = 0;

            List<string[]> monthTrade = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthTrade));
            foreach (string[] s in monthTrade)
            {
                this.sltMonthTrade.Items.Add(new ListItem(s[0], s[1]));
            }
            this.sltMonthTrade.Items.Insert(0, new ListItem("请选择", "-1"));
            this.sltMonthTrade.SelectedIndex = 0;

            if (Action.ToLower().Equals("edit"))
            {
                int id = -1;
                if (int.TryParse(RecID, out id))
                {
                    Entities.ProjectTask_BusinessScale info = BLL.ProjectTask_BusinessScale.Instance.GetProjectTask_BusinessScale(id);
                    if (info.MonthStock > 0)
                    {
                        this.sltMonthStock.Value = info.MonthStock.ToString();
                    }
                    if (info.MonthSales > 0)
                    {
                        this.sltMonthSales.Value = info.MonthSales.ToString();
                    }
                    if (info.MonthTrade > 0)
                    {
                        this.sltMonthTrade.Value = info.MonthTrade.ToString();
                    }
                }
            }
        }
    }
}