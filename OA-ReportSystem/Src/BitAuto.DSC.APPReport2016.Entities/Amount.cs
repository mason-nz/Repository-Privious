using System;
namespace BitAuto.DSC.APPReport2016.Entities
{
    public class AmountModel
    {
        public int YearMonth { get; set; }
        public int ItemId { get; set; }
        public decimal Amount { get; set; }
        public decimal Percent { get; set; }
        public decimal? MonthBasis { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
