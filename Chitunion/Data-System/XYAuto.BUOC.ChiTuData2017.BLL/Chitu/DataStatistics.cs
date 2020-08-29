/********************************************************
*创建人：hant
*创建时间：2017/12/21 14:20:15 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Response;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Chitu
{
    public class DataStatistics
    {
        public static readonly DataStatistics Instance = new DataStatistics();

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ResposeDataStatistics GetDataByUserID(int UserId)
        {
            ResposeDataStatistics list = new ResposeDataStatistics();
            List<Entities.Chitu.DataStatistics> listsource = GetDataByChannelAndDate(UserId);
            List<Channel> listchannel = new List<Channel>();
            if (listsource.Count > 0)
            {
                if (RolesVerification.Instance.IsAllData())
                {
                    foreach (var item in listsource)
                    {
                        ResposeDataStatistics model = new ResposeDataStatistics();
                        if (item.ChannelID > 0)
                        {
                            Dto.Response.Channel channel = new Channel();
                            channel.ChannelName = BLL.TaskInfo.AppInfo.Instance.GetChannelNameByChannelId(item.ChannelID);
                            channel.Accumulated = item.Accumulated;
                            channel.ProfitLastMonth = item.ProfitLastMonth;
                            channel.ProfitOfThisMonth = item.ProfitOfThisMonth;
                            channel.ProfitOfYesterday = item.ProfitOfYesterday;
                            channel.SettledAmount = item.SettledAmount;
                            listchannel.Add(channel);
                        }
                        else
                        {
                            list.Accumulated = item.Accumulated;
                            list.ProfitLastMonth = item.ProfitLastMonth;
                            list.ProfitOfThisMonth = item.ProfitOfThisMonth;
                            list.ProfitOfYesterday = item.ProfitOfYesterday;
                            list.SettledAmount = item.SettledAmount;
                        }
                    }
                }
                else
                {
                    list.Accumulated = listsource[0].Accumulated;
                    list.ProfitLastMonth = listsource[0].ProfitLastMonth;
                    list.ProfitOfThisMonth = listsource[0].ProfitOfThisMonth;
                    list.ProfitOfYesterday = listsource[0].ProfitOfYesterday;
                    list.SettledAmount = listsource[0].SettledAmount;
                }
                
            }
            list.ChannelList = listchannel;
            return list;
        }
        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private List<Entities.Chitu.DataStatistics> GetDataByChannelAndDate(int channelid)
        {
            string dt = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            StringBuilder swhere = new StringBuilder();
            swhere.AppendFormat(" AND [Date]='{0}'", dt);
            if (channelid > 0)
            {
                swhere.AppendFormat(" AND ChannelID = {0}", channelid);
            }
            return Dal.Chitu.DataStatistics.Instance.GetDataByChannelAndDate(swhere.ToString());
        }
    }
}
