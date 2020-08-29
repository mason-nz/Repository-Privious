using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.YanFa.Crm2009;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class UCCust : System.Web.UI.UserControl
    {
        private Entities.ProjectTaskInfo task = null;
        /// <summary>
        /// 任务
        /// </summary>
        public Entities.ProjectTaskInfo Task
        {
            get { return task; }
            set
            {
                task = value;
                TaskID = task != null ? task.PTID.ToString() : "";
            }
        }


        #region innernal

        public string TaskID = "";
        public Entities.ProjectTask_Cust CCCust;
        public int CarType = -1;
        public string TypeID = "";
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Task == null) { return; }

                BindCCCustomerInfo();
                //BindSurveyInfo();
                BindCCMemberList();

                BindCstMemberList();

                //问卷
                SurveyListID.RequestTaskID = TaskID;
            }
        }




        private void BindCCCustomerInfo()
        {
            Entities.ProjectTask_Cust ccCust = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(this.Task.PTID);
            CCCust = ccCust;
            if (ccCust != null)
            {
                CarType = ccCust.CarType;
                TypeID = ccCust.TypeID;
                spanCustName.InnerText = ccCust.CustName;
                spanCustAbbr.InnerText = ccCust.AbbrName;
                spanAddress.InnerText = ccCust.Address;

                spanBrandName.InnerText = BLL.Util.List2String(ccCust.BrandNames, ",", "", "");
                spanContactName.InnerText = ccCust.ContactName;
                spanFax.InnerText = ccCust.Fax;
                if (!string.IsNullOrEmpty(ccCust.IndustryID))
                {
                    spanCustIndustry.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(ccCust.IndustryID));
                }
                if (!string.IsNullOrEmpty(ccCust.LevelID))
                {
                    spanCustLevel.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(ccCust.LevelID));
                }

                // 集团 厂商 没有 [所属集团 所属厂商]; 没有新增会员按钮
                if (ccCust.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Company).ToString() || ccCust.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Bloc).ToString())
                {
                    liPid.Visible = false;
                    liCustPid.Visible = false;
                }
                else
                {
                    if (ccCust.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.SynthesizedShop).ToString())
                    {
                        liCustPid.Visible = false;
                        //综合店 没有厂商 
                    }
                    if (ccCust.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence).ToString())
                    {
                        liFourS.Visible = true;
                        if (!string.IsNullOrEmpty(ccCust.FoursPid))
                        {
                            BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.FoursPid);
                            if (pmodel != null)
                            {
                                spanFourPidName.InnerText = pmodel.CustName;
                            }
                        }
                    }
                }

                spanNotes.InnerText = ccCust.Notes;
                spanOfficeTel.InnerText = ccCust.OfficeTel;
                if (!string.IsNullOrEmpty(ccCust.Pid))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.Pid);
                    if (pmodel != null)
                    {
                        spanPidName.InnerText = pmodel.CustName;
                    }
                }
                if (!string.IsNullOrEmpty(ccCust.CustPid))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo pmodel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.CustPid);
                    if (pmodel != null)
                    {
                        spanCustPidName.InnerText = pmodel.CustName;
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
                if (!string.IsNullOrEmpty(ccCust.ProvinceID))
                {
                    provinceCity += BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(ccCust.ProvinceID);
                }
                if (!string.IsNullOrEmpty(ccCust.CityID))
                {
                    provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(ccCust.CityID);
                }
                if (!string.IsNullOrEmpty(ccCust.CountyID))
                {
                    provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(ccCust.CountyID);
                }
                spanArea.InnerText = provinceCity;
                //if (!string.IsNullOrEmpty(model.SaleOrgID) && model.SaleOrgID != "-1")
                //{
                //    Entities.SalesNetwork salesNetwork = BLL.SalesNetwork.Instance.GetSalesNetwork(model.SaleOrgID);
                //    if (model != null)
                //    {
                //        spanSaleNetwork.InnerText = salesNetwork.SnName;
                //    }
                //}
                if (!string.IsNullOrEmpty(ccCust.ShopLevel))
                {
                    spanShopLevel.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(ccCust.ShopLevel));
                }
                if (!string.IsNullOrEmpty(ccCust.TypeID))
                {
                    spanCustType.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(ccCust.TypeID));
                }
                spanZipcode.InnerText = ccCust.Zipcode;



                //经营范围
                if (ccCust.CarType == 1)
                {
                    spanCarType.InnerText = "新车";
                }
                else if (ccCust.CarType == 2 || ccCust.CarType == 3)
                {
                    if (ccCust.CarType == 2)
                    {
                        spanCarType.InnerText = "二手车";
                    }
                    else if (ccCust.CarType == 3)
                    {
                        spanCarType.InnerText = "新车/二手车";
                    }
                    //二手车经营类型 置换型：1；零售型：2
                    if (ccCust.UsedCarBusinessType == "1")
                    {
                        spanUsedCarBusiness.InnerText = "置换型";
                    }
                    else if (ccCust.UsedCarBusinessType == "2")
                    {
                        spanUsedCarBusiness.InnerText = "零售型";
                    }

                    //所属交易市场 
                    if (!string.IsNullOrEmpty(ccCust.TradeMarketID))
                    {
                        BitAuto.YanFa.Crm2009.Entities.CustInfo custinfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.TradeMarketID.ToString());
                        if (custinfo != null)
                        {
                            spanTradeMarketID.InnerText = custinfo.CustName;
                        }
                    }
                }
                else if (ccCust.CarType == 3)
                {
                    spanCarType.InnerText = "新车/二手车";
                }

                #region Modify=masj，Date=2012-04-13 注释掉
                ////车商通会员ID
                //if (ccCust.CstMemberID != "" && ccCust.CstMemberID != null)
                //{
                //    string[] strCstMemberID = ccCust.CstMemberID.Split(',');
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

                if (!string.IsNullOrEmpty(ccCust.OriginalCustID))
                {
                    if (ccCust.Status != null)
                    {
                        switch (ccCust.Status.Value)
                        {
                            case 0:
                                imgCustStatus.Src = "/Images/xt.gif";
                                imgCustStatus.Attributes.Add("title", "在用");
                                break;
                            case 1:
                                imgCustStatus.Src = "/Images/xt_1.gif";
                                imgCustStatus.Attributes.Add("title", "停用");
                                break;
                            default:
                                break;
                        }
                    }

                    if (ccCust.Lock != null && ccCust.Lock.Value == 1)
                    {
                        imgCustStatusLock.Src = "/Images/lock.gif";
                        imgCustStatusLock.Attributes.Add("title", "锁定");
                    }
                    else if (ccCust.Lock != null && ccCust.Lock.Value == 0)
                    {
                        imgCustStatusLock.Src = "/Images/unlock.gif";
                        imgCustStatusLock.Attributes.Add("title", "未锁定");
                    }

                    liCustStatus.Style.Remove("display");
                    liCustLock.Style.Remove("display");
                }
            }
        }

        private void BindCCMemberList()
        {
            List<Entities.ProjectTask_DMSMember> ccMembers = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(this.Task.PTID);
            if (ccMembers.Count > 0)
            {
                Control ctl;
                foreach (Entities.ProjectTask_DMSMember m in ccMembers)
                {
                    ctl = this.LoadControl("~/CustInfo/DetailV/UCMember.ascx", m);
                    this.PlaceHolder.Controls.Add(ctl);
                }
            }
        }

        private void BindCstMemberList()
        {
            List<Entities.ProjectTask_CSTMember> ccMembers = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(this.Task.PTID);

            Control ctl;
            foreach (Entities.ProjectTask_CSTMember m in ccMembers)
            {
                ctl = this.LoadControl("~/CustInfo/DetailV/UCCstMemberFromCC.ascx", m);
                this.PlaceHolderCstMembers.Controls.Add(ctl);
            }
        }

        /// <summary>
        /// 重写LoadControl，带参数。
        /// </summary>
        private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            //And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }

            // Finally return the fully initialized UC
            return ctl;
        }
    }
}