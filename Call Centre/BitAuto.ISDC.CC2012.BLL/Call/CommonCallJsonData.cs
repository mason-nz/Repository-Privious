using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 公共话务保存对应实体类
    /// <summary>
    /// 公共话务保存对应实体类
    /// 强斐
    /// 2016-8-2
    /// </summary>
    public class CommonCallJsonData
    {
        /// 页面数据
        /// <summary>
        /// 页面数据
        /// </summary>
        public PageDataInfo PageData { get; set; }
        /// 话务数据
        /// <summary>
        /// 话务数据
        /// </summary>
        public CallDataInfo CallData { get; set; }
        /// 短信信息
        /// <summary>
        /// 短信信息
        /// </summary>
        public SMSDataInfo SMSData { get; set; }

        public static CommonCallJsonData GetCommonCallJsonData(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CommonCallJsonData>(json);
        }

        /// 保存后存储
        /// <summary>
        /// 保存后存储
        /// </summary>
        public string CBID { get; set; }
    }

    /// 页面数据
    /// <summary>
    /// 页面数据
    /// </summary>
    public class PageDataInfo : ObjectToString
    {
        public string TaskType { private get; set; }
        public string TaskID { private get; set; }
        public string BGID { private get; set; }
        public string SCID { private get; set; }
        public string CRMCustID { private get; set; }
        public string CustType { private get; set; }
        public string CBName { private get; set; }
        public string CBSex { private get; set; }
        public string CBMemberCode { private get; set; }
        public string CBMemberName { private get; set; }

        public ProjectSource TaskType_Out
        {
            get
            {
                switch (TaskType)
                {
                    case "客户核实":
                        return ProjectSource.S2_客户核实;
                    case "其他任务":
                        return ProjectSource.S4_其他任务;
                    case "厂家集客":
                        return ProjectSource.S6_厂家集客;
                    case "工单":
                        return ProjectSource.S3_工单;
                    default:
                        return ProjectSource.None;
                }
            }
        } //任务结果表，任务话务表，来电去电表中的枚举
        public VisitBusinessTypeEnum BusinessType_Out
        {
            get
            {
                switch (TaskType)
                {
                    case "客户核实":
                        return VisitBusinessTypeEnum.S3_客户核实;
                    case "其他任务":
                        return VisitBusinessTypeEnum.S4_其他任务;
                    case "厂家集客":
                        return VisitBusinessTypeEnum.S6_厂家集客;
                    case "工单":
                        return VisitBusinessTypeEnum.S1_工单;
                    default:
                        return VisitBusinessTypeEnum.None;
                }
            }
        } //号码任务表中的枚举
        public string TaskID_Out { get { return TaskID; } }
        public int BGID_Out { get { return CommonFunction.ObjectToInteger(BGID, -1); } }
        public int SCID_Out { get { return CommonFunction.ObjectToInteger(SCID, -1); } }
        public long CRMCustID_Out { get { return CommonFunction.ObjectToLong(CRMCustID, -1); } }
        public CustTypeEnum CustType_Out
        {
            get
            {
                if (CommonFunction.ObjectToInteger(CustType) == (int)CustTypeEnum.T02_经销商)
                    return CustTypeEnum.T02_经销商;
                else
                    return CustTypeEnum.T01_个人;
            }
        }
        public string CBName_Out { get { return CBName; } }
        public int CBSex_Out { get { return CommonFunction.ObjectToInteger(CBSex, -1); } }
        public string CBMemberCode_Out { get { return CBMemberCode; } }
        public string CBMemberName_Out { get { return CBMemberName; } }
    }
    /// 话务数据
    /// <summary>
    /// 话务数据
    /// </summary>
    public class CallDataInfo : ObjectToString
    {
        public string UserEvent { private get; set; }
        public string ExtensionNum { private get; set; }
        public string Beijiao { private get; set; }
        public string Zhujiao { private get; set; }
        public string CallID { private get; set; }
        public string SkillGroup { private get; set; }
        public string CallType { private get; set; }
        public string CurrentDate { private get; set; }
        public string LuoDiHao { private get; set; }
        public string SessionID { private get; set; }
        public string AudioURL { private get; set; }
        public string IsEstablished { private get; set; }
        public string EstablishedStartTime { private get; set; }
        public string TaskID { private get; set; } //自动外呼使用
        public string TaskType { private get; set; } //自动外呼使用

        public string UserEvent_Out { get { return UserEvent; } }
        public string ExtensionNum_Out { get { return ExtensionNum; } }
        public string Beijiao_Out { get { return BLL.Util.HaoMaProcess(Beijiao); } }
        public string Zhujiao_Out { get { return BLL.Util.HaoMaProcess(Zhujiao); } }
        public long CallID_Out { get { return CommonFunction.ObjectToLong(CallID); } }
        public string SkillGroup_Out { get { return SkillGroup; } }
        public int CallType_Out { get { return CommonFunction.ObjectToInteger(CallType) == 2 ? 2 : 1; } }
        public DateTime CurrentDate_Out { get { return CommonFunction.ObjectToDateTime(CurrentDate); } }
        public string LuoDiHao_Out { get { return BLL.Util.HaoMaProcess(LuoDiHao); } }
        public string SessionID_Out { get { return SessionID; } }
        public string AudioURL_Out { get { return AudioURL; } }
        public bool IsEstablished_Out { get { return bool.Parse(IsEstablished); } }
        public DateTime? EstablishedStartTime_Out { get { return CommonFunction.ObjectToDateTimeOrNull(EstablishedStartTime); } }

        public string Phone_Out { get { return ExtensionNum_Out != Beijiao_Out ? Beijiao_Out : Zhujiao_Out; } } //客户手机号
        public DateTime? BeginTime_Out { get { return UserEvent_Out == "Established" ? CurrentDate_Out : EstablishedStartTime_Out; } } //接通时间
        public DateTime? EndTime_Out { get { return UserEvent_Out == "Established" ? null : (DateTime?)CurrentDate_Out; } } //挂断时间
        public int TallTime_Out { get { return UserEvent_Out == "Released" && BeginTime_Out.HasValue && EndTime_Out.HasValue ? (int)(EndTime_Out.Value - BeginTime_Out.Value).TotalSeconds : 0; } } //通话时长
    }
    /// 短信信息
    /// <summary>
    /// 短信信息
    /// </summary>
    public class SMSDataInfo : ObjectToString
    {
        public string Phone { private get; set; }
        public string PageType { private get; set; }
        public string MemberName { private get; set; }
        public string MemberAddress { private get; set; }
        public string MemberTel { private get; set; }
        public string TemplateID { private get; set; }
        public string Content { private get; set; }

        public string Phone_Out { get { return Phone; } }
        public int PageType_Out { get { return CommonFunction.ObjectToInteger(PageType); } }
        public string MemberName_Out { get { return MemberName; } }
        public string MemberAddress_Out { get { return MemberAddress; } }
        public string MemberTel_Out { get { return MemberTel; } }
        public int TemplateID_Out { get { return CommonFunction.ObjectToInteger(TemplateID); } }
        public string Content_Out { get { return Content; } }
    }

    public class ObjectToString
    {
        public override string ToString()
        {
            if (this == null)
                return "";
            string info = "";
            foreach (PropertyInfo proInfo in this.GetType().GetProperties())
            {
                if (proInfo.Name.EndsWith("_Out"))
                {
                    info += proInfo.Name.Replace("_Out", "") + "=" + CommonFunction.ObjectToString(proInfo.GetValue(this, null));
                    info += " ";
                }
            }
            return "[" + info + "]";
        }
    }
}
