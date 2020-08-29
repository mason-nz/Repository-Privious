using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.WebService.YpGroupOrderService;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class GroupOrderHelper
    {
        private string YPGroupOrderTokenKey = System.Configuration.ConfigurationManager.AppSettings["YPGroupOrderTokenKey"].ToString();//易湃团购订单TokenKey
        private string YPGroupOrderAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["YPGroupOrderAuthorizeCode"].ToString();//易湃团购订单AuthorizeCode
        ActiveOrder_Crm service = new ActiveOrder_Crm();

        public GroupOrderHelper()
        {
            DasSoapHeader soapHeader = new DasSoapHeader();
            soapHeader.TokenKey = new Guid(YPGroupOrderTokenKey);
            soapHeader.AuthorizeCode = YPGroupOrderAuthorizeCode;
            service.DasSoapHeaderValue = soapHeader;
        }

        /// <summary>
        /// 取得团购订单数据
        /// </summary>
        /// <param name="lastMaxId">上次已获取过的订单最大id</param>
        /// <returns></returns>
        public DataTable GetTGCarOrderList(int lastMaxId)
        {
            DataSet ds = service.GetTGCarOrderList(lastMaxId, 0);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 更新团购订单内容，且把团购订单提交到订单库 
        /// </summary>
        /// <param name="model">团购订单实体</param>
        /// <param name="errorMsg">如出错，则返回出错信息</param>
        /// <returns>1成功处理订单，-1需要重新处理</returns>
        public int SetTGCarOrderState(Entities.GroupOrder model, ref string errorMsg)
        {
            int status = -1;
            if (model == null ||
                (model != null && (model.OrderID == null || model.OrderID.Value <= 0 || model.TaskID <= 0)))
            {
                errorMsg = "团购订单实体，或易湃团购订单ID无效，或团购任务ID无效";
            }
            else
            {
                Entities.GroupOrderTask otModel = BLL.GroupOrderTask.Instance.GetGroupOrderTask(model.TaskID);
                if (otModel == null)
                {
                    errorMsg = "根据团购订单实体，获取任务无效";
                }
                else
                {
                    Entities.GroupOrderOrg onModel = BLL.GroupOrderOrg.Instance.GetGroupOrderOrgByOrderID(otModel.OrderID.Value);
                    if (onModel == null)
                    {
                        errorMsg = "根据团购订单ID，获取团购订单日志实体无效";
                    }
                    else
                    {
                        //object[] array = new object[] { model.OrderID.Value, model.IsReturnVisit.Value, model.CallRecord };
                        //status = (int)WebServiceHelper.InvokeWebService(GroupOrderServiceURL, "SetTGCarOrderState", array);
                        status = service.SetTGCarOrderState(model.OrderID.Value, IntToEnum(model.IsReturnVisit.Value), model.CallRecord);
                        //errorMsg = array[array.Length - 1].ToString();
                        //service.SetTGCarOrderStateCompleted += new SetTGCarOrderStateCompletedEventHandler(AsyncSetTGCarOrderStateComplete);
                        //BLL.Loger.Log4Net.Info("异步调用接口SetTGCarOrderStateAsync——开始！");
                        //service.SetTGCarOrderStateAsync(model.OrderID.Value, IntToEnum(model.IsReturnVisit.Value), model.CallRecord);
                        //BLL.Loger.Log4Net.Info("异步调用接口SetTGCarOrderStateAsync——结束！");
                    }
                }
            }
            return status;
        }

        //void AsyncSetTGCarOrderStateComplete(object sender, SetTGCarOrderStateCompletedEventArgs e)
        //{
        //    int res = e.Result;
        //    BLL.Loger.Log4Net.Info("异步调用接口内部——结束！返回值为：" + res);
        //}
        private GPOrderTradeState IntToEnum(int n)
        {
            if (Enum.IsDefined(typeof(GPOrderTradeState), n))
                return (GPOrderTradeState)n;
            else
                throw new Exception(n + "在易湃接口枚举类型GPOrderTradeState中，没有找到对应的项");
        }


        ///// <summary>
        /////订单处理状态枚举
        ///// </summary>
        //public enum GPOrderTradeState
        //{
        //    /// <summary>
        //    /// 未知
        //    /// </summary>
        //    Unknown = 0,

        //    /// <summary>
        //    /// 是
        //    /// </summary>
        //    Yes = 1,

        //    /// <summary>
        //    /// 否
        //    /// </summary>
        //    No = 2
        //}

    }
}
