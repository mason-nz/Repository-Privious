using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class WechatUser : DataBase
    {
        public static readonly WechatUser Instance = new WechatUser();
        /// <summary>
        /// 根据用户ID查询邀请人ID
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public int GetInviterId(int UserId)
        {
            string sql = string.Format($"SELECT Inviter FROM v_LE_WeiXinUser W WHERE W.UserID={UserId} and status=0 AND W.Inviter IS NOT NULL AND W.Inviter <> ''");
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 根据用户ID获取用户类型
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public int GetUserTypeByUserId(int UserId)
        {
            string sql = string.Format($"SELECT Type FROM dbo.UserInfo WHERE UserID={UserId}");
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 判断是否为微信注册用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool IsWxUser(int UserId)
        {
            string sql = string.Format($"SELECT COUNT(1) FROM  LE_WeiXinUser WHERE UserID={UserId}");
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? false : (Convert.ToInt32(obj) > 0 ? true : false);
        }
    }
}
