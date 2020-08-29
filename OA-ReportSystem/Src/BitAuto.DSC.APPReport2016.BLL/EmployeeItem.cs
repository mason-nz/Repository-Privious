using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class EmployeeItem
    {
        public static EmployeeItem Instance = new EmployeeItem();

        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public int GetMaxDate(int ItemType = 0)
        {
            string maxDate = Dal.EmployeeItem.Instance.GetMaxDate(ItemType);
            string yearMonth = "";
            if (!string.IsNullOrWhiteSpace(maxDate))
            {
                try
                {
                    yearMonth = DateTime.Parse(maxDate).ToString("yyyyMM");
                }
                catch
                {
                    yearMonth = maxDate;
                }
            }
            else
            {
                yearMonth = DateTime.Now.ToString("yyyyMM");
            }

            return int.Parse(yearMonth);
        }

        public DataTable GetItemData(int ItemType, int YearMonth)
        {
            return Dal.EmployeeItem.Instance.GetItemData(ItemType, YearMonth);
        }
    }
}
