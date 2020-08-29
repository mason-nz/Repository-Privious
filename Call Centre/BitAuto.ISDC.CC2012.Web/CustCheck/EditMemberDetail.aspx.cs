using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.CustCheck
{
    public partial class EditMemberDetail : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性 
        public string TID
        {
            get
            {
                return Request["TID"] == null ? string.Empty : Request["TID"].ToString();
            }
        }

        private string MemberID
        {
            get
            {
                return Request["MemberID"] == null ? string.Empty : Request["MemberID"].ToString();
            }
        }

        public string MemberType
        {
            get
            {
                return Request["Type"] == null ? string.Empty : Request["Type"].ToString();
            }
        }

        public string btnDisplay;

        public string OriginalDMSMemberID;
        public string OriginalCSTRecID;
        public Entities.ProjectTaskInfo Task;
        public int CarType = -1;

        public int imgLock = 0;
        public int imgStatusLock = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindCustInfo();
                    //BindQuestionnaireInfo();
                    LoadMemberInfo();
                }
            }
            catch (Exception ex)
            {
                //日志
            }
        }


        #region 客户信息
        private void BindCustInfo()
        {
            Entities.ProjectTask_Cust query = new Entities.ProjectTask_Cust(); 
            if (TID != string.Empty)
            {
                Task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(TID);
                Entities.ProjectTask_Cust ccCust = BLL.ProjectTask_Cust.Instance.GetCustInfoModelByPTID(TID);
                if (ccCust != null)
                {
                    CarType = ccCust.CarType;
                    spanCustName.InnerText = ccCust.CustName;
                    spanCustAbbr.InnerText = ccCust.AbbrName;
                    spanAddress.InnerText = ccCust.Address;
                    hdnCustType.Value = ccCust.TypeID;
                    spanBrandName.InnerText = BitAuto.ISDC.CC2012.Web.WebUtil.Converter.List2String(ccCust.BrandNames, ",", "", "");
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
                    if (!string.IsNullOrEmpty(ccCust.ShopLevel))
                    {
                        spanShopLevel.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(ccCust.ShopLevel));
                    }
                    if (!string.IsNullOrEmpty(ccCust.TypeID))
                    {
                        spanCustType.InnerText = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(Convert.ToInt32(ccCust.TypeID));
                        custid.InnerText = ccCust.TypeID;
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
                                    imgStatusLock = 1;
                                    break;
                                case 1:
                                    imgCustStatus.Src = "/Images/xt_1.gif";
                                    imgCustStatus.Attributes.Add("title", "停用");
                                    imgStatusLock = 1;
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (ccCust.Lock != null && ccCust.Lock.Value == 1)
                        {
                            imgCustStatusLock.Src = "/Images/lock.gif";
                            imgCustStatusLock.Attributes.Add("title", "锁定");
                            imgLock = 1;
                        }
                        else if (ccCust.Lock != null && ccCust.Lock.Value == 0)
                        {
                            imgCustStatusLock.Src = "/Images/unlock.gif";
                            imgCustStatusLock.Attributes.Add("title", "未锁定");
                            imgLock = 1;
                        }

                        liCustStatus.Style.Remove("display");
                        liCustLock.Style.Remove("display");
                    }
                }
            }
            else
            {
                throw new Exception("无法找到此任务");
            }
        }
        #endregion

        #region 调查问卷信息
        //private void BindQuestionnaireInfo()
        //{
        //    repeaterQ.DataSource = BLL.CC_Questionnaire.Instance.GetCC_Questionnaire(string.Empty, "CreateTime");
        //    repeaterQ.DataBind();
        //}

        //protected void repeaterQ_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        int qustID = int.Parse(DataBinder.Eval(e.Item.DataItem, "QustID").ToString().Trim());
        //        Repeater rOpt = e.Item.FindControl("repeaterOpt") as Repeater;
        //        rOpt.DataSource = BLL.CC_QuestionnairOption.Instance.GetCC_QuestionnairOption(" QustID=" + qustID, "CreateTime");
        //        rOpt.DataBind();
        //    }
        //}

        //protected string IsCheck(string custId, string optId)
        //{
        //    string result = string.Empty;
        //    Entities.CC_TaskQuestionnaire taskQ = BLL.CC_TaskQuestionnaire.Instance.GetCC_TaskQuestionnaire(OriginalDMSMemberID, int.Parse(custId), 1);
        //    if (taskQ != null)
        //    {
        //        if (taskQ.QustOptID == int.Parse(optId))
        //        {
        //            result = "checked=true;";
        //        }
        //    }

        //    return result;
        //}
        //protected string GetQustDescript( string custId)
        //{
        //    string result = string.Empty;
        //    Entities.CC_TaskQuestionnaire taskQ = BLL.CC_TaskQuestionnaire.Instance.GetCC_TaskQuestionnaire(OriginalDMSMemberID, int.Parse(custId), 1);
        //    if (taskQ != null)
        //    {
        //        result = taskQ.Description;
        //    }

        //    return result;
        //}
        #endregion

        private void LoadMemberInfo()
        {
            Control ctl;
            Control ct2;
            int memberId = -1;

            if (int.TryParse(MemberID, out memberId))
            {
                //车易通
                Entities.ProjectTask_DMSMember m = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMember(memberId);
                //4-19 正式测试注释去掉
                if (m != null)
                {
                    if (m.Status != -1)
                    {
                        OriginalDMSMemberID = m.OriginalDMSMemberID;
                        if (OriginalDMSMemberID != "")
                        {
                            BitAuto.YanFa.Crm2009.Entities.DMSMember dmsM = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(m.OriginalDMSMemberID));
                            if (dmsM != null)
                            {
                                if (dmsM.SyncStatus != (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.Rejected)
                                {
                                    btnDisplay = "style='display:none;'";
                                    ctl = this.LoadControl("~/CustInfo/DetailV/UCMember.ascx", m); 
                                }
                                else
                                {
                                    btnDisplay = "style='display:block;'";
                                    ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditMember.ascx", m);
                                }
                                this.PlaceHolder.Controls.Add(ctl);
                            }
                            else
                            {
                                btnDisplay = "style='display:none;'";
                            }
                        }
                    }
                }

                //车商通
                Entities.ProjectTask_CSTMember mcst = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(memberId);
                if (mcst != null)
                {
                    if (mcst.Status != -1)
                    {
                        OriginalCSTRecID = mcst.OriginalCSTRecID;
                        if (OriginalCSTRecID != "")
                        {
                            BitAuto.YanFa.Crm2009.Entities.CstMember cstM = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCSTMember(OriginalCSTRecID);
                            if (cstM != null)
                            {
                                if (cstM.SyncStatus != (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.Rejected)
                                {
                                    btnDisplay = "style='display:none;'";
                                    ct2 = this.LoadControl("~/CustInfo/DetailV/UCCstMember.ascx", cstM);
                                }
                                else
                                {
                                    btnDisplay = "style='display:block;'";
                                    ct2 = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditSndCstMember.ascx", cstM);
                                }
                                this.CSTPlaceHolder.Controls.Add(ct2);
                            }
                            else
                            {
                                btnDisplay = "style='display:none;'";
                            }
                        }
                    }
                }
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