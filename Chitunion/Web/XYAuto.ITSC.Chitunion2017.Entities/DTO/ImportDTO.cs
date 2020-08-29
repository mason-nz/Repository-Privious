using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class ImportDTO
    {
        public MediaTypeEnum MediaType { get; set; }
        public dynamic MediaInfo { get; set; }

        public PublishBasicInfo PubBasicInfo { get; set; }
        public List<MediaAreaMapping> MappingList { get; set; }
        public List<PublishDetailInfo> PubDetailList { get; set; }
        public ADPositionDTO PubExtend { get; set; }
        public dynamic Interaction { get; set; }

        public string Key { get; set; }
        public string UserName { get; set; }
    }
}
