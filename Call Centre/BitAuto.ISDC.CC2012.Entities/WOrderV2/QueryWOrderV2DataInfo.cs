using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// <summary>
    /// 新版工单记录查询实体
    /// </summary>
    public class QueryWOrderV2DataInfo
    {
        public string OrderID { get; set; }
        public string MemberName { get; set; }
        public string BeginCreateTime { get; set; }
        public string EndCreateTime { get; set; }
        public int CreateUserID { get; set; }
        public string CreateUserName { get; set; }
        public int WorkOrderStatus { get; set; }
        public string WorkOrderStatusList { get; set; }
        public int CategoryID { get; set; }
        public string Phone { get; set; }
        public int BGID { get; set; }
        public int TagID { get; set; }
        public int BigTagID { get; set; }
        public string ReVisitStr { get; set; }
        public int BusiType { get; set; }
        public string ComplaintLevel { get; set; }

        #region CRM
        public string CRMCustID { get; set; } //crm客户id
        public string CRMCustName { get; set; } //crm客户名称
        public int CRMProvinceID { get; set; } //省
        public int CRMCityID { get; set; } //市
        public int CRMCountyID { get; set; } //区
        public string ProcessUserName { get; set; } //当前处理人
        #endregion

        #region 权限控制，不做where条件
        /// CC权限控制
        /// <summary>
        /// CC权限控制
        /// </summary>
        public int CC_LoginID { get; set; }
        /// 智能平台工单权限控制
        /// <summary>
        /// 智能平台工单权限控制
        /// </summary>
        public CRMOrderRightZN RightZN { get; set; }
        /// crm个人工单权限控制
        /// <summary>
        /// crm个人工单权限控制
        /// </summary>
        public CRMOrderRightGR RightGR { get; set; }
        #endregion
    }
    /// 智能平台权限控制
    /// <summary>
    /// 智能平台权限控制
    /// </summary>
    public class CRMOrderRightZN
    {
        public string CRM_LoginDepartID { get; set; } //crm 的登录人的部门
    }
    /// 个人工单权限控制
    /// <summary>
    /// 个人工单权限控制
    /// </summary>
    public class CRMOrderRightGR
    {
        public string CRM_LoginDepartID { get; set; } //crm 的登录人的部门
        public int CRM_LoginID { get; set; } //crm 的登录人
    }
}
