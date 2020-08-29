using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.DictInfo
{
    public class DictInfo : DataBase
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeID(int typeID)
        {
            string sql = string.Format("SELECT DictId,DictName FROM DictInfo WHERE Status=0 And DictType>0 AND DictType={0} ORDER BY OrderNum", typeID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ITSC, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
    }
}
