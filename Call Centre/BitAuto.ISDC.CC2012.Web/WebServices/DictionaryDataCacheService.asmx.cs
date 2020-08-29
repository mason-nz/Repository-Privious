using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// DictionaryDataCacheService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class DictionaryDataCacheService : System.Web.Services.WebService
    {
        [WebMethod(Description = "重置缓存数据")]
        public bool ResetDictionaryDataCache(string Verifycode)
        {
            BLL.Util.LogForWeb("info", "调用接口ResetDictionaryDataCache");
            string msg = "";
            if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "重置缓存数据，授权失败。"))
            {
                BLL.Util.LogForWeb("info", "权限错误：" + Verifycode);
                BLL.Util.LogForWeb("info", "权限错误：" + msg);
                return false;
            }
            try
            {
                DictionaryDataCache.Instance.ResetData();
                BLL.Util.LogForWeb("info", "重置成功");
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
    }
}
