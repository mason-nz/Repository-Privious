using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class MediaUserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; } = string.Empty;
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 注册来源
        /// </summary>
        public string RegisterFromName { get; set; } = string.Empty;
        /// <summary>
        /// 注册方式
        /// </summary>
        public string RegisterTypeName { get; set; } = string.Empty;
        /// <summary>
        /// 来源ID
        /// </summary>
        public int SourceID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 认证状态
        /// </summary>
        public int ApproveStatus { get; set; }
        /// <summary>
        /// 认证状态名称
        /// </summary>
        public string ApproveStatusName { get; set; } = string.Empty;

        /// <summary>
        /// 原因（认证未通过）
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        public int Status { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        public string AttentionName { get; set; }

        public string StatusName { get; set; }
    }

    public class MediaUserDetialModel
    { /// <summary>
      /// 用户ID
      /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; } = string.Empty;
        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; } = string.Empty;
        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; } = string.Empty;
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string WxNumber { get; set; } = string.Empty;
        /// <summary>
        /// 用户类型（1：企业 2：个人）
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 公司或个人真实名称
        /// </summary>
        public string TrueName { get; set; } = string.Empty;
        /// <summary>
        /// 营业执照图片地址
        /// </summary>
        public string BLicenceURL { get; set; }
        /// <summary>
        /// 身份证正面图片
        /// </summary>
        public string IDCardFrontURL { get; set; }

        /// <summary>
        /// 认证状态
        /// </summary>
        public int ApproveStatus { get; set; }
        /// <summary>
        /// 认证状态名称
        /// </summary>
        public string ApproveStatusName { get; set; } = string.Empty;
        /// <summary>
        /// 审核人
        /// </summary>
        public string ApproveUserName { get; set; } = string.Empty;
        /// <summary>
        /// 原因（认证未通过）
        /// </summary>
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 注册来源
        /// </summary>
        public string RegisterFromName { get; set; } = string.Empty;
        /// <summary>
        /// 注册方式
        /// </summary>
        public string RegisterTypeName { get; set; } = string.Empty;

        public string AttentionName { get; set; }

    }
}
