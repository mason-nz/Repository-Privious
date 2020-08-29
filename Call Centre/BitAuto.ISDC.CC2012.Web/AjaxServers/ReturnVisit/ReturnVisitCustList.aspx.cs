using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QueryBrandInfo = BitAuto.YanFa.Crm2009.Entities.QueryBrandInfo;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;
using System.Diagnostics;
//using BitAuto.ISDC.CC2012.BLL;
namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    public partial class ReturnVisitCustList : PageBase
    {
        #region 定义属性

        public string RequestPageSize
        {
            get { return Request.QueryString["pageSize"] == null ? PageCommon.Instance.PageSize.ToString() : Request.QueryString["pageSize"].Trim(); }
        }
        public string RequestCustName
        {
            get { return Request.QueryString["CustName"] == null ? string.Empty : Request.QueryString["CustName"].Trim(); }
        }
        public string RequestBrand
        {
            get { return Request.QueryString["Brand"] == null ? string.Empty : Request.QueryString["Brand"].Trim(); }
        }
        //public string RequestAgentNum
        //{
        //    get { return Request.QueryString["AgentNum"] == null ? string.Empty : Request.QueryString["AgentNum"].Trim(); }
        //}
        public string RequestSearchTrueNameID
        {
            get { return Request.QueryString["SearchTrueNameID"] == null ? string.Empty : Request.QueryString["SearchTrueNameID"]; }
        }

        public string RequestProvinceID
        {
            get { return Request.QueryString["Province"] == null ? "-1" : Request.QueryString["Province"]; }
        }
        public string RequestCityID
        {
            get { return Request.QueryString["City"] == null ? "-1" : Request.QueryString["City"]; }
        }
        public string RequestCountyID
        {
            get { return Request.QueryString["County"] == null ? "-1" : Request.QueryString["County"]; }
        }

        //add by qizq  2012-5-30取经营范围和客户类型
        public string ClientType
        {
            get { return Request["ClientType"] == null ? string.Empty : Request["ClientType"].ToString(); }
        }
        public string CarType
        {
            get { return Request["CarType"] == null ? string.Empty : Request["CarType"].ToString(); }
        }

        public string StartTime
        {
            get { return Request["StartTime"] == null ? string.Empty : Request["StartTime"].ToString(); }
        }
        public string EndTime
        {
            get { return Request["EndTime"] == null ? string.Empty : Request["EndTime"].ToString(); }
        }
        public int Contact
        {
            get { return Request["Contact"] == "-2" ? -2 : int.Parse(Request["Contact"].ToString()); }
        }
        /// <summary>
        /// 集采项目名
        /// </summary>
        public string RequestProjectName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("ProjectName"); }
        }
        /// <summary>
        /// CC项目名称
        /// </summary>
        public string ReqeustCCProjectName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CCProjectName"); }
        }

        public string radioTaoche
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("radioTaoche"); }
        }
        //add bif 2014-4-17 无坐席标识
        public int RequestNoResponser
        {
            get { return Request["NoResponser"] == "-2" ? -2 : int.Parse(Request["NoResponser"].ToString()); }
        }

        public string TagID
        {
            get { return Request.QueryString["TagID"] == null ? "0" : Request.QueryString["TagID"]; }
        }
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;
        public string FzPerson = string.Empty;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                nowDt.Value = DateTime.Now.ToString("yyyy-MM-dd");
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }

        }
        #endregion

        private void BindData()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int totcount = 0;
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = 20;
            }
            Entities.QueryCC_CustUserMapping query = new Entities.QueryCC_CustUserMapping();
            if (!string.IsNullOrEmpty(RequestCustName))
            {
                query.CustName = RequestCustName.Trim();
            }
            if (!string.IsNullOrEmpty(RequestBrand))
            {
                query.Brandids = RequestBrand.Trim();
            }
            if (!string.IsNullOrEmpty(RequestSearchTrueNameID))
            {
                query.UserName = RequestSearchTrueNameID.Trim();
            }
            //坐席查询条件：是否有坐席
            query.NoResponser = RequestNoResponser;
            if (!string.IsNullOrEmpty(RequestProvinceID) && int.Parse(RequestProvinceID) > 0)
            {
                query.ProvinceID = RequestProvinceID.Trim();
            }
            if (!string.IsNullOrEmpty(RequestCityID) && int.Parse(RequestCityID) > 0)
            {
                query.CityID = RequestCityID.Trim();
            }
            if (!string.IsNullOrEmpty(RequestCountyID) && int.Parse(RequestCountyID) > 0)
            {
                query.CountyID = RequestCountyID.Trim();
            }
            //add by qizq 2012-5-30 客户类型经和营范围
            if (!string.IsNullOrEmpty(ClientType))
            {
                query.TypeID = ClientType;
            }
            if (!string.IsNullOrEmpty(CarType))
            {
                query.CarType = CarType;
            }
            //add lxw 12.6.8 最近访问时间 
            if (!string.IsNullOrEmpty(StartTime))
            {
                query.StartTime = DateTime.Parse(StartTime);
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                query.EndTime = DateTime.Parse(EndTime);
            }
            if (Contact != -2)
            {
                query.Contact = Contact;
            }
            if (!string.IsNullOrEmpty(RequestProjectName))
            {
                query.ProjectName = RequestProjectName.Trim();
            }
            if (!string.IsNullOrEmpty(radioTaoche))
            {
                query.radioTaoche = radioTaoche;
            }
            //取当前人所对应的数据权限组
            Entities.QueryUserGroupDataRigth QueryUserGroupDataRigth = new Entities.QueryUserGroupDataRigth();
            QueryUserGroupDataRigth.UserID = userID;
            DataTable dtUserGroupDataRigth = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigth(QueryUserGroupDataRigth, "", 1, 100000, out totcount);
            string Rolename = string.Empty;
            query.UserID = userID;//进列表肯定有查看本人负责的客户信息
            if (dtUserGroupDataRigth != null && dtUserGroupDataRigth.Rows.Count > 0)
            {
                for (int i = 0; i < dtUserGroupDataRigth.Rows.Count; i++)
                {
                    //4s电话营销，非4s电话营销
                    if (dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "6" || dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "7" || dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "19" || dtUserGroupDataRigth.Rows[i]["bgid"].ToString() == "27")
                    {
                        //本组
                        query.BGIDStr += dtUserGroupDataRigth.Rows[i]["bgid"].ToString() + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(ReqeustCCProjectName))
            {
                query.ReqeustCCProjectName = ReqeustCCProjectName;
            }
            query.TagID = TagID;
            int count;
            DataTable dt;
            string order = "ISNULL(ISNULL(b1.NextVisitDate,b2.NextVisitDate),'9999-12-30'),ISNULL(a.LastUpdateTime,'9999-12-30')";
            dt = BLL.CC_UserCustDataRigth.Instance.GetCustUserMappingByUserID(query, order, PageCommon.Instance.PageIndex, PageSize, out count);
            string s1 = sw.Elapsed.ToString();
            RecordCount = count;
            repeaterList.DataSource = dt;
            repeaterList.DataBind();
            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize, PageCommon.Instance.PageIndex, 1);
            sw.Stop();
            string s2 = sw.Elapsed.ToString();
        }

        protected string getLastVisitTime(string LastTimeReturnVisit, string LastTimeWorkOrder)
        {
            DateTime LastTimeR, LastTimeW;

            bool isLastTimeR = false, isLastTimeW = false;
            isLastTimeR = DateTime.TryParse(LastTimeReturnVisit, out LastTimeR);
            isLastTimeW = DateTime.TryParse(LastTimeWorkOrder, out LastTimeW);

            string msg = "";
            if (isLastTimeR && isLastTimeW)
            {
                if (DateTime.Compare(LastTimeR, LastTimeW) > 0)
                {
                    msg = LastTimeR.ToString("yyyy-MM-dd");
                }
                else
                {
                    msg = LastTimeW.ToString("yyyy-MM-dd");
                }
            }
            else if (isLastTimeR && !isLastTimeW)
            {
                msg = LastTimeR.ToString("yyyy-MM-dd");
            }
            else if (!isLastTimeR && isLastTimeW)
            {
                msg = LastTimeW.ToString("yyyy-MM-dd");
            }
            else
            {
                msg = "";
            }
            return msg;
        }

        protected string getBrandNames(string CustID)
        {
            QueryBrandInfo queryBrandInfo = new QueryBrandInfo();
            queryBrandInfo.CustID = CustID;
            int o;
            string s = "";
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetCustBrandInfo(queryBrandInfo, "", 1, 10000, out o);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += dt.Rows[i]["name"] + " ";
                }
            }

            return s;
        }
        public bool IsMine(string Custid, string theCustomNames)
        {
            bool flag = false;
            string str = BLL.Util.GetLoginRealName();
            for (int i = 0; i < theCustomNames.Split(',').Length; i++)
            {
                if (str == theCustomNames.Split(',')[i])
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        public string GetAreaName(string p, string c, string n)
        {
            string pname = "";
            string cname = "";
            string nname = "";
            DataRow pdrow = DictionaryDataCache.Instance.AreaInfo_Province.ContainsKey(p) ? DictionaryDataCache.Instance.AreaInfo_Province[p] : null;
            DataRow cdrow = DictionaryDataCache.Instance.AreaInfo_City.ContainsKey(c) ? DictionaryDataCache.Instance.AreaInfo_City[c] : null;
            DataRow ndrow = DictionaryDataCache.Instance.AreaInfo_County.ContainsKey(n) ? DictionaryDataCache.Instance.AreaInfo_County[n] : null;

            if (pdrow != null) pname = pdrow["AreaName"].ToString();
            if (cdrow != null) cname = cdrow["AreaName"].ToString();
            if (ndrow != null) nname = ndrow["AreaName"].ToString();

            return pname + " " + cname + " " + nname;
        }
    }
}