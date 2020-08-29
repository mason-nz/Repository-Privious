using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto
{
    public class RecommendSearchDto : CreatePublishQueryBase
    {
        [Necessary(MtName = "CategoryId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入分类Id")]
        public int CategoryId { get; set; }

        public string MediaName { get; set; }
    }

    public class RankingQueryDto : CreatePublishQueryBase
    {
        public string Keyword { get; set; }

        public int CategoryId { get; set; }
        public new int PageSize { get; set; } = 100;
    }
}