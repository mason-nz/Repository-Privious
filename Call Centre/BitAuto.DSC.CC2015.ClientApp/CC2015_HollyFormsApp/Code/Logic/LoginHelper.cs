using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;
using System.Windows.Forms;

namespace CC2015_HollyFormsApp
{
    public class LoginHelper
    {
        //上一个AgentStateDetail表数据ID
        public static int PreOid = -1;
        public static readonly LoginHelper Instance = new LoginHelper();

        protected LoginHelper()
        {

        }

        /// 根据域账号、密码及分机，登录客服客户端
        /// <summary>
        /// 根据域账号、密码及分机，登录客服客户端
        /// 1、先验证域账号
        /// 2、根据UserID，去找CTI中客服ID
        /// 3、再根据客服ID，注册分机并登录，然后绑定宇高录音
        /// </summary>
        /// <param name="domainAccount">域账号</param>
        /// <param name="pwd">域账号密码</param>
        /// <param name="extension">分机号码</param>
        /// <returns>若验证成功返回True，否则返回False</returns>
        public bool Login(string domainAccount, string pwd, string extension, out string errorMsg)
        {
            bool flag = false;
            LoginUser.isLoggedIn = false;
            errorMsg = string.Empty;
            Loger.Log4Net.Info(string.Format("[登录登出模块] Login ( {0}, *** , {1} ) ", domainAccount, extension));

            //域账号和域密码校验
            Employee model = DomainAccountLogin(domainAccount, pwd, out errorMsg);
            if (model == null)
            {
                //用户名&密码校验失败
                Loger.Log4Net.Info("[登录登出模块] DomainAccountLogin=用户名&密码校验失败");
                return false;
            }

            //调用HR接口service.GetEmployeeByDomainAccount(domainAccount)返回model中的Password是默认密码：123.abc，需要重新赋值下
            model.Password = pwd;
            //根据Employee实体，初始化客服信息如（域账号、客服ID、所属分组、分机、部门、真实姓名等）
            if (InitLoginUser(model, extension, out errorMsg) == false)
            {
                Loger.Log4Net.Info(string.Format("[登录登出模块] Login({0}, pwd, {1}) End.", domainAccount, extension));
                return false;
            }

            //启动客户端主连接
            //签入
            if (HollyContactHelper.Instance.SignIn(LoginUser.AgentNum, LoginUser.ExtensionNum, out errorMsg))
            {
                LoginHelper.Instance.InsertAgentState();
                LoginUser.isLoggedIn = true;
                flag = true;
            }
            else
            {
                //签入失败
                return false;
            }
            Loger.Log4Net.Info(string.Format("[登录登出模块] Login({0}, pwd, {1}) End.", domainAccount, extension));
            return flag;
        }
        /// 插入客服实时监控表逻辑
        /// <summary>
        /// 插入客服实时监控表逻辑
        /// </summary>
        /// <param name="errorMsg">报错信息</param>
        /// <returns>成功返回True,否则返回False</returns>
        public bool InsertAgentState()
        {
            //登录成功后，插入状态到临时表
            DateTime tdate = Common.GetCurrentTime();
            //登录后，首先进入置忙
            if (AgentTimeStateHelper.Instance.InsertAgentState2DB(AgentState.AS4_置忙, BusyStatus.BS0_自动, Calltype.C0_未知, 0, tdate))
            {
                //数据自检：上次签入的结束时间错误时，赋值当前时间
                string msg = AgentTimeStateHelper.Instance.UpdateLoginOffTime(LoginUser.UserID.ToString(), tdate);
                Loger.Log4Net.Info("[登录登出模块] InsertAgentState()更新退出时间...." + msg);
                //插入一条签入数据，记录id，退出时更新结束时间
                LoginUser.LoginOnOid = AgentTimeStateHelper.Instance.InsertAgentStateDetail2DB(AgentState.AS2_签入, BusyStatus.BS0_自动, Calltype.C0_未知, tdate, tdate);
                Loger.Log4Net.Info("[登录登出模块] InsertAgentState()登录状态LoginOnOid....is:" + LoginUser.LoginOnOid);
                return true;
            }
            else
            {
                string errorMsg = "登录成功后，插入状态到临时表失败";
                Loger.Log4Net.Info("[登录登出模块] InsertAgentState()...." + errorMsg);
                return false;
            }
        }
        /// 删除客服实时监控表逻辑
        /// <summary>
        /// 删除客服实时监控表逻辑
        /// </summary>
        /// <param name="errorMsg">报错信息</param>
        /// <returns>成功返回True,否则返回False</returns>
        public bool DeleteAgentStateToDB(ref string errorMsg)
        {
            //退出后，删除状态临时表记录
            if (!AgentTimeStateHelper.Instance.DeleteAgentStateToDB())
            {
                errorMsg = "登录退出后，插入状态到临时表失败";
                Loger.Log4Net.Info("[登录登出模块]DeleteAgentStateToDB..." + errorMsg);
                return false;
            }
            else
            {
                //记录客服退出状态时间
                DateTime tdate = Common.GetCurrentTime();
                //插入一条签出数据
                AgentTimeStateHelper.Instance.InsertAgentStateDetail2DB(AgentState.AS1_签出, BusyStatus.BS0_自动, Calltype.C0_未知, tdate, tdate);
                Loger.Log4Net.Info("[登录登出模块]DeleteAgentStateToDB...客服退出时 LoginOnOid....IS:" + LoginUser.LoginOnOid);
                Loger.Log4Net.Info("[登录登出模块]DeleteAgentStateToDB...客服退出时 GetCurrentTime()....IS:" + tdate.ToString("yyyy-MM-dd HH:mm:ss"));
                //更新签入数据的结束时间
                if (LoginUser.LoginOnOid != null)
                {
                    AgentTimeStateHelper.Instance.UpdateStateDetail2DBAsync(Convert.ToInt32(LoginUser.LoginOnOid), tdate);
                    LoginUser.LoginOnOid = null;
                }

                Loger.Log4Net.Info("[登录登出模块]DeleteAgentStateToDB...退出成功....");
                return true;
            }

        }

        /// 根据Employee实体，初始化客服信息如（域账号、客服ID、所属分组、分机、部门、真实姓名等）
        /// <summary>
        /// 根据Employee实体，初始化客服信息如（域账号、客服ID、所属分组、分机、部门、真实姓名等）
        /// </summary>
        /// <param name="model">Employee实体</param>
        /// <param name="extension">分机号码</param>
        private bool InitLoginUser(Employee model, string extension, out string errormsg)
        {
            errormsg = "";
            Loger.Log4Net.Info("[登录登出模块] InitLoginUser() Begin...");
            LoginUser.DomainAccount = model.DomainAccount;
            LoginUser.Password = model.Password;
            LoginUser.UserID = AgentTimeStateHelper.Instance.GetUserIDByNameDomainAccount(model.DomainAccount);
            if (LoginUser.UserID == null || LoginUser.UserID.Value < 0)
            {
                Loger.Log4Net.Info("[登录登出模块] InitLoginUser() LoginUser.UserID：" + LoginUser.UserID);
                errormsg = "通过域账户名称，在集中权限系统中无法找到此人";
                return false;
            }
            LoginUser.Department = model.Department;
            LoginUser.TrueName = model.CnName;
            LoginUser.ExtensionNum = extension;
            string str = AgentTimeStateHelper.Instance.GetBGNameAndOutNum(Convert.ToInt32(LoginUser.UserID));
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = str.Split(',');

                LoginUser.BGID = strArray[0];
                LoginUser.GroupName = strArray[1];
                LoginUser.OutNum = strArray[2];
                if (string.IsNullOrEmpty(strArray[3]))
                {
                    errormsg = "客服工号未设置,请联系管理员";
                    Loger.Log4Net.Info("[登录登出模块] InitLoginUser()客服登录 客服工号未设置 AgentID is:" + LoginUser.UserID + "！");
                    return false;
                }
                else
                {
                    LoginUser.AgentNum = strArray[3];
                }
                Loger.Log4Net.Info("[登录登出模块] InitLoginUser() 客服登录...AgentID is:" + LoginUser.UserID + ",AgentNum is:" + LoginUser.AgentNum + ",GroupName is:" + LoginUser.GroupName + ",OutNum is:" + LoginUser.OutNum);
            }
            else
            {
                Loger.Log4Net.Info("[登录登出模块] InitLoginUser() AgentID is:" + LoginUser.UserID + ",所属业务组为空！");
            }
            Loger.Log4Net.Info("[登录登出模块] InitLoginUser() End...");
            return true;
        }
        /// 域账号验证（调用组织架构接口）
        /// <summary>
        /// 域账号验证（调用组织架构接口）
        /// </summary>
        /// <param name="domainAccount">域账号</param>
        /// <param name="pwd">域账号密码</param>
        /// <returns>若验证成功返回Employee实体，否则返回NULL</returns>
        private Employee DomainAccountLogin(string domainAccount, string pwd, out string errormsg)
        {
            errormsg = "";
            Loger.Log4Net.Info(string.Format("[登录登出模块] DomainAccountLogin({0}, pwd) begin...", domainAccount));
            Employee model = null;
            try
            {
                string organizationService = ConfigurationManager.AppSettings["OrganizationService"];
                IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService), organizationService);

                LoginResult loginResult = service.Login(domainAccount, pwd);
                if (loginResult != LoginResult.Success)
                {
                    errormsg = "密码错误,请联系管理域帐号的网管";
                }
                else
                {
                    model = service.GetEmployeeByDomainAccount(domainAccount);
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[登录登出模块] DomainAccountLogin()", ex);
            }

            Loger.Log4Net.Info(string.Format("[登录登出模块] DomainAccountLogin({0}, pwd) end.return {1}", domainAccount, model));
            return model;
        }

        /// 客服异常退出前，要进行更新状态明细操作
        /// <summary>
        /// 客服异常退出前，要进行更新状态明细操作
        /// </summary>
        /// <param name="e"></param>
        public void ExceptionUpdateAgentState(Exception e)
        {
            Loger.Log4Net.Error("[Main] CC客户端出错...", e);
            DateTime tdate = Common.GetCurrentTime();
            if (LoginUser.LoginOnOid != null)
            {
                Loger.Log4Net.Info("[Main] CC客户端出错 LoginOnOid....IS:" + LoginUser.LoginOnOid);
                AgentTimeStateHelper.Instance.UpdateStateDetail2DBAsync(Convert.ToInt32(LoginUser.LoginOnOid), tdate);
                LoginUser.LoginOnOid = null;
            }
            if (LoginUser.UserID != null)
            {
                AgentTimeStateHelper.Instance.DeleteAgentStateToDB();
            }
            Common.SendErrorEmail("西安CC客户端（" + Common.GetVersion() + "）出错", e.Message, e.Source, e.StackTrace);
        }
        /// 退出登录
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public bool ExitLogin()
        {
            bool flag = false;
            string errorMsg = string.Empty;
            Loger.Log4Net.Info("[登录登出模块] ExitLogin() Begin...");

            if ((int)Main.Main_PhoneStatus > 6)
            {
                MessageBox.Show("当前状态下不能退出！");
                return false;
            }

            if (LoginUser.isLoggedIn)
            {
                //修改数据库
                LoginHelper.Instance.DeleteAgentStateToDB(ref errorMsg);
                //签出
                HollyContactHelper.Instance.SignOut();
                //更新话务数据
                if (Main.Main_PhoneStatus == PhoneStatus.PS06_话后 && BusinessProcess.CallRecordORIG != null)
                {
                    Loger.Log4Net.Info("[==BusinessProcess=][UpdateCallRecordAfterTime] 签出前更新话后信息");
                    BusinessProcess.Instance.UpdateCallRecordAfterTime();
                }
                //更新上一个状态
                SingOutUpdateDate(Common.GetCurrentTime());
            }
            //卸载
            HollyContactHelper.Instance.UnLoad();
            LoginUser.isLoggedIn = false;
            flag = true;
            Loger.Log4Net.Info("[登录登出模块] ExitLogin() End.");
            return flag;
        }

        /// 签出之后更新上一个状态的时间
        /// <summary>
        /// 签出之后更新上一个状态的时间
        /// </summary>
        /// <param name="date"></param>
        public void SingOutUpdateDate(DateTime date)
        {
            if (LoginHelper.PreOid != -1)
            {
                //更新AgentStateDetail表（更新上一个状态的结束时间）
                AgentTimeStateHelper.Instance.UpdateStateDetail2DBAsync(LoginHelper.PreOid, date);
                Loger.Log4Net.Info("[Main] 更新上一个状态的结束时间 PreOid....IS:" + LoginHelper.PreOid);
                //签出之后，PreOid置-1，表示一个状态转换一个周期完成
                LoginHelper.PreOid = -1;
            }
        }
    }
}
