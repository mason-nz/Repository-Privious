/********************************************************
*创建人：lixiong
*创建时间：2017/7/25 9:59:55
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Web;
using XYAuto.BUOC.ChiTuData2017.Entities.CarSerialInfo;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.CaChe;

namespace XYAuto.BUOC.ChiTuData2017.BLL.CarSerialInfo
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
    }
}