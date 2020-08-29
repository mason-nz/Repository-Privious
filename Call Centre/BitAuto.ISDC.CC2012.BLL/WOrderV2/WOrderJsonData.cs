using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 工单主数据
    /// <summary>
    /// 工单主数据
    /// </summary>
    public class WOrderJsonData
    {
        //公共数据
        public CommonJsonData Common { get; set; }
        //个人数据
        public CustBaseInfoJsonData CustBaseInfo { get; set; }
        //工单数据
        public WOrderInfoJsonData WOrderInfo { get; set; }
        //添加工单操作：添加 或者 转出
        public string Oper { get; set; }
        //添加工单的crm客户id
        public string CRMCustID_Out
        {
            get
            {
                if (Common != null && !string.IsNullOrEmpty(Common.CRMCustID_Out))
                {
                    //url参数中传递的值
                    return Common.CRMCustID_Out;
                }
                else if (CustBaseInfo != null && !string.IsNullOrEmpty(CustBaseInfo.CRMCustID_Out))
                {
                    //用户选择的值
                    return CustBaseInfo.CRMCustID_Out;
                }
                else return "";
            }
        }

        //工单状态
        public WorkOrderStatus OrderStatus { get { return Oper == "添加" ? WorkOrderStatus.Completed : WorkOrderStatus.Pending; } }
        //工单操作            
        public WOrderOperTypeEnum OperType { get { return Oper == "添加" ? WOrderOperTypeEnum.L01_添加 : WOrderOperTypeEnum.L02_转出; } }

        public bool Validate(out string msg)
        {
            if (!Common.Validate())
            {
                msg = Common.Message;
                return false;
            }
            else if (!CustBaseInfo.Validate())
            {
                msg = CustBaseInfo.Message;
                return false;
            }
            else if (!WOrderInfo.Validate())
            {
                msg = WOrderInfo.Message;
                return false;
            }
            else
            {
                msg = "";
                return true;
            }
        }

        /// json转换实体类
        /// <summary>
        /// json转换实体类
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static WOrderJsonData CreateWOrderJsonData(string json)
        {
            WOrderJsonData jsondata = (WOrderJsonData)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(WOrderJsonData));
            return jsondata;
        }

        //保存之后，个人用户id
        public string CBID = "";
        //保存之后，工单id
        public string WOrderID = "";
    }
    /// 公共部分数据
    /// <summary>
    /// 公共部分数据
    /// </summary>
    public class CommonJsonData
    {
        public string CallSource { get; set; }//通话来源
        public string ModuleSource { get; set; } //功能来源
        public string CRMCustID { get; set; }//客户回访的crm的客户id
        public string RelatedID { get; set; }//IM对话id或者未接来电id

        public CallSourceEnum CallSource_Out { get { return (CallSourceEnum)Enum.Parse(typeof(CallSourceEnum), CallSource); } }//通话来源
        public ModuleSourceEnum ModuleSource_Out { get { return (ModuleSourceEnum)Enum.Parse(typeof(ModuleSourceEnum), ModuleSource); } } //功能来源
        public string CRMCustID_Out { get { return CRMCustID; } }//客户回访的id

        /// IM对话id
        /// <summary>
        /// IM对话id
        /// </summary>
        public long RelatedCSID { get { return CommonFunction.ObjectToLong(RelatedID, -1); } }
        /// 未接来电id
        /// <summary>
        /// 未接来电id
        /// </summary>
        public int RelatedMissID { get { return CommonFunction.ObjectToInteger(RelatedID, -1); } }
        /// 联系人ID
        /// <summary>
        /// 联系人ID
        /// </summary>
        public int RelatedContactID { get { return CommonFunction.ObjectToInteger(RelatedID, -1); } }

        //错误信息
        public string Message = "";
        //方法校验
        public bool Validate()
        {
            if ((CallSource_Out == CallSourceEnum.C03_IM对话 || CallSource_Out == CallSourceEnum.C04_IM留言)
                && RelatedCSID <= 0)
            {
                Message = "IM工单没有对应的会话ID！";
                return false;
            }
            if (ModuleSource_Out == ModuleSourceEnum.M04_未接来电 && RelatedMissID <= 0)
            {
                Message = "未接来电没有主键ID！";
                return false;
            }
            else return true;
        }
    }
    /// 个人用户数据
    /// <summary>
    /// 个人用户数据
    /// </summary>
    public class CustBaseInfoJsonData
    {
        public string Phone { private get; set; }//电话
        public string CallIds { private get; set; } //通话ids
        public string SmsIds { private get; set; } //短信ids
        public string CBName { private get; set; } //姓名
        public string CBSex { private get; set; }//性别
        public string ProvinceID { private get; set; }//省
        public string CityID { private get; set; }//市
        public string CountyID { private get; set; } //区县
        public string CustTypeID { private get; set; }//客户类别
        public string MemberCode { private get; set; }//经销商id
        public string MemberName { private get; set; }//经销商名称
        public string CRMCustID { private get; set; } //客户id 用户选择的
        public string CRMCustName { private get; set; } //客户名称 用户选择的

        public string Phone_Out { get { return Phone; } }//电话
        public List<string> CallIds_Out { get { return new List<string>(CallIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)); } } //通话ids
        public List<string> SmsIds_Out { get { return new List<string>(SmsIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)); } } //短信ids
        public string CBName_Out { get { return CBName; } } //姓名
        public int CBSex_Out { get { return CommonFunction.ObjectToInteger(CBSex, -1); } }//性别
        public int ProvinceID_Out { get { return CommonFunction.ObjectToInteger(ProvinceID, -1); } }//省
        public int CityID_Out { get { return CommonFunction.ObjectToInteger(CityID, -1); } }//市
        public int CountyID_Out { get { return CommonFunction.ObjectToInteger(CountyID, -1); } } //区县
        public CustTypeEnum CustTypeID_Out { get { return (CustTypeEnum)Enum.Parse(typeof(CustTypeEnum), CustTypeID); } }//客户类别
        public string MemberCode_Out { get { return CustTypeID_Out == CustTypeEnum.T02_经销商 ? MemberCode : ""; } }//经销商id
        public string MemberName_Out { get { return CustTypeID_Out == CustTypeEnum.T02_经销商 ? MemberName : ""; } }//经销商名称
        public string CRMCustID_Out { get { return CRMCustID; } } //客户id  用户选择的
        public string CRMCustName_Out { get { return CRMCustName; } } //客户名称 用户选择的

        //错误信息
        public string Message = "";
        //方法校验
        public bool Validate()
        {
            if (Phone_Out == "")
            {
                Message = "请填写用户号码！";
                return false;
            }
            else if (CBName_Out == "")
            {
                Message = "请填写用户姓名！";
                return false;
            }
            else if (CBSex_Out <= 0)
            {
                Message = "请选择用户性别！";
                return false;
            }
            else if (ProvinceID_Out <= 0)
            {
                Message = "请选择用户所在省份！";
                return false;
            }
            else if (CustTypeID_Out <= 0)
            {
                Message = "请选择客户类别！";
                return false;
            }
            else if (CustTypeID_Out == CustTypeEnum.T02_经销商 && MemberName_Out == "")
            {
                Message = "请填写所属经销商！";
                return false;
            }
            //有效性校验
            else if (!BLL.Util.IsTelephone(Phone_Out))
            {
                Message = "请填写正确的用户号码！";
                return false;
            }
            else
                return true;
        }
    }
    /// 工单数据
    /// <summary>
    /// 工单数据
    /// </summary>
    public class WOrderInfoJsonData
    {
        public string DataSource { private get; set; } //工单来源
        public string Category { private get; set; } //工单分类
        public string BusinessType { private get; set; } //业务类型
        public string BusinessTag { private get; set; } //标签
        public string VisitType { private get; set; } //访问分类
        public string IsJieTong { private get; set; } //是否接通
        public string NoJtReason { private get; set; } //未接通原因
        public string ComplaintLevel { private get; set; } //投诉级别
        public string ContactName { private get; set; } //联系人
        public string ContactTel { private get; set; } //联系电话
        public string Content { private get; set; } //工单记录
        public string IsSysCRM { private get; set; } //是否同步crm
        public List<AttachmentJsonData> Attachment { get; set; } //附件集合
        public string CustTypeID { private get; set; }//客户类别

        public CustTypeEnum CustTypeID_Out { get { return (CustTypeEnum)Enum.Parse(typeof(CustTypeEnum), CustTypeID); } }//客户类别
        public WorkOrderDataSource DataSource_Out { get { return (WorkOrderDataSource)Enum.Parse(typeof(WorkOrderDataSource), DataSource); } } //工单来源
        public WOrderCategoryEnum Category_Out { get { return (WOrderCategoryEnum)Enum.Parse(typeof(WOrderCategoryEnum), Category); } } //工单分类
        public int BusinessType_Out { get { return CommonFunction.ObjectToInteger(BusinessType, -1); } } //业务类型
        public int BusinessTag_Out { get { return CommonFunction.ObjectToInteger(BusinessTag, -1); } } //标签
        public int VisitType_Out { get { return IsHuifang && IsJxs ? CommonFunction.ObjectToInteger(VisitType, -1) : -1; } } //访问分类
        public int IsJieTong_Out { get { return IsHuifang && IsJxs ? CommonFunction.ObjectToInteger(IsJieTong, -1) : -1; } } //是否接通
        public int NoJtReason_Out { get { return IsJieTong_Out == 0 ? CommonFunction.ObjectToInteger(NoJtReason, -1) : -1; } } //未接通原因
        public int ComplaintLevel_Out { get { return IsTousu ? CommonFunction.ObjectToInteger(ComplaintLevel, -1) : -1; } } //投诉级别
        public string ContactName_Out { get { return ContactName; } } //联系人
        public string ContactTel_Out { get { return ContactTel; } } //联系电话
        public string Content_Out { get { return Content; } } //工单记录
        public int IsSysCRM_Out { get { return IsHuifang && IsJxs ? (bool.Parse(IsSysCRM) ? 1 : 0) : -1; } } //是否同步crm
        public List<AttachmentJsonData> Attachment_Out { get { return Attachment; } } //附件集合

        public bool IsHuifang { get { return Category_Out == BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W04_回访; } }
        public bool IsTousu { get { return Category_Out == BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W03_投诉; } }
        public bool IsJxs { get { return CustTypeID_Out == BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商; } }

        //错误信息
        public string Message = "";
        //方法校验
        public bool Validate()
        {
            //非空校验
            if ((int)DataSource_Out < 0)
            {
                Message = "请选择工单来源！";
                return false;
            }
            else if ((int)Category_Out < 0)
            {
                Message = "请选择工单类型！";
                return false;
            }
            else if (!(IsHuifang && IsJxs) && BusinessType_Out < 0)
            {
                //除了回访经销商外，必填
                Message = "请选择业务类型！";
                return false;
            }
            else if (!(IsHuifang && IsJxs) && BusinessTag_Out < 0)
            {
                //除了回访经销商外，必填
                Message = "请选择标签！";
                return false;
            }
            else if ((IsHuifang && IsJxs) && IsSysCRM_Out == 1 && VisitType_Out < 0)
            {
                //回访经销商且同步时，必填
                Message = "请选择访问分类！";
                return false;
            }
            else if ((IsHuifang && IsJxs) && IsJieTong_Out < 0)
            {
                //回访经销商，必填
                Message = "请选择是否接通！";
                return false;
            }
            else if ((IsHuifang && IsJxs) && IsJieTong_Out == 0 && NoJtReason_Out < 0)
            {
                //回访经销商且未接通，必填
                Message = "请选择未接通原因！";
                return false;
            }
            else if (IsTousu && ComplaintLevel_Out < 0)
            {
                //投诉时，必填
                Message = "请选择投诉级别！";
                return false;
            }
            else if ((ContactName_Out == "" && ContactTel_Out != "") || (ContactName_Out != "" && ContactTel_Out == ""))
            {
                //有联系人时，必填
                Message = "请输入完整的联系人信息！";
                return false;
            }
            else if (Content_Out == "")
            {
                Message = "请输入工单记录！";
                return false;
            }
            //有效性校验
            else if (ContactTel_Out != "" && !BLL.Util.IsTelephone(ContactTel_Out))
            {
                Message = "请填写正确的联系人电话号码！";
                return false;
            }
            else return true;
        }
    }
    /// 附件数据
    /// <summary>
    /// 附件数据
    /// </summary>
    public class AttachmentJsonData
    {
        public string FileRealName { private get; set; } //真实名称
        public string SmallFileVirtuPath { private get; set; } //缩略图
        public string FileSize { private get; set; } //文件大小
        public string FileType { private get; set; } //文件类型
        public string FileAllPath { private get; set; } //文件位置

        public string FileRealName_Out { get { return FileRealName; } } //真实名称
        public int FileSize_Out { get { return CommonFunction.ObjectToInteger(FileSize); } } //文件大小
        public string FileType_Out { get { return FileType.ToLower(); } } //文件类型
        public string FileAllPath_Out { get { return FileAllPath; } } //文件位置
    }
}