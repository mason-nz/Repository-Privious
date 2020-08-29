/********************************************************
*创建人：lixiong
*创建时间：2017/6/6 11:19:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1
{
    public class RespAdTemplateDto
    {
        public int TemplateID { get; set; }
        public string BaseMediaName { get; set; }
        public string BaseMediaLogoUrl { get; set; }
        public int BaseMediaID { get; set; }
        public int BaseAdID { get; set; }
        public string AdTemplateName { get; set; }
        public DateTime CreateTime { get; set; }
        public string AuditUser { get; set; }
        public string SubmitUser { get; set; }
        public int AuditStatus { get; set; }
    }
}