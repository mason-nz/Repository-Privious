using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.ZHY
{
    public class BaseAdvertiserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public List<int> SubGuestType { get; set; } = new List<int>() { 1 };
        public int Number { get; set; }
        public string Operater { get; set; }
        public string CompanyName { get; set; }
        public int AccountId { get; set; }
    }
    public class ZhyAdvertiserInfo : BaseAdvertiserInfo
    {

        public string CustomerName { get; set; }
        public int CashBalance { get; set; }
        public int VirtualBalance { get; set; }
        public int DividedBalance { get; set; }
        public int SilverCardBalance { get; set; }
        public int TodaySpend { get; set; }
    }
}