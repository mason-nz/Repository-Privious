using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class HolidaysInfo
    {
        public static readonly HolidaysInfo Instance = new HolidaysInfo();
        private int CacheDataTime = (WebConfigurationManager.AppSettings["CacheDataTime"] != null ? int.Parse(WebConfigurationManager.AppSettings["CacheDataTime"].ToString()) : 1);//Cache失效时间，默认为1天（单位：天）

        /// <summary>
        /// 节假日数据，查询后，插入到缓存中，默认缓存1天
        /// </summary>
        /// <returns></returns>
        public DataTable GetHolidaysInfo()
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache["HolidaysInfo"] != null)
            {
                return (DataTable)objCache["HolidaysInfo"];
            }
            else
            {
                DataTable dt = Dal.HolidaysInfo.Instance.GetHolidaysInfo();
                if (dt != null)
                {
                    objCache.Insert("HolidaysInfo", dt, null, DateTime.Now.AddDays(CacheDataTime), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                return dt;
            }
            //return Dal.HolidaysInfo.Instance.GetHolidaysInfo();
        }
    }
}
