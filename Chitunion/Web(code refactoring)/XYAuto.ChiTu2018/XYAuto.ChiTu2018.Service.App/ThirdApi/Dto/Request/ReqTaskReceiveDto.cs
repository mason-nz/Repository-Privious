using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;

namespace XYAuto.ChiTu2018.Service.App.ThirdApi.Dto.Request
{
    /// <summary>
    /// 注释：ReqTaskReceiveDto
    /// 作者：lix
    /// 日期：2018/5/21 20:00:01
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqTaskReceiveDto
    {
        [Necessary(MtName = "TaskType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入TaskType")]
        public int TaskType { get; set; }
        [Necessary(MtName = "TaskId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入TaskId")]
        public int TaskId { get; set; }
        public int MediaId { get; set; }
        public string Ip { get; set; }

        public int ChannelId { get; set; } = (int)LeOrderChannelTypeEnum.赤兔;

        public string OrderUrl { get; set; }

        public int UserId { get; set; }

        public int ShareType { get; set; }

        public int PromotionChannelId { get; set; }
    }
}
