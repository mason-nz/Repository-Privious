using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.ENUM
{
    public class ENUM
    {
        /// <summary>
        /// 公用文章渠道类型
        /// </summary>
        public enum EnumResourceType
        {
            微信 = 1,
            汽车之家,
            今日头条,
            网易汽车,
            行圆新闻后台,
            搜狐
        }

        public enum EnumMediaType
        {
            微信 = 14001,
            APP = 14002,
            新浪微博 = 14003,
            视频 = 14004,
            直播 = 14005,
            头条 = 14006,
            搜狐 = 14007
        }

        public enum EnumLabelType
        {
            分类= 65001,
            市场场景,
            IP,
            标签,
            子IP,
            分发场景
        }

        public enum EnumBatchMediaStatus
        {
            待打=1001,
            待审,
            审核中,
            已审
        }

        public enum EnumTaskType
        {
            媒体= 2001,
            子品牌,
            车型
        }
    }
}
