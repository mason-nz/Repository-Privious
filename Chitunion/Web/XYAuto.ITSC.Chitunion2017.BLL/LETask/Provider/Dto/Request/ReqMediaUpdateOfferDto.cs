using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request
{
    public class ReqMediaUpdateOfferDto
    {
        [Necessary(MtName = "MediaId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入MediaId")]
        public int MediaId { get; set; }
        [Necessary(MtName = "CategoryId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入CategoryId")]
        public int CategoryId { get; set; }
        public Overlayarea OverlayArea { get; set; }
        public List<Deliveryprice> DeliveryPrices { get; set; }
    }

    public class Overlayarea
    {
        public int ProvinceId { get; set; } = -2;
        public int CityId { get; set; } = -2;
    }

    public class Deliveryprice
    {
        public int ADPosition1 { get; set; }
        public int ADPosition2 { get; set; }
        public decimal Price { get; set; }
    }

}
