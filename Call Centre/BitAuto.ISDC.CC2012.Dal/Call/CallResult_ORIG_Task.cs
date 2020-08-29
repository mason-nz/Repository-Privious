using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CallResult_ORIG_Task : DataBase
    {
        public static CallResult_ORIG_Task Instance = new CallResult_ORIG_Task();

        /// 根据业务ID获取实体类型
        /// <summary>
        /// 根据业务ID获取实体类型
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public CallResult_ORIG_TaskInfo GetCallResult_ORIG_TaskInfoByBusinessID(string BusinessID)
        {
            string sql = "SELECT * FROM CallResult_ORIG_Task WHERE BusinessID='" + BusinessID + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new CallResult_ORIG_TaskInfo(dt.Rows[0]);
            }
            else return null;
        }
    }
}
