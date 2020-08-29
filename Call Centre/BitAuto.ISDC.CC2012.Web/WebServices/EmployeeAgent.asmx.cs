using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.Services.Organization.Remoting;
using System.Configuration;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// EmployeeAgent 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class EmployeeAgent : System.Web.Services.WebService
    {
        /// <summary>
        /// 坐席工号生成，起点：1000，后续按照此点往后补漏，且自增加1
        /// </summary>
        private int genNewAgentID_StartPoint = ConfigurationManager.AppSettings["GenNewAgentID_StartPoint"] != null ? int.Parse(ConfigurationManager.AppSettings["GenNewAgentID_StartPoint"].ToString()) : 1000;

        [WebMethod(Description = "添加外包人员")]
        public void AddWBEmployeeAgent(string Verifycode, string WBUserIDStr, int RegionID, ref string msg)
        {
            msg = "";
            BLL.Loger.Log4Net.Info("调用外包人人员添加接口。人员UserID【" + WBUserIDStr + "】,区域ID【" + RegionID.ToString() + "】");

            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 1, ref msg, "添加外包人员，授权失败。"))
            {
                #region 判断参数

                if (RegionID < 1 || RegionID > 3)
                {
                    msg += "RegionID参数值不正确，应该是1、2、3；";
                }
                string[] useridlist = WBUserIDStr.Split(',');
                int intval = 0;
                foreach (string id in useridlist)
                {
                    if (!int.TryParse(id, out intval))
                    {
                        msg += "WBUserIDStr参数值不正确，用户ID都应该为数字";
                    }
                }
                #endregion

                if (msg == "")
                {
                    BLL.EmployeeAgent.Instance.AddOrUpdateWBEmployee(WBUserIDStr, RegionID, genNewAgentID_StartPoint);
                }
            }
            BLL.Loger.Log4Net.Info("调用外包人人员添加接口。人员UserID【" + WBUserIDStr + "】,区域ID【" + RegionID.ToString() + "】,结果：" + msg);
        }

        [WebMethod(Description = "获取修改的专席人员")]
        public DataTable GetEmployeeAgentNewData(string Verifycode, long timestamp, out string msg)
        {
            msg = "";
            BLL.Util.LogForWeb("info", "调用获取修改的专席人员接口。Verifycode【" + Verifycode + "】");
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "获取获取修改的专席人员，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return new DataTable() { TableName = "ExclusiveAgent" };
            }

            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgentExclusiveByTimestamp(timestamp, 5000);
            dt.TableName = "ExclusiveAgent";
            return dt;
        }
    }
}
