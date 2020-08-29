using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Materiel
{
    //MaterielExtendFoot
    public class MaterielExtendFoot
    {
        public int RecID { get; set; }

        //物料ID
        public int MaterielID { get; set; }

        public int FootContentType { get; set; }

        public string FootContentUrl { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}