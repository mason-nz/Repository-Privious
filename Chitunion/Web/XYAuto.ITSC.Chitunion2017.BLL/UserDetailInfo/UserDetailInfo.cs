using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Dal;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class UserDetailInfo
    {
        #region Instance
        public static readonly UserDetailInfo Instance = new UserDetailInfo();
        #endregion

        #region 根据userid获取用户名、真实姓名、手机号码
        /// <summary>
        /// 根据userid获取用户名、真实姓名、手机号码
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public Dictionary<string, object> GetUserInfoByUserID(int userid)
        {
            return Dal.UserDetailInfo.Instance.GetUserInfoByUserID(userid);
        }
        #endregion
    }
}
