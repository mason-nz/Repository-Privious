using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;
using BitAuto.ISDC.CC2012.Entities;
using System.IO;

namespace CC2015_HollyFormsApp
{
    public partial class Login : Form
    {
        public static bool IsBindRecord = true;
        public static bool IsForceLogin = false;
        private int loadConfigCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["LoadConfigCount"]);
        private int currentLoadConfigCount = 0;//当前调用load函数后，返回服务器消息数量
        Timer TimerLogin = new Timer();
        private int DelayLogin = 5;

        public Login()
        {
            InitializeComponent();
            txtExtensionName.Text = Common.LoadNodeStrFromLocalXML(Constant.ClientLocalConfigXMLName, Constant.ClientLocalConfigExtensionNode);
            txtDomainAccount.Text = Common.LoadNodeStrFromLocalXML(Constant.ClientLocalConfigXMLName, Constant.ClientLocalConfigADNameNode);

            //加载电话控件
            HollyContactHelper.AxUniSoftPhone = this.axUniSoftPhone1;
            HollyContactHelper.AxUniSoftPhone.OnStatusChange += new EventHandler(AxUniSoftPhone_OnStatusChange);

            //标题
            this.label2.Text = "易车客服中心管理系统（Holly）" + Common.GetVersion();

            this.Load += new EventHandler(Login_Load);

            TimerLogin.Enabled = false;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //更新（AutoUpdate）
            UpdateAutoUpdateEXE();
            //下载mdb模板
            MDBFileHelper.Instance.DownLoadMdbFile();
            //密码
            string pwd = Common.LoadNodeStrFromLocalXML(Constant.ClientLocalConfigXMLName, Constant.ClientLocalConfigPassWordNode);
            if (!string.IsNullOrEmpty(pwd))
            {
                txtExtensionName.Enabled = false;
                txtDomainAccount.Enabled = false;
                txtPwd.Enabled = false;
                button.Enabled = false;
            }
            else
            {
                AutoLoginInit();
            }
            //界面刷新
            Application.DoEvents();
            //异步 自动登录
            new System.Action(AutoLogin).BeginInvoke(null, null);
        }
        /// 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //分机号码
            string extension = txtExtensionName.Text.Trim();
            //域账号
            string domainAccount = txtDomainAccount.Text.Trim();
            //密码
            string pwd = txtPwd.Text.Trim();

            MainLogin(extension, domainAccount, pwd);
        }
        /// 关闭按钮
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, EventArgs e)
        {
            if (LoginHelper.Instance.ExitLogin())
            {
                System.Environment.Exit(0);
            }
        }

        /// 自动登录
        /// <summary>
        /// 自动登录
        /// </summary>
        private void AutoLogin()
        {
            if (InvokeRequired)
            {
                this.Invoke(new System.Action(AutoLogin));
            }
            else
            {
                //分机号码
                string extension = txtExtensionName.Text.Trim();
                //域账号
                string domainAccount = txtDomainAccount.Text.Trim();

                //分机和域账号存在
                if (!string.IsNullOrEmpty(extension) && !string.IsNullOrEmpty(domainAccount))
                {
                    //重启逻辑
                    //密码
                    string pwd = Common.LoadNodeStrFromLocalXML(Constant.ClientLocalConfigXMLName, Constant.ClientLocalConfigPassWordNode);
                    pwd = BitAuto.ISDC.CC2012.BLL.Util.TryDecryptString(pwd);
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        txtPwd.Text = pwd;
                        MainLogin(extension, domainAccount, pwd);
                    }
                    //自动登录逻辑
                    else
                    {
                        pwd = txtPwd.Text.Trim();
                        if (!string.IsNullOrEmpty(pwd) && cbx_alogin.Checked)
                        {
                            //延时登录
                            TimerLogin.Enabled = true;
                            TimerLogin.Interval = 1000;
                            TimerLogin.Tag = DelayLogin;//延时时间
                            SetErrorZText("自动登录：" + DelayLogin);
                            TimerLogin.Tick += (s, e) =>
                            {
                                int delay = CommonFunction.ObjectToInteger(TimerLogin.Tag) - 1;
                                if (delay >= 1)
                                {
                                    SetErrorZText("自动登录：" + delay);
                                }
                                else
                                {
                                    MainLogin(extension, domainAccount, pwd);
                                }
                                TimerLogin.Tag = delay;
                            };
                        }
                    }
                }
            }
        }

        #region 登录
        /// 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="domainAccount"></param>
        /// <param name="pwd"></param>
        private void MainLogin(string extension, string domainAccount, string pwd)
        {
            TimerLogin.Enabled = false;
            SetErrorZText("");
            SetButtonEnable(false);

            Loger.Log4Net.Info("[登录窗口]btnLogin_Click Begin...");
            if (domainAccount.Trim() == "")
            {
                SetErrorZText("请输入域账号！");
                txtDomainAccount.Focus();
                SetButtonEnable(true);
                return;
            }
            if (pwd.Trim() == "")
            {
                SetErrorZText("请输入密码！");
                txtPwd.Focus();
                SetButtonEnable(true);
                return;
            }
            if (extension.Trim() == "")
            {
                SetErrorZText("请输入分机号码！");
                txtExtensionName.Focus();
                SetButtonEnable(true);
                return;
            }

            //保存分机号码及域账号到xml文件
            Common.SaveLoginInfoToLocalXML(extension, domainAccount);

            //根据分机号设置服务器IP
            if (HollyContactHelper.Instance.ModifySoftphoneINIByDN(CommonFunction.ObjectToInteger(extension)) == false)
            {
                SetErrorZText("分机号不正确，请重新输入！");
                txtExtensionName.Focus();
                SetButtonEnable(true);
                return;
            }
            else
            {
                HollyContactHelper.AxUniSoftPhone.OnMessage -= new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnMessageEventHandler(AxUniSoftPhone_OnMessage);
                HollyContactHelper.AxUniSoftPhone.OnError -= new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnErrorEventHandler(AxUniSoftPhone_OnError);

                HollyContactHelper.AxUniSoftPhone.OnMessage += new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnMessageEventHandler(AxUniSoftPhone_OnMessage);
                HollyContactHelper.AxUniSoftPhone.OnError += new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnErrorEventHandler(AxUniSoftPhone_OnError);

                //启动客户端主连接
                if (!HollyContactHelper.Instance.Load())
                {
                    SetErrorZText("电话控件装载失败！");
                    SetButtonEnable(true);
                    return;
                }
                else
                {
                    //异步加载，回调事件中继续登录
                    Loger.Log4Net.Info("[登录窗口]btnLogin_Click： 异步加载，回调事件中继续登录");
                }
            }
            Loger.Log4Net.Info("[登录窗口]btnLogin_Click End");
        }
        /// 启用或禁用按钮
        /// <summary>
        /// 启用或禁用按钮
        /// </summary>
        /// <param name="en"></param>
        private void SetButtonEnable(bool en)
        {
            button.Enabled = en;
            button.BackgroundImage = en ? global::CC2015_HollyFormsApp.Properties.Resources.Login_btn : global::CC2015_HollyFormsApp.Properties.Resources.graybtn;
            Application.DoEvents();
        }
        /// 继续登录逻辑
        /// <summary>
        /// 继续登录逻辑
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="domainAccount"></param>
        /// <param name="pwd"></param>
        private void ContinueLogin()
        {
            //分机号码
            string extension = txtExtensionName.Text.Trim();
            //域账号
            string domainAccount = txtDomainAccount.Text.Trim();
            //密码
            string pwd = txtPwd.Text.Trim();

            string errormsg = "";
            //登录逻辑
            if (!LoginHelper.Instance.Login(domainAccount, pwd, extension, out errormsg))
            {
                txtExtensionName.Focus();
                if (errormsg != "")
                {
                    SetErrorZText(errormsg);
                    SetButtonEnable(true);
                }
            }
            else
            {
                HollyContactHelper.AxUniSoftPhone.OnMessage -= new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnMessageEventHandler(AxUniSoftPhone_OnMessage);
                HollyContactHelper.AxUniSoftPhone.OnError -= new AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnErrorEventHandler(AxUniSoftPhone_OnError);

                //自动登录数据存储
                AutoLoginInitSave();

                //登录成功
                this.Hide();
                Main main = new Main();
                main.Show();
            }
        }
        /// 更新=更新程序
        /// <summary>
        /// 更新=更新程序
        /// </summary>
        private void UpdateAutoUpdateEXE()
        {
            try
            {
                string updateexe = "CC2015_HollyFormsApp.AutoUpdate.exe";
                string filename = AppDomain.CurrentDomain.BaseDirectory + updateexe + ".bak";
                if (File.Exists(filename))
                {
                    File.Copy(filename, AppDomain.CurrentDomain.BaseDirectory + updateexe, true);
                    File.Delete(filename);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 界面
        /// 设置错误信息
        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="text"></param>
        public void SetErrorZText(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetErrorZText), text);
            }
            else
            {
                this.lb_error.Text = text;
                this.lb_error.Visible = text != "";
                Application.DoEvents();
            }
        }

        #region 标题可拖动
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }
        #endregion

        /// 绘制
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                              this.panel2.ClientRectangle,
                              System.Drawing.Color.FromArgb(221, 221, 221),
                              1,
                              ButtonBorderStyle.Solid,
                              System.Drawing.Color.FromArgb(221, 221, 221),
                              1,
                              ButtonBorderStyle.Solid,
                              System.Drawing.Color.FromArgb(221, 221, 221),
                              1,
                              ButtonBorderStyle.Solid,
                             System.Drawing.Color.FromArgb(221, 221, 221),
                              1,
                              ButtonBorderStyle.Solid);
        }
        /// 登录按钮样式
        /// <summary>
        /// 登录按钮样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_MouseEnter(object sender, EventArgs e)
        {
            this.button.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_btnhr;
        }
        /// 登录按钮样式
        /// <summary>
        /// 登录按钮样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_MouseLeave(object sender, EventArgs e)
        {
            this.button.BackgroundImage = global::CC2015_HollyFormsApp.Properties.Resources.Login_btn;
        }
        #endregion

        #region 厂家事件
        /// 签入之后自动转休息
        /// <summary>
        /// 签入之后自动转休息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnStatusChange(object sender, EventArgs e)
        {
            var pre = HollyContactHelper.ConvertPhoneStatus(HollyContactHelper.Instance.GetPreStatus());
            var cur = HollyContactHelper.ConvertPhoneStatus(HollyContactHelper.Instance.GetCurStatus());
            if ((pre == PhoneStatus.PS01_就绪 || pre == PhoneStatus.PS02_签出) && cur == PhoneStatus.PS04_置忙)
            {
                //签入成功，要自动休息。设置置忙到置闲，设置置闲到休息
                HollyContactHelper.Instance.SetReady();
                HollyContactHelper.Instance.RestStart(BusyStatus.BS0_自动);
            }
            else if ((pre == PhoneStatus.PS01_就绪 || pre == PhoneStatus.PS02_签出) && cur == PhoneStatus.PS03_置闲)
            {
                //签入成功，要自动休息。从置闲转到休息
                HollyContactHelper.Instance.RestStart(BusyStatus.BS0_自动);
            }
        }
        /// 监听load成功事件
        /// <summary>
        /// 监听load成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnMessage(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnMessageEvent e)
        {
            if (e.messageContent.StartsWith("服务器已上线:"))
            {
                currentLoadConfigCount++;
            }
            if (currentLoadConfigCount == loadConfigCount)
            {
                Loger.Log4Net.Info("当前调用Load函数，返回的服务器信息条数为：" + currentLoadConfigCount);
                currentLoadConfigCount = 0;
                ContinueLogin();
            }
        }
        /// 厂家错误信息
        /// <summary>
        /// 厂家错误信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxUniSoftPhone_OnError(object sender, AxUniSoftPhoneControl.IUniSoftPhoneEvents_OnErrorEvent e)
        {
            switch (e.errDesc)
            {
                case "Err_IContact_NoAgentID":
                    SetErrorZText("登录人工号为空");
                    break;
                case "Err_IContact_AgentIDInService":
                    SetErrorZText("登录人工号已被占用");
                    break;
                case "Err_IContact_AgentDNNotExist":
                    SetErrorZText("分机号码不存在");
                    break;
                case "Err_IContact_AgentDNInService":
                    SetErrorZText("分机号码已被占用");
                    break;
                case "Err_IContact_SkillDescError":
                    SetErrorZText("登录人技能组信息错误");
                    break;
                case "Err_IContact_SkillNotExist":
                    SetErrorZText("登录人技能组不存在");
                    break;
                case "Err_IContact_AgentBusy":
                    SetErrorZText("登录人状态不空闲");
                    break;
                case "Err_IContact_DeviceBusy":
                    SetErrorZText("目标设备正忙");
                    break;
                case "Err_IContact_AgentTimerExpired":
                    SetErrorZText("访问合力系统连接超时");
                    break;
                case "Err_IContact_InvalidAgentID":
                    SetErrorZText("登录人工号无效");
                    break;
                default:
                    SetErrorZText(e.errDesc);
                    break;
            }
            SetButtonEnable(true);
        }
        #endregion

        #region 自动登录系统
        /// 自动登录初始化
        /// <summary>
        /// 自动登录初始化
        /// </summary>
        private void AutoLoginInit()
        {
            bool istest = Common.IsTestVersion();
            cbx_alogin.Visible = istest;
            cbx_rpaw.Visible = istest;
            if (istest)
            {
                Dictionary<string, string> dic = AutoLoginHelper.ReadConfig();
                cbx_alogin.Checked = bool.Parse(dic["autologin"]);
                cbx_rpaw.Checked = bool.Parse(dic["remeberpwd"]);
                txtDomainAccount.Text = dic["name"];
                txtPwd.Text = dic["pwd"];
                txtExtensionName.Text = dic["extension"];
            }
            else
            {
                cbx_alogin.Checked = false;
                cbx_rpaw.Checked = false;
            }
            //事件
            cbx_alogin.CheckedChanged += (s, e) =>
            {
                if (cbx_alogin.Checked)
                {
                    cbx_rpaw.Checked = true;
                }
                else
                {
                    TimerLogin.Enabled = false;
                    SetErrorZText("");
                }
            };
            cbx_rpaw.CheckedChanged += (s, e) =>
            {
                if (cbx_rpaw.Checked == false)
                {
                    cbx_alogin.Checked = false;
                }
                else
                {
                    TimerLogin.Enabled = false;
                    SetErrorZText("");
                }
            };
        }
        /// 自动登录数据存储
        /// <summary>
        /// 自动登录数据存储
        /// </summary>
        private void AutoLoginInitSave()
        {
            if (cbx_alogin.Visible && cbx_rpaw.Visible && cbx_rpaw.Checked)
            {
                AutoLoginHelper.SaveConfig(cbx_alogin.Checked, cbx_rpaw.Checked, txtDomainAccount.Text.Trim(), txtPwd.Text.Trim(), txtExtensionName.Text.Trim());
            }
            else
            {
                AutoLoginHelper.SaveConfig(false, false, txtDomainAccount.Text.Trim(), "", txtExtensionName.Text.Trim());
            }
        }
        #endregion
    }
}
