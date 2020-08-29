using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto
{
    public class RequestMediaPcAppDto : CreateBusinessBaseDto
    {
        public int MediaID { get; set; }
        [Necessary(MtName = "广告终端")]
        public string Terminal { get; set; }
        //[Necessary(MtName = "DailyLive", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public float DailyLive { get; set; }
        //[Necessary(MtName = "DailyIP", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public float DailyIP { get; set; }

        [Necessary(MtName = "网址")]
        public string WebSite { get; set; }
        [Necessary(MtName = "媒体介绍")]
        public string Remark { get; set; }


    }
}
