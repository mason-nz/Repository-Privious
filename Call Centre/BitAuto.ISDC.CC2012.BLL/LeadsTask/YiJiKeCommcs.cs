using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class YiJiKeCommcs
    {

        #region Instance
        public static readonly YiJiKeCommcs Instance = new YiJiKeCommcs();
        #endregion

        /// <summary>
        /// 验证授权逻辑
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="msg">返回信息</param>
        /// <param name="errorMsg">传入信息</param>
        /// <returns>验证通过为True，否则返回False</returns>
        public bool Verify(string Verifycode,ref string msg, string errorMsg)
        {
            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            if (BLL.CallRecord_ORIG_Authorizer.Instance.IsExistsByIPAndCode(userHostAddress, Verifycode, 0))
            {
                return true;
            }
            else
            {
                msg = errorMsg + "userHostAddress=" + userHostAddress + ",Verifycode=" + Verifycode;
                return false;
            }
        }
    }
}
