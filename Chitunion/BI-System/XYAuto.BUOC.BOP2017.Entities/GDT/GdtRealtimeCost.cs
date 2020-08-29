/********************************************************
*创建人：lixiong
*创建时间：2017/8/21 13:48:52
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    public class GdtRealtimeCost
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int Level { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }
        public int CampaignId { get; set; }
        public int AdgroupId { get; set; }
        public DateTime PullTime { get; set; }
    }
}