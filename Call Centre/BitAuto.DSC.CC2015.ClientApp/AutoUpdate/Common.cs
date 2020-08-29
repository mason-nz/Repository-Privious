using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities;
using System.Diagnostics;

namespace CC2015_HollyFormsApp.AutoUpdate
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
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "Temp_xian.xml");
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
                if (fileName != String.Empty && fileName.ToLower() != "ICSharpCode.SharpZipLib.dll".ToLower())
                {
                    //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入
                    if (theEntry.CompressedSize == 0)
                        break;
                    //解压文件到指定的目录
                    directoryName = Path.GetDirectoryName(unzippath + theEntry.Name);
                    //建立下面的目录和子目录
                    Directory.CreateDirectory(directoryName);

                    //解压覆盖文件
                    try
                    {
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
                    catch
                    {
                    }

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

        /// 判断是否打开切换窗口
        /// <summary>
        /// 判断是否打开切换窗口
        /// </summary>
        /// <returns></returns>
        public static bool IsCanShowVersionForm()
        {
            try
            {
                //密码
                string pwd = Common.LoadNodeStrFromLocalXML("UserConfig.xml", "Userconfig/AutoPassWord");
                if (!string.IsNullOrEmpty(pwd))
                {
                    //表示自动重启登录，不用选择
                    return false;
                }

                string lisfile = AppDomain.CurrentDomain.BaseDirectory + "版本切换许可.lis";
                if (File.Exists(lisfile))
                {
                    Dictionary<string, string> dic = CommonFunction.GetAllNodeContentByFile<string, string>(lisfile, "key", "value", "root");
                    if (dic.ContainsKey("SECRET"))
                    {
                        return dic["SECRET"] == "yiche-cc-woxiangqiehuanjiuqiehuan";
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        /// 获取地址
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        public static string GetLineUrl()
        {
            string url = "";
            if (MainHTTP.versiontype == VersionType.正式)
            {
                url = VersionUrl.OnLine;
            }
            else if (MainHTTP.versiontype == VersionType.测试)
            {
                url = VersionUrl.OffLine;
            }
            return url;
        }
        /// 获取指定xml文件中，指定节点中的值
        /// <summary>
        /// 获取指定xml文件中，指定节点中的值
        /// </summary>
        /// <param name="filename">xml文件名称（必须放在程序基目录）</param>
        /// <param name="xpath">节点名称</param>
        /// <returns>返回节点内容，若异常，返回字符串空</returns>
        public static string LoadNodeStrFromLocalXML(string filename, string xpath)
        {
            string nodeStr = string.Empty;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory + filename);
                XmlNode root = doc.SelectSingleNode(xpath);
                if (root != null)
                {
                    nodeStr = root.InnerText;
                }
            }
            catch (Exception ex)
            {
            }
            return nodeStr;
        }
        /// 查找相关主程序进行
        /// <summary>
        /// 查找相关主程序进行
        /// </summary>
        /// <returns></returns>
        public static Process GetMainAppProcess()
        {
            string appexe = Common.GetValByKey("StartApp", false, "HTTP");
            return GetAppProcess(appexe);
        }
        /// 查找相关主程序进行
        /// <summary>
        /// 查找相关主程序进行
        /// </summary>
        /// <returns></returns>
        public static Process GetAppProcess(string appname)
        {
            Process[] processes;
            processes = Process.GetProcesses();
            string str = "";
            foreach (Process p in processes)
            {
                try
                {
                    str = p.ProcessName;
                    if (appname.ToLower().Contains(str.ToLower()))
                    {
                        return p;
                    }
                }
                catch
                {
                }
            }
            return null;
        }
    }

    public enum VersionType
    {
        正式 = 1,
        测试 = 2
    }

    public class VersionUrl
    {
        /// <summary>
        /// 线上
        /// </summary>
        public const string OnLine = "http://ncc.sys.bitauto.com/";
        /// <summary>
        /// 线下
        /// </summary>
        public const string OffLine = "http://ncc.sys1.bitauto.com/";
    }
}
