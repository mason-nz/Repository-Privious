using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.DSC.IM_DMS2014.BLL;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_DMS2014.Web.WebService
{
    /// <summary>
    /// Utils 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class Utils : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        ///// <summary>
        ///// 回写工单号到IM会话表、留言表
        ///// </summary>
        ///// <param name="imtype">1：会话工单，2：留言工单</param>
        ///// <param name="id"></param>
        ///// <param name="orderid"></param>
        ///// <param name="errormsg"></param>
        //[WebMethod(Description = "根据会话ID更新工单ID")]        
        //public void UpdateCCWorkOrder2IM(int imtype,int id,string orderid,out string errormsg)
        //{
        //    if (imtype == 1)
        //    {
        //        Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 开始!会话ID:" + id + ",工单号：" + orderid);
        //    }
        //    else
        //    {
        //        Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 开始!留言ID:" + id + ",工单号：" + orderid);
        //    }
        //    errormsg = "";

        //    try
        //    {
        //        if (imtype == 1)
        //        {
        //            Entities.Conversations model = new Entities.Conversations();
        //            model = BLL.Conversations.Instance.GetConversations(id);

        //            if (model != null)
        //            {
        //                model.OrderID = orderid;

        //                BLL.Conversations.Instance.Update(model);
        //            }
        //            else
        //            {
        //                errormsg = "会话不存在!";
        //            }
        //        }
        //        else if (imtype == 2)
        //        {
        //            Entities.UserMessage model = new Entities.UserMessage();
        //            model = BLL.UserMessage.Instance.GetModel(id);

        //            if (model != null)
        //            {
        //                model.OrderID = orderid;

        //                BLL.UserMessage.Instance.Update(model);
        //            }
        //            else
        //            {
        //                errormsg = "留言不存在!";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 出错!ID:" + id + ",工单号：" + orderid);
        //        Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 出错!errorMessage:" + ex.Message);
        //        Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 出错!errorStackTrace:" + ex.StackTrace);
        //        errormsg = ex.Message;
        //    }
                        
        //}

        /// <summary>
        /// 回写工单号到IM会话表、留言表
        /// </summary>
        /// <param name="imtype">1：会话工单，2：留言工单</param>
        /// <param name="id"></param>
        /// <param name="orderid"></param>
        /// <param name="errormsg"></param>
        [WebMethod(Description = "根据会话ID更新工单ID")]
        public void UpdateCCWorkOrder2IMHaveKey(int imtype, int id, string orderid,string key, out string errormsg)
        {
            string CC_WorkOrder_Key = ConfigurationUtil.GetAppSettingValue("CC_WorkOrder_Key");
            if (key == CC_WorkOrder_Key)
            {
                if (imtype == 1)
                {
                    Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 开始!会话ID:" + id + ",工单号：" + orderid);
                }
                else
                {
                    Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 开始!留言ID:" + id + ",工单号：" + orderid);
                }
                errormsg = "";

                try
                {
                    if (imtype == 1)
                    {
                        Entities.Conversations model = new Entities.Conversations();
                        model = BLL.Conversations.Instance.GetConversations(id);

                        if (model != null)
                        {
                            model.OrderID = orderid;

                            BLL.Conversations.Instance.Update(model);
                        }
                        else
                        {
                            errormsg = "会话不存在!";
                        }
                    }
                    else if (imtype == 2)
                    {
                        Entities.UserMessage model = new Entities.UserMessage();
                        model = BLL.UserMessage.Instance.GetModel(id);

                        if (model != null)
                        {
                            model.OrderID = orderid;

                            BLL.UserMessage.Instance.Update(model);
                        }
                        else
                        {
                            errormsg = "留言不存在!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 出错!ID:" + id + ",工单号：" + orderid);
                    Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 出错!errorMessage:" + ex.Message);
                    Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 出错!errorStackTrace:" + ex.StackTrace);
                    errormsg = ex.Message;
                }
            }
            else
            {
                errormsg = "验证码不正确!";
                Loger.Log4Net.Info("[Utils.asmx]UpdateCCWorkOrder2IM 验证码不正确，ID:" + id + ",工单号：" + orderid);
            }

        }
    }
}
