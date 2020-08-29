using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BitAuto.ISDC.CC2012.Entities;

namespace CC2015_HollyFormsApp
{
    public static class AutoLoginHelper
    {
        private static string fileName = "";

        static AutoLoginHelper()
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\CC2015_HollyFormsApp";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            fileName = root + "\\historydb.his";
            if (!File.Exists(fileName))
            {
                FileStream fs = File.Create(fileName);
                fs.Close();
            }
        }
        /// 保存配置文件
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="autologin"></param>
        /// <param name="remeberpwd"></param>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="extension"></param>
        public static void SaveConfig(bool autologin, bool remeberpwd, string name, string pwd, string extension)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["autologin"] = BitAuto.ISDC.CC2012.BLL.Util.EncryptString(autologin.ToString());
            dic["remeberpwd"] = BitAuto.ISDC.CC2012.BLL.Util.EncryptString(remeberpwd.ToString());
            dic["name"] = BitAuto.ISDC.CC2012.BLL.Util.EncryptString(name);
            dic["pwd"] = BitAuto.ISDC.CC2012.BLL.Util.EncryptString(pwd);
            dic["extension"] = BitAuto.ISDC.CC2012.BLL.Util.EncryptString(extension);
            try
            {
                CommonFunction.SaveDictionaryToFile<string, string>(dic, "key", "value", fileName);
            }
            catch
            {
            }
        }
        /// 读取配置文件
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ReadConfig()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                dic = CommonFunction.GetAllNodeContentByFile<string, string>(fileName, "key", "value", null);
            }
            catch
            {
            }
            DecryptStringDic(dic, "autologin", "false");
            DecryptStringDic(dic, "remeberpwd", "false");
            DecryptStringDic(dic, "name");
            DecryptStringDic(dic, "pwd");
            DecryptStringDic(dic, "extension");
            return dic;
        }
        /// 解密数据
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="name"></param>
        /// <param name="dvalue"></param>
        private static void DecryptStringDic(Dictionary<string, string> dic, string name, string dvalue = "")
        {
            if (dic.ContainsKey(name) && dic[name] != "")
            {
                dic[name] = BitAuto.ISDC.CC2012.BLL.Util.TryDecryptString(dic[name]);
            }
            else
            {
                dic[name] = dvalue;
            }
        }
    }
}
