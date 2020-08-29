using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask
{
    public class LeWeixinOAuth
    {
        
        public static readonly LeWeixinOAuth Instance = new LeWeixinOAuth();

        public WeixinInfo GetWeixinInfoByAppId(string appId)
        {
            return Dal.LETask.LeWeixin.Instance.GetWeixinInfoByAppId(appId);
        }
        /// <summary>
        /// 添加微信信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddWeixinInfo(WeixinInfo info)
        {
            // info.Status = 1;//未审核
            return Dal.LETask.LeWeixin.Instance.AddWeixinInfo(info);
        }

        public int UpdateWeixinInfo(WeixinInfo info)
        {
            return Dal.LETask.LeWeixin.Instance.UpdateWeixinInfo(info);
        }

        public int VerifyTaskArticleId(int materielId, int taskType)
        {
            return Dal.LETask.LeTaskInfo.Instance.VerifyTaskArticleId(materielId, taskType);
        }
    }
}
