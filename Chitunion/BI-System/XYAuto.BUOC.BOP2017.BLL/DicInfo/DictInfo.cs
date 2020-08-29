using System.Data;

namespace XYAuto.BUOC.BOP2017.BLL.DicInfo
{
    public class DictInfo
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeId(int typeId)
        {
            return Dal.DicInfo.DictInfo.Instance.GetDictInfoByTypeId(typeId);
        }
    }
}