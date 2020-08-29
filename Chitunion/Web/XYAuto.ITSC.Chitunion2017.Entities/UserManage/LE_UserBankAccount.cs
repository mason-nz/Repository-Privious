using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.UserManage
{
    public enum AccountType
    {
        支付宝=96001,
        银行卡
    }
    public class LE_UserBankAccount
    {
        public int RecID { get; set; } = -2;
        public int UserID { get; set; } = -2;
        public string AccountName { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int Status { get; set; } = -2;
        public int AccountType { get; set; } = -2;
    }
}
