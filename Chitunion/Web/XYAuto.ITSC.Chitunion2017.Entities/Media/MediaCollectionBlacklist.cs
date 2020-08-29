using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class MediaCollectionBlacklist
    {
        public MediaCollectionBlacklist()
        {
            this.CreateTime = DateTime.Now;
        }

        //自增ID
        public int RecID { get; set; }

        //媒体Id
        public int MediaID { get; set; }

        //媒体类型
        public int MediaType { get; set; }

        //0:正常  1：删除
        public int Status { get; set; }

        //类型(1：收藏 2：拉黑)
        public int RelationType { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //创建人
        public int CreateUserID { get; set; }
    }
}