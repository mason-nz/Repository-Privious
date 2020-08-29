using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    //智慧云推送过来的广告主用户与广告运营角色绑定关系，告运营角色->（一对多）广告主
    public class GdtRoleUser
    {
        //自增Id
        public int Id { get; set; }

        //赤兔用户id
        public int UserId { get; set; }

        //授权用户（授权给哪个用户）
        public int AuthToUserId { get; set; }

        //创建人
        public int CreateUserId { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}