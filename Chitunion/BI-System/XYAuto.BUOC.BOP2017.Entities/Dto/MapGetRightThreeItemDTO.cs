using System;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class MapGetRightThreeItemDTO
    {
        public DateTime Date { get; set; } = Entities.Constants.Constant.DATE_INVALID_VALUE;
        public string Key { get; set; }
        public decimal Value { get; set; }
    }
}