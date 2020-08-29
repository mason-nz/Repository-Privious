using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_DMS2014.Core
{
    [Serializable]
    public class EPInfoEntity
    {
        protected DateTime dtNow = DateTime.Now;
        /// <summary>
        /// 易湃登录账号ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 易湃登录账号名称（可以是用户真实姓名，也可以是用户E-mail地址）
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 易湃登录账号电话（可空）
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 易湃登录账号E-mail（可空）
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 易湃登录账号职务（可空）
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 易湃登录账号所属易湃会员编号
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 易湃登录账号当前时间戳，格式：yyyyMMddHHmmss（前后时差不应该超过30分钟）
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// 易湃登录账号所属应用ID（GUID格式）
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 判断易湃登录账号当前时间戳与当前时间间隔，是否在30分钟之内
        /// </summary>
        public bool IsValid
        {
            get
            {
                DateTime dt;
                dt = DateTime.ParseExact(this.DateTimeFormat, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                TimeSpan ts1 = new TimeSpan(dtNow.Ticks);
                TimeSpan ts2 = new TimeSpan(dt.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                return Math.Abs(ts.TotalMinutes) < 30 ? true : false;
            }
        }
    }
}
