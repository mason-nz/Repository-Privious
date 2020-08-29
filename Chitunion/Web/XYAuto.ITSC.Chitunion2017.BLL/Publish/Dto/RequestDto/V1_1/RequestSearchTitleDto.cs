using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1
{
    public class RequestSearchTitleDto
    {
        public RequestSearchTitleDto()
        {
            PageSize = 10;
        }

        [Necessary(MtName = "Keyword")]
        public string Keyword { get; set; }//关键字查询

        public int CreateUserId { get; set; }

        public int MediaId { get; set; }
        public int PageSize { get; set; }
    }
}