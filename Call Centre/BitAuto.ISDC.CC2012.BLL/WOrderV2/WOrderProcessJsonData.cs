using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 处理实体类型
    /// <summary>
    /// 处理实体类型
    /// </summary>
    public class WOrderProcessJsonData
    {
        public string WorkOrderStatus { private get; set; } //状态
        public string IsReturnVisit { private get; set; } //是否回访
        public string ProcessContent { private get; set; } //回复
        public string CallIds { private get; set; } //话务
        public List<ToAndCcPerson> Recevicer { get; set; } //接收人
        public List<ToAndCcPerson> ExtendRecev { get; set; } //抄送人
        public List<AttachmentJsonData> imgData { get; set; } //附件

        public WorkOrderStatus WorkOrderStatus_Out { get { return (WorkOrderStatus)Enum.Parse(typeof(WorkOrderStatus), WorkOrderStatus); } }
        public int IsReturnVisit_Out { get { return CommonFunction.ObjectToInteger(IsReturnVisit, -1); } }
        public string ProcessContent_Out { get { return ProcessContent; } }
        public List<long> CallID_Out
        {
            get
            {
                List<long> list = new List<long>();
                if (!string.IsNullOrEmpty(CallIds))
                {
                    foreach (var item in CallIds.Split(','))
                    {
                        long a = CommonFunction.ObjectToLong(item, -1);
                        if (a > 0 && list.Contains(a) == false)
                        {
                            list.Add(a);
                        }
                    }
                }
                return list;
            }
        }

        public static WOrderProcessJsonData GetWOrderProcessJsonData(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<WOrderProcessJsonData>(json);
        }

        //保存之后存储
        public int ProcessID = -1;
    }
    /// 接收人和抄送人
    /// <summary>
    /// 接收人和抄送人
    /// </summary>
    public class ToAndCcPerson
    {
        public string UserNum { private get; set; }
        public string UserID { private get; set; }
        public string UserName { private get; set; }

        public string UserNum_Out { get { return UserNum; } }
        public int UserID_Out { get { return CommonFunction.ObjectToInteger(UserID); } }
        public string UserName_Out { get { return UserName; } }
    }
    /// 处理工单的权限类型
    /// <summary>
    /// 处理工单的权限类型
    /// </summary>
    public class WOrderProcessRightJsonData
    {
        /// 权限类型
        /// <summary>
        /// 权限类型
        /// </summary>
        public int RightType { private get; set; }
        /// 权限数据
        /// <summary>
        /// 权限数据: 
        /// 当RightType=R01_人员ID = 1时：空
        /// 当RightType=R02_部门ID = 2时：登录人的部门id
        /// 当RightType=R03_员工编号 = 3时：登录人的员工编号
        /// 当RightType=R04_功能权限 = 4时：权限码
        /// </summary>
        public string RightData { private get; set; }
        //工单ID（必填，验证有效性用）
        public string OrderID { get; set; }
        //登录人ID（必填，验证有效性用）
        public int LoginUserID { get; set; }

        /// 权限类型
        /// <summary>
        /// 权限类型
        /// </summary>
        public WOrderProcessRightTypeEnum RightType_Out { get { return (WOrderProcessRightTypeEnum)RightType; } }
        /// 权限数据
        /// <summary>
        /// 权限数据: 
        /// 当RightType=R01_人员ID = 1时：空
        /// 当RightType=R02_部门ID = 2时：登录人的部门id
        /// 当RightType=R03_员工编号 = 3时：登录人的员工编号
        /// 当RightType=R04_功能权限 = 4时：权限码
        /// </summary>
        public string RightData_Out { get { return CommonFunction.ObjectToString(RightData); } }

        public static WOrderProcessRightJsonData GetWOrderProcessRightJsonData(string json)
        {
            WOrderProcessRightJsonData right = Newtonsoft.Json.JsonConvert.DeserializeObject<WOrderProcessRightJsonData>(json);
            return right;
        }
    }
}
