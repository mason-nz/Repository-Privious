using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.LeadsTask
{
    public partial class CSLeadTaskView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //任务实体
        public Entities.LeadsTask model = new Entities.LeadsTask();
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
                    //return "YJK231";
                    return string.Empty;
                }
            }
        }
        //下单品牌，车型，车款组合
        public string OrderCarInfo = string.Empty;
        //需匹配车型
        public string DCarInfo = string.Empty;
        //需匹配车款
        public string DCarName = string.Empty;
        //失败原因
        public string FailReason = string.Empty;

        //未接通原因
        public string NotEstablishReason = string.Empty;

        //接通后失败原因
        public string NotSuccessReason = string.Empty;

        //需求单查看页面
        public string DemandDetailsUrl = System.Configuration.ConfigurationManager.AppSettings["CJKDemandDetailsUrl"].ToString();
        //项目名称
        public string ProjectName;
        public string CustName;
        public string PlanBuyCarTime;

        //需求品牌
        public string DBrand;
        public string TargetProvinceName;
        public string TargetCityName;
        public string ProvinceName;
        public string CityName;

        //************************
        public string strBoughtCarYear = "";
        public string strBoughtCarMonth = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            DemandDetailsUrl += "&OrderCode={0}";
            if (!IsPostBack)
            { 
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
                    this.Remark.InnerText = model.Remark.ToString();
                    //下单地区
                    ProvinceName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.ProvinceID.ToString());
                    CityName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CityID.ToString());
                    //加载客户名称，需匹配车型列表
                    GetDemandIDInfo(model.DemandID);
                    //目标下单区
                    TargetProvinceName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.TargetProvinceID.ToString());
                    TargetCityName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.TargetCityID.ToString());
 
                    //根据Leads处理实体加载地区，下单车型，需匹配车型信息
                    LoadLeadInfo(model);
                }
            }
        }
        /// <summary>
        /// 根据Leads处理实体加载信息
        /// </summary>
        /// <param name="model"></param>
        protected void LoadLeadInfo(Entities.LeadsTask model)
        {
            //项目名称
            Entities.ProjectInfo ProjectInfoModel = BLL.ProjectInfo.Instance.GetProjectInfo(model.ProjectID);
            if (ProjectInfoModel != null)
            {
                ProjectName = ProjectInfoModel.Name;
            }


            BitAuto.YanFa.Crm2009.Entities.CJKDemandInfo cjkmodel = null;
            cjkmodel = BitAuto.YanFa.Crm2009.BLL.CJKDemandBll.Instance.GetCJKDemandInfo(model.DemandID);
            if (cjkmodel != null)
            {
                CustName = cjkmodel.CustName;
            }

            PlanBuyCarTime = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.LeadPlanBuyCarTime), Convert.ToInt32(model.PBuyCarTime));

            //地区
            if (model.ProvinceID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                PlaceStr += BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.ProvinceID.ToString());
            }
            if (model.CityID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                PlaceStr += " " + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CityID.ToString());
            }
            PlaceStr = PlaceStr.Trim(' ');
            //下单车型
            if (!string.IsNullOrEmpty(model.OrderCarMaster))
            {
                OrderCarInfo += model.OrderCarMaster + " ";
            }
            if (!string.IsNullOrEmpty(model.OrderCarSerial))
            {
                OrderCarInfo += model.OrderCarSerial + " ";
            }
            if (!string.IsNullOrEmpty(model.OrderCar))
            {
                OrderCarInfo += model.OrderCar + " ";
            }
            OrderCarInfo = OrderCarInfo.TrimEnd(' ');
            //需匹配车型
            if (!string.IsNullOrEmpty(model.DCarMaster))
            {
                DCarInfo += model.DCarMaster + " ";
            }
            if (!string.IsNullOrEmpty(model.DCarSerial))
            {
                DCarInfo += model.DCarSerial + " ";
            }
            DCarInfo = DCarInfo.TrimEnd(' ');

            ////加载失败原因
            //if (model.FailReason != Entities.Constants.Constant.INT_INVALID_VALUE)
            //{
            //    int _failreason;
            //    int.TryParse(model.FailReason.ToString(), out _failreason);
            //    FailReason = BLL.Util.GetEnumOptText(typeof(Entities.CJKLeadTaskFailReason), _failreason);
            //}

            //加载未接通原因
            if (model.NotEstablishReason != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                int _notEstablishReason;
                int.TryParse(model.NotEstablishReason.ToString(), out _notEstablishReason);
                NotEstablishReason = BLL.Util.GetEnumOptText(typeof(Entities.NotEstablishReason), _notEstablishReason);
            }

            //加载接通后失败原因
            if (model.NotSuccessReason != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                int _notSuccessReason;
                int.TryParse(model.NotSuccessReason.ToString(), out _notSuccessReason);
                NotSuccessReason = BLL.Util.GetEnumOptText(typeof(Entities.NotSuccessReason), _notSuccessReason);
            }
            ////加载需匹配车款
            //if (model.DCarID != Entities.Constants.Constant.INT_INVALID_VALUE)
            //{
            //    int _dcarid;
            //    int.TryParse(model.DCarID.ToString(), out _dcarid);
            //    DCarName = model.DCarName;
            //}

          
            string[] strYM = model.BoughtCarYearMonth.Split(',');
            if (strYM.Length == 2)
            {
                strBoughtCarYear = strYM[0];
                if (strYM[1] == "-1")
                {
                    strBoughtCarMonth = "";
                }
                else
                {
                    strBoughtCarMonth = strYM[1];
                }
            }
           
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
            }
        }
    }
}