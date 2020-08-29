using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class EmployeeItem
    {
        public static EmployeeItem Instance = new EmployeeItem();

        /// <summary>
        /// 获取最大日期
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public string GetMaxDate(int ItemType)
        {
            string sql = "select max(YearMonth) from EmployeeItem where 1=1  ";
            if (ItemType >= 0)
            {
                sql += " and ItemType='" + ItemType + "'";
            }

            object obj = SqlHelper.ExecuteScalar(DataBase.ConnectionStrings, CommandType.Text, sql);

            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemType"></param>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        public DataTable GetItemData(int ItemType, int YearMonth)
        {
            string sql = "select * from EmployeeItem where ItemType=" + ItemType + " and YearMonth=" + YearMonth + "";
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.Text, sql).Tables[0];
        }

    }
}
