using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query.Publish
{
    public class PublishQuery<T> : QueryPageBase<T>
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int PublishStatus { get; set; }
        public int EndTime { get; set; }//到期时间:传入的参数是 7 天 或者 10天
        public string Source { get; set; }

        public List<int> MediaId { get; set; }
        public string MediaIds { get; set; }
        public int MediaType { get; set; }

        public int Media_Id { get; set; }

        public int BusinessType { get; set; }
    }

    public class FrontPublishQuery<T> : PublishQuery<T>
    {
    }

    public class QueryPublishItemInfo : QueryPageBase<QueryPublishItemInfo>
    {
    }

    public class PublishSearchAutoQuery<T> : QueryPageBase<T>
    {
        public PublishSearchAutoQuery()
        {
            MediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
            BusinessType = Entities.Constants.Constant.INT_INVALID_VALUE;
            Wx_Status = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        //public string Keyword { get; set; }//关键字查询
        public int CreateUserId { get; set; }

        public int MediaId { get; set; }

        public int Wx_Status { get; set; }//刊例审核状态

        public int BusinessType { get; set; }
    }

    public class PublishAuditInfoQuery<T> : QueryPageBase<T>
    {
        public PublishAuditInfoQuery()
        {
            MediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            PubId = Entities.Constants.Constant.INT_INVALID_VALUE;
            CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
            BusinessType = Entities.Constants.Constant.INT_INVALID_VALUE;
            TemplateId = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int CreateUserId { get; set; }
        public int MediaId { get; set; }
        public int PubId { get; set; }
        public int BusinessType { get; set; }
        public int TemplateId { get; set; }
    }
}