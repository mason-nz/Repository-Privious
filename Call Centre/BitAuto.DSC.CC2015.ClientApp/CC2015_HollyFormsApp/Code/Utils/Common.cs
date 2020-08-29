using System;
using System.Text;
using System.Xml;
using System.IO;
using CC2015_HollyFormsApp.CCWeb.CallRecordService;
using System.Configuration;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.Data;
using ICSharpCode.SharpZipLib.Checksums;
using System.Collections;

namespace CC2015_HollyFormsApp
{
    public static class Common
    {
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
                Loger.Log4Net.Info(string.Format("Call LoadNodeStrFromLocalXML({0},{1}) Failed.", filename, xpath), ex);
            }
            return nodeStr;
        }
        /// 根据登录域账号及分机信息，保存到客户端本地配置文件中
        /// <summary>
        /// 根据登录域账号及分机信息，保存到客户端本地配置文件中
        /// </summary>
        /// <param name="extension">登录域账号</param>
        /// <param name="domainAccount">分机号码</param>
        public static void SaveLoginInfoToLocalXML(string extension, string domainAccount, string pwd = "")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + Constant.ClientLocalConfigXMLName);

            AddNode(doc, Constant.ClientLocalConfigADNameNode, domainAccount);
            AddNode(doc, Constant.ClientLocalConfigExtensionNode, extension);
            AddNode(doc, Constant.ClientLocalConfigPassWordNode, pwd);

            doc.Save(AppDomain.CurrentDomain.BaseDirectory + Constant.ClientLocalConfigXMLName);
        }

        /// 客户端的参数
        /// <summary>
        /// 客户端的参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isDES">是否已加密</param>
        /// <returns></returns>
        public static string GetValByKey(string key, string TypeName)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.xml";
            if (File.Exists(file))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.xml");
                XmlNode root = doc.SelectSingleNode("Userconfig/" + TypeName + "/" + key);
                string skinName = root.InnerText.Trim();
                return skinName;
            }
            else return "";
        }
        /// 保存参数
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

        private static void AddNode(XmlDocument doc, string key, string value)
        {
            XmlNode xn = doc.SelectSingleNode(key);
            if (xn == null)
            {
                //保存域账号
                XmlElement xe = doc.CreateElement(key.Split('/')[1]);
                xe.InnerText = value;
                doc.SelectSingleNode(key.Split('/')[0]).AppendChild(xe);
            }
            else
            {
                xn.InnerText = value;
            }
        }
        /// 获取皮肤文件路径
        /// <summary>
        /// 获取皮肤文件路径
        /// </summary>
        public static string GetSkinFileName()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "UserConfig.xml");
            XmlNode root = doc.SelectSingleNode("Userconfig/skin");
            string skinName = root.InnerText;
            return skinName;
        }
        /// 处理字符串为合法url
        /// <summary>
        /// 处理字符串为合法url
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Uri getUrl(string address)
        {
            string tempaddress = address;
            if ((!address.StartsWith("http://")) && (!address.StartsWith("https://")) && (!address.StartsWith("ftp://")))
            {
                tempaddress = "http://" + address;
            }
            Uri myurl;
            try
            {
                myurl = new Uri(tempaddress);
            }
            catch
            {
                myurl = new Uri("about:blank");
            }
            return myurl;
        }
        /// 保存皮肤文件路径
        /// <summary>
        /// 保存皮肤文件路径
        /// </summary>
        /// <param name="skinPath"></param>
        public static void SaveSkin(string skinPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "UserConfig.xml");
            XmlNode root = doc.SelectSingleNode("Userconfig/skin");
            root.InnerText = skinPath;
            doc.Save(AppDomain.CurrentDomain.BaseDirectory + "UserConfig.xml");
        }
        /// 时间戳转为C#格式时间
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(int timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp.ToString() + "0000000");
            //以 100 毫微秒 1000*1000*1000/100=1 000 000 0
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        /// 时间转时间戳
        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetTime(DateTime time)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - dtStart).TotalSeconds;
        }
        /// ASPNET实现javascript的escape 
        /// <summary>
        /// ASPNET实现javascript的escape 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回加密后字符串</returns>
        public static string EscapeString(string str)
        {
            return Microsoft.JScript.GlobalObject.escape(str);
        }
        /// 生成新CallID
        /// <summary>
        /// 生成新CallID
        /// </summary>
        /// <param name="timestamp">CTI事件时间戳</param>
        /// <returns>返回新CallID（10位）</returns>
        public static long GetNewCallID(DateTime date)
        {
            int timestamp = GetTime(date);
            Loger.Log4Net.Info("[======Common======] GetNewCallID ：生成新CallID时 date：" + date + " timestamp：" + timestamp);
            Random r = new Random();
            Int64 callid = 0;
            string scallid = "";
            //分机号码
            string extNum = LoginUser.ExtensionNum;
            if (String.IsNullOrEmpty(extNum))
            {
                extNum = r.Next(1000, 10000).ToString();
            }
            try
            {
                scallid = extNum + timestamp.ToString() + r.Next(100, 1000).ToString();
                callid = Convert.ToInt64(scallid);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("生成新CallID时，出错:", ex);
            }
            Loger.Log4Net.Info("[======Common======] GetNewCallID ：生成新CallID：" + callid);
            return callid;
        }
        /// 发送邮件事件实现
        /// <summary>
        /// 发送邮件事件实现
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="source"></param>
        /// <param name="stackTrace"></param>
        public static void SendErrorEmail(string subject, string errorMsg, string source, string stackTrace)
        {
            //string mailBody = string.Format("错误信息：{0}<br/>错误Source：{1}<br/>错误StackTrace：{2}<br/>登录客服：{3}<br/>",
            //        errorMsg, source, stackTrace, LoginUser.TrueName);
            //string userEmails = ConfigurationManager.AppSettings["ReceiveErrorEmail"];
            //string[] userEmail = userEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //BitAuto.ISDC.CC2012.BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
        }
        /// 话务数据备份
        /// <summary>
        /// 话务数据备份
        /// </summary>
        public static void WriteCallDataBackUp(CallRecord_ORIG BackupModel)
        {
            string fpath = AppDomain.CurrentDomain.BaseDirectory + "log\\CallRecord_ORIG\\SendError_" + DateTime.Today.ToString("yyyy_MM_dd") + ".txt";

            string path = Path.GetDirectoryName(fpath);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(fpath))
            {
                FileInfo myfile = new FileInfo(fpath);
                FileStream fs = myfile.Create();
                fs.Close();
            }

            StreamWriter sw = File.AppendText(fpath);

            string sContent = "话务数据备份-" + DateTime.Now.ToString() + "\r\n";
            if (BackupModel != null)
            {
                sContent += "SessionID：" + (BackupModel.SessionID == null ? "null" : BackupModel.SessionID) + ",\r\n";
                sContent += "CallID：" + (BackupModel.CallID.HasValue ? BackupModel.CallID.Value.ToString() : "null") + ",\r\n";
                sContent += "ExtensionNum：" + (BackupModel.ExtensionNum == null ? "null" : BackupModel.ExtensionNum) + ",\r\n";
                sContent += "PhoneNum：" + (BackupModel.PhoneNum == null ? "null" : BackupModel.PhoneNum) + ",\r\n";
                sContent += "ANI：" + (BackupModel.ANI == null ? "null" : BackupModel.ANI) + ",\r\n";
                sContent += "CallStatus：" + (BackupModel.CallStatus.HasValue ? BackupModel.CallStatus.ToString() : "null") + ",\r\n";

                sContent += "SwitchINNum：" + (BackupModel.SwitchINNum == null ? "null" : BackupModel.SwitchINNum) + ",\r\n";
                sContent += "OutBoundType：" + (BackupModel.OutBoundType.HasValue ? BackupModel.OutBoundType.ToString() : "null") + ",\r\n";
                sContent += "SkillGroup：" + (BackupModel.SkillGroup == null ? "null" : BackupModel.SkillGroup) + ",\r\n";
                sContent += "InitiatedTime：" + (BackupModel.InitiatedTime.HasValue ? BackupModel.InitiatedTime.ToString() : "null") + ",\r\n";
                sContent += "RingingTime：" + (BackupModel.RingingTime.HasValue ? BackupModel.RingingTime.ToString() : "null") + ",\r\n";
                sContent += "EstablishedTime：" + (BackupModel.EstablishedTime.HasValue ? BackupModel.EstablishedTime.ToString() : "null") + ",\r\n";

                sContent += "AgentReleaseTime：" + (BackupModel.AgentReleaseTime.HasValue ? BackupModel.AgentReleaseTime.ToString() : "null") + ",\r\n";
                sContent += "CustomerReleaseTime：" + (BackupModel.CustomerReleaseTime.HasValue ? BackupModel.CustomerReleaseTime.ToString() : "null") + ",\r\n";
                sContent += "AfterWorkBeginTime：" + (BackupModel.AfterWorkBeginTime.HasValue ? BackupModel.AfterWorkBeginTime.ToString() : "null") + ",\r\n";
                sContent += "AfterWorkTime：" + (BackupModel.AfterWorkTime.HasValue ? BackupModel.AfterWorkTime.ToString() : "null") + ",\r\n";
                sContent += "ConsultTime：" + (BackupModel.ConsultTime.HasValue ? BackupModel.ConsultTime.ToString() : "null") + ",\r\n";
                sContent += "ReconnectCall：" + (BackupModel.ReconnectCall.HasValue ? BackupModel.ReconnectCall.ToString() : "null") + ",\r\n";

                sContent += "TallTime：" + (BackupModel.TallTime.HasValue ? BackupModel.TallTime.ToString() : "null") + ",\r\n";
                sContent += "AudioURL：" + (BackupModel.AudioURL == null ? "null" : BackupModel.AudioURL) + ",\r\n";
                sContent += "CreateTime：" + (BackupModel.CreateTime.HasValue ? BackupModel.CreateTime.ToString() : "null") + ",\r\n";
                sContent += "CreateUserID：" + (BackupModel.CreateUserID.HasValue ? BackupModel.CreateUserID.ToString() : "null") + ",\r\n";
                sContent += "TransferInTime：" + (BackupModel.TransferInTime.HasValue ? BackupModel.TransferInTime.ToString() : "null") + ",\r\n";
                sContent += "TransferOutTime：" + (BackupModel.TransferOutTime.HasValue ? BackupModel.TransferOutTime.ToString() : "null") + "\r\n";
            }

            sw.WriteLine(sContent);
            sw.Flush();
            sw.Close();
        }
        /// 呼入技能组
        /// <summary>
        /// 呼入技能组
        /// </summary>
        /// <param name="skcode">字母ID</param>
        /// <returns></returns>
        public static string GetSkillGroupText(string skcode)
        {
            //字母ID
            return " (" + skcode + ")";
        }

        #region 时间计时
        /// 服务器时间与当前时间的差值
        /// <summary>
        /// 服务器时间与当前时间的差值
        /// </summary>
        private static TimeSpan CC_AgentTimeSpan = new TimeSpan();

        /// 获取当前时间
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentTime()
        {
            DateTime dt;
            string logMsg = string.Empty;
            //如果CC_AgentTimeSpan为空，则计算
            if (CC_AgentTimeSpan == new TimeSpan())
            {
                //先去服务器时间
                DateTime ccdate = AgentTimeStateHelper.Instance.GetCurrentTime();
                //再取客户端时间
                DateTime agdate = DateTime.Now;
                //计算差值
                CC_AgentTimeSpan = ccdate - agdate;
                //返回服务器时间
                dt = ccdate;
                logMsg = string.Format("CC系统服务器时间：{0}，客户端时间：{1}，（生成）计算时间差(毫秒)：{2}", dt.ToString("yyyy-MM-dd HH:mm:ss.fff"), agdate.ToString("yyyy-MM-dd HH:mm:ss.fff"), CC_AgentTimeSpan.TotalMilliseconds);
            }
            else
            {
                //取客户端时间
                DateTime agdate = DateTime.Now;
                //返回客户端时间+CC_AgentTimeSpan
                DateTime calc = agdate + CC_AgentTimeSpan;
                dt = calc;
                logMsg = string.Format("CC系统服务器时间：{0}，客户端时间：{1}，（现有）计算时间差(毫秒)：{2}", dt.ToString("yyyy-MM-dd HH:mm:ss.fff"), agdate.ToString("yyyy-MM-dd HH:mm:ss.fff"), CC_AgentTimeSpan.TotalMilliseconds);
            }

            Loger.Log4Net.Info("[======Common======] GetCurrentTime ：获取当前服务器时间：" + logMsg);
            return dt;
        }
        /// 重新计算时间差值
        /// <summary>
        /// 重新计算时间差值
        /// </summary>
        /// <returns></returns>
        public static void RefreshTime()
        {
            CC_AgentTimeSpan = new TimeSpan();
        }
        #endregion

        #region 读写文件
        /// 获取版本号
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            //本地客户端版本
            return Common.GetValByKey("Versions_Local", "HTTP").Trim();
        }
        /// 写文件
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        /// <param name="content">文件内容</param>
        /// <returns>成功返回True，是吧返回False</returns>
        public static bool WriteFile(string filePath, string content)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(content);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                Loger.Log4Net.Error("WriteFile Error,FilePath:" + filePath, e);
                return false;
            }

        }
        #endregion

        /// 解压缩文件(压缩文件中含有子目录)
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
        /// 转换时间
        /// <summary>
        /// 转换时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ObjectToDateTime(object obj)
        {
            if (obj == null || obj.ToString().Trim() == "")
            {
                return null;
            }
            DateTime a;
            if (DateTime.TryParse(obj.ToString().Substring(0, 19), out a))
            {
                int fff = int.Parse(obj.ToString().Substring(20));
                a = a.AddMilliseconds(fff);
                return a;
            }
            else
            {
                return null;
            }
        }
        /// 选择字符串
        /// <summary>
        /// 选择字符串
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ChooseString(string a, string b, Choose type)
        {
            if (type == Choose.手机号)
            {
                return a.Length > b.Length ? a : b;
            }
            else if (type == Choose.分机)
            {
                return a.Length > b.Length ? b : a;
            }
            return "";
        }
        private static DataTable linedt = null;
        /// 获取所有的热线和技能组信息
        /// <summary>
        /// 获取所有的热线和技能组信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllSkillInfoAndLineInfo()
        {
            if (linedt == null)
                linedt = AgentTimeStateHelper.Instance.GetHotlineSkillGroupByTelMainNum("");
            return linedt;
        }
        /// 获取字母ID根据数字ID
        /// <summary>
        /// 获取字母ID根据数字ID
        /// </summary>
        /// <param name="SGID"></param>
        /// <returns></returns>
        public static string GetManufacturerSGIDBySGID(int SGID)
        {
            DataTable dt = GetAllSkillInfoAndLineInfo();
            DataRow[] drs = dt.Select("sgid='" + SGID + "'");
            if (drs.Length > 0)
            {
                return BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToString(drs[0]["ManufacturerSGID"]);
            }
            else return AgentTimeStateHelper.Instance.GetManufacturerSGIDBySGID(SGID);
        }
        /// 获取服务器端的最新版本号
        /// <summary>
        /// 获取服务器端的最新版本号
        /// </summary>
        /// <returns></returns>
        public static string GetSeverVersion()
        {
            try
            {
                string ServerVersionsName = Common.GetValByKey("ServerVersionsName", "HTTP").Trim();
                return ClientAssistantHelper.Instance.GetClientServerVersion(ServerVersionsName);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("GetSeverVersion 异常", ex);
                return null;
            }
        }
        /// 重新启动
        /// <summary>
        /// 重新启动
        /// </summary>
        public static void Restart()
        {
            //保存用户名密码
            string pwd = BitAuto.ISDC.CC2012.BLL.Util.EncryptString(LoginUser.Password);
            Common.SaveLoginInfoToLocalXML(LoginUser.ExtensionNum, LoginUser.DomainAccount, pwd);
            //退出系统
            if (LoginHelper.Instance.ExitLogin())
            {
                //重新启动更新程序
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "CC2015_HollyFormsApp.AutoUpdate.exe");
                System.Environment.Exit(0);
            }
        }
        /// 检查版本是否旧的，是否继续
        /// <summary>
        /// 检查版本是否旧的，是否继续
        /// </summary>
        /// <returns></returns>
        public static bool IsOldVersionContinue()
        {
            bool iscontinue = true;
            //不可用（3种状态 true false null 只有ture不更新）
            string OldVersionEnable = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("OldVersionEnable", false);
            //不可用
            if (OldVersionEnable != "true")
            {
                if (UpdateTip.HasShow)
                {
                    if (UpdateTip.Instance != null)
                    {
                        //关闭上一个提示窗口
                        UpdateTip.Instance.Close();
                    }
                    //显示提示窗口
                    UpdateTip form = new UpdateTip();
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                    iscontinue = false;
                }
            }
            return iscontinue;
        }
        /// 获取登录区域-参数：分机号
        /// <summary>
        /// 获取登录区域-参数：分机号
        /// </summary>
        public static AreaType GetLoginAreaType(int extensionnum)
        {
            try
            {
                string LocalRegion = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalRegion", false);
                string[] groups = LocalRegion.Split(',');
                foreach (string groupitem in groups)
                {
                    string min = groupitem.Split('|')[0].Split('-')[0];
                    string max = groupitem.Split('|')[0].Split('-')[1];
                    string areaname = groupitem.Split('|')[1];
                    int min_ext = BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(min);
                    int max_ext = BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(max);

                    if (min_ext <= extensionnum && extensionnum <= max_ext)
                    {
                        return (AreaType)Enum.Parse(typeof(AreaType), areaname);
                    }
                }
            }
            catch
            {
            }
            return AreaType.西安;
        }
        /// 给座机加前缀
        /// <summary>
        /// 给座机加前缀
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string AddPrex(string phone)
        {
            if (phone.Length == 8)
            {
                phone = GetAreaCode() + phone;
            }
            return phone;
        }
        /// 获取区号
        /// <summary>
        /// 获取区号
        /// </summary>
        /// <returns></returns>
        public static string GetAreaCode()
        {
            string areacode = "";
            if (LoginUser.LoginAreaType == AreaType.西安)
            {
                areacode = "029";
            }
            else if (LoginUser.LoginAreaType == AreaType.北京)
            {
                areacode = "010";
            }
            return areacode;
        }
        /// 压缩文件夹的方法
        /// <summary>
        /// 压缩文件夹的方法
        /// </summary>
        /// <param name="DirToZip">需要压缩的文件路径</param>
        /// <param name="ZipedFile">压缩文件存放路径（需要包含压缩文件名称）</param>
        /// <param name="CompressionLevel">压缩率0（无压缩）-9（压缩率最高）</param>
        public static void ZipDir(string DirToZip, string ZipedFile, int CompressionLevel)
        {
            //压缩文件为空时默认与压缩文件夹同一级目录  
            if (ZipedFile == string.Empty)
            {
                ZipedFile = DirToZip.Substring(DirToZip.LastIndexOf("/") + 1);
                ZipedFile = DirToZip.Substring(0, DirToZip.LastIndexOf("/")) + "//" + ZipedFile + ".zip";
            }

            if (Path.GetExtension(ZipedFile) != ".zip")
            {
                ZipedFile = ZipedFile + ".zip";
            }

            using (ZipOutputStream zipoutputstream = new ZipOutputStream(File.Create(ZipedFile)))
            {
                zipoutputstream.SetLevel(CompressionLevel);
                Crc32 crc = new Crc32();
                Hashtable fileList = GetAllFies(DirToZip);
                foreach (DictionaryEntry item in fileList)
                {
                    string str1 = item.Key.ToString();

                    FileStream fs = File.OpenRead(item.Key.ToString());
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(DirToZip.Length));
                    entry.DateTime = (DateTime)item.Value;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipoutputstream.PutNextEntry(entry);
                    zipoutputstream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        private static Hashtable GetAllFies(string dir)
        {
            Hashtable FilesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            GetAllDirFiles(fileDire, FilesList);
            GetAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// <summary>  
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// </summary>  
        /// <param name="dirs"></param>  
        /// <param name="filesList"></param>  
        private static void GetAllDirsFiles(DirectoryInfo[] dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                GetAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        /// 获取一个文件夹下的文件
        /// <summary>  
        /// 获取一个文件夹下的文件  
        /// </summary>  
        /// <param name="strDirName">目录名称</param>  
        /// <param name="filesList">文件列表HastTable</param>  
        private static void GetAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }

        /// 是否测试版本
        /// <summary>
        /// 是否测试版本
        /// </summary>
        /// <returns></returns>
        public static bool IsTestVersion()
        {
            string url = Common.GetValByKey("BaseURL", "HTTP").Trim();
            if (url.Contains("sys1"))
            {
                //测试
                return true;
            }
            else
            {
                //正式
                return false;
            }
        }
    }

    public class KeyValue
    {
        public string Key = "";
        public string Value = "";

        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            KeyValue b = obj as KeyValue;
            if (b != null)
            {
                return this.Key.Equals(b.Key);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
