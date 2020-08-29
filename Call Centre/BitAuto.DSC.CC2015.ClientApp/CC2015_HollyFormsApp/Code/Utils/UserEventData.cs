using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    public class UserEventData
    {
        /// 事件类型
        /// <summary>
        /// 事件类型
        /// </summary>
        public UserEvent UserEvent { get; set; }
        /// 被叫
        /// <summary>
        /// 被叫
        /// </summary>
        public string BejiaoNum { get; set; }
        /// 主叫
        /// <summary>
        /// 主叫
        /// </summary>
        public string ZhujiaoNum { get; set; }
        /// 话务ID
        /// <summary>
        /// 话务ID
        /// </summary>
        public long CallID { get; set; }
        /// 技能组
        /// <summary>
        /// 技能组
        /// </summary>
        public string UserChoice { get; set; }
        /// 呼叫类型
        /// <summary>
        /// 呼叫类型
        /// </summary>
        public Calltype CallType { get; set; }
        /// 当前时间 EscapeString（yyyy-MM-dd HH:mm:ss）
        /// <summary>
        /// 当前时间 EscapeString（yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public DateTime CurrentDate { get; set; }
        /// 热线落地号码
        /// <summary>
        /// 热线落地号码
        /// </summary>
        public string SYS_DNIS { get; set; }
        /// 厂家ID
        /// <summary>
        /// 厂家ID
        /// </summary>
        public string RecordID { get; set; }
        /// 录音地址
        /// <summary>
        /// 录音地址
        /// </summary>
        public string RecordIDURL { get; set; }
        /// 是否接通
        /// <summary>
        /// 是否接通
        /// </summary>
        public bool IsEstablished { get; set; }
        /// 接通时间
        /// <summary>
        /// 接通时间
        /// </summary>
        public DateTime? EstablishedStartTime { get; set; }
        /// 任务ID
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID { get; set; }
        /// 任务类型
        /// <summary>
        /// 任务类型
        /// </summary>
        public int? TaskType { get; set; }

        public override string ToString()
        {
            //CallRecord_ORIG和CallRecordInfo表主被叫字段名称相同含义相反
            //CallRecordInfo存储是，会根据呼入呼出，自动置换位置
            string zhujiao = Common.ChooseString(ZhujiaoNum, BejiaoNum, Choose.手机号);
            string beijiao = Common.ChooseString(ZhujiaoNum, BejiaoNum, Choose.分机);

            string strMsg = string.Format(
                                "UserEvent={0}&UserName={1}&CalledNum={2}&CallerNum={3}&CallID={4}&UserChoice={5}&" +
                                "CallType={6}&MediaType={7}&CallState={8}&CurrentDate={9}&SYS_DNIS={10}&" +
                                "RecordID={11}&RecordIDURL={12}&IsEstablished={13}&EstablishedStartTime={14}&" +
                                "TaskID={15}&TaskType={16}",
                              UserEvent.ToString(),
                              LoginUser.ExtensionNum,
                              beijiao,
                              zhujiao,
                              CallID,
                              UserChoice,
                              (int)CallType,
                              -2,
                              -2,
                              Common.EscapeString(CurrentDate.ToString("yyyy-MM-dd HH:mm:ss")),
                              SYS_DNIS,
                              RecordID,
                              RecordIDURL,
                              IsEstablished.ToString(),
                              (EstablishedStartTime.HasValue ? Common.EscapeString(EstablishedStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss")) : ""),
                              CommonFunction.ObjectToString(TaskID),
                              (TaskType.HasValue ? TaskType.Value.ToString() : ""));
            return strMsg;
        }
    }

    public class NoticeData
    {
        public NoticeData(string action, string data)
        {
            UserEvent = CC2015_HollyFormsApp.UserEvent.Notice;
            Action = action;
            Data = data;
        }

        /// 事件类型
        /// <summary>
        /// 事件类型
        /// </summary>
        public UserEvent UserEvent { get; set; }
        /// 类型
        /// <summary>
        /// 类型
        /// </summary>
        public string Action { get; set; }
        /// 数据
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }

        public override string ToString()
        {
            string strMsg = string.Format(
                                "UserEvent={0}&Action={1}&Data={2}",
                              UserEvent.ToString(), Action, Data);
            return strMsg;
        }
    }
}
