using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Entities;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.Web.CallRecordService
{
    /// <summary>
    /// CCDataInterfaceService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CCDataInterfaceService : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        /// <summary>
        /// 惠买车根据业务分组，分类获取一段时间范围内的话务总表数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [WebMethod(Description = "惠买车根据业务分组，分类获取一段时间范围内的话务总表数据")]
        public DataTable GetCallDataHuiMC(string Verifycode, string starttime, string endtime, int currentPage, out int totalCount, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataHuiMC ...BEGIN...开始时间：" + endtime + ",结束时间：" + endtime);
            totalCount = 0;
            int pageSize = 10;
            //验证授权码
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            if (sVerify == Verifycode)
            { }
            else
            {
                msg = "授权码错误!!";
                return null;
            }

            //验证开始、结束时间格式是否正确
            DateTime dt;
            if (!DateTime.TryParse(starttime, out dt))
            {
                msg = "开始时间格式不正确!!";
                return null;
            }

            if (!DateTime.TryParse(endtime, out dt))
            {
                msg = "结束时间格式不正确!!";
                return null;
            }

            DataTable mydt = null;

            try
            {
                mydt = BLL.CallRecord_ORIG.Instance.GetCallDataHuiMC(starttime, endtime, " c.CreateTime desc", currentPage, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataHuiMC ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }


            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataHuiMC ...获取数据操作结束!");
            return mydt;
        }

        /// <summary>
        /// 惠买车获取Inbound话务总表数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [WebMethod(Description = "惠买车获取Inbound话务总表数据")]
        public DataTable GetInboundDataHuiMC(string Verifycode, string starttime, string endtime, int currentPage, out int totalCount, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetInboundDataHuiMC ...BEGIN...开始时间：" + endtime + ",结束时间：" + endtime);
            totalCount = 0;
            int pageSize = 20;

            //
            string spageSize = ConfigurationUtil.GetAppSettingValue("CCInterfaceHMCPageSize");
            if (!string.IsNullOrEmpty(spageSize))
            {
                int itmp = 0;
                if (Int32.TryParse(spageSize, out itmp))
                {
                    pageSize = itmp;
                }
            }

            //验证授权码
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            if (sVerify == Verifycode)
            { }
            else
            {
                msg = "授权码错误!!";
                return null;
            }

            //验证开始、结束时间格式是否正确
            DateTime dt;
            if (!DateTime.TryParse(starttime, out dt))
            {
                msg = "开始时间格式不正确!!";
                return null;
            }

            if (!DateTime.TryParse(endtime, out dt))
            {
                msg = "结束时间格式不正确!!";
                return null;
            }

            DataTable mydt = null;

            try
            {
                mydt = BLL.CallRecord_ORIG.Instance.GetInboundDataHuiMC(starttime, endtime, " c.CreateTime desc", currentPage, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetInboundDataHuiMC ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }

            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetInboundDataHuiMC ...获取数据操作结束!");
            return mydt;
        }

        /// <summary>
        /// 惠买车获取工单数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="query">条件查询对象</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [WebMethod(Description = "惠买车获取工单数据")]
        public DataTable GetWorkOrderInfoHMC(string Verifycode, QueryWorkOrderInfo query, int currentPage, out int totalCount, ref string msg)
        {
            string starttime = query.BeginCreateTime;
            string endtime = query.EndCreateTime;
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]WorkOrderInfoExportHMC ...BEGIN...开始时间：" + starttime + ",结束时间：" + endtime);
            totalCount = 0;
            int pageSize = 20;

            string spageSize = ConfigurationUtil.GetAppSettingValue("CCInterfaceHMCPageSize");
            if (!string.IsNullOrEmpty(spageSize))
            {
                int itmp = 0;
                if (Int32.TryParse(spageSize, out itmp))
                {
                    pageSize = itmp;
                }
            }

            //验证授权码
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            if (sVerify == Verifycode)
            { }
            else
            {
                msg = "授权码错误!!";
                return null;
            }

            //验证开始、结束时间格式是否正确
            DateTime dt;
            if (!DateTime.TryParse(starttime, out dt))
            {
                msg = "开始时间格式不正确!!";
                return null;
            }

            if (!DateTime.TryParse(endtime, out dt))
            {
                msg = "结束时间格式不正确!!";
                return null;
            }

            DataTable mydt = null;

            try
            {
                mydt = BLL.WorkOrderInfo.Instance.WorkOrderInfoExportHMC(query, "woi.CreateTime DESC", currentPage, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]WorkOrderInfoExportHMC ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }

            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]WorkOrderInfoExportHMC ...获取数据操作结束!");
            return mydt;
        }

        /// <summary>
        /// 惠买车根据CallID获取话务总表数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="callid">话务标识</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [WebMethod(Description = "惠买车根据CallID获取话务总表数据")]
        public Entities.CallRecord_ORIG GetCallDataByCallIDHuiMC(string Verifycode, long callid, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataByCallIDHuiMC ...CallID：" + callid);
            //验证授权码
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            if (sVerify == Verifycode)
            { }
            else
            {
                msg = "授权码错误!!";
                return null;
            }


            Entities.CallRecord_ORIG model = null;

            try
            {
                model = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(callid);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataByCallIDHuiMC ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }


            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataByCallIDHuiMC ...获取数据操作结束!");
            return model;
        }

        /*

        /// <summary>
        /// 易车商城获取Inbound话务总表数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [WebMethod(Description = "易车商城获取Inbound话务总表数据")]
        public DataTable GetInboundDataShop(string Verifycode, string starttime, string endtime, int currentPage, out int totalCount, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetInboundDataShop ...BEGIN...开始时间：" + endtime + ",结束时间：" + endtime);
            totalCount = 0;
            int pageSize = 20;

            //
            string spageSize = ConfigurationUtil.GetAppSettingValue("CCInterfaceShopPageSize");
            if (!string.IsNullOrEmpty(spageSize))
            {
                int itmp = 0;
                if (Int32.TryParse(spageSize, out itmp))
                {
                    pageSize = itmp;
                }
            }

            //验证授权码
            //string sVerify = "";
            //sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            //if (sVerify == Verifycode)
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "易车商城获取Inbound话务总表数据，授权失败。"))
            { }
            else
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetInboundDataShop验证失败！msg=" + msg);
                return null;
            }

            //验证开始、结束时间格式是否正确
            DateTime dt;
            if (!DateTime.TryParse(starttime, out dt))
            {
                msg = "开始时间格式不正确!!";
                return null;
            }

            if (!DateTime.TryParse(endtime, out dt))
            {
                msg = "结束时间格式不正确!!";
                return null;
            }

            DataTable mydt = null;

            try
            {
                mydt = BLL.CallRecord_ORIG.Instance.GetInboundDataShop(starttime, endtime, " c.CreateTime desc", currentPage, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetInboundDataShop ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }

            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetInboundDataShop ...获取数据操作结束!");
            return mydt;
        }

        /// <summary>
        /// 易车商城获取工单数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="query">条件查询对象</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [WebMethod(Description = "易车商城获取工单数据")]
        public DataTable GetWorkOrderInfoShop(string Verifycode, QueryWorkOrderInfo query, int currentPage, out int totalCount, ref string msg)
        {
            string starttime = query.BeginCreateTime;
            string endtime = query.EndCreateTime;
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetWorkOrderInfoShop ...BEGIN...开始时间：" + starttime + ",结束时间：" + endtime);
            totalCount = 0;
            int pageSize = 20;

            string spageSize = ConfigurationUtil.GetAppSettingValue("CCInterfaceShopPageSize");
            if (!string.IsNullOrEmpty(spageSize))
            {
                int itmp = 0;
                if (Int32.TryParse(spageSize, out itmp))
                {
                    pageSize = itmp;
                }
            }

            //验证授权码
            //string sVerify = "";
            //sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            //if (sVerify == Verifycode)
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "易车商城获取工单数据，授权失败。"))
            { }
            else
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetWorkOrderInfoShop验证失败！msg=" + msg);
                return null;
            }

            //验证开始、结束时间格式是否正确
            DateTime dt;
            if (!DateTime.TryParse(starttime, out dt))
            {
                msg = "开始时间格式不正确!!";
                return null;
            }

            if (!DateTime.TryParse(endtime, out dt))
            {
                msg = "结束时间格式不正确!!";
                return null;
            }

            DataTable mydt = null;

            try
            {
                mydt = BLL.WorkOrderInfo.Instance.WorkOrderInfoExportShop(query, "woi.CreateTime DESC", currentPage, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetWorkOrderInfoShop ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }

            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetWorkOrderInfoShop ...获取数据操作结束!");
            return mydt;
        }

        */

        /// <summary>
        /// 根据业务分组，分类获取一段时间范围内的话务总表数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        [WebMethod(Description = "根据业务分组，分类获取一段时间范围内的话务总表数据")]
        public DataTable GetCallData(string Verifycode, CallRecord_ORIGCondition query, int PageIndex, int PageSize, out int totalCount, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallData ...BEGIN...开始时间：" + query.StartTime + ",结束时间：" + query.EndTime);
            totalCount = 0;
            //验证授权码
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            if (sVerify == Verifycode)
            { }
            else
            {
                msg = "授权码错误!!";
                return null;
            }

            //验证开始、结束时间格式是否正确
            DateTime dt;
            if (!DateTime.TryParse(query.StartTime, out dt))
            {
                msg = "开始时间格式不正确!!";
                return null;
            }

            if (!DateTime.TryParse(query.EndTime, out dt))
            {
                msg = "结束时间格式不正确!!";
                return null;
            }

            DataTable mydt = null;

            try
            {
                mydt = BLL.CallRecord_ORIG.Instance.GetCallData(query.StartTime, query.EndTime, query.BGID, query.SCID, " c.CreateTime desc", PageIndex, PageSize, out totalCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallData ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }


            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallData ...获取数据操作结束!");
            return mydt;
        }
        /// <summary>
        /// 根据CallID获取话务总表数据
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <param name="callid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [WebMethod(Description = "根据CallID获取话务总表数据")]
        public Entities.CallRecord_ORIG GetCallDataByCallID(string Verifycode, long callid, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataByCallID...CallID：" + callid);
            //验证授权码
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            if (sVerify == Verifycode)
            { }
            else
            {
                msg = "授权码错误!!";
                return null;
            }


            Entities.CallRecord_ORIG model = null;

            try
            {
                model = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(callid);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataByCallID...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
            }


            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCallDataByCallID...获取数据操作结束!");
            return model;
        }

        [WebMethod(Description = "新增客户信息")]
        public bool CCDataInterface_InsertCustData(string Verifycode, string jsonDataStr, out string CustID, out string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterface_InsertCustData]Begin...jsonDataStr:" + jsonDataStr);
            msg = "";
            CustID = "";
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "新增客户信息授权失败。"))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    JSONCustData info = null;
                    try
                    {
                        info = serializer.Deserialize<JSONCustData>(jsonDataStr);
                    }
                    catch (Exception ex)
                    {
                        BLL.Loger.Log4Net.Info("Json格式转换失败，失败原因：" + ex.Message);
                        msg = "Json格式转换失败!";
                        return false;
                    }

                    #region 验证数据格式
                    if (string.IsNullOrEmpty(info.CustName))
                    {
                        msg = "客户姓名是必填项!";
                        return false;
                    }
                    int sex = 0;
                    if (info.Sex != string.Empty && !int.TryParse(info.Sex, out sex))
                    {
                        msg = "性别类型必须是INT型!";
                        return false;
                    }
                    else if (string.IsNullOrEmpty(info.Sex))
                    {
                        msg = "性别是必填项!";
                        return false;
                    }
                    else if (sex > 2 || sex < 1)
                    {
                        msg = "性别值超出范围，应该是1或2!";
                        return false;
                    }

                    int custCategory = 0;
                    if (info.CustCategory != string.Empty && !int.TryParse(info.CustCategory, out custCategory))
                    {
                        msg = "客户类别必须是INT型!";
                        return false;
                    }
                    else if (string.IsNullOrEmpty(info.CustCategory))
                    {
                        msg = "客户类别是必填项!";
                        return false;
                    }


                    if (info.Tels == null)
                    {
                        msg = "电话是必填项!";
                        return false;
                    }
                    else if (info.Tels.Length == 0)
                    {
                        msg = "电话数组至少有一个电话!";
                        return false;
                    }

                    //Regex reTel = new Regex(@"(^0[0-9]{2,3}[0-9]{7,8}$)|(^13[0-9]{9}$)|(^15[0-9]{9}$)|(^18[0-9]{9}$)|(^19[0-9]{9}$)|(^14[0-9]{9}$)|(^400\d{7}$)");
                    foreach (string str in info.Tels)
                    {
                        if (!BLL.Util.IsTelephoneAnd400Tel(str))
                        {
                            msg = "电话格式错误!";
                            return false;
                        }
                    }

                    int pid = 0;
                    if (info.ProvinceID != string.Empty && !int.TryParse(info.ProvinceID, out pid))
                    {
                        msg = "省份ID类型必须是INT型!";
                        return false;
                    }
                    else if (string.IsNullOrEmpty(info.ProvinceID))
                    {
                        msg = "省份ID是必填项!";
                        return false;
                    }
                    int cityid = 0;
                    if (info.CityID != string.Empty && !int.TryParse(info.CityID, out cityid))
                    {
                        msg = "城市ID类型必须是INT型!";
                        return false;
                    }
                    else if (string.IsNullOrEmpty(info.CityID))
                    {
                        msg = "城市ID是必填项!";
                        return false;
                    }

                    int createuserid = 0;
                    if (int.TryParse(info.CreateUserID, out createuserid))
                    {

                    }

                    #endregion

                    if (BLL.CustBasicInfo.Instance.InsertCustInfo(info.CustName, info.Tels, sex, createuserid, custCategory, pid, cityid, out msg, out CustID))
                    {
                        BLL.Loger.Log4Net.Info("[CCDataInterface_InsertCustData]新增客户成功,CustID:" + CustID);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterface_InsertCustData.asmx]新增客户成功出错!errorStackTrace:" + ex.StackTrace);
            }
            return false;
        }


        [WebMethod(Description = "IM根据客户号获取业务记录")]
        public DataTable CCDataInterface_GetCustHistoryInfo(string Verifycode, int userid, QueryCustHistoryInfo query, int currentPage, out int totalCount, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCustHistoryInfo ...BEGIN...客户号：" + query.CustID);
            totalCount = 0;
            int pageSize = 9999;

            //验证授权码
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeCode");
            if (sVerify == Verifycode)
            { }
            else
            {
                msg = "授权码错误!!";
                return null;
            }


            DataTable mydt = null;

            try
            {
                mydt = BLL.CallRecord_ORIG.Instance.GetCustBaseInfo_ServiceRecord_IM(userid, query, currentPage, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCustHistoryInfo ...获取数据操作出错!errorStackTrace:" + ex.StackTrace);
                msg = "操作出错!errorStackTrace:" + ex.StackTrace;
            }

            BLL.Loger.Log4Net.Info("[CCDataInterfaceService.asmx]GetCustHistoryInfo获取数据操作结束,客户号：" + query.CustID);
            return mydt;
        }

        [WebMethod(Description = "IM获取用户信息")]
        public string GetEmployeeAgentInfo(string Verifycode, int userid)
        {
            BLL.Util.LogForWeb("info", "IM获取用户信息：GetEmployeeAgentInfo");
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "权限错误"))
                {
                    BLL.Util.LogForWeb("info", "权限验证失败：" + msg + "  传递参数：Verifycode（" + Verifycode + "）");
                    return "";
                }
                else
                {
                    //查询数据
                    DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgentForIM(userid);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        BLL.Util.LogForWeb("info", "查询数据为空");
                        return "";
                    }
                    else
                    {
                        string info = "{";
                        info += "'UserID':'" + userid + "',";
                        info += "'AgentNum':'" + dt.Rows[0]["AgentNum"].ToString() + "',";
                        info += "'UserName':'" + dt.Rows[0]["TrueName"].ToString() + "',";
                        info += "'BGID':'" + dt.Rows[0]["BGID"].ToString() + "',";
                        info += "'BGName':'" + dt.Rows[0]["BGName"].ToString() + "',";
                        info += "'BusinessLineIDs':'" + dt.Rows[0]["LineIds"].ToString().TrimEnd(',') + "',";
                        info += "'ManageBGIDs':'" + dt.Rows[0]["manageBgs"].ToString().TrimEnd(',') + "'}";
                        return info;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", ex.Message);
                BLL.Util.LogForWeb("info", ex.StackTrace);
                return "";
            }
            finally
            {
                BLL.Util.LogForWeb("info", "IM获取用户信息结束\r\n\r\n");
            }
        }

        [WebMethod(Description = "IM获取客户信息")]
        public string GetCustBasicInfo(string Verifycode, string tel, string name)
        {
            BLL.Util.LogForWeb("info", "IM获取客户信息：GetCustBasicInfo");
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "权限错误"))
                {
                    BLL.Util.LogForWeb("info", "权限验证失败：" + msg + "  传递参数：Verifycode（" + Verifycode + "）");
                    return "";
                }
                else
                {
                    //查询数据
                    DataTable dt = BLL.CustBasicInfo.Instance.GetCustBasicInfoForIM(tel);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        BLL.Util.LogForWeb("info", "查询数据为空");
                        return "";
                    }
                    else
                    {
                        string info = "{";
                        info += "'Tel':'" + tel + "',";
                        info += "'Name':'" + name + "',";
                        info += "'Sex':'" + dt.Rows[0]["sex"].ToString() + "',";
                        info += "'CustID':'" + dt.Rows[0]["CustID"].ToString() + "',";
                        info += "'ProvinceID':'" + dt.Rows[0]["ProvinceID"].ToString().TrimEnd(',') + "',";
                        info += "'CityID':'" + dt.Rows[0]["CityID"].ToString().TrimEnd(',') + "'}";
                        return info;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", ex.Message);
                BLL.Util.LogForWeb("info", ex.StackTrace);
                return "";
            }
            finally
            {
                BLL.Util.LogForWeb("info", "IM获取客户信息结束\r\n\r\n");
            }
        }

        [WebMethod(Description = "IM个人版添加工单url（手机号和对话id必填，其他选填，int类型-1代表空）")]
        public string GetAddWOrderComeIn_IMGR_URL(string Verifycode, CallSourceEnum callsource, string phone, long csid, string cbname, int cbsex, int province, int city, int county, int businesstype, int businesstag)
        {
            BLL.Util.LogForWeb("info", "IM个人版添加工单url：GetAddWOrderComeIn_IMGR_URL " + phone + " " + csid);
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "权限错误"))
                {
                    BLL.Util.LogForWeb("info", "权限验证失败：" + msg + "  传递参数：Verifycode（" + Verifycode + "）");
                    return "";
                }
                else
                {
                    string root = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress", false);
                    BLL.WOrderRequest q = BLL.WOrderRequest.AddWOrderComeIn_IMGR(callsource, phone, csid, cbname, cbsex, province, city, county, businesstype, businesstag);
                    return root + "/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
                }
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", ex.Message);
                BLL.Util.LogForWeb("info", ex.StackTrace);
                return "";
            }
        }
        [WebMethod(Description = "IM经销商版-新车-添加工单url（手机号和对话id必填，其他选填，int类型-1代表空）")]
        public string GetAddWOrderComeIn_IMJXS_NC_URL(string Verifycode, CallSourceEnum callsource, string phone, long csid, string cbname, int cbsex, int province, int city, int county, string membercode)
        {
            BLL.Util.LogForWeb("info", "IM经销商版添加工单url：GetAddWOrderComeIn_IMJXS_URL " + phone + " " + csid);
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "权限错误"))
                {
                    BLL.Util.LogForWeb("info", "权限验证失败：" + msg + "  传递参数：Verifycode（" + Verifycode + "）");
                    return "";
                }
                else
                {
                    string root = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress", false);
                    BLL.WOrderRequest q = BLL.WOrderRequest.AddWOrderComeIn_IMJXS(callsource, ModuleSourceEnum.M06_IM经销商_新车, phone, csid, cbname, cbsex, province, city, county, membercode);
                    return root + "/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
                }
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", ex.Message);
                BLL.Util.LogForWeb("info", ex.StackTrace);
                return "";
            }
        }
        [WebMethod(Description = "IM经销商版-二手车-添加工单url（手机号和对话id必填，其他选填，int类型-1代表空）")]
        public string GetAddWOrderComeIn_IMJXS_SC_URL(string Verifycode, CallSourceEnum callsource, string phone, long csid, string cbname, int cbsex, int province, int city, int county, string membercode)
        {
            BLL.Util.LogForWeb("info", "IM经销商版添加工单url：GetAddWOrderComeIn_IMJXS_URL " + phone + " " + csid);
            try
            {
                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "权限错误"))
                {
                    BLL.Util.LogForWeb("info", "权限验证失败：" + msg + "  传递参数：Verifycode（" + Verifycode + "）");
                    return "";
                }
                else
                {
                    string root = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress", false);
                    BLL.WOrderRequest q = BLL.WOrderRequest.AddWOrderComeIn_IMJXS(callsource, ModuleSourceEnum.M07_IM经销商_二手车, phone, csid, cbname, cbsex, province, city, county, membercode);
                    return root + "/WOrderV2/AddWOrderInfo.aspx?" + q.ToString();
                }
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", ex.Message);
                BLL.Util.LogForWeb("info", ex.StackTrace);
                return "";
            }
        }
    }

    /// <summary>
    /// 查询话务信息条件类
    /// </summary>
    //[XmlInclude(typeof(CallRecord_ORIGCondition))]
    public class CallRecord_ORIGCondition
    {
        private int _bgid = 0;
        private int _scid = 0;
        private string _businessid = "";
        private string _StartTime = "";
        private string _EndTime = "";

        /// <summary>
        /// 按业务组查询
        /// </summary>
        public int BGID
        {
            get { return _bgid; }
            set { _bgid = value; }
        }

        /// <summary>
        /// 按分类查询
        /// </summary>
        public int SCID
        {
            get { return _scid; }
            set { _scid = value; }
        }

        /// <summary>
        /// 按业务ID查询
        /// </summary>
        public string BusinessID
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
    }

    [Serializable]
    public class JSONCustData
    {
        private string _CustName = "";
        private string _Sex = "";
        private string[] tels = null;
        private string _ProvinceID = "";
        private string _CityID = "";
        private string _CreateUserID = "";
        private string _CustCategory = "";

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustName
        {
            get { return HttpUtility.UrlDecode(_CustName); }
            set { _CustName = value; }
        }
        /// <summary>
        /// 客户性别
        /// </summary>
        public string Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
        /// <summary>
        /// 客户电话
        /// </summary>
        public string[] Tels
        {
            get { return tels; }
            set { tels = value; }
        }
        /// <summary>
        /// 省份ID
        /// </summary>
        public string ProvinceID
        {
            get { return _ProvinceID; }
            set { _ProvinceID = value; }
        }

        /// <summary>
        /// 城市ID
        /// </summary>
        public string CityID
        {
            get { return _CityID; }
            set { _CityID = value; }
        }

        /// <summary>
        /// 创建者ID
        /// </summary>
        public string CreateUserID
        {
            get { return _CreateUserID; }
            set { _CreateUserID = value; }
        }

        public string CustCategory
        {
            get { return _CustCategory; }
            set { _CustCategory = value; }
        }
    }
}
