using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto
{
    public class RequestMediaWeiXinDto : CreateBusinessBaseDto
    {
        [Necessary(MtName = "二维码url")]
        public string TwoCodeURL { get; set; }

        //[Necessary(MtName = "头像url")]
        //public string HeadIconURL { get; set; }

        [Necessary(MtName = "男粉丝数比例", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public decimal FansMalePer { get; set; }

        [Necessary(MtName = "女粉丝数比例", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public decimal FansFemalePer { get; set; }

        [Necessary(MtName = "AreaID", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AreaID { get; set; }//媒体领域

        [Necessary(MtName = "LevelType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int LevelType { get; set; }//媒体级别

        [Necessary(MtName = "粉丝数url")]
        public string FansCountURL { get; set; }

        public string OrderRemark { get; set; }
        [Necessary(MtName = "Sign")]
        public string Sign { get; set; }
        public bool IsAuth { get; set; }
        public bool IsReserve { get; set; }

        /* 互动参数 */
        public int RecID { get; set; }

        [Necessary(MtName = "ReferReadCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int ReferReadCount { get; set; }

        [Necessary(MtName = "AveragePointCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AveragePointCount { get; set; }

        [Necessary(MtName = "MoreReadCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int MoreReadCount { get; set; }

        [Necessary(MtName = "OrigArticleCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int OrigArticleCount { get; set; }
        [Necessary(MtName = "UpdateCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int UpdateCount { get; set; }
        [Necessary(MtName = "MaxinumReading", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int MaxinumReading { get; set; }

        //[Necessary(MtName = "ScreenShotURL")]
        public string ScreenShotURL { get; set; }

    }
}
