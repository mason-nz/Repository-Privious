using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class Empolyee
    {
        public static Empolyee Instance = new Empolyee();

        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public int GetMaxDate()
        {
            string maxDate = Dal.Empolyee.Instance.GetMaxDate();
            try
            {
                //验证日期格式
                //格式为yyyyMM ，长度为6
                maxDate = maxDate.Trim();
                maxDate = maxDate.Substring(0, 4) + "-" + maxDate.Substring(4, 2);
                maxDate = DateTime.Parse(maxDate).ToString("yyyyMM");
            }
            catch
            {
                maxDate = DateTime.Now.ToString("yyyyMM");
            }

            return Int32.Parse(maxDate);
        }

        public DataTable GetData(int YearMonth)
        {
            return Dal.Empolyee.Instance.GetData(YearMonth);
        }

        public DataTable GetData(int StartDate, int EndDate)
        {
            return Dal.Empolyee.Instance.GetData(StartDate, EndDate);
        }



    }
}
