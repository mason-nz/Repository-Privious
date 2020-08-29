using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CustomerCallIn : DataBase
    {
        public static CustomerCallIn Instance = new CustomerCallIn();

        /// 根据厂家id获取信息
        /// <summary>
        /// 根据厂家id获取信息
        /// </summary>
        /// <param name="VenderCallID"></param>
        /// <returns></returns>
        public CustomerCallInInfo GetCustomerCallInInfoByVenderCallID(string VenderCallID)
        {
            string sql = "SELECT * FROM CustomerCallIn WHERE VenderCallID='" + StringHelper.SqlFilter(VenderCallID) + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return new CustomerCallInInfo(dt.Rows[0]);
            }
            else return null;
        }
    }
}
