using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.YanFa.Crm2009.Entities;
using BitAuto.ISDC.CC2012.WebService;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailH
{
    public partial class UCMemberDetail : System.Web.UI.UserControl
    {
        public string SysUrl = System.Configuration.ConfigurationManager.AppSettings["SysUrl"].ToString();
        public string CrmUrl = System.Configuration.ConfigurationManager.AppSettings["CrmUrl"].ToString();
        public string CRMMemberServerInfo = System.Configuration.ConfigurationManager.AppSettings["CRMMemberServerInfo"].ToString();
        private BitAuto.YanFa.Crm2009.Entities.DMSMember memberInfo;
        /// <summary>
        /// 会员
        /// </summary>
        public BitAuto.YanFa.Crm2009.Entities.DMSMember MemberInfo { get { return memberInfo; } set { memberInfo = value; } }

        private string custInfoHref = "";
        /// <summary>
        /// 客户信息地址
        /// </summary>
        public string CustInfoHref { get { return custInfoHref; } set { custInfoHref = value; } }

        private string memberInfoHref = "";
        /// <summary>
        /// 会员信息地址
        /// </summary>
        public string MemberInfoHref { get { return memberInfoHref; } set { memberInfoHref = value; } }

        public bool ListOfContact = false;
        public bool ListOfCooperationProjects = false;
        public bool ListOfCustUser = false;
        public bool ListOfReturnVisit = false;
        public bool ListOfBusinessLicense = false;
        public bool ListOfBusinessBrandLicense = false;
        public bool ListOfBusinessScaleInfo = false;
        public bool ListOfTaskRecord = false;
        public bool ListOfWorkOrder = false;

        //自用
        protected string CustID = "";
        protected string MemberID = "";

        protected string Lat = "0";
        protected string Lng = "0";

        public string memberName = "";//当前CSTMember名

        public string tabI
        {
            get { return HttpContext.Current.Request["i"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["i"].ToString()); }
        }

        /// <summary>
        /// 未申请开通会员(显示按钮)
        /// </summary>
        protected bool NotApplyFor = false;

        /// <summary>
        /// 已开通会员
        /// </summary>
        protected bool IsApplyForSuccessful = false;

        /// <summary>
        /// 当前会员数量
        /// </summary>
        public int CCDMSMemberCount = 0;


        private string DealerInfoServiceURL = System.Configuration.ConfigurationManager.AppSettings["DealerInfoServiceURL"].ToString();//服务URL

        public bool IsCanEdit = false;
        public bool IsCanDelete = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.MemberInfo != null)
                {
                    this.CustID = this.MemberInfo.CustID;
                    this.MemberID = this.MemberInfo.ID.ToString();
                    int userID = BLL.Util.GetLoginUserID();
                    string departID = BLL.Util.GetDepartmentID();

                    //如果会员创建人是本人，并且是驳回的会员，则可以编辑。 modify:yangyh  date:2013-08-23
                    if ((this.memberInfo.CreateUserID == userID || BLL.Util.CheckButtonRight("SYS024BUG200304")) && memberInfo.SyncStatus == 170008)
                    {
                        IsCanEdit = true;
                    }
                    if ((this.memberInfo.CreateUserID == userID || BLL.Util.CheckButtonRight("SYS024BUG200305")) && memberInfo.SyncStatus == 170008)
                    {
                        IsCanDelete = true;
                    }
                    BindCustomerInfo(this.CustID);
                    BindRepterMemberNameList(this.CustID);
                    BindMemberInfo(this.MemberInfo);

                    if (!string.IsNullOrEmpty(MemberInfo.MemberCode))
                    {
                        lnkViewJiCaiProject.Style.Add("display", BitAuto.YanFa.Crm2009.BLL.JiCaiProject.Instance.Exists(MemberInfo) ? string.Empty : "none");
                        int memberCode = 0;
                        if (int.TryParse(MemberInfo.MemberCode, out memberCode))
                        {
                            li400.Style.Remove("display");
                            DataSet ds = WebService.DealerInfoServiceHelper.Instance.GetDealer400(memberCode);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                span400.InnerText = ds.Tables[0].Rows[0][2].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void BindCustomerInfo(string custID)
        {
            //Crm2009.Entities.CustInfo model = new Crm2009.Entities.CustInfo();
            //model = Crm2009.BLL.CustInfo.Instance.GetCustInfo(custID);
            //if (model != null)
            //{
            //    if (model.TypeID == ((int)Crm2009.Entities.EnumCustomType.FourS).ToString() || model.TypeID == ((int)Crm2009.Entities.EnumCustomType.SynthesizedShop).ToString() || model.TypeID == ((int)Crm2009.Entities.EnumCustomType.Licence).ToString())
            //    {
            //        IsDealer = true;
            //    }
            //}
            //else
            //{
            //    throw new Exception("参数无效！");
            //}
        }

        /// <summary>
        /// 绑定客户所属会员名称
        /// </summary>
        /// <param name="custID"></param>
        private void BindRepterMemberNameList(string custID)
        {
            //modify by qizhiqiang
            //Crm2009.Entities.QueryDMSMember q = new Crm2009.Entities.QueryDMSMember();
            //q.CustID = custID;
            //int tc = 0;
            //DataTable dt = Crm2009.BLL.DMSMember.Instance.GetDMSMember(q, "DMSMember.CreateTime", 1, 100, out tc);
            //dt.DefaultView.RowFilter = "RowNumber<=4 OR ID='" + MemberID + "'";
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    CCDMSMemberCount = dt.DefaultView.ToTable().Rows.Count;
            //}
            //this.RepterMemberNameList.DataSource = dt;
            //this.RepterMemberNameList.DataBind();

            //dt.DefaultView.RowFilter = "RowNumber>4 And ID<>'" + MemberID + "'";
            //this.RepterMemberNameListMore.DataSource = dt.DefaultView.ToTable();
            //this.RepterMemberNameListMore.DataBind();

            string strURL = "";
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectByCustID(custID);
            //**update by qizq 2012-5-31，把车商通先去掉，如果将来要开通只需注掉下面代码
            //只看车易通
            //foreach (DataRow newRow in dt.Rows)
            //{
            //    if (newRow["type"].ToString() == "0")
            //    {
            //        newRow.Delete();
            //    }
            //}
            //dt.AcceptChanges();
            //**
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

                    if (i == 2 && dt.Rows.Count > 3)
                    {
                        literalMemberNameList.Text += "<li class=\"select_box\" style='width:20px;'><span>更多会员<img src=\"/images/arrow.jpg\" width=\"7\" height=\"4\" /></span><ul class=\"s_ul\" style=\"display: none;\">";

                        //literalMemberNameList.Text += "<div class=\"main_box\"><ul>";
                        //literalMemberNameList.Text += "<li class=\"select_box\"><span>更多会员<img src=\"../../images/arrow.jpg\" width=\"7\" height=\"4\" /></span>";
                        //literalMemberNameList.Text += "<ul class=\"s_ul\">"; 
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
                        //literalMemberNameList.Text += "<li>" + "<a href=\"" + strURL + dt.Rows[i]["id"] + "&CustID=" + CustID + "\">"  + (dt.Rows[i]["pxh"].ToString() == "1" ? "易湃会员" : "车商通会员") + getAbbr(dt.Rows[i]["Abbr"] + "") + getSyncStatus(dt.Rows[i]["SyncStatus"].ToString()) + "</li>"; 

                    }
                    literalMemberNameList.Text += "</ul></li></ul></div>";
                }
                else
                {
                    literalMemberNameList.Text += "</ul></div>";
                }
            }
        }

        private void BindMemberInfo(BitAuto.YanFa.Crm2009.Entities.DMSMember memberInfo)
        {
            this.spanMemberName.InnerText = memberInfo.Name;
            this.spanAbbrName.InnerText = memberInfo.Abbr;
            this.spanMemberCode.InnerText = memberInfo.MemberCode;
            this.spanPhone.InnerText = memberInfo.Phone;
            this.spanFax.InnerText = memberInfo.Fax;
            this.spanCompanyWebSite.InnerText = memberInfo.CompanyWebSite;
            this.spanEmail.InnerText = memberInfo.Email;
            this.spanPostcode.InnerText = memberInfo.Postcode;
            this.spanArea.InnerText = memberInfo.ProvinceName + " " + memberInfo.CityName + " " + memberInfo.CountyName;
            this.spanAddress.InnerText = memberInfo.ContactAddress;
            this.spanTrafficInfo.InnerText = memberInfo.TrafficInfo;
            if (memberInfo.MapCoordinateList.Count > 0)
            {
                this.Lat = memberInfo.MapCoordinateList[0].Latitude;
                this.Lng = memberInfo.MapCoordinateList[0].Longitude;
            }

            #region 2013.12.25 去掉车商通会员前代码
            /*
            if (memberInfo.MemberType == "1")
            {
                spanMemberType.InnerText = "4S";
            }
            else if (memberInfo.MemberType == "2")
            {
                spanMemberType.InnerText = "特许经销商";
            }
            else if (memberInfo.MemberType == "3")
            {
                spanMemberType.InnerText = "综合店";
            }
            else if (memberInfo.MemberType == "4")
            {
                spanMemberType.InnerText = "车易达";
            }
            else if (memberInfo.MemberType == "5")
            {
                spanMemberType.InnerText = "二手车中心";
            }
            
            */
            #endregion

            #region 2013.12.25 去掉车商通会员后代码
            if (string.IsNullOrEmpty(memberInfo.MemberType))
            {
                spanMemberType.InnerText = "";
            }
            else
            {
                spanMemberType.InnerText = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberTypeText(memberInfo.MemberType);
            }
            #endregion

            this.spanBrand.InnerText = memberInfo.BrandNames;
            this.spanSerialOfBrand.InnerText = memberInfo.SerialOfBrandNames;
            this.spanSerial.InnerText = memberInfo.SerialNames;
            this.spanEnterpriseBrief.InnerText = memberInfo.EnterpriseBrief;
            this.spanRemarks.InnerText = memberInfo.Remarks;

            this.spanSyncStatus.InnerText = BitAuto.YanFa.Crm2009.BLL.DMSMember.GetSyncStatusDescription(memberInfo.SyncStatus);
            if ((EnumDMSSyncStatus)memberInfo.SyncStatus == EnumDMSSyncStatus.Rejected)
            {
                this.spanSyncStatus.InnerText += ("(" + BitAuto.YanFa.Crm2009.BLL.DMSMember.GetCurrentLogDescription(memberInfo.ID) + ")");
            }
            this.NotApplyFor = BitAuto.YanFa.Crm2009.BLL.DMSMember.NotApplyFor((BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus)memberInfo.SyncStatus);
            this.IsApplyForSuccessful = BitAuto.YanFa.Crm2009.BLL.DMSMember.IsApplyForSuccessful((BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus)memberInfo.SyncStatus);
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
    }
}