using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class AEMediaInfoDTO
    {
        public List<MediaWeixin> MediaList { get; set; }
        public List<MediaCommonlyClass> CategoryList { get; set; }
        public List<PublishRemark> RemarkList { get; set; }
        public List<MediaAreaMapping> AreaList { get; set; }
    }
}
