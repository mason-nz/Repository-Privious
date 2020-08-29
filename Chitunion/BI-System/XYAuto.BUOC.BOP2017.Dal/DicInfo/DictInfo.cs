using System.Data;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.DicInfo
{
    public class DictInfo : DataBase
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeId(int typeId)
        {
            string sql = string.Format("SELECT DictId,DictName FROM DictInfo WHERE Status=0 And DictType>0 AND DictType={0} ORDER BY OrderNum", typeId);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
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