using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.AdTemplate
{
    //SaleAreaRelation
    public class SaleAreaRelation
    {
        public int RecID { get; set; }

        //城市组ID
        public int GroupID { get; set; }

        //省ID
        public int ProvinceID { get; set; }

        //城市ID
        public int CityID { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }

    public class SaleAreaRelationTable
    {
        public SaleAreaRelationTable()
        {
            this.CreateTime = DateTime.Now;
            this.TemplateID = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int RecID { get; set; }

        //城市组ID
        public int GroupID { get; set; }

        //省ID
        public int ProvinceID { get; set; }

        //城市ID
        public int CityID { get; set; }

        public int IsPublic { get; set; }
        public int TemplateID { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}