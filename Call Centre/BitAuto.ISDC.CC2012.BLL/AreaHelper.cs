using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class AreaHelper
    {
        public static readonly AreaHelper Instance = new AreaHelper();

        /// 根据名称查询ID
        /// <summary>
        /// 根据名称查询ID
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public string GetAreaIDByName(string areaName)
        {
            List<AreaInfo> list = DictionaryDataCache.Instance.AllAreaInfoList;
            AreaInfo info = list.FirstOrDefault(x => x.AreaName == areaName);
            if (info != null)
            {
                return info.AreaID;
            }
            else return "";
        }
        /// 模糊查询
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="areaType"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        public AreaInfo GetAreaInfoByLikeNameFromCache(string name, AreaType areaType, int PID)
        {
            List<AreaInfo> list = DictionaryDataCache.Instance.AllAreaInfoList;
            AreaInfo item = list.Find(delegate(AreaInfo o)
            {
                //模糊
                return o.AreaName.Contains(name) && o.Level == ((int)areaType).ToString() && o.PID == PID.ToString();
            });

            return item;
        }
        /// 通过ID获取名称
        /// <summary>
        /// 通过ID获取名称
        /// </summary>
        /// <param name="AreaID"></param>
        /// <returns></returns>
        public string GetAreaNameByID(string AreaID)
        {
            string areaname = "";
            List<AreaInfo> list = DictionaryDataCache.Instance.AllAreaInfoList;
            AreaInfo item = list.Find(delegate(AreaInfo o) { return o.AreaID == AreaID; });
            if (item != null)
            {
                areaname = item.AreaName;
            }
            return areaname;
        }
    }
}
