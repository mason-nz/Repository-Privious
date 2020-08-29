using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.WebService.EasyPass;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.WebService;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class TongMemberList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int locationid
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryInt("locationid");
            }
        }
        public int carid
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryInt("carid");
            }
        }
        //为分页

        public int PageIndex
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryInt("PageIndex");
            }
        }

        public int GroupLength = 5;
        public int PageSize = 5;
        public string strhtml = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        { 
            BindData();
        }

        private void BindData()
        {
            int[] memberIDs;
            string CacheName = "DmsMember" + locationid.ToString() + "_" + carid.ToString();

            string ispage = BLL.Util.GetCurrentRequestQueryStr("ispage");

            if (ispage != "" && Cache[CacheName] != null)
            {
                memberIDs = (int[])Cache[CacheName];
            }
            else
            {
                memberIDs = FanXianHelper.Instance.GetFanxianDealers(locationid, carid);
                Cache.Insert(CacheName, memberIDs, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            List<string> list = new List<string>();
            if (memberIDs.Length > 0)
            {
                foreach (int i in memberIDs)
                {
                    list.Add(i.ToString());
                }
            }

            List<string> currList = GetPagedIntList(list, PageSize, Convert.ToInt32(PageIndex));
            string CodeStr = "";
            foreach (string item in currList)
            {
                CodeStr += "'" + item + "',";
            }
            if (CodeStr.Length > 0)
            {
                CodeStr = CodeStr.Substring(0, CodeStr.Length - 1);
            }
            if (CodeStr != "")
            {
                DataTable dt = BLL.CRMDMSMember.Instance.GetDMSMemberByCodeStr(CodeStr);

                this.rptList.DataSource = dt;
                this.rptList.DataBind();
                litPagerDown.Text = PageCommon.Instance.LinkStringByPostForHC("", GroupLength, memberIDs.Length, PageSize, Convert.ToInt32(PageIndex), 1);
            }
        }

        private List<string> GetPagedIntList(List<string> list, int PageSize, int PageIndex)
        {
            List<string> currPageList = new List<string>();
            if (PageIndex <= 0)
            {
                PageIndex = 1;
            }

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize - 1;

            if (rowbegin >= list.Count)
            {
                return currPageList;
            }

            if (rowend > list.Count - 1)
            {
                rowend = list.Count - 1;
            }

            for (int i = rowbegin; i <= rowend; i++)
            {
                currPageList.Add(list[i]);
            }

            return currPageList;
        }

        public string GetAreaName(string AreaID)
        {
            return AreaHelper.Instance.GetAreaNameByID(AreaID);
        }

        public string Get400(string memberCode)
        {
            string Phone400 = "";
            int intval = 0;
            DataSet ds = new DataSet();
            if (int.TryParse(memberCode, out intval))
            {
                ds = DealerInfoServiceHelper.Instance.GetDealer400(intval);
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Phone400 = ds.Tables[0].Rows[0]["Dealer400"].ToString();
            }
            return Phone400;
        }
    }
}