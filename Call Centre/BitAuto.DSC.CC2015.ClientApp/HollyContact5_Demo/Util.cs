using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BitAuto.Utils;
using System.IO;
using System.Text.RegularExpressions;

namespace HollyContact5_Demo
{
    public class Util
    {
        /// <summary>
        /// 根据枚举的值，得到枚举对应的文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumOptText(Type type, int value)
        {
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                FieldInfo field = fields[i];
                if (((int)Enum.Parse(type, field.Name)).ToString() == value.ToString())
                {
                    object[] objs = field.GetCustomAttributes(typeof(EnumTextValueAttribute), false);
                    if (objs == null || objs.Length == 0)
                    {
                        return field.Name;
                    }
                    else
                    {
                        EnumTextValueAttribute da = (EnumTextValueAttribute)objs[0];
                        return da.Text;
                    }
                }
            }
            return "";
        }

        /// 根据分机号码，调整OCX控件，Softphone.ini文件中的SIPServer配置
        /// <summary>
        /// 根据分机号码，调整OCX控件，Softphone.ini文件中的SIPServer配置
        /// </summary>
        /// <param name="DN">分机号码</param>
        /// <returns>设置成功返回True,否则返回False</returns>
        public static bool ModifySoftphoneINIByDN(int DN)
        {
            try
            {
                string DN_SIPServerIP_Mapping = System.Configuration.ConfigurationManager.AppSettings["DN_SIPServerIP_Mapping"];
                string sipServerIP = string.Empty;
                string[] array = DN_SIPServerIP_Mapping.Split(',');
                if (array.Length > 0)
                {
                    foreach (string DN_SIPServerIP_item in array)
                    {
                        string array_DNs_Start = DN_SIPServerIP_item.Split('|')[0].Split('-')[0];
                        string array_DNs_End = DN_SIPServerIP_item.Split('|')[0].Split('-')[1];
                        string sipServerIP_Temp = DN_SIPServerIP_item.Split('|')[1];
                        if (DN >= int.Parse(array_DNs_Start) &&
                            DN <= int.Parse(array_DNs_End))
                        {
                            sipServerIP = sipServerIP_Temp;
                            break;
                        }
                    }
                }
                Loger.Log4Net.Info("[HollyContactHelper]ModifySoftphoneINIByDN——根据DN" + DN + ",选择的SIPServerIP为：" + sipServerIP);
                if (sipServerIP == string.Empty)
                {
                    Loger.Log4Net.Info("[HollyContactHelper]ModifySoftphoneINIByDN——根据DN" + DN + ",未找到SIPServerIP");
                    return false;
                }
                string fileName = GetSoftphoneINIPath();
                string fileContent = BitAuto.Utils.FileHelper.ReadFile(fileName);
                Regex rg = new Regex(@"SIPServerIP =(\d+)\.(\d+)\.(\d+)\.(\d+)");//用正则获取orgSIPServerIP
                Match m = rg.Match(fileContent);
                string orgSIPServerIP = m.Groups[0].ToString().Split('=')[1].ToString().Trim();
                if (orgSIPServerIP != sipServerIP)
                {
                    fileContent = fileContent.Replace(orgSIPServerIP, sipServerIP);//替换原有orgSIPServerIP内容
                    return WriteFile(fileName, fileContent);//更新文件内容
                }
                return true;
            }
            catch (Exception e)
            {
                Loger.Log4Net.Error("[HollyContactHelper]ModifySoftphoneINIByDN_DN:" + DN, e);
                return false;
            }
        }

        /// 获取配置文件的位置
        /// <summary>
        /// 获取配置文件的位置
        /// </summary>
        /// <returns></returns>
        private static string GetSoftphoneINIPath()
        {
            //先取根目录，再取默认目录
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "UniSoftPhone2.1(mini)\\Softphone.ini";
            if (File.Exists(fileName))
            {
                return fileName;
            }

            string DefaultSoftphoneINI = System.Configuration.ConfigurationManager.AppSettings["DefaultSoftphoneINI"];
            if (!string.IsNullOrEmpty(DefaultSoftphoneINI))
            {
                DefaultSoftphoneINI = DefaultSoftphoneINI.TrimEnd('\\') + "\\Softphone.ini";
                if (File.Exists(DefaultSoftphoneINI))
                {
                    return DefaultSoftphoneINI;
                }
            }

            return "";
        }

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

        internal static bool IsModifyINI(int dnOrig, int dnTarget)
        {
            //根据原始登录分机号码，判断当时签入时，登录到哪个UIP（IP地址）上面
            //若这个IP，与目标分机号码不在同一个平台，那么就返回True，否则返回FALSE
            //目前厂商给出的查询方式是，查询MongoDB中，表MonitorAgentLiveData，字段：DomainFlag
            //这块，由于比较复杂，实现逻辑没有写，直接写死True了。
            return true;
            //if ((dnOrig % 10 + 30) != dnTarget / 1000)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        internal static int ModifyListenOrigDN(int dnTarget)
        {
            if (dnTarget>31000&&dnTarget<32000)
            {
                return 40001;
            }
            else if (dnTarget > 32000 && dnTarget < 33000)
            {
                return 40002;
            }
            else if (dnTarget > 33000 && dnTarget < 34000)
            {
                return 40003;
            }
            else if (dnTarget > 34000 && dnTarget < 35000)
            {
                return 40004;
            }
            else if (dnTarget > 35000 && dnTarget < 36000)
            {
                return 40005;
            }
            else
            {
                return -1;
            }
        }
    }
}
