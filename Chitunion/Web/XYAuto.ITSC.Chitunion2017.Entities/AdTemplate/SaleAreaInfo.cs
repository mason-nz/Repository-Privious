using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.AdTemplate
{
    //SaleAreaInfo
    public class SaleAreaInfo
    {
        //城市组ID
        public int GroupID { get; set; }

        //城市组名称
        public string GroupName { get; set; }

        //模板ID
        public int TemplateID { get; set; }

        //城市组类型 0:普通 1:全国 2:其它
        public int GroupType { get; set; }

        public bool IsPublic { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }

    public class SaleAreaInfoTable
    {
        public SaleAreaInfoTable()
        {
            this.CreateTime = DateTime.Now;
        }

        //城市组ID
        public int GroupID { get; set; }

        //城市组名称
        public string GroupName { get; set; }

        //模板ID
        public int TemplateID { get; set; }

        //城市组类型 0:普通 1:全国 2:其它
        public int GroupType { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }

    public class AreaInfoEntity
    {
        public string AreaID { get; set; }
        public string PID { get; set; }
        public string AreaName { get; set; }
    }
}