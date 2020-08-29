using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto
{
    public class AdvIdToOptIdReqDTO
    {
        public int OperaterId { get; set; }
        public int OperateType { get; set; }
        public List<int> AdvertiserIds { get; set; }
    }
}