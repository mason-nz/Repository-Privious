using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace BitAuto.DSC.IM_2015.BLL
{
    /// 字典数据缓存集合
    /// <summary>
    /// 字典数据缓存集合
    /// 强斐
    /// 2015-1-21
    /// </summary>
    public class DictionaryDataCache
    {
        public static DictionaryDataCache Instance = new DictionaryDataCache();

        #region 字典信息
        /// 车款信息
        /// <summary>
        /// 车款信息
        /// </summary>
        public Dictionary<string, DataRow> Car_Car
        {
            get
            {
                return GetData(DataCacheKey.Car_Car);
            }
        }
        ///  车型信息
        /// <summary>
        ///  车型信息
        /// </summary>
        public Dictionary<string, DataRow> Car_Serial
        {
            get
            {
                return GetData(DataCacheKey.Car_Serial);
            }
        }
        /// 品牌信息
        /// <summary>
        /// 品牌信息
        /// </summary>
        public Dictionary<string, DataRow> Car_Brand
        {
            get
            {
                return GetData(DataCacheKey.Car_Brand);
            }
        }
        /// 主品牌信息
        /// <summary>
        /// 主品牌信息
        /// </summary>
        public Dictionary<string, DataRow> Car_MasterBrand
        {
            get
            {
                return GetData(DataCacheKey.Car_MasterBrand);
            }
        }
        /// 省信息
        /// <summary>
        /// 省信息
        /// </summary>
        public Dictionary<string, DataRow> AreaInfo_Province
        {
            get
            {
                return GetData(DataCacheKey.AreaInfo_Province);
            }
        }
        /// 城市信息
        /// <summary>
        /// 城市信息
        /// </summary>
        public Dictionary<string, DataRow> AreaInfo_City
        {
            get
            {
                return GetData(DataCacheKey.AreaInfo_City);
            }
        }
        /// 区县信息
        /// <summary>
        /// 区县信息
        /// </summary>
        public Dictionary<string, DataRow> AreaInfo_County
        {
            get
            {
                return GetData(DataCacheKey.AreaInfo_County);
            }
        }
        /// 会员信息
        /// <summary>
        /// 会员信息
        /// </summary>
        public Dictionary<string, DataRow> DMSMember
        {
            get
            {
                return GetData(DataCacheKey.DMSMember);
            }
        }
        /// 专题活动
        /// <summary>
        /// 专题活动
        /// </summary>
        public Dictionary<string, DataRow> AcitvityInfo
        {
            get
            {
                return GetData(DataCacheKey.AcitvityInfo);
            }
        }
        #endregion

        public DictionaryDataCache()
        {
        }

        #region 对外接口
        /// 从缓存中获取表数据
        /// <summary>
        /// 从缓存中获取表数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataTable GetDataTableByKey(DataCacheKey key)
        {
            GetData(key);
            DataTable dt = DataCacheHelper.GetCache(key.ToString() + "_Table") as DataTable;
            return dt;
        }
        /// 预置缓存中的数据
        /// <summary>
        /// 预置缓存中的数据
        /// </summary>
        public void InitData()
        {
            GetData(DataCacheKey.Car_Car);
            GetData(DataCacheKey.Car_Serial);
            GetData(DataCacheKey.Car_Brand);
            GetData(DataCacheKey.Car_MasterBrand);
            GetData(DataCacheKey.AreaInfo_Province);
            GetData(DataCacheKey.AreaInfo_City);
            GetData(DataCacheKey.AreaInfo_County);
            GetData(DataCacheKey.DMSMember);
            GetData(DataCacheKey.AcitvityInfo);
        }
        /// 刷新缓存中的数据
        /// <summary>
        /// 刷新缓存中的数据
        /// </summary>
        public void ResetData()
        {
            CacheData(DataCacheKey.Car_Car);
            CacheData(DataCacheKey.Car_Serial);
            CacheData(DataCacheKey.Car_Brand);
            CacheData(DataCacheKey.Car_MasterBrand);
            CacheData(DataCacheKey.AreaInfo_Province);
            CacheData(DataCacheKey.AreaInfo_City);
            CacheData(DataCacheKey.AreaInfo_County);
            CacheData(DataCacheKey.DMSMember);
            CacheData(DataCacheKey.AcitvityInfo);
        }
        #endregion

        #region 辅助
        /// 获取数据-主入口
        /// <summary>
        /// 获取数据-主入口
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Dictionary<string, DataRow> GetData(DataCacheKey key)
        {
            //缓存字典
            Dictionary<string, DataRow> data = DataCacheHelper.GetCache(key.ToString()) as Dictionary<string, DataRow>;
            if (data == null)
            {
                //没有数据则查询数据，并缓存
                data = CacheData(key);
            }
            return data;
        }
        /// 缓存
        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Dictionary<string, DataRow> CacheData(DataCacheKey key)
        {
            Dictionary<string, DataRow> data = new Dictionary<string, DataRow>();

            //当前晚上0点过期
            DateTime date = DateTime.Today.AddDays(1);
            string keycol = "";
            DataTable dt = GetDataFunc(key, out keycol);
            if (dt == null)
                return data;
            foreach (DataRow dr in dt.Rows)
            {
                data[dr[keycol].ToString()] = dr;
            }
            //缓存字典
            DataCacheHelper.SetCache(key.ToString(), data, date, System.Web.Caching.Cache.NoSlidingExpiration);
            //缓存表
            DataCacheHelper.SetCache(key.ToString() + "_Table", dt, date, System.Web.Caching.Cache.NoSlidingExpiration);
            return data;
        }
        /// 查询数据库，获取数据
        /// <summary>
        /// 查询数据库，获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keycol"></param>
        /// <returns></returns>
        private DataTable GetDataFunc(DataCacheKey key, out string keycol)
        {
            switch (key)
            {
                case DataCacheKey.Car_Car:
                    return Dal.DictionaryDataCache.Instance.GetCar_Car(out keycol);
                case DataCacheKey.Car_Serial:
                    return Dal.DictionaryDataCache.Instance.GetCar_Serial(out keycol);
                case DataCacheKey.Car_Brand:
                    return Dal.DictionaryDataCache.Instance.GetCar_Brand(out keycol);
                case DataCacheKey.Car_MasterBrand:
                    return Dal.DictionaryDataCache.Instance.GetCar_MasterBrand(out keycol);
                case DataCacheKey.AreaInfo_Province:
                    return Dal.DictionaryDataCache.Instance.GetAreaInfo_Province(out keycol);
                case DataCacheKey.AreaInfo_City:
                    return Dal.DictionaryDataCache.Instance.GetAreaInfo_City(out keycol);
                case DataCacheKey.AreaInfo_County:
                    return Dal.DictionaryDataCache.Instance.GetAreaInfo_County(out keycol);
                case DataCacheKey.DMSMember:
                    return Dal.DictionaryDataCache.Instance.GetDMSMember(out keycol);
                case DataCacheKey.AcitvityInfo:
                    return Dal.DictionaryDataCache.Instance.GetAcitvityInfo(out keycol);
                default:
                    keycol = "";
                    return null;
            }
        }
        #endregion
    }

    public enum DataCacheKey
    {
        Car_Car = 0,
        Car_Serial,
        Car_Brand,
        Car_MasterBrand,
        AreaInfo_Province,
        AreaInfo_City,
        AreaInfo_County,
        DMSMember,
        AcitvityInfo
    }
}
