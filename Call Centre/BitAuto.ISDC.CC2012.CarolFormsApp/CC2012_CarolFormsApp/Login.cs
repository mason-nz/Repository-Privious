using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RECONCOMLibrary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;

namespace CC2012_CarolFormsApp
{
    public partial class Login : Form
    {
        //public static string UserName = string.Empty;
        public static bool IsBindRecord = true;
        public static bool IsForceLogin = false;
        private bool IsTestLogin = bool.Parse(System.Configuration.ConfigurationSettings.AppSettings["IsTestLogin"].ToString());
        public Login()
        {
            //ExecuteRegvr32();
            SyncSeverTime();
            InitializeComponent();
            txtUserName.Text = LoadLoginUserNameFromLocalXML();
            txtDomainAccount.Text = LoadLoginDomainAccountFromLocalXML();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form_KeyDown);

            if (IsTestLogin)
            {
                label4.Visible = true;
                ckxIsForceLogin.Visible = true;
            }
        }

        //同步西门服务器时间到客户端机器
        private void SyncSeverTime()
        {
            SYSTimeHelper.ClientB();
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;   //将Handled设置为true，指示已经处理过KeyPress事件
                btnLogin_Click(null, null);
            }
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            Loger.Log4Net.Info("[Login]btnLogin_Click begin...");
            try
            {
                string organizationService = ConfigurationManager.AppSettings["OrganizationService"];
                IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                                   organizationService);
                string userName = txtUserName.Text.Trim();
                string domainAccount = txtDomainAccount.Text.Trim();
                string password = txtPwd.Text.Trim();
                Loger.Log4Net.Info("[Login]btnLogin_Click service.Login(domainAccount, password) begin...");
                LoginResult loginResult = service.Login(domainAccount, password);
                if (loginResult == LoginResult.Success)
                {
                    LoginUser.DomainAccount = domainAccount;
                    LoginUser.Password = password;
                    Employee eModel = service.GetEmployeeByDomainAccount(domainAccount);
                    
                    if (eModel != null)
                    {
                        SqlTool tool = new SqlTool();
                        //LoginUser.AgentID = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetUserIDByNameDomainAccount("lihefeng");
                        LoginUser.UserID = tool.GetUserIDByNameDomainAccount(domainAccount);
                        LoginUser.EmployeeID = eModel.EmployeeID;
                        LoginUser.Department = eModel.Department;
                        LoginUser.TrueName = eModel.CnName;
                        LoginUser.ExtensionNum = userName;                                                
                        //LoginUser.GroupName = tool.GetUserGroupNamesStr(Convert.ToInt32(LoginUser.AgentID));

                        string str = tool.GetBGNameAndOutNum(Convert.ToInt32(LoginUser.UserID));
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArray = str.Split(',');

                            LoginUser.BGID = strArray[0];
                            LoginUser.GroupName = strArray[1];
                            LoginUser.OutNum = strArray[2];
                            if (string.IsNullOrEmpty(strArray[3]))
                            {
                                MessageBox.Show("坐席工号未设置,请联系管理员！");
                                Loger.Log4Net.Info("[Login]坐席登录 坐席工号未设置 AgentID is:" + LoginUser.UserID + "！");
                                return;
                            }
                            else
                            {
                                LoginUser.AgentNum = strArray[3];
                            }
                            
                        }
                        else
                        {
                            Loger.Log4Net.Info("[Login]btnLogin_Click...AgentID is:" + LoginUser.UserID +",所属业务组为空！");
                        }
                        Loger.Log4Net.Info("[Login]坐席登录...AgentID is:"+ LoginUser.UserID + ",AgentNum is:"+ LoginUser.AgentNum +",GroupName is:"+ LoginUser.GroupName);
                    }
                    //DomainAccount = domainAccount;
                    //Password = password;
                }
                else
                {
                    MessageBox.Show("密码错误,请联系管理域帐号的网管！");
                    return;
                }
                if (string.IsNullOrEmpty(userName))
                {
                    MessageBox.Show("请输入坐席分机号码！");
                }
                else
                {
                    Loger.Log4Net.Info("[Login]btnLogin_Click CarolHelper.Instance.Login begin...");
                    int r = CarolHelper.Instance.Login(Program.rc, userName, true);
                    if (r == (int)enErrorCode.E_SUCCESS)
                    {
                        SaveLoginInfoToLocalXML(userName, domainAccount);
                        //LoginUser.ExtensionNum = userName;
                        Loger.Log4Net.Info("[Login]btnLogin_Click LoginUser.ExtensionNum is:" + LoginUser.ExtensionNum + ",LoginUser.UserID is:" + LoginUser.EmployeeID);
                        this.Hide();
                        Main main = new Main();
                        main.Show();
                    }
                    else if (r == (int)enErrorCode.E_TSV_REJECT)
                    {
                        Loger.Log4Net.Info("[Login]btnLogin_Click CarolHelper.Instance.Login is enErrorCode.E_TSV_REJECT...");
                        if (IsForceLogin)
                        {
                            Loger.Log4Net.Info("[Login]btnLogin_Click CarolHelper.Instance.Login 坐席分机强制登录开始...");
                            CarolHelper.Instance.Clear(Program.rc, userName);
                            r = CarolHelper.Instance.Login(Program.rc, userName, true);
                            if (r == (int)enErrorCode.E_SUCCESS)
                            {
                                Loger.Log4Net.Info("[Login]btnLogin_Click CarolHelper.Instance.Login 坐席分机登录成功...");
                                SaveLoginInfoToLocalXML(userName, domainAccount);
                                LoginUser.ExtensionNum = userName;
                                this.Hide();
                                Main main = new Main();
                                main.Show();
                            }
                            else
                            {
                                Loger.Log4Net.Info("[Login]btnLogin_Click CarolHelper.Instance.Login 坐席分机强制登录失败..."+ r);
                            }
                        }
                        else
                        {
                            Loger.Log4Net.Info("[Login]btnLogin_Click CarolHelper.Instance.Login 坐席分机已登录，不能重复登录...");
                            MessageBox.Show("坐席分机已登录，不能重复登录");
                        }
                    }
                    //else if (r == (int)enErrorCode.E_TSV_REJECT)
                    //{
                    //    //MessageBox.Show("坐席分机已登录，不能重复登录");
                    //    CarolHelper.Instance.Clear(Program.rc, userName);
                    //    r = CarolHelper.Instance.Login(Program.rc, userName, true);
                    //    if (r == (int)enErrorCode.E_SUCCESS)
                    //    {
                    //        SaveLoginUserNameToLocalXML(userName);
                    //        UserName = userName;
                    //        this.Hide();
                    //        Main main = new Main();
                    //        main.Show();
                    //    }
                    //}
                    else if (r == -1)
                    {
                        MessageBox.Show("宇高录音登录失败");
                        //CarolHelper.Instance.Clear(Program.rc, userName);
                    }
                }

                this.txtUserName.Focus();
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Login]btnLogin_Click errorMessage is:" + ex.Message);
                Loger.Log4Net.Info("[Login]btnLogin_Click errorSource is:" + ex.Source);
                Loger.Log4Net.Info("[Login]btnLogin_Click errorStackTrace is:" + ex.StackTrace);
            }
        }

        private void ExecuteRegvr32()
        {
            try
            {
                //反注册旧COM组件
                //Regsvr32COM(@"C:\Windows\system32\ReconCOMDLL.dll", "/u /s ");
                Regsvr32COM(@"C:\Windows\ReconCOMDLL.dll", "/u /s ");

                Loger.Log4Net.Info("[Login]反注册Windows目录COM组件成功!");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Login]反注册Windows目录COM组件失败 ErrorMsg:" + ex.Message);
                Loger.Log4Net.Info("[Login]反注册Windows目录COM组件失败 ErrorStackTrace:" + ex.StackTrace);
            }

            try
            {
                //反注册旧COM组件
                Regsvr32COM(@"C:\Windows\System32\ReconCOMDLL.dll", "/u /s ");

                Loger.Log4Net.Info("[Login]反注册System32目录COM组件成功!");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Login]反注册System32目录COM组件失败 ErrorMsg:" + ex.Message);
                Loger.Log4Net.Info("[Login]反注册System32目录COM组件失败 ErrorStackTrace:" + ex.StackTrace);
            }
            

            //注册新COM组件
            string sourceFile = System.Environment.CurrentDirectory;
            sourceFile += "\\ReconCOMDLL.dll";
            Loger.Log4Net.Info("[Login]ExecuteRegvr32 sourceFile is:" + sourceFile + "!");
            try
            {
                //if (!System.IO.File.Exists(@"C:\Windows\ReconCOMDLL.dll"))
                {
                   System.IO.File.Copy(@sourceFile, @"C:\Windows\ReconCOMDLL.dll", true);
                }
                Regsvr32COM(@"C:\Windows\ReconCOMDLL.dll", "/s ");
                Loger.Log4Net.Info("[Login]注册新COM组件成功!");
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[Login]注册新COM组件失败 ErrorMsg:" + ex.Message);
                Loger.Log4Net.Info("[Login]注册新COM组件失败 ErrorStackTrace:" + ex.StackTrace);
            }                       
        }

        private void Regsvr32COM(string FilePath,string parameters)
        {

            Loger.Log4Net.Info("[Login]Regsvr32COM FilePath is:" + FilePath + "!");
            if (System.IO.File.Exists(FilePath))
            {
                Loger.Log4Net.Info("[Login]Regsvr32COM 找到注册COM组件...");                
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = "Regsvr32.exe";
                    process.StartInfo.UseShellExecute = false; //是否使用操作系统外壳启动该程序
                    process.StartInfo.CreateNoWindow = false;          //不再新窗口显示该值

                    //这里相当于传参数 
                    process.StartInfo.Arguments = parameters + FilePath;//"/s "
                    process.Start();

                    //测试同步执行 
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Info("[Login]Regsvr32COM ErrorMsg:" + ex.Message);
                    Loger.Log4Net.Info("[Login]Regsvr32COM ErrorStackTrace:" + ex.StackTrace);
                }

            }
            else
            {
                Loger.Log4Net.Info("[Login]Regsvr32COM 未找到COM组件...");
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

        private void panel4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
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


        private string LoadLoginUserNameFromLocalXML()
        {
            string loginUserName = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "UserConfig.xml");
            XmlNode root = doc.SelectSingleNode("Userconfig/UserName");
            if (root != null)
            {
                loginUserName = root.InnerText;
            }
            return loginUserName;
        }

        private string LoadLoginDomainAccountFromLocalXML()
        {
            string loginDomainAccount = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "UserConfig.xml");
            XmlNode root = doc.SelectSingleNode("Userconfig/DomainAccount");
            if (root != null)
            {
                loginDomainAccount = root.InnerText;
            }
            return loginDomainAccount;
        }

        private void SaveLoginInfoToLocalXML(string userName, string domainAccount)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "UserConfig.xml");
            XmlNode root = doc.SelectSingleNode("Userconfig/UserName");
            if (root == null)
            {
                //保存坐席号
                XmlElement ele = doc.CreateElement("UserName");
                ele.InnerText = userName;
                doc.SelectSingleNode("Userconfig").AppendChild(ele);
            }
            else
            {
                root.InnerText = userName;
            }

            XmlNode rootDomainAccount = doc.SelectSingleNode("Userconfig/DomainAccount");
            if (rootDomainAccount == null)
            {
                //保存域账号
                XmlElement eleDomain = doc.CreateElement("DomainAccount");
                eleDomain.InnerText = domainAccount;
                doc.SelectSingleNode("Userconfig").AppendChild(eleDomain);
            }
            else
            {
                rootDomainAccount.InnerText = domainAccount;
            }

            doc.Save(AppDomain.CurrentDomain.BaseDirectory + "UserConfig.xml");
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = global::CC2012_CarolFormsApp.Properties.Resources.Login_btnhr;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = global::CC2012_CarolFormsApp.Properties.Resources.Login_btn;

        }

        private void ckxIsForceLogin_CheckedChanged(object sender, EventArgs e)
        {
            IsForceLogin = ((CheckBox)sender).Checked;
        }

    }



    public static class LoginUser
    {
        private static string _domainAccount = string.Empty;
        /// <summary>
        /// 登录域账号
        /// </summary>
        public static string DomainAccount
        {
            get { return _domainAccount; }
            set { _domainAccount = value; }
        }

        private static string _password = string.Empty;
        /// <summary>
        /// 登录域账号密码
        /// </summary>
        public static string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private static int? employeeid = null;
        /// <summary>
        /// 登录者ID
        /// </summary>
        public static int? EmployeeID
        {
            get { return employeeid; }
            set { employeeid = value; }
        }

        private static string _AgentNum = string.Empty;
        /// <summary>
        /// 坐席工号，即西门子CTI 帐号
        /// </summary>
        public static string AgentNum
        {
            get { return _AgentNum; }
            set { _AgentNum = value; }
        }

        private static string _extensionNum = string.Empty;
        /// <summary>
        /// 登录分机号码
        /// </summary>
        public static string ExtensionNum
        {
            get { return _extensionNum; }
            set { _extensionNum = value; }
        }

        private static Department _department = null;
        /// <summary>
        /// 登录者所在部门
        /// </summary>
        public static Department Department
        {
            get { return _department; }
            set { _department = value; }
        }

        private static string _trueName = string.Empty;
        /// <summary>
        /// 登录者真实姓名
        /// </summary>
        public static string TrueName
        {
            get { return _trueName; }
            set { _trueName = value; }
        }

        private static string _groupName = string.Empty;
        /// <summary>
        /// 登录者所属分组名
        /// </summary>
        public static string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        private static string _BGID = string.Empty;
        /// <summary>
        /// 登录者所属分组ID
        /// </summary>
        public static string BGID
        {
            get { return _BGID; }
            set { _BGID = value; }
        }

        private static string _OutNum = string.Empty;
        /// <summary>
        /// 登录者所属分组外呼出局号
        /// </summary>
        public static string OutNum
        {
            get { return _OutNum; }
            set { _OutNum = value; }
        }

        private static int? userid = null;
        /// <summary>        
        /// 因为原先userID获取后，无法取到所属分组信息
        /// 担心其它地方用到影响之前逻辑，所以新增agentID,从集成权限系统获取
        /// CC客服系统帐号
        /// </summary>
        public static int? UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        private static int? _LoginOnOid = null;
        /// <summary>        
        /// 坐席登录时的登录状态明细记录的唯一标识
        /// 退出时，根据标识列新登录状态结束时间
        /// </summary>
        public static int? LoginOnOid
        {
            get { return _LoginOnOid; }
            set { _LoginOnOid = value; }
        }

        private static DateTime _EndTime;
        /// <summary>        
        /// 坐席登录时的登录状态明细记录的唯一标识
        /// 退出时，根据标识列新登录状态结束时间
        /// </summary>
        public static DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
    }
}
