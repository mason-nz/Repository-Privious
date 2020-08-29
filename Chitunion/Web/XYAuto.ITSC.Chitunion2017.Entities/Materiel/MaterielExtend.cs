using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Materiel
{
    //MaterielExtend
    public class MaterielExtend
    {
        //自增Id
        public int MaterielID { get; set; }

        public string Name { get; set; }

        //外部物料ID
        public int ThirdID { get; set; }

        //文章来源
        public int ArticleFrom { get; set; }

        //头部内容形式
        public int HeadContentType { get; set; }

        public int ArticleID { get; set; }

        public string Tag { get; set; }

        public string Category { get; set; }

        //腰部内容形式
        public int BodyContentType { get; set; }

        //合同号
        public string ContractNumber { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //更新时间
        public DateTime LastUpdateTime { get; set; }

        #region 字段

        public int SerialId { get; set; }
        public string SerialName { get; set; }
        public string BrandName { get; set; }
        public string ArticleFromName { get; set; }//文章来源
        public string MaterielName { get; set; }

        public string HeadContentTypeName { get; set; }
        public string HeadContentClass { get; set; }
        public string HeadContentTag { get; set; }
        public string HeadContentUrl { get; set; }

        public string BodyContentTypeName { get; set; }

        public string FootContentUrl { get; set; }
        public string FootContentTypeName { get; set; }

        #endregion 字段
    }
}