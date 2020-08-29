using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask
{
    public partial class YTGActivityTaskDeal : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
        public int sexValue = 0;
        public int isSuccess = -2;
        public int isConnected = -2;
        public int provinceID = -1;
        public int cityID = -1;
        public int testDriveProvinceID = -1;
        public int testDriveCityID = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userid = BLL.Util.GetLoginUserID();
                //增加“易团购--邀约处理”处理功能验证逻辑
                if (BLL.Util.CheckRight(userid, "SYS024MOD101505"))
                {
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
                        else if (model.Status != (int)Entities.YTGActivityTaskStatus.Processing && model.Status != (int)Entities.YTGActivityTaskStatus.NoProcess)
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
                            if (model.Sex.HasValue)
                            {
                                sexValue = model.Sex.Value;
                            }
                            if (model.IsSuccess.HasValue)
                            {
                                isSuccess = model.IsSuccess.Value;
                            }
                            if (model.IsJT.HasValue)
                            {
                                isConnected = model.IsJT.Value;
                            }
                            if (model.ProvinceID.HasValue)
                            {
                                provinceID = model.ProvinceID.Value;
                            }
                            if (model.CityID.HasValue)
                            {
                                cityID = model.CityID.Value;
                            }
                            if (model.TestDriveProvinceID.HasValue)
                            {
                                testDriveProvinceID = model.TestDriveProvinceID.Value;
                            }
                            if (model.TestDriveCityID.HasValue)
                            {
                                testDriveCityID = model.TestDriveCityID.Value;
                            }
                            this.Remark.Value = model.Remark;

                            BindYiXiangCheXing(model.ActivityID);
                            BindPredictBuyTime();
                            BindFailReason();
                        }
                    }
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
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
        /// 绑定意向车型数据
        /// </summary>
        /// <param name="ActionID"></param>
        private void BindYiXiangCheXing(string ActionID)
        {
            Entities.YTGActivityInfo actionModel = BLL.YTGActivity.Instance.GetComAdoInfo<Entities.YTGActivityInfo>(ActionID);
            selYiXiangCheXing.DataSource = BLL.YTGActivity.Instance.GetCarSerialsByIds(actionModel.CarSerials);
            selYiXiangCheXing.DataTextField = "Name";
            selYiXiangCheXing.DataValueField = "CSID";
            selYiXiangCheXing.DataBind();
            selYiXiangCheXing.Items.Insert(0, new ListItem("请选择", "-2"));

            if (model.DCarSerialID.HasValue)
            {
                selYiXiangCheXing.Items.FindByValue(model.DCarSerialID.Value.ToString()).Selected = true;
            }
        }
        /// <summary>
        /// 绑定预计购车时间
        /// </summary>
        protected void BindPredictBuyTime()
        {
            this.selPredictBuyTime.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.YTGActivityPlanBuyCarTime));
            selPredictBuyTime.DataTextField = "name";
            selPredictBuyTime.DataValueField = "value";
            selPredictBuyTime.DataBind();
            selPredictBuyTime.Items.Insert(0, new ListItem("请选择", "-2"));

            if (model.PBuyCarTime.HasValue)
            {
                selPredictBuyTime.Items.FindByValue(model.PBuyCarTime.Value.ToString()).Selected = true;
            }
        }
        /// <summary>
        /// 绑定失败原因
        /// </summary>
        protected void BindFailReason()
        {
            this.selFailReason.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.YTGActivityTaskFailReason));
            selFailReason.DataTextField = "name";
            selFailReason.DataValueField = "value";
            selFailReason.DataBind();
            selFailReason.Items.Insert(0, new ListItem("请选择", "-2"));

            if (model.FailReason.HasValue)
            {
                selFailReason.Items.FindByValue(model.FailReason.Value.ToString()).Selected = true;
            }
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