using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class YTGCallResultNotice : DataBase
    {
        private YTGCallResultNotice() { }
        private static YTGCallResultNotice _instance = null;
        public static YTGCallResultNotice Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGCallResultNotice();
                }
                return _instance;
            }
        }

        /// 获取需求回传的数据
        /// <summary>
        /// 获取需求回传的数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetNeedCallResultNotice(int attempts)
        {
            string sql = @"SELECT  a.* ,
                                                b.SignID ,
                                                b.DealerID ,
                                                b.IsSuccess ,
                                                b.FailReason ,
                                                b.Remark ,
                                                b.LastUpdateUserID
                                    FROM    dbo.YTGCallResultNotice a
                                            INNER JOIN dbo.YTGActivityTask b ON ( a.TaskID = b.TaskID )
                                    WHERE   a.Status = 0
                                            OR ( a.Status = 2
                                                 AND a.FailCount < " + attempts + @"
                                               )";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
    }
}
