using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CustPhoneVisitBusiness : DataBase
    {
        public static CustPhoneVisitBusiness Instance = new CustPhoneVisitBusiness();

        /// 获取实体类
        /// <summary>
        /// 获取实体类
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="taskid"></param>
        /// <param name="businesstype"></param>
        /// <returns></returns>
        public CustPhoneVisitBusinessInfo GetCustPhoneVisitBusinessInfo(string phone, string taskid, int businesstype)
        {
            string sql = "SELECT * FROM CustPhoneVisitBusiness WHERE PhoneNum='" + SqlFilter(phone) + "'  AND TaskID='" + SqlFilter(taskid) + "' AND BusinessType=" + businesstype;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return new CustPhoneVisitBusinessInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
    }
}
