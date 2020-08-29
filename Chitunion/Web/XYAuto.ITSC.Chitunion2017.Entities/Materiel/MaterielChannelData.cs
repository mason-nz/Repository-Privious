using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Materiel
{
    //MaterielChannelData
    public class MaterielChannelData
    {
        public int RecID { get; set; }

        //渠道ID
        public int ChannelID { get; set; }

        //物料ID
        public int MaterielID { get; set; }

        //数据日期
        public DateTime DataDate { get; set; }

        //阅读数
        public int ReadCount { get; set; }

        //点赞数
        public int LikeCount { get; set; }

        //评论数
        public int CommentCount { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //更新时间
        public DateTime LastUpdateTime { get; set; }

        public string MediaTypeName { get; set; }
        public string MediaName { get; set; }
        public string MediaNumber { get; set; }

        public string ChannelTypeName { get; set; }
        public string PayTypeName { get; set; }
        public string PayModeName { get; set; }
        public decimal UnitCost { get; set; }

        public int[] MaterielIDs { get; set; }
        public string MaterialName { get; set; }
        public string FootContentURL { get; set; }
        public string CarSerialName { get; set; }
        public string BrandName { get; set; }
    }
}