using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_2_1
{
    public class RequestArticleQueryDto : CreatePublishQueryBase
    {
        public string ArticleIds { get; set; } = string.Empty;
        public int Resource { get; set; } = -2;
        public int CarSerialId { get; set; } = -2;
        public int CopyrightState { get; set; } = -2;
        public DateTime StartDate { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
        public DateTime EndDate { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
    }
}
