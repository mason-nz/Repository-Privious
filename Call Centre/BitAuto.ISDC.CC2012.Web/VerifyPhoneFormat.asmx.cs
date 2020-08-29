using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BitAuto.ISDC.CC2012.Web
{
    /// <summary>
    /// VerifyPhoneFormat 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class VerifyPhoneFormat : System.Web.Services.WebService
    {
        private string code = "E0F3C0C3-5317-4D5E-9548-7E31A506EC37";//授权码


        [WebMethod(Description = "验证电话号码，若在库中找不到对于关系，会从网上http://sj.kvgo.net下载归属地，存在本地库中")]
        public bool VerifyFormat(string Verifycode, string phoneNumber, out string outNumber, out string errorMsg)
        {
            if (code.Equals(Verifycode))
            {
                return BLL.PhoneNumDataDict.VerifyFormatBeiJin(phoneNumber, out outNumber, out errorMsg);
            }
            else
            {
                outNumber = string.Empty; errorMsg = "授权失败";
                return false;
            }
        }

        [WebMethod(Description = "验证电话号码，若在库中找不到对于关系，会从网上http://sj.kvgo.net下载归属地，存在本地库中")]
        public bool VerifyFormatXiAn(string Verifycode, string phoneNumber, out string outNumber, out string errorMsg)
        {
            if (code.Equals(Verifycode))
            {
                return BLL.PhoneNumDataDict.VerifyFormatXiAn(phoneNumber, out outNumber, out errorMsg);
            }
            else
            {
                outNumber = string.Empty; errorMsg = "授权失败";
                return false;
            }
        }
    }
}
