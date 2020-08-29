using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.WxEditor
{
    public class ArticleGroupInfo
    {
        public ArticleGroupInfo() {
            this.SourceType = (int)Entities.Enum.SourceTypeEnum.默认;
        }

        public int GroupID { get; set; }
        public string WxMaterialID { get; set; }
        public int ArticleCount { get; set; }
        public int SourceType { get; set; }
        public string FromUrl { get; set; }
        public int FromWxID { get; set; }
        public string FromWxNumber { get; set; }
        public string FromWxName { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int Status { get; set; }
    }
}
