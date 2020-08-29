using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class UCSecondCarView : System.Web.UI.UserControl
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
        public string CSTMemberID = "";

        #region innernal

        public string TaskID = "";
        public Entities.ProjectTask_Cust CCCust;
        public int CarType = -1;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Task == null) { return; }

                BindCCCustomerInfo();
                BindCstMemberList();
                //问卷
                SurveyListID.RequestTaskID = TaskID;
            }
        }


        private void BindCCCustomerInfo()
        {
            Entities.ProjectTask_Cust ccCust = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(this.Task.PTID);
            CCCust = ccCust;
            #region 客户信息
            if (ccCust != null)
            {
                CarType = ccCust.CarType;
                spanCustName.InnerText = ccCust.CustName;
                spanCustAbbr.InnerText = ccCust.AbbrName;
                spanAddress.InnerText = ccCust.Address;
                //经营范围
                if (ccCust.CarType == 1)
                {
                    spanCarType.InnerText = "新车";
                }
                else if (ccCust.CarType == 2)
                {
                    spanCarType.InnerText = "二手车";
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
                ////车商通会员ID
                //if (!string.IsNullOrEmpty(ccCust.CstMemberID))
                //{
                //    spanCstMemberID.InnerHtml = "<a href='/CustCheck/CSTMember/CSTMemberInfo.aspx?CSTMemberID=" + ccCust.CstMemberID + "' target='_blank'>" + ccCust.CstMemberID.ToString() + "</a>";
                //    CSTMemberID = spanCstMemberID.InnerText;
                //}
                //else
                //{
                //    spanCstMemberID.InnerText = "";
                //}

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
                if (ccCust.TradeMarketID != null)
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo custinfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.TradeMarketID.ToString());
                    if (custinfo != null)
                    {
                        spanTradeMarketID.InnerText = custinfo.CustName;
                    }
                }

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


                spanNotes.InnerText = ccCust.Notes;
                spanOfficeTel.InnerText = ccCust.OfficeTel;
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
                if (!string.IsNullOrEmpty(ccCust.TypeID))
                {
                    spanCustType.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(ccCust.TypeID));
                }
                spanZipcode.InnerText = ccCust.Zipcode;

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
            #endregion

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
            List<Entities.ProjectTask_CSTMember> ccMembers = BLL.CC_CSTMember.Instance.GetCC_CSTMemberByTID(this.Task.PTID);

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