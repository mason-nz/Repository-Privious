using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1
{
    public class RequestAuditInfoQueryDto //: CreatePublishQueryBase
    {
        public RequestAuditInfoQueryDto()
        {
            MediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            PubId = Entities.Constants.Constant.INT_INVALID_VALUE;
            CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
            BusinessType = Entities.Constants.Constant.INT_INVALID_VALUE;
            TemplateId = Entities.Constants.Constant.INT_INVALID_VALUE;
            TopSize = 1;
        }

        public int CreateUserId { get; set; }
        public int MediaId { get; set; }
        public int PubId { get; set; }
        public int BusinessType { get; set; }
        public int TopSize { get; set; }
        public int TemplateId { get; set; }
    }
}