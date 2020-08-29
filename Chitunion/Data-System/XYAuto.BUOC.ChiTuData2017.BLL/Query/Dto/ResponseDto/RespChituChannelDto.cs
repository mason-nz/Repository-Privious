/********************************************************
*创建人：lixiong
*创建时间：2017/9/14 16:21:40
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto
{
    public class RespChituChannelDto
    {
        public int ChannelId { get; set; }
        public int DistributeId { get; set; }
        public int MaterielId { get; set; }
        public string PromotionUrlCode { get; set; }

        public int CreateUserId { get; set; }

        //日期
        public DateTime Dt { get; set; }

        //渠道
        public string Channel { get; set; }

        //pv
        public int Pv { get; set; }

        //uv
        public int Uv { get; set; }

        //平均访问时间
        public double Avg_dur { get; set; }

        //订单量
        public int Orders { get; set; }

        //订单独立手机号
        public int Order_Phones { get; set; }

        /* 冗余 */
        public int DistributeDetailType { get; set; }
    }
}