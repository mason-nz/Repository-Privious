using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_2_1
{
    public class ResponseArticleListDto
    {
        public int ArticleId { get; set; } = -2;
        public string Title { get; set; } = string.Empty;
        public int Resource { get; set; } = -2;
        public int CopyrightState { get; set; } = 2;
        public DateTime PublishTime { get; set; } = new DateTime(1990, 1, 1);
        public int ReadNum { get; set; } = 0;
        public int LikeNum { get; set; } = 0;
        public int ComNum { get; set; } = 0;
        public string Category { get; set; } = string.Empty;
    }
}
