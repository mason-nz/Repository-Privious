/********************************************************
*创建人：lixiong
*创建时间：2017/5/25 9:30:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class MediaDifferenceInfo
    {
        public int FansCount { get; set; }

        public decimal FansMalePer { get; set; }

        public decimal FansFemalePer { get; set; }

        public List<int> CommonlyClass { get; set; }

        public List<FansAreaDto> FansArea { get; set; }

        public string FansAreaShotUrl { get; set; }//粉丝区域截图

        public string FansSexScaleUrl { get; set; }//男女粉丝比例截图

        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public List<CoverageAreaDto> AreaMedia { get; set; }//区域媒体
        public bool IsAreaMedia { get; set; }
        public List<OrderRemarkDto> OrderRemark { get; set; }//下单备注
    }
}