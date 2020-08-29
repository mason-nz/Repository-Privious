using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto
{
    public class RequestMediaWeiBoDto : CreateBusinessBaseDto
    {
        public RequestMediaWeiBoDto()
        {
            this.Sex = "-1";
        }

        public string Sex { get; set; }
        [Necessary(MtName = "粉丝数截图")]
        public string FansCountURL { get; set; }

        [Necessary(MtName = "FansSex")]
        public string FansSex { get; set; }

        [Necessary(MtName = "Profession", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int Profession { get; set; }
        [Necessary(MtName = "AuthType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AuthType { get; set; }//微博认证
        [Necessary(MtName = "LevelType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int LevelType { get; set; }//媒体级别

        [Necessary(MtName = "AreaID", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AreaID { get; set; }

        public string OrderRemark { get; set; }

        public bool IsReserve { get; set; }

        [Necessary(MtName = "Sign")]
        public string Sign { get; set; }

        /* 互动参数 */
        public int RecID { get; set; }

        //public int MediaID { get; set; }

        [Necessary(MtName = "AverageForwardCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AverageForwardCount { get; set; }
        [Necessary(MtName = "AverageCommentCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AverageCommentCount { get; set; }

        [Necessary(MtName = "AveragePointCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AveragePointCount { get; set; }

        public string ScreenShotURL { get; set; }

    }

    public class RequestMediaVideoDto : CreateBusinessBaseDto
    {
        public int MediaID { get; set; }
        [Necessary(MtName = "Platform", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int Platform { get; set; }

        [Necessary(MtName = "Sex")]
        public string Sex { get; set; }

        [Necessary(MtName = "粉丝数截图")]
        public string FansCountURL { get; set; }

        [Necessary(MtName = "Profession", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int Profession { get; set; }
        [Necessary(MtName = "LevelType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int LevelType { get; set; }//媒体级别

        public bool IsAuth { get; set; }

        public bool IsReserve { get; set; }
        // [Necessary(MtName = "Sign")]
        public string Sign { get; set; }

        /* 互动参数 */
        public int RecID { get; set; }


        [Necessary(MtName = "AveragePlayCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AveragePlayCount { get; set; }
        [Necessary(MtName = "AveragePointCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AveragePointCount { get; set; }
        [Necessary(MtName = "AverageCommentCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AverageCommentCount { get; set; }
        [Necessary(MtName = "AverageBarrageCount", IsValidateThanAt = true, ThanMaxValue = -1, Message = "{0}必须大于{1}")]
        public int AverageBarrageCount { get; set; }

        public string ScreenShotURL { get; set; }


    }
}
