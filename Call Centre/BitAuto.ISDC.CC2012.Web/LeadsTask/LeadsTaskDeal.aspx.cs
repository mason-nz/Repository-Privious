using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
/*add by qizq 2014-5-19*/
/*leads处理页面*/
namespace BitAuto.ISDC.CC2012.Web.LeadsTask
{
    public partial class LeadsTaskDeal : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
                    return string.Empty;

                }
            }
        }
        //下单品牌，车型，车款组合
        public string OrderCarInfo = string.Empty;
        //需匹配车型
        public string DCarInfo = string.Empty;
        //需求单查看页面
        public string DemandDetailsUrl = System.Configuration.ConfigurationManager.AppSettings["DemandDetailsUrl"].ToString();
        public int userid;
        public int Sex;
        public string tel;
        protected void Page_Load(object sender, EventArgs e)
        {

            DemandDetailsUrl += "?DemandID={0}";
            if (!IsPostBack)
            { 
                userid = BLL.Util.GetLoginUserID();
                //增加“任务列表--线索邀约”处理 功能验证逻辑
                if (!BLL.Util.CheckRight(userid, "SYS024BUT101203"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
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
                        Sex = (int)model.Sex;
                        tel = model.Tel;
                        //根据需匹配车型绑定需匹配车款
                        int _dcarserialid;
                        int.TryParse(model.DCarSerialID.ToString(), out _dcarserialid);
                        BindDCarList(_dcarserialid, model.DemandID);
                        //绑定失败原因
                        BindFailReason();
                        //根据Leads处理实体加载地区，下单车型，需匹配车型信息
                        LoadLeadInfo(model);
                    }
                }
            }
        }
        //根据需匹配车型绑定需匹配车款
        protected void BindDCarList(int DCarSerialID, string DemandID)
        {
            DataTable dt = BLL.LeadsTask.Instance.GetCarListByCarSerialID(DCarSerialID);

            #region 只显示需求单指定的车款

            BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandCarQuery query = new BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandCarQuery();
            query.DemandID = DemandID;
            DataTable carDt = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandCar(query);
            if (carDt != null)
            {
                #region 只显示需求单指定的车款

                for (int dtID = dt.Rows.Count - 1; dtID >= 0; dtID--)
                {
                    bool flog = false;
                    for (int carID = carDt.Rows.Count - 1; carID >= 0; carID--)
                    {
                        if (dt.Rows[dtID]["CarID"].ToString() == "0")
                        {
                            flog = true;
                            break;
                        }
                        if (dt.Rows[dtID]["CarID"].ToString() == carDt.Rows[carID]["CarID"].ToString())
                        {
                            flog = true;
                            break;
                        }
                    }
                    if (!flog)
                    {
                        //不在就删除
                        dt.Rows.RemoveAt(dtID);
                    }
                }
                #endregion

                #region 去除多余的年份

                for (int i = dt.Rows.Count-1; i >=0; i--)
                {
                    if (dt.Rows[i]["CarID"].ToString() == "0")
                    {
                        if ((i + 1) != dt.Rows.Count)
                        {
                            if (dt.Rows[i + 1]["CarID"].ToString() == "0")
                            {
                                //如果下一条是0，就没有车款，删除
                                dt.Rows[i].Delete();
                            }
                        }
                        else
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                }
                #endregion

                this.dllDCarName.DataSource = dt;
                this.dllDCarName.DataTextField = "CarName";
                this.dllDCarName.DataValueField = "CarID";
                this.dllDCarName.DataBind();
                dllDCarName.Items.Insert(0, new ListItem("请选择车款", "-2"));

            }

            #endregion
        }
        /// <summary>
        /// 绑定失败原因
        /// </summary>
        protected void BindFailReason()
        {
            this.selFailReson.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.LeadTaskFailReason));
            selFailReson.DataTextField = "name";
            selFailReson.DataValueField = "value";
            selFailReson.DataBind();
            selFailReson.Items.Insert(0, new ListItem("请选择", "-2"));
        }
        /// <summary>
        /// 根据Leads处理实体加载信息
        /// </summary>
        /// <param name="model"></param>
        protected void LoadLeadInfo(Entities.LeadsTask model)
        {
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
            //是否成功
            if (model.IsSuccess != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                if (model.IsSuccess == 1)
                {
                    this.rdoSuccess.Checked = true;
                }
                else if (model.IsSuccess == 0)
                {
                    this.rdoFail.Checked = true;
                }
            }
            //加载失败原因
            if (model.FailReason != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                var li = this.selFailReson.Items.FindByValue(model.FailReason.ToString());
                this.selFailReson.SelectedIndex = this.selFailReson.Items.IndexOf(li);
            }
            //加载需匹配车款
            if (model.DCarID != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                var li = this.dllDCarName.Items.FindByValue(model.DCarID.ToString());
                this.dllDCarName.SelectedIndex = this.dllDCarName.Items.IndexOf(li);
            }
        }
    }
}