using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Distribute
{
    //MaterielExtend
    public class MaterielExtend
    {
        //自增Id
        public int MaterielId { get; set; }

        public int Resource { get; set; }

        //头腿、腰腿、头腰腿、腿
        public int MaterielType { get; set; }

        public int SceneId { get; set; }

        //0:正常，-1：作废
        public int Status { get; set; }

        public string Memo { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        //外部物料ID
        public int ThirdId { get; set; }

        //文章来源
        public int ArticleFrom { get; set; }

        //头部内容形式
        public int HeadContentType { get; set; }

        //腰部内容形式
        public int BodyContentType { get; set; }

        //合同号
        public string ContractNumber { get; set; }

        //创建人
        public int CreateUserId { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //更新时间
        public DateTime LastUpdateTime { get; set; }

        //物料名称
        public string Name { get; set; }

        //文章id
        public int ArticleID { get; set; }

        //头部内容url
        public string HeadContentURL { get; set; }

        //腿部内容url
        public string FootContentURL { get; set; }

        //车型id
        public int SerialId { get; set; }

        //标签
        public string Tag { get; set; }

        //分类
        public string Category { get; set; }

        public string MaterielUrl { get; set; }
    }
}