using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Web.Caching;
using System.Web;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类CallRecord_ORIG_Business 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-05-27 10:46:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecord_ORIG_Business
    {
        public static readonly CallRecord_ORIG_Business Instance = new CallRecord_ORIG_Business();
        private Random R = new Random();

        protected CallRecord_ORIG_Business()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG_Business model)
        {
            BLL.Loger.Log4Net.Info("[BLL]InsertCallRecord_ORIG_Business ...插入开始...CallID:" + model.CallID);
            return Dal.CallRecord_ORIG_Business.Instance.Insert(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Business model)
        {
            return Dal.CallRecord_ORIG_Business.Instance.Update(model);
        }
        /// 获取业务url
        /// <summary>
        /// 获取业务url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="Source"></param>
        /// <param name="CarType"></param>
        /// <returns></returns>
        public string GetTaskUrl(string TaskID, string BGID, string SCID)
        {
            try
            {
                if (string.IsNullOrEmpty(TaskID))
                {
                    return "";
                }
                //支持新旧工单（不查数据库）
                switch (Dal.CallRecord_ORIG_Business.Instance.GetProjectSource(TaskID))
                {
                    case ProjectSource.S2_客户核实:
                        return "/CRMStopCust/View.aspx?TaskID={0}";
                    case ProjectSource.S3_工单:
                        string newurl = "/WOrderV2/WorkOrderView.aspx?OrderID={0}"; //工单id长度17位
                        string oldurl = "/WorkOrder/WorkOrderView.aspx?OrderID={0}"; //工单id长度16位及以下
                        if (TaskID.Length == 17)
                        {
                            return newurl;
                        }
                        else return oldurl;
                    case ProjectSource.S4_其他任务:
                        return "/OtherTask/OtherTaskDealView.aspx?OtherTaskID={0}";
                    case ProjectSource.S5_易集客:
                        return "/LeadsTask/LeadTaskView.aspx?TaskID={0}";
                    case ProjectSource.S6_厂家集客:
                        return "/LeadsTask/CSLeadTaskView.aspx?TaskID={0}";
                    case ProjectSource.S7_易团购:
                        return "/YTGActivityTask/YTGActivityTaskView.aspx?TaskID={0}";
                }

                string url = "";
                DataTable businessDt = DictionaryDataCache.Instance.GetDataTableByKey(DataCacheKey.BusinessUrL);
                if (BGID != "" && SCID != "" && businessDt != null && businessDt.Rows.Count > 0)
                {
                    string where = "1=1";
                    where += " And BGID='" + CommonFunction.ObjectToInteger(BGID, -1) + "'";
                    where += " And SCID='" + CommonFunction.ObjectToInteger(SCID, -1) + "'";
                    DataRow[] rowList = businessDt.Select(where);
                    if (rowList.Length > 0)
                    {
                        url = rowList[0]["BusinessDetailURL"].ToString();
                    }
                    else
                    {
                        url = "";
                    }
                }
                else
                {
                    url = "";
                }
                return url;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("获取url时出现异常：BLL.CallRecord_ORIG_Business.GetTaskUrl()........" + ex.Message.ToString(), ex);
                return "";
            }
        }

        /// 增加一个URL
        /// <summary>
        /// 增加一个URL
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="webBaseUrl"></param>
        /// <returns></returns>
        public int AddBusinessUrl(int BGID, int SCID, string webBaseUrl)
        {
            return Dal.CallRecord_ORIG_Business.Instance.AddBusinessUrl(BGID, SCID, webBaseUrl);
        }
        /// 删除业务url
        /// <summary>
        /// 删除业务url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public int DeleteBusinessUrl(int BGID, int SCID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.DeleteBusinessUrl(BGID, SCID);
        }
        /// 根据业务组，分类取url
        /// <summary>
        /// 根据业务组，分类取url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public DataTable GetBusinessUrl(int BGID, int SCID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetBusinessUrl(BGID, SCID);
        }
        /// 返回业务查看页面，由于业务里包括易湃的惠买车
        /// <summary>
        /// 返回业务查看页面，由于业务里包括易湃的惠买车
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <param name="GetViewUrl"></param>
        /// <param name="CreateUserID"></param>
        /// <returns></returns>
        public string GetViewUrl(string BusinessID, string GetViewUrl)
        {
            if (string.IsNullOrEmpty(BusinessID))
            {
                return "";
            }
            GetViewUrl += "&r={1}";
            GetViewUrl = string.Format(GetViewUrl, BusinessID, R.Next(100000));
            return GetViewUrl;
        }
        /// 根据CallID，更新业务数据，businessID为业务ID，BGID为分组ID，SCID为分类ID
        /// <summary>
        /// 根据CallID，更新业务数据，businessID为业务ID，BGID为分组ID，SCID为分类ID
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <param name="callID"></param>
        /// <param name="businessID"></param>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="createuserid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int UpdateBusinessDataByCallID(Int64 callID, string businessID, int BGID, int SCID, int createuserid, ref string msg)
        {
            msg = "";
            try
            {
                //插入更新数据，操作现表
                if (BLL.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callID))
                {
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]在系统中已经存在业务数据，更新！callID=" + callID);
                    Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Update(model);
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]更新完毕：callID=" + callID);
                    return recID;
                }
                else
                {
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]准备插入数据：callID=" + callID);
                    Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Insert(model);
                    model.RecID = recID;
                    BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]插入完毕：callID=" + callID);
                    return recID;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BLL.Loger.Log4Net.Info("[BLL.UpdateBusinessDataByCallID]出错！callID=" + callID + ",errorMsg:" + msg);
                return -1;
            }
        }
        /// 是否存在该记录-现在表
        /// <summary>
        /// 是否存在该记录-现在表
        /// </summary>
        public bool IsExistsByCallID(Int64 callid)
        {
            return Dal.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callid);
        }
        /// 任务类型解析
        /// <summary>
        /// 任务类型解析
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        public ProjectSource GetProjectSource(string businessID)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetProjectSource(businessID);
        }

        #region 分月查询
        /// 查询话务业务数据
        /// <summary>
        /// 查询话务业务数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallRecord_ORIG_Business(QueryCallRecord_ORIG_Business query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetCallRecord_ORIG_Business(query, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// 查询话务业务实体类
        /// <summary>
        /// 查询话务业务实体类
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG_Business GetByCallID(Int64 CallID, string tableEndName)
        {
            return Dal.CallRecord_ORIG_Business.Instance.GetByCallID(CallID, tableEndName);
        }
        #endregion
    }
}

