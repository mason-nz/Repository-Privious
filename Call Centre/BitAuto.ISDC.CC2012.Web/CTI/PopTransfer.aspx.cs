using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.AjaxServers;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.CTI
{
    public partial class PopTransfer : PageBase
    {
        #region get参数
        private string UserEvent
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("UserEvent"); }
        }
        private string UserName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("UserName"); }
        }
        private string CalledNum
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CalledNum"); }
        }
        /// 主叫
        /// <summary>
        /// 主叫
        /// </summary>
        private string CallerNum
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CallerNum"); }
        }
        /// 落地号
        /// <summary>
        /// 落地号
        /// </summary>
        private string SYS_DNIS
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("SYS_DNIS"); }
        }
        private string CallID
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CallID"); }
        }
        #endregion

        public string logmsg = "";
        public string gotourl = "";

        public TelNumManage TelManage = BLL.CallDisplay.Manage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //获取工单类型
                WorkOrderDataSource dataSource = GetDataSource();
                //获取跳转地址
                string url = GetAddWOrderUrl(dataSource);

                sw.Stop();
                logmsg += "查询客户ID选择页面耗时s：" + sw.Elapsed.TotalSeconds.ToString("0.00");
                gotourl = url;
            }
        }

        /// 获取工单类型
        /// <summary>
        /// 获取工单类型
        /// </summary>
        /// <returns></returns>
        private WorkOrderDataSource GetDataSource()
        {
            WorkOrderDataSource? dataSource = null;
            TelNum telModel = TelManage.TelNumList.FirstOrDefault(x => x.AreaCode + x.Tel == SYS_DNIS);
            if (telModel != null)
            {
                dataSource = telModel.DataSource;
            }
            else
            {
                if (SYS_DNIS == "2446")
                {
                    dataSource = WorkOrderDataSource.OSEHotLine;
                }
                else if (SYS_DNIS == "2448")
                {
                    dataSource = WorkOrderDataSource.BitShopLine;
                }
                else
                {
                    dataSource = WorkOrderDataSource.TeleContact;
                }
            }
            return dataSource.Value;
        }
        /// 获取跳转地址
        /// <summary>
        /// 获取跳转地址
        /// </summary>
        /// <returns></returns>
        private string GetAddWOrderUrl(WorkOrderDataSource dataSource)
        {
            //查询客户
            CTIHandlerHelper.GetCustIDByTel(CallerNum);
            //生成参数
            string param = BLL.WOrderRequest.AddWOrderComeIn_CallIn(dataSource, CallerNum).ToString();
            //返回链接
            return "/WOrderV2/AddWOrderInfo.aspx?" + param;
        }
    }
}