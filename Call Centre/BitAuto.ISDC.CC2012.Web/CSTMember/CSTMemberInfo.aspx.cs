using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.YanFa.Crm2009.BLL;
using BitAuto.YanFa.Crm2009.Entities;
using BitAuto.YanFa.Crm2009.Entities.Constants;
using System.Data;
using BitAuto.Utils;
using BitAuto.YanFa.DMSInterface;
using System.Configuration;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CSTMember
{
    public partial class CSTMemberInfo : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string RequestCSTMemberID
        {
            get { return Request.QueryString["CSTMemberID"] == null ? string.Empty : Request.QueryString["CSTMemberID"].ToString().Trim(); }
        }
        public string SysUrl = System.Configuration.ConfigurationManager.AppSettings["SysUrl"].ToString();
        public string CrmUrl = System.Configuration.ConfigurationManager.AppSettings["CrmUrl"].ToString();
        public string CSTRecID { get { return (Request["CSTRecID"] + "").ToString().Trim(); } }
        public string HasRight = "1";
        //zhaoxinxin 20120229: 属性
        protected string clientStatus = "";
        protected string clientFullName = "";
        protected string clientShortName = "";
        protected string clientCode = "";
        protected string clientType = "";
        protected string upCompanyName = "";
        protected string region = "";
        protected string detailAddr = "";
        protected string postCode = "";
        protected string contact = "";
        protected string depart = "";
        protected string position = "";
        protected string tele = "";
        protected string phone = "";
        protected string email = "";
        protected string syncTime = "";
        protected string UCount = "";
        protected string lastAddUbTime = "";
        protected string UbTotalAmount = "";
        protected string UbTotalExpend = "";
        protected string activeTime = "";
        protected string productOpenKey = "";
        protected string CustID = "";
        protected string TrafficInfo = "";
        protected string SyncStatus = "";
        protected string Status = "";
        protected string SyncStatusValue = "";
        protected string CreateTime = "";
        protected string BrandIdsName = "";
        protected string vendorClass = "";
        protected string CSTMemberID = "";
        protected string SyncTime = "";

        public string tabI
        {
            get { return HttpContext.Current.Request["i"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["i"].ToString()); }
        }

        public bool ListOfContact = false;
        public bool ListOfCooperationProjects = false;
        public bool ListOfCustUser = false;
        public bool ListOfReturnVisit = false;
        public bool ListOfBusinessLicense = false;
        public bool ListOfBusinessBrandLicense = false;
        public bool ListOfBusinessScaleInfo = false;
        public bool ListOfTaskRecord = false;
        public bool ListOfWorkOrder = false;
        public bool IsCanEdit = false;
        public bool IsCanDelete = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                lbCSTMember.Text = "";
                BindRepterMemberNameList(CustID);

                int userID = BLL.Util.GetLoginUserID();

                ////编辑权限判断
                //功能点验证逻辑
                if (BLL.Util.CheckRight(userID, "SYS024BUT2201"))//客户联系人
                {
                    ListOfContact = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2202"))//合作项
                {
                    ListOfCooperationProjects = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2203"))//负责员工
                {
                    ListOfCustUser = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2204"))//访问记录
                {
                    ListOfReturnVisit = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2205"))//年检记录
                {
                    ListOfBusinessLicense = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2206"))//二手车规模
                {
                    ListOfBusinessScaleInfo = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2207"))//任务记录
                {
                    ListOfTaskRecord = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2208"))//品牌授权书
                {
                    ListOfBusinessBrandLicense = true;
                }
                if (BLL.Util.CheckRight(userID, "SYS024BUT2213"))//工单
                {
                    ListOfWorkOrder = true;
                }

                HasRight = "0";
                //}
            }
            //获取引用链接所需要的“Key”
            GetProductOpenKey();
        }
        private void BindData()
        {
            //获取查询对象信息
            BitAuto.YanFa.Crm2009.Entities.QueryCstMember query = new BitAuto.YanFa.Crm2009.Entities.QueryCstMember();
            try
            {
                query.CSTRecID = Request.QueryString["CSTRecID"].ToString();
            }
            catch
            {
                query.CSTRecID = "";
            }

            //把对象信息存入页面变量中
            DataTable dtCstMember = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetTableByRecID(query);
            if (dtCstMember.Rows.Count == 0)
            {
                Response.Write(@"<script language='javascript'>javascript:alert('您要查看的会员不存在');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                //ScriptHelper.ShowCustomScript(this.Page, "$.jAlert('您要查看的会员不存在',function(){window.close();});");
            }
            else
            {
                if (dtCstMember.Rows[0]["FullName"] == DBNull.Value || dtCstMember.Rows[0]["FullName"].ToString() == "")
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('您要查看的会员不存在');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                    //ScriptHelper.ShowCustomScript(Page, "$.jAlert('您要查看的会员不存在',function(){window.close();});");
                }
                else
                {

                    GetCSTMemberProperties(dtCstMember);

                    //获取联系人列表，并把列表的第一个人放入页面变量中
                    string strWhereLinkMan = "CSTRecID = '" + dtCstMember.Rows[0]["CSTRecID"].ToString() + "'";
                    DataSet dsLinkMan = BitAuto.YanFa.Crm2009.BLL.CSTLinkMan.Instance.GetList(strWhereLinkMan);
                    GetLinkManProperties(dsLinkMan);
                }
            }
        }

        //用于把查询出的数据存入页面变量（客户基本信息）
        private void GetCSTMemberProperties(DataTable dt)
        {
            if (!string.IsNullOrEmpty(dt.Rows[0]["Status"].ToString()))
            {
                this.clientStatus = GetCstStatusString(Convert.ToInt32(dt.Rows[0]["Status"].ToString()));
            }
            this.Status = dt.Rows[0]["Status"].ToString();
            this.clientFullName = dt.Rows[0]["FullName"].ToString();
            this.clientShortName = dt.Rows[0]["ShortName"].ToString();
            this.clientCode = dt.Rows[0]["VendorCode"].ToString();
            if (!string.IsNullOrEmpty(dt.Rows[0]["VendorClass"].ToString()))
            {
                this.clientType = GetCstTypeString(Convert.ToInt32(dt.Rows[0]["VendorClass"].ToString()));
                vendorClass = dt.Rows[0]["VendorClass"].ToString();
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["SuperId"].ToString()))
            {
                this.upCompanyName = GetSuperCompanyString(Convert.ToInt32(dt.Rows[0]["SuperId"].ToString()));
            }
            //this.region = dt.Rows[0]["ProvinceID"].ToString();
            this.region = GetRegionString(dt.Rows[0]["ProvinceID"].ToString(), dt.Rows[0]["CityID"].ToString(), dt.Rows[0]["CountyID"].ToString());
            this.detailAddr = dt.Rows[0]["Address"].ToString();

            this.postCode = dt.Rows[0]["PostCode"].ToString();

            if (dt.Rows[0]["SyncTime"] != DBNull.Value)
            {
                this.syncTime = Convert.ToDateTime(dt.Rows[0]["SyncTime"]).ToShortDateString();
            }
            BitAuto.YanFa.Crm2009.Entities.CSTExpandInfo dstCSTExpandInfo = BitAuto.YanFa.Crm2009.BLL.CSTExpandInfo.Instance.GetModelByCSTRecID(CSTRecID);
            if (dstCSTExpandInfo != null)
            {
                if (dstCSTExpandInfo.LastAddUbTime != Constant.DATE_INVALID_VALUE)
                {
                    this.lastAddUbTime = dstCSTExpandInfo.LastAddUbTime.ToShortDateString();
                }
                if (dstCSTExpandInfo.UBTotalAmount != Constant.INT_INVALID_VALUE)
                {
                    this.UbTotalAmount = dstCSTExpandInfo.UBTotalAmount.ToString();
                }
                if (dstCSTExpandInfo.UBTotalExpend != Constant.INT_INVALID_VALUE)
                {
                    this.UbTotalExpend = dstCSTExpandInfo.UBTotalExpend.ToString();
                }
            }
            this.CustID = dt.Rows[0]["CustID"].ToString();
            this.SyncStatus = GetSyncStatusDesc(dt.Rows[0]["SyncStatus"].ToString());
            //如果会员创建人是本人，并且是驳回的会员，则可以编辑。 modify:yangyh  date:2013-08-23
            if ((dt.Rows[0]["CreateUserID"].ToString() == BLL.Util.GetLoginUserID().ToString() || BLL.Util.CheckButtonRight("SYS024BUG200401")) && dt.Rows[0]["SyncStatus"].ToString() == "170008")
            {
                IsCanEdit = true;
            }
            if ((dt.Rows[0]["CreateUserID"].ToString() == BLL.Util.GetLoginUserID().ToString() || BLL.Util.CheckButtonRight("SYS024BUG200402")) && dt.Rows[0]["SyncStatus"].ToString() == "170008")
            {
                IsCanDelete = true;
            }
            this.SyncStatusValue = dt.Rows[0]["SyncStatus"].ToString();
            this.CreateTime = string.Format("{0:yyyy-MM-dd}", dt.Rows[0]["CreateTime"]);

            DataTable dstTable = BitAuto.YanFa.Crm2009.BLL.CSTMember_Brand.Instance.GetList(" CSTRecID ='" + CSTRecID + "'").Tables[0];
            if (dstTable.Rows.Count > 0)
            {
                string BrandIds = "";
                foreach (DataRow row in dstTable.Rows)
                {
                    BrandIds += row["BrandID"].ToString() + ",";
                }
                BrandIds = BrandIds.TrimEnd(',');
                BrandIdsName = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandNames(BrandIds);
            }
            CSTMemberID = dt.Rows[0]["CSTMemberID"].ToString();
        }

        //用于把查询出的数据存入页面变量（客户联系人信息）
        private void GetLinkManProperties(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.contact = ds.Tables[0].Rows[0]["Name"].ToString();
                this.tele = ds.Tables[0].Rows[0]["Tel"].ToString();
                this.phone = ds.Tables[0].Rows[0]["Mobile"].ToString();
                this.email = ds.Tables[0].Rows[0]["Email"].ToString();
            }
        }

        //用于生成显示在外部的上级公司的字符串，如果从数据库中可以查出
        private string GetSuperCompanyString(int superId)
        {
            //BitAuto.YanFa.Crm2009.Entities.QueryCstMember query = new QueryCstMember();
            //query.CstMemberID = superId;
            //DataTable dtCstMember = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetTableByCstID(query);
            //房良培修改为从接口调取
            DataTable dt = new DataTable();
            if (Cache["SuperVerdor"] != null)
            {
                dt = (DataTable)Cache["SuperVerdor"];
            }
            else
            {
                string strMsg = string.Empty;
                dt = WebService.CstMemberServiceHelper.Instance.GetSuperVendor(out strMsg);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Cache.Insert("SuperVerdor", dt, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
                }
            }
            string CSTFullName = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["DVTId"].ToString() == superId.ToString())
                    {
                        CSTFullName = row["Name"].ToString();
                    }
                }
            }
            else if (superId == -2)
            {
                CSTFullName = "";
            }
            else
            {
                CSTFullName = "[" + superId.ToString() + "]";
            }
            return CSTFullName;
        }

        private string GetCstStatusString(int statusNum)
        {
            string StatusString = "";
            switch (statusNum)
            {
                case 0:
                    StatusString = "禁用";
                    break;
                case 1:
                    StatusString = "启用";
                    break;
                case -1:
                    StatusString = "已删除";
                    break;
            }
            return StatusString;
        }

        //获取CST客户的种类：2.4S店;3.专业公司;4.厂商;5.其它
        private string GetCstTypeString(int vendorClass)
        {
            string strResult = "";
            switch (vendorClass)
            {
                case 1:
                    strResult = "个人用户";
                    break;
                case 2:
                    strResult = "4S店";
                    break;
                case 3:
                    strResult = "经纪公司";
                    break;
                case 4:
                    strResult = "厂商";
                    break;
                case 5:
                    strResult = "其他";
                    break;
            }
            return strResult;
        }

        //获取地区字符串
        private string GetRegionString(string provinceID, string cityID, string CountyID)
        {
            string regionStr = "";
            string provinceName = "";
            string cityName = "";
            string countyName = "";
            BitAuto.YanFa.Crm2009.Entities.AreaInfo provinceAreaInfo = new BitAuto.YanFa.Crm2009.Entities.AreaInfo();
            provinceAreaInfo = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(provinceID);
            if (provinceAreaInfo != null)
            {
                provinceName = provinceAreaInfo.AreaName;
            }
            else
            {
                provinceName = provinceID;
            }
            BitAuto.YanFa.Crm2009.Entities.AreaInfo cityAreaInfo = new BitAuto.YanFa.Crm2009.Entities.AreaInfo();
            cityAreaInfo = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(cityID);
            if (cityAreaInfo != null)
            {
                cityName = cityAreaInfo.AreaName;
            }
            else
            {
                cityName = cityID;
            }
            BitAuto.YanFa.Crm2009.Entities.AreaInfo countyAreaInfo = new BitAuto.YanFa.Crm2009.Entities.AreaInfo();
            countyAreaInfo = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(CountyID);
            if (countyAreaInfo != null)
            {
                countyName = countyAreaInfo.AreaName;
            }
            else
            {
                countyName = CountyID;
            }
            regionStr = provinceName + " " + cityName + " " + countyName;
            return regionStr;


        }

        //获取引用链接所需要的“Key”
        private void GetProductOpenKey()
        {
            this.productOpenKey = ConfigurationManager.AppSettings["CSTMemberServiceKey"].ToString();
        }

        //以下显示选项卡内容

        /// <summary>
        /// 绑定客户所属会员名称
        /// </summary>
        /// <param name="custID"></param>
        private void BindRepterMemberNameList(string custID)
        {
            string strURL = "";
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectByCustID(custID);
            //只看车商通
            //foreach (DataRow newRow in dt.Rows)
            //{
            //    if (newRow["type"].ToString() == "1")
            //    {
            //        newRow.Delete();
            //    }
            //}
            //dt.AcceptChanges();
            if (dt != null && dt.Rows.Count > 0)
            {
                
                literalMemberNameList.Text += "<div class=\"main_box\"><ul>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region 2013.12.25 去掉车商通会员前代码
                    /*
                    if (dt.Rows[i]["type"].ToString() == "0")
                    {
                        strURL = "/CSTMember/CSTMemberInfo.aspx?CSTRecID=";
                    }
                    else
                    {
                        strURL = "/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=";
                    }
                    
                    string cstName = (dt.Rows[i]["pxh"].ToString() == "1" ? "&nbsp;&nbsp;&nbsp;&nbsp;易湃会员" : "&nbsp;&nbsp;车商通会员") + getAbbr(dt.Rows[i]["Abbr"] + "") + getSyncStatus(dt.Rows[i]["SyncStatus"].ToString()) + "&nbsp;&nbsp;&nbsp;&nbsp;";
                    */
                    #endregion

                    #region 2013.12.25 去掉车商通会员后代码
                    strURL = "/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=";
                    string cstName = "&nbsp;&nbsp;&nbsp;&nbsp;易湃会员" + getAbbr(dt.Rows[i]["Abbr"] + "") + getSyncStatus(dt.Rows[i]["SyncStatus"].ToString()) + "&nbsp;&nbsp;&nbsp;&nbsp;";
                    #endregion
                    

                    lbCSTMember.Text += "<li id='one" + i + "' class=' ' style='width:auto;'><a href='" + strURL + dt.Rows[i]["id"] + "&CustID=" + CustID + "&i=" + i + "' style='text-decoration:none; color:#555;'>" + cstName + "</a></li>";

                    //literalMemberNameList.Text += "<li class=\"\">"
                    //            + "<a href=\"" + strURL + dt.Rows[i]["id"] + "&CustID=" + CustID + "\">"
                    //             + (dt.Rows[i]["pxh"].ToString() == "1" ? "易湃会员" : "车商通会员") + getAbbr(dt.Rows[i]["Abbr"] + "") + getSyncStatus(dt.Rows[i]["SyncStatus"].ToString()) + "</a></li>";

                    if (i == 2 && dt.Rows.Count > 3)
                    {
                        literalMemberNameList.Text += "<li class=\"select_box\" style='width:20px'><span>更多会员<img src=\"/images/arrow.jpg\" width=\"7\" height=\"4\" /></span><ul class=\"s_ul\" style=\"display: none;\">";

                        //literalMemberNameList.Text += "</ul>";
                        //literalMemberNameList.Text += "<div  onmouseover=\"showMoreMember(true);\" onmouseout=\"showMoreMember(false);\"  class=\"gegnduo\"><a href=\"#\" class=\"gd\" style=\"width:60px;\"    onmouseover=\"showMoreMember(true);\" onmouseout=\"showMoreMember(false);\" >更多会员&gt;&gt;</a><ul   id=\"moreinfo\"   style=\"display:none\"  class=\"more\">";
                        break;
                    }
                }
                if (dt.Rows.Count > 3)
                {
                    for (int i = 3; i < dt.Rows.Count; i++)
                    {
                        #region 2013.12.25 去掉车商通会员前代码
                        /*
                        if (dt.Rows[i]["type"].ToString() == "0")
                        {
                            strURL = "/CSTMember/CSTMemberInfo.aspx?CSTRecID=";
                        }
                        else
                        {
                            strURL = "/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=";
                        }
                        literalMemberNameList.Text += "<li><a href=\"" + strURL + dt.Rows[i]["id"] + "&CustID=" + CustID + "\">"
                                     + (dt.Rows[i]["pxh"].ToString() == "1" ? "易湃会员" : "车商通会员") + getAbbr(dt.Rows[i]["Abbr"] + "") + getSyncStatus(dt.Rows[i]["SyncStatus"].ToString()) + "</a></li>";
                        */
                         #endregion

                        #region 2013.12.25 去掉车商通会员后代码
                        strURL = "/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=";
                        literalMemberNameList.Text += "<li><a href=\"" + strURL + dt.Rows[i]["id"] + "&CustID=" + CustID + "\">"
                                     + "易湃会员" + getAbbr(dt.Rows[i]["Abbr"] + "") + getSyncStatus(dt.Rows[i]["SyncStatus"].ToString()) + "</a></li>";
                        #endregion

                        //literalMemberNameList.Text += "<li class=\"\" style='overflow:visible;'>"
                        //            + "<a href=\"" + strURL + dt.Rows[i]["id"] + "&CustID=" + CustID + "\">"
                        //             + (dt.Rows[i]["pxh"].ToString() == "1" ? "易湃会员" : "车商通会员") + getAbbr(dt.Rows[i]["Abbr"] + "") + getSyncStatus(dt.Rows[i]["SyncStatus"].ToString()) + "</a></li>";

                    }
                    literalMemberNameList.Text += "</ul></li></ul></div>";
                }
                else
                {
                    literalMemberNameList.Text += "</ul></div>";
                }
            }
        }
        private string getSyncStatus(string SyncStatus)
        {
            if (SyncStatus == ((int)EnumDMSSyncStatus.CreateSuccessful).ToString())
            {
                return "";
            }
            else if (SyncStatus == ((int)EnumDMSSyncStatus.Rejected).ToString())
            {
                return "—已打回";
            }
            else
            {
                return "—未开通";
            }
        }

        private string getAbbr(string Abbr)
        {
            if (!string.IsNullOrEmpty(Abbr))
            {
                return "(" + Abbr + ")";
            }
            return "";
        }
        private string getdivclass(string id)
        {
            if (id == CSTRecID)
            {
                return "current";
            }
            return "";
        }

        /// <summary>
        /// 获取会员开通状态
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetSyncStatusDesc(string s)
        {
            string SyncStatus = "";
            int status = 0;
            if (!string.IsNullOrEmpty(s))
            {
                if (int.TryParse(s, out status))
                {
                    SyncStatus = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptTextCSTMember(status);
                }
            }
            return SyncStatus;
        }
        public string CstMemberID()
        {
            return BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectCstMemberIDByCSTRecID(CSTRecID);
        }

        public string GetRejectedStr(string SyncStatusValue)
        {
            int recid = 0;
            if (SyncStatusValue == "170008")
            {
                string result = "";
                DataTable dstTable = new DataTable();
                dstTable = BitAuto.YanFa.Crm2009.BLL.CSTMemberSyncLog.Instance.GetSyncLog(CSTRecID);
                for (int i = 0; i < dstTable.Rows.Count; i++)
                {
                    if (dstTable.Rows[i]["SyncStatus"].ToString() == "170008")
                    {
                        if (int.Parse(dstTable.Rows[i]["RecID"].ToString()) > recid)
                        {
                            result = "<br><div style= \"word-wrap:break-word;word-break:break-all;width:200px;\">(" + dstTable.Rows[i]["Description"].ToString() + ")</div>";
                            recid = int.Parse(dstTable.Rows[i]["RecID"].ToString());
                        }
                    }
                }
                return result;
            }
            else
            {
                return "";
            }
        }

        public string GetStatus(string status)
        {
            if (status == "1")
            {
                return "<img src=\"/Images/xt.gif\" alt=\"启用\" title ='启用'>";
            }
            else if (status == "0")
            {
                return "<img src=\"/Images/jinyong.ico\" alt=\"禁用\" title ='禁用'>";
            }
            else
            {
                return "";
            }
        }
    }
}