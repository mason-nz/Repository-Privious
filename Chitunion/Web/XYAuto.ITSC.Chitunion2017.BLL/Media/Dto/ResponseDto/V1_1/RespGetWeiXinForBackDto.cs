using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1
{
    public class RespGetWeiXinForBackDto : RespGetWeiXinDto
    {
        //public string OrderRemark { get; set; }
        public string CommonlyClassStr { get; set; }

        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        //public string FansAreaShotUrl { get; set; }//粉丝区域分布截图

        //public string Sign { get; set; }

        /// <summary>
        /// 媒体级别
        /// </summary>
        public string LevelTypeName { get; set; }

        public string TrueName { get; set; }
        public string UserName { get; set; }
        //public string Source { get; set; }
    }

    public class RespMediaAuditDetailViewDto
    {
        public RespGetWeiXinForBackDto MediaInfo { get; set; }
        public RespGetWeiXinForBackDto BaseMediaInfo { get; set; }
    }
}