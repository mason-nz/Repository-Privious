using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.Utils.Config;
using System.Data.SqlClient;
using System.Messaging;

namespace BitAuto.ISDC.CC2012.Web.YiJiKeService
{
    /// <summary>
    /// CCYiJiKeService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CCYiJiKeService : System.Web.Services.WebService
    {
        [WebMethod(Description = "拉取需求单客户通知接口")]
        public string PullCustNotify(string Verifycode, string DemandID, int BatchNo)
        {
            BLL.Loger.Log4Net.Info("调用【拉取需求单客户通知接口】：{需求单ID:" + DemandID + ",批次号：" + BatchNo.ToString() + "}：");
            string errMsg = "";
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref errMsg, "拉取需求单客户通知接口，授权失败。"))
                {
                    // 插入消息队列
                    string QueueName = "";
                    BLL.LeadsTask.Instance.CreateQueue(false, out QueueName);
                    MessageQueue MQ = new MessageQueue(QueueName);
                    MQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                    MQ.Send("YJK," + DemandID + "," + BatchNo.ToString());

                }
                else
                {
                    errMsg += "调用【拉取需求单客户通知接口】授权失败";
                    BLL.Loger.Log4Net.Info("调用【拉取需求单客户通知接口】授权失败：IP:[" + System.Web.HttpContext.Current.Request.UserHostAddress + "],验证码：[" + Verifycode + "]");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【拉取需求单客户通知接口】出错{需求单ID:" + DemandID + ",批次号：" + BatchNo.ToString() + "}：",ex);
            }
            return errMsg;
        }

        [WebMethod(Description = "根据需求ID,结束CC项目接口")]
        public string EndCCProject(string Verifycode, string DemandID)
        {
            string errMsg = "";
            BLL.Loger.Log4Net.Info("调用【根据需求ID,结束CC项目接口】：{需求单ID:" + DemandID + "}");
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref errMsg, "结束CC项目，授权失败。"))
                {
                    //一个需求单号决定一个yjk项目
                    BLL.ProjectInfo.Instance.EndCCProjectForYJK(DemandID, out errMsg);
                }
                else
                {
                    errMsg += "调用【拉取需求单客户通知接口】授权失败";
                    BLL.Loger.Log4Net.Info("调用【根据需求ID,结束CC项目接口】授权失败：IP:[" + System.Web.HttpContext.Current.Request.UserHostAddress + "],验证码：[" + Verifycode + "]");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【根据需求ID,结束CC项目接口】出错（需求ID:" + DemandID + "）：",ex);
            }
            BLL.Loger.Log4Net.Info("调用【根据需求ID,结束CC项目接口】：{需求单ID:" + DemandID + "} ,返回值为[" + errMsg + "]");
            return errMsg;
        }

        [WebMethod(Description = "拉取厂商需求单的线索数据")]
        public string PullCJKLeadTask(string Verifycode, string DemandID, int BatchNo, int ExpectedNum)
        {
            BLL.Loger.Log4Net.Info("调用【拉取厂商需求单的线索数据】：{需求单ID:" + DemandID + ",批次号：" + BatchNo.ToString() + "}：");
            string errMsg = "";
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref errMsg, "拉取厂商需求单的线索数据，授权失败。"))
                {
                    // 插入消息队列
                    string QueueName = "";
                    BLL.LeadsTask.Instance.CreateQueue(false, out QueueName);
                    MessageQueue MQ = new MessageQueue(QueueName);
                    MQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                    MQ.Send("CJK," + DemandID + "," + BatchNo.ToString() + "," + ExpectedNum);

                    BLL.Loger.Log4Net.Info("调用通知拉取厂商需求单接口成功，消息名称是：" + QueueName + "发送的消息为：" + "CJK," + DemandID + "," + BatchNo.ToString() + "," + ExpectedNum);
                }
                else
                {
                    errMsg += "调用【拉取厂商需求单的线索数据】授权失败";
                    BLL.Loger.Log4Net.Info("调用【拉取厂商需求单的线索数据】授权失败：IP:[" + System.Web.HttpContext.Current.Request.UserHostAddress + "],验证码：[" + Verifycode + "]");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【拉取厂商需求单的线索数据】出错{需求单ID:" + DemandID + ",批次号：" + BatchNo.ToString() + "}：",ex);
            }
            return errMsg;
        }

        [WebMethod(Description = "结束厂商需求单的相应任务")]
        public string EndCJKProject(string Verifycode, string DemandID)
        {
            string errMsg = "";
            BLL.Loger.Log4Net.Info("调用【结束厂商需求单的相应任务】：{需求单ID:" + DemandID + "}");
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref errMsg, "结束厂商需求单的相应任务，授权失败。"))
                {
                    //一个需求单号和一个批次决定一个cjk项目
                    BLL.ProjectInfo.Instance.EndCCProjectForCJK(DemandID, out errMsg);
                }
                else
                {
                    errMsg += "调用【结束厂商需求单的相应任务】授权失败";
                    BLL.Loger.Log4Net.Info("调用【结束厂商需求单的相应任务】授权失败：IP:[" + System.Web.HttpContext.Current.Request.UserHostAddress + "],验证码：[" + Verifycode + "]");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【结束厂商需求单的相应任务】出错（需求ID:" + DemandID + "）：",ex);
            }
            BLL.Loger.Log4Net.Info("调用【结束厂商需求单的相应任务】：{需求单ID:" + DemandID + "} ,返回值为[" + errMsg + "]");
            return errMsg;
        }

        [WebMethod(Description = "根据批次结束厂商需求单的相应任务", MessageName = "EndCJKProjectByBatch")]
        public string EndCJKProjectByBatch(string Verifycode, string DemandID, int BatchNo)
        {
            string errMsg = "";
            BLL.Loger.Log4Net.Info("调用【根据批次结束厂商需求单的相应任务 EndCJKProjectByBatch】：{需求单ID:" + DemandID + "，批次：" + BatchNo.ToString() + "}");
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref errMsg, "根据批次结束厂商需求单的相应任务 EndCJKProjectByBatch，授权失败。"))
                {
                    //一个需求单号和一个批次决定一个cjk项目
                    BLL.ProjectInfo.Instance.EndCCProjectForCJKByBatch(DemandID, BatchNo, out errMsg);
                }
                else
                {
                    errMsg += "调用【根据批次结束厂商需求单的相应任务 EndCJKProjectByBatch】授权失败";
                    BLL.Loger.Log4Net.Info("调用【根据批次结束厂商需求单的相应任务 EndCJKProjectByBatch】授权失败：IP:[" + System.Web.HttpContext.Current.Request.UserHostAddress + "],验证码：[" + Verifycode + "]");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【根据批次结束厂商需求单的相应任务】出错（需求ID:" + DemandID + "）：" ,ex);
            }
            BLL.Loger.Log4Net.Info("调用【根据批次结束厂商需求单的相应任务】：{需求单ID:" + DemandID + "} ,返回值为[" + errMsg + "]");
            return errMsg;
        }


        [WebMethod(Description = "根据区域结束厂商需求单的相应任务", MessageName = "EndCJKProjectByAreaID")]
        public string EndCJKProjectByAreaID(string Verifycode, string DemandID, int AreaID)
        {
            string errMsg = "";
            BLL.Loger.Log4Net.Info("调用【根据区域结束厂商需求单的相应任务】：{需求单ID:" + DemandID + ",区域：" + AreaID.ToString() + "}");
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref errMsg, "根据区域结束厂商需求单的相应任务，授权失败。"))
                {
                    //一个需求单号和一个批次决定一个cjk项目
                    BLL.ProjectInfo.Instance.RevokeCJKTaskByDemandID_Area(DemandID, AreaID, out errMsg);
                }
                else
                {
                    errMsg += "调用【根据区域结束厂商需求单的相应任务】授权失败";
                    BLL.Loger.Log4Net.Info("调用【根据区域结束厂商需求单的相应任务】授权失败：IP:[" + System.Web.HttpContext.Current.Request.UserHostAddress + "],验证码：[" + Verifycode + "]");
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【根据区域结束厂商需求单的相应任务】出错（需求ID:" + DemandID + "）:",ex);
            }
            BLL.Loger.Log4Net.Info("调用【根据区域结束厂商需求单的相应任务】：{需求单ID:" + DemandID + "} ,返回值为[" + errMsg + "]");
            return errMsg;
        }
    }
}
