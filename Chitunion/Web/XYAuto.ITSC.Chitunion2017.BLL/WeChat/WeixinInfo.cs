using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    /// <summary>
    /// 注释：WeixinInfo
    /// 作者：masj
    /// 日期：2018/6/13 17:00:39
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WeixinInfo
    {
        public static readonly WeixinInfo Instance = new WeixinInfo();

        /// <summary>
        /// 根据AppId，返回主键ID
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public int GetRecIDByAppId(string appId)
        {
            return Dal.WeChat.WeixinInfo.Instance.GetRecIDByAppId(appId);
        }

        public DataTable GetList()
        {
            return Dal.WeChat.WeixinInfo.Instance.GetList();
        }

        /// <summary>
        /// 在表LE_WeiXinUser中，维护指定openIds的userType内容
        /// </summary>
        /// <param name="openIds"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public bool UpdateUserType(List<string> openIds, int userType)
        {
            return Dal.WeChat.WeixinInfo.Instance.UpdateUserType(openIds, userType);
        }
    }
}
