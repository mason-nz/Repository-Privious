/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 20:16:33
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1
{
    public class RespCarBrandDto
    {
        public int MasterId { get; set; }
        public string MasterName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }

    }

    public class RespCarSerialDto : RespCarBrandDto
    {
        public int CarSerialId { get; set; }
        public string ShowName { get; set; }
    }





    public class RespCarMasterDto
    {
        public int MasterId { get; set; }
        public string Name { get; set; }
    }


    public class RespCarAllInfoDto
    {
        public int MasterBrandID { get; set; }
        public string MasterBrandName { get; set; }
        public List<RespCarBrandSerialDto> carBrandList { get; set; }
    }


    public class RespCarBrandSerialDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public List<RespCarSerialOnlyDto> carSerialList { get; set; }
    }

    public class RespCarSerialOnlyDto
    {
        public int CarSerialId { get; set; }
        public string ShowName { get; set; }
    }
}