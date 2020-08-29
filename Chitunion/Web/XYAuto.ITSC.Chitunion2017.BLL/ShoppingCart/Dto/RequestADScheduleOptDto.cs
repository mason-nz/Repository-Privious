using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto
{
    public class RequestADScheduleOptDto
    {
        public int MediaType { get; set; } = -2;
        public int OptType { get; set; } = -2;
        public int CartID { get; set; } = -2;
        public int RecID { get; set; } = -2;
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;

            if (OptType != 1 && OptType != 2 && OptType != 3)
                sb.Append("参数操作类型OptType错误!");


            if (!Enum.IsDefined(typeof(Entities.EnumMediaType),MediaType))
                sb.Append("参数媒体类型错误!");

            if (Convert.ToDateTime(BeginTime.ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                sb.Append("排期开始日期必须大于当天!\n");


            if (MediaType == 14002)
            {
                if (Convert.ToDateTime(BeginTime.ToShortDateString()) > Convert.ToDateTime(EndTime.ToShortDateString()))
                    sb.Append("广告位排期结束时间要大于开始时间!\n");

                if (Convert.ToDateTime(EndTime.ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    sb.Append("排期结束日期必须大于当天!\n");

                if (Convert.ToDateTime(BeginTime.ToShortDateString()).AddMonths(6) < Convert.ToDateTime(EndTime.ToShortDateString()))
                    sb.Append("排期时间跨度不能超过6个月!\n");

            }


            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
