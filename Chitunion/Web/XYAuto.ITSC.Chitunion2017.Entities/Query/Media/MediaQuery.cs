using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query.Media
{
    public class MediaQuery<T> : QueryPageBase<T>
    {
        public MediaQuery()
        {
            this.CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.MediaId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.WxId = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.MediaType = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.RelationType = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.PublishStatus = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.AuditStatus = Entities.Constants.Constant.INT_INVALID_VALUE;
            this.StatisticType = StatisticTypeEnum.Init;
        }

        public string Name { get; set; }
        public string Number { get; set; }
        public int MediaId { get; set; }
        public int WxId { get; set; }
        public int CreateUserId { get; set; }

        public int MediaType { get; set; }
        public int RelationType { get; set; }

        public int PublishStatus { get; set; }
        public int AuditStatus { get; set; }
        public StatisticTypeEnum StatisticType { get; set; }//统计类型 0:粉丝性别比 1:区域分布
    }
}