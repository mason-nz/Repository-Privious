using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HollyContact5_Demo
{

    public partial class VoiceSample : Form
    {
        private const string DllName = "SoftPhoneForHollyContact5.dll";
        private HollyContactHelper hollycontacthelper = null;
        private int loadConfigCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["LoadConfigCount"]);
        private int currentLoadConfigCount = 0;//当前调用load函数后，返回服务器消息数量
        /// <summary>
        /// 标识是否为跨号段监听，默认为否
        /// </summary>
        private bool isOverPrefixListen = false;

        public VoiceSample()
        {
            InitializeComponent();

        }

        private void VoiceSample_Load(object sender, EventArgs e)
        {
            HollyContactHelper.AxUniSoftPhone=axUniSoftPhone1;
            hollycontacthelper = HollyContactHelper.Instance;

            comboBox_ConDeviceType.Items.Clear();
            comboBox_ConDeviceType.Items.Add(HollyContactHelper.ConDeviceType.未定义);
            comboBox_ConDeviceType.Items.Add(HollyContactHelper.ConDeviceType.IVR);
            comboBox_ConDeviceType.Items.Add(HollyContactHelper.ConDeviceType.分机号);
            comboBox_ConDeviceType.Items.Add(HollyContactHelper.ConDeviceType.技能组);
            comboBox_ConDeviceType.Items.Add(HollyContactHelper.ConDeviceType.外线号码);
            comboBox_ConDeviceType.Items.Add(HollyContactHelper.ConDeviceType.线路号);
            comboBox_ConDeviceType.Items.Add(HollyContactHelper.ConDeviceType.座席工号);

            comboBox_ConDeviceType.SelectedItem = HollyContactHelper.ConDeviceType.分机号;

            cb_rest.Items.Clear();
            cb_rest.Items.Add(BusyStatus.BS0_自动);
            cb_rest.Items.Add(BusyStatus.BS1_小休);
            cb_rest.Items.Add(BusyStatus.BS2_任务回访);
            cb_rest.Items.Add(BusyStatus.BS3_业务处理);
            cb_rest.Items.Add(BusyStatus.BS4_会议);
            cb_rest.Items.Add(BusyStatus.BS5_培训);
            cb_rest.Items.Add(BusyStatus.BS6_离席);
            cb_rest.SelectedItem = BusyStatus.BS0_自动;

            comboBox_CTIDataKey.Items.Clear();
            List<KeyValuePair<string, string>> dit = new List<KeyValuePair<string, string>>();
            dit.Add(new KeyValuePair<string, string>("Key_IContact_ANI（正常呼入—主叫号码）", "Key_IContact_ANI"));
            dit.Add(new KeyValuePair<string, string>("Key_IContact_sysDNIS（正常呼入—被叫号码）", "Key_IContact_sysDNIS"));
            dit.Add(new KeyValuePair<string, string>("Skill_ID（正常呼入—技能组ID）", "Skill_ID"));
            dit.Add(new KeyValuePair<string, string>("varAgentIDz（正常呼入—专属坐席ID）", "varAgentIDz"));
            dit.Add(new KeyValuePair<string, string>("Key_IContact_DNIS（正常呼出—被叫号码）", "Key_IContact_DNIS"));
            dit.Add(new KeyValuePair<string, string>("transANI（跨号段转接—主叫号码）", "transANI"));
            dit.Add(new KeyValuePair<string, string>("sysAni（自动外呼—主叫号码）", "sysAni"));
            dit.Add(new KeyValuePair<string, string>("mphone（自动外呼—被叫号码）", "mphone"));
            dit.Add(new KeyValuePair<string, string>("skillId（自动外呼—技能组ID）", "skillId"));
            dit.Add(new KeyValuePair<string, string>("Id（自动外呼—业务ID）", "Id"));
            comboBox_CTIDataKey.DataSource = dit;
            comboBox_CTIDataKey.DisplayMember = "Key";
            comboBox_CTIDataKey.ValueMember = "Value";
        }

        #region 日志
        private void LogMessage(string message)
        {
            txtLog.Text = txtLog.Text + "\r\n>> INCOMING \r\n" + message.Replace("\n", "\r\n") + "\r\n" +
                "*********" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "***************************\r\n\r\n";
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.SelectionLength = 0;
            txtLog.ScrollToCaret();
        }
        private void LogRequest(String request)
        {
            txtLog.Text = txtLog.Text + "\r\n<< OUTGOING \r\n" + request.Replace("\n", "\r\n") + "\r\n" +
                "*********" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "***************************\r\n\r\n";
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.SelectionLength = 0;
            txtLog.ScrollToCaret();
        }

        private HollyContactHelper.ConDeviceType GetCurrentConDeviceType()
        {
            return (HollyContactHelper.ConDeviceType)comboBox_ConDeviceType.SelectedItem;
        }
        #endregion

        #region 事件
        /// 签入成功_回调事件
        /// <summary>
        /// 签入成功_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnSignInSuccess(object sender, EventArgs e)
        {
            LogMessage("签入成功 OnSignInSuccess");
        }
        /// 签出成功_回调事件
        /// <summary>
        /// 签出成功_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnSignOutSuccess(object sender, EventArgs e)
        {
            LogMessage("签出成功 OnSignOutSuccess");

            #region 跨号段监控逻辑——开始
            if (isOverPrefixListen)
            {
                int dnOrig = int.Parse(txtBox_DN.Text);//原始登录分机号码
                int dnTarget = int.Parse(txtBox_ListenAgentDN.Text);//目标分机号码
                if (Util.IsModifyINI(dnOrig, dnTarget))
                {
                    Util.ModifySoftphoneINIByDN(dnTarget);
                }

                btn_UNLoad_Click(null, null);
                btn_Load_Click(null,null);
            }
            #endregion 跨号段监控逻辑——结束
        }
        /// 振铃_回调事件
        /// <summary>
        /// 振铃_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnBelling(object sender, EventArgs e)
        {
            
            string log = string.Format("振铃 OnBelling\r\nGetLineInfo_SourceActType:{0}，GetLineInfo_SourceDeviceType:{1}",
                axUniSoftPhone1.GetLineInfo_SourceActType(), axUniSoftPhone1.GetLineInfo_SourceDeviceType());
            LogMessage(log);
        }
        /// 应答成功_回调事件
        /// <summary>
        /// 应答成功_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnAnswerSuccess(object sender, EventArgs e)
        {
            LogMessage("应答成功 OnAnswerSuccess");
        }
        /// 挂机_回调事件
        /// <summary>
        /// 挂机_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnHangup(object sender, EventArgs e)
        {
            LogMessage("挂机 OnHangup\r\n录音地址：" + axUniSoftPhone1.GetLineInfo_RecordFilePath() + axUniSoftPhone1.GetLineInfo_RecordFileName());
        }
        /// 呼出成功_回调事件
        /// <summary>
        /// 呼出成功_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnCallOutSuccess(object sender, EventArgs e)
        {
            LogMessage("呼出成功 OnCallOutSuccess\r\n录音地址：" + axUniSoftPhone1.GetLineInfo_RecordFilePath() + axUniSoftPhone1.GetLineInfo_RecordFileName());
        }
        /// 呼出失败_回调事件
        /// <summary>
        /// 呼出失败_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnCallOutFailed(object sender, EventArgs e)
        {
            LogMessage("呼出失败 OnCallOutFailed\r\n录音地址：" + axUniSoftPhone1.GetLineInfo_RecordFilePath() + axUniSoftPhone1.GetLineInfo_RecordFileName());
        }
        /// 呼出通话结束_回调事件
        /// <summary>
        /// 呼出通话结束_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnCallOutEnd(object sender, EventArgs e)
        {
            LogMessage("呼出通话结束 OnCallOutEnd\r\n录音地址：" + axUniSoftPhone1.GetLineInfo_RecordFilePath() + axUniSoftPhone1.GetLineInfo_RecordFileName());
        }

        /// 软电话状态改变_回调事件
        /// <summary>
        /// 软电话状态改变_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axUniSoftPhone1_OnStatusChange(object sender, EventArgs e)
        {
            string message = HollyContactHelper.ConvertPhoneStatus(axUniSoftPhone1.PreStatus) + " -> " +
                HollyContactHelper.ConvertPhoneStatus(axUniSoftPhone1.CurStatus);
            message += "\r\nGetLineInfo_SourceActType：" + axUniSoftPhone1.GetLineInfo_SourceActType();
            message += "\r\nGetLineInfo_DestActType：" + axUniSoftPhone1.GetLineInfo_DestActType();
            LogMessage("状态变化 OnStatusChange：" + message + "\r\n录音地址：" + axUniSoftPhone1.GetLineInfo_RecordFilePath() + axUniSoftPhone1.GetLineInfo_RecordFileName());
        }
        /// 软电话通知信息_回调事件
        /// <summary>
        /// 软电话通知信息_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">messageContent</param>
        private void axUniSoftPhone1_OnMessage(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnMessageEvent e)
        {
            string message = HollyContactHelper.ConvertPhoneStatus(axUniSoftPhone1.PreStatus) + " -> " +
                HollyContactHelper.ConvertPhoneStatus(axUniSoftPhone1.CurStatus);
            LogMessage("消息通知 OnMessage：" + message + "\r\n" + e.messageContent + "\r\n我们定义枚举状态：" + HollyContactHelper.ConvertPhoneStatus(axUniSoftPhone1.CurStatus));
            if (e.messageContent.StartsWith("服务器已上线:"))
            {
                currentLoadConfigCount++;
            }
            if (currentLoadConfigCount==loadConfigCount)
            {
                LogMessage("当前调用Load函数，返回的服务器信息条数为：" + currentLoadConfigCount);
                currentLoadConfigCount = 0;
                //跨号段监控逻辑
                if (isOverPrefixListen)
                {
                    btn_Init_Click(null, null);
                    btn_SignIn_Click(null, null);
                }
            }
            #region 跨号段监控逻辑——开始
            if (e.messageContent.Equals("签入成功。") && isOverPrefixListen )
            {
                btn_Ready_Click(null, null);
            }
            if (e.messageContent.Equals("示闲成功。") && isOverPrefixListen )
            {
                isOverPrefixListen = false; 
                bool flag = hollycontacthelper.ListenStart(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
                LogRequest("ListenStart-->>" + flag);
            }
            #endregion 跨号段监控逻辑——结束
        }
        /// 软电话报告错误信息_回调事件
        /// <summary>
        /// 软电话报告错误信息_回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">CTIErrorCode、ErrorCode、ErrorDesc</param>
        private void axUniSoftPhone1_OnError(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnErrorEvent e)
        {
            string message = string.Format("{3}\r\nCTIErrorCode:{0}\r\nErrorCode:{1}\r\nErrorDesc:{2}", e.cTIErrorCode, e.errCode, e.errDesc, "错误 OnError：");
            LogMessage(message);
        }

        private void axUniSoftPhone1_OnQueryAgentStatus(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnQueryAgentStatusEvent e)
        {
            string message = string.Format("axUniSoftPhone1_OnQueryAgentStatusAreaCode\r\nAreaCode:{0}\r\nStrStatusList:{1}",e.areaCode,e.strStatusList);
            LogMessage(message);
        }

        private void axUniSoftPhone1_OnQueryAgentStatusReturn(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnQueryAgentStatusReturnEvent e)
        {
            string message = string.Format("axUniSoftPhone1_OnQueryAgentStatusReturn\r\nStrStatusList:{0}",e.strStatusList);
            LogMessage(message);
        }

        private void axUniSoftPhone1_OnQueryIdleAgentsReturn(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnQueryIdleAgentsReturnEvent e)
        {
            string message = string.Format("axUniSoftPhone1_OnQueryIdleAgentsReturn\r\nStrStatusList:{0}", e.strStatusList);
            LogMessage(message);
        }

        private void axUniSoftPhone1_OnQueryAgentStatus(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnQueryQueueStatusEvent e)
        {
            string message = string.Format("axUniSoftPhone1_OnQueryAgentStatus\r\nStrStatusList:{0}", e.strStatusList);
            LogMessage(message);
        }
        #endregion

        #region 按钮
        private void btn_Load_Click(object sender, EventArgs e)
        {
            currentLoadConfigCount = 0;
            bool flag = hollycontacthelper.Load();
            LogRequest("Load-->>" + flag);
            
        }

        private void btn_UNLoad_Click(object sender, EventArgs e)
        {
            hollycontacthelper.UnLoad();
            LogRequest("UnLoad-->>" + true);
        }

        private void btn_Init_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> skills = new Dictionary<string, string>();
            skills.Add(txtBox_Skill.Text.Trim(), "1");
            hollycontacthelper.AndSkill(skills);
            LogRequest("Init-->>" + true);
        }

        private void btn_SignIn_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.SignIn(txtBox_AgentID.Text.Trim(), txtBox_DN.Text.Trim());
            LogRequest("SignIn-->>" + flag);
        }

        private void btn_SignOut_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.SignOut();
            LogRequest("SignOut-->>" + flag);
        }

        private void btn_Ready_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.SetReady();
            LogRequest("SetReady-->>" + flag);
        }

        private void btn_Busy_Click(object sender, EventArgs e)
        {
            bool flag = axUniSoftPhone1.actBusy();
            LogRequest("SetBusy-->>" + flag);
        }

        private void btn_Answer_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ActAnswer();
            LogRequest("ActAnswer-->>" + flag);
        }

        private void btn_Release_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ActHangup();
            LogRequest("ActHangup-->>" + flag);
        }

        private void btn_Mute_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.HoldStart();
            LogRequest("HoldStart-->>" + flag);
        }

        private void btn_Cancel_Mute_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.HoldEnd();
            LogRequest("HoldEnd-->>" + flag);
        }

        private void btn_CallOut_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ActCallOut(GetCurrentConDeviceType(), txtBox_NumbertoDial.Text.Trim());
            LogRequest("ActCallOut-->>" + flag + ",DestDeviceType:" + GetCurrentConDeviceType() + ",DestNo:" + txtBox_NumbertoDial.Text.Trim());
        }

        private void btn_ACW_Start_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.AfterCallStart();
            LogRequest("AfterCallStart-->>" + flag);
        }

        private void btn_ACW_End_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.AfterCallEnd();
            LogRequest("AfterCallEnd-->>" + flag);
        }

        private void btn_NumbertoTransfer_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.TransferStart(GetCurrentConDeviceType(), txtBox_NumbertoDial.Text.Trim());
            LogRequest("TransferStart-->>" + flag + ",DestDeviceType:" + GetCurrentConDeviceType() + ",DestNo:" + txtBox_NumbertoDial.Text.Trim());
        }

        private void btn_CancelTransferCall_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.TransferCancel();
            LogRequest("TransferCancel-->>" + flag);
        }

        private void btn_Consult_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConsultStart(GetCurrentConDeviceType(), txtBox_NumbertoDial.Text.Trim());
            LogRequest("ConsultStart-->>" + flag);
        }

        private void btn_CancelConsult_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConsultCancel();
            LogRequest("ConsultCancel-->>" + flag);
        }

        private void btn_EndConsult_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConsultEnd();
            LogRequest("ConsultEnd-->>" + flag);
        }

        private void btn_ForwardCall_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConsultToTransfer();
            LogRequest("ConsultToTransfer-->>" + flag);
        }

        private void btn_Conference_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConferenceStart(GetCurrentConDeviceType(), txtBox_NumbertoDial.Text.Trim());
            LogRequest("ConferenceStart-->>" + flag);
        }

        private void btn_Cancel_Conference_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConferenceCancel();
            LogRequest("ConferenceCancel-->>" + flag);
        }

        private void btn_End_Conference_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConferenceEnd();
            LogRequest("ConferenceEnd-->>" + flag);
        }

        private void btn_Consult2Conference_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ConsultToConference();
            LogRequest("ConsultToConference-->>" + flag);
        }

        private void btn_Listen_Click(object sender, EventArgs e)
        {
            #region 跨号段监控逻辑——开始
            //int dnOrig = int.Parse(txtBox_DN.Text);//原始登录分机号码
            //int dnTarget =0;
            //int.TryParse(txtBox_ListenAgentDN.Text,out dnTarget);//目标分机号码
            //if (dnTarget>0 && Util.IsModifyINI(dnOrig, dnTarget))
            //{
            //    btn_SignOut_Click(null, null);//签出
            //    isOverPrefixListen = true;
            //}
            //else
            #endregion 跨号段监控逻辑——结束
            {
                bool flag = hollycontacthelper.ListenStart(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
                LogRequest("ListenStart-->>" + flag);
            }
        }

        private void btn_ForceInsert_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ListenToQC(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
            LogRequest("ListenToQC-->>" + flag);
        }

        private void btn_Intercept_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ListenToLJ(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
            LogRequest("ListenToLJ-->>" + flag);
        }

        private void btn_DisconnectCall_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ListenToQCai(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
            LogRequest("ListenToQCai-->>" + flag);
        }

        private void btn_ForceSetIdle_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ListenToQZZX(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
            LogRequest("ListenToQZZX-->>" + flag);
        }

        private void btn_ForceSetBusy_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ListenToQZZM(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
            LogRequest("ListenToQZZM-->>" + flag);
        }

        private void btn_ForceLogout_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.ListenToQZQC(HollyContactHelper.ConDeviceType.座席工号, txtBox_NumbertoDial.Text.Trim());
            LogRequest("ListenToQZQC-->>" + flag);
        }

        private void btn_Rest_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.RestStart((BusyStatus)cb_rest.SelectedItem);
            LogRequest("RestStart-->>" + flag);
        }

        private void btn_Rest_End_Click(object sender, EventArgs e)
        {
            bool flag = hollycontacthelper.RestEnd();
            LogRequest("RestEnd-->>" + flag);
        }

        /// <summary>
        /// 转IVR满意度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TransferCall_Click(object sender, EventArgs e)
        {
            axUniSoftPhone1.SetOneSendCTIData("IVRFlowFlag", "4");
            axUniSoftPhone1.SetOneSendCTIData("ServiceID", "999");//我们这边生成的CallID，17位
            axUniSoftPhone1.DestDeviceType = axUniSoftPhone1.ConDeviceType_MODE_IVR;//IVR类型
            axUniSoftPhone1.DestNo = "58103008";
            bool flag = axUniSoftPhone1.actTransferCall();
            LogRequest(DllName + " actTransferCall:" + flag);
        }

        /// <summary>
        /// 转IVR满意度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TransferCall2_Click(object sender, EventArgs e)
        {
            string OriContactID = axUniSoftPhone1.GetLineInfo_AcceptCallID();
            string Key_IContact_ANI = axUniSoftPhone1.GetOneRecvCTIData("Key_IContact_ANI");
            if (Key_IContact_ANI.Trim() == "asterisk")
            {
                Key_IContact_ANI = axUniSoftPhone1.GetOneRecvCTIData("transANI");
            }
            string inputNum = txtBox_NumbertoDial.Text.Trim();
            axUniSoftPhone1.SetOneSendCTIData("IVRFlowFlag", "5");
            axUniSoftPhone1.SetOneSendCTIData("OriContactID", OriContactID);
            axUniSoftPhone1.SetOneSendCTIData("SkillDesc", inputNum);
            axUniSoftPhone1.SetOneSendCTIData("transANI", Key_IContact_ANI);

            axUniSoftPhone1.DestDeviceType = axUniSoftPhone1.ConDeviceType_MODE_IVR;//IVR类型
            axUniSoftPhone1.DestNo = "58103008";
            bool flag = axUniSoftPhone1.actTransferCall();

            LogRequest(DllName + " actTransferCall:" + flag);
            LogRequest(string.Format("OriContactID:{0},\r\ntransANI{1},\r\ninputSkillNum:{2}", OriContactID, Key_IContact_ANI, inputNum));
        }

        #endregion

        #region 不使用功能

        private void btn_SendDTMF_Click(object sender, EventArgs e)
        {
            bool flag = axUniSoftPhone1.actSendDTMF("2");
            LogRequest(DllName + " actSendDTMF:" + flag);
        }



        private void btn_HelpTransferCall_Click(object sender, EventArgs e)
        {
            axUniSoftPhone1.SetOneSendCTIData("IVRFlowFlag", "4");
            axUniSoftPhone1.SetOneSendCTIData("ServiceID", "999");//我们这边生成的CallID，17位
            axUniSoftPhone1.DestDeviceType = axUniSoftPhone1.ConDeviceType_MODE_IVR;//IVR类型
            axUniSoftPhone1.DestNo = "58103008";
            bool flag = axUniSoftPhone1.actHelpTransferCall();
            LogRequest(DllName + " actHelpTransferCall:" + flag);
            //axUniSoftPhone1.actOPConsult(1);
        }
        #endregion

        /// <summary>
        /// 获取坐席状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //string message = "状态(" + axUniSoftPhone1.CurStatus + ")：" + HollyContactHelper.ConvertPhoneStatus(axUniSoftPhone1.CurStatus) +
            //  "  呼叫来源：" + HollyContactHelper.ConvertConCallType(axUniSoftPhone1.GetLineInfo_SourceActType()) +
            //  "  呼叫目标：" + HollyContactHelper.ConvertConCallType(axUniSoftPhone1.GetLineInfo_DestActType());
            //LogMessage(message + "\r\n");
            bool flag = axUniSoftPhone1.actQueryAgentStatus("1001");
            LogRequest(DllName + " actQueryAgentStatus:" + flag + ",AreaCode:" + "1001");
            //bool flag = axUniSoftPhone1.actQueryIdleAgents();
            //LogRequest(DllName + " actQueryIdleAgents:" + flag + ",IsInBoundCall:" + axUniSoftPhone1.IsInBoundCall.ToString());

        }

        /// <summary>
        /// 情况内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        /// <summary>
        /// 获取空闲坐席
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryIdleAgents_Click(object sender, EventArgs e)
        {
            bool flag = axUniSoftPhone1.actQueryIdleAgents();
            LogRequest(DllName + " actQueryIdleAgents:" + flag);

            LogRequest("ConCallType:"+HollyContactHelper.ConvertConCallType(axUniSoftPhone1.GetLineInfo_SourceActType()));
        }

        private void btnGetCTIData_Click(object sender, EventArgs e)
        {
            txtCTIDataValue.Text = string.Empty;
            string ctiDataKey = comboBox_CTIDataKey.SelectedValue != null ? comboBox_CTIDataKey.SelectedValue.ToString().Trim() : comboBox_CTIDataKey.Text;
            if (string.IsNullOrEmpty(ctiDataKey))
            {
                MessageBox.Show("随路数据中的Key不能为空！");
            }
            else
            {
                string value = axUniSoftPhone1.GetOneRecvCTIData(ctiDataKey);
                txtCTIDataValue.Text = value;
                LogRequest(DllName + " GetOneRecvCTIData[" + ctiDataKey + "]:" + value);
            }
        }


    }
}
