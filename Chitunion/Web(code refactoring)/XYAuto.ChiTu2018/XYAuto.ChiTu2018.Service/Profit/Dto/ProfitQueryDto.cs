/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.Profit.Dto
* 类 名 称 ：ProfitQueryDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 14:46:11
********************************/

using XYAuto.ChiTu2018.Service.BaseDto;

namespace XYAuto.ChiTu2018.Service.Profit.Dto
{
    public sealed class ProfitQueryDto:PaginationDto
    {
        public int UserID { get; set; }
    }
}
