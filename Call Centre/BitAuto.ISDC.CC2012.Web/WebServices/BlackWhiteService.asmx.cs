using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// BlackWhiteService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class BlackWhiteService : System.Web.Services.WebService
    {
        [WebMethod(Description = "获取新增的需要同步的黑、白名单数据（黑名单和白名单的数据一起同步）")]
        public DataTable GetSynBlackWhiteData_Insert(string Verifycode)
        {
            try
            {
                BLL.Util.LogForWeb("info", "调用接口GetSynBlackWhiteData_Insert");

                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "获取新增的需要同步的黑、白名单数据，授权失败。"))
                {
                    BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                    BLL.Util.LogForWeb("info", "权限错误：" + msg);
                    return new DataTable();
                }

                return BLL.BlackWhiteList.Instance.GetSynchrodata_BlackWhiteData_Insert();
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", "调用接口GetSynBlackWhiteData_Insert异常：" + ex.Message);
                BLL.Util.LogForWeb("error", ex.StackTrace);
                return new DataTable();
            }
        }
        [WebMethod(Description = "获取修改的需要同步的黑、白名单数据（黑名单和白名单的数据一起同步）")]
        public DataTable GetSynBlackWhiteData_Update(string Verifycode)
        {
            try
            {
                BLL.Util.LogForWeb("info", "调用接口GetSynBlackWhiteData_Update");

                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "获取修改的需要同步的黑、白名单数据，授权失败。"))
                {
                    BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                    BLL.Util.LogForWeb("info", "权限错误：" + msg);
                    return new DataTable();
                }

                return BLL.BlackWhiteList.Instance.GetSynchrodata_BlackWhiteData_Update();
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", "调用接口GetSynBlackWhiteData_Update异常：" + ex.Message);
                BLL.Util.LogForWeb("error", ex.StackTrace);
                return new DataTable();
            }
        }
        [WebMethod(Description = "将指定的RecIDS(多个RecID之间用“，”隔开)的SynchrodataStatus值改为2")]
        public bool UpdateSuccessSynchrodateStatus(string Verifycode, string RecIDs)
        {
            try
            {
                BLL.Util.LogForWeb("info", "开始更新RecId为（" + RecIDs + "）的数据的状态");

                string msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "UpdateSuccessSynchrodateStatus，授权失败。"))
                {
                    BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                    BLL.Util.LogForWeb("info", "权限错误：" + msg);
                    return false;
                }

                bool bl = BLL.BlackWhiteList.Instance.UpdateSuccessSynchrodateStatus(RecIDs);
                BLL.Util.LogForWeb("info", "更新RecId为（" + RecIDs + "）的数据的状态结束,，返回结果为：" + bl);
                return bl;
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", "调用接口UpdateSuccessSynchrodateStatus异常：" + ex.Message);
                BLL.Util.LogForWeb("error", ex.StackTrace);
                return false;
            }
        }
        [WebMethod(Description = "获取北京库变化的热线表数据")]
        public DataTable GetChangedCallDisplayFromBJ(string Verifycode, long maxid, out string msg)
        {
            try
            {
                msg = "";
                BLL.Util.LogForWeb("info", "调用接口GetChangedCallDisplayFromBJ");
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "GetChangedCallDisplayFromBJ，授权失败。"))
                {
                    BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                    BLL.Util.LogForWeb("info", "权限错误：" + msg);
                    return null;
                }
                DataTable dt = BLL.CallDisplay.Instance.GetChangedCallDisplayFromBJ(maxid);
                dt.TableName = "CallDisplay";
                msg = "获取北京库变化的数据" + dt.Rows.Count + "条";
                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BLL.Util.LogForWeb("info", "调用接口GetChangedCallDisplayFromBJ异常：" + ex.Message);
                BLL.Util.LogForWeb("error", ex.StackTrace);
                return null;
            }
        }
        [WebMethod(Description = "获取黑名单数据")]
        public byte[] GetBlackListDataForOutCall(string verifyCode, long timestamp, int maxRow, out string msg)
        {
            try
            {
                BLL.Util.LogForWeb("info", "调用接口GetBlackListDataForOutCall：timestamp=" + timestamp + " maxRow=" + maxRow);

                msg = "";
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(verifyCode, 0, ref msg, "GetBlackListDataForOutCall，授权失败。"))
                {
                    BLL.Util.LogForWeb("info", "权限错误：" + verifyCode);
                    BLL.Util.LogForWeb("info", "权限错误：" + msg);
                    return null;
                }
                maxRow = Math.Abs(maxRow);
                timestamp = Math.Abs(timestamp);
                if (maxRow > 10000)
                {
                    msg = "参数[maxRow]值超过10000";
                    BLL.Util.LogForWeb("info", msg);
                    return null;
                }
                DataTable dt = BLL.BlackWhiteList.Instance.GetBlackListDataForOutCall(timestamp, maxRow);
                byte[] b = BLL.Util.DataTableToBinary(dt);
                BLL.Util.LogForWeb("info", "查询数据条数=" + dt.Rows.Count + " 压缩后大小=" + (b.Length / 1024.0 / 1024.0).ToString("0.000") + "MB");
                return b;
            }
            catch (Exception ex)
            {
                BLL.Util.LogForWeb("info", "调用接口GetBlackListDataForOutCall异常：" + ex.Message);
                BLL.Util.LogForWeb("error", ex.StackTrace);
                msg = ex.Message;
                return null;
            }
        }
    }
}
