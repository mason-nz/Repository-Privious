using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_DMS2014.Entities
{
    public class ServeTime
    {
        /// <summary>
        /// 小时
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// 分钟
        /// </summary>
        public int Min { get; set; }


        public ServeTime()
        {
        }

        public ServeTime(string time)
        {
            try
            {
                string[] array = time.Split(':');
                Hour = CommonFunc.ObjectToInteger(array[0]);
                Min = CommonFunc.ObjectToInteger(array[1]);
            }
            catch
            {
            }
        }

        public ServeTime(int hour, int min)
        {
            Hour = hour;
            Min = min;
        }

        public override string ToString()
        {
            return Hour + ":" + Min.ToString("00");
        }
    }
}
