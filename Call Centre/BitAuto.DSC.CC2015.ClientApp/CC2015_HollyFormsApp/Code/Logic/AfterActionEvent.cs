using System;
using System.Collections.Generic;
using System.Text;

namespace CC2015_HollyFormsApp
{
    /// 操作完成之后触发事件实现
    /// <summary>
    /// 操作完成之后触发事件实现
    /// </summary>
    public class AfterActionEvent
    {
        /// 事件是否有效
        /// <summary>
        /// 事件是否有效
        /// </summary>
        private bool hasEvent = false;
        /// 状态变化情况
        /// <summary>
        /// 状态变化情况
        /// </summary>
        private PhoneStatus? pre_status, cur_status;
        /// 回调事件
        /// <summary>
        /// 回调事件
        /// </summary>
        private Action<object> afterAction;
        /// 回调事件的参数
        /// <summary>
        /// 回调事件的参数
        /// </summary>
        private object[] paras = null;
        //名称
        public string Name = "";

        public int ID = 0;

        private List<PhoneStatus> curlist = null;

        public AfterActionEvent(PhoneStatus? pre, PhoneStatus? cur, Action<object> action, object[] paras, string name)
        {
            this.pre_status = pre;
            this.cur_status = cur;
            this.afterAction = action;
            this.paras = paras;
            this.hasEvent = true;
            this.Name = name;
        }

        public AfterActionEvent(PhoneStatus pre, List<PhoneStatus> cur, Action<object> action, object[] paras, string name)
        {
            this.pre_status = pre;
            this.curlist = cur;
            this.afterAction = action;
            this.paras = paras;
            this.hasEvent = true;
            this.Name = name;
        }

        /// 触发事件
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="in_pre"></param>
        /// <param name="in_cur"></param>
        public bool OnceActionEvent(PhoneStatus in_pre, PhoneStatus in_cur)
        {
            //存在事件
            if (hasEvent)
            {
                if (curlist != null)
                {
                    //第四种情况（pre->curList（任意））
                    if (in_pre == pre_status && curlist.Contains(in_cur))
                    {
                        return ActionEvent();
                    }
                }
                else
                {
                    //第一种情况（pre->cur）
                    if (pre_status.HasValue && cur_status.HasValue && in_pre == pre_status && in_cur == cur_status)
                    {
                        return ActionEvent();
                    }
                    //第二种情况（pre->任意）
                    else if (pre_status.HasValue && cur_status.HasValue == false && in_pre == pre_status && in_pre != in_cur)
                    {
                        return ActionEvent();
                    }
                    //第三种情况（任意->cur）
                    else if (pre_status.HasValue == false && cur_status.HasValue && in_cur == cur_status && in_pre != in_cur)
                    {
                        return ActionEvent();
                    }
                }
            }
            return false;
        }
        private bool ActionEvent()
        {
            if (afterAction != null)
            {
                afterAction(paras);
                //事件只执行一次
                hasEvent = false;
                return true;
            }
            return false;
        }
    }

    /// 事件管理类
    /// <summary>
    /// 事件管理类
    /// </summary>
    public static class AfterActionEventManage
    {
        private static int ID = 0;

        public static int GetID()
        {
            ID++;
            return ID;
        }

        /// 事件队列
        /// <summary>
        /// 事件队列
        /// </summary>
        private static List<AfterActionEvent> AfterActionList = new List<AfterActionEvent>();

        /// 注册一次状态切换（pre->cur）(null代表任意状态) 事件，触发action回调
        /// <summary>
        /// 注册一次状态切换（pre->cur）(null代表任意状态) 事件，触发action回调
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="cur"></param>
        /// <param name="action"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static void RegisterOnceAfterActionEvent1(PhoneStatus? pre, PhoneStatus? cur, Action<object> action, object[] paras, string name)
        {
            AfterActionEvent ae = new AfterActionEvent(pre, cur, action, paras, name);
            ae.ID = GetID();
            AfterActionList.Add(ae);
            Loger.Log4Net.Info("[Main][ActionEvent] 添加监控事件 >>" + name + " ID=" + ae.ID + " 总事件数=" + AfterActionList.Count);
        }
        /// 注册一次状态切换（pre->cur）(null代表任意状态) 事件，触发action回调
        /// <summary>
        /// 注册一次状态切换（pre->cur）(null代表任意状态) 事件，触发action回调
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="cur"></param>
        /// <param name="action"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static void RegisterOnceAfterActionEvent2(PhoneStatus pre, List<PhoneStatus> cur, Action<object> action, object[] paras, string name)
        {
            AfterActionEvent ae = new AfterActionEvent(pre, cur, action, paras, name);
            ae.ID = GetID();
            AfterActionList.Add(ae);
            Loger.Log4Net.Info("[Main][ActionEvent] 添加监控事件 >>" + name + " ID=" + ae.ID + " 总事件数=" + AfterActionList.Count);
        }

        /// 触发事件
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="in_pre"></param>
        /// <param name="in_cur"></param>
        public static void ActionEvent(PhoneStatus in_pre, PhoneStatus in_cur)
        {
            var array = AfterActionList.ToArray();
            foreach (AfterActionEvent item in array)
            {
                if (item.OnceActionEvent(in_pre, in_cur))
                {
                    //需要移除
                    AfterActionList.Remove(item);
                    Loger.Log4Net.Info("[Main][ActionEvent] 触发监控事件 >>" + item.Name + "已完成 ID=" + item.ID + "总事件数=" + AfterActionList.Count);
                }
            }
        }
    }
}
