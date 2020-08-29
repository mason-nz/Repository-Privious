using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.WebAPI.Models
{
    /// 业务类型
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum Business
    {
        未知 = -2,
        惠买车 = 0,
        汽车金融 = 1,
        企业 = 2,
        易车商城 = 3
    }
    /// 用户类型
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        黑名单 = 0,
        白名单 = 1,
        一般用户 = 2
    }
    /// 结果
    /// <summary>
    /// 结果
    /// </summary>
    public enum ResultType
    {
        失败 = 0,
        成功 = 1
    }
}