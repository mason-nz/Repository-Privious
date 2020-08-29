using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.CarsDataSynchronization
{
    public class CarInfoBll
    {
        public static readonly CarInfoBll Instance = new CarInfoBll();
        /// <summary>
        ///  2017-03-03张立彬
        /// 批量添加汽车品牌信息
        /// </summary>
        /// <param name="feedbackData"></param>
        /// <returns></returns>
        public string InserCarBrandInfo(List<CarBrandModel> lsitCarBrand)
        {
            if (lsitCarBrand != null && lsitCarBrand.Count > 0)
            {
                CarInfoDal.Instance.ClearCarBrandInfo();
                long sqlBulkCopyInsertRunTime = CarInfoDal.Instance.InsertCarBrandInfo(lsitCarBrand);
                return string.Format("插入{0}条汽车品牌信息用时：{1}毫秒", lsitCarBrand.Count, sqlBulkCopyInsertRunTime);
            }
            return "获取汽车品牌数量小于等于零或对象为null";
        }
        /// <summary>
        ///  2017-03-03张立彬
        /// 批量添加汽车车系信息
        /// </summary>
        /// <param name="feedbackData"></param>
        /// <returns></returns>
        public string InserCarSerailInfo(List<CardSerialModel> lsitCarBrand,string CarBrandID)
        {
            if (lsitCarBrand != null && lsitCarBrand.Count > 0)
            {
                CarInfoDal.Instance.ClearCarSerialInfo(CarBrandID);
                long sqlBulkCopyInsertRunTime = CarInfoDal.Instance.InsertCarSerialInfo(lsitCarBrand);
                return string.Format("插入汽车品牌ID为{0}，共{1}条汽车车系信息用时：{2}毫秒", CarBrandID , lsitCarBrand.Count, sqlBulkCopyInsertRunTime);
            }
            return "获取汽车品牌ID为" + CarBrandID + "，下的汽车车系数量小于等于零或对象为null";
        }
    }
}
