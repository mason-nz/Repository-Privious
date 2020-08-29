using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage
{
    public class SearchAutoComplete
    {
        /// <summary>
        /// 返回前台搜索框自动填充内容
        /// </summary>
        /// <returns></returns>
        public List<SearchTitleResponse> GetSeaarchTitle(RequestFrontPublishQueryDto requestDto)
        {
            var query = new FrontPublishQuery<SearchTitleResponse>()
            {
                PageSize = requestDto.PageSize,
                KeyWord = XYAuto.Utils.StringHelper.SqlFilter(requestDto.Keyword),
                BusinessType = requestDto.BusinessType
            };
            if (query.PageSize > 20)
                query.PageSize = 20;
            return !Enum.IsDefined(typeof(MediaType), requestDto.BusinessType)
                ? new List<SearchTitleResponse>()
                : Dal.Publish.PublishInfoQuery.Instance.GetSearchAutoComplete(query);
        }
    }
}