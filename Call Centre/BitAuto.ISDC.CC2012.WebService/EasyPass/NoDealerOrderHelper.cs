using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class NoDealerOrderHelper
    {
        NoDealerOrderProxy proxy = new NoDealerOrderProxy();
        private string AuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["AuthorizeCode"].ToString();//验证码
        //private string WithoutDealerOrderServiceURL = System.Configuration.ConfigurationManager.AppSettings["WithoutDealerOrderServiceURL"].ToString();//服务URL

        /// <summary>
        /// 取得某车款在各城市的经销商数量xml,返回utf-8编码的内容 
        /// </summary>
        /// <param name="carid">车款ID</param>
        /// <returns></returns>
        public string GetCarDealerXML(int carid)
        {
            return proxy.GetCarDealerXML(AuthorizeCode, carid);
            //return WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "GetCarDealerXML", new object[] { AuthorizeCode, carid }).ToString();
        }

        /// <summary>
        /// 取得某地区某个车款的经销商,返回utf-8编码的内容
        /// </summary>
        /// <param name="carid">车款ID</param>
        /// <param name="locationId">地区ID</param>
        /// <param name="businessLevel">经销商店的级别（4s店1,综合0）</param>
        /// <returns></returns>
        public DataSet GetDealerListByLocationId(int carid, int locationId, int businessLevel)
        {
            return proxy.GetDealerListByLocationId(AuthorizeCode, carid, locationId, businessLevel);
            //return (DataSet)WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "GetDealerListByLocationId", new object[] { AuthorizeCode, carid, locationId, businessLevel });
        }

        /// <summary>
        /// 取得无主的新车订单
        /// </summary>
        /// <param name="lastMaxId">上次已获取过的订单最大id</param>
        /// <returns></returns>
        public DataTable GetNewCarOrderList(int lastMaxId)
        {
            DataSet ds = proxy.GetNewCarOrderList(AuthorizeCode, lastMaxId);
            //DataSet ds = (DataSet)WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "GetNewCarOrderList", new object[] { AuthorizeCode, lastMaxId });
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 取得无主的置换车订单
        /// </summary>
        /// <param name="lastMaxId">上次已获取过的订单最大id</param>
        /// <returns></returns>
        public DataTable GetReplaceOrderList(int lastMaxId)
        {
            DataSet ds = proxy.GetReplaceOrderList(AuthorizeCode, lastMaxId);
            //DataSet ds = (DataSet)WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "GetReplaceOrderList", new object[] { AuthorizeCode, lastMaxId });
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }


        /// <summary>
        /// 取得试驾订单数据
        /// </summary>
        /// <param name="lastMaxId">上次已获取过的订单最大id</param>
        /// <returns></returns>
        public DataTable GetTestDriveOrderList(int lastMaxId)
        {
            DataSet ds = proxy.GetShiJiaOrderList(AuthorizeCode, lastMaxId);
            //DataSet ds = (DataSet)WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "GetShiJiaOrderList", new object[] { AuthorizeCode, lastMaxId });
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 更新新车订单内容，且把订单提交到订单库 
        /// </summary>
        /// <param name="model">新车订单（核实状态）实体</param>
        /// <param name="processFlag">处理状态（1处理成功2处理失败）</param>
        /// <param name="processMemo">处理备注</param>
        /// <param name="errorMsg">如出错，则返回出错信息</param>
        /// <returns>1成功处理订单，-1需要重新处理</returns>
        public int SetNewCarOrder(Entities.OrderNewCar model, short processFlag, string processMemo, ref string errorMsg)
        {
            int status = -1;
            if (model == null ||
                (model != null && (model.YPOrderID == null || model.YPOrderID.Value <= 0 || model.TaskID <= 0)))
            {
                errorMsg = "新车订单实体为空，或易湃订单ID无效，或任务ID无效";
            }
            else
            {
                Entities.OrderTask otModel = BLL.OrderTask.Instance.GetOrderTask(model.TaskID);
                if (otModel == null)
                {
                    errorMsg = "根据新车订单实体，获取任务无效";
                }
                else
                {
                    Entities.OrderNewCarLog onModel = BLL.OrderNewCarLog.Instance.GetOrderNewCarLog(otModel.RelationID.Value);
                    if (onModel == null)
                    {
                        errorMsg = "根据新车订单日志ID，获取新车订单日志实体无效";
                    }
                    else
                    {
                        int dealerID = 0;
                        int.TryParse(model.DMSMemberCode, out dealerID);

                        #region 如果没有三级ID，传二级ID

                        int locationID = (int)model.CountyID;
                        string locationName = ""; //地区名
                        if (model.CountyID == -1)
                        {
                            locationID = (int)model.CityID;
                            locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CityID.Value.ToString());
                        }
                        else
                        {
                            locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CountyID.Value.ToString());
                        }

                        #endregion

                        //object[] array = new object[] { AuthorizeCode, model.YPOrderID.Value,
                        //                                       dealerID,
                        //                                       onModel.OrderQuantity.Value,
                        //                                       model.OrderRemark,
                        //                                       model.UserGender.Value == 2 ? 0 : model.UserGender.Value,
                        //                                       model.UserName,
                        //                                       model.UserPhone,
                        //                                       model.UserMobile,
                        //                                       model.UserMail,
                        //                                       locationID,
                        //                                       locationName,
                        //                                       model.UserAddress,
                        //                                       model.CarTypeID.Value,
                        //                                       onModel.CarPrice.Value,
                        //                                       model.CarColor,
                        //                                       BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(model.CarTypeID.Value),
                        //                                       onModel.CarPromotions,
                        //                                       processFlag,
                        //                                       processMemo,
                        //                                       errorMsg };
                        //status = (int)WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "SetNewCarOrder", array);
                        //errorMsg = array[array.Length - 1].ToString();
                        status = proxy.SetNewCarOrder(AuthorizeCode, model.YPOrderID.Value,
                                                               dealerID,
                                                               onModel.OrderQuantity.Value,
                                                               model.OrderRemark,
                                                               model.UserGender.Value == 2 ? 0 : model.UserGender.Value,
                                                               model.UserName,
                                                               model.UserPhone,
                                                               model.UserMobile,
                                                               model.UserMail,
                                                               locationID,
                                                               locationName,
                                                               model.UserAddress,
                                                               model.CarTypeID.Value,
                                                               onModel.CarPrice.Value,
                                                               model.CarColor,
                                                               BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(model.CarTypeID.Value),
                                                               onModel.CarPromotions,
                                                               processFlag,
                                                               processMemo,
                                                               0,//DasAccountID
                                                               out errorMsg);
                    }
                }
            }
            return status;
        }

        /// <summary>
        /// 更新置换订单内容，且把订单提交到订单库
        /// </summary>
        /// <param name="model">置换订单（核实状态）实体</param>
        /// <param name="processFlag">处理状态（1处理成功2处理失败）</param>
        /// <param name="processMemo">处理备注</param>
        /// <param name="ErrorMsg">如出错，则返回出错信息</param>
        /// <returns>1成功处理订单，-1需要重新处理</returns>
        public int SetReplacementOrder(Entities.OrderRelpaceCar model, short processFlag, string processMemo, ref string errorMsg)
        {
            int status = -1;
            if (model == null ||
                (model != null && (model.YPOrderID == null || model.YPOrderID.Value <= 0 || model.TaskID <= 0)))
            {
                errorMsg = "置换订单实体为空，或易湃订单ID无效，或任务ID无效";
            }
            else
            {
                Entities.OrderTask otModel = BLL.OrderTask.Instance.GetOrderTask(model.TaskID);
                if (otModel == null)
                {
                    errorMsg = "根据置换订单实体，获取任务无效";
                }
                else
                {
                    Entities.OrderRelpaceCarLog orModel = BLL.OrderRelpaceCarLog.Instance.GetOrderRelpaceCarLog(otModel.RelationID.Value);
                    if (orModel == null)
                    {
                        errorMsg = "根据置换订单日志ID，获取置换订单日志实体无效";
                    }
                    else
                    {
                        int dealerID = 0;
                        int.TryParse(model.DMSMemberCode, out dealerID);

                        #region 如果没有三级ID，传二级ID

                        int locationID = (int)model.CountyID;
                        string locationName = ""; //地区名
                        if (model.CountyID == -1)
                        {
                            locationID = (int)model.CityID;
                            locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CityID.Value.ToString());
                        }
                        else
                        {
                            locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CountyID.Value.ToString());
                        }

                        int RepCarCountyID = (int)model.RepCarCountyID;
                        if (model.RepCarCountyID == -1)
                        {
                            RepCarCountyID = (int)model.RepCarCityID;
                        }

                        #endregion

                        //object[] array = new object[] { AuthorizeCode,model.YPOrderID.Value,
                        //                                       dealerID,
                        //                                       orModel.OrderPrice.Value,
                        //                                       orModel.OrderQuantity.Value,
                        //                                       model.OrderRemark,
                        //                                       model.UserGender.Value == 2 ? 0 : model.UserGender.Value,
                        //                                       model.UserName,
                        //                                       model.UserPhone,
                        //                                       model.UserMobile,
                        //                                       model.UserMail,
                        //                                       locationID,
                        //                                       locationName,
                        //                                       model.UserAddress,
                        //                                       model.CarTypeID.Value,
                        //                                       orModel.CarPrice.Value,
                        //                                       model.CarColor,
                        //                                       BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(model.CarTypeID.Value),
                        //                                       orModel.CarPromotions,
                        //                                       model.RepCarTypeId.Value,
                        //                                       (short)model.ReplacementCarBuyYear.Value,
                        //                                       (short)model.ReplacementCarBuyMonth.Value,
                        //                                       model.ReplacementCarColor,
                        //                                       (int)model.ReplacementCarUsedMiles,
                        //                                       RepCarCountyID,
                        //                                       (decimal)model.SalePrice,
                        //                                       processFlag,
                        //                                       processMemo,
                        //                                       errorMsg };
                        //status = (int)WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "SetReplacementOrder", array);
                        //errorMsg = array[array.Length - 1].ToString();
                        status = proxy.SetReplacementOrder(AuthorizeCode, model.YPOrderID.Value,
                                                               dealerID,
                                                               orModel.OrderPrice.Value,
                                                               orModel.OrderQuantity.Value,
                                                               model.OrderRemark,
                                                               model.UserGender.Value == 2 ? 0 : model.UserGender.Value,
                                                               model.UserName,
                                                               model.UserPhone,
                                                               model.UserMobile,
                                                               model.UserMail,
                                                               locationID,
                                                               locationName,
                                                               model.UserAddress,
                                                               model.CarTypeID.Value,
                                                               orModel.CarPrice.Value,
                                                               model.CarColor,
                                                               BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(model.CarTypeID.Value),
                                                               orModel.CarPromotions,
                                                               model.RepCarTypeId.Value,
                                                               (short)model.ReplacementCarBuyYear.Value,
                                                               (short)model.ReplacementCarBuyMonth.Value,
                                                               model.ReplacementCarColor,
                                                               (int)model.ReplacementCarUsedMiles,
                                                               RepCarCountyID,
                                                               (decimal)model.SalePrice,
                                                               processFlag,
                                                               processMemo,
                                                               0,//DasAccountID
                                                               out errorMsg);
                        
                    }
                }
            }
            return status;
        }

        public int SetTestDriveOrder(Entities.OrderNewCar model, short processFlag, string processMemo, ref string errorMsg)
        {
            int status = -1;

            if (model == null)
            {
                errorMsg = "订单实体为空";
                return status;
            }
            if (model.OrderType != 1)
            {
                errorMsg = "不是试驾订单";
                return status;
            }
            if (model.YPOrderID == null || model.YPOrderID.Value <= 0 || model.TaskID <= 0)
            {
                errorMsg = "易湃订单ID无效，或任务ID无效";
            }
            else
            {
                Entities.OrderTask otModel = BLL.OrderTask.Instance.GetOrderTask(model.TaskID);
                if (otModel == null)
                {
                    errorMsg = "根据试驾订单实体，获取任务无效";
                }
                else
                {
                    Entities.OrderNewCarLog onModel = BLL.OrderNewCarLog.Instance.GetOrderNewCarLog(otModel.RelationID.Value);
                    if (onModel == null)
                    {
                        errorMsg = "根据试驾订单日志ID，获取试驾订单日志实体无效";
                    }
                    else
                    {
                        int dealerID = 0;
                        int.TryParse(model.DMSMemberCode, out dealerID);

                        #region 如果没有三级ID，传二级ID

                        int locationID = (int)model.CountyID;
                        string locationName = ""; //地区名
                        if (model.CountyID == -1)
                        {
                            locationID = (int)model.CityID;
                            locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CityID.Value.ToString());
                        }
                        else
                        {
                            locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(model.CountyID.Value.ToString());
                        }

                        int RepCarCountyID = (int)model.CountyID;
                        if (model.CountyID == -1)
                        {
                            RepCarCountyID = (int)model.CityID;
                        }

                        #endregion

                        //object[] array = new object[] { 
                        //                                       AuthorizeCode,
                        //                                       model.YPOrderID.Value,
                        //                                       model.OrderRemark,
                        //                                       model.UserGender.Value == 2 ? 0 : model.UserGender.Value,
                        //                                       model.UserName,
                        //                                       model.UserPhone,
                        //                                       model.UserMobile,
                        //                                       model.UserMail,
                        //                                       locationID,
                        //                                       locationName,
                        //                                       model.UserAddress,
                        //                                       model.OrderCreateTime,
                        //                                       model.CarTypeID.Value,
                        //                                       BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(model.CarTypeID.Value),
                        //                                       dealerID,
                        //                                       processFlag,
                        //                                       processMemo,
                        //                                       errorMsg };
                        //status = (int)WebServiceHelper.InvokeWebService(WithoutDealerOrderServiceURL, "SetShiJiaOrder", array);
                        //errorMsg = array[array.Length - 1].ToString();
                        status = proxy.SetShiJiaOrder(AuthorizeCode,
                                                               model.YPOrderID.Value,
                                                               model.OrderRemark,
                                                               model.UserGender.Value == 2 ? 0 : model.UserGender.Value,
                                                               model.UserName,
                                                               model.UserPhone,
                                                               model.UserMobile,
                                                               model.UserMail,
                                                               locationID,
                                                               locationName,
                                                               model.UserAddress,
                                                               model.OrderCreateTime.Value,
                                                               model.CarTypeID.Value,
                                                               BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(model.CarTypeID.Value),
                                                               dealerID,
                                                               processFlag,
                                                               processMemo,
                                                               0,//DasAccountID
                                                               out errorMsg);
                    }
                }
            }
            return status;
        }
    }


    class NoDealerOrderProxy : EasyPass.WithoutDealerOrderService.WithoutDealerOrderService
    {
        public NoDealerOrderProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["WithoutDealerOrderServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["WithoutDealerOrderServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
