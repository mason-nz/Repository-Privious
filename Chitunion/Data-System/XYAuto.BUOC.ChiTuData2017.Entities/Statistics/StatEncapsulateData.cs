using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //封装数据明细
    public class StatEncapsulateData
    {
        //主键ID
        public int RecId { get; set; }

        //物料ID
        public int MaterialId { get; set; }

        //文章标题
        public string Title { get; set; }

        //文章URL
        public string Url { get; set; }

        //物料封装类型（枚举）
        public int MaterielTypeId { get; set; }

        //物料封装类型名称
        public string MaterialName { get; set; }

        //封装时间
        public DateTime EncapsulateTime { get; set; }

        //渠道ID
        public int ChannelId { get; set; }

        //渠道名称
        public string ChannelName { get; set; }

        //场景ID
        public int SceneId { get; set; }

        //场景名称
        public string SceneName { get; set; }

        //账号
        public string AccountName { get; set; }

        //账号分值（头部文章）
        public decimal AccountScore { get; set; }

        //文章分值（头部文章）
        public decimal ArticleScore { get; set; }

        //状态ID
        public int ConditionId { get; set; }

        //状态名称（可用、作废、置为腰）
        public string ConditionName { get; set; }

        //原因
        public string Reason { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}