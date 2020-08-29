namespace HollyContact5_Demo
{
    partial class VoiceSample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VoiceSample));
            this.btn_Load = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBox_AgentID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBox_Skill = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBox_DN = new System.Windows.Forms.TextBox();
            this.btn_UNLoad = new System.Windows.Forms.Button();
            this.btn_Init = new System.Windows.Forms.Button();
            this.btn_SignIn = new System.Windows.Forms.Button();
            this.btn_SignOut = new System.Windows.Forms.Button();
            this.btn_Ready = new System.Windows.Forms.Button();
            this.btn_Busy = new System.Windows.Forms.Button();
            this.btn_ACW_Start = new System.Windows.Forms.Button();
            this.btn_ACW_End = new System.Windows.Forms.Button();
            this.btn_Rest = new System.Windows.Forms.Button();
            this.btn_Rest_End = new System.Windows.Forms.Button();
            this.btn_Answer = new System.Windows.Forms.Button();
            this.btn_Release = new System.Windows.Forms.Button();
            this.btn_Mute = new System.Windows.Forms.Button();
            this.btn_Cancel_Mute = new System.Windows.Forms.Button();
            this.txtBox_NumbertoDial = new System.Windows.Forms.TextBox();
            this.txtBox_ListenAgentDN = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labe24 = new System.Windows.Forms.Label();
            this.btn_CallOut = new System.Windows.Forms.Button();
            this.btn_NumbertoTransfer = new System.Windows.Forms.Button();
            this.btn_CancelTransferCall = new System.Windows.Forms.Button();
            this.btn_HelpTransferCall = new System.Windows.Forms.Button();
            this.btn_Consult = new System.Windows.Forms.Button();
            this.btn_CancelConsult = new System.Windows.Forms.Button();
            this.btn_EndConsult = new System.Windows.Forms.Button();
            this.btn_ForwardCall = new System.Windows.Forms.Button();
            this.btn_Consult2Conference = new System.Windows.Forms.Button();
            this.btn_Conference = new System.Windows.Forms.Button();
            this.btn_Cancel_Conference = new System.Windows.Forms.Button();
            this.btn_End_Conference = new System.Windows.Forms.Button();
            this.btn_SendDTMF = new System.Windows.Forms.Button();
            this.btn_Listen = new System.Windows.Forms.Button();
            this.btn_ForceInsert = new System.Windows.Forms.Button();
            this.btn_DisconnectCall = new System.Windows.Forms.Button();
            this.btn_Intercept = new System.Windows.Forms.Button();
            this.btn_ForceLogout = new System.Windows.Forms.Button();
            this.btn_ForceSetBusy = new System.Windows.Forms.Button();
            this.btn_ForceSetIdle = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_ConDeviceType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_rest = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_TransferCall = new System.Windows.Forms.Button();
            this.btn_TransferCall2 = new System.Windows.Forms.Button();
            this.btn_QueryIdleAgents = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGetCTIData = new System.Windows.Forms.Button();
            this.txtCTIDataValue = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_CTIDataKey = new System.Windows.Forms.ComboBox();
            this.axUniSoftPhone1 = new AxUniSoftPhoneControl.AxUniSoftPhone();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axUniSoftPhone1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Load
            // 
            this.btn_Load.Location = new System.Drawing.Point(196, 16);
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(75, 23);
            this.btn_Load.TabIndex = 0;
            this.btn_Load.Text = "装载";
            this.btn_Load.UseVisualStyleBackColor = true;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 406);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(522, 244);
            this.txtLog.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "AgentID：";
            // 
            // txtBox_AgentID
            // 
            this.txtBox_AgentID.Location = new System.Drawing.Point(90, 18);
            this.txtBox_AgentID.Name = "txtBox_AgentID";
            this.txtBox_AgentID.Size = new System.Drawing.Size(100, 21);
            this.txtBox_AgentID.TabIndex = 3;
            this.txtBox_AgentID.Text = "1098";

            // 
            // txtBox_AgentID
            // 
            this.txtBox_ListenAgentDN.Location = new System.Drawing.Point(90, 228);
            this.txtBox_ListenAgentDN.Name = "txtBox_ListenAgentID";
            this.txtBox_ListenAgentDN.Size = new System.Drawing.Size(100, 21);
            this.txtBox_ListenAgentDN.TabIndex = 3;
            this.txtBox_ListenAgentDN.Text = "";


            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Skill：";
            // 
            // txtBox_Skill
            // 
            this.txtBox_Skill.Location = new System.Drawing.Point(90, 45);
            this.txtBox_Skill.Name = "txtBox_Skill";
            this.txtBox_Skill.Size = new System.Drawing.Size(100, 21);
            this.txtBox_Skill.TabIndex = 3;
            this.txtBox_Skill.Text = "1098";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "DN：";
            // 
            // txtBox_DN
            // 
            this.txtBox_DN.Location = new System.Drawing.Point(90, 72);
            this.txtBox_DN.Name = "txtBox_DN";
            this.txtBox_DN.Size = new System.Drawing.Size(100, 21);
            this.txtBox_DN.TabIndex = 3;
            this.txtBox_DN.Text = "35197";
            // 
            // btn_UNLoad
            // 
            this.btn_UNLoad.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_UNLoad.Location = new System.Drawing.Point(277, 16);
            this.btn_UNLoad.Name = "btn_UNLoad";
            this.btn_UNLoad.Size = new System.Drawing.Size(75, 23);
            this.btn_UNLoad.TabIndex = 0;
            this.btn_UNLoad.Text = "卸载";
            this.btn_UNLoad.UseVisualStyleBackColor = true;
            this.btn_UNLoad.Click += new System.EventHandler(this.btn_UNLoad_Click);
            // 
            // btn_Init
            // 
            this.btn_Init.Location = new System.Drawing.Point(196, 43);
            this.btn_Init.Name = "btn_Init";
            this.btn_Init.Size = new System.Drawing.Size(75, 23);
            this.btn_Init.TabIndex = 0;
            this.btn_Init.Text = "初始化";
            this.btn_Init.UseVisualStyleBackColor = true;
            this.btn_Init.Click += new System.EventHandler(this.btn_Init_Click);
            // 
            // btn_SignIn
            // 
            this.btn_SignIn.Location = new System.Drawing.Point(196, 70);
            this.btn_SignIn.Name = "btn_SignIn";
            this.btn_SignIn.Size = new System.Drawing.Size(75, 23);
            this.btn_SignIn.TabIndex = 0;
            this.btn_SignIn.Text = "签入";
            this.btn_SignIn.UseVisualStyleBackColor = true;
            this.btn_SignIn.Click += new System.EventHandler(this.btn_SignIn_Click);
            // 
            // btn_SignOut
            // 
            this.btn_SignOut.Location = new System.Drawing.Point(277, 70);
            this.btn_SignOut.Name = "btn_SignOut";
            this.btn_SignOut.Size = new System.Drawing.Size(75, 23);
            this.btn_SignOut.TabIndex = 0;
            this.btn_SignOut.Text = "签出";
            this.btn_SignOut.UseVisualStyleBackColor = true;
            this.btn_SignOut.Click += new System.EventHandler(this.btn_SignOut_Click);
            // 
            // btn_Ready
            // 
            this.btn_Ready.Location = new System.Drawing.Point(196, 99);
            this.btn_Ready.Name = "btn_Ready";
            this.btn_Ready.Size = new System.Drawing.Size(75, 23);
            this.btn_Ready.TabIndex = 0;
            this.btn_Ready.Text = "置闲";
            this.btn_Ready.UseVisualStyleBackColor = true;
            this.btn_Ready.Click += new System.EventHandler(this.btn_Ready_Click);
            // 
            // btn_Busy
            // 
            this.btn_Busy.Location = new System.Drawing.Point(277, 99);
            this.btn_Busy.Name = "btn_Busy";
            this.btn_Busy.Size = new System.Drawing.Size(75, 23);
            this.btn_Busy.TabIndex = 0;
            this.btn_Busy.Text = "置忙";
            this.btn_Busy.UseVisualStyleBackColor = true;
            this.btn_Busy.Click += new System.EventHandler(this.btn_Busy_Click);
            // 
            // btn_ACW_Start
            // 
            this.btn_ACW_Start.Location = new System.Drawing.Point(358, 99);
            this.btn_ACW_Start.Name = "btn_ACW_Start";
            this.btn_ACW_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_ACW_Start.TabIndex = 0;
            this.btn_ACW_Start.Text = "话后开始";
            this.btn_ACW_Start.UseVisualStyleBackColor = true;
            this.btn_ACW_Start.Click += new System.EventHandler(this.btn_ACW_Start_Click);
            // 
            // btn_ACW_End
            // 
            this.btn_ACW_End.Location = new System.Drawing.Point(439, 99);
            this.btn_ACW_End.Name = "btn_ACW_End";
            this.btn_ACW_End.Size = new System.Drawing.Size(75, 23);
            this.btn_ACW_End.TabIndex = 0;
            this.btn_ACW_End.Text = "话后结束";
            this.btn_ACW_End.UseVisualStyleBackColor = true;
            this.btn_ACW_End.Click += new System.EventHandler(this.btn_ACW_End_Click);
            // 
            // btn_Rest
            // 
            this.btn_Rest.Location = new System.Drawing.Point(196, 128);
            this.btn_Rest.Name = "btn_Rest";
            this.btn_Rest.Size = new System.Drawing.Size(75, 23);
            this.btn_Rest.TabIndex = 0;
            this.btn_Rest.Text = "休息";
            this.btn_Rest.UseVisualStyleBackColor = true;
            this.btn_Rest.Click += new System.EventHandler(this.btn_Rest_Click);
            // 
            // btn_Rest_End
            // 
            this.btn_Rest_End.Location = new System.Drawing.Point(277, 128);
            this.btn_Rest_End.Name = "btn_Rest_End";
            this.btn_Rest_End.Size = new System.Drawing.Size(75, 23);
            this.btn_Rest_End.TabIndex = 0;
            this.btn_Rest_End.Text = "结束休息";
            this.btn_Rest_End.UseVisualStyleBackColor = true;
            this.btn_Rest_End.Click += new System.EventHandler(this.btn_Rest_End_Click);
            // 
            // btn_Answer
            // 
            this.btn_Answer.Location = new System.Drawing.Point(277, 154);
            this.btn_Answer.Name = "btn_Answer";
            this.btn_Answer.Size = new System.Drawing.Size(75, 23);
            this.btn_Answer.TabIndex = 0;
            this.btn_Answer.Text = "应答";
            this.btn_Answer.UseVisualStyleBackColor = true;
            this.btn_Answer.Click += new System.EventHandler(this.btn_Answer_Click);
            // 
            // btn_Release
            // 
            this.btn_Release.Location = new System.Drawing.Point(358, 154);
            this.btn_Release.Name = "btn_Release";
            this.btn_Release.Size = new System.Drawing.Size(75, 23);
            this.btn_Release.TabIndex = 0;
            this.btn_Release.Text = "挂机";
            this.btn_Release.UseVisualStyleBackColor = true;
            this.btn_Release.Click += new System.EventHandler(this.btn_Release_Click);
            // 
            // btn_Mute
            // 
            this.btn_Mute.Location = new System.Drawing.Point(196, 183);
            this.btn_Mute.Name = "btn_Mute";
            this.btn_Mute.Size = new System.Drawing.Size(75, 23);
            this.btn_Mute.TabIndex = 0;
            this.btn_Mute.Text = "保持";
            this.btn_Mute.UseVisualStyleBackColor = true;
            this.btn_Mute.Click += new System.EventHandler(this.btn_Mute_Click);
            // 
            // btn_Cancel_Mute
            // 
            this.btn_Cancel_Mute.Location = new System.Drawing.Point(277, 183);
            this.btn_Cancel_Mute.Name = "btn_Cancel_Mute";
            this.btn_Cancel_Mute.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel_Mute.TabIndex = 0;
            this.btn_Cancel_Mute.Text = "取消保持";
            this.btn_Cancel_Mute.UseVisualStyleBackColor = true;
            this.btn_Cancel_Mute.Click += new System.EventHandler(this.btn_Cancel_Mute_Click);
            // 
            // txtBox_NumbertoDial
            // 
            this.txtBox_NumbertoDial.Location = new System.Drawing.Point(90, 185);
            this.txtBox_NumbertoDial.Name = "txtBox_NumbertoDial";
            this.txtBox_NumbertoDial.Size = new System.Drawing.Size(100, 21);
            this.txtBox_NumbertoDial.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "号码：";


            this.labe24.AutoSize = true;
            this.labe24.Location = new System.Drawing.Point(8, 228);
            this.labe24.Name = "labe24";
            this.labe24.Size = new System.Drawing.Size(41, 12);
            this.labe24.TabIndex = 2;
            this.labe24.Text = "跨域监控分机号：";
            // 
            // btn_CallOut
            // 
            this.btn_CallOut.Location = new System.Drawing.Point(196, 154);
            this.btn_CallOut.Name = "btn_CallOut";
            this.btn_CallOut.Size = new System.Drawing.Size(75, 23);
            this.btn_CallOut.TabIndex = 0;
            this.btn_CallOut.Text = "外呼";
            this.btn_CallOut.UseVisualStyleBackColor = true;
            this.btn_CallOut.Click += new System.EventHandler(this.btn_CallOut_Click);
            // 
            // btn_NumbertoTransfer
            // 
            this.btn_NumbertoTransfer.Location = new System.Drawing.Point(196, 212);
            this.btn_NumbertoTransfer.Name = "btn_NumbertoTransfer";
            this.btn_NumbertoTransfer.Size = new System.Drawing.Size(75, 23);
            this.btn_NumbertoTransfer.TabIndex = 0;
            this.btn_NumbertoTransfer.Text = "转接";
            this.btn_NumbertoTransfer.UseVisualStyleBackColor = true;
            this.btn_NumbertoTransfer.Click += new System.EventHandler(this.btn_NumbertoTransfer_Click);
            // 
            // btn_CancelTransferCall
            // 
            this.btn_CancelTransferCall.Location = new System.Drawing.Point(277, 212);
            this.btn_CancelTransferCall.Name = "btn_CancelTransferCall";
            this.btn_CancelTransferCall.Size = new System.Drawing.Size(75, 23);
            this.btn_CancelTransferCall.TabIndex = 0;
            this.btn_CancelTransferCall.Text = "取消转接";
            this.btn_CancelTransferCall.UseVisualStyleBackColor = true;
            this.btn_CancelTransferCall.Click += new System.EventHandler(this.btn_CancelTransferCall_Click);
            // 
            // btn_HelpTransferCall
            // 
            this.btn_HelpTransferCall.Location = new System.Drawing.Point(171, 20);
            this.btn_HelpTransferCall.Name = "btn_HelpTransferCall";
            this.btn_HelpTransferCall.Size = new System.Drawing.Size(75, 23);
            this.btn_HelpTransferCall.TabIndex = 0;
            this.btn_HelpTransferCall.Text = "挂起转接";
            this.btn_HelpTransferCall.UseVisualStyleBackColor = true;
            this.btn_HelpTransferCall.Click += new System.EventHandler(this.btn_HelpTransferCall_Click);
            // 
            // btn_Consult
            // 
            this.btn_Consult.Location = new System.Drawing.Point(196, 241);
            this.btn_Consult.Name = "btn_Consult";
            this.btn_Consult.Size = new System.Drawing.Size(75, 23);
            this.btn_Consult.TabIndex = 0;
            this.btn_Consult.Text = "咨询";
            this.btn_Consult.UseVisualStyleBackColor = true;
            this.btn_Consult.Click += new System.EventHandler(this.btn_Consult_Click);
            // 
            // btn_CancelConsult
            // 
            this.btn_CancelConsult.Location = new System.Drawing.Point(277, 241);
            this.btn_CancelConsult.Name = "btn_CancelConsult";
            this.btn_CancelConsult.Size = new System.Drawing.Size(75, 23);
            this.btn_CancelConsult.TabIndex = 0;
            this.btn_CancelConsult.Text = "取消咨询";
            this.btn_CancelConsult.UseVisualStyleBackColor = true;
            this.btn_CancelConsult.Click += new System.EventHandler(this.btn_CancelConsult_Click);
            // 
            // btn_EndConsult
            // 
            this.btn_EndConsult.Location = new System.Drawing.Point(358, 241);
            this.btn_EndConsult.Name = "btn_EndConsult";
            this.btn_EndConsult.Size = new System.Drawing.Size(75, 23);
            this.btn_EndConsult.TabIndex = 0;
            this.btn_EndConsult.Text = "结束咨询";
            this.btn_EndConsult.UseVisualStyleBackColor = true;
            this.btn_EndConsult.Click += new System.EventHandler(this.btn_EndConsult_Click);
            // 
            // btn_ForwardCall
            // 
            this.btn_ForwardCall.Location = new System.Drawing.Point(439, 241);
            this.btn_ForwardCall.Name = "btn_ForwardCall";
            this.btn_ForwardCall.Size = new System.Drawing.Size(75, 23);
            this.btn_ForwardCall.TabIndex = 0;
            this.btn_ForwardCall.Text = "咨询变转接";
            this.btn_ForwardCall.UseVisualStyleBackColor = true;
            this.btn_ForwardCall.Click += new System.EventHandler(this.btn_ForwardCall_Click);
            // 
            // btn_Consult2Conference
            // 
            this.btn_Consult2Conference.Location = new System.Drawing.Point(439, 270);
            this.btn_Consult2Conference.Name = "btn_Consult2Conference";
            this.btn_Consult2Conference.Size = new System.Drawing.Size(75, 23);
            this.btn_Consult2Conference.TabIndex = 0;
            this.btn_Consult2Conference.Text = "咨询变会议";
            this.btn_Consult2Conference.UseVisualStyleBackColor = true;
            this.btn_Consult2Conference.Click += new System.EventHandler(this.btn_Consult2Conference_Click);
            // 
            // btn_Conference
            // 
            this.btn_Conference.Location = new System.Drawing.Point(12, 20);
            this.btn_Conference.Name = "btn_Conference";
            this.btn_Conference.Size = new System.Drawing.Size(75, 23);
            this.btn_Conference.TabIndex = 0;
            this.btn_Conference.Text = "会议";
            this.btn_Conference.UseVisualStyleBackColor = true;
            this.btn_Conference.Click += new System.EventHandler(this.btn_Conference_Click);
            // 
            // btn_Cancel_Conference
            // 
            this.btn_Cancel_Conference.Location = new System.Drawing.Point(90, 20);
            this.btn_Cancel_Conference.Name = "btn_Cancel_Conference";
            this.btn_Cancel_Conference.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel_Conference.TabIndex = 0;
            this.btn_Cancel_Conference.Text = "取消会议";
            this.btn_Cancel_Conference.UseVisualStyleBackColor = true;
            this.btn_Cancel_Conference.Click += new System.EventHandler(this.btn_Cancel_Conference_Click);
            // 
            // btn_End_Conference
            // 
            this.btn_End_Conference.Location = new System.Drawing.Point(358, 270);
            this.btn_End_Conference.Name = "btn_End_Conference";
            this.btn_End_Conference.Size = new System.Drawing.Size(75, 23);
            this.btn_End_Conference.TabIndex = 0;
            this.btn_End_Conference.Text = "结束会议";
            this.btn_End_Conference.UseVisualStyleBackColor = true;
            this.btn_End_Conference.Click += new System.EventHandler(this.btn_End_Conference_Click);
            // 
            // btn_SendDTMF
            // 
            this.btn_SendDTMF.Location = new System.Drawing.Point(252, 20);
            this.btn_SendDTMF.Name = "btn_SendDTMF";
            this.btn_SendDTMF.Size = new System.Drawing.Size(75, 23);
            this.btn_SendDTMF.TabIndex = 0;
            this.btn_SendDTMF.Text = "二次外呼";
            this.btn_SendDTMF.UseVisualStyleBackColor = true;
            this.btn_SendDTMF.Click += new System.EventHandler(this.btn_SendDTMF_Click);
            // 
            // btn_Listen
            // 
            this.btn_Listen.Location = new System.Drawing.Point(196, 299);
            this.btn_Listen.Name = "btn_Listen";
            this.btn_Listen.Size = new System.Drawing.Size(75, 23);
            this.btn_Listen.TabIndex = 0;
            this.btn_Listen.Text = "监听";
            this.btn_Listen.UseVisualStyleBackColor = true;
            this.btn_Listen.Click += new System.EventHandler(this.btn_Listen_Click);
            // 
            // btn_ForceInsert
            // 
            this.btn_ForceInsert.Location = new System.Drawing.Point(277, 299);
            this.btn_ForceInsert.Name = "btn_ForceInsert";
            this.btn_ForceInsert.Size = new System.Drawing.Size(75, 23);
            this.btn_ForceInsert.TabIndex = 0;
            this.btn_ForceInsert.Text = "强插";
            this.btn_ForceInsert.UseVisualStyleBackColor = true;
            this.btn_ForceInsert.Click += new System.EventHandler(this.btn_ForceInsert_Click);
            // 
            // btn_DisconnectCall
            // 
            this.btn_DisconnectCall.Location = new System.Drawing.Point(358, 299);
            this.btn_DisconnectCall.Name = "btn_DisconnectCall";
            this.btn_DisconnectCall.Size = new System.Drawing.Size(75, 23);
            this.btn_DisconnectCall.TabIndex = 0;
            this.btn_DisconnectCall.Text = "强拆";
            this.btn_DisconnectCall.UseVisualStyleBackColor = true;
            this.btn_DisconnectCall.Click += new System.EventHandler(this.btn_DisconnectCall_Click);
            // 
            // btn_Intercept
            // 
            this.btn_Intercept.Location = new System.Drawing.Point(439, 299);
            this.btn_Intercept.Name = "btn_Intercept";
            this.btn_Intercept.Size = new System.Drawing.Size(75, 23);
            this.btn_Intercept.TabIndex = 0;
            this.btn_Intercept.Text = "拦截";
            this.btn_Intercept.UseVisualStyleBackColor = true;
            this.btn_Intercept.Click += new System.EventHandler(this.btn_Intercept_Click);
            // 
            // btn_ForceLogout
            // 
            this.btn_ForceLogout.Location = new System.Drawing.Point(358, 328);
            this.btn_ForceLogout.Name = "btn_ForceLogout";
            this.btn_ForceLogout.Size = new System.Drawing.Size(75, 23);
            this.btn_ForceLogout.TabIndex = 0;
            this.btn_ForceLogout.Text = "强制签出";
            this.btn_ForceLogout.UseVisualStyleBackColor = true;
            this.btn_ForceLogout.Click += new System.EventHandler(this.btn_ForceLogout_Click);
            // 
            // btn_ForceSetBusy
            // 
            this.btn_ForceSetBusy.Location = new System.Drawing.Point(277, 328);
            this.btn_ForceSetBusy.Name = "btn_ForceSetBusy";
            this.btn_ForceSetBusy.Size = new System.Drawing.Size(75, 23);
            this.btn_ForceSetBusy.TabIndex = 0;
            this.btn_ForceSetBusy.Text = "强制置忙";
            this.btn_ForceSetBusy.UseVisualStyleBackColor = true;
            this.btn_ForceSetBusy.Click += new System.EventHandler(this.btn_ForceSetBusy_Click);
            // 
            // btn_ForceSetIdle
            // 
            this.btn_ForceSetIdle.Location = new System.Drawing.Point(196, 328);
            this.btn_ForceSetIdle.Name = "btn_ForceSetIdle";
            this.btn_ForceSetIdle.Size = new System.Drawing.Size(75, 23);
            this.btn_ForceSetIdle.TabIndex = 0;
            this.btn_ForceSetIdle.Text = "强制置闲";
            this.btn_ForceSetIdle.UseVisualStyleBackColor = true;
            this.btn_ForceSetIdle.Click += new System.EventHandler(this.btn_ForceSetIdle_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "外拨类型：";
            // 
            // comboBox_ConDeviceType
            // 
            this.comboBox_ConDeviceType.FormattingEnabled = true;
            this.comboBox_ConDeviceType.Items.AddRange(new object[] {
            "dadsf,1",
            "ddd,2"});
            this.comboBox_ConDeviceType.Location = new System.Drawing.Point(90, 156);
            this.comboBox_ConDeviceType.Name = "comboBox_ConDeviceType";
            this.comboBox_ConDeviceType.Size = new System.Drawing.Size(100, 20);
            this.comboBox_ConDeviceType.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_HelpTransferCall);
            this.groupBox1.Controls.Add(this.btn_SendDTMF);
            this.groupBox1.Controls.Add(this.btn_Conference);
            this.groupBox1.Controls.Add(this.btn_Cancel_Conference);
            this.groupBox1.Location = new System.Drawing.Point(12, 348);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(522, 52);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "不使用功能";
            // 
            // cb_rest
            // 
            this.cb_rest.FormattingEnabled = true;
            this.cb_rest.Items.AddRange(new object[] {
            "dadsf,1",
            "ddd,2"});
            this.cb_rest.Location = new System.Drawing.Point(90, 130);
            this.cb_rest.Name = "cb_rest";
            this.cb_rest.Size = new System.Drawing.Size(100, 20);
            this.cb_rest.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "休息类型：";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(358, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "获取坐席状态";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(457, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "清空";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_TransferCall
            // 
            this.btn_TransferCall.Location = new System.Drawing.Point(358, 212);
            this.btn_TransferCall.Name = "btn_TransferCall";
            this.btn_TransferCall.Size = new System.Drawing.Size(75, 23);
            this.btn_TransferCall.TabIndex = 0;
            this.btn_TransferCall.Text = "转IVR满意度";
            this.btn_TransferCall.UseVisualStyleBackColor = true;
            this.btn_TransferCall.Click += new System.EventHandler(this.btn_TransferCall_Click);
            // 
            // btn_TransferCall2
            // 
            this.btn_TransferCall2.Location = new System.Drawing.Point(439, 212);
            this.btn_TransferCall2.Name = "btn_TransferCall2";
            this.btn_TransferCall2.Size = new System.Drawing.Size(95, 23);
            this.btn_TransferCall2.TabIndex = 0;
            this.btn_TransferCall2.Text = "跨分机号段-转接";
            this.btn_TransferCall2.UseVisualStyleBackColor = true;
            this.btn_TransferCall2.Click += new System.EventHandler(this.btn_TransferCall2_Click);
            // 
            // btn_QueryIdleAgents
            // 
            this.btn_QueryIdleAgents.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_QueryIdleAgents.Location = new System.Drawing.Point(358, 43);
            this.btn_QueryIdleAgents.Name = "btn_QueryIdleAgents";
            this.btn_QueryIdleAgents.Size = new System.Drawing.Size(91, 23);
            this.btn_QueryIdleAgents.TabIndex = 8;
            this.btn_QueryIdleAgents.Text = "获取空闲坐席";
            this.btn_QueryIdleAgents.UseVisualStyleBackColor = true;
            this.btn_QueryIdleAgents.Click += new System.EventHandler(this.btn_QueryIdleAgents_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 275);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "随路数据Key：";
            // 
            // btnGetCTIData
            // 
            this.btnGetCTIData.Location = new System.Drawing.Point(196, 270);
            this.btnGetCTIData.Name = "btnGetCTIData";
            this.btnGetCTIData.Size = new System.Drawing.Size(75, 23);
            this.btnGetCTIData.TabIndex = 0;
            this.btnGetCTIData.Text = "获取随路数据";
            this.btnGetCTIData.UseVisualStyleBackColor = true;
            this.btnGetCTIData.Click += new System.EventHandler(this.btnGetCTIData_Click);
            // 
            // txtCTIDataValue
            // 
            this.txtCTIDataValue.Location = new System.Drawing.Point(90, 301);
            this.txtCTIDataValue.Name = "txtCTIDataValue";
            this.txtCTIDataValue.Size = new System.Drawing.Size(100, 21);
            this.txtCTIDataValue.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(37, 304);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "value：";
            // 
            // comboBox_CTIDataKey
            // 
            this.comboBox_CTIDataKey.FormattingEnabled = true;
            this.comboBox_CTIDataKey.Items.AddRange(new object[] {
            "dadsf,1",
            "ddd,2"});
            this.comboBox_CTIDataKey.Location = new System.Drawing.Point(90, 272);
            this.comboBox_CTIDataKey.Name = "comboBox_CTIDataKey";
            this.comboBox_CTIDataKey.Size = new System.Drawing.Size(100, 20);
            this.comboBox_CTIDataKey.TabIndex = 4;
            // 
            // axUniSoftPhone1
            // 
            this.axUniSoftPhone1.Location = new System.Drawing.Point(443, 299);
            this.axUniSoftPhone1.Name = "axUniSoftPhone1";
            this.axUniSoftPhone1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axUniSoftPhone1.OcxState")));
            this.axUniSoftPhone1.Size = new System.Drawing.Size(192, 192);
            this.axUniSoftPhone1.TabIndex = 0;
            this.axUniSoftPhone1.OnSignInSuccess += new System.EventHandler(this.axUniSoftPhone1_OnSignInSuccess);
            this.axUniSoftPhone1.OnSignOutSuccess += new System.EventHandler(this.axUniSoftPhone1_OnSignOutSuccess);
            this.axUniSoftPhone1.OnBelling += new System.EventHandler(this.axUniSoftPhone1_OnBelling);
            this.axUniSoftPhone1.OnAnswerSuccess += new System.EventHandler(this.axUniSoftPhone1_OnAnswerSuccess);
            this.axUniSoftPhone1.OnHangup += new System.EventHandler(this.axUniSoftPhone1_OnHangup);
            this.axUniSoftPhone1.OnCallOutSuccess += new System.EventHandler(this.axUniSoftPhone1_OnCallOutSuccess);
            this.axUniSoftPhone1.OnCallOutFailed += new System.EventHandler(this.axUniSoftPhone1_OnCallOutFailed);
            this.axUniSoftPhone1.OnCallOutEnd += new System.EventHandler(this.axUniSoftPhone1_OnCallOutEnd);
            this.axUniSoftPhone1.OnQueryAgentStatus += new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnQueryAgentStatusEventHandler(this.axUniSoftPhone1_OnQueryAgentStatus);
            this.axUniSoftPhone1.OnQueryAgentStatusReturn += new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnQueryAgentStatusReturnEventHandler(this.axUniSoftPhone1_OnQueryAgentStatusReturn);
            this.axUniSoftPhone1.OnStatusChange += new System.EventHandler(this.axUniSoftPhone1_OnStatusChange);
            this.axUniSoftPhone1.OnError += new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnErrorEventHandler(this.axUniSoftPhone1_OnError);
            this.axUniSoftPhone1.OnMessage += new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnMessageEventHandler(this.axUniSoftPhone1_OnMessage);
            this.axUniSoftPhone1.OnQueryIdleAgentsReturn += new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnQueryIdleAgentsReturnEventHandler(this.axUniSoftPhone1_OnQueryIdleAgentsReturn);
            // 
            // VoiceSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 662);
            this.Controls.Add(this.btn_DisconnectCall);
            this.Controls.Add(this.btn_TransferCall);
            this.Controls.Add(this.btn_TransferCall2);
            this.Controls.Add(this.btn_ForceSetIdle);
            this.Controls.Add(this.btn_ForceSetBusy);
            this.Controls.Add(this.btn_ForceLogout);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_QueryIdleAgents);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cb_rest);
            this.Controls.Add(this.btn_Intercept);
            this.Controls.Add(this.btn_Rest_End);
            this.Controls.Add(this.btn_Rest);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox_CTIDataKey);
            this.Controls.Add(this.comboBox_ConDeviceType);
            this.Controls.Add(this.txtCTIDataValue);
            this.Controls.Add(this.txtBox_NumbertoDial);
            this.Controls.Add(this.txtBox_DN);
            this.Controls.Add(this.txtBox_Skill);
            this.Controls.Add(this.txtBox_AgentID);
            this.Controls.Add(this.txtBox_ListenAgentDN);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labe24);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btn_UNLoad);
            this.Controls.Add(this.btn_SignOut);
            this.Controls.Add(this.btn_Busy);
            this.Controls.Add(this.btn_ACW_End);
            this.Controls.Add(this.btn_ACW_Start);
            this.Controls.Add(this.btn_Cancel_Mute);
            this.Controls.Add(this.btn_Mute);
            this.Controls.Add(this.btn_Release);
            this.Controls.Add(this.btn_CancelTransferCall);
            this.Controls.Add(this.btn_Consult2Conference);
            this.Controls.Add(this.btn_ForwardCall);
            this.Controls.Add(this.btn_EndConsult);
            this.Controls.Add(this.btn_CancelConsult);
            this.Controls.Add(this.btn_End_Conference);
            this.Controls.Add(this.btn_ForceInsert);
            this.Controls.Add(this.btn_Listen);
            this.Controls.Add(this.btnGetCTIData);
            this.Controls.Add(this.btn_Consult);
            this.Controls.Add(this.btn_NumbertoTransfer);
            this.Controls.Add(this.btn_CallOut);
            this.Controls.Add(this.btn_Answer);
            this.Controls.Add(this.btn_Ready);
            this.Controls.Add(this.btn_SignIn);
            this.Controls.Add(this.btn_Init);
            this.Controls.Add(this.btn_Load);
            this.Controls.Add(this.axUniSoftPhone1);
            this.Name = "VoiceSample";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VoiceSample";
            this.Load += new System.EventHandler(this.VoiceSample_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axUniSoftPhone1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Load;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBox_AgentID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBox_Skill;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBox_DN;
        private System.Windows.Forms.Button btn_UNLoad;
        private System.Windows.Forms.Button btn_Init;
        private System.Windows.Forms.Button btn_SignIn;
        private System.Windows.Forms.Button btn_SignOut;
        private System.Windows.Forms.Button btn_Ready;
        private System.Windows.Forms.Button btn_Busy;
        private System.Windows.Forms.Button btn_ACW_Start;
        private System.Windows.Forms.Button btn_ACW_End;
        private System.Windows.Forms.Button btn_Rest;
        private System.Windows.Forms.Button btn_Rest_End;
        private System.Windows.Forms.Button btn_Answer;
        private System.Windows.Forms.Button btn_Release;
        private System.Windows.Forms.Button btn_Mute;
        private System.Windows.Forms.Button btn_Cancel_Mute;
        private System.Windows.Forms.TextBox txtBox_NumbertoDial;
        private System.Windows.Forms.TextBox txtBox_ListenAgentDN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labe24;
        private System.Windows.Forms.Button btn_CallOut;
        private System.Windows.Forms.Button btn_NumbertoTransfer;
        private System.Windows.Forms.Button btn_CancelTransferCall;
        private System.Windows.Forms.Button btn_HelpTransferCall;
        private System.Windows.Forms.Button btn_Consult;
        private System.Windows.Forms.Button btn_CancelConsult;
        private System.Windows.Forms.Button btn_EndConsult;
        private System.Windows.Forms.Button btn_ForwardCall;
        private System.Windows.Forms.Button btn_Consult2Conference;
        private System.Windows.Forms.Button btn_Conference;
        private System.Windows.Forms.Button btn_Cancel_Conference;
        private System.Windows.Forms.Button btn_End_Conference;
        private System.Windows.Forms.Button btn_SendDTMF;
        private System.Windows.Forms.Button btn_Listen;
        private System.Windows.Forms.Button btn_ForceInsert;
        private System.Windows.Forms.Button btn_DisconnectCall;
        private System.Windows.Forms.Button btn_Intercept;
        private System.Windows.Forms.Button btn_ForceLogout;
        private System.Windows.Forms.Button btn_ForceSetBusy;
        private System.Windows.Forms.Button btn_ForceSetIdle;
        private AxUniSoftPhoneControl.AxUniSoftPhone axUniSoftPhone1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_ConDeviceType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_rest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_TransferCall;
        private System.Windows.Forms.Button btn_TransferCall2;
        private System.Windows.Forms.Button btn_QueryIdleAgents;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGetCTIData;
        private System.Windows.Forms.TextBox txtCTIDataValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_CTIDataKey;
    }
}