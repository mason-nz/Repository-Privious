﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.CustInfo;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class UCEditCust : System.Web.UI.UserControl
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

        //页面初始化时的地区
        public string InitialProvinceID = "";
        public string InitialCityID = "";
        public string InitialCountyID = "";
        public string OriginalCustID = "";

        public string CarType = "";

        public string TaskID = "";

        //在CRM中是否有会员
        public bool HasMembersInCrm = false;
        //是否显示新增会员
        public bool IsShowAddMembers = true;

        //是否存在表CC_DelCustRelation记录
        public bool IsExistDelCustRelationRecord = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlData();
                LoadTaskData();
                IsExistDelCustRelationRecord = BLL.ProjectTask_DelCustRelation.Instance.IsExistsByTID(TaskID);

                //问卷
                SurveyListID.RequestTaskID = TaskID;
            }
        }

        public void BindControlData()
        {
            //客户类型
            List<string[]> customType = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumCustomType));
            foreach (string[] s in customType)
            {
                if (Task.Source == 1)//Excel导入客户
                {
                    switch (int.Parse(s[1]))
                    {
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Company:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Bloc:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.SP:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.CompanyRegion:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Other:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Individual:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.TradeMarket:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.BrokerageFirm:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Showroom:
                        case (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.UsedCarCenter:
                            continue;
                        default: break;
                    }
                }
                this.selCustType.Items.Add(new ListItem(s[0], s[1]));
            }
            this.selCustType.Items.Insert(0, new ListItem("请选择", "-1"));
            this.selCustType.SelectedIndex = 0;

            //客户行业
            List<string[]> customIndustry = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumCustomIndustry));
            foreach (string[] s in customIndustry)
            {
                this.selCustIndustry.Items.Add(new ListItem(s[0], s[1]));
            }
            this.selCustIndustry.Items.Insert(0, new ListItem("请选择", "-1"));
            this.selCustIndustry.SelectedIndex = 0;

            //客户级别
            List<string[]> customLevel = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumCustomLevel));
            foreach (string[] s in customLevel)
            {
                this.selCustLevel.Items.Add(new ListItem(s[0], s[1]));
            }
            this.selCustLevel.Items.Insert(0, new ListItem("请选择", "-1"));
            this.selCustLevel.SelectedIndex = 0;

            //经营店级别
            List<string[]> shopLevel = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumOperateShopLevel));
            foreach (string[] s in shopLevel)
            {
                this.selShopLevel.Items.Add(new ListItem(s[0], s[1]));
            }
            this.selShopLevel.Items.Insert(0, new ListItem("请选择", "-1"));
            this.selShopLevel.SelectedIndex = 0;

        }

        //加载任务数据
        private void LoadTaskData()
        {
            //1 没有保存过
            //1.1 来源是excel，从excel导入到CC_Custs
            //1.2 来源是crm，从crm导入到CC_Custs

            //2 已保存 从CC_Custs等表中读取信息，显示


            if (this.Task == null) { throw new Exception("必须设置Task对象"); }
            //1 没有保存过
            if (BLL.ProjectTask_Cust.Instance.Exists(this.Task.PTID) == false)
            {
                if (this.Task.Source == 1)//1.1 来源是excel，从excel导入到CC_Custs
                {
                    BLL.ProjectTask_Cust.Instance.SyncInfoFromExcel(this.Task);
                }
                else if (this.Task.Source == 2)//1.2 来源是crm，从crm导入到CC_Custs
                {
                    BLL.ProjectTask_Cust.Instance.SyncInfoFromCrm(this.Task);
                }
            }
            //2 已保存 从CC_Custs等表中读取信息，显示
            LoadInfoFromCC_Cust(this.Task);

            //Control ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditSurvey.ascx", this.Task.TID);
            //this.PlaceSurvey.Controls.Add(ctl);
        }

        /// <summary>
        /// 从CC_Custs中读取客户信息与会员信息。
        /// </summary>
        private void LoadInfoFromCC_Cust(Entities.ProjectTaskInfo task)
        {
            #region 加载客户信息
            Entities.ProjectTask_Cust ccCust = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(task.PTID);

            CarType = ccCust.CarType.ToString();

            this.OriginalCustID = ccCust.OriginalCustID;

            this.tfCustName.Value = ccCust.CustName;
            this.tfCustAbbr.Value = ccCust.AbbrName;
            ListItem li = this.selCustType.Items.FindByValue(ccCust.TypeID);
            this.selCustType.SelectedIndex = this.selCustType.Items.IndexOf(li);
            //如果客户类型为特许经销商，显示所属4S
            if (ccCust.TypeID == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence))
            {
                //lbl4S.Style["display"] = "blank";

            }
            if (task.Source == 2)//CRM库来源
            {
                selCustType.Attributes.Add("disabled", "disabled");//不能编辑客户类别    
            }

            li = this.selCustIndustry.Items.FindByValue(ccCust.IndustryID);
            this.selCustIndustry.SelectedIndex = this.selCustIndustry.Items.IndexOf(li);

            this.InitialProvinceID = ccCust.ProvinceID;
            this.InitialCityID = ccCust.CityID;
            this.InitialCountyID = ccCust.CountyID;
            this.tfAddress.Value = ccCust.Address;
            this.tfZipcode.Value = ccCust.Zipcode;
            if (!string.IsNullOrEmpty(ccCust.TradeMarketID))
            {
                BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.TradeMarketID.ToString());
                if (custInfo != null)
                {
                    tfCustPidName1.Value = custInfo.CustName;
                    tfCustPid1.Value = ccCust.TradeMarketID.ToString();
                }
            }
            #region Modify=masj，Date=2012-04-13 注释掉
            //if (ccCust.CstMemberID != "" && ccCust.CstMemberID != null)
            //{
            //    this.txtCtsMemberID.Value = ccCust.CstMemberID;
            //}
            #endregion
            if (ccCust.CarType == 3)
            {
                this.chkOldCarType.Checked = true;
                if (!string.IsNullOrEmpty(ccCust.UsedCarBusinessType))
                {
                    this.sltUsedCarBusinessType.Value = ccCust.UsedCarBusinessType.ToString();
                }
                if (!string.IsNullOrEmpty(ccCust.TradeMarketID))
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.TradeMarketID.ToString());
                    if (custInfo != null)
                    {
                        tfCustPidName1.Value = custInfo.CustName;
                        tfCustPid1.Value = ccCust.TradeMarketID.ToString();
                    }
                }
            }

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

            li = this.selCustLevel.Items.FindByValue(ccCust.LevelID);
            this.selCustLevel.SelectedIndex = this.selCustLevel.Items.IndexOf(li);

            BitAuto.YanFa.Crm2009.Entities.CustInfo crmCust = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.CustPid);
            if (crmCust != null) { this.tfCustPid.Value = crmCust.CustID; this.tfCustPidName.Value = crmCust.CustName; }

            crmCust = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.Pid);
            if (crmCust != null) { this.tfPid.Value = crmCust.CustID; this.tfPidName.Value = crmCust.CustName; }

            crmCust = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ccCust.FoursPid);
            if (crmCust != null) { this.tf4sPid.Value = crmCust.CustID; this.tf4sName.Value = crmCust.CustName; }


            li = this.selShopLevel.Items.FindByValue(ccCust.ShopLevel);
            this.selShopLevel.SelectedIndex = this.selShopLevel.Items.IndexOf(li);

            this.tfBrand.Value = BLL.Util.List2String(ccCust.BrandIDs, ",", "", "");
            this.tfBrandName.Value = BLL.Util.List2String(ccCust.BrandNames, ",", "", "");

            this.tfOfficeTel.Value = ccCust.OfficeTel;
            this.tfFax.Value = ccCust.Fax;
            this.tfContactName.Value = ccCust.ContactName;
            this.tfNotes.Value = ccCust.Notes;

            #endregion

            #region 加载会员信息(如果没有会员，并且从excel中加载，加载一个空会员控件)
            List<Entities.ProjectTask_DMSMember> ccMembers = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(task.PTID);
            if (ccMembers.Count > 0)
            {
                Control ctl;
                foreach (Entities.ProjectTask_DMSMember m in ccMembers)
                {
                    //在CRM中有会员
                    if (string.IsNullOrEmpty(m.OriginalDMSMemberID) == false) { HasMembersInCrm = true; }

                    ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditMember.ascx", m);
                    this.PlaceHolder.Controls.Add(ctl);
                }
            }
            else
            {
                if (task.Source == 1)
                {
                    Control ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditMember.ascx", task.PTID);
                    this.PlaceHolder.Controls.Add(ctl);
                }
            }
            #endregion

            #region 加载车商通会员信息
            //如果是新车二手车或二手车
            if (ccCust.CarType == 2 || ccCust.CarType == 3)
            {
                Entities.ProjectTask_CSTMember queryCstMember = new Entities.ProjectTask_CSTMember();

                List<Entities.ProjectTask_CSTMember> ccCstMembers = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(task.PTID);
                if (ccCstMembers.Count > 0)
                {
                    Control ctl;
                    foreach (Entities.ProjectTask_CSTMember m in ccCstMembers)
                    {
                        ////在CRM中有会员
                        //if (string.IsNullOrEmpty(m.OriginalDMSMemberID) == false) { HasMembersInCrm = true; }

                        ctl = this.LoadControl("~/CustInfo/EditVWithCalling/UCEditCstMember.ascx", m);
                        this.PlaceHolderCstMember.Controls.Add(ctl);
                    }
                }
            }
            //如果是新车客户，显示车商通的会员
            if (ccCust.CarType == 1)
            {
                List<Entities.ProjectTask_CSTMember> ccMemberList = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(this.Task.PTID);

                Control ctl;
                foreach (Entities.ProjectTask_CSTMember m in ccMemberList)
                {
                    ctl = this.LoadControl("~/CustInfo/DetailV/UCCstMemberFromCC.ascx", m);
                    this.PlaceHolderShowCstMember.Controls.Add(ctl);
                }
            }
            #endregion
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