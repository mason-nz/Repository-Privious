using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.ResponseDto
{
    public class ResponseRecommendSearch
    {
        public int RecID { get; set; }
        public int MediaID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }

        public int SortNumber { get; set; }

        public int FansCount { get; set; }
    }

    public class ResRecommendWeiXin : ResponseRecommendSearch
    {
        public int UpdateCount { get; set; }//更新次数

        public int ReferReadCount { get; set; }//参考阅读数

        public int AveragePointCount { get; set; }//平均点赞数

        public int MoreReadCount { get; set; }//10W+阅读量文章数

        public int OrigArticleCount { get; set; }//原创文章数

        public int MaxinumReading { get; set; }//最高阅读数
    }

    public class ResRecommendWeiBo : ResponseRecommendSearch
    {
        public int AverageForwardCount { get; set; }//平均转发数
        public int AverageCommentCount { get; set; }//平均评论数
        public int AveragePointCount { get; set; }//平均点赞数
    }

    public class ResRecommendVideo : ResponseRecommendSearch
    {
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int AveragePlayCount { get; set; }//平均播放数
        public int AveragePointCount { get; set; }//平均点赞数
        public int AverageCommentCount { get; set; }//平均评论数

        public int AverageBarrageCount { get; set; }//平均弹幕数
    }

    public class ResRecommendBroadcast : ResponseRecommendSearch
    {
        public string Sex { get; set; }

        public int CumulateReward { get; set; }//累计打赏数/收礼
    }

    public class ResRecommendApp
    {
        public string HeadIconURL { get; set; }
        public int RecID { get; set; }
        public int MediaID { get; set; }
        public string Name { get; set; }
        public int SortNumber { get; set; }
        public string AdPosition { get; set; } //广告位置
        public int AdForm { get; set; } //广告形式
        public string AdLegendURL { get; set; } //广告图例
    }
}