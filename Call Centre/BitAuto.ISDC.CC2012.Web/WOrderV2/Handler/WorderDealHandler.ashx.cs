using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using System.Web.Script.Serialization;
using BitAuto.ISDC.CC2012.Entities;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.Handler
{
    /// <summary>
    /// WorderDealHandler 的摘要说明
    /// </summary>
    public class WorderDealHandler : IHttpHandler, IRequiresSessionState
    {
        /// action参数
        /// <summary>
        /// action参数
        /// </summary>
        private string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("action");
            }
        }
        /// 工单ID
        /// <summary>
        /// 工单ID
        /// </summary>
        private string OrderID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("OrderID");
            }
        }
        /// 附件的存储位置枚举
        /// <summary>
        /// 附件的存储位置枚举
        /// </summary>
        private int StoragePathType
        {
            get
            {
                return BLL.Util.GetCurrentRequestInt("StoragePathType");
            }
        }
        /// json对象数据
        /// <summary>
        /// json对象数据
        /// </summary>
        private string JsonData
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("JsonData");
            }
        }
        /// 工单处理权限
        /// <summary>
        /// 工单处理权限
        /// </summary>
        private string RightData
        {
            get
            {
                string key = BLL.Util.GetCurrentRequestStr("RightData");
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

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                bool flag = true;
                string msg = "";
                JavaScriptSerializer jserializer = new JavaScriptSerializer();
                switch (Action)
                {
                    case "GetWOrderProcessByOrderID":
                        List<WOrderProcessInfo> ListProcess = GetWOrderProcessByOrderID(ref flag, ref msg);
                        AJAXHelper.WrapJsonResponse(flag, msg, jserializer.Serialize(ListProcess));
                        break;
                    case "GetAttachmentProcessListByOrderID":
                        List<CommonAttachmentInfo> listAttach = GetAttachmentProcessListByOrderID(ref flag, ref msg);
                        AJAXHelper.WrapJsonResponse(flag, msg, jserializer.Serialize(listAttach));
                        break;
                    case "GetCallReportByOrderID":
                        List<RetWOrderData> list2 = GetCallReportByOrderID(ref flag, ref msg);
                        AJAXHelper.WrapJsonResponse(flag, msg, jserializer.Serialize(list2));
                        break;
                    case "GetReceiverPeopleByOrderID":
                        List<WOrderToAndCCInfo> listRecevier = GetReceiverPeopleByOrderID(ref flag, ref msg);
                        AJAXHelper.WrapJsonResponse(flag, msg, jserializer.Serialize(listRecevier));
                        break;

                    case "SaveProcess":
                        string data = SaveProcess(ref flag, ref msg);
                        AJAXHelper.WrapJsonResponse(flag, data, msg);
                        break;
                    default:
                        AJAXHelper.WrapJsonResponse(false, "没有对应的操作", "没有对应的操作");
                        break;
                }
            }
            catch (Exception ex)
            {
                AJAXHelper.WrapJsonResponse(false, "操作失败！", ex.Message);
            }
        }

        private List<WOrderToAndCCInfo> GetReceiverPeopleByOrderID(ref bool flag, ref string msg)
        {
            List<WOrderToAndCCInfo> list = null;
            try
            {
                list = BLL.WOrderToAndCC.Instance.GetReceiverPeopleByOrderID(OrderID);

            }
            catch (Exception ex)
            {
                msg = "查询出错" + ex.Message;
                flag = false;
            }
            return list;
        }

        /// 根据工单ID获取处理记录
        /// <summary>
        /// 根据工单ID获取处理记录
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private List<WOrderProcessInfo> GetWOrderProcessByOrderID(ref bool flag, ref string msg)
        {
            List<WOrderProcessInfo> list = null;
            try
            {
                list = BLL.WOrderProcess.Instance.GetWOrderProcessByOrderID(OrderID);
                foreach (WOrderProcessInfo model in list)
                {
                    model.StatusStr = GetProcessWorkStatusEnumStr(CommonFunction.ObjectToString(model.WorkOrderStatus_Value), CommonFunction.ObjectToString(model.IsReturnVisit_Value));
                }
            }
            catch (Exception ex)
            {
                msg = "查询出错" + ex.Message;
                flag = false;
            }
            return list;
        }
        /// 查询某一个工单ID下的所有处理记录的附件信息
        /// <summary>
        /// 查询某一个工单ID下的所有处理记录的附件信息
        /// </summary>
        /// <param name="OrderID"></param>        
        /// <returns></returns>
        private List<CommonAttachmentInfo> GetAttachmentProcessListByOrderID(ref bool flag, ref string msg)
        {
            List<CommonAttachmentInfo> list = null;
            try
            {
                list = BLL.CommonAttachment.Instance.GetAttachmentProcessListByOrderID(OrderID, StoragePathType);
            }
            catch (Exception ex)
            {
                msg = "查询出错：" + ex.Message;
                flag = false;
            }
            return list;
        }
        /// 查询处理记录的话务
        /// <summary>
        /// 查询处理记录的话务
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private List<RetWOrderData> GetCallReportByOrderID(ref bool flag, ref string msg)
        {
            List<RetWOrderData> list = new List<RetWOrderData>();
            try
            {
                //查询处理记录的话务
                List<WOrderDataInfo> listData = BLL.WOrderData.Instance.GetCallReportByOrderID(OrderID, false);
                foreach (WOrderDataInfo item in listData)
                {
                    RetWOrderData model = new RetWOrderData();
                    model.AudioURL = item.AudioURL_Value;
                    model.DataID = CommonFunction.ObjectToString(item.DataID_Value);
                    model.ReceiverID = item.ReceiverID_Value;
                    list.Add(model);
                }
            }
            catch (Exception)
            {
                msg = "查询出错";
                flag = false;
            }
            return list;
        }
        /// 处理记录状态枚举转换
        /// <summary>
        /// 处理记录状态枚举转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetProcessWorkStatusEnumStr(string workOrderStatus, string isReturnVisit)
        {
            WorkOrderStatus w = (WorkOrderStatus)Enum.Parse(typeof(WorkOrderStatus), workOrderStatus);
            if (w == WorkOrderStatus.Processed)
            {
                int r = CommonFunction.ObjectToInteger(isReturnVisit, -1);
                if (r == -1)
                {
                    return BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)w);
                }
                else if (r == 0)
                {
                    return "未回访";
                }
                else if (r == 1)
                {
                    return "已回访";
                }
                else return "";
            }
            else
            {
                return BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)w);
            }
        }
        /// 工单处理
        /// <summary>
        /// 工单处理
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string SaveProcess(ref bool flag, ref string msg)
        {
            try
            {
                WOrderProcessJsonData jsondata = WOrderProcessJsonData.GetWOrderProcessJsonData(JsonData);
                if (jsondata == null)
                {
                    flag = false;
                    msg = "参数错误！";
                    return "";
                }
                WOrderInfoInfo worderinfo = null;
                WOrderOperTypeEnum oper = WOrderOperTypeEnum.None;
                WOrderProcessRightJsonData right = WOrderProcessRightJsonData.GetWOrderProcessRightJsonData(RightData);
                flag = BLL.WOrderProcess.Instance.ValidateWOrderProcessRight(OrderID, ref msg, ref oper, out worderinfo, right);
                if (flag == false)
                {
                    return "";
                }
                int loginuserid = BLL.Util.GetLoginUserID();
                SysRightUserInfo sysinfo = BLL.EmployeeSuper.Instance.GetSysRightUserInfo(loginuserid);
                if (sysinfo == null)
                {
                    flag = false;
                    msg = "获取不到当前登录人信息！";
                    return "";
                }
                //处理工单
                BLL.WOrderProcess.Instance.WOrderProcessMain(jsondata, sysinfo, oper, worderinfo);

                flag = true;
                msg = "";
                return BLL.Util.GetEnumOptText(typeof(WOrderOperTypeEnum), (int)oper);
            }
            catch (Exception ex)
            {
                WOrderInfo.ErrorToLog4("工单处理", ex);
                flag = false;
                msg = ex.Message;
                return "";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        //录音返回实体，因为WOrderData表的DataID返回
        public class RetWOrderData
        {
            public int ReceiverID { get; set; }
            public string DataID { get; set; }
            public string AudioURL { get; set; }
        }


    }
}