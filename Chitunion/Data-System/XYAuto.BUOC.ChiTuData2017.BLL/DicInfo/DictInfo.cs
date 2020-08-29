using System.Collections.Generic;
using System.Data;
using System.Web;
using XYAuto.BUOC.ChiTuData2017.Entities.CarSerialInfo;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.CaChe;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DicInfo
{
    public class DictInfo
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeId(int typeId)
        {

            return Dal.DicInfo.DictInfo.Instance.GetDictInfoByTypeId(typeId);

        }

        public List<Entities.DictInfo.DictInfo> GetDictInfo()
        {
            return CacheHelper<List<Entities.DictInfo.DictInfo>>.Get(HttpRuntime.Cache,
             () => "xy.chitu-data.dicInfo",
             () => Dal.DicInfo.DictInfo.Instance.GetDictInfo(), null);
        }
    }
}