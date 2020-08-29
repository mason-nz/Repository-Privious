using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    /// <summary>
    /// 2017-03-14 张立彬
    /// 接口权限类
    /// </summary>
    public class AuthorizationCommon
    {

        /// <summary>
        /// 刊例查询验证
        /// </summary>
        ///<param name="mediaType">媒体类型</param>
        /// <returns></returns>
        public static bool PublishSelectVerification(int mediaType)
        {
            string ModuleID = string.Empty;
            MediaType M = (MediaType)mediaType;
            switch (M)
            {
                case MediaType.WeiXin:
                    ModuleID = "SYS001BUT500101";
                    break;
                case MediaType.WeiBo:
                    ModuleID = "SYS001BUT500201";
                    break;
                case MediaType.Video:
                    ModuleID = "SYS001BUT500301";
                    break;
                case MediaType.Broadcast:
                    ModuleID = "SYS001BUT500401";
                    break;
                case MediaType.APP:
                    ModuleID = "SYS001BUT500501";
                    break;
                default:
                    break;
            }
            return Common.UserInfo.CheckRight(ModuleID, Chitunion2017.Common.UserInfo.SYSID);

        }
        /// <summary>
        /// 订单查询验证
        /// </summary>
        /// <param name="orderType">0主订单 1子订单</param>
        /// <returns></returns>
        public static bool OrderSelectVerification(int orderType)
        {
            string ModuleID = string.Empty;
            if (orderType == 0)
            {
                ModuleID = "SYS001BUT10001";
            }
            else
            {
                ModuleID = "SYS001BUT20001";
            }
            return Common.UserInfo.CheckRight(ModuleID, Chitunion2017.Common.UserInfo.SYSID);
        }
    }
}
