using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class UserQueryArgs
    {
        /// <summary>
        /// 列表类型（广告主：g_user 媒体主：m_user）
        /// </summary>
        public string ListType { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 来源ID
        /// </summary>
        public int Source { get; set; }
        /// <summary>
        /// 注册来源
        /// </summary>
        public int RegisterFrom { get; set; }
        /// <summary>
        /// 注册方式
        /// </summary>
        public int RegisterType { get; set; }
        /// <summary>
        ///状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 认证状态
        /// </summary>
        public int ApproveStatus { get; set; }
        /// <summary>
        /// 注册开始日期
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 注册结束日期
        /// </summary>
        public string EndTime { get; set; }

        public int AttentionStatus { get; set; }

        public long Level1 { get; set; }
    }

    public class UserBatchQueryArgs
    {
        public int UserID { get; set; }

        public List<int> UserIDList { get; set; }

        public int Status { get; set; }

        public string Reason { get; set; }

        public string ListType { get; set; }
    }
    public class UserTokenInfo
    {
        public string openId { get; set; } = string.Empty;

        public string TrueName { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;
    }
}
