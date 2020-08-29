using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.BLL.WorkOrder;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类WorkOrderRevert 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderRevert
    {
        #region Instance
        public static readonly WorkOrderRevert Instance = new WorkOrderRevert();
        #endregion

        #region Contructor
        protected WorkOrderRevert()
        { }
        #endregion


        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderRevert GetWorkOrderRevert(long WORID)
        {
            Entities.WorkOrderReceiver receiver = BLL.WorkOrderReceiver.Instance.GetWorkOrderReceiver((int)WORID);
            Entities.WorkOrderLog log = BLL.WorkOrderLog.Instance.GetWorkOrderLogByReceiverRecID((int)WORID);
            Entities.WorkOrderRevert model = new Entities.WorkOrderRevert();
            if (receiver != null && log != null)
            {
                model = GetWorkOrderRevertByLogDesc(log.LogDesc);
                model.CallID = receiver.CallID;
                model.CreateTime = receiver.CreateTime;
                model.CreateUserID = receiver.CreateUserID;
                model.OrderID = receiver.OrderID;
                model.ReceiverDepartName = receiver.ReceiverDepartName;
                model.ReceiverID = receiver.ReceiverUserID.ToString();
                model.WORID = receiver.RecID;
                model.AudioURL = receiver.AudioURL;
                return model;
            }
            else
            {
                return null;
            }
        }

        public Entities.WorkOrderRevert GetWorkOrderRevertByCallID(Int64 CallID)
        {
            Entities.WorkOrderRevert model = new Entities.WorkOrderRevert();
            Entities.QueryWorkOrderReceiver query = new QueryWorkOrderReceiver();
            query.CallID = (long)CallID;
            int totalCount = 0;
            DataTable dt = BLL.WorkOrderReceiver.Instance.GetWorkOrderReceiver(query, "", 1, 9999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                model = GetWorkOrderRevert(long.Parse(dt.Rows[0]["RecID"].ToString()));
            }
            else
            {
                model = null;
            }
            return model;

        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Insert(Entities.WorkOrderRevert model)
        {
            int intVal = 0;
            Entities.WorkOrderReceiver receiver = new Entities.WorkOrderReceiver();
            Entities.WorkOrderLog log = new Entities.WorkOrderLog();

            receiver.OrderID = model.OrderID;
            receiver.CallID = model.CallID;
            receiver.CreateTime = DateTime.Now;
            receiver.CreateUserID = model.CreateUserID;
            receiver.ReceiverDepartName = model.ReceiverDepartName;
            receiver.RevertContent = model.RevertContent;
            if (int.TryParse(model.ReceiverID, out intVal))
            {
                receiver.ReceiverUserID = intVal;
            }

            log.CreateTime = model.CreateTime;
            log.CreateUserID = model.CreateUserID;
            log.OrderID = model.OrderID;
            log.LogDesc = GetLogDescByModel(model);

            int recid = BLL.WorkOrderReceiver.Instance.Insert(receiver);
            log.ReceiverRecID = recid;

            BLL.WorkOrderLog.Instance.Insert(log);
            return recid;
        }

        private static string GetLogDescByModel(Entities.WorkOrderRevert model)
        {
            Dictionary<string, string> logDescDic = new Dictionary<string, string>();
            logDescDic.Add("ProvinceName", model.ProvinceName);
            logDescDic.Add("CityName", model.CityName);
            logDescDic.Add("AttentionCarBrandName", model.AttentionCarBrandName);
            logDescDic.Add("AttentionCarSerialName", model.AttentionCarSerialName);
            logDescDic.Add("AttentionCarTypeName", model.AttentionCarTypeName);
            logDescDic.Add("CategoryName", model.CategoryName);
            logDescDic.Add("Contact", model.Contact);
            logDescDic.Add("ContactTel", model.ContactTel);
            logDescDic.Add("CountyName", model.CountyName);
            logDescDic.Add("CRMCustID", model.CRMCustID);
            logDescDic.Add("CustName", model.CustName);
            logDescDic.Add("PriorityLevelName", model.PriorityLevelName);
            logDescDic.Add("ReceiverName", model.ReceiverName);
            logDescDic.Add("SaleCarBrandName", model.SaleCarBrandName);
            logDescDic.Add("NominateActivity", model.NominateActivity);
            logDescDic.Add("SaleCarSerialName", model.SaleCarSerialName);
            logDescDic.Add("SaleCarTypeName", model.SaleCarTypeName);
            logDescDic.Add("SelectDealerID", model.SelectDealerID);
            logDescDic.Add("SelectDealerName", model.SelectDealerName);
            logDescDic.Add("TagName", model.TagName);
            logDescDic.Add("Title", model.Title);
            logDescDic.Add("WorkOrderStatus", model.WorkOrderStatus);
            logDescDic.Add("LastProcessDate", model.LastProcessDate);
            logDescDic.Add("DataSource", model.DataSource);



            string logDesc = BLL.WorkOrderLog.Instance.GetLogDesc(logDescDic);

            return logDesc;
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderRevert model)
        {
            int ReturnID = 0;
            int intVal = 0;
            Entities.WorkOrderReceiver receiver = new Entities.WorkOrderReceiver();
            Entities.WorkOrderLog log = new Entities.WorkOrderLog();

            receiver = BLL.WorkOrderReceiver.Instance.GetWorkOrderReceiver((int)model.WORID);
            log = BLL.WorkOrderLog.Instance.GetWorkOrderLogByReceiverRecID((int)model.WORID);
            if (receiver != null && log != null)
            {
                receiver.CallID = model.CallID;
                receiver.ReceiverDepartName = model.ReceiverDepartName;
                receiver.RevertContent = model.RevertContent;
                if (int.TryParse(model.ReceiverID, out intVal))
                {
                    receiver.ReceiverUserID = intVal;
                }

                log.LogDesc = GetLogDescByModel(model);

                ReturnID = BLL.WorkOrderReceiver.Instance.Update(receiver);
                BLL.WorkOrderLog.Instance.Update(log);
            }

            return ReturnID;
        }

        #endregion

        public DataTable GetWorkOrderRevertByOrderID(string OrderID, string OrderByStr)
        {
            DataTable dt = new DataTable();
            int totalCount = 0;
            Entities.QueryWorkOrderReceiver query = new QueryWorkOrderReceiver();
            query.OrderID = OrderID;
            dt = Dal.WorkOrderReceiver.Instance.GetWorkOrderReceiver(query, OrderByStr, 1, 99999, out totalCount);

            #region 处理LogDesc字段

            if (dt != null)
            {
                #region 反射，添加字段

                PropertyInfo[] myfList = typeof(LogDesc).GetProperties();
                for (int i = 0; i < myfList.Length; i++)
                {
                    dt.Columns.Add(myfList[i].Name);
                }

                #endregion

                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        LogDesc checkInfo = (LogDesc)Newtonsoft.Json.JsonConvert.DeserializeObject(dr["LogDesc"].ToString(), typeof(LogDesc));
                        if (checkInfo != null)
                        {
                            for (int i = 0; i < myfList.Length; i++)
                            {
                                dr[myfList[i].Name] = myfList[i].GetValue(checkInfo, null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            #endregion

            return dt;
        }

        private Entities.WorkOrderRevert GetWorkOrderRevertByLogDesc(string LogDesc)
        {
            Entities.WorkOrderRevert model = new Entities.WorkOrderRevert();

            PropertyInfo[] myfList = typeof(LogDesc).GetProperties();
            LogDesc checkInfo = (LogDesc)Newtonsoft.Json.JsonConvert.DeserializeObject(LogDesc, typeof(LogDesc));
            if (checkInfo != null)
            {
                for (int i = 0; i < myfList.Length; i++)
                {
                    PropertyInfo p = typeof(Entities.WorkOrderRevert).GetProperty(myfList[i].Name);
                    if (p != null)
                    {
                        object val = myfList[i].GetValue(checkInfo, null);
                        p.SetValue(model, val, null);
                    }
                }
            }
            return model;
        }
    }
}

