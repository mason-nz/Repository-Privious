using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CC2015_HollyFormsApp
{
    /// 网页发送的命令管理类
    /// <summary>
    /// 网页发送的命令管理类
    /// </summary>
    public class WebCommandManage
    {
        /// 上一个命令
        /// <summary>
        /// 上一个命令
        /// </summary>
        private static string LastCommand = "";
        /// 上一个命令到达时间
        /// <summary>
        /// 上一个命令到达时间
        /// </summary>
        private static DateTime LastArrive = new DateTime();

        private string cmd = "";
        private DateTime arrive = new DateTime();

        public WebCommandManage(string cmd)
        {
            //记录当前命令和当前命令到达时间
            this.cmd = cmd;
            this.arrive = DateTime.Now;
        }

        /// 获取当前有效的命令，如果无效，返回空
        /// <summary>
        /// 获取当前有效的命令，如果无效，返回空
        /// </summary>
        /// <returns></returns>
        public string GetWebCommand()
        {
            if (cmd != LastCommand)
            {
                //和上个命令不相同，有效，可以返回
                LastCommand = cmd;
                LastArrive = arrive;
                return cmd;
            }
            else
            {
                //和上个命令相同，比较间隔时间 1秒
                if ((arrive - LastArrive).TotalSeconds < 1)
                {
                    //命令无效
                    LastCommand = cmd;
                    LastArrive = arrive;
                    return null;
                }
                else
                {
                    //命令有效
                    LastCommand = cmd;
                    LastArrive = arrive;
                    return cmd;
                }
            }
        }
    }
}
