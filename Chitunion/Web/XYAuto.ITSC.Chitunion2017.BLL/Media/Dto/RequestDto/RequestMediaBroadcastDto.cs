using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto
{
    public class RequestMediaBroadcastDto : CreateBusinessBaseDto
    {
        public int MediaID { get; set; }

        [Necessary(MtName = "Platform", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int Platform { get; set; }

        public string RoomID { get; set; }

        [Necessary(MtName = "Sex")]
        public string Sex { get; set; }

        [Necessary(MtName = "Profession", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int Profession { get; set; }

        [Necessary(MtName = "粉丝数截图url")]
        public string FansCountURL { get; set; }

        [Necessary(MtName = "LevelType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int LevelType { get; set; }//媒体级别

        public bool IsAuth { get; set; }

        public bool IsReserve { get; set; }

        /* 互动参数 */
        public int RecID { get; set; }

        [Necessary(MtName = "AudienceCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AudienceCount { get; set; }

        [Necessary(MtName = "MaximumAudience", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int MaximumAudience { get; set; }

        [Necessary(MtName = "AverageAudience", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AverageAudience { get; set; }

        [Necessary(MtName = "CumulateReward", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int CumulateReward { get; set; }

        [Necessary(MtName = "CumulateIncome", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int CumulateIncome { get; set; }

        [Necessary(MtName = "CumulatePoints", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int CumulatePoints { get; set; }

        [Necessary(MtName = "CumulateSendCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int CumulateSendCount { get; set; }

        public string ScreenShotURL { get; set; }
    }
}