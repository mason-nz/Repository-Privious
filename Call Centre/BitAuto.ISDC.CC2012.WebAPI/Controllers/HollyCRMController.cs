using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.WebAPI.AutoCallService;
using BitAuto.ISDC.CC2012.WebAPI.Helper;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;
using System.Collections.Concurrent;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using Microsoft.Ajax.Utilities;

namespace BitAuto.ISDC.CC2012.WebAPI.Controllers
{
    public class HollyCRMController : ApiController
    {
        /// 验证黑白名单接口（http://apincc.sys1.bitauto.com/hollycrm/ivr?method=checkCallNo&business=1&callNo=13809321982&calledNo=87237676）
        /// <summary>
        /// 验证黑白名单接口
        /// </summary>
        /// <param name="method">checkCallNo</param>
        /// <param name="business">业务类型</param>
        /// <param name="callNo">来电号码</param>
        /// <param name="calledNo">被叫号码</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage IVR(string method, string callNo, string calledNo, int business)
        {
            CommonHelper.Log("[验证黑白名单接口] 参数：" + method + " " + business + " " + callNo + " " + calledNo);
            //一般用户
            UserType usertype = UserType.一般用户;
            try
            {
                bool flag = VerifyLogic(method, "checkCallNo", callNo, calledNo, "[验证黑白名单接口]");

                //数据库校验
                if (flag)
                {
                    //查询数据库校验
                    calledNo = CommonHelper.BeijiaoProcess(calledNo);
                    callNo = BitAuto.ISDC.CC2012.BLL.Util.HaoMaProcess(callNo);

                    string msg = "";
                    string r = BLL.BlackWhiteList.Instance.GetPhoneNumberType(callNo, calledNo, out msg).ToString();
                    usertype = (UserType)Enum.Parse(typeof(UserType), r);
                    CommonHelper.Log("[验证黑白名单接口] 参数calledNo=" + calledNo + ",callNo=" + callNo + ",结果为=" + msg + " " + usertype.ToString());
                }
                else
                {
                    usertype = UserType.一般用户;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[验证黑白名单接口] 异常：", ex);
                usertype = UserType.一般用户;
            }
            CommonHelper.Log("[验证黑白名单接口]\r\n");
            return CommonHelper.CreateXMLResult(((int)usertype).ToString());
        }

        /// 语音留言接口 （http://apincc.sys1.bitauto.com/hollycrm/ivr?method=addMediaInfo&business=1&callNo=13809321982&calledNo=02968210222&mediaInTime=2015-06-06 12:12:00&mediaOutTime=2015-06-06 12:14:00&filePath=/var/spool/hollyRecord/2015/06/3001/&fileName=3001_20150611161443831.mp3&contactID=20150611111301189_1_047_UIP-1）
        /// <summary>
        /// 语音留言接口
        /// </summary>
        /// <param name="method">addMediaInfo</param>
        /// <param name="business">业务分类</param>
        /// <param name="callNo">主叫号码</param>
        /// <param name="calledNo">被叫号码</param>
        /// <param name="contactID">联络ID</param>
        /// <param name="mediaInTime">录音开始时间</param>
        /// <param name="mediaOutTime">录音结束时间</param>
        /// <param name="filePath">录音文件路径</param>
        /// <param name="fileName">录音文件名称</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage IVR(string method, int business, string callNo, string calledNo, string contactID, DateTime mediaInTime, DateTime mediaOutTime, string filePath, string fileName)
        {
            CommonHelper.Log("[语音留言接口] 参数：" + method + " " + business + " " + callNo + " " + calledNo + " " + contactID + " " + mediaInTime + " " + mediaOutTime + " " + filePath + " " + fileName);
            ResultType resulttype = ResultType.成功;
            try
            {
                #region 校验
                bool flag = CommonHelper.CheckIP("[语音留言接口]");
                if (method.ToLower() != "addMediaInfo".ToLower())
                {
                    CommonHelper.Log("[语音留言接口] method 错误：" + method);
                    flag = false;
                }
                if (string.IsNullOrEmpty(callNo))
                {
                    CommonHelper.Log("[语音留言接口] 参数（主叫号码）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(calledNo))
                {
                    CommonHelper.Log("[语音留言接口] 参数（被叫号码）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(contactID))
                {
                    CommonHelper.Log("[语音留言接口] 参数（联络ID）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    CommonHelper.Log("[语音留言接口] 参数（录音文件路径）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    CommonHelper.Log("[语音留言接口] 参数（录音文件名称）为空");
                    flag = false;
                }
                #endregion

                if (!filePath.StartsWith("/"))
                {
                    filePath = "/" + filePath;
                }
                if (!filePath.EndsWith("/"))
                {
                    filePath = filePath + "/";
                }

                string host = ConfigurationUtil.GetAppSettingValue("RecordURl").TrimEnd('/');
                calledNo = CommonHelper.BeijiaoProcess(calledNo);
                callNo = BitAuto.ISDC.CC2012.BLL.Util.HaoMaProcess(callNo);

                //数据入库
                if (flag)
                {
                    //数据入库
                    if (BLL.CustomerVoiceMsg.Instance.GetCustomerVoiceMsgInfo(contactID) == null)
                    {
                        CustomerVoiceMsgInfo info = new CustomerVoiceMsgInfo();
                        info.Vender = (int)Vender.Holly;
                        info.CallNO = callNo;
                        info.CalledNo = calledNo;
                        info.StartTime = mediaInTime;
                        info.EndTime = mediaOutTime;
                        info.FileFullName = host + filePath + fileName;
                        info.SessionID = contactID;
                        info.SourceType = 1;//来源：1 留言 2未接来电
                        info.Status = 0;//状态：0待处理 1处理中 2已处理
                        info.CreateTime = DateTime.Now;
                        BLL.CommonBll.Instance.InsertComAdoInfo(info);

                        resulttype = ResultType.成功;
                    }
                    else
                    {
                        CommonHelper.Log("[语音留言接口] 数据重复：" + contactID);
                        resulttype = ResultType.失败;
                    }
                }
                else
                {
                    resulttype = ResultType.失败;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[语音留言接口] 异常：", ex);
                resulttype = ResultType.失败;
            }
            CommonHelper.Log("[语音留言接口]\r\n");
            return CommonHelper.CreateXMLResult(((int)resulttype).ToString());
        }

        /// 满意度调查接口 （http://apincc.sys1.bitauto.com/hollycrm/ivr?method=addSatisfyIn&business=0&serviceId=30021433992425459&surveyResult=21）
        /// <summary>
        /// 满意度调查接口
        /// </summary>
        /// <param name="method">addSatisfyIn</param>
        /// <param name="business">业务分类</param>
        /// <param name="serviceId">话务单标识</param>
        /// <param name="surveyResult">满意度结果</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage IVR(string method, int business, string serviceId, string surveyResult)
        {
            CommonHelper.Log("[满意度调查接口] 参数：" + method + " " + business + " " + serviceId + " " + surveyResult);
            ResultType resulttype = ResultType.成功;
            try
            {
                #region 校验
                bool flag = CommonHelper.CheckIP("[满意度调查接口]");
                if (method.ToLower() != "addSatisfyIn".ToLower())
                {
                    CommonHelper.Log("[满意度调查接口] method 错误：" + method);
                    flag = false;
                }
                if (string.IsNullOrEmpty(serviceId))
                {
                    CommonHelper.Log("[满意度调查接口] 参数（CallID）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(surveyResult))
                {
                    CommonHelper.Log("[满意度调查接口] 参数（满意度结果）为空");
                    flag = false;
                }
                if (CommonFunction.ObjectToLong(serviceId) == 0)
                {
                    CommonHelper.Log("[满意度调查接口] 参数（CallID）格式不正确：" + serviceId);
                    flag = false;
                }
                if (CommonFunction.ObjectToInteger(surveyResult) == 0)
                {
                    CommonHelper.Log("[满意度调查接口] 参数（满意度结果）格式不正确：" + surveyResult);
                    flag = false;
                }
                #endregion

                //数据入库
                if (flag)
                {
                    //数据入库
                    Entities.IVRSatisfaction model = new Entities.IVRSatisfaction();
                    model.CallID = -2;
                    model.CallRecordID = CommonFunction.ObjectToLong(serviceId);
                    model.Score = CommonFunction.ObjectToInteger(surveyResult);
                    model.CreateTime = DateTime.Now;
                    int oid = BitAuto.ISDC.CC2012.BLL.IVRSatisfaction.Instance.Insert(model);
                    if (oid > 0)
                    {
                        resulttype = ResultType.成功;
                    }
                    else
                    {
                        resulttype = ResultType.失败;
                    }
                }
                else
                {
                    resulttype = ResultType.失败;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[满意度调查接口] 异常：", ex);
                resulttype = ResultType.失败;
            }
            CommonHelper.Log("[满意度调查接口]\r\n");
            return CommonHelper.CreateXMLResult(((int)resulttype).ToString());
        }

        /// 业务信息推送接口（http://apincc.sys1.bitauto.com/hollycrm/ivr?method=addIvrInfo&business=0&callNo=13809321982&calledNo=02968210222&startTime=2015-06-06 13:58:21&endTime=2015-06-06 14:58:21&handle=20150611111301189_1_047_UIP-1&ivrKey=1）
        /// <summary>
        /// 业务信息推送接口
        /// </summary>
        /// <param name="method"></param>
        /// <param name="business"></param>
        /// <param name="callNo"></param>
        /// <param name="calledNo"></param>
        /// <param name="handle"></param>
        /// <param name="ivrKey"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage IVR(string method, int business, string callNo, string calledNo, string handle, string ivrKey, DateTime startTime, DateTime endTime)
        {
            CommonHelper.Log("[业务信息推送接口] 参数：" + method + " " + business + " " + callNo + " " + calledNo + " " + handle + " " + ivrKey + " " + startTime + " " + endTime);
            ResultType resulttype = ResultType.成功;
            try
            {
                #region 校验
                bool flag = CommonHelper.CheckIP("[业务信息推送接口]");
                if (method.ToLower() != "addIvrInfo".ToLower())
                {
                    CommonHelper.Log("[业务信息推送接口] method 错误：" + method);
                    flag = false;
                }
                if (string.IsNullOrEmpty(callNo))
                {
                    CommonHelper.Log("[业务信息推送接口] 参数（主叫号码）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(calledNo))
                {
                    CommonHelper.Log("[业务信息推送接口] 参数（被叫号码）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(handle))
                {
                    CommonHelper.Log("[业务信息推送接口] 参数（厂家ID）为空");
                    flag = false;
                }
                if (string.IsNullOrEmpty(ivrKey))
                {
                    CommonHelper.Log("[业务信息推送接口] 参数（IVR按键路径记录）为空");
                    flag = false;
                }

                calledNo = CommonHelper.BeijiaoProcess(calledNo);
                callNo = BitAuto.ISDC.CC2012.BLL.Util.HaoMaProcess(callNo);
                #endregion

                //数据入库
                if (flag)
                {
                    ////根据厂家id获取cc的callid
                    //Entities.CallRecord_ORIG model = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGBySessionID(handle);
                    //long callid = -2;
                    //if (model != null)
                    //{
                    //    callid = model.CallID.Value;
                    //}

                    //数据入库
                    CustomerCallInInfo info = new CustomerCallInInfo();
                    info.VenderCallID = handle;
                    info.Vender = (int)Vender.Holly;
                    info.CallNO = callNo;
                    info.CalledNo = calledNo;
                    info.StartTime = startTime;
                    info.EndTime = endTime;
                    info.IvrKeys = ivrKey;
                    info.CallID = -1;
                    info.CreateTime = DateTime.Now;

                    if (BLL.CustomerCallIn.Instance.GetCustomerCallInInfoByVenderCallID(handle) == null)
                    {
                        //新增
                        BLL.CommonBll.Instance.InsertComAdoInfo(info);
                    }
                    else
                    {
                        //修改
                        BLL.CommonBll.Instance.UpdateComAdoInfo(info);
                    }

                    ////删除详细表数据
                    //BLL.CustomerCallInPressKey.Instance.DeleteDataByVenderCallID(handle);
                    ////拆分入库详细表
                    //int sort = 1;
                    //foreach (char a in ivrKey.ToCharArray())
                    //{
                    //    CustomerCallInPressKeyInfo item = new CustomerCallInPressKeyInfo();
                    //    item.VenderCallID = handle;
                    //    item.Vender = (int)Vender.Holly;
                    //    item.SortNum = sort;
                    //    item.PressKey = a.ToString();
                    //    item.CallID = callid;
                    //    item.CreateTime = DateTime.Now;
                    //    BLL.CommonBll.Instance.InsertComAdoInfo(item);
                    //    sort++;
                    //}

                    resulttype = ResultType.成功;
                }
                else
                {
                    resulttype = ResultType.失败;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[业务信息推送接口] 异常：", ex);
                resulttype = ResultType.失败;
            }
            CommonHelper.Log("[业务信息推送接口]\r\n");
            return CommonHelper.CreateXMLResult(((int)resulttype).ToString());
        }

        /// 呼出专属客服，根据呼入主、被叫号码查询近期外呼客服工号接口
        /// <summary>
        /// 呼出专属客服，根据呼入主、被叫号码查询近期外呼客服工号接口
        /// </summary>
        /// <param name="method">执行具体方法</param>
        /// <param name="callNo">主叫号码</param>
        /// <param name="calledNo">被叫号码</param>
        /// <returns>若能找到，则返回客服工号；若找不到，则返回空。</returns>
        [HttpGet]
        public HttpResponseMessage IVR(string method, string callNo, string calledNo)
        {
            CommonHelper.Log("[专属客服接口] 参数：" + method + " " + callNo + " " + calledNo);
            string agentID = string.Empty;//坐席工号
            try
            {
                bool flag = VerifyLogic(method, "getagentidbycallno", callNo, calledNo, "[验证专属客服接口]");

                //数据库校验
                if (flag)
                {
                    CommonHelper.Log("[专属客服接口] 读取配置文件，获取热线信息");
                    string path = AppDomain.CurrentDomain.BaseDirectory + "1-热线数据统计.xml";
                    Dictionary<string, string> hotlineConfig = CommonFunction.GetAllNodeContentByFile<string, string>(path, "key", "OutCallNum", null);
                    if (hotlineConfig != null)
                    {
                        //根据被叫查询出局号码
                        string callOutPrefix = hotlineConfig.FirstOrDefault(x => calledNo.Contains(x.Key)).Value;
                        //读取有效期
                        string validitytime = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExclusiveAgentValidityTime", false);
                        //解析有效期
                        DateTime querytime = DateTime.Today.AddDays(-1 * CommonFunction.ObjectToInteger(validitytime));
                        //查询专属坐席
                        string callOutStartTime = "";
                        agentID = BLL.HollyData.Instance.GetLastAgentIDByORIDNIS(callNo, callOutPrefix, querytime, out callOutStartTime);
                        CommonHelper.Log(string.Format("[专属客服接口] 主叫号码={0},被叫号码={1},专属坐席ID={2},上次外呼时间={3},被叫号码外呼出局号={4},查询专属坐席时间范围（大于此时间）={5}",
                            callNo, calledNo, agentID, callOutStartTime, callOutPrefix, querytime.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                    else
                    {
                        CommonHelper.Log("[专属客服接口] 配置文件错误！");
                        agentID = "";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[专属客服接口] 异常：", ex);
            }
            return CommonHelper.CreateXMLResult(agentID);
        }


        /// 获取自动外呼数据接口
        /// <summary>
        /// 获取自动外呼数据接口
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Servelet(string method)
        {
            try
            {
                #region 校验
                bool flag = CommonHelper.CheckIP("[自动外呼业务信息推送接口]");
                if (!flag)
                {
                    throw new Exception("IP地址非法");
                }
                if (method.ToLower() != "addCallSampleIn".ToLower())
                {
                    throw new Exception("非法方法，方法名异常,异常URL：" + HttpContext.Current.Request.Url.OriginalString);
                }
                #endregion

                bool isNull = false;
                var client = new ACProviderClient();
                var s = client.HollyGetOneData(out isNull);

                if (!isNull)
                {
                    CommonHelper.Log(string.Format("[自动外呼业务信息推送接口调用成功, 调用数据:{0} {1}]", Environment.NewLine, s));


                }
                client.Close();
                client = null;
                return new HttpResponseMessage { Content = new StringContent(s, System.Text.Encoding.UTF8, "application/xml") };
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[自动外呼业务信息推送接口异常]", ex);
                return CommonHelper.CreateCallInXMLResult(null);
            }

        }
        /// 自动外呼回写接口
        /// <summary>
        /// 自动外呼回写接口
        /// </summary>
        /// <param name="method"></param>
        /// <param name="Id"></param>
        /// <param name="callResult"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Servelet(string method, string Id, string callResult)
        {
            try
            {
                #region 校验
                bool flag = CommonHelper.CheckIP("[自动外呼业务信息推送接口]");
                if (!flag)
                {
                    throw new Exception("IP地址非法");
                }
                if (method.ToLower() != "finishCallSample".ToLower())
                {
                    throw new Exception("非法方法，方法异常");
                }

                if (string.IsNullOrEmpty(Id))
                {
                    throw new Exception("非法方法，参数异常，Id不能为空");
                }
                if (string.IsNullOrEmpty(callResult))
                {
                    throw new Exception("非法方法，参数异常，callResult不能为空");
                }
                #endregion

                CommonHelper.Log(string.Format("【自动外呼业务信息回写数据接口调用成功，参数如下：id:{0},callResult:{1}】", Id, callResult));
                AutoCallService.ACProviderClient client = new ACProviderClient();
                var s = client.HollyFinishCall(Id, callResult);
                client.Close();
                return new HttpResponseMessage { Content = new StringContent(s, System.Text.Encoding.UTF8, "application/xml") };
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[自动外呼业务信息推送接口异常]", ex);
                return CommonHelper.CreateXMLResult("0");
            }
        }
        /// 自动外呼项目停止接口
        /// <summary>
        /// 自动外呼项目停止接口
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="sas"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage HaltStop(string pid, string sas)
        {
            try
            {
                #region 校验
                bool flag = CommonHelper.CheckIP("[自动外呼业务暂停停止项目接口]");
                if (!flag)
                {
                    throw new Exception("IP地址非法");
                }
                if (string.IsNullOrEmpty(pid))
                {
                    throw new Exception("非法方法，参数异常，pid不能为空");
                }
                if (string.IsNullOrEmpty(sas))
                {
                    throw new Exception("非法方法，参数异常，sas不能为空");
                }
                #endregion

                CommonHelper.Log(string.Format("【自动外呼业务暂停停止项目接口调用成功，参数如下：pid:{0},sas:{1}】", pid, sas));
                int nStatus = -1;
                if (int.TryParse(sas, out nStatus))
                {
                    if (nStatus != 2 && nStatus != 3)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{code:-1,msg:'状态操作必须为为数字：2,3'}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    AutoCallService.ACProviderClient client = new ACProviderClient();
                    client.HaltOrStopProject(pid, nStatus);
                    client.Close();
                    return new HttpResponseMessage { Content = new StringContent("{code:0,msg:''}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{code:-1,msg:'状态操作必须为为数字：2,3'}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[自动外呼业务信息推送接口异常]", ex);
                return new HttpResponseMessage { Content = new StringContent("{code:-1,msg:\"" + ex.Message + "\"}", System.Text.Encoding.UTF8, "application/xml") };
            }
        }

        private static readonly string cckey = "yiche-ClineLog-!@#$#@!";
        /// 获取当前的任务ID
        /// <summary>
        /// 获取当前的任务ID
        /// </summary>
        /// <param name="beijiao"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCurrentTaskID(string key, string beijiao)
        {
            try
            {
                #region 校验
                if (key != cckey)
                {
                    throw new Exception("[GetCurrentTaskID] 秘钥不正确：" + key);
                }
                #endregion

                string taskid = BLL.AutoCall_TaskInfo.Instance.GetCurrentTaskIDByPhone(beijiao);
                CommonHelper.Log("[获取当前的任务ID接口] 手机号：" + beijiao);
                return new HttpResponseMessage { Content = new StringContent(taskid, System.Text.Encoding.UTF8, "application/xml") };
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[获取当前的任务ID接口异常]", ex);
                return new HttpResponseMessage { Content = new StringContent("", System.Text.Encoding.UTF8, "application/xml") };
            }
        }

        /// 验证IP及参数逻辑
        /// <summary>
        /// 验证IP及参数逻辑
        /// </summary>
        /// <param name="method">方法名参数</param>
        /// <param name="methodValue">方法名具体值</param>
        /// <param name="callNo">主叫号码</param>
        /// <param name="calledNo">被叫号码</param>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>验证通过，返回True，否则返回False</returns>
        private static bool VerifyLogic(string method, string methodValue, string callNo, string calledNo, string interfaceName)
        {
            bool flag = CommonHelper.CheckIP(interfaceName);
            if (method.ToLower() != methodValue.ToLower())
            {
                CommonHelper.Log(interfaceName + " method 错误：" + method);
                flag = false;
            }
            if (string.IsNullOrEmpty(callNo))
            {
                CommonHelper.Log(interfaceName + " 参数（来电号码）为空");
                flag = false;
            }
            if (string.IsNullOrEmpty(calledNo))
            {
                CommonHelper.Log(interfaceName + " 参数（被叫号码）为空");
                flag = false;
            }
            return flag;
        }

        #region 测试代码
        private static int nCallNum = 0;
        /// <summary>
        /// http://apincc.sys1.bitauto.com/hollycrm/servelet?method=addCallSampleIn
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Servelet_test(string method)
        {
            try
            {

                if (nCallNum++ < 1)
                {
                    Random rnd = new Random();
                    var cinf = new CCallInInfo()
                    {
                        Result = "2", //(nT++ % 4).ToString(),
                        Id = rnd.Next(0, 10000).ToString(), //"1",//row[0].ToString(),
                        mphone = "86013146611863",
                        voiceCode = "1234",
                        skillId = "",
                        ivrNo = "" //"ivrNo" + Interlocked.Increment(ref lgIdTmp)
                    };
                    var sR = CommonHelper.CreateCallInXMLResult(cinf);
                    CommonHelper.Log("推送数据调用成功。");
                    return sR;
                }
                else
                {
                    var sRNull = CommonHelper.CreateCallInXMLResult(null);
                    CommonHelper.Log("调用成功,数据为空。");
                    return sRNull;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.Log("[自动外呼业务信息推送接口异常]", ex);
                return CommonHelper.CreateCallInXMLResult(null);
            }
        }
        #endregion
    }
}
