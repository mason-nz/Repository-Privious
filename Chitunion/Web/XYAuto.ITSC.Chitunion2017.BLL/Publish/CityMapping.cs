using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish
{
    public class CityMapping
    {

        public static Dictionary<int, string> MunicipalityCity
        {
            get
            {
                return new Dictionary<int, string>()
                {
                    { 2,    "北京"},
                    { 24,   "上海"},
                    { 26,   "天津"},
                    { 31,   "重庆"}
                };

            }
        }

        /// <summary>
        /// 是否是直辖市
        /// </summary>
        /// <param name="provinceId">城市Id</param>
        /// <returns></returns>
        public static bool IsMunicipality(int provinceId)
        {
            var value = MunicipalityCity.FirstOrDefault(s => s.Key == provinceId);

            return !string.IsNullOrWhiteSpace(value.Value);
        }
    }
}
