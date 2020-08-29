using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CarBrandHelper
    {
        public static readonly CarBrandHelper Instance = new CarBrandHelper();

        protected CarBrandHelper()
        {
        }

        /// <summary>
        /// 得到主品牌信息
        /// </summary>
        /// <param name="brandNameList">主品牌名称列表，用逗号分隔</param>
        /// <returns>ids, names. 用逗号分隔</returns>
        public string[] GetCarBrandByNames(string brandNameList)
        {
            return Dal.CarBrand.Instance.GetCarBrandByNames(brandNameList);
        }

        /// <summary>
        /// 得到主品牌信息
        /// </summary>
        /// <param name="brandNameList">主品牌名称列表，用逗号分隔</param>
        /// <returns>id数组</returns>
        public List<int> GetCarBrandIDsByNames(string brandNameList)
        {
            return Dal.CarBrand.Instance.GetCarBrandIDsByNames(brandNameList);
        }
    }
}
