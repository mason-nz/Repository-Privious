using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WechatDataStatic
{
    public class WechatDataStatic
    {
        public static readonly WechatDataStatic Instance = new WechatDataStatic();
        public DataTable StaticInviteDataForDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticInviteDataForDay();
        }
        public DataTable StaticSignDataForDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticSignDataForDay();
        }
        public DataTable StaticDistributeForDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDistributeForDay();
        }
        public DataTable StaticDataSumForWeek()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataSumForWeek();
        }
        /// <summary>
        /// 微信用户数据统计
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinUserDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinUserDay();
        }
        /// <summary>
        /// 微信渠道数据统计
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinChannelDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinChannelDay();
        }
        /// <summary>
        /// 微信抢单赚钱统计数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinOrderDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinOrderDay();
        }

        /// <summary>
        /// 赤兔联盟微信号推广分渠道数据需求—渠道效果数据表
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinChannelResultDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinChannelResultDay();
        }
        /// <summary>
        /// 赤兔联盟微信号推广分渠道数据需求—新关注粉丝明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinNewAttentionDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinNewAttentionDay();
        }

        /// <summary>
        /// 赤兔联盟微信号推广分渠道数据需求—注册用户明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinRegisterUserDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinRegisterUserDay();

        }
        /// <summary>
        /// 物料分发（周报） 
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinDispenseForWeek()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinDispenseForWeek();
        }
        /// <summary>
        /// 类目选择的分类数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinCategorySelectDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinCategorySelectDay();
        }
        /// <summary>
        /// 类目被选择数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataWeiXinGetCategoryUserDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinGetCategoryUserDay();

        }
        /// <summary>
        /// 分发用户明细数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataDistributionDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataDistributionDay();

        }
        /// <summary>
        /// 物料详情
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataMaterialDetailDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataMaterialDetailDay();
        }
        /// <summary>
        /// 物料领取明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticDataMaterialCollectingDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataMaterialCollectingDay();
        }
        /// <summary>
        /// 抢单赚钱登录用户明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticUserLoginLogDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticUserLoginLogDay();
        }

        /// <summary>
        /// 邀请有礼用户明细
        /// </summary>
        /// <returns></returns>
        public DataTable StaticInviteRecordDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticInviteRecordDay();
        }
        /// <summary>
        /// 分类文章数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticCategoryArticleDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticCategoryArticleDay();
        }
        /// <summary>
        /// 个人信息页面数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticUserInfoDay()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticUserInfoDay();
        }
        /// <summary>
        /// 经纪人周数据
        /// </summary>
        /// <returns></returns>
        public DataTable StaticBrokerForWeek()
        {
            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticBrokerForWeek();
        }
        public DataTable StaticDataWeiXinOldAttentionDay()
        {

            return Dal.WechatDataStatic.WechatDataStatic.Instance.StaticDataWeiXinOldAttentionDay();
        }
    }
}
