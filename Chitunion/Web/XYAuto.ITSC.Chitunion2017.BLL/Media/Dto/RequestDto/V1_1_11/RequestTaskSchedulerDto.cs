/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 11:14:06
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11
{
    public class RequestTaskSchedulerDto : CreatePublishQueryBase
    {
        public int ChannelId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string Category { get; set; }
        public int UserId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public string StartDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string EndDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        public int TaskStatus { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int CarSerialId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}