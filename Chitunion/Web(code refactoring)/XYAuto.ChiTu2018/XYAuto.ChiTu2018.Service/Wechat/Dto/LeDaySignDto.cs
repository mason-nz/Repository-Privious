/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.Wechat.Dto
* 类 名 称 ：LE_DaySignDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/11 15:48:47
********************************/

using System;

namespace XYAuto.ChiTu2018.Service.Wechat.Dto
{
    public class LeDaySignDto
    {

        public int RecID { get; set; }

        public DateTime? SignTime { get; set; }

        public int? SignUserID { get; set; }

        public int? SignNumber { get; set; }

        public decimal? SignPrice { get; set; }

        public string IP { get; set; }
    }
}
