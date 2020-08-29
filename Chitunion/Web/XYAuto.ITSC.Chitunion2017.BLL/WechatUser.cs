using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatSign;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class WechatUser
    {
        public static readonly WechatUser Instance = new WechatUser();
        /// <summary>
        ///  根据用户ID查询邀请人ID
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns>大于0 返回正常用户ID 否则未找到</returns>
        public int GetInviterId(int UserId)
        {
            return Dal.WechatUser.Instance.GetInviterId(UserId);
        }
        /// <summary>
        /// 根据用户ID获取用户类型
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public int GetUserTypeByUserId(int UserId)
        {
            if (UserId <= 0)
            {
                return 0;
            }
            return Dal.WechatUser.Instance.GetUserTypeByUserId(UserId);
        }
    }
}
