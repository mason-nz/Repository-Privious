using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto
{
    public class CreateBusinessBaseDto
    {
    }

    public class CreatePublishQueryBase
    {
        public CreatePublishQueryBase()
        {
            PageIndex = 1;
            PageSize = 20;
            Source = Entities.Constants.Constant.INT_INVALID_VALUE;
            Status = Entities.Constants.Constant.INT_INVALID_VALUE;
            EndTime = Entities.Constants.Constant.INT_INVALID_VALUE;
            PublishStatus = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        [Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入合法的业务类型")]
        public int BusinessType { get; set; }//业务类型businesstype（微信：14001 APP：14002 微博：14003 视频：14004 直播：14005）

        public string Number { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int PublishStatus { get; set; }
        public int EndTime { get; set; }//到期时间:传入的参数是 7 天 或者 10天
        public int Source { get; set; }//来源

        public int OrderBy { get; set; }//排序

        [Necessary(MtName = "PageIndex", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录条数
        /// </summary>
        [Necessary(MtName = "PageSize", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int PageSize { get; set; }

        public string SqlWhere { get; set; }

        public int UserId { get; set; }
    }
}