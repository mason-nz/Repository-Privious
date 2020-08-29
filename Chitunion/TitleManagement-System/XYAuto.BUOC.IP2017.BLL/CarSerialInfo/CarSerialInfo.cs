using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XYAuto.BUOC.IP2017.BLL.Business.Support;
using XYAuto.BUOC.IP2017.Entities.DTO;

namespace XYAuto.BUOC.IP2017.BLL.CarSerialInfo
{
    public class CarSerial
    {
        #region Instance

        public static readonly CarSerial Instance = new CarSerial();

        #endregion Instance

        public List<RespCarBrandDto> GetMasterBrandList()
        {
            return CacheHelper<List<RespCarBrandDto>>.Get(HttpRuntime.Cache,
                () => "xy.chitu.master.brand",
                () => Dal.CarSerialInfo.CarSerial.Instance.GetMasterBrandList(), null);
        }

        public List<RespCarBrandDto> GetBrandList(int masterBrandId)
        {
            return Dal.CarSerialInfo.CarSerial.Instance.GetBrandList(masterBrandId);
        }

        public List<RespCarSerialDto> GetCarSerialList(int brandId)
        {
            return Dal.CarSerialInfo.CarSerial.Instance.GetCarSerialList(brandId);
        }
        public List<RespCarSerialDto> GetCarSerialList()
        {
            return Dal.CarSerialInfo.CarSerial.Instance.GetCarSerialList();
        }
    }
}
