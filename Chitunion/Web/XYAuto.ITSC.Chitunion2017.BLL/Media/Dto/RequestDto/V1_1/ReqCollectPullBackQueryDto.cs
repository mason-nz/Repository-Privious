using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class ReqCollectPullBackQueryDto : CreatePublishQueryBase
    {
        [Necessary(MtName = "OperateType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入合法的业务类型")]
        public int OperateType { get; set; }//类型(1：收藏 2：拉黑)

        public int CreateUserId { get; set; }
    }
}