using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CustomerCallInPressKey : DataBase
    {
        public readonly static CustomerCallInPressKey Instance = new CustomerCallInPressKey();

        /// 删除厂家id下所有数据
        /// <summary>
        /// 删除厂家id下所有数据
        /// </summary>
        /// <param name="vid"></param>
        public void DeleteDataByVenderCallID(string vendercallid)
        {
            string sql = "DELETE FROM CustomerCallInPressKey WHERE VenderCallID='" + StringHelper.SqlFilter(vendercallid) + "'";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql);
        }
    }
}
