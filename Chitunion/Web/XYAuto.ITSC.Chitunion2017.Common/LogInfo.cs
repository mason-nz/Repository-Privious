using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace XYAuto.ITSC.Chitunion2017.Common
{
    public class LogInfo
    {
        public static readonly LogInfo Instance = new LogInfo();

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="LogModuleID">系统模块编号</param>
        /// <param name="ActionType">操作类型</param>
        /// <param name="Content">操作内容</param>
        public void InsertLog(int LogModuleID, int ActionType, string Content)
        {
            try
            {
                //if (LogModuleID.Length > 50)
                //{
                //    LogModuleID = LogModuleID.Substring(0, 50);
                //}
                if (Content.Length > 2000)
                {
                    Content = Content.Substring(0, 2000);
                }
                string IP = "";
                try
                {
                    IP = GetIPAddress();//System.Web.HttpContext.Current.Request.ServerVariables["remote_addr"];
                }
                catch { }
                int UserID = UserInfo.GetLoginUserID();
                DateTime CreateTime = DateTime.Now;
                Dal.LogInfo.Instance.InsertLog(LogModuleID, ActionType, Content, UserID, IP, CreateTime);
            }
            catch (Exception)
            {
                //写错误日志到日志文件
            }
        }

        public void InsertLog(LogModuleType mtype, ActionType atype, string Content) {
       
            InsertLog((int)mtype, (int)atype, Content);
        }

        /// <summary>
        /// 获取当前发出请求的客户端[IP]地址
        /// </summary>
        /// <returns></returns>
        public static String GetIPAddress()
        {
            string s_UserIp = "";
            HttpRequest f_Request = HttpContext.Current.Request;

            // 如果使用代理，获取真实[IP]
            if (f_Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty)
            {
                s_UserIp = f_Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                s_UserIp = f_Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (s_UserIp == null || s_UserIp == String.Empty)
            {
                s_UserIp = f_Request.UserHostAddress;
            }
            return s_UserIp;
        }


        /// <summary>
        /// 操作模块
        /// </summary>
        public enum LogModuleType
        {
            /// <summary>
            /// 账号管理
            /// </summary>
            账号管理 = 17001,
            /// <summary>
            /// 媒体管理
            /// </summary>
            媒体管理 = 17002,
            /// <summary>
            /// 刊例管理
            /// </summary>
            刊例管理 = 17003,
            /// <summary>
            /// 订单管理
            /// </summary>
            订单管理 = 17004,
            /// <summary>
            /// 标签管理
            /// </summary>
            标签管理 = 17005,
            /// <summary>
            /// 角色权限管理
            /// </summary>
            角色权限管理 = 17006
        }


        /// <summary>
        /// 操作模块
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// 增加
            /// </summary>
            Add = 1,
            /// <summary>
            /// 删除
            /// </summary>
            Delete = 2,
            /// <summary>
            /// 修改
            /// </summary>
            Modify = 3,
            /// <summary>
            /// 查询
            /// </summary>
            Select = 4
        }

    }
}
