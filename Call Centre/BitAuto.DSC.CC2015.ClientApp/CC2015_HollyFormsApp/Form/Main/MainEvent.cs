using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;
using AxUniSoftPhoneControl;
using System.Drawing;
using CC2015_HollyFormsApp.Code.Logic;

namespace CC2015_HollyFormsApp
{
    public partial class Main : Form
    {
        /// 话务的callid
        /// <summary>
        /// 话务的callid
        /// </summary>
        private long NewCallID = -1;

        private void RegisterEvent()
        {
            HollyContactHelper.AxUniSoftPhone.OnStatusChange += new EventHandler(AxUniSoftPhone_OnStatusChange);
            HollyContactHelper.AxUniSoftPhone.OnBelling += new EventHandler(AxUniSoftPhone_OnBelling);
            HollyContactHelper.AxUniSoftPhone.OnHangup += new EventHandler(AxUniSoftPhone_OnHangup);
            HollyContactHelper.AxUniSoftPhone.OnAnswerSuccess += new EventHandler(AxUniSoftPhone_OnAnswerSuccess);
            HollyContactHelper.AxUniSoftPhone.OnCallOutSuccess += new EventHandler(AxUniSoftPhone_OnCallOutSuccess);
            HollyContactHelper.AxUniSoftPhone.OnMessage += new IUniSoftPhoneEvents_OnMessageEventHandler(AxUniSoftPhone_OnMessage);

            HollyContactHelper.AxUniSoftPhone.OnSignInSuccess += new EventHandler(AxUniSoftPhone_OnSignInSuccess);
            HollyContactHelper.AxUniSoftPhone.OnSignOutSuccess += new EventHandler(AxUniSoftPhone_OnSignOutSuccess);
        }

        #region 签入和签出事件
        /// 签入成功
        /// <summary>
        /// 签入成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnSignInSuccess(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[事件][OnSignInSuccess] 签入成功事件");
            LoginUser.isLoggedIn = true;
            //更新数据库
            LoginHelper.Instance.InsertAgentState();
            //置忙原因是：自动
            Main_BusyStatus = BusyStatus.BS0_自动;
        }
        /// 签出成功
        /// <summary>
        /// 签出成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnSignOutSuccess(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[事件][OnSignOutSuccess] 签出成功事件");
            LoginUser.isLoggedIn = false;
            string msg = "";
            //修改数据库
            LoginHelper.Instance.DeleteAgentStateToDB(ref msg);
            //置忙原因是：自动
            Main_BusyStatus = BusyStatus.BS0_自动;
        }
        #endregion

        #region 状态变化事件
        /// 状态变化事件
        /// <summary>
        /// 状态变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnStatusChange(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[事件][AxUniSoftPhone_OnStatusChange] 状态变化事件");

            PhoneStatus pre = HollyContactHelper.ConvertPhoneStatus(HollyContactHelper.Instance.GetPreStatus());
            PhoneStatus cur = HollyContactHelper.ConvertPhoneStatus(HollyContactHelper.Instance.GetCurStatus());

            //优先级从高到低
            //更新客服状态表
            UpdatAgentDBAsync(HollyContactHelper.ConvertAgentState(cur));
            //操作完成事件触发
            AfterActionEventManage.ActionEvent(pre, cur);
            //设置界面
            SetMainStatus(cur);
            //设置状态条
            ShowStatusTool(cur, HollyContactHelper.ConvertAgentState(cur));
        }
        private void UpdatAgentDBAsync(AgentState Cur_AgentState)
        {
            Action<AgentState> action = new Action<AgentState>(UpdatAgentDB);
            action.BeginInvoke(Cur_AgentState, null, null);
        }
        /// 更新客服状态表
        /// <summary>
        /// 更新客服状态表
        /// </summary>
        /// <param name="Cur_AgentState"></param>
        private void UpdatAgentDB(AgentState Cur_AgentState)
        {
            Loger.Log4Net.Info("[Main][UpdatAgentDB] 更新客服状态表" + Cur_AgentState);
            //签入和签出另有地方记录日志，此处不重复记录
            if (Cur_AgentState != AgentState.AS1_签出 && Cur_AgentState != AgentState.AS2_签入)
            {
                //当前时间
                DateTime date = Common.GetCurrentTime();
                Calltype ctype = Calltype.C0_未知;
                if (LoginHelper.PreOid == -1)
                {
                    //没有上一个数据，直接插入新数据
                    //1.插入CAgent表
                    AgentTimeStateHelper.Instance.InsertAgentState2DBAsync(Cur_AgentState, Main_BusyStatus, ctype, 0, date);
                    Loger.Log4Net.Info("[Main][UpdatAgentDB] 插入CAgent表");
                }
                else
                {
                    //有上一个数据，先更新上一个数据，再插入新数据，在重置PreOid
                    //1.更新CAgent表
                    AgentTimeStateHelper.Instance.UpdateAgentState2DBAsync(Cur_AgentState, Main_BusyStatus, ctype, 0, date);
                    //2.更新AgentStateDetail表（更新上一个状态的结束时间）
                    AgentTimeStateHelper.Instance.UpdateStateDetail2DBAsync(LoginHelper.PreOid, date);
                    Loger.Log4Net.Info("[Main][UpdatAgentDB] 更新上一个状态的结束时间 PreOid....IS:" + LoginHelper.PreOid + ",结束时间为：" + date);
                }
                //3.插入AgentStateDetail表（新状态）
                LoginHelper.PreOid = AgentTimeStateHelper.Instance.InsertAgentStateDetail2DB(Cur_AgentState, Main_BusyStatus, ctype, date, date);
                Loger.Log4Net.Info("[Main][UpdatAgentDB] 插入新的状态 PreOid....IS:" + LoginHelper.PreOid);
            }
            else if (Cur_AgentState == AgentState.AS1_签出)
            {
                //当前时间
                DateTime date = Common.GetCurrentTime();
                LoginHelper.Instance.SingOutUpdateDate(date);
            }
        }
        /// 设置状态条
        /// <summary>
        /// 设置状态条
        /// </summary>
        /// <param name="cur"></param>
        private void ShowStatusTool(PhoneStatus cur, AgentState agentState)
        {
            //主叫被叫还原
            SetZhujiaoDn("");
            SetBeijiaoDn("");
            //计时通话时长
            if (agentState == AgentState.AS9_通话中 && GetTimeForCall() == false)
            {
                StartTimeForCall();
            }
            //结束计时
            if (agentState == AgentState.AS5_话后 && GetTimeForCall())
            {
                EndTimeForCall();
            }

            if (agentState == AgentState.AS9_通话中 || agentState == AgentState.AS8_振铃 || agentState == AgentState.AS5_话后)
            {
                //拨号时，其实是在呼叫分机，所以实际的主被叫不对
                if (cur == PhoneStatus.PS14_呼出拨号中)
                {
                    SetZhujiaoDn(LoginUser.ExtensionNum);
                    //处理号码
                    SetBeijiaoDn(HollyContactHelper.Instance.HaoMaProcess(OutCallNumber));
                }
                else
                {
                    //通话中，设置主叫和被叫
                    SetZhujiaoDn(HollyContactHelper.Instance.GetZhujiaoPhone());
                    SetBeijiaoDn(HollyContactHelper.Instance.GetBeijiaoPhone());
                }
            }
            else
            {
                RestTimeForCall();
            }

            //设置其他文字
            SetCTIStatus(cur.ToString().Substring(5));
            SetLuodiNum(HollyContactHelper.Instance.GetLuodiNum(), HollyContactHelper.Instance.GetSkillGroupName());
        }
        #endregion

        #region 厂家电话动作事件
        /// 监控状态校验
        /// <summary>
        /// 监控状态校验
        /// </summary>
        /// <param name="conmonitortype"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckMonitorIsError(HollyContactHelper.ConMonitorType conmonitortype, string name)
        {
            Main_ConMonitor = conmonitortype;
            if (conmonitortype != HollyContactHelper.ConMonitorType.非监控 && conmonitortype != HollyContactHelper.ConMonitorType.拦截)
            {
                Loger.Log4Net.Info("[事件][" + name + "] 监控中，无业务逻辑");
                //无法通过校验
                return true;
            }
            //通过校验
            return false;
        }
        /// 其他话务事件
        /// <summary>
        /// 其他话务事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnMessage(object sender, IUniSoftPhoneEvents_OnMessageEvent e)
        {
            //客服锁定事件
            if (Main_PhoneStatus == PhoneStatus.PS03_置闲 && e.messageContent.Trim() == "座席被锁定")
            {
                SetMainStatus(PhoneStatus.PS18_客服被锁定);
            }
            else if (Main_PhoneStatus == PhoneStatus.PS18_客服被锁定 && e.messageContent.Trim().Contains("话路被挂断"))
            {
                SetMainStatus(PhoneStatus.PS03_置闲);
            }
            //监听通话事件
            if (e.messageContent.Contains("监听开始") && Main_PhoneStatus == PhoneStatus.PS19_监听振铃)
            {
                SetMainStatus(PhoneStatus.PS20_监听中);
            }
            //强插通话事件
            if (e.messageContent.Contains("强插开始") && Main_PhoneStatus == PhoneStatus.PS21_强插振铃)
            {
                SetMainStatus(PhoneStatus.PS22_强插中);
            }
        }

        /// 呼入振铃事件
        /// <summary>
        /// 呼入振铃事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnBelling(object sender, EventArgs e)
        {
            HollyContactHelper.ConCallType concalltype = HollyContactHelper.Instance.GetSourceType();
            HollyContactHelper.ConMonitorType conmonitortype = HollyContactHelper.Instance.GetMonitorType();
            Loger.Log4Net.Info("[事件][OnBelling] 呼入振铃事件 ：" + concalltype + " 监控：" + conmonitortype);

            if (CheckMonitorIsError(conmonitortype, "OnBelling"))
            {
                return;
            }

            bool has_set_title = false;
            switch (concalltype)
            {
                case HollyContactHelper.ConCallType.未定义:
                    break;
                case HollyContactHelper.ConCallType.呼入_分配接入:
                    CustomerInCome_OnBelling();
                    has_set_title = true;
                    break;
                case HollyContactHelper.ConCallType.呼入_内部转入:
                    //转接技能组
                    ConsultInCome_OnBelling("转接");
                    has_set_title = true;
                    break;
                case HollyContactHelper.ConCallType.呼入_内部拨入:
                    break;
                case HollyContactHelper.ConCallType.呼入_咨询接入:
                    //咨询
                    ConsultInCome_OnBelling("咨询");
                    has_set_title = true;
                    break;
                case HollyContactHelper.ConCallType.呼入_会议接入:
                    break;
                case HollyContactHelper.ConCallType.呼出_内部呼叫:
                    break;
                case HollyContactHelper.ConCallType.呼出_外拨呼叫:
                    break;
                case HollyContactHelper.ConCallType.呼出_自动外拨:
                    break;
                case HollyContactHelper.ConCallType.转IVR流程:
                    break;
                default:
                    break;
            }
            if (!has_set_title)
            {
                SetlblAgentStatusName("来电振铃");
            }
        }
        /// 呼入应答成功
        /// <summary>
        /// 呼入应答成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnAnswerSuccess(object sender, EventArgs e)
        {
            HollyContactHelper.ConCallType concalltype = HollyContactHelper.Instance.GetSourceType();
            HollyContactHelper.ConMonitorType conmonitortype = HollyContactHelper.Instance.GetMonitorType();
            Loger.Log4Net.Info("[事件][OnAnswerSuccess] 呼入接通成功事件 ：" + concalltype + " 监控：" + conmonitortype);

            if (CheckMonitorIsError(conmonitortype, "OnAnswerSuccess"))
            {
                return;
            }

            switch (concalltype)
            {
                case HollyContactHelper.ConCallType.未定义:
                    break;
                case HollyContactHelper.ConCallType.呼入_分配接入:
                    //普通通话
                    CustomerInCome_OnAnswer();
                    break;
                case HollyContactHelper.ConCallType.呼入_内部转入:
                    //转接技能组
                    CompelInCome_OnAnswer();
                    break;
                case HollyContactHelper.ConCallType.呼入_内部拨入:
                    break;
                case HollyContactHelper.ConCallType.呼入_咨询接入:
                    //咨询
                    ConsultInCome_OnAnswer();
                    break;
                case HollyContactHelper.ConCallType.呼入_会议接入:
                    break;
                case HollyContactHelper.ConCallType.呼出_内部呼叫:
                    break;
                case HollyContactHelper.ConCallType.呼出_外拨呼叫:
                    break;
                case HollyContactHelper.ConCallType.呼出_自动外拨:
                    break;
                case HollyContactHelper.ConCallType.转IVR流程:
                    break;
                default:
                    break;
            }
        }

        /// 外拨初始化事件-自制
        /// <summary>
        /// 外拨初始化事件-自制
        /// </summary>
        private void AxUniSoftPhone_OnCallOutInitiated()
        {
            Loger.Log4Net.Info("[事件][OnCallOutInitiated][自制] 外呼初始化事件");
            CustomerOutCome_OnInit();
            CustomerOutCome_OnBelling();
        }
        /// 外呼成功
        /// <summary>
        /// 外呼成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnCallOutSuccess(object sender, EventArgs e)
        {
            HollyContactHelper.ConCallType concalltype = HollyContactHelper.Instance.GetSourceType();
            HollyContactHelper.ConMonitorType conmonitortype = HollyContactHelper.Instance.GetMonitorType();
            Loger.Log4Net.Info("[事件][CallOutSuccess] 外呼接起成功事件 ：" + concalltype + " 监控：" + conmonitortype);

            if (CheckMonitorIsError(conmonitortype, "CallOutSuccess"))
            {
                return;
            }

            switch (concalltype)
            {
                case HollyContactHelper.ConCallType.未定义:
                    break;
                case HollyContactHelper.ConCallType.呼入_分配接入:
                    break;
                case HollyContactHelper.ConCallType.呼入_内部转入:
                    break;
                case HollyContactHelper.ConCallType.呼入_内部拨入:
                    break;
                case HollyContactHelper.ConCallType.呼入_咨询接入:
                    break;
                case HollyContactHelper.ConCallType.呼入_会议接入:
                    break;
                case HollyContactHelper.ConCallType.呼出_内部呼叫:
                    break;
                case HollyContactHelper.ConCallType.呼出_外拨呼叫:
                    CustomerOutCome_OnAnswer();
                    break;
                case HollyContactHelper.ConCallType.呼出_自动外拨:
                    break;
                case HollyContactHelper.ConCallType.转IVR流程:
                    break;
                default:
                    break;
            }
        }

        /// 挂断事件
        /// <summary>
        /// 挂断事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnHangup(object sender, EventArgs e)
        {
            CurrentCallInfo.CallIDForHangupEmpty = -1;
            CurrentCallInfo.CallNumForHangupEmpty = string.Empty;
            HollyContactHelper.ConHangupFlag conhangupflag = HollyContactHelper.Instance.GetHangupFlag();
            HollyContactHelper.ConMonitorType conmonitortype = HollyContactHelper.Instance.GetMonitorType();
            HollyContactHelper.ConCallType concalltype = HollyContactHelper.Instance.GetSourceType();
            Loger.Log4Net.Info("[事件][OnHangup] 挂断事件 ：" + conhangupflag + "-" + concalltype + " 监控：" + conmonitortype);

            if (CheckMonitorIsError(conmonitortype, "OnHangup"))
            {
                return;
            }

            switch (conhangupflag)
            {
                case HollyContactHelper.ConHangupFlag.客服人工挂机:
                    OnHangup(ReleaseType.客服挂断);
                    break;
                case HollyContactHelper.ConHangupFlag.对方用户挂机:
                    OnHangup(ReleaseType.用户挂断);
                    break;
                case HollyContactHelper.ConHangupFlag.转接客服挂机:
                case HollyContactHelper.ConHangupFlag.转接外线挂机:
                case HollyContactHelper.ConHangupFlag.转接内线挂机:
                case HollyContactHelper.ConHangupFlag.转IVR挂机:
                    OnHangup(ReleaseType.客服挂断);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 具体实现（数据入库）
        /// 获取呼入类型
        /// <summary>
        /// 获取呼入类型
        /// </summary>
        /// <returns></returns>
        private CallInType GetCallInType()
        {
            if (HollyContactHelper.Instance.IsAutoCall_HasLog)
            {
                Loger.Log4Net.Info("[事件][CustomerInCome_OnBelling] 判断呼入类型：自动呼入");
                return CallInType.自动呼入;
            }
            else
            {
                if (Main_ConMonitor == HollyContactHelper.ConMonitorType.非监控)
                {
                    Loger.Log4Net.Info("[事件][CustomerInCome_OnBelling] 判断呼入类型：普通呼入");
                    return CallInType.普通呼入;
                }
                else
                {
                    Loger.Log4Net.Info("[事件][CustomerInCome_OnBelling] 判断呼入类型：监控呼入");
                    return CallInType.监控呼入;
                }
            }
        }

        /// 用户进线
        /// <summary>
        /// 用户进线
        /// </summary>
        private void CustomerInCome_OnBelling()
        {
            Loger.Log4Net.Info("[事件][CustomerInCome_OnBelling] 客服振铃");
            DateTime date = Common.GetCurrentTime();
            //新的话务ID
            NewCallID = Common.GetNewCallID(date);
            CurrentCallInfo.CallIDForHangupEmpty = CurrentCallInfo.CallID = NewCallID;
            CurrentCallInfo.CallNumForHangupEmpty = CurrentCallInfo.CallNum = HollyContactHelper.Instance.GetZhujiaoPhone();
            LabelColor = Color.Black;

            //构造数据和发送数据
            CallInType callintype = GetCallInType();
            // 普通呼入
            if (callintype == CallInType.普通呼入)
            {
                //呼入
                //初始化话务数据
                BusinessProcess.Instance.InitCallRecordORIG(NewCallID, Calltype.C1_呼入);
                BusinessProcess.Instance.SetInitiatedTime(date);
                BusinessProcess.Instance.SetRingingTime(date);

                //发送消息到客户端
                UserEventData ud = BusinessProcess.Instance.InitUserEventData(UserEvent.Transferred, Calltype.C1_呼入, date, NewCallID);
                SendToWeb(ud.ToString());
                //3秒自动摘机
                AfterTime = 3;
                LabelTitle = "客户来电";

                //判断是否专属客户&坐席
                string agentid = "";
                if (HollyContactHelper.Instance.IsSpecialAgentCall(out agentid))
                {
                    SendLogToServer(date, agentid);
                    LabelColor = Color.Orange;
                }
            }
            else if (callintype == CallInType.自动呼入)
            {
                //自动外呼
                BusinessProcess.OutBoundType = OutBoundTypeEnum.OT4_自动外呼;
                //初始化话务数据
                BusinessProcess.Instance.InitCallRecordORIG(NewCallID, Calltype.C2_呼出);
                BusinessProcess.Instance.SetInitiatedTime(date);
                BusinessProcess.Instance.SetRingingTime(date);

                //呼入
                string BusinessID = HollyContactHelper.Instance.GetZDBusinessID();
                if (!string.IsNullOrEmpty(BusinessID))
                {
                    //发送消息到客户端
                    UserEventData ud = BusinessProcess.Instance.InitUserEventData(UserEvent.AutoCall, Calltype.C1_呼入, date, NewCallID, BusinessID, 1);
                    //提前预设接通时间
                    ud.IsEstablished = true;
                    ud.EstablishedStartTime = date;
                    SendToWeb(ud.ToString());
                }
                else
                {
                    MessageBox.Show("获取当前通话的任务ID失败，无法打开任务处理页面！");
                    //当前没有通话页面，无法发送页面事件
                    CurrentOutBoundTabPageName = "";
                    Loger.Log4Net.Info("[事件][CustomerInCome_OnBelling] 获取当前通话的任务ID失败，无法打开任务处理页面！");
                }

                //1秒自动摘机
                AfterTime = 1;
                LabelTitle = "外呼来电";
            }
            else if (callintype == CallInType.监控呼入)
            {
                //初始化话务数据
                BusinessProcess.Instance.InitCallRecordORIG(NewCallID, Calltype.C4_拦截);
                BusinessProcess.Instance.SetInitiatedTime(date);
                BusinessProcess.Instance.SetRingingTime(date);

                if (Main_ConMonitor_Calltype == Calltype.C1_呼入)
                {
                    //发送消息到客户端
                    UserEventData ud = BusinessProcess.Instance.InitUserEventData(UserEvent.Transferred, Calltype.C1_呼入, date, NewCallID);
                    SendToWeb(ud.ToString());
                }
                //自动摘机
                AfterTime = 1;
                LabelTitle = "接管来电";
            }

            //清空
            NewCallID = -1;
            //自动摘机
            SetlblAgentStatusName(LabelTitle + " " + AfterTime, LabelColor);
            timer_zhaiji.Enabled = true;
        }

        /// 呼入客服接通成功
        /// <summary>
        /// 呼入客服接通成功
        /// </summary>
        private void CustomerInCome_OnAnswer()
        {
            Loger.Log4Net.Info("[事件][CustomerInCome_OnAnswer] 客服接通");
            if (BusinessProcess.CallRecordORIG != null)
            {
                //初始化话务数据
                DateTime date = Common.GetCurrentTime();
                BusinessProcess.Instance.SetEstablishedTime(date);

                //自动外呼不发接通成功消息
                if (HollyContactHelper.Instance.IsAutoCall == false)
                {
                    //发送消息到客户端
                    UserEventData ud = BusinessProcess.Instance.InitUserEventData(UserEvent.Established, Calltype.C1_呼入, date);
                    SendToWeb(ud.ToString());
                }
            }
            else
            {
                Loger.Log4Net.Info("[事件][CustomerInCome_OnAnswer] 客服接通异常：BusinessProcess.CallRecordORIG 为空");
            }
        }

        /// 外呼初始化
        /// <summary>
        /// 外呼初始化
        /// </summary>
        private void CustomerOutCome_OnInit()
        {
            Loger.Log4Net.Info("[事件][CustomerOutCome_OnInit] 外呼建立");
            //初始化话务数据
            DateTime date = Common.GetCurrentTime();
            CurrentCallInfo.CallIDForHangupEmpty = CurrentCallInfo.CallID = Common.GetNewCallID(date);
            BusinessProcess.Instance.InitCallRecordORIG(CurrentCallInfo.CallID, Calltype.C2_呼出);
            BusinessProcess.Instance.SetInitiatedTime(date);

            if (BusinessProcess.OutBoundType != OutBoundTypeEnum.OT2_客户端呼出)
            {
                //发送消息到客户端
                UserEventData ud = BusinessProcess.Instance.InitUserEventData(UserEvent.Initiated, Calltype.C2_呼出, date);
                SendToWeb(ud.ToString());
            }
        }
        /// 外呼振铃
        /// <summary>
        /// 外呼振铃
        /// </summary>
        private void CustomerOutCome_OnBelling()
        {
            Loger.Log4Net.Info("[事件][CustomerOutCome_OnBelling] 外呼振铃");
            //初始化话务数据
            DateTime date = Common.GetCurrentTime();
            BusinessProcess.Instance.SetRingingTime(date);

            if (BusinessProcess.OutBoundType != OutBoundTypeEnum.OT2_客户端呼出)
            {
                //发送消息到客户端
                UserEventData ud = BusinessProcess.Instance.InitUserEventData(UserEvent.NetworkReached, Calltype.C2_呼出, date);
                SendToWeb(ud.ToString());
            }
        }
        /// 外呼应答
        /// <summary>
        /// 外呼应答
        /// </summary>
        private void CustomerOutCome_OnAnswer()
        {
            //合力bug：外呼假失败
            HollyBUGForCallOut();

            Loger.Log4Net.Info("[事件][CustomerOutCome_OnAnswer] 外呼应答");
            if (BusinessProcess.CallRecordORIG != null)
            {
                //初始化话务数据
                DateTime date = Common.GetCurrentTime();
                BusinessProcess.Instance.SetEstablishedTime(date);

                if (BusinessProcess.OutBoundType != OutBoundTypeEnum.OT2_客户端呼出)
                {
                    //发送消息到客户端
                    UserEventData ud = BusinessProcess.Instance.InitUserEventData(UserEvent.Established, Calltype.C2_呼出, date);
                    SendToWeb(ud.ToString());
                }
            }
            else
            {
                Loger.Log4Net.Info("[事件][CustomerOutCome_OnAnswer] 外呼应答异常：BusinessProcess.CallRecordORIG 为空");
            }
        }

        /// 合力bug：外呼假失败
        /// <summary>
        /// 合力bug：外呼假失败
        /// </summary>
        private void HollyBUGForCallOut()
        {
            if (ActCallOutResult == false)
            {
                //合力bug：假失败
                //需要重新出发初始化和振铃事件
                Loger.Log4Net.Info("[事件][CustomerOutCome_OnAnswer] 合力BUG：假失败；需要重新出发初始化和振铃事件--");
                OutCall();
                Loger.Log4Net.Info("[事件][CustomerOutCome_OnAnswer] 合力BUG：假失败；完成重新出发初始化和振铃事件--");
                ActCallOutResult = true;
            }
        }

        #region 接受方
        /// 咨询振铃&转接振铃事件
        /// <summary>
        /// 咨询振铃&转接振铃事件
        /// </summary>
        private void ConsultInCome_OnBelling(string type)
        {
            Loger.Log4Net.Info("[事件][ConsultInCome_OnBelling] 咨询振铃");

            //初始化话务数据
            DateTime date = Common.GetCurrentTime();
            BusinessProcess.Instance.InitCallRecordORIG(Common.GetNewCallID(date), Calltype.C3_转接);
            BusinessProcess.Instance.SetInitiatedTime(date);
            BusinessProcess.Instance.SetRingingTime(date);

            //发送消息到web端
            //发送消息到客户端
            UserEventData ud1 = BusinessProcess.Instance.InitUserEventData(UserEvent.Transferred, Calltype.C1_呼入, date);
            SendToWeb(ud1.ToString());

            //3秒自动摘机
            AfterTime = 3;
            LabelTitle = type + "来电";
            SetlblAgentStatusName(LabelTitle + " " + AfterTime);
            timer_zhaiji.Enabled = true;
        }
        /// 咨询应答成功事件
        /// <summary>
        /// 咨询应答成功事件
        /// </summary>
        private void ConsultInCome_OnAnswer()
        {
            Loger.Log4Net.Info("[事件][ConsultInCome_OnAnswer] 咨询接通");

            //初始化话务数据
            DateTime date = Common.GetCurrentTime();
            BusinessProcess.Instance.SetEstablishedTime(date);

            //发送消息到客户端
            UserEventData ud2 = BusinessProcess.Instance.InitUserEventData(UserEvent.Established, Calltype.C1_呼入, date);
            SendToWeb(ud2.ToString());

            Action<object> callback = new Action<object>(x =>
            {
                //和用户接通了
                DateTime date2 = Common.GetCurrentTime();
                //设置和用户通话时间（转入时间）
                BusinessProcess.Instance.SetTransferInTime(date2);
            });

            AgentToCustomerCallBack(callback);
        }
        /// 强转应答成功事件
        /// <summary>
        /// 强转应答成功事件
        /// </summary>
        private void CompelInCome_OnAnswer()
        {
            Loger.Log4Net.Info("[事件][CompelInCome_OnAnswer] 强转接通");

            //初始化话务数据
            DateTime date = Common.GetCurrentTime();
            BusinessProcess.Instance.SetEstablishedTime(date);
            //设置和用户通话时间（转入时间）
            BusinessProcess.Instance.SetTransferInTime(date);

            //发送消息到客户端
            UserEventData ud2 = BusinessProcess.Instance.InitUserEventData(UserEvent.Established, Calltype.C1_呼入, date);
            SendToWeb(ud2.ToString());
        }
        #endregion

        #region 发起方
        /// 咨询开始事件
        /// <summary>
        /// 咨询开始事件
        /// </summary>
        private void OnBeginConsult()
        {
            Loger.Log4Net.Info("[事件][OnBeginConsult] 咨询开始");
            Action<object> callback = new Action<object>(x =>
            {
                //初始化话务数据
                DateTime date = Common.GetCurrentTime();
                BusinessProcess.Instance.SetConsultTime(date);
            });
            BeginConsultCallBack(callback);
        }
        /// 咨询结束事件
        /// <summary>
        /// 咨询结束事件
        /// </summary>
        private void OnEndConsult()
        {
            Loger.Log4Net.Info("[事件][OnEndConsult] 咨询结束");
            Action<object> callback = new Action<object>(x =>
            {
                //初始化话务数据
                DateTime date = Common.GetCurrentTime();
                BusinessProcess.Instance.SetReconnectCall(date);
            });
            EndConsultCallBack(callback);
        }
        /// 咨询和技能转接完成
        /// <summary>
        /// 咨询和技能转接完成
        /// </summary>
        private void OnEndTransfer()
        {
            DateTime date = Common.GetCurrentTime();
            BusinessProcess.Instance.SetTransferOutTime(date);
        }
        #endregion

        /// 挂断通用事件
        /// <summary>
        /// 挂断通用事件
        /// </summary>
        /// <param name="releasetype"></param>
        private void OnHangup(ReleaseType releasetype)
        {
            Loger.Log4Net.Info("[事件][OnHangup] 挂断");
            //3种情况=呼入，呼出，未知
            Calltype ctype = HollyContactHelper.Instance.GetCallDir();
            //话后处理
            if (ctype == Calltype.C2_呼出)
            {
                //合力bug：外呼假失败
                HollyBUGForCallOut();
                //呼出没有话后
                Action<object> callback = new Action<object>(x =>
                {
                    //结束话后
                    HollyContactHelper.Instance.AfterCallEnd();
                    //呼出时计算话后信息
                    Loger.Log4Net.Info("[==BusinessProcess=][UpdateCallRecordAfterTime] 呼出时退出话后更新话后信息");
                    BusinessProcess.Instance.UpdateCallRecordAfterTime();
                });
                ToAfterCallCallBack(callback);
            }
            else
            {
                //呼入+自动外呼 存在话后时间
                //话后有时间限制
                AfterTime = CommonFunction.ObjectToInteger(ConfigurationUtil.GetAppSettingValue("AfterTime"), 60);
                SetlblAgentStatusName("话后 " + AfterTime);
                timer_after.Enabled = true;
            }

            if (BusinessProcess.CallRecordORIG != null)
            {
                //初始化话务数据
                DateTime date = Common.GetCurrentTime();
                //设置挂断时间
                BusinessProcess.Instance.SetReleaseTime(date, releasetype);
                //数据入库
                CallRecordHelper.Instance.InsertCallRecordNew(false);

                if (ctype != Calltype.C2_呼出)
                {
                    //呼入+自动外呼时，计算话后信息
                    //注册退出话后的事件
                    Action<object> callback = new Action<object>(x =>
                    {
                        Loger.Log4Net.Info("[==BusinessProcess=][UpdateCallRecordAfterTime] 呼入时退出话后更新话后信息");
                        BusinessProcess.Instance.UpdateCallRecordAfterTime();
                    });
                    //退出话后时触发
                    ExitAfterCallCallBack(callback);
                }

                //发送消息到客户端
                if (BusinessProcess.OutBoundType != OutBoundTypeEnum.OT2_客户端呼出)
                {
                    UserEventData ud = null;
                    //呼入（普通呼入，转接，拦截）
                    if (ctype == Calltype.C1_呼入 && HollyContactHelper.Instance.IsAutoCall == false)
                    {
                        ud = BusinessProcess.Instance.InitUserEventData(UserEvent.Released, Calltype.C1_呼入, date);
                    }
                    //呼出（呼出，自动外呼，未知）
                    else
                    {
                        ud = BusinessProcess.Instance.InitUserEventData(UserEvent.Released, Calltype.C2_呼出, date);
                    }
                    SendToWeb(ud.ToString());
                }
                //挂断是，数据还原初始值
                BusinessProcess.OutBoundType = OutBoundTypeEnum.OT0_未知;
            }
            else
            {
                Loger.Log4Net.Info("[事件][OnHangup] 异常：BusinessProcess.CallRecordORIG 为空");
            }
        }

        /// 发送日志到服务器端
        /// <summary>
        /// 发送日志到服务器端
        /// </summary>
        /// <param name="date"></param>
        /// <param name="agentid"></param>
        private void SendLogToServer(DateTime date, string agentid)
        {
            //服务器端写日志
            //地址：\log\Inbound_ExclusiveAgent\yyyy-MM-dd.log
            //内容：呼入来电时间：{0}，来电主叫：{1}，来电被叫：{2}，接听坐席工号：{3}，专属坐席工号：{4}，CallID：{5}，合力联络ID：{6},落地号：{7}
            string logmsg = "呼入来电时间：{0}，来电主叫：{1}，来电被叫：{2}，接听坐席工号：{3}，专属坐席工号：{4}，CallID：{5}，合力联络ID：{6},落地号：{7}";
            logmsg = string.Format(logmsg,
                date,
                BusinessProcess.CallRecordORIG.PhoneNum,
                BusinessProcess.CallRecordORIG.ANI,
                LoginUser.AgentNum,
               agentid,
               BusinessProcess.CallRecordORIG.CallID,
               BusinessProcess.CallRecordORIG.SessionID,
               BusinessProcess.CallRecordORIG.SwitchINNum);
            string path = "log\\Inbound_ExclusiveAgent";
            Loger.Log4Net.Info("[事件][SendLogToServer] " + logmsg);
            AgentTimeStateHelper.Instance.SendLogToServerAsync(path, logmsg);
        }
        #endregion

        #region 状态改变时触发事件-注册事件，在状态变化方法里监听
        /// 退出话后时执行
        /// <summary>
        /// 退出话后时执行
        /// </summary>
        /// <param name="callback"></param>
        private void ExitAfterCallCallBack(Action<object> callback)
        {
            //话后状态到其他状态时执行
            AfterActionEventManage.RegisterOnceAfterActionEvent2(PhoneStatus.PS06_话后, null, callback, null, "退出话后时执行-更新话后信息");
        }
        /// 置闲完成后触发事件
        /// <summary>
        /// 置闲完成后触发事件
        /// </summary>
        /// <param name="callback"></param>
        private void AfterToReadyCallBack(Action<object> callback, string name)
        {
            //当前状态
            PhoneStatus cur = HollyContactHelper.ConvertPhoneStatus(HollyContactHelper.Instance.GetCurStatus());
            //判断是异步触发还是同步触发
            if (cur != PhoneStatus.PS03_置闲)
            {
                HollyContactHelper.Instance.ToReady();
                //注册事件，当状态有cur变成【PS03_置闲】时触发                    
                AfterActionEventManage.RegisterOnceAfterActionEvent1(cur, PhoneStatus.PS03_置闲, callback, null, "置闲完成后触发事件-" + name);
            }
            else
            {
                callback(null);
            }
        }
        /// 开始转接成功后触发事件
        /// <summary>
        /// 开始转接成功后触发事件
        /// </summary>
        /// <param name="callback"></param>
        private void BeginConsultCallBack(Action<object> callback)
        {
            //从任意状态->咨询发起状态
            AfterActionEventManage.RegisterOnceAfterActionEvent1(null, PhoneStatus.PS09_咨询通话_发起方, callback, null, "开始转接成功后触发");
        }
        /// 取消转接成功后触发事件
        /// <summary>
        /// 取消转接成功后触发事件
        /// </summary>
        /// <param name="callback"></param>
        private void EndConsultCallBack(Action<object> callback)
        {
            //从咨询发起状态->普通通话状态
            AfterActionEventManage.RegisterOnceAfterActionEvent1(PhoneStatus.PS09_咨询通话_发起方, PhoneStatus.PS08_普通通话, callback, null, "取消转接成功后触发");
        }
        /// 咨询接受时，客服和用户联通时触发
        /// <summary>
        /// 咨询接受时，客服和用户联通时触发
        /// </summary>
        /// <param name="callback"></param>
        private void AgentToCustomerCallBack(Action<object> callback)
        {
            //由PS10_咨询方通话_接受者变成【PS08_普通通话】【PS12_会议方通话_接受者】任意一种情况时，触发
            AfterActionEventManage.RegisterOnceAfterActionEvent2(PhoneStatus.PS10_咨询方通话_接受者,
                new List<PhoneStatus>() { PhoneStatus.PS08_普通通话, PhoneStatus.PS12_会议方通话_接受者 }, callback, null, "咨询接受时，客服和用户联通时触发");
        }
        /// 到达话后时触发
        /// <summary>
        /// 到达话后时触发
        /// </summary>
        /// <param name="callback"></param>
        private void ToAfterCallCallBack(Action<object> callback)
        {
            //从任意状态->话后
            AfterActionEventManage.RegisterOnceAfterActionEvent1(null, PhoneStatus.PS06_话后, callback, null, "到达话后时触发-呼出结束话后");
        }
        #endregion
    }
}
