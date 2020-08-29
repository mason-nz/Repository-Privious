/********************************************************
*创建人：hant
*创建时间：2017/12/18 14:26:41 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo
{
    public class AppInfo
    {
        #region
        public static readonly AppInfo Instance = new AppInfo();
        #endregion

        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="appid">主键ID</param>
        /// <returns></returns>
        public Entities.Task.AppInfo GeAppInfo(int appid)
        {
            return Dal.TaskInfo.AppInfo.Instance.GeAppInfo(appid);
        }

        /// <summary>
        /// 根据主键ID 和 Key 获取渠道信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public Entities.Task.AppInfo GeAppInfo(int appid,string appkey)
        {
            return Dal.TaskInfo.AppInfo.Instance.GeAppInfo(appid,appkey);
        }

        /// <summary>
        /// 更新秘钥
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public bool UpdateAppKey(int appid, string appkey)
        {
            return Dal.TaskInfo.AppInfo.Instance.UpdateAppKey(appid, appkey);
        }

        public int GeChannelIDByUserId(int UserId)
        {
            return Dal.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(UserId);
        }

        public string GetChannelNameByChannelId(int ChannelId)
        {
            return Dal.TaskInfo.AppInfo.Instance.GetChannelNameByChannelId(ChannelId);
        }
        public string GetAppKeyByChannelID(int AppId)
        {
            return Dal.TaskInfo.AppInfo.Instance.GetAppKeyByAppID(AppId);
        }
    }
}
