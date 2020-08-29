using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_2015.MainInterface;

namespace BitAuto.DSC.IM_2015.MainWindowsService
{

    public class BusinessWaiteQueueInfo
    {
        //public static readonly object InnerLocker = new object();
        public string Name;
        public string Value;

        /// <summary>
        /// 当前总的排队编号
        /// </summary>
        public  long WaitNum = 0; 
        //最大排队量
        //public int MaxWaiteNum;

        //true:运行中，false：停止
        //0:false,1 true
        private int _isBusy = 0;


        /// <summary>
        /// 判断如果当前状态为不忙时，设置为忙，并且返回true;
        /// </summary>
        /// <returns></returns>
        public bool Check_SetBusy()
        {
            return Interlocked.CompareExchange(ref _isBusy, 1, 0) == 0;
        }


        /// <summary>
        /// 如果当前状态为忙时设置为不忙。
        /// </summary>
        public void SetIdle()
        {
            Interlocked.CompareExchange(ref _isBusy, 0, 1);
        }
        /// <summary>
        /// 本条业务线下的等待队列
        /// </summary>
        public ConcurrentDictionary<long, ProxyNetFriend> DicWaitNetFriends = new ConcurrentDictionary<long, ProxyNetFriend>();

        public int GetListWaiteAgentsCount()
        {
            return this.DicWaitNetFriends.Count;

        }
    }
}
