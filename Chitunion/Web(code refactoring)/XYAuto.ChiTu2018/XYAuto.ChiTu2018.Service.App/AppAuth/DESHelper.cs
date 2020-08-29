using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XYAuto.ChiTu2018.Service.App.AppAuth
{
    public class DESHelper
    {
        /// <summary>
        /// DES加密算法
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <param name="sKey">加密码Key</param>
        /// <returns>正确返回加密后的结果，错误返回源字符串</returns>
        public static string ToDESEncrypt(string encryptString, string sKey)
        {
            try
            {

                byte[] keyBytes = Encoding.UTF8.GetBytes(sKey);
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

                // java 默认的是ECB模式，PKCS5padding；c#默认的CBC模式，PKCS7padding 所以这里我们默认使用ECB方式
                desProvider.Mode = CipherMode.ECB;
                MemoryStream memStream = new MemoryStream();
                CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);

                crypStream.Write(inputByteArray, 0, inputByteArray.Length);
                crypStream.FlushFinalBlock();
                return Convert.ToBase64String(memStream.ToArray());

            }
            catch
            {
                return encryptString;
            }
        }


        /// <summary>
        /// DES解密算法
        /// </summary>
        /// <param name="decryptString">要解密的字符串</param>
        /// <param name="sKey">加密Key</param>
        /// <returns>正确返回加密后的结果，错误返回源字符串</returns>
        public static string ToDESDecrypt(string decryptString, string sKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(sKey);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);

            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            // java 默认的是ECB模式，PKCS5padding；c#默认的CBC模式，PKCS7padding 所以这里我们默认使用ECB方式
            desProvider.Mode = CipherMode.ECB;
            MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);

            crypStream.Write(inputByteArray, 0, inputByteArray.Length);
            crypStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(memStream.ToArray());

        }
    }
}
