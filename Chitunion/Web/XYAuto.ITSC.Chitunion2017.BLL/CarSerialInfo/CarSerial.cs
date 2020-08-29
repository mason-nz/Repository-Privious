/********************************************************
*创建人：lixiong
*创建时间：2017/7/25 9:59:55
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Support;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;

namespace XYAuto.ITSC.Chitunion2017.BLL.CarSerialInfo
{
    public class CarSerial
    {
        #region Instance

        public static readonly CarSerial Instance = new CarSerial();

        #endregion Instance

        public List<RespCarMasterDto> GetMasterBrandList()
        {
            return CacheHelper<List<RespCarMasterDto>>.Get(HttpRuntime.Cache,
                () => "xy.chitu.master.brand",
                () => Dal.CarSerialInfo.CarSerial.Instance.GetMasterBrandList(), null);
        }

        public List<RespCarBrandDto> GetBrandList(int masterBrandId)
        {
            return Dal.CarSerialInfo.CarSerial.Instance.GetBrandList(masterBrandId);
        }

        /// <summary>
        /// 根据主品牌信息，查询品牌、车型相关信息
        /// </summary>
        /// <param name="masterBrandId">主品牌ID</param>
        /// <returns></returns>
        public List<RespCarAllInfoDto> GetCarAllInfoListCache(int minute = 10)
        {
            return CacheHelper<List<RespCarAllInfoDto>>.Get(HttpRuntime.Cache,
                () => "xy.chitu.carallinfoJson",
                () => GetCarAllInfoList(string.Empty), null, minute);
        }

        /// <summary>
        /// 根据主品牌信息，查询品牌、车型相关信息
        /// </summary>
        /// <param name="masterBrandName">主品牌名称</param>
        /// <returns></returns>
        public List<RespCarAllInfoDto> GetCarAllInfoList(string queryMasterName)
        {
            List<RespCarAllInfoDto> list = new List<RespCarAllInfoDto>();
            DataTable dt = Dal.CarSerialInfo.CarSerial.Instance.GetBrandSerialList(queryMasterName);

            if (dt != null && dt.Rows.Count > 0)
            {
                //dt.DefaultView.RowFilter = string.Empty;
                //dt.DefaultView.RowStateFilter = DataViewRowState.None;
                DataView dv = new DataView(dt);
                DataTable dtCarMaster = dv.ToTable(true, "MasterId", "MasterName");
                if (dtCarMaster != null && dtCarMaster.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCarMaster.Rows)
                    {
                        int masterId = int.Parse(dr["MasterId"].ToString());
                        string masterName = dr["MasterName"].ToString();

                        dv.RowFilter = "MasterId=" + masterId;
                        DataTable dtBrand = dv.ToTable(true, "BrandID", "BrandName");
                        List<RespCarBrandSerialDto> listBrand = new List<RespCarBrandSerialDto>();
                        if (dtBrand != null && dtBrand.Rows.Count > 0)
                        {
                            foreach (DataRow drBrand in dtBrand.Rows)
                            {
                                int brandID = int.Parse(drBrand["BrandID"].ToString());
                                string brandName = drBrand["BrandName"].ToString();

                                dv.RowFilter = "BrandID=" + brandID;
                                DataTable dtSerial = dv.ToTable(true, "SerialID", "ShowName");
                                List<RespCarSerialOnlyDto> listSerial = new List<RespCarSerialOnlyDto>();
                                if (dtSerial != null && dtSerial.Rows.Count > 0)
                                {
                                    foreach (DataRow drSerial in dtSerial.Rows)
                                    {
                                        int serialID = int.Parse(drSerial["SerialID"].ToString());
                                        string showName = drSerial["ShowName"].ToString();

                                        listSerial.Add(new RespCarSerialOnlyDto { CarSerialId = serialID, ShowName = showName });
                                    }
                                }
                                listBrand.Add(new RespCarBrandSerialDto { BrandId = brandID, BrandName = brandName, carSerialList = listSerial });
                            }
                        }
                        list.Add(new RespCarAllInfoDto { MasterBrandID = masterId, MasterBrandName = masterName, carBrandList = listBrand });
                    }
                    return list;
                }
            }
            return null;
        }

        public List<RespCarSerialDto> GetCarSerialList(int brandId)
        {
            return Dal.CarSerialInfo.CarSerial.Instance.GetCarSerialList(brandId);
        }

        public List<RespCarMasterDto> GetMasterListByName(string masterName)
        {
            return Dal.CarSerialInfo.CarSerial.Instance.GetMasterListByName(masterName);
        }
    }
}