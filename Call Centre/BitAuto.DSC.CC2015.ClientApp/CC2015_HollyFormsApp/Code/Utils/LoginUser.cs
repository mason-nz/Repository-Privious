using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Services.Organization.Remoting;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    public static class LoginUser
    {
        /// <summary>
        /// 客服是否已登录，已登录：true,未登录:false
        /// </summary>
        private static bool _isLoggedIn = false;
        public static bool isLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; }
        }

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

        private static string _AgentNum = string.Empty;
        /// <summary>
        /// 客服工号
        /// </summary>
        public static string AgentNum
        {
            get { return _AgentNum.Trim(); }
            set { _AgentNum = value; }
        }

        private static string _extensionNum = string.Empty;
        /// <summary>
        /// 登录分机号码
        /// </summary>
        public static string ExtensionNum
        {
            get
            {
                return _extensionNum;
            }
            set
            {
                _extensionNum = value;
                int extensionnum = CommonFunction.ObjectToInteger(_extensionNum);
                LoginUser.LoginAreaType = Common.GetLoginAreaType(extensionnum);
            }
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
        /// 集中权限系统中的UserID
        /// </summary>
        public static int? UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        private static int? _LoginOnOid = null;
        /// <summary>        
        /// 客服登录状态的明细数据ID（为了在退出时更新时间所以记录下来）
        /// </summary>
        public static int? LoginOnOid
        {
            get { return _LoginOnOid; }
            set { _LoginOnOid = value; }
        }

        private static DateTime _EndTime;
        /// <summary>        
        /// 客服登录时的登录状态明细记录的唯一标识
        /// 退出时，根据标识列新登录状态结束时间
        /// </summary>
        public static DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        /// 坐席签入区域
        /// <summary>
        /// 坐席签入区域
        /// </summary>
        public static AreaType LoginAreaType { get; set; }
    }
}
