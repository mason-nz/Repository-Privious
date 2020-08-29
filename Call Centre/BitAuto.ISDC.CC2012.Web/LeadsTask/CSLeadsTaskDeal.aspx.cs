using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.YanFa.Crm2009.Entities;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.LeadsTask
{
    public partial class CSLeadsTaskDeal : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //任务实体
        public Entities.LeadsTask model = new Entities.LeadsTask();

        public string BGID = "20";
        public string SCID = "353";


        //地区
        public string PlaceStr = string.Empty;
        //任务ＩＤ
        public string TaskID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["TaskID"]))
                {
                    return Request["TaskID"];
                }
                else
                {
                    return string.Empty;

                }
            }
        }
        //下单品牌，车型，车款组合
        public string OrderCarInfo = string.Empty;
        //需匹配车型
        public string DCarInfo = string.Empty;
        //需求单查看页面
        public string DemandDetailsUrl = System.Configuration.ConfigurationManager.AppSettings["CJKDemandDetailsUrl"].ToString();
        public int userid;
        //项目名称
        public string ProjectName;
        //客户名称
        public string CustName;
        //需求品牌
        public string DBrand;
        public string DBrandID;
        public string tel;

        public string ProvinceName;
        public string CityName;
        //************************
        public int intIsBoughtCar = -1;

        public int intBoughtCarMasterID = -1;
        public string strBoughtCarMaster = "";
        public int intBoughtCarSerialID = -1;
        public string strBoughtCarSerial = "";
        public string strBoughtCarYear = "";
        public string strBoughtCarMonth = "";
        public string strBoughtCarDealerID = "";
        public string strBoughtCarDealerName = "";
        //未购车
        public int intHasBuyCarPlan = -1;
        public int intIsAttention = -1;
        public int intIsContactedDealer = -1;
        public int intIsSatisfiedService = -1;
        public string strContactedWhichDealer = "";

        public int intIntentionCarMasterID = -1;
        public string strIntentionCarMaster = "";
        public int intIntentionCarSerialID = -1;
        public string strIntentionCarSerial = "";

        public string ALLNotEstablishReasonStr = "";
        public string ALLNotSuccessReasonStr = "";
        public string BoolStr = "1|是;0|否";


        public string IsPass = "-1";
        public string IsSuccess = "-1";
        public string FailReason = "-1";
        public string NotEstablishReason = "-1";
        public string BlackWhiteList = "-1";
        protected void Page_Load(object sender, EventArgs e)
        {

            DemandDetailsUrl += "&OrderCode={0}";
            if (!IsPostBack)
            {
                ALLNotEstablishReasonStr = CallResult_ORIG_Task.GetNotEstablishReasonStr();
                BlackWhiteList = Convert.ToInt16(Entities.NotEstablishReason.N05_免打扰屏蔽).ToString();
                List<int> filterList = new List<int>() { 24, 1, 10, 16, 12, 13, 11, 14, 6, 8, 9, 17, 25, 26, 27, 28, 23 };
                ALLNotSuccessReasonStr = CallResult_ORIG_Task.GetNotSuccessReasonStr(filterList);
                userid = BLL.Util.GetLoginUserID();
                model = BLL.LeadsTask.Instance.GetLeadsTask(TaskID);

                if (model == null)
                {
                    #region 任务不存在
                    Response.Write(@"<script language='javascript'>alert('该任务不存在，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                    #endregion
                }
                else
                {
                    this.Remark.Value = model.Remark.ToString();
                    if (model.AssignUserID != BLL.Util.GetLoginUserID())
                    {
                        #region 当前人不是处理人
                        Response.Write(@"<script language='javascript'>alert('您不是该任务的当前处理人，页面将被关闭。');try {
                                                                               window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                                           }
                                                                           catch (e) {
                                                                               window.opener = null; window.open('', '_self'); window.close();
                                                                           }</script>");
                        #endregion
                    }
                    //判断是否是处理状态
                    else if (model.Status != (int)Entities.LeadsTaskStatus.Processing && model.Status != (int)Entities.LeadsTaskStatus.NoProcess)
                    {
                        #region 任务不在处理状态
                        Response.Write(@"<script language='javascript'>alert('该任务不处于处理状态，页面将被关闭。');try {
                                                                               window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                                           }
                                                                           catch (e) {
                                                                               window.opener = null; window.open('', '_self'); window.close();
                                                                           }</script>");
                        #endregion
                    }
                    else
                    {
                        //项目名称
                        Entities.ProjectInfo ProjectInfoModel = BLL.ProjectInfo.Instance.GetProjectInfo(model.ProjectID);
                        ProjectName = ProjectInfoModel.Name;

                        //性别
                        int Sex = -2;
                        Sex = (int)model.Sex;
                        if (Sex == 1)
                        {
                            this.rdoMan.Checked = true;
                        }
                        else if (Sex == 2)
                        {
                            this.rdoWomen.Checked = true;
                        }
                        //下单地区
                        ProvinceName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.ProvinceID.ToString());
                        CityName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CityID.ToString());

                        //加载客户名称，需匹配车型列表
                        GetDemandIDInfo(model.DemandID);
                        //绑定失败原因
                        BindFailReason();
                        //绑定预计购车时间
                        BindPlanBuyCarTime();
                        //根据Leads处理实体加载地区，下单车型，需匹配车型信息
                        LoadLeadInfo(model);

                        //绑定月份
                        BindLatelyThreeYear();

                        intIsBoughtCar = model.IsBoughtCar.HasValue ? model.IsBoughtCar.Value : -1;

                        intBoughtCarMasterID = model.BoughtCarMasterID.HasValue ? model.BoughtCarMasterID.Value : -1;
                        strBoughtCarMaster = model.BoughtCarMaster;
                        intBoughtCarSerialID = model.BoughtCarSerialID.HasValue ? model.BoughtCarSerialID.Value : -1;
                        strBoughtCarSerial = model.BoughtCarSerial;
                        if (!string.IsNullOrEmpty(model.BoughtCarYearMonth))
                        {
                            string[] strYM = model.BoughtCarYearMonth.Split(',');
                            if (strYM.Length == 2)
                            {
                                strBoughtCarYear = strYM[0];
                                strBoughtCarMonth = strYM[1];
                            }
                        }
                        strBoughtCarDealerID = model.BoughtCarDealerID;
                        strBoughtCarDealerName = model.BoughtCarDealerName;
                        //未购车
                        intHasBuyCarPlan = model.HasBuyCarPlan.HasValue ? model.HasBuyCarPlan.Value : -1;
                        intIsAttention = model.IsAttention.HasValue ? model.IsAttention.Value : -1;
                        intIsContactedDealer = model.IsContactedDealer.HasValue ? model.IsContactedDealer.Value : -1;
                        intIsSatisfiedService = model.IsSatisfiedService.HasValue ? model.IsSatisfiedService.Value : -1;
                        strContactedWhichDealer = model.ContactedWhichDealer;

                        intIntentionCarMasterID = model.IntentionCarMasterID.HasValue ? model.IntentionCarMasterID.Value : -1;
                        strIntentionCarMaster = model.IntentionCarMaster;
                        intIntentionCarSerialID = model.IntentionCarSerialID.HasValue ? model.IntentionCarSerialID.Value : -1;
                        strIntentionCarSerial = model.IntentionCarSerial;

                    }

                }
            }
        }

        private void BindLatelyThreeYear()
        {
            DateTime dtNow = DateTime.Now;

            ListItem[] items = {
                                   new ListItem() { Value = "-1", Text = "请选择年份" },
                                   new ListItem() { Value = dtNow.Year.ToString(), Text = dtNow.Year.ToString() + "年" },
                                   new ListItem() { Value = dtNow.AddYears(-1).Year.ToString(), Text = dtNow.AddYears(-1).Year.ToString() + "年" },
                                   new ListItem() { Value = dtNow.AddYears(-2).Year.ToString(), Text = dtNow.AddYears(-2).Year.ToString() + "年" }
                               };
            selBoughtYear.DataSource = items;
            selBoughtYear.DataBind();
        }
        /// <summary>
        /// 取广告需求单基本信息,客户名称，需匹配车型
        /// </summary>
        /// <param name="DemandID"></param>
        protected void GetDemandIDInfo(string DemandID)
        {
            BitAuto.YanFa.Crm2009.Entities.CJKDemandInfo cjkmodel = null;
            cjkmodel = BitAuto.YanFa.Crm2009.BLL.CJKDemandBll.Instance.GetCJKDemandInfo(DemandID);
            if (cjkmodel != null)
            {
                CustName = cjkmodel.CustName;
                DBrand = cjkmodel.BrandName;
                DBrandID = cjkmodel.BrandID.ToString();

                string SerialIDS = string.Empty;
                string SerialNames = string.Empty;
                SerialIDS = cjkmodel.SerialIDs;
                SerialNames = cjkmodel.SerialNames;
                //如果厂商需求单的需匹配车型为空，那边cc根据品牌加载品牌下所有车型
                if (!string.IsNullOrEmpty(SerialIDS.Trim()) && !string.IsNullOrEmpty(SerialNames.Trim()))
                {
                    string[] serialidarry = SerialIDS.Split(',');
                    string[] SerialNamesarry = SerialNames.Split(',');
                    int len = Math.Min(serialidarry.Length, SerialNamesarry.Length);
                    for (int i = 0; i < len; i++)
                    {
                        this.dllCarSerial.Items.Add(new ListItem(SerialNamesarry[i], serialidarry[i]));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(DBrandID))
                    {
                        DataTable dt = null;
                        dt = BLL.LeadsTask.Instance.GetSerialByBrandID(cjkmodel.BrandID);
                        if (dt != null)
                        {
                            this.dllCarSerial.DataSource = dt;
                            this.dllCarSerial.DataTextField = "Name";
                            this.dllCarSerial.DataValueField = "serialID";
                            this.dllCarSerial.DataBind();
                        }
                    }
                }
                BindTargetAreaData(cjkmodel.CJKAreaQuantityDetailList);
            }
        }
        /// <summary>
        /// 绑定失败原因
        /// </summary>
        protected void BindFailReason()
        {
            //this.selFailReson.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.CJKLeadTaskFailReason));
            //selFailReson.DataTextField = "name";
            //selFailReson.DataValueField = "value";
            //selFailReson.DataBind();
            //selFailReson.Items.Insert(0, new ListItem("请选择", "-2"));
        }

        /// <summary>
        /// 预计购车时间
        /// </summary>
        protected void BindPlanBuyCarTime()
        {
            this.selWantBuyCarTime.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.LeadPlanBuyCarTime));
            selWantBuyCarTime.DataTextField = "name";
            selWantBuyCarTime.DataValueField = "value";
            selWantBuyCarTime.DataBind();
            selWantBuyCarTime.Items.Insert(0, new ListItem("请选择", "-2"));
        }

        /// <summary>
        /// 根据Leads处理实体加载信息
        /// </summary>
        /// <param name="model"></param>
        protected void LoadLeadInfo(Entities.LeadsTask model)
        {
            this.spantxtCustName.Value = model.UserName;
            //下单车型
            if (!string.IsNullOrEmpty(model.OrderCarMaster))
            {
                OrderCarInfo += model.OrderCarMaster + " ";
            }
            if (!string.IsNullOrEmpty(model.OrderCarSerial))
            {
                OrderCarInfo += model.OrderCarSerial + " ";
            }
            OrderCarInfo = OrderCarInfo.TrimEnd(' ');
            //目标车型
            if (!string.IsNullOrEmpty(model.DCarMaster))
            {
                DCarInfo += model.DCarMaster + " ";
            }
            if (!string.IsNullOrEmpty(model.DCarSerial))
            {
                DCarInfo += model.DCarSerial + " ";
            }
            DCarInfo = DCarInfo.TrimEnd(' ');
            //是否成功
            if (model.IsSuccess != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                if (model.IsSuccess == 1)
                {
                    //this.rdoSuccess.Checked = true;
                    IsSuccess = "1";
                }
                else if (model.IsSuccess == 0)
                {
                    //this.rdoFail.Checked = true;
                    IsSuccess = "0";
                }
            }
            //加载失败原因
            if (model.NotSuccessReason != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                FailReason = model.NotSuccessReason.ToString();
                //var li = this.selFailReson.Items.FindByValue(model.FailReason.ToString());
                //this.selFailReson.SelectedIndex = this.selFailReson.Items.IndexOf(li);
            }
            //加载未接通原因
            if (model.NotEstablishReason != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                NotEstablishReason = model.NotEstablishReason.ToString();
                //var li = this.selFailReson.Items.FindByValue(model.FailReason.ToString());
                //this.selFailReson.SelectedIndex = this.selFailReson.Items.IndexOf(li);
            }
            //加载需匹配车型
            if (model.DCarSerialID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                var li = this.dllCarSerial.Items.FindByValue(model.DCarSerialID.ToString());
                this.dllCarSerial.SelectedIndex = this.dllCarSerial.Items.IndexOf(li);
            }
            //是否接通
            if (model.IsJT != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                if (model.IsJT == 1)
                {
                    //this.rdoy.Checked = true;
                    IsPass = "1";
                }
                else if (model.IsJT == 0)
                {
                    //this.rdon.Checked = true;
                    IsPass = "0";
                }
            }

            //加载预计购车时间
            if (model.PBuyCarTime != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                var li = this.selWantBuyCarTime.Items.FindByValue(model.PBuyCarTime.ToString());
                this.selWantBuyCarTime.SelectedIndex = this.selWantBuyCarTime.Items.IndexOf(li);
            }


        }

        protected void BindTargetAreaData(List<CJKAreaQuantityDetailInfo> list)
        {
            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn("ProvinceID", typeof(string));
            DataColumn col2 = new DataColumn("ProvinceName", typeof(string));
            dt.Columns.AddRange(new DataColumn[] { col1, col2 });

            DataRow row = dt.NewRow();
            row["ProvinceID"] = model.TargetProvinceID;
            row["ProvinceName"] = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.TargetProvinceID.ToString());
            dt.Rows.Add(row);

            foreach (CJKAreaQuantityDetailInfo mod in list)
            {
                if (mod.Unit == "省")
                {
                    DataRow newRow = dt.NewRow();
                    newRow["ProvinceID"] = mod.AreaID;
                    newRow["ProvinceName"] = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(mod.AreaID);

                    dt.Rows.Add(newRow);
                }
                else if (mod.Unit == "市")
                {
                    DataRow newRow = dt.NewRow();
                    newRow["ProvinceID"] = CC2012.BLL.YTGActivity.Instance.GetAreaParentID(int.Parse(mod.AreaID)).ToString();
                    newRow["ProvinceName"] = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(newRow["ProvinceID"].ToString());
                    dt.Rows.Add(newRow);
                }
            }

            DataView dv = new DataView(dt);
            dt = dv.ToTable(true);

            ddlTargetProvince.DataSource = dt;
            ddlTargetProvince.DataValueField = "ProvinceID";
            ddlTargetProvince.DataTextField = "ProvinceName";

            ddlTargetProvince.DataBind();
            ddlTargetProvince.SelectedIndex = 0;
        }

    }
}