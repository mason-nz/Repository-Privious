/********************************************************
*创建人：lixiong
*创建时间：2017/5/25 10:36:45
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class MediaAuditViewInfo
    {
        public int MediaID { get; set; }
        public string Number { get; set; }

        public string Name { get; set; }
        public string HeadIconURL { get; set; }
        public string TwoCodeURL { get; set; }
        public int FansCount { get; set; }

        public decimal FansMalePer { get; set; }

        public decimal FansFemalePer { get; set; }

        public List<int> CommonlyClass { get; set; }

        public List<FansAreaDto> FansArea { get; set; }

        public string FansAreaShotUrl { get; set; }//粉丝区域截图

        public string FansSexScaleUrl { get; set; }//男女粉丝比例截图

        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string OrderRemark { get; set; }

        public string WxNumber { get; set; }
        public string NickName { get; set; }
        public string HeadImg { get; set; }
        public string QrCodeUrl { get; set; }
        public int AuthFansCount { get; set; }
        public int AuthProvinceID { get; set; }
        public int AuthCityID { get; set; }
        public string AuthProvinceName { get; set; }
        public string AuthCityName { get; set; }
    }
}