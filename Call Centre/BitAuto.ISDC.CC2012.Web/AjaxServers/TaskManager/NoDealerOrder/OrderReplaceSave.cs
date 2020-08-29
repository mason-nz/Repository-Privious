using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public class OrderReplaceSave
    {
        public static Entities.OrderRelpaceCar Save(ReplaceCarConsultInfo replaceInfo, out string msg, int userID, long tid)
        {
            msg = "";

            #region 检查

            if (replaceInfo == null)
            {
                msg += "置换车联系信息为空<br/>";
            }


            if (replaceInfo.CarMasterID == "0")
            {
                msg += "请选择意向车型的品牌<br/>";
            }
            if (replaceInfo.CarSerialID == "0")
            {
                msg += "请选择意向车型的系列<br/>";
            }
            if (replaceInfo.CarTypeID == "0")
            {
                msg += "请选择意向车型的车型<br/>";
            }
            //if (replaceInfo.CarColor == "")
            //{
            //    msg += "请选择意向车的颜色<br/>";
            //}
            if (replaceInfo.RepCarMasterID == "")
            {
                msg += "请选择现有车型的品牌<br/>";
            }
            if (replaceInfo.RepCarSerialID == "")
            {
                msg += "请选择现有车型的系列<br/>";
            }
            if (replaceInfo.RepCarTypeId == "")
            {
                msg += "请选择现有车型的车型<br/>";
            }
            //if (replaceInfo.ReplacementcCarColor == "")
            //{
            //    msg += "请选择现有车的颜色<br/>";
            //}
            if (replaceInfo.RepCarProvinceID == "-1")
            {
                msg += "请选择车牌所在地省份<br/>";
            }
            if (replaceInfo.RepCarCityID == "-1")
            {
                msg += "请选择车牌所在地城市<br/>";
            }
            //if (replaceInfo.RepCarCountyID == "-1")
            //{
            //    msg += "请选择车牌所在地区县<br/>";
            //}
            if (replaceInfo.ReplacementCarUsedMiles == "")
            {
                msg += "请填写已行驶里程<br/>";
            }
            decimal decVal = 0;
            if (!decimal.TryParse(replaceInfo.ReplacementCarUsedMiles, out decVal))
            {
                msg += "已行驶里程应该为数字<br/>";
            }
            if (replaceInfo.SellPrice == "")
            {
                msg += "请填写预售价格<br/>";
            }

            if (!decimal.TryParse(replaceInfo.SellPrice, out decVal))
            {
                msg += "预售价格应该为数字<br/>";
            }
            if (replaceInfo.CallRecord == "")
            {
                msg += "请填写电话记录<br/>";
            }

            if (replaceInfo.UserName == "")
            {
                msg += "请填写用户姓名<br/>";
            }

            decimal zdPrice = BLL.CarTypeAPI.Instance.GetCarReferPriceByCarTypeID(int.Parse(replaceInfo.RepCarTypeId));
            
            //                            (注意)这里 CarPrice 是预售价格
            if (decimal.Parse(replaceInfo.SellPrice) > zdPrice * (decimal)1.5 || decimal.Parse(replaceInfo.SellPrice) < 0)
            {
                msg += "预售价格不能小于等于0，并且不能大于厂商指导价的1.5倍（" + (zdPrice * (decimal)1.5).ToString() + "）【厂商指导价：" + (zdPrice).ToString() + "】<br/>";
            }

            if (decimal.Parse(replaceInfo.ReplacementCarUsedMiles) > 100 || decimal.Parse(replaceInfo.ReplacementCarUsedMiles) <= 0)
            {
                msg += "已行驶里程不能小于等于0，并且不能大于100<br/>";
            }


            if (msg != "")
            {
                return null;
            }

            #endregion

            #region 获取置换订单实体类

            Entities.OrderRelpaceCar repCarModel = BLL.OrderRelpaceCar.Instance.GetOrderRelpaceCar(tid);
            if (repCarModel != null)
            {
                #region 修改值

                repCarModel.CarMasterID = int.Parse(replaceInfo.CarMasterID);
                repCarModel.CarSerialID = int.Parse(replaceInfo.CarSerialID);
                repCarModel.CarTypeID = int.Parse(replaceInfo.CarTypeID);
                repCarModel.CarColor = replaceInfo.CarColor;
                repCarModel.DMSMemberCode = replaceInfo.DMSMemberCode;
                repCarModel.DMSMemberName = replaceInfo.DMSMemberName;
                repCarModel.RepCarMasterID = int.Parse(replaceInfo.RepCarMasterID);
                repCarModel.RepCarSerialID = int.Parse(replaceInfo.RepCarSerialID);
                repCarModel.RepCarTypeId = int.Parse(replaceInfo.RepCarTypeId);
                repCarModel.ReplacementCarColor = replaceInfo.ReplacementcCarColor;
                repCarModel.ReplacementCarBuyMonth = int.Parse(replaceInfo.ReplacementCarBuyMonth);
                repCarModel.ReplacementCarBuyYear = int.Parse(replaceInfo.ReplacementCarBuyYear);
                repCarModel.RepCarProvinceID = int.Parse(replaceInfo.RepCarProvinceID);
                repCarModel.RepCarCityID = int.Parse(replaceInfo.RepCarCityID);
                repCarModel.RepCarCountyID = int.Parse(replaceInfo.RepCarCountyID);
                repCarModel.ReplacementCarUsedMiles = decimal.Parse(replaceInfo.ReplacementCarUsedMiles);
                repCarModel.SalePrice = decimal.Parse(replaceInfo.SellPrice);
               // repCarModel.CarPrice = decimal.Parse(replaceInfo.CarPrice);
                repCarModel.CallRecord = replaceInfo.CallRecord;
                repCarModel.UserName = replaceInfo.UserName;

                #endregion
            }
            else
            {
                msg += "没有找到ID为" + tid.ToString() + "的联系记录信息";
            }

            #endregion

            return repCarModel;

        }
    }
}