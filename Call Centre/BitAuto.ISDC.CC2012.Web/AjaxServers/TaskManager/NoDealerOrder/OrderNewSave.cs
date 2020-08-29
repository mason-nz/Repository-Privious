using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public class OrderNewSave
    {
        internal static Entities.OrderNewCar Save(NewCarConsultInfo newInfo, out string msg, int userID, int TID)
        {
            msg = string.Empty;
            Entities.OrderNewCar model = BLL.OrderNewCar.Instance.GetOrderNewCar(TID);

            #region 检查格式
            if (newInfo == null)
            {
                msg += "新车联系信息为空<br/>";
            }
            if (newInfo.CarMasterID == "0")
            {
                msg += "请选择关注车型的品牌<br/>";
            }
            if (newInfo.CarSerialID == "0")
            {
                msg += "请选择关注车型的系列<br/>";
            }
            if (newInfo.CarTypeID == "0")
            {
                msg += "请选择关注车型的车型<br/>";
            }

            if (newInfo.CallRecord == "")
            {
                msg += "请填写电话记录<br/>";
            }
            if (newInfo.UserName == "")
            {
                msg += "请填写用户姓名<br/>";
            }

            if (msg != "")
            {
                return null;
            }

            #endregion

            #region 新车订单信息实体类

            int _carMasterID;
            if (int.TryParse(newInfo.CarMasterID, out _carMasterID))
            {
                model.CarMasterID = _carMasterID;
            }
            int _carSerialID;
            if (int.TryParse(newInfo.CarSerialID, out _carSerialID))
            {
                model.CarSerialID = _carSerialID;
            }
            int _carTypeID;
            if (int.TryParse(newInfo.CarTypeID, out _carTypeID))
            {
                model.CarTypeID = _carTypeID;
            }
            if (newInfo.CarColor != "请选择颜色" && newInfo.CarColor != "-1" && newInfo.CarColor != "null")
            {
                model.CarColor = newInfo.CarColor;
            }
            else
            {
                model.CarColor = "";
            }
            model.CallRecord = newInfo.CallRecord;
            model.DMSMemberCode = newInfo.DMSMemberCode;
            model.DMSMemberName = newInfo.DMSMemberName;
            model.UserName = newInfo.UserName;

            #endregion

            return model;
        }
    }
}