using System;

namespace XYAuto.BUOC.BOP2017.Entities.GDT
{
    //赤兔用户，广点通代理商子客户（简称子客）之间的关联表
    public class GdtAccountRelation
    {
        //自增Id
        public int Id { get; set; }

        //赤兔用户id
        public int UserId { get; set; }

        //广点通代理商子客户（简称子客）帐号 id
        public int AccountId { get; set; }

        //创建人
        public int CreateUserId { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}