using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestGetCommonlyCalssDto
    {
        public RequestGetCommonlyCalssDto()
        {
            this.BusinessType = (int)Entities.Enum.MediaType.WeiXin;
            this.Wx_Status = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.PageSize = 5;
        }

        public int BusinessType { get; set; }

        [Necessary(MtName = "MediaId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int MediaId { get; set; }

        public int PageSize { get; set; }
        public bool IsAuditPass { get; set; }
        public int Wx_Status { get; set; }
    }
}