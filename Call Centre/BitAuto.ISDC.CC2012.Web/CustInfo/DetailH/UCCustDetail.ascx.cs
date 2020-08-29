using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using System.Data;
using System.Collections;
using BitAuto.YanFa.Crm2009.Entities;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailH
{
    public partial class UCCustDetail : System.Web.UI.UserControl
    {
        public string SysUrl = System.Configuration.ConfigurationManager.AppSettings["SysUrl"].ToString();
        public string CrmUrl = System.Configuration.ConfigurationManager.AppSettings["CrmUrl"].ToString();
        private BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo;
        /// <summary>
        /// 客户信息
        /// </summary>
        public BitAuto.YanFa.Crm2009.Entities.CustInfo CustInfo { get { return custInfo; } set { custInfo = value; } }

        private string tId = "";
        public string TID { get { return Request["TID"]; } set { tId = value; } }

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
        public int CarType = -1;
        //自用
        public string CustID = "";
        public string BLAnnualSurvey = "";
        public int StarLevel = 0;

        public string tabI
        {
            get { return HttpContext.Current.Request["i"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["i"].ToString()); }
        }
        public string TypeID = "";
        /// <summary>
        /// 当前会员数量
        /// </summary>
        public int CCDMSMemberCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomerInfo();
                if (this.CustInfo != null)
                {
                    CarType = this.CustInfo.CarType;
                    TypeID = custInfo.TypeID;
                    BindRepterMemberNameList(this.CustInfo.CustID);
                    if (this.ListOfContact == false && this.ListOfCooperationProjects == false && this.ListOfCustUser == false && this.ListOfReturnVisit == false && this.ListOfBusinessScaleInfo == false && this.ListOfTaskRecord == false)
                    {
                        //this.divMoreInfoOfCust.Visible = false;
                    }
                    //BLL.CrmCustInfo.Instance.GetCC_CrmCustInfo(
                    //Entities.QueryCC_Tasks query = new Entities.QueryCC_Tasks();
                    //query.=this.CustInfo.CustID;
                    //query.Source=2;
                    //query.Status=0;
                    //int totalCount=0;
                    //BLL.CC_Tasks.Instance.GetCC_Tasks(query, "", 1, 1, out totalCount);
                }
                if (spanCarType.InnerText == "新车")//如果是新车，二手车规模标签项隐藏！
                {
                    ListOfBusinessScaleInfo = false;
                }
            }
        }

        private void BindCustomerInfo()
        {
            if (this.CustInfo != null)
            {
                this.CustID = this.CustInfo.CustID;

                spanCustName.InnerText = this.CustInfo.CustName;
                spanAbbrName.InnerText = this.CustInfo.AbbrName;
                spanAddress.InnerText = this.CustInfo.Address;
                if (!string.IsNullOrEmpty(CustInfo.CreateTime))
                {
                    spanCreateTime.InnerText = CustInfo.CreateTime;
                }
                spanCreateUserName.InnerText = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(CustInfo.CreateUserID);
                if (!string.IsNullOrEmpty(CustInfo.LastUpdateTime))
                {
                    spanModifyTime.InnerText = CustInfo.LastUpdateTime;
                }
                spanModifyUserName.InnerText = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(CustInfo.LastUpdateUserID);

                BitAuto.YanFa.Crm2009.Entities.QueryBrandInfo queryBrandInfo = new BitAuto.YanFa.Crm2009.Entities.QueryBrandInfo();
                queryBrandInfo.CustID = this.CustInfo.CustID;
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
                spanBrand.InnerText = s;
                spanCarType.InnerText = BLL.Util.GetCustCatTypeName(this.CustInfo.CarType);
                spanContractName.InnerText = this.CustInfo.contactName;
                spanFax.InnerText = this.CustInfo.Fax;
                if (!string.IsNullOrEmpty(this.CustInfo.IndustryID))
                {
                    spanIndustry.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.IndustryID));
                }
                if (!string.IsNullOrEmpty(this.CustInfo.LevelID))
                {
                    spanLevel.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.LevelID));
                }

                // 集团 厂商 没有 [所属集团 所属厂商]; 没有新增会员按钮
                if (this.CustInfo.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Company).ToString() || this.CustInfo.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Bloc).ToString())
                {
                    liPid.Visible = false;
                    liCustPid.Visible = false;
                }
                else
                {
                    if (this.CustInfo.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.SynthesizedShop).ToString())
                    {
                        liCustPid.Visible = false;
                        //综合店 没有厂商 
                    }
                    if (CustInfo.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence).ToString())
                    {
                        liFourS.Visible = true;
                        if (!string.IsNullOrEmpty(CustInfo.FoursPid))
                        {
                            BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(CustInfo.FoursPid);
                            if (pmodel != null)
                            {
                                spanFourPidName.InnerText = pmodel.CustName;
                            }
                        }
                    }
                }

                spanLicenseNumber.InnerText = this.CustInfo.LicenseNumber;
                //spanAS.InnerText = this.CustInfo.BLAnnualSurvey == 0 ? "未知" : (this.CustInfo.BLAnnualSurvey == 1 ? "通过" : "未通过");
                this.BLAnnualSurvey = this.CustInfo.BLAnnualSurvey.ToString();
                //信用等级
                this.StarLevel = this.CustInfo.StarLevel;


                spanNotes.InnerText = this.CustInfo.Notes;
                spanOfficeTel.InnerText = this.CustInfo.Officetel;
                if (!string.IsNullOrEmpty(this.CustInfo.Pid))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustInfo.Pid);
                    if (pmodel != null)
                    {
                        spanPid.InnerText = pmodel.CustName;
                    }
                }
                if (!string.IsNullOrEmpty(this.CustInfo.CustPid))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustInfo.CustPid);
                    if (pmodel != null)
                    {
                        spanCustPid.InnerText = pmodel.CustName;
                    }
                }
                //if (!string.IsNullOrEmpty(model.ProducerID) && model.ProducerID != "-1")
                //{
                //    Entities.ProducerInfo producerInfo = BLL.ProducerInfo.Instance.GetProducerInfo(model.ProducerID);
                //    if (model != null)
                //    {
                //        spanProducer.InnerText = producerInfo.ChinaName;
                //    }
                //}
                string provinceCity = "";
                if (!string.IsNullOrEmpty(this.CustInfo.ProvinceID))
                {
                    provinceCity += BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.CustInfo.ProvinceID);
                }
                if (!string.IsNullOrEmpty(this.CustInfo.CityID))
                {
                    provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.CustInfo.CityID);
                }
                if (!string.IsNullOrEmpty(this.CustInfo.CountyID))
                {
                    provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(this.CustInfo.CountyID);
                }
                spanProvinceCity.InnerText = provinceCity;
                //if (!string.IsNullOrEmpty(model.SaleOrgID) && model.SaleOrgID != "-1")
                //{
                //    Entities.SalesNetwork salesNetwork = BLL.SalesNetwork.Instance.GetSalesNetwork(model.SaleOrgID);
                //    if (model != null)
                //    {
                //        spanSaleNetwork.InnerText = salesNetwork.SnName;
                //    }
                //}
                if (!string.IsNullOrEmpty(this.CustInfo.ShopLevel))
                {
                    spanShopLevel.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.ShopLevel));
                }
                if (!string.IsNullOrEmpty(this.CustInfo.TypeID))
                {
                    spanType.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(this.CustInfo.TypeID));
                }
                spanZipCode.InnerText = this.CustInfo.zipcode;

                if (this.CustInfo.Lock == 1)
                {
                    //spanLock.InnerText = "是";
                    spanLock.Style["display"] = "block";
                }
                else
                {
                    //spanLock.InnerText = "否";
                    spanLock.Style["display"] = "none";
                }
                if (this.CustInfo.Status == 0)
                {
                    //spanStatus.InnerText = "在用";
                    spanStatus.Style["display"] = "none";
                }
                else
                {
                    //spanStatus.InnerText = "停用";
                    spanStatus.Style["display"] = "block";
                }
                //二手车经营类型 置换型：1；零售型：2
                if (custInfo.UsedCarBusinessType == "1")
                {
                    spanUsedCarBusiness.InnerText = "置换型";
                }
                else if (custInfo.UsedCarBusinessType == "2")
                {
                    spanUsedCarBusiness.InnerText = "零售型";
                }

                //所属交易市场 
                if (!string.IsNullOrEmpty(custInfo.TradeMarketID))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo custinfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(custInfo.TradeMarketID.ToString());
                    if (custinfo != null)
                    {
                        spanTradeMarketID.InnerText = custinfo.CustName;
                    }
                }

                #region Modify=masj，Date=2012-04-13 注释掉
                ////车商通会员ID
                //Crm2009.Entities.QueryCstMember queryCstMember = new Crm2009.Entities.QueryCstMember();
                //queryCstMember.CustID = custInfo.CustID;
                //queryCstMember.Status = 0;

                //DataTable dtCstMember = Crm2009.BLL.CstMember.Instance.GetTable(queryCstMember);
                //string[] strCstMemberID = new string[dtCstMember.Rows.Count];
                ////ArrayList arr_cstMember = new ArrayList();

                //for (int k = 0; k < dtCstMember.Rows.Count; k++)
                //{
                //    strCstMemberID[k]=dtCstMember.Rows[k]["CstMemberID"].ToString();
                //}
                //if (strCstMemberID != null)
                //{
                //    spanCstMemberID.InnerHtml = "";
                //    for (int k = 0; k < strCstMemberID.Length; k++)
                //    {
                //        spanCstMemberID.InnerHtml += "<a href='/CustCheck/CSTMember/CSTMemberInfo.aspx?CSTMemberID=" + strCstMemberID[k] + "' target='_blank'>" + strCstMemberID[k] + "</a>";
                //        if (k != strCstMemberID.Length - 1)
                //        {
                //            spanCstMemberID.InnerHtml += ",";
                //        }
                //    }
                //}
                #endregion
            }
        }

        /// <summary>
        /// 绑定会员信息
        /// </summary>
        /// <param name="custID"></param>
        private void BindRepterMemberNameList(string custID)
        {
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
                        literalMemberNameList.Text += "<li class=\"select_box\" style='width:20px;' id='liSelect'><span>更多会员<img src=\"/images/arrow.jpg\" width=\"7\" height=\"4\" /></span><ul class=\"s_ul\" style=\"display: none;\">";
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
    }
}