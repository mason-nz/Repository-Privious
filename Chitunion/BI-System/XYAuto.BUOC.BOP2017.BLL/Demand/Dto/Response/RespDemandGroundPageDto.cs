/********************************************************
*创建人：lixiong
*创建时间：2017/10/12 11:09:18
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Response
{
    public class RespDemandGroundPageDto
    {
        //落地页id（自增）
        public int GroundId { get; set; }

        //需求单号
        public int DemandBillNo { get; set; }

        //落地页url
        public string PromotionUrl { get; set; }

        //状态（0正常）
        public int Status { get; set; } = 0;

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;

        //创建人
        public int CreateUserId { get; set; }

        public int DeliveryCount { get; set; }

        public int AuditStatus { get; set; }
        public List<DeliveryCarInfoDto> CarInfo { get; set; }
        public List<DeliveryCitysInfoDto> AreaInfo { get; set; }
    }
}