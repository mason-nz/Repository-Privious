/********************************************************
*创建人：lixiong
*创建时间：2017/8/8 11:20:13
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1_11
{
    public class RespMaterielInfoDto
    {
        public int MaterielId { get; set; }
        public int ThirdMaterielId { get; set; }
        public string Name { get; set; }
        public int ArticleFrom { get; set; }
        public string ArticleFromName { get; set; }
        public int SerialId { get; set; }
        public string SerialName { get; set; }
        public string ContractNumber { get; set; }
        public MaterielContent Head { get; set; }
        public MaterielContent Body { get; set; }
        public MaterielContent Foot { get; set; }
        //public List<ChannelItem> ChannelInfo { get; set; }

        public List<ChannelItem> ChannelItem { get; set; }
    }

    public class MaterielContent
    {
        public int ContentType { get; set; }
        public string ContentTypeName { get; set; }
        public string ContentClass { get; set; }
        public string ContentTag { get; set; }
        public string Url { get; set; }
    }

    public class Channelinfo
    {
        public int ChannelID { get; set; }
        public string MediaTypeName { get; set; }
        public string MediaNumber { get; set; }
        public string MediaName { get; set; }
        public int ChannelType { get; set; }
        public string ChannelTypeName { get; set; }
        public int PayType { get; set; }
        public string PayTypeName { get; set; }
        public string PayModeName { get; set; }
        public int PayMode { get; set; }
        public string PromotionUrl { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class ChannelItem
    {
        public dynamic Name { get; set; }

        public List<Channelinfo> Channelinfo { get; set; }
    }
}