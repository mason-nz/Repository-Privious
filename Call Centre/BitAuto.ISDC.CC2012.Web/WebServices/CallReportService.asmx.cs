using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// CallReportService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CallReportService : System.Web.Services.WebService
    {
        [WebMethod(Description = "同步小时路由数据")]
        public bool SyncRoutepointHourData(string Verifycode, byte[] data, ref string msg)
        {
            BLL.Util.LogForWeb("info", "调用接口SyncRoutepointHourData");
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "同步小时路由数据，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return false;
            }
            try
            {
                DataSet ds = BitAuto.ISDC.CC2012.BLL.Util.RetrieveDataSet(data);
                if (ds == null || ds.Tables.Count == 0)
                {
                    msg = "传入参数data解析失败";
                    BLL.Util.LogForWeb("info", msg);
                    return false;
                }
                else
                {
                    BLL.Util.LogForWeb("info", "传入数据量：" + ds.Tables[0].Rows.Count);
                }
                bool a = BLL.Routepoint.Instance.SyncRoutepointHourData(ds.Tables[0], out msg);
                BLL.Util.LogForWeb("info", "操作结果：" + a.ToString() + " " + msg);
                return a;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return false;
            }
        }
        [WebMethod(Description = "获取Genesys小时路由数据的最大时间")]
        public DateTime? GetMaxDateTimeFromHourData(string Verifycode)
        {
            BLL.Util.LogForWeb("info", "调用接口获取Genesys小时路由数据的最大时间");
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "获取Genesys小时路由数据的最大时间，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return null;
            }
            try
            {
                DateTime dt = BLL.Routepoint.Instance.GetMaxDateTimeFromTable(BLL.Routepoint.TableName.REPORT_ROUTEPOINT_HOUR, Entities.Vender.Genesys);
                BLL.Util.LogForWeb("info", "获取数据：" + dt);
                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return null;
            }
        }
        [WebMethod(Description = "清除Genesys某个时间段的小时路由数据")]
        public int ClearHourDataFormBeginToEnd(string Verifycode, DateTime st, DateTime et)
        {
            BLL.Util.LogForWeb("info", "调用接口清除Genesys某个时间段的小时路由数据");
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "清除Genesys某个时间段的小时路由数据，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return -1;
            }
            try
            {
                int a = BLL.Routepoint.Instance.ClearDataFormBeginToEnd(BLL.Routepoint.TableName.REPORT_ROUTEPOINT_HOUR, Entities.Vender.Genesys, st, et);
                BLL.Util.LogForWeb("info", "删除数据：" + a);
                return a;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return -1;
            }
        }

        [WebMethod(Description = "同步15分钟路由数据")]
        public bool SyncRoutepoint15MinData(string Verifycode, byte[] data, ref string msg)
        {
            BLL.Util.LogForWeb("info", "调用接口SyncRoutepoint15MinData");
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "同步15分钟路由数据，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return false;
            }
            try
            {
                DataSet ds = BitAuto.ISDC.CC2012.BLL.Util.RetrieveDataSet(data);
                if (ds == null || ds.Tables.Count == 0)
                {
                    msg = "传入参数data解析失败";
                    BLL.Util.LogForWeb("info", msg);
                    return false;
                }
                else
                {
                    BLL.Util.LogForWeb("info", "传入数据量：" + ds.Tables[0].Rows.Count);
                }
                bool a = BLL.Routepoint.Instance.SyncRoutepoint15MinData(ds.Tables[0], out msg);
                BLL.Util.LogForWeb("info", "操作结果：" + a.ToString() + " " + msg);
                return a;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return false;
            }
        }
        [WebMethod(Description = "获取Genesys15分钟路由数据的最大时间")]
        public DateTime? GetMaxDateTimeFrom15MinData(string Verifycode)
        {
            BLL.Util.LogForWeb("info", "调用接口获取Genesys15分钟路由数据的最大时间");
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "获取Genesys15分钟路由数据的最大时间，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return null;
            }
            try
            {
                DateTime dt = BLL.Routepoint.Instance.GetMaxDateTimeFromTable(BLL.Routepoint.TableName.REPORT_ROUTEPOINT_15MINUTES, Entities.Vender.Genesys);
                BLL.Util.LogForWeb("info", "获取数据：" + dt);
                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return null;
            }
        }
        [WebMethod(Description = "清除Genesys某个时间段的15分钟路由数据")]
        public int Clear15MinDataFormBeginToEnd(string Verifycode, DateTime st, DateTime et)
        {
            BLL.Util.LogForWeb("info", "调用接口清除Genesys某个时间段的15分钟路由数据");
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "清除Genesys某个时间段的15分钟路由数据，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return -1;
            }
            try
            {
                int a = BLL.Routepoint.Instance.ClearDataFormBeginToEnd(BLL.Routepoint.TableName.REPORT_ROUTEPOINT_15MINUTES, Entities.Vender.Genesys, st, et);
                BLL.Util.LogForWeb("info", "删除数据：" + a);
                return a;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return -1;
            }
        }

        [WebMethod(Description = "清除某厂家某个时间段的小时路由数据")]
        public int ClearVendorHourDataFormBeginToEnd(string Verifycode, DateTime st, DateTime et, Entities.Vender vendor)
        {
            BLL.Util.LogForWeb("info", "调用接口清除厂家某个时间段的小时路由数据");
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "清除厂家某个时间段的小时路由数据，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return -1;
            }
            try
            {
                int a = BLL.Routepoint.Instance.ClearDataFormBeginToEnd(BLL.Routepoint.TableName.REPORT_ROUTEPOINT_HOUR, vendor, st, et);
                BLL.Util.LogForWeb("info", "删除数据：" + a);
                return a;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return -1;
            }
        }
        [WebMethod(Description = "清除某厂家某个时间段的15分钟路由数据")]
        public int ClearVendor15MinDataFormBeginToEnd(string Verifycode, DateTime st, DateTime et, Entities.Vender vendor)
        {
            BLL.Util.LogForWeb("info", "调用接口清除厂家某个时间段的15分钟路由数据");
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "清除厂家某个时间段的15分钟路由数据，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return -1;
            }
            try
            {
                int a = BLL.Routepoint.Instance.ClearDataFormBeginToEnd(BLL.Routepoint.TableName.REPORT_ROUTEPOINT_15MINUTES, vendor, st, et);
                BLL.Util.LogForWeb("info", "删除数据：" + a);
                return a;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return -1;
            }
        }

        [WebMethod(Description = "通过合力数据更新话务总表")]
        public bool UpdateCallRecordORIGByHolly(string Verifycode, byte[] data, WorkType worktype, ref string msg)
        {
            BLL.Util.LogForWeb("info", "调用接口UpdateCallRecordORIGByHolly == 工作方式： " + worktype);
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "通过合力数据更新话务总表，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return false;
            }
            try
            {
                DataSet ds = BitAuto.ISDC.CC2012.BLL.Util.RetrieveDataSet(data);
                if (ds == null || ds.Tables.Count == 0)
                {
                    msg = "传入参数data解析失败";
                    BLL.Util.LogForWeb("info", msg);
                    return false;
                }
                else
                {
                    BLL.Util.LogForWeb("info", "传入数据量：" + ds.Tables[0].Rows.Count);
                }
                int a = BLL.CallRecord_ORIG.Instance.UpdateCallRecordORIGByHolly(ds.Tables[0], (int)worktype, new Action<string, string>(Log));
                BLL.Util.LogForWeb("info", "操作结果：" + a.ToString() + " " + msg);
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\n" + ex.StackTrace;
                BLL.Util.LogForWeb("info", "发生异常");
                BLL.Util.LogForWeb("info", msg);
                return false;
            }
        }

        public void Log(string staus, string msg)
        {
            BLL.Util.LogForWeb(staus, msg);
        }

        public enum WorkType { 同步话务 = 1, 同步未接 = 2 }
    }
}
