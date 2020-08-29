using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM2014.Core
{
    public class WaitAgentAlloc
    {
        /// <summary>
        /// 网友标识
        /// </summary>
        private string userid;
        public string UserID
        {
            get
            {
                return userid;
            }
            set
            {
                userid = value;
            }
        }
        /// <summary>
        /// 等待开始时间
        /// </summary>
        private DateTime waitbegintime;
        public DateTime WaitBeginTime
        {
            get
            {
                return waitbegintime;
            }
            set
            {
                waitbegintime = value;
            }
        }
        /// <summary>
        /// 等待结束时间
        /// </summary>
        private DateTime waitendtime;
        public DateTime WaitEndTime
        {
            get
            {
                return waitendtime;
            }
            set
            {
                waitbegintime = value;
            }
        }
        /// <summary>
        /// 是否是等待状态，0是等待，1是等待结束
        /// </summary>
        private int waitstatus;
        public int WaitStatus
        {
            get
            {
                return waitstatus;
            }
            set
            {
                waitstatus = value;
            }
        }
    }
}
