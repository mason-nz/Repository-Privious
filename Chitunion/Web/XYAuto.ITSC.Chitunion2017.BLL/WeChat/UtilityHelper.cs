/********************************************************
*创建人：hant
*创建时间：2018/1/12 10:22:50 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    /// <summary>
    /// 微信通用处理工具类
    /// </summary>
    public class UtilityHelper
    {
        #region 字段/属性

        /// <summary>
        /// 微信自定义密钥常量
        /// </summary>
        private static readonly string Token;

        #endregion

        #region 构造函数

        static UtilityHelper()
        {
            Token = ConfigurationManager.AppSettings["WeiXinToken"];
        }

        #endregion

        #region 方法

        #region 检查加密签名是否一致

        /// <summary>
        /// 检查加密签名是否一致
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            List<string> stringList = new List<string> { Token, timestamp, nonce };
            // 字典排序
            stringList.Sort();
            return Sha1Encrypt(string.Join("", stringList)) == signature;
        }

        #endregion

        #region 对字符串SHA1加密

        /// <summary>
        /// 对字符串SHA1加密
        /// </summary>
        /// <param name="targetString">源字符串</param>
        /// <returns>加密后的十六进制字符串</returns>
        private static string Sha1Encrypt(string targetString)
        {
            byte[] byteArray = Encoding.Default.GetBytes(targetString);
            HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
            byteArray = hashAlgorithm.ComputeHash(byteArray);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte item in byteArray)
            {
                stringBuilder.AppendFormat("{0:x2}", item);
            }
            return stringBuilder.ToString();
        }

        #endregion

        #region 根据加密类型对字符串SHA1加密
        /// <summary>
        /// 根据加密类型对字符串SHA1加密
        /// </summary>
        /// <param name="targetString">源字符串</param>
        /// <param name="encryptType">加密类型：MD5/SHA1</param>
        /// <returns>加密后的字符串</returns>
        private static string Sha1Encrypt(string targetString, string encryptType)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(targetString, encryptType);
        }

        #endregion

        #endregion
    }
}
