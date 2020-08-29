using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CC2015_HollyFormsApp.CCWebVerifyPhoneFormat;

namespace CC2015_HollyFormsApp
{
    public class VerifyPhoneFormatHelper
    {
        public static readonly VerifyPhoneFormatHelper Instance = new VerifyPhoneFormatHelper();
        VerifyPhoneFormatSoapClient verifyPhoneService = null;
        public const string VerifyCode = "E0F3C0C3-5317-4D5E-9548-7E31A506EC37";

        protected VerifyPhoneFormatHelper()
        {
            verifyPhoneService = new CCWebVerifyPhoneFormat.VerifyPhoneFormatSoapClient();
        }

        /// 根据电话号码，判断是否要在外呼时，加出局号码（北京本地）
        /// <summary>
        /// 根据电话号码，判断是否要在外呼时，加出局号码（北京本地）
        /// </summary>
        /// <param name="mobile">电话号码</param>
        /// <param name="outNumber">出局号码，如“0”</param>
        /// <param name="errorMsg">报错信息</param>
        /// <returns></returns>
        public bool VerifyFormat(string mobile, out string outNumber, out string errorMsg)
        {
            return verifyPhoneService.VerifyFormat(VerifyCode, mobile, out outNumber, out errorMsg);
        }
        /// 根据电话号码，判断是否要在外呼时，加出局号码（西安本地）
        /// <summary>
        /// 根据电话号码，判断是否要在外呼时，加出局号码（西安本地）
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="outNumber"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool VerifyFormatXiAn(string mobile, out string outNumber, out string errorMsg)
        {
            return verifyPhoneService.VerifyFormatXiAn(VerifyCode, mobile, out outNumber, out errorMsg);
        }
    }
}
