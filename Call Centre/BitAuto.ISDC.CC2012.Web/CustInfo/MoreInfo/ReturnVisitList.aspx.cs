using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class ReturnVisitList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int PageSize
        {
            get
            {
                return this.AjaxPager_RVL.PageSize;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                //增加 访问记录页面权限
                //int userId = BLL.Util.GetLoginUserID();
                //if (BLL.Util.CheckRight(userId, "SYS024BUT2204"))
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
            BitAuto.YanFa.Crm2009.Entities.QueryReturnVisit QueryReturnVisit = new BitAuto.YanFa.Crm2009.Entities.QueryReturnVisit();
            QueryReturnVisit.CustID = ch.CustID;
            DataTable table = BitAuto.YanFa.Crm2009.BLL.ReturnVisit.Instance.GetReturnVisit(QueryReturnVisit, ch.CurrentPage, PageSize, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                table.Columns.Add("showRemark");

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string remark = table.Rows[i]["Remark"].ToString();
                    string showStr = remark;

                    if (remark.Length > 20)
                    {
                        showStr = remark.Substring(0, 20);
                    }

                    table.Rows[i]["showRemark"] = showStr;
                }
                this.repeater_RVL.DataSource = table;
            }
            //绑定列表数据
            this.repeater_RVL.DataBind();
            //分页控件
            AjaxPager_RVL.PageSize = 5;
            AjaxPager_RVL.InitPager(totalCount);   
        }

        protected string getTypeStr(string type)
        {
            string RVType = "";
            switch (type)
            {
                case "1":
                    RVType = "短信联系";
                    break;
                case "2":
                    RVType = "电话联系";
                    break;
                case "3":
                    RVType = "发送传真";
                    break;
                case "4":
                    RVType = "电子邮件";
                    break;
                case "5":
                    RVType = "信件邮递";
                    break;
                case "6":
                    RVType = "一般会议";
                    break;
                case "7":
                    RVType = "上门拜访";
                    break;
            }
            return RVType;
        }

        protected string getUserTrueNameByID(string userid)
        {
            BitAuto.YanFa.Crm2009.Entities.ContactInfo c = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfoByUserID(int.Parse(userid));
            if (c != null)
            {
                return c.CName;
            }
            return "";
        }

        protected string DateDiff(string BeginTime, string EndTime)
        {
            string dateDiff = "";
            try
            {
                DateTime dtBeginTime = DateTime.Parse(BeginTime);
                DateTime dtEndTime = DateTime.Parse(EndTime);
                dateDiff = dtBeginTime.ToString("yyyy-MM-dd") + " " + dtBeginTime.Hour + ":" + dtBeginTime.Minute + "-" +
                          dtEndTime.Hour + ":" + dtEndTime.Minute + "";
                //TimeSpan ts = DateTime.Parse(EndTime) - DateTime.Parse(BeginTime);
                //dateDiff = String.Format("{0:N2} ", ((ts.Hours * 60 + ts.Minutes) / 60.0)) + "小时";  
            }
            catch
            {
            }
            return dateDiff;
        }

        public string getUserClassStr(string UserClass)
        {
            string re = "";
            switch (UserClass)
            {
                case "1":
                    re = "销售";
                    break;
                case "2":
                    re = "客服";
                    break;
                case "3":
                    re = "编辑";
                    break;
            }
            return re;
        }
    }
}