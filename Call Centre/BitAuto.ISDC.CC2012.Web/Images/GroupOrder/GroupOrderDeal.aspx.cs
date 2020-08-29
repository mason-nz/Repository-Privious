using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.GroupOrder
{
    public partial class GroupOrderDeal : System.Web.UI.Page
    {
        #region 参数
        /// <summary>
        ///  任务ID
        /// </summary>
        public string TaskID
        {
            get
            {
                if (HttpContext.Current.Request["TaskID"] != null)
                {
                    return HttpContext.Current.Request["TaskID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CustName = string.Empty;
        public string CustTel = string.Empty;
        public string CityID = string.Empty;
        public string ProvinceID = string.Empty;
        public string LocationName = string.Empty;
        public string AreaID = string.Empty;
        public string AreaName = string.Empty;
        public string OrderID = string.Empty;
        public string OrderCode = string.Empty;
        public string DealerID = string.Empty;
        public string DealerName = string.Empty;
        public string CarName = string.Empty;
        public string OrderTime = string.Empty;
        public string OrderPrice = string.Empty;
        public string Remark = string.Empty;
        public string MemberID = string.Empty;
        public string CustID = string.Empty;
        public string UserName = string.Empty;

        public string WantCarBrandID = string.Empty;
        public string WantCarSerialID = string.Empty;
        public string WantCarID = string.Empty;
        #endregion

        #region 加载页面
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 页面初次加载
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (!string.IsNullOrEmpty(TaskID))
                {
                    #region 绑定失败理由
                    BindFailReson();
                    #endregion

                    #region 绑定预计购车时间
                    BindPlanBuyCarTime();
                    #endregion

                    #region 任务不为空

                    int TaskIDInt = 0;
                    if (int.TryParse(TaskID, out TaskIDInt))
                    {
                        #region 任务数据格式正确
                        Entities.GroupOrderTask model = GroupOrderTask.Instance.GetGroupOrderTask(TaskIDInt);
                        if (model != null)
                        {
                            #region 任务存在
                            if (model.TaskStatus == (int)Entities.GroupTaskStatus.NoAllocation || model.TaskStatus == (int)Entities.GroupTaskStatus.Processed)
                            {
                                #region 任务不处于处理状态
                                Response.Write(@"<script language='javascript'>alert('当前任务不处于处理状态，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                                #endregion
                            }
                            else
                            {
                                #region 任务处于处理状态
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
                                else
                                {
                                    #region 加载页面
                                    Entities.GroupOrder grouporder = BLL.GroupOrder.Instance.GetGroupOrder(TaskIDInt);
                                    if (grouporder != null)
                                    {
                                        #region 订单存在
                                        //客户名称
                                        CustName = grouporder.CustomerName;
                                        //客户性别
                                        if (grouporder.UserGender == 1)
                                        {
                                            rdoMan.Checked = true;
                                        }
                                        else if (grouporder.UserGender == 2)
                                        {
                                            rdoWomen.Checked = true;
                                        }
                                        //客户电话
                                        CustTel = grouporder.CustomerTel;
                                        //客户城市id
                                        CityID = grouporder.CityID.ToString();

                                        //客户省份ID
                                        ProvinceID = grouporder.ProvinceID.ToString();

                                        //客户省份城市
                                        if (!string.IsNullOrEmpty(grouporder.ProvinceName))
                                        {
                                            LocationName += grouporder.ProvinceName + " ";
                                        }
                                        if (!string.IsNullOrEmpty(grouporder.CityName))
                                        {
                                            LocationName += grouporder.CityName;
                                        }
                                        //大区id
                                        AreaID = grouporder.AreaID.ToString();
                                        UserName = grouporder.UserName;
                                        //大区名称
                                        AreaName = BLL.Util.GetEnumOptText(typeof(Entities.EnumArea), Convert.ToInt32(grouporder.AreaID));
                                        //订单ID
                                        OrderID = grouporder.OrderID.ToString();
                                        //订单编号
                                        OrderCode = grouporder.OrderCode.ToString();
                                        //经销商id
                                        DealerID = grouporder.DealerID.ToString();

                                        //根据经销商id，取名称
                                        if (!string.IsNullOrEmpty(DealerID) && DealerID != "0" && DealerID != "-2")
                                        {
                                            BitAuto.YanFa.Crm2009.Entities.DMSMember DMSModel = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(DealerID);
                                            if (DMSModel != null)
                                            {
                                                MemberID = DMSModel.ID.ToString();
                                                CustID = DMSModel.CustID;
                                            }
                                        }
                                        //经销商名称
                                        DealerName = grouporder.DealerName;
                                        //车款名称
                                        if (!string.IsNullOrEmpty(grouporder.CarMasterName))
                                        {
                                            CarName += grouporder.CarMasterName;
                                        }
                                        if (!string.IsNullOrEmpty(grouporder.CarSerialName))
                                        {
                                            CarName += "-" + grouporder.CarSerialName;
                                        }
                                        if (!string.IsNullOrEmpty(grouporder.CarName))
                                        {
                                            CarName += "-" + grouporder.CarName;
                                        }
                                        //下单时间
                                        DateTime ordertime = System.DateTime.Now;
                                        if (DateTime.TryParse(grouporder.OrderCreateTime.ToString(), out ordertime))
                                        {
                                            OrderTime = ordertime.ToString("yyyy-MM-dd HH:mm:ss");
                                        }
                                        else
                                        {
                                            OrderTime = "";
                                        }
                                        //价格
                                        OrderPrice = grouporder.OrderPrice + "万元";
                                        //是否回访
                                        selReturnVisit.SelectedIndex = selReturnVisit.Items.IndexOf(selReturnVisit.Items.FindByValue(grouporder.IsReturnVisit.ToString()));
                                        Remark = grouporder.CallRecord;
                                        //失败原因
                                        selFailReson.SelectedIndex = selFailReson.Items.IndexOf(selFailReson.Items.FindByValue(grouporder.FailReasonID.ToString()));

                                        //加载意向车型
                                        WantCarBrandID = grouporder.WantCarMasterID.ToString();
                                        WantCarSerialID = grouporder.WantCarSerialID.ToString();
                                        WantCarID = grouporder.WantCarID.ToString();
                                        //预计购车时间
                                        if (grouporder.PlanBuyCarTime != -2)
                                        {
                                            dllPlanBuyCarTime.SelectedIndex = dllPlanBuyCarTime.Items.IndexOf(dllPlanBuyCarTime.Items.FindByValue(grouporder.PlanBuyCarTime.ToString()));
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 订单不存在
                                        Response.Write(@"<script language='javascript'>alert('该任务对应订单不存在，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region 任务不存在
                            Response.Write(@"<script language='javascript'>alert('任务不存在，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region 格式不正确
                        Response.Write(@"<script language='javascript'>alert('任务的数据格式不正确，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                        #endregion
                    }
                    #endregion

                }
                else
                {
                    #region 任务为空
                    Response.Write(@"<script language='javascript'>alert('任务不能为空，页面将被关闭。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                    #endregion
                }
            }
            #endregion
        }
        #endregion
        #region 绑定失败理由
        protected void BindFailReson()
        {
            DataTable dt = null;
            dt = BLL.GO_FailureReason.Instance.GetAllList();
            selFailReson.DataSource = dt;
            selFailReson.DataTextField = "ReasonName";
            selFailReson.DataValueField = "RecID";
            selFailReson.DataBind();
            selFailReson.Items.Insert(0, new ListItem("请选择", "-2"));
        }
        #endregion
        /// <summary>
        /// 绑定预计购车时间
        /// </summary>
        protected void BindPlanBuyCarTime()
        {
            dllPlanBuyCarTime.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.GO_PlanBuyCarTime));
            dllPlanBuyCarTime.DataTextField = "Name";
            dllPlanBuyCarTime.DataValueField = "value";
            dllPlanBuyCarTime.DataBind();
            dllPlanBuyCarTime.Items.Insert(0, new ListItem("请选择", "-2"));
        }
    }
}