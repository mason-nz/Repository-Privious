using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// WorkOrderService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WorkOrderService : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取工单数据 (WP使用地方：1.日常工作——呼叫中心工单；)
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="msg">返回错误信息</param>
        /// <returns>返回DataTable</returns>
        [WebMethod(Description = "获取工单数据")]
        public DataTable GetWorkOrderListByWhere(string Verifycode, Entities.QueryWorkOrderInfo query, int userId, string departmentId, string order, int currentPage, int pageSize, out int totalCount, ref string msg)
        {
            totalCount = 0;
            DataTable dt = null;
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "获取工单数据，授权失败。"))
            {
                dt = BLL.WorkOrderInfo.Instance.GetWorkOrderInfoByUserID(query, userId, departmentId, order, currentPage, pageSize, out totalCount);
            }

            return dt;

        }

        /// <summary>
        /// 获取工单数据
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="demandID">查询条件</param>
        /// <param name="order">排序字段</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">返回错误信息</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取工单数据")]
        public DataTable GetWorkOrderListByDemandID(string Verifycode, string demandID, string order, int currentPage, int pageSize, out int totalCount, ref string msg)
        {
            totalCount = 0;
            DataTable dt = null;
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "获取工单数据，授权失败。"))
            {
                if (!string.IsNullOrEmpty(demandID))
                {
                    Entities.QueryWorkOrderInfo query = new Entities.QueryWorkOrderInfo();
                    query.DemandID = demandID;

                    dt = BLL.WorkOrderInfo.Instance.GetWorkOrderInfoForDemandInfo(query, order, currentPage, pageSize, out totalCount);
                }
            }

            return dt;

        }

        /// <summary>
        /// 根据条件查询工单分类
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取工单分类")]
        public DataTable GetWorkOrderCategoryByWhere(string Verifycode, string where, ref string msg)
        {
            DataTable dt = null;
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "获取工单数据，授权失败。"))
            {
                dt = BLL.Util.GetTableInfoBySql("select * from WorkOrderCategory where status=0 " + where + " order by OrderNum asc");
            }

            return dt;
        }

        /// <summary>
        /// 获取订单所有状态
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "工单的所有状态")]
        public DataTable GetAllWorkOrderStatus()
        {
            return BLL.Util.GetDataFromEnum(typeof(Entities.WorkOrderStatus));
        }

        [WebMethod(Description = "插入工单")]
        public void InsertWorkOrderInfo(Entities.WorkOrderInfo info)
        {
            BLL.WorkOrderInfo.Instance.InsertWorkOrderInfo(info);
        }


        #region
        /// <summary>
        /// 获取工单数据 (WP使用地方：1.日常工作——呼叫中心工单；2：日常工作——呼叫中心工单;3:日常工作——只能平台工单)
        /// </summary> 
        /// <param name="Verifycode">授权码</param>
        /// <param name="query">查询条件</param>
        /// <param name="userId">crm登录人id</param>
        /// <param name="departmentId">crm登录人部门id</param>
        /// <param name="order">排序字段</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="msg">错误信息</param>
        /// <returns>返回DataTable</returns>
        [WebMethod(Description = "获取工单数据")]
        public DataTable GetWOrderListByWhere(string Verifycode, Entities.QueryWOrderV2DataInfo query, string order, int currentPage, int pageSize, out int totalCount, ref string msg)
        {
            totalCount = 0;
            DataTable dt = null;
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "获取工单数据，授权失败。"))
            {
                dt = BLL.WOrderInfo.Instance.GetWorkOrderInfoForList(query, order, currentPage, pageSize, out totalCount);
            }
            return dt;
        }
        /// 获取业务类型
        /// <summary>
        /// 获取业务类型
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取业务类型")]
        public DataTable GetWOrderBusinessType(string Verifycode)
        {
            string msg = "";
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "授权失败。"))
            {
                BitAuto.ISDC.CC2012.Web.BusinessTypeHandler.ReturnMessage message = new BitAuto.ISDC.CC2012.Web.BusinessTypeHandler.ReturnMessage();
                try
                {
                    return BLL.WOrderBusiType.Instance.GetAllData(new Entities.QueryWOrderBusiTypeInfo() { Status = "1" });
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("WorkOrderService下的GetWorkOrderBusinessType方法异常", ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// 获取工单标签数据
        /// <summary>
        /// 获取工单标签数据
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <param name="businessTypeId">业务类型ID</param>
        /// <returns></returns>
        [WebMethod(Description = "获取工单标签数据")]
        public DataTable GetWOrderLabelData(string Verifycode, string businessTypeId, string pId)
        {
            string msg = "";
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "授权失败。"))
            {
                try
                {
                    Entities.QueryWOrderTag query = new Entities.QueryWOrderTag();
                    query.Status = "1";
                    query.BusiTypeID = businessTypeId.Trim();
                    query.PID = pId;
                    return BLL.WOrderTag.Instance.GetAllData(query);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("WorkOrderService下的GetWorkOrderLabelData方法异常", ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// 获取工单类型
        /// <summary>
        /// 获取工单类型
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取工单类型")]
        public DataTable GetWOrderCategory(string Verifycode)
        {
            string msg = "";
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "授权失败。"))
            {
                try
                {
                    DataTable dt;
                    dt = BLL.Util.GetEnumDataTable(typeof(Entities.WOrderCategoryEnum));
                    dt.TableName = "WOrderCategory";
                    return dt;
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("WorkOrderService下的GetWOrderCategory方法异常", ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
