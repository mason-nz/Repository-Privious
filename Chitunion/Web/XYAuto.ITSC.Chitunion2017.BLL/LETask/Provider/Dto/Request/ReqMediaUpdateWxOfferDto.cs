using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request
{
    public class ReqMediaUpdateWxOfferDto : ReqMediaUpdateOfferDto
    {

        [Necessary(MtName = "FansCount", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入FansCount")]
        public int FansCount { get; set; }
        
        [Necessary(MtName = "FansMalePer", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入FansMalePer")]
        public decimal FansMalePer { get; set; }

        [Necessary(MtName = "FansFemalePer", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入FansFemalePer")]
        public decimal FansFemalePer { get; set; }

    }
    
}
