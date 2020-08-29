using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //用户银行账户表
    public class LeUserBankAccount
    {

        //主键
        public int RecID { get; set; }

        //用户ID
        public int UserID { get; set; }

        //账户名称
        public string AccountName { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }

        //枚举
        public int AccountType { get; set; }

        public string AccountTypeName { get; set; }


    }
}