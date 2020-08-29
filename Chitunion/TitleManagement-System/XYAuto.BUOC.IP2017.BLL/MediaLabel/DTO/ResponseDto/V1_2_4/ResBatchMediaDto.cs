using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.MediaLabel.DTO.ResponseDto.V1_2_4
{    
    public class ResBatchMediaDto
    {
        public int MediaType { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string HomeUrl { get; set; } = string.Empty;
        public List<ResBatchMediaCategoryDto> Category { get; set; }
        public List<ResBatchMediaCategoryDto> MarketScene { get; set; }
        public List<ResBatchMediaCategoryDto> DistributeScene { get; set; }
        public List<ResBatchMediaIplabelDto> IPLabel { get; set; }
        public string ArticleIDs { get; set; } = string.Empty;
    }

    public class ResBatchMediaCategoryDto
    {
        public int DictId { get; set; } = -2;
        public string DictName { get; set; } = string.Empty;
    }    

    public class ResBatchMediaIplabelDto
    {
        public int DictId { get; set; } = -2;
        public string DictName { get; set; } = string.Empty;
        public List<ResBatchMediaCategoryDto> SubIP { get; set; }
    }

}
