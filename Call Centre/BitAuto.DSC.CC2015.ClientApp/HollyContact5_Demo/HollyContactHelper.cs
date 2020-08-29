using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxUniSoftPhoneControl;

namespace HollyContact5_Demo
{
    /// 合力厂家电话具体实现类
    /// <summary>
    /// 合力厂家电话具体实现类
    /// 强斐
    /// 2015-6-3
    /// </summary>
    public class HollyContactHelper
    {
        private static AxUniSoftPhone axUniSoftPhone;
        /// 电话控件
        /// <summary>
        /// 电话控件
        /// </summary>
        public static AxUniSoftPhone AxUniSoftPhone
        {
            get
            {
                return axUniSoftPhone;
            }
            set
            {
                axUniSoftPhone = value;
                //注册消息事件和错误事件
                RegisterLogEvent();
            }
        }

        #region 日志记录
        /// 注册消息和错误方法事件
        /// <summary>
        /// 注册消息和错误方法事件
        /// </summary>
        private static void RegisterLogEvent()
        {
            axUniSoftPhone.OnMessage -= new IUniSoftPhoneEvents_OnMessageEventHandler(axunisoftphone_OnMessage);
            axUniSoftPhone.OnError -= new IUniSoftPhoneEvents_OnErrorEventHandler(axunisoftphone_OnError);
            axUniSoftPhone.OnStatusChange -= new EventHandler(axUniSoftPhone_OnStatusChange);

            axUniSoftPhone.OnMessage += new IUniSoftPhoneEvents_OnMessageEventHandler(axunisoftphone_OnMessage);
            axUniSoftPhone.OnError += new IUniSoftPhoneEvents_OnErrorEventHandler(axunisoftphone_OnError);
            axUniSoftPhone.OnStatusChange += new EventHandler(axUniSoftPhone_OnStatusChange);
        }
        static void axunisoftphone_OnMessage(object sender, IUniSoftPhoneEvents_OnMessageEvent e)
        {
            string message = string.Format("CurStatus：{0}；CurStatusName：{1}；", axUniSoftPhone.CurStatus, axUniSoftPhone.CurStatusName);
            Loger.Log4Net.Info("[@@@厂家消息通知@@@]OnMessage ：厂家状态（" + message + "）厂家消息（" + e.messageContent + "）程序状态（" + ConvertPhoneStatus(axUniSoftPhone.CurStatus) + "）");
        }
        static void axunisoftphone_OnError(object sender, IUniSoftPhoneEvents_OnErrorEvent e)
        {
            string message = string.Format("[^^^厂家错误通知^^^]OnError ：CTIErrorCode:{0}   ErrorCode:{1}  ErrorDesc:{2}", e.cTIErrorCode, e.errCode, e.errDesc);
            Loger.Log4Net.Info(message);
        }
        static void axUniSoftPhone_OnStatusChange(object sender, EventArgs e)
        {
            string message = ConvertPhoneStatus(axUniSoftPhone.PreStatus) + "（" + axUniSoftPhone.PreStatus + "） -> " +
                ConvertPhoneStatus(axUniSoftPhone.CurStatus) + "（" + axUniSoftPhone.CurStatus + "）";
            Loger.Log4Net.Info("[>>>厂家状态变化<<<]OnStatusChange ：" + message);
        }
        #endregion

        public static readonly HollyContactHelper Instance = new HollyContactHelper();

        private HollyContactHelper()
        {
        }

        /// 装载及初始化
        /// <summary>
        /// 装载及初始化
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            axUniSoftPhone.LoadDLL("SoftPhoneForHollyContact5.dll");
            bool flag = axUniSoftPhone.actInitialize();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==装载及初始化->" + flag);
            return flag;
        }
        /// 卸载
        /// <summary>
        /// 卸载
        /// </summary>
        public void UnLoad()
        {
            axUniSoftPhone.UnLoadDLL();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==卸载->" + true);
        }
        /// 添加技能组
        /// <summary>
        /// 添加技能组
        /// </summary>
        /// <param name="skills"></param>
        public void AndSkill(Dictionary<string, string> skills)
        {
            if (skills == null) return;
            foreach (string strSkillGroup in skills.Keys)
            {
                //技能组，组内级别
                axUniSoftPhone.AddOneSkillGroup(strSkillGroup, skills[strSkillGroup]);
                Loger.Log4Net.Info("[HollyContactHelper]用户操作==添加技能组->" + strSkillGroup + " " + skills[strSkillGroup]);
            }
        }
        /// 签入
        /// <summary>
        /// 签入
        /// </summary>
        public bool SignIn(string WorkNo, string AgentDN)
        {
            //工号
            axUniSoftPhone.WorkNo = WorkNo;
            //分机
            axUniSoftPhone.AgentDN = AgentDN;
            bool flag = axUniSoftPhone.actSignIn();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==签入->" + flag);
            return flag;
        }
        /// 签出
        /// <summary>
        /// 签出
        /// </summary>
        public bool SignOut()
        {
            bool flag = axUniSoftPhone.actSignOut();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==签出->" + flag);
            return flag;
        }

        /// 置闲
        /// <summary>
        /// 置闲
        /// </summary>
        /// <returns></returns>
        public bool SetReady()
        {
            bool flag = axUniSoftPhone.actIdle();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==置闲->" + flag);
            return flag;
        }
        /// 置忙
        /// <summary>
        /// 置忙
        /// </summary>
        /// <returns></returns>
        private bool SetBusy()
        {
            bool flag = axUniSoftPhone.actBusy();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==置忙->" + flag);
            return flag;
        }
        /// 从其他状态转换置闲状态
        /// <summary>
        /// 从其他状态转换置闲状态
        /// </summary>
        /// <returns></returns>
        public bool ToReady()
        {
            PhoneStatus cur = ConvertPhoneStatus(GetCurStatus());
            if (cur == PhoneStatus.PS04_置忙)
            {
                //置闲
                return HollyContactHelper.Instance.SetReady();
            }
            else if (cur == PhoneStatus.PS05_休息)
            {
                //结束休息
                return HollyContactHelper.Instance.RestEnd();
            }
            else if (cur == PhoneStatus.PS06_话后)
            {
                //结束话后
                return HollyContactHelper.Instance.AfterCallEnd();
            }
            else return false;
        }

        /// 应答
        /// <summary>
        /// 应答
        /// </summary>
        /// <returns></returns>
        public bool ActAnswer()
        {
            bool flag = axUniSoftPhone.actAnswer();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==应答->" + flag);
            return flag;
        }
        /// 挂机
        /// <summary>
        /// 挂机
        /// </summary>
        /// <returns></returns>
        public bool ActHangup()
        {
            bool flag = axUniSoftPhone.actHangup();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==挂机->" + flag);
            return flag;
        }
        /// 外呼
        /// <summary>
        /// 外呼
        /// </summary>
        /// <returns></returns>
        public bool ActCallOut(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actCallOut();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==外呼（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }

        /// 话后开始
        /// <summary>
        /// 话后开始
        /// </summary>
        /// <returns></returns>
        public bool AfterCallStart()
        {
            bool flag = axUniSoftPhone.actNeaten();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==话后开始->" + flag);
            return flag;
        }
        /// 话后结束
        /// <summary>
        /// 话后结束
        /// </summary>
        /// <returns></returns>
        public bool AfterCallEnd()
        {
            bool flag = axUniSoftPhone.actEndNeaten();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==话后结束->" + flag);
            return flag;
        }

        /// 保持开始
        /// <summary>
        /// 保持开始
        /// </summary>
        /// <returns></returns>
        public bool HoldStart()
        {
            bool flag = axUniSoftPhone.actHold();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==保持开始->" + flag);
            return flag;
        }
        /// 取消保持
        /// <summary>
        /// 取消保持
        /// </summary>
        /// <returns></returns>
        public bool HoldEnd()
        {
            bool flag = axUniSoftPhone.actCancelHold();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==取消保持->" + flag);
            return flag;
        }
        /// 休息
        /// <summary>
        /// 休息
        /// </summary>
        /// <param name="busystatus"></param>
        /// <returns></returns>
        public bool RestStart(BusyStatus busystatus)
        {
            string name =Util.GetEnumOptText(typeof(BusyStatus), (int)busystatus);
            axUniSoftPhone.SetOneBusiParamData("RestCode", name);
            bool flag = axUniSoftPhone.actRest();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==休息（" + busystatus + "）->" + flag);
            return flag;
        }
        /// 结束休息
        /// <summary>
        /// 结束休息
        /// </summary>
        /// <returns></returns>
        public bool RestEnd()
        {
            bool flag = axUniSoftPhone.actEndRest();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==结束休息->" + flag);
            return flag;
        }

        /// 开始转接
        /// <summary>
        /// 开始转接
        /// </summary>
        /// <returns></returns>
        public bool TransferStart(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actTransferCall();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==开始转接（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }
        /// 转接取消
        /// <summary>
        /// 转接取消
        /// </summary>
        /// <returns></returns>
        public bool TransferCancel()
        {
            bool flag = axUniSoftPhone.actCancelTransfer();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==转接取消->" + flag);
            return flag;
        }

        /// 开始咨询
        /// <summary>
        /// 开始咨询
        /// </summary>
        /// <returns></returns>
        public bool ConsultStart(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actConsult();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==开始咨询（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }
        /// 取消咨询
        /// <summary>
        /// 取消咨询
        /// </summary>
        /// <returns></returns>
        public bool ConsultCancel()
        {
            bool flag = axUniSoftPhone.actCancelConsult();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==取消咨询->" + flag);
            return flag;
        }
        /// 结束咨询
        /// <summary>
        /// 结束咨询
        /// </summary>
        /// <returns></returns>
        public bool ConsultEnd()
        {
            bool flag = axUniSoftPhone.actEndConsult();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==结束咨询->" + flag);
            return flag;
        }
        /// 咨询变转接
        /// <summary>
        /// 咨询变转接
        /// </summary>
        /// <returns></returns>
        public bool ConsultToTransfer()
        {
            bool flag = axUniSoftPhone.actForwardCall();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==咨询变转接->" + flag);
            return flag;
        }
        /// 咨询变会以
        /// <summary>
        /// 咨询变会以
        /// </summary>
        /// <returns></returns>
        public bool ConsultToConference()
        {
            bool flag = axUniSoftPhone.actConsult2Conference();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==咨询变会以->" + flag);
            return flag;
        }

        /// 开始会议
        /// <summary>
        /// 开始会议
        /// </summary>
        /// <returns></returns>
        public bool ConferenceStart(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actConference();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==开始会议（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }
        /// 取消会议
        /// <summary>
        /// 取消会议
        /// </summary>
        /// <returns></returns>
        public bool ConferenceCancel()
        {
            bool flag = axUniSoftPhone.actCancelConference();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==取消会议->" + flag);
            return flag;
        }
        /// 会议结束
        /// <summary>
        /// 会议结束
        /// </summary>
        /// <returns></returns>
        public bool ConferenceEnd()
        {
            bool flag = axUniSoftPhone.actEndConference();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==会议结束->" + flag);
            return flag;
        }

        /// 开始监听
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <returns></returns>
        public bool ListenStart(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actListen();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==开始监听（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }
        /// 监听取消
        /// <summary>
        /// 监听取消
        /// </summary>
        /// <returns></returns>
        public bool ListenEnd()
        {
            bool flag = ActHangup();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==监听取消->" + flag);
            return flag;
        }

        /// 强插
        /// <summary>
        /// 强插
        /// </summary>
        /// <returns></returns>
        public bool ListenToQC(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actForceInsert();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==强插（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }
        /// 拦截
        /// <summary>
        /// 拦截
        /// </summary>
        /// <returns></returns>
        public bool ListenToLJ(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actIntercept();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==拦截（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }

        /// <summary>
        /// 强拆
        /// </summary>
        /// <returns></returns>
        public bool ListenToQCai(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actDisconnectCall();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==强拆（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }

        /// <summary>
        /// 强制置闲
        /// </summary>
        /// <returns></returns>
        public bool ListenToQZZX(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actForceSetIdle();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==强制置闲（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }

        /// <summary>
        /// 强制置忙
        /// </summary>
        /// <returns></returns>
        public bool ListenToQZZM(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actForceSetBusy();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==强制置忙（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }

        /// <summary>
        /// 强制签出
        /// </summary>
        /// <returns></returns>
        public bool ListenToQZQC(ConDeviceType devicetype, string DestNo)
        {
            axUniSoftPhone.DestDeviceType = ConvertConDeviceType(devicetype);
            axUniSoftPhone.DestNo = DestNo;
            bool flag = axUniSoftPhone.actForceLogout();
            Loger.Log4Net.Info("[HollyContactHelper]用户操作==强制签出（" + devicetype + "," + DestNo + "）->" + flag);
            return flag;
        }

        /// 获取当前状态
        /// <summary>
        /// 获取当前状态
        /// </summary>
        /// <returns></returns>
        public int GetCurStatus()
        {
            return axUniSoftPhone.CurStatus;
        }
        /// 获取前一个状态
        /// <summary>
        /// 获取前一个状态
        /// </summary>
        /// <returns></returns>
        public int GetPreStatus()
        {
            return axUniSoftPhone.PreStatus;
        }

        public int GetConCallType()
        {
            return 0;
        }

        #region 枚举转换
        public static int ConvertConCallType(ConCallType calltype)
        {
            switch (calltype)
            {
                case ConCallType.未定义:
                    return axUniSoftPhone.ConCallType_ctUnknow;
                case ConCallType.呼入_分配接入:
                    return axUniSoftPhone.ConCallType_ctIVRTransIn;
                case ConCallType.呼入_内部转入:
                    return axUniSoftPhone.ConCallType_ctInnerTransIn;
                case ConCallType.呼入_内部拨入:
                    return axUniSoftPhone.ConCallType_ctInnerDialIn;
                case ConCallType.呼入_咨询接入:
                    return axUniSoftPhone.ConCallType_ctConsultIn;
                case ConCallType.呼入_会议接入:
                    return axUniSoftPhone.ConCallType_ctConfIn;
                case ConCallType.呼出_内部呼叫:
                    return axUniSoftPhone.ConCallType_ctInnerDialOut;
                case ConCallType.呼出_外拨呼叫:
                    return axUniSoftPhone.ConCallType_ctManualDialOut;
                case ConCallType.呼出_自动外拨:
                    return axUniSoftPhone.ConCallType_ctAutoDialOut;
                case ConCallType.转IVR流程:
                    return axUniSoftPhone.ConCallType_ctTransToIVR;
                default:
                    return axUniSoftPhone.ConCallType_ctUnknow;
            }
        }
        public static ConCallType ConvertConCallType(int calltype)
        {
            if (calltype == axUniSoftPhone.ConCallType_ctUnknow)
            {
                return ConCallType.未定义;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctIVRTransIn)
            {
                return ConCallType.呼入_分配接入;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctInnerTransIn)
            {
                return ConCallType.呼入_内部转入;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctInnerDialIn)
            {
                return ConCallType.呼入_内部拨入;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctConsultIn)
            {
                return ConCallType.呼入_咨询接入;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctConfIn)
            {
                return ConCallType.呼入_会议接入;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctInnerDialOut)
            {
                return ConCallType.呼出_内部呼叫;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctManualDialOut)
            {
                return ConCallType.呼出_外拨呼叫;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctAutoDialOut)
            {
                return ConCallType.呼出_自动外拨;
            }
            else if (calltype == axUniSoftPhone.ConCallType_ctTransToIVR)
            {
                return ConCallType.转IVR流程;
            }
            else return ConCallType.未定义;
        }
        /// 通话类型
        /// <summary>
        /// 通话类型
        /// </summary>
        public enum ConCallType
        {
            未定义,
            呼入_分配接入,
            呼入_内部转入,
            呼入_内部拨入,
            呼入_咨询接入,
            呼入_会议接入,
            呼出_内部呼叫,
            呼出_外拨呼叫,
            呼出_自动外拨,
            转IVR流程
        }

        public static int ConvertConDeviceType(ConDeviceType devicetype)
        {
            switch (devicetype)
            {
                case ConDeviceType.未定义:
                    return axUniSoftPhone.ConDeviceType_MODE_NONE;
                case ConDeviceType.分机号:
                    return axUniSoftPhone.ConDeviceType_MODE_DN;
                case ConDeviceType.座席工号:
                    return axUniSoftPhone.ConDeviceType_MODE_AGENTID;
                case ConDeviceType.技能组:
                    return axUniSoftPhone.ConDeviceType_MODE_SKILL;
                case ConDeviceType.外线号码:
                    return axUniSoftPhone.ConDeviceType_MODE_PSTN;
                case ConDeviceType.IVR:
                    return axUniSoftPhone.ConDeviceType_MODE_IVR;
                case ConDeviceType.线路号:
                    return axUniSoftPhone.ConDeviceType_MODE_LINE;
                default:
                    return axUniSoftPhone.ConDeviceType_MODE_NONE;
            }
        }
        public static ConDeviceType ConvertConDeviceType(int devicetype)
        {
            if (devicetype == axUniSoftPhone.ConDeviceType_MODE_NONE)
            {
                return ConDeviceType.未定义;
            }
            else if (devicetype == axUniSoftPhone.ConDeviceType_MODE_DN)
            {
                return ConDeviceType.分机号;
            }
            else if (devicetype == axUniSoftPhone.ConDeviceType_MODE_AGENTID)
            {
                return ConDeviceType.座席工号;
            }
            else if (devicetype == axUniSoftPhone.ConDeviceType_MODE_SKILL)
            {
                return ConDeviceType.技能组;
            }
            else if (devicetype == axUniSoftPhone.ConDeviceType_MODE_PSTN)
            {
                return ConDeviceType.外线号码;
            }
            else if (devicetype == axUniSoftPhone.ConDeviceType_MODE_IVR)
            {
                return ConDeviceType.IVR;
            }
            else if (devicetype == axUniSoftPhone.ConDeviceType_MODE_LINE)
            {
                return ConDeviceType.线路号;
            }
            else return ConDeviceType.未定义;
        }
        /// 设备类型
        /// <summary>
        /// 设备类型
        /// </summary>
        public enum ConDeviceType
        {
            未定义,
            分机号,
            座席工号,
            技能组,
            外线号码,
            IVR,
            线路号
        }

        public static int ConvertConMonitorType(ConMonitorType monitortype)
        {
            switch (monitortype)
            {
                case ConMonitorType.非监控:
                    return axUniSoftPhone.ConMonitorType_etNotMonitor;
                case ConMonitorType.监听:
                    return axUniSoftPhone.ConMonitorType_etListenMonitor;
                case ConMonitorType.强插:
                    return axUniSoftPhone.ConMonitorType_etForceInsertMonitor;
                case ConMonitorType.拦截:
                    return axUniSoftPhone.ConMonitorType_etInterceptMonitor;
                default:
                    return axUniSoftPhone.ConMonitorType_etNotMonitor;
            }
        }
        public static ConMonitorType ConvertConMonitorType(int monitortype)
        {
            if (monitortype == axUniSoftPhone.ConMonitorType_etNotMonitor)
            {
                return ConMonitorType.非监控;
            }
            else if (monitortype == axUniSoftPhone.ConMonitorType_etListenMonitor)
            {
                return ConMonitorType.监听;
            }
            else if (monitortype == axUniSoftPhone.ConMonitorType_etForceInsertMonitor)
            {
                return ConMonitorType.强插;
            }
            else if (monitortype == axUniSoftPhone.ConMonitorType_etInterceptMonitor)
            {
                return ConMonitorType.拦截;
            }
            else
            {
                return ConMonitorType.非监控;
            }
        }
        /// 监控类型
        /// <summary>
        /// 监控类型
        /// </summary>
        public enum ConMonitorType
        {
            非监控,
            监听,
            强插,
            拦截
        }

        public static int ConvertConHangupFlag(ConHangupFlag hangupflag)
        {
            switch (hangupflag)
            {
                case ConHangupFlag.座席人工挂机:
                    return axUniSoftPhone.ConHangupFlag_hfAgentHangup;
                case ConHangupFlag.对方用户挂机:
                    return axUniSoftPhone.ConHangupFlag_hfCustomerHangup;
                case ConHangupFlag.转接坐席挂机:
                    return axUniSoftPhone.ConHangupFlag_hfTransAgentHangup;
                case ConHangupFlag.转接外线挂机:
                    return axUniSoftPhone.ConHangupFlag_hfTransOuterHangup;
                case ConHangupFlag.转接内线挂机:
                    return axUniSoftPhone.ConHangupFlag_hfTransInnerHangup;
                case ConHangupFlag.转IVR挂机:
                    return axUniSoftPhone.ConHangupFlag_hfTransIVRHangup;
                default:
                    return axUniSoftPhone.ConHangupFlag_hfAgentHangup;
            }
        }
        public static ConHangupFlag ConvertConHangupFlag(int hangupflag)
        {
            if (hangupflag == axUniSoftPhone.ConHangupFlag_hfAgentHangup)
            {
                return ConHangupFlag.座席人工挂机;
            }
            else if (hangupflag == axUniSoftPhone.ConHangupFlag_hfCustomerHangup)
            {
                return ConHangupFlag.对方用户挂机;
            }
            else if (hangupflag == axUniSoftPhone.ConHangupFlag_hfTransAgentHangup)
            {
                return ConHangupFlag.转接坐席挂机;
            }
            else if (hangupflag == axUniSoftPhone.ConHangupFlag_hfTransOuterHangup)
            {
                return ConHangupFlag.转接外线挂机;
            }
            else if (hangupflag == axUniSoftPhone.ConHangupFlag_hfTransInnerHangup)
            {
                return ConHangupFlag.转接内线挂机;
            }
            else if (hangupflag == axUniSoftPhone.ConHangupFlag_hfTransIVRHangup)
            {
                return ConHangupFlag.转IVR挂机;
            }
            else return ConHangupFlag.座席人工挂机;
        }
        /// 挂机类型
        /// <summary>
        /// 挂机类型
        /// </summary>
        public enum ConHangupFlag
        {
            座席人工挂机,
            对方用户挂机,
            转接坐席挂机,
            转接外线挂机,
            转接内线挂机,
            转IVR挂机
        }

        /// 转程序中的枚举
        /// <summary>
        /// 转程序中的枚举
        /// </summary>
        /// <param name="phonestatus"></param>
        /// <returns></returns>
        public static PhoneStatus ConvertPhoneStatus(int phonestatus)
        {
            if (phonestatus == axUniSoftPhone.ConStatus_stReady)
            {
                return PhoneStatus.PS01_就绪;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stLogout)
            {
                return PhoneStatus.PS02_签出;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stIdle)
            {
                return PhoneStatus.PS03_置闲;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stBusy)
            {
                return PhoneStatus.PS04_置忙;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stRest)
            {
                return PhoneStatus.PS05_休息;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stNeaten)
            {
                return PhoneStatus.PS06_话后;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stBelling)
            {
                return PhoneStatus.PS07_来电震铃;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stTalking)
            {
                return PhoneStatus.PS08_普通通话;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stConsult)
            {
                return PhoneStatus.PS09_咨询通话_发起方;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stCounselor)
            {
                return PhoneStatus.PS10_咨询方通话_接受者;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stConference)
            {
                return PhoneStatus.PS11_会议通话_发起方;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stConfMember)
            {
                return PhoneStatus.PS12_会议方通话_接受者;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stHold)
            {
                return PhoneStatus.PS13_保持;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stCallOutDialing)
            {
                return PhoneStatus.PS14_呼出拨号中;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stConsultDialing)
            {
                return PhoneStatus.PS15_咨询拨号中;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stConfDialing)
            {
                return PhoneStatus.PS16_会议拨号中;
            }
            else if (phonestatus == axUniSoftPhone.ConStatus_stTransfering)
            {
                return PhoneStatus.PS17_转接拨号中;
            }
            else return PhoneStatus.PS01_就绪;
        }
        /// 转数据库枚举
        /// <summary>
        /// 转数据库枚举
        /// </summary>
        /// <param name="phonestatus"></param>
        /// <returns></returns>
        public static AgentState ConvertAgentState(PhoneStatus phonestatus)
        {
            switch (phonestatus)
            {
                case PhoneStatus.PS02_签出:
                    return AgentState.AS1_签出;
                case PhoneStatus.PS03_置闲:
                    return AgentState.AS3_置闲;
                case PhoneStatus.PS04_置忙:
                case PhoneStatus.PS05_休息:
                    return AgentState.AS4_置忙;
                case PhoneStatus.PS06_话后:
                    return AgentState.AS5_话后;
                case PhoneStatus.PS07_来电震铃:
                    return AgentState.AS8_振铃;
                case PhoneStatus.PS08_普通通话:
                case PhoneStatus.PS09_咨询通话_发起方:
                case PhoneStatus.PS10_咨询方通话_接受者:
                case PhoneStatus.PS11_会议通话_发起方:
                case PhoneStatus.PS12_会议方通话_接受者:
                case PhoneStatus.PS13_保持:
                    return AgentState.AS9_通话中;
                default:
                    return AgentState.AS0_未知;
            }
        }
        #endregion
    }
}
