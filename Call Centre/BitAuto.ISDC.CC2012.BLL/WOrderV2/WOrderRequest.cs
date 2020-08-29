using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 新增工单的请求参数
    /// <summary>
    /// 新增工单的请求参数
    /// </summary>
    public class WOrderRequest
    {
        public const int Level = 5;

        /// 通话来源
        /// <summary>
        /// 通话来源
        /// </summary>
        public CallSourceEnum CallSource { get; set; }
        /// 功能来源
        /// <summary>
        /// 功能来源
        /// </summary>
        public ModuleSourceEnum ModuleSource { get; set; }

        public WOrderRequest(CallSourceEnum call, ModuleSourceEnum module)
        {
            CallSource = call;
            ModuleSource = module;

            //设置默认值
            IsPhoneCanModify = true;
            IsShowCallOutBtn = true;
            IsCustTypeCanModify = true;
            IsDataSourceCanModify = true;
            IsCategoryCanModify = true;

            CustType = CustTypeEnum.None;
            DataSource = WorkOrderDataSource.None;
            Category = WOrderCategoryEnum.None;

            BusinessType = -1;
            BusinessTag = -1;
        }

        #region 可选参数
        /// 电话号码
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
        /// 电话号码是否可以修改
        /// <summary>
        /// 电话号码是否可以修改
        /// </summary>
        public bool IsPhoneCanModify { get; set; }
        /// 电话是否可以外呼
        /// <summary>
        /// 电话是否可以外呼
        /// </summary>
        public bool IsShowCallOutBtn { get; set; }

        /// 客户类型
        /// <summary>
        /// 客户类型
        /// </summary>
        public CustTypeEnum CustType { get; set; }
        /// 客户类型是否可以修改
        /// <summary>
        /// 客户类型是否可以修改
        /// </summary>
        public bool IsCustTypeCanModify { get; set; }
        /// CRM客户ID 限定经销商查询使用
        /// <summary>
        /// CRM客户ID 限定经销商查询使用
        /// </summary>
        public string CRMCustID { get; set; }

        /// 业务线来源
        /// <summary>
        /// 业务线来源
        /// </summary>
        public WorkOrderDataSource DataSource { get; set; }
        /// 工单分类
        /// <summary>
        /// 工单分类
        /// </summary>
        public WOrderCategoryEnum Category { get; set; }
        /// 业务线来源是否可以修改
        /// <summary>
        /// 业务线来源是否可以修改
        /// </summary>
        public bool IsDataSourceCanModify { get; set; }
        /// 工单分类是否可以修改
        /// <summary>
        /// 工单分类是否可以修改
        /// </summary>
        public bool IsCategoryCanModify { get; set; }

        /// 业务类型
        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }
        /// 业务标签
        /// <summary>
        /// 业务标签
        /// </summary>
        public int BusinessTag { get; set; }
        /// 关联的数据：IM的对话id，未接来电的主键id，客户回访的联系人主键id
        /// <summary>
        /// 关联的数据：IM的对话id，未接来电的主键id，客户回访的联系人主键id
        /// </summary>
        public string RelatedData { get; set; }

        #region 优先级最高参数 高于数据库
        public string MaxName = ""; //姓名
        public int MaxSex = -1; //性别
        public string MaxMember = ""; //经销商code
        #endregion

        #region 优先级最低参数 低于数据库
        public string CBName = ""; //姓名
        public int CBSex = -1; //性别
        public int CBProvince = -1; //省
        public int CBCity = -1; //市
        public int CBCounty = -1; //区县
        public string CBMember = ""; //经销商code
        #endregion
        #endregion

        #region 辅助
        public override string ToString()
        {
            return GetUrl() + "&key=" + GetKey() + "&r=" + new Random().Next();
        }
        /// 秘钥
        /// <summary>
        /// 秘钥
        /// </summary>
        /// <returns></returns>
        private string GetKey()
        {
            string key = BLL.Util.EncryptString(GetUrl());
            string new_key = "";
            //level 精简等级 保留可以被整除的idx对应的字符
            for (int i = 0; i < key.Length; i = i + Level)
            {
                new_key += key[i];
            }
            return new_key;
        }
        /// 参数
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        private string GetUrl()
        {
            string info = "CallSource=" + (int)CallSource + "&ModuleSource=" + (int)ModuleSource;

            //电话号码
            if (!string.IsNullOrEmpty(Phone))
            {
                info += "&Phone=" + Phone;
            }
            if (!string.IsNullOrEmpty(CRMCustID))
            {
                info += "&CRMCustID=" + CRMCustID;
            }
            //枚举
            if (CustType != CustTypeEnum.None)
            {
                info += "&CustType=" + (int)CustType;
            }
            if (DataSource != WorkOrderDataSource.None)
            {
                info += "&DataSource=" + (int)DataSource;
            }
            if (Category != WOrderCategoryEnum.None)
            {
                info += "&Category=" + (int)Category;
            }
            //4 其他
            if (BusinessType > 0)
            {
                info += "&BusinessType=" + BusinessType;
            }
            if (BusinessTag > 0)
            {
                info += "&BusinessTag=" + BusinessTag;
            }
            if (!string.IsNullOrEmpty(RelatedData))
            {
                info += "&RelatedData=" + System.Web.HttpUtility.UrlEncode(RelatedData);
            }
            //bool值
            if (!IsPhoneCanModify) //是默认值不用传值
            {
                info += "&IsPhoneCanModify=" + IsPhoneCanModify;
            }
            if (!IsShowCallOutBtn) //是默认值不用传值
            {
                info += "&IsShowCallOutBtn=" + IsShowCallOutBtn;
            }
            if (!IsCustTypeCanModify) //是默认值不用传值
            {
                info += "&IsCustTypeCanModify=" + IsCustTypeCanModify;
            }
            if (!IsDataSourceCanModify) //是默认值不用传值
            {
                info += "&IsDataSourceCanModify=" + IsDataSourceCanModify;
            }
            if (!IsCategoryCanModify) //是默认值不用传值
            {
                info += "&IsCategoryCanModify=" + IsCategoryCanModify;
            }
            if (!string.IsNullOrEmpty(MaxName))
            {
                info += "&MaxName=" + System.Web.HttpUtility.UrlEncodeUnicode(MaxName);
            }
            if (MaxSex > 0)
            {
                info += "&MaxSex=" + MaxSex;
            }
            if (!string.IsNullOrEmpty(MaxMember))
            {
                info += "&MaxMember=" + MaxMember;
            }
            if (!string.IsNullOrEmpty(CBName))
            {
                info += "&CBName=" + System.Web.HttpUtility.UrlEncodeUnicode(CBName);
            }
            if (CBSex > 0)
            {
                info += "&CBSex=" + CBSex;
            }
            if (CBProvince > 0)
            {
                info += "&CBProvince=" + CBProvince;
            }
            if (CBCity > 0)
            {
                info += "&CBCity=" + CBCity;
            }
            if (CBCounty > 0)
            {
                info += "&CBCounty=" + CBCounty;
            }
            if (!string.IsNullOrEmpty(CBMember))
            {
                info += "&CBMember=" + CBMember;
            }
            return info;
        }
        #endregion

        /// 从URL参数中解析具体值
        /// <summary>
        /// 从URL参数中解析具体值
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest GetWOrderRequestFromRequest()
        {
            string callsource = BLL.Util.GetCurrentRequestStr("CallSource");
            string modulesource = BLL.Util.GetCurrentRequestStr("ModuleSource");

            if (string.IsNullOrEmpty(callsource) || string.IsNullOrEmpty(modulesource))
            {
                return null;
            }

            CallSourceEnum CallSource = (CallSourceEnum)Enum.Parse(typeof(CallSourceEnum), callsource);
            ModuleSourceEnum ModuleSource = (ModuleSourceEnum)Enum.Parse(typeof(ModuleSourceEnum), modulesource);

            string Phone = BLL.Util.GetCurrentRequestStr("Phone");
            string IsPhoneCanModify = BLL.Util.GetCurrentRequestStr("IsPhoneCanModify");
            string IsShowCallOutBtn = BLL.Util.GetCurrentRequestStr("IsShowCallOutBtn");
            string CustType = BLL.Util.GetCurrentRequestStr("CustType");
            string IsCustTypeCanModify = BLL.Util.GetCurrentRequestStr("IsCustTypeCanModify");
            string CRMCustID = BLL.Util.GetCurrentRequestStr("CRMCustID");
            string DataSource = BLL.Util.GetCurrentRequestStr("DataSource");
            string Category = BLL.Util.GetCurrentRequestStr("Category");
            string IsDataSourceCanModify = BLL.Util.GetCurrentRequestStr("IsDataSourceCanModify");
            string IsCategoryCanModify = BLL.Util.GetCurrentRequestStr("IsCategoryCanModify");
            string BusinessType = BLL.Util.GetCurrentRequestStr("BusinessType");
            string BusinessTag = BLL.Util.GetCurrentRequestStr("BusinessTag");
            string RelatedData = BLL.Util.GetCurrentRequestStr("RelatedData");

            string CBName = BLL.Util.GetCurrentRequestStr("CBName");
            string CBSex = BLL.Util.GetCurrentRequestStr("CBSex");
            string CBProvince = BLL.Util.GetCurrentRequestStr("CBProvince");
            string CBCity = BLL.Util.GetCurrentRequestStr("CBCity");
            string CBCounty = BLL.Util.GetCurrentRequestStr("CBCounty");
            string CBMember = BLL.Util.GetCurrentRequestStr("CBMember");

            string MaxName = BLL.Util.GetCurrentRequestStr("MaxName");
            string MaxSex = BLL.Util.GetCurrentRequestStr("MaxSex");
            string MaxMember = BLL.Util.GetCurrentRequestStr("MaxMember");

            string key = BLL.Util.GetCurrentRequestStr("key");

            WOrderRequest info = new WOrderRequest(CallSource, ModuleSource);
            info.Phone = Phone;
            if (!string.IsNullOrEmpty(IsPhoneCanModify))
            {
                info.IsPhoneCanModify = bool.Parse(IsPhoneCanModify);
            }
            if (!string.IsNullOrEmpty(IsShowCallOutBtn))
            {
                info.IsShowCallOutBtn = bool.Parse(IsShowCallOutBtn);
            }
            if (!string.IsNullOrEmpty(CustType))
            {
                info.CustType = (CustTypeEnum)Enum.Parse(typeof(CustTypeEnum), CustType);
            }
            if (!string.IsNullOrEmpty(IsCustTypeCanModify))
            {
                info.IsCustTypeCanModify = bool.Parse(IsCustTypeCanModify);
            }
            info.CRMCustID = CRMCustID;

            if (!string.IsNullOrEmpty(DataSource))
            {
                info.DataSource = (WorkOrderDataSource)Enum.Parse(typeof(WorkOrderDataSource), DataSource);
            }
            if (!string.IsNullOrEmpty(Category))
            {
                info.Category = (WOrderCategoryEnum)Enum.Parse(typeof(WOrderCategoryEnum), Category);
            }
            if (!string.IsNullOrEmpty(IsDataSourceCanModify))
            {
                info.IsDataSourceCanModify = bool.Parse(IsDataSourceCanModify);
            }
            if (!string.IsNullOrEmpty(IsCategoryCanModify))
            {
                info.IsCategoryCanModify = bool.Parse(IsCategoryCanModify);
            }
            info.BusinessType = CommonFunction.ObjectToInteger(BusinessType, -1);
            info.BusinessTag = CommonFunction.ObjectToInteger(BusinessTag, -1);
            info.RelatedData = RelatedData;

            info.MaxName = MaxName;
            info.MaxSex = CommonFunction.ObjectToInteger(MaxSex, -1);
            info.MaxMember = MaxMember;

            info.CBName = CBName;
            info.CBSex = CommonFunction.ObjectToInteger(CBSex, -1);
            info.CBProvince = CommonFunction.ObjectToInteger(CBProvince, -1);
            info.CBCity = CommonFunction.ObjectToInteger(CBCity, -1);
            info.CBCounty = CommonFunction.ObjectToInteger(CBCounty, -1);
            info.CBMember = CBMember;

            //yiche 测试秘钥
            if (info.GetKey() != key && key != "yiche")
            {
                return null;
            }
            else if ((CallSource == CallSourceEnum.C03_IM对话 || CallSource == CallSourceEnum.C04_IM留言)
                && info.RelatedData == "")
            {
                //IM工单没有对应的对话id
                return null;
            }
            else if (ModuleSource == ModuleSourceEnum.M04_未接来电 && info.RelatedData == "")
            {
                //未接来电没有recid
                return null;
            }
            else
            {
                return info;
            }
        }

        #region 入口
        /// 呼入弹屏入口
        /// <summary>
        /// 呼入弹屏入口
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_CallIn(WorkOrderDataSource dataSource, string phone)
        {
            if (Enum.IsDefined(typeof(WorkOrderDataSource), (int)dataSource) == false)
            {
                dataSource = WorkOrderDataSource.None;
            }
            WOrderRequest info = new WOrderRequest(CallSourceEnum.C01_呼入, ModuleSourceEnum.M00_无);
            info.Phone = phone;
            info.IsPhoneCanModify = false;
            info.IsShowCallOutBtn = false;
            info.DataSource = dataSource;
            info.IsDataSourceCanModify = dataSource == WorkOrderDataSource.None;
            return info;
        }
        /// 外乎弹屏入口
        /// <summary>
        /// 外乎弹屏入口
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_CallOut(string phone, string crmcustid, string cbname = "", int cbsex = -1, string cbmembercode = "", int contactid = -1)
        {
            WOrderRequest info = new WOrderRequest(CallSourceEnum.C02_呼出, ModuleSourceEnum.M03_客户回访);
            info.Phone = phone.Replace("-", "").Replace(" ", ""); //去掉座机的中杠和空格
            info.IsPhoneCanModify = false;
            info.IsShowCallOutBtn = false;

            info.CustType = CustTypeEnum.T02_经销商;
            info.IsCategoryCanModify = false;
            info.CRMCustID = crmcustid;

            info.DataSource = WorkOrderDataSource.TeleContact; //默认为“电话”，且不可更改
            info.Category = WOrderCategoryEnum.W04_回访; //默认为“回访”，且不可更改
            info.IsCustTypeCanModify = false;
            info.IsDataSourceCanModify = false;

            info.RelatedData = contactid.ToString();

            //备选参数
            info.CBName = cbname;
            info.CBSex = cbsex;
            info.CBMember = cbmembercode;

            //info.MaxName = cbname;
            //info.MaxSex = cbsex;
            //info.MaxMember = cbmembercode;
            return info;
        }
        /// 客户回访工单入口
        /// <summary>
        /// 客户回访工单入口
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_CRMCustID(string crmcustid)
        {
            WOrderRequest info = new WOrderRequest(CallSourceEnum.C00_无, ModuleSourceEnum.M03_客户回访);

            info.CustType = CustTypeEnum.T02_经销商;
            info.IsCategoryCanModify = false;
            info.CRMCustID = crmcustid;

            info.DataSource = WorkOrderDataSource.TeleContact; //默认为“电话”，且不可更改
            info.Category = WOrderCategoryEnum.W04_回访; //默认为“回访”，且不可更改
            info.IsCustTypeCanModify = false;
            info.IsDataSourceCanModify = false;
            return info;
        }
        /// 未接来电工单入口
        /// <summary>
        /// 未接来电工单入口
        /// </summary>
        /// <param name="misscall_recid"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_MissedCall(int misscall_recid, string phone, WorkOrderDataSource dataSource)
        {
            if (Enum.IsDefined(typeof(WorkOrderDataSource), (int)dataSource) == false)
            {
                dataSource = WorkOrderDataSource.None;
            }
            WOrderRequest info = new WOrderRequest(CallSourceEnum.C00_无, ModuleSourceEnum.M04_未接来电);
            info.Phone = phone;
            info.IsPhoneCanModify = false;
            info.IsShowCallOutBtn = true;
            info.DataSource = dataSource;
            info.IsDataSourceCanModify = dataSource == WorkOrderDataSource.None;
            info.RelatedData = misscall_recid.ToString();
            return info;
        }
        /// 有电话号码入口-客户池
        /// <summary>
        /// 有电话号码入口-客户池
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_CustPool(string phone)
        {
            WOrderRequest info = new WOrderRequest(CallSourceEnum.C00_无, ModuleSourceEnum.M01_客户池);
            info.Phone = phone.Replace("-", "").Replace(" ", ""); //去掉座机的中杠和空格
            info.IsPhoneCanModify = false;
            info.IsShowCallOutBtn = true;
            return info;
        }
        /// 无电话号码入口-客户池&工单
        /// <summary>
        /// 无电话号码入口-客户池&工单
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_NoPhone(ModuleSourceEnum module)
        {
            WOrderRequest info = new WOrderRequest(CallSourceEnum.C00_无, module);
            return info;
        }

        /// IM工单入口-个人
        /// <summary>
        /// IM工单入口-个人
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_IMGR(CallSourceEnum callsource, string phone, long relatedid,
            string cbname, int cbsex, int province, int city, int county, int businesstype, int businesstag)
        {
            if (callsource != CallSourceEnum.C03_IM对话 && callsource != CallSourceEnum.C04_IM留言)
            {
                return null;
            }
            WOrderRequest info = new WOrderRequest(callsource, ModuleSourceEnum.M05_IM个人);
            info.Phone = phone.Replace("-", "").Replace(" ", ""); //去掉座机的中杠和空格
            info.IsPhoneCanModify = string.IsNullOrEmpty(phone);
            info.IsShowCallOutBtn = false;
            info.CustType = CustTypeEnum.T01_个人;
            info.DataSource = WorkOrderDataSource.IMOnLine;
            info.IsDataSourceCanModify = false;

            info.RelatedData = relatedid.ToString();
            info.BusinessType = businesstype;
            info.BusinessTag = businesstag;

            info.CBName = cbname;
            info.CBSex = cbsex;
            info.CBProvince = province;
            info.CBCity = city;
            info.CBCounty = county;
            return info;
        }
        /// IM工单入口-经销商
        /// <summary>
        /// IM工单入口-经销商
        /// </summary>
        /// <returns></returns>
        public static WOrderRequest AddWOrderComeIn_IMJXS(CallSourceEnum callsource, ModuleSourceEnum module, string phone, long relatedid,
            string cbname, int cbsex, int province, int city, int county, string membercode)
        {
            if (callsource != CallSourceEnum.C03_IM对话 && callsource != CallSourceEnum.C04_IM留言)
            {
                return null;
            }
            if (module != ModuleSourceEnum.M06_IM经销商_新车 && module != ModuleSourceEnum.M07_IM经销商_二手车)
            {
                return null;
            }
            WOrderRequest info = new WOrderRequest(callsource, module);
            info.Phone = phone.Replace("-", "").Replace(" ", ""); //去掉座机的中杠和空格
            info.IsPhoneCanModify = string.IsNullOrEmpty(phone);
            info.IsShowCallOutBtn = false;
            info.CustType = CustTypeEnum.T02_经销商;
            info.DataSource = WorkOrderDataSource.IMOnLine;
            info.IsDataSourceCanModify = false;

            info.RelatedData = relatedid.ToString();

            info.CBName = cbname;
            info.CBSex = cbsex;
            info.CBProvince = province;
            info.CBCity = city;
            info.CBCounty = county;
            info.CBMember = membercode;
            return info;
        }
        #endregion
    }
}
