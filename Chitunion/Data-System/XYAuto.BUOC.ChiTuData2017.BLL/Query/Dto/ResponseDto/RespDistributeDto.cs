/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 15:58:15
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto
{
    public class RespDistributeChannelDto : RespDistributeDto
    {
        public List<RespDistributeDto> Channel { get; set; }
    }

    public class RespDistributeDto
    {
        [JsonIgnore]
        public int ChannelCount { get; set; }

        public int MaterielId { get; set; }
        public int DistributeId { get; set; }
        public DateTime Date { get; set; }
        public long PV { get; set; }
        public long UV { get; set; }

        //public int ChannelId { get; set; }
        //public string ChannelName { get; set; }

        [JsonIgnore]
        public double OnLineAvgTime { get; set; }

        public string OnLineAvgTimeFormt { get; set; }//要换算成为yyyy:MM:dd
        public int BrowsePageAvg { get; set; }
        public int InquiryNumber { get; set; }
        public decimal JumpProportion { get; set; }
        public int SessionNumber { get; set; }
        public int TelConnectNumber { get; set; }

        public int ForwardNumber { get; set; }
    }

    public class RespDistributeListDto : RespDistributeDto
    {
        [JsonIgnore]
        public string Url { get; set; }

        public string Title { get; set; }

        [JsonIgnore]
        public int Source { get; set; }

        /// <summary>
        /// 分发类型
        /// </summary>
        public int DistributeType { get; set; }

        public string DistributeTypeName { get; set; }

        /// <summary>
        /// 组装操作人（物料表创建人）
        /// </summary>
        public string AssembleUser { get; set; }

        /// <summary>
        /// 组装时间
        /// </summary>
        public DateTime AssembleTime { get; set; }

        /// <summary>
        /// 分发操作人（渠道创建人）
        /// </summary>
        public string DistributeUser { get; set; }

        //分发时间
        public DateTime DistributeDate { get; set; }

        private string _distributeUrl;

        public string DistributeUrl
        {
            get
            {
                if (DistributeType == (int)DistributeTypeEnum.QuanWangYu)
                {
                    return Url;
                }
                return _distributeUrl;
            }
            set { value = _distributeUrl; }
        }//分发url

        [JsonIgnore]
        public int CreateUserId { get; set; }

        public int TotalDistribute { get; set; }

        public int TotalBrowsePageAvg { get; set; }
        public int TotalInquiryNumber { get; set; }
        public int TotalTelConnectNumber { get; set; }
    }
}