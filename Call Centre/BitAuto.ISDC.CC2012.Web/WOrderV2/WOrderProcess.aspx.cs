using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2
{
    public partial class WOrderProcess : PageBase
    {
        /// 工单id
        /// <summary>
        /// 工单id
        /// </summary>
        public string OrderID { get { return BLL.Util.GetCurrentRequestStr("OrderID"); } }
        /// 权限控制
        /// <summary>
        /// 权限控制
        /// </summary>
        public string Right
        {
            get
            {
                string key= BLL.Util.GetCurrentRequestStr("Right");
                try
                {
                    if (key != "")
                    {
                        key = BitAuto.YanFa.Crm2009.BLL.Util.Decrypt(key, "yicheforcc");
                    }
                }
                catch
                {
                }
                return key;
            }
        }

        /// 工单实体类
        /// <summary>
        /// 工单实体类
        /// </summary>
        public Entities.WOrderInfoInfo WOrderInfo = null;
        /// 工单状态
        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderStatus WOrderStatus = WorkOrderStatus.Closed;
        /// 建立工单的CRM客户id
        /// <summary>
        /// 建立工单的CRM客户id
        /// </summary>
        public string CRMCustID = "";
        /// 标题
        /// <summary>
        /// 标题
        /// </summary>
        public string TitleName = "";

        //客户类别
        public int CustTypeId = -1;
        //登录人userid
        public int loginuserid = -1;
        //登录人员工编号
        public string loginusernum = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WOrderProcessRightJsonData right = WOrderProcessRightJsonData.GetWOrderProcessRightJsonData(Right);
                if (ValidateRight(right))
                {
                    ucCustInfoView.CustID = WOrderInfo.CBID_Value;
                    ucCustInfoView.Telphone = WOrderInfo.Phone_Value;
                    ucCustInfoView.CanSeeTelImg = true;
                    ucWOrderBasicInfo.WOrderInfo = WOrderInfo;
                    ucWOrderBasicInfo.CustCategoryID = ucCustInfoView.CustType;
                    ucWOrderBasicInfo.CanSeeTelImg = true;
                    CustTypeId = (int)ucCustInfoView.CustType;
                    //审核+处理
                    if (WOrderStatus == WorkOrderStatus.Pending ||
                        WOrderStatus == WorkOrderStatus.Untreated ||
                        WOrderStatus == WorkOrderStatus.Processing)
                    {
                        var c = this.LoadControl("~/WOrderV2/UserControl/WOrderDealControl/WOrderProcess.ascx", this.OrderID, this.WOrderStatus);
                        this.ucPlaceHolder.Controls.Add(c);
                    }
                    //回访
                    else if (WOrderStatus == WorkOrderStatus.Processed)
                    {
                        var c = this.LoadControl("~/WOrderV2/UserControl/WOrderDealControl/WOrderReturnVisit.ascx", this.OrderID);
                        this.ucPlaceHolder.Controls.Add(c);
                    }
                }
            }
        }
        /// 权限验证
        /// <summary>
        /// 权限验证
        /// </summary>
        /// <returns></returns>
        private bool ValidateRight(WOrderProcessRightJsonData right)
        {
            string msg = "";
            WOrderOperTypeEnum oper = WOrderOperTypeEnum.None;

            bool a = BLL.WOrderProcess.Instance.ValidateWOrderProcessRight(OrderID, ref msg, ref oper, out WOrderInfo, right);
            if (a == false)
            {
                BLL.Util.CloseCurrentPageAfterAlert(Response, msg);
                return false;
            }
            else
            {
                TitleName = "工单" + BLL.Util.GetEnumOptText(typeof(WOrderOperTypeEnum), (int)oper);
                WOrderStatus = (WorkOrderStatus)WOrderInfo.WorkOrderStatus_Value;
                CRMCustID = WOrderInfo.CRMCustID_Value;
                return true;
            }
        }
        /// 重写LoadControl，带参数。
        /// <summary>
        /// 重写LoadControl，带参数。
        /// </summary>
        private System.Web.UI.UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }
            System.Web.UI.UserControl ctl = Page.LoadControl(UserControlPath) as System.Web.UI.UserControl;
            System.Reflection.ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }
            return ctl;
        }
    }
}