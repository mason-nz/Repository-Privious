using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //数据概况
    public class StatDataProfiling
    {
        //主键
        public int RecId { get; set; }

        //头部文章抓取量
        public int HeadArticle { get; set; }

        //头部文章抓取量覆盖账号
        public int HeadArticleAccount { get; set; }

        //头部文章抓机洗入库量
        public int HeadAutoClean { get; set; }

        //头部文章抓机洗入库量覆盖账号
        public int HeadAutoCleanAccount { get; set; }

        //腰部文章抓取量
        public int WaistArticle { get; set; }

        //腰部文章机洗入库量
        public int WaistArticleClean { get; set; }

        //腰部文章匹配车型量
        public int WaistArticleMatched { get; set; }

        //腰部文章未匹配车型量
        public int WaistArticleUnmatched { get; set; }

        //物料封装量
        public int MaterialPackaged { get; set; }

        //物料分发量
        public int MaterialDistribute { get; set; }

        //物料转发量
        public int MaterialForward { get; set; }

        //获取线索量
        public int Clues { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //统计日期
        public DateTime Date { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}