using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4
{    
    public class ResArticleQueryDto
    {
        public long ArticleId { get; set; } = -2;
        public string Title { get; set; } = string.Empty;
        public int Resource { get; set; } = -2;
        public int ReadNum { get; set; } = 0;
        public int LikeNum { get; set; } = 0;
        public int ComNum { get; set; } = 0;
        public DateTime PublishTime { get; set; } = new DateTime(1900, 1, 1);
        public string Url { get; set; } = string.Empty;
    }

    public class ResArticleQueryOrReciveDto
    {
        public object ListInfo { get; set; }
        public ResArticleQueryOrReciveMediainfo MediaInfo { get; set; }
    }
    public class ResArticleQueryOrReciveMediainfo
    {
        public int MediaType { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string HomeUrl { get; set; } = string.Empty;
    }

}
