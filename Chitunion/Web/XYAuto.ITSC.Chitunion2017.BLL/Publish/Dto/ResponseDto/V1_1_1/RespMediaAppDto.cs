/********************************************************
*创建人：lixiong
*创建时间：2017/6/6 11:53:56
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1
{
    public class RespMediaAppBaseDto
    {
        public int MediaID { get; set; }
        public int BaseMediaID { get; set; }
        public int AdTemplateId { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }
        public int DailyLive { get; set; }
        public string TrueName { get; set; }
        public DateTime CreateTime { get; set; }

        [JsonIgnore]
        public string CommonlyClassStr { get; set; }

        public List<CommonlyClassDto> CommonlyClass { get; set; }//常见分类
    }

    public class RespMediaAppDto : RespMediaAppBaseDto
    {
        public string MediaRelationsName { get; set; }
        public string OperatingTypeName { get; set; }
        public int UpShelfAdCount { get; set; }
        public int OtherAdCount { get; set; }

        [JsonIgnore]
        public string AdCount { get; set; }

        public string AuditUser { get; set; }
        public List<CoverageAreaDto> CoverageArea { get; set; }//覆盖区域
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CreateUser { get; set; }

        public int HasOnPub { get; set; }

        public int AuditStatus { get; set; }
    }

    public class RespMediaAppByYunYingDto : RespMediaAppBaseDto
    {
        public string CreateUserRole { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public int HasOnPub { get; set; }
    }
}