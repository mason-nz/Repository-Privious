using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.GroupOrder
{
    public partial class GroupOrderView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private long Transfer = 0;
        public long RequestTaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    if (long.TryParse(Request["TaskID"], out Transfer))
                    {
                        return Transfer;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 是否有查看处理订单的权限  1,有 0，没有
        /// </summary>
        public string IsEditTask = "0";

        /// <summary>
        /// 处理人是否是当前用户
        /// </summary>
        public string IsBelong = "0";

        /// <summary>
        /// 判断该任务状态是否已处理  1,有 0，没有
        /// </summary>
        public string IsProcessed = "0";

        public string Source = string.Empty;

        public string OrderCode = "";
        public string DealerName = "";
        public string DealerID = "";
        public string CarSerialName = "";
        public string CreateTime = "";
        public string OrderPrice = "";
        public string IsReturnVisit = "";
        public string FailReason = "";
        public string CallRecord = "";
        public string MemberID = string.Empty;
        public string CustID = string.Empty;
        public string wantcarname = string.Empty;
        public string planbuycartime = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断是否有查看该处理订单的权限
                if (BLL.Util.CheckButtonRight("SYS024BUT1204"))
                {
                    IsEditTask = "1";
                }
                OrderInfoBind();
            }
        }

        private void OrderInfoBind()
        {
            Entities.GroupOrder custBasicInfo = BLL.GroupOrder.Instance.GetGroupOrderCustInfo(RequestTaskID);
            if (custBasicInfo != null)
            {
                int _userID = BLL.Util.GetLoginUserID();
                if (custBasicInfo.AssignUserID == _userID)
                {
                    IsBelong = "1";
                }

                //                if (IsEditTask == "0" && IsBelong == "0")
                //                {
                //                    Response.Write(@"<script language='javascript'>alert('此订单不是您处理的且无管理员查看权限，无法查看。');try {
                //                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                //                                                       }
                //                                                       catch (e) {
                //                                                           window.opener = null; window.open('', '_self'); window.close();
                //                                                       }</script>");
                //                }

                OrderCode = custBasicInfo.OrderCode.ToString();
                DealerID = custBasicInfo.DealerID.ToString();
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
                DealerName = custBasicInfo.DealerName;

                CarSerialName = custBasicInfo.CarMasterName + "-" + custBasicInfo.CarSerialName + "-" + custBasicInfo.CarName;
                if (!string.IsNullOrEmpty(custBasicInfo.WantCarMasterName))
                {
                    wantcarname += custBasicInfo.WantCarMasterName;
                }
                if (!string.IsNullOrEmpty(custBasicInfo.WantCarSerialName))
                {
                    if (!string.IsNullOrEmpty(wantcarname))
                    {
                        wantcarname += "-" + custBasicInfo.WantCarSerialName;
                    }
                    else
                    {
                        wantcarname += custBasicInfo.WantCarSerialName;
                    }
                }
                if (!string.IsNullOrEmpty(custBasicInfo.WantCarName))
                {
                    if (!string.IsNullOrEmpty(wantcarname))
                    {
                        wantcarname += "-" + custBasicInfo.WantCarName;
                    }
                    else
                    {
                        wantcarname += custBasicInfo.WantCarName;
                    }
                }
                if (custBasicInfo.PlanBuyCarTime != -2)
                {
                    planbuycartime = BLL.Util.GetEnumOptText(typeof(Entities.GO_PlanBuyCarTime), Convert.ToInt32(custBasicInfo.PlanBuyCarTime));
                }

                //CreateTime = custBasicInfo.CreateTime.ToString();
                //下单时间
                CreateTime = custBasicInfo.OrderCreateTime.ToString();
                OrderPrice = custBasicInfo.OrderPrice.ToString() + "万元";
                IsReturnVisit = BLL.Util.GetEnumOptText(typeof(Entities.IsReturnVisit), (int)custBasicInfo.IsReturnVisit);
                FailReason = custBasicInfo.FailReason;
                CallRecord = custBasicInfo.CallRecord;

                if (custBasicInfo.TaskStatus == (int)Entities.GroupTaskStatus.Processed)
                {
                    IsProcessed = "1";
                }

            }
        }
    }
}