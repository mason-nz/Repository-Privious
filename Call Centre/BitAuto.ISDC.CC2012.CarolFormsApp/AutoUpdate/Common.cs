using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Configuration;

namespace AutoUpdate
{
    public class Common
    {
        #region 查询和设置参数

        /// <summary>
        /// 客户端的参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isDES">是否已加密</param>
        /// <returns></returns>
        public static string GetValByKey(string key, bool isDES, string TypeName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.xml");

            XmlNode root = doc.SelectSingleNode("Userconfig/" + TypeName + "/" + key);
            string skinName = root.InnerText.Trim();
            if (isDES)
            {
                //解密
                skinName = Common.DecryptDES(skinName);
            }
            return skinName;
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void SetValByKey(string key, string val, string TypeName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.xml");
            XmlNode root = doc.SelectSingleNode("Userconfig/" + TypeName + "/" + key);
            root.InnerText = val;
            doc.Save(AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.xml");
        }

        /// <summary>
        /// 服务器端的参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetServerValByKey(string key)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Temp.xml");
            XmlNode root = doc.SelectSingleNode("Userconfig/" + key);
            string skinName = root.InnerText.Trim();
            return skinName;
        }

        #endregion

        #region 加密和解密

        /// <summary>
        /// 密钥
        /// </summary>
        static string cryptoKey = ";led?s3547eds";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptoContext"></param>
        /// <returns></returns>
        public static string EncryptDES(string encryptoContext)
        {
            //取 8 位 key
            cryptoKey = cryptoKey.PadLeft(8, '0').Substring(0, 8);
            //设置加密的 key，其值来自参数
            byte[] key = Encoding.UTF8.GetBytes(cryptoKey);
            //设置加密的 iv 向量，这里使用硬编码演示
            byte[] iv = Encoding.UTF8.GetBytes("helloDES");
            //将需要加密的正文放进 byte 数组
            byte[] context = Encoding.UTF8.GetBytes(encryptoContext);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //创建内存流，用于取加密后的结果
            MemoryStream ms = new MemoryStream();
            //创建加密流，创建加密使用 des.CreateEncryptor
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);

            //向加密流写入正文
            cs.Write(context, 0, context.Length);
            //将缓冲区数据写入，然后清空缓冲区
            cs.FlushFinalBlock();

            //从内存流返回结果，并编码为 base64string 
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptoContext"></param>
        /// <returns></returns>
        public static string DecryptDES(string decryptoContext)
        {
            //取 8 位 key
            cryptoKey = cryptoKey.PadLeft(8, '0').Substring(0, 8);
            //设置解密的 key，其值来自参数
            byte[] key = Encoding.UTF8.GetBytes(cryptoKey);
            //设置解密的 iv 向量，这里使用硬编码演示
            byte[] iv = Encoding.UTF8.GetBytes("helloDES");
            //将解密正文返回到 byte 数组，加密时编码为 base64string ，这里要使用 FromBase64String 直接取回 byte 数组
            byte[] context = Convert.FromBase64String(decryptoContext);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //创建内存流，用于取解密结果
            MemoryStream ms = new MemoryStream();
            //创建解密的流， 这里的是 des.CreateDecryptor 
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write);

            //向解密流写入数据
            cs.Write(context, 0, context.Length);
            //将当前缓冲区写入绑定的内存流，然后清空缓冲区
            cs.FlushFinalBlock();

            //从内存流返回值，并编码到 UTF8 输出原文
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        #endregion

        #region 压缩和解压

        /// <summary>
        /// 解压缩文件(压缩文件中含有子目录)
        /// </summary>
        /// <param name="zipfilepath">待解压缩的文件路径</param>
        /// <param name="unzippath">解压缩到指定目录</param>
        public static void UnZip(string zipfilepath, string unzippath)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(zipfilepath));

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(unzippath);
                string fileName = Path.GetFileName(theEntry.Name);

                //生成解压目录
                Directory.CreateDirectory(directoryName);

                if (fileName != String.Empty)
                {
                    //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入
                    if (theEntry.CompressedSize == 0)
                        break;
                    //解压文件到指定的目录
                    directoryName = Path.GetDirectoryName(unzippath + theEntry.Name);
                    //建立下面的目录和子目录
                    Directory.CreateDirectory(directoryName);

                    FileStream streamWriter = File.Create(unzippath + theEntry.Name);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                }
            }
            s.Close();
        }

        #endregion

        #region GetConfigurationList
        /// <summary>
        /// 取得appSettings里的值列表
        /// </summary>
        /// <param name="key">配置文件中的key</param>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>返回value</returns>
        public static string GetConfiguration(string key, string filePath)
        {
            try
            {
                AppSettingsSection appSection = null;                       //AppSection对象
                Configuration configuration = null;                         //Configuration对象     
                //KeyValueConfigurationCollection k = null;                   //返回的键值对类型
                ExeConfigurationFileMap file = new ExeConfigurationFileMap();
                file.ExeConfigFilename = filePath;

                configuration = ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);

                //取得AppSettings节
                appSection = (AppSettingsSection)configuration.Sections["appSettings"];
                //取得AppSetting节的键值对
                return appSection.Settings[key].Value.Trim();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion

        #region SetConfiguration
        /// <summary>
        /// 设置appSetting的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">App.config文件路径</param>
        public static void SetConfiguration(string key, string value, string filePath)
        {
            Configuration configuration = null;                 //Configuration对象
            AppSettingsSection appSection = null;               //AppSection对象 
            ExeConfigurationFileMap file = new ExeConfigurationFileMap();
            file.ExeConfigFilename = filePath;

            configuration = ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);

            //取得AppSetting节
            appSection = (AppSettingsSection)configuration.Sections["appSettings"];

            //赋值并保存
            appSection.Settings[key].Value = value;
            configuration.Save();
        }
        #endregion
    }
}
