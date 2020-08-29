/********************************************************
*创建人：lixiong
*创建时间：2017/5/11 16:41:54
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.AdTemplate;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1
{
    public class RequestAdQueryDto : CreatePublishQueryBase
    {
        public RequestAdQueryDto()
        {
            //AdStatus = Entities.Constants.Constant.INT_INVALID_VALUE;
            Wx_Status = string.Empty;
            this.TemplateAuditStatus = (int)Entities.Enum.AppTemplateEnum.已通过;
        }

        public string Wx_Status { get; set; }

        //public int AdStatus { get; set; }//广告状态
        public string Keyword { get; set; }//帐号、名称查询

        public string AdName { get; set; }//广告名称
        public string MediaName { get; set; }//媒体
        public int CreateUserId { get; set; }
        public string SubmitUser { get; set; }//提交人
        public string StartDateTime { get; set; }//提交日期开始时间
        public string EndDateTime { get; set; }//提交日期结束时间
        public bool IsPassed { get; set; }//是否通过
        public bool IsAuditView { get; set; }//是否是审核页面
        public string AdTemplateName { get; set; }//广告模板名称
        public int TemplateAuditStatus { get; set; }

        public int IsAreaMedia { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int AreaProvniceId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int AreaCityId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}