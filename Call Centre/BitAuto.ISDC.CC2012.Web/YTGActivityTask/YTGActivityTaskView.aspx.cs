using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;


namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask
{
    public partial class YTGActivityTaskView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //任务id  
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
        //易团购任务实体
        public Entities.YTGActivityTaskInfo model = new Entities.YTGActivityTaskInfo();
        public int userid;
        public int isSuccess = -2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userid = BLL.Util.GetLoginUserID();
                model = BLL.YTGActivityTask.Instance.GetComAdoInfo<YTGActivityTaskInfo>(TaskID);

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
                    if (model.Sex.HasValue)
                    {
                        string strSex = model.Sex.Value.ToString();
                        if (strSex == "1")
                        {
                            strSex = "先生";
                        }
                        else if (strSex == "2")
                        {
                            strSex = "女士";
                        }
                        else
                        {
                            strSex = "";
                        }
                        spanSex.InnerText = strSex;
                    }
                    if (model.ProvinceID.HasValue)
                    {
                        spanXiDanArea.InnerText += BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.ProvinceID.Value.ToString());
                    }
                    if (model.CityID.HasValue)
                    {
                        spanXiDanArea.InnerText +="  " + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CityID.Value.ToString());
                    }
                    if (model.ValueOrDefault_OrderCreateTime > (new DateTime(1900, 01, 01)))
                    {
                        spanXiDanDate.InnerText = model.ValueOrDefault_OrderCreateTime.ToString("yyyy-MM-dd");
                    }
                    if (model.TestDriveProvinceID.HasValue)
                    {
                        spanShiJiaArea.InnerText += BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.TestDriveProvinceID.Value.ToString());
                    }
                    if (model.TestDriveCityID.HasValue)
                    {
                        spanShiJiaArea.InnerText += "  " + BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.TestDriveCityID.Value.ToString());
                    }
                    //意向车型
                    spanYiXiangCheXing.InnerText = GetYiXiangCheXing();
                    spanPredictBuyTime.InnerText = GetPredictBuyTime();
                    if (model.IsSuccess.HasValue)
                    {
                        string strIsSuccess = model.IsSuccess.Value.ToString();
                        isSuccess = model.IsSuccess.Value;
                        if (strIsSuccess == "1")
                        {
                            strIsSuccess = "是";
                        }
                        else if (strIsSuccess == "0")
                        {
                            strIsSuccess = "否";
                        }
                        else
                        {
                            strIsSuccess = "";
                        }
                        spanIsSuccess.InnerText = strIsSuccess;
                    }

                    if (model.IsJT.HasValue)
                    {
                        string strIsJT = model.IsJT.Value.ToString();
                        if (strIsJT == "1")
                        {
                            strIsJT = "是";
                        }
                        else if (strIsJT == "0")
                        {
                            strIsJT = "否";
                        }
                        else
                        {
                            strIsJT = "";
                        }
                        spanIsConnected.InnerText = strIsJT;
                    }
                    spanFailReason.InnerText = GetFailReason();

                    spanRemark.InnerText = model.Remark;

                }
            }
        }
        /// <summary>
        /// 获取下单车型信息
        /// </summary>
        /// <returns></returns>
        protected string GetOrderCarSerialName()
        {
            List<CarSerial> list = BLL.YTGActivity.Instance.GetCarSerialsByIds(model.ValueOrDefault_OrderCarSerialID.ToString());
            string strName = "";
            foreach (CarSerial node in list)
            {
                strName += node.Name;
            }
            return strName;
        }
        /// <summary>
        /// 获取意向车型名称
        /// </summary>
        /// <param name="ActionID"></param>
        private string GetYiXiangCheXing()
        {

            List<Entities.CarSerial> list = BLL.YTGActivity.Instance.GetCarSerialsByIds(model.ValueOrDefault_DCarSerialID.ToString());

            if (list.Count > 0)
            {
                return list[0].Name;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取预计购车时间
        /// </summary>
        protected string GetPredictBuyTime()
        {
            return BLL.Util.GetEnumOptText(typeof(Entities.YTGActivityPlanBuyCarTime), model.ValueOrDefault_PBuyCarTime);

        }
        /// <summary>
        /// 获取失败原因
        /// </summary>
        protected string GetFailReason()
        {
            return BLL.Util.GetEnumOptText(typeof(Entities.YTGActivityTaskFailReason), model.ValueOrDefault_FailReason);

        }
        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <returns></returns>
        protected string GetProjectName()
        {
            if (model.ProjectID.HasValue)
            {
                Entities.ProjectInfo proModel = BLL.ProjectInfo.Instance.GetProjectInfo(model.ValueOrDefault_ProjectID);
                return proModel.Name;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取活动名称
        /// </summary>
        /// <returns></returns>
        protected string GetActivityName()
        {
            Entities.YTGActivityInfo activityModel = BLL.YTGActivity.Instance.GetComAdoInfo<Entities.YTGActivityInfo>(model.ActivityID);
            return activityModel.ActivityName;
        }
    }
}