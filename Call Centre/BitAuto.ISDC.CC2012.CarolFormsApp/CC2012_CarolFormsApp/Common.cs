using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CC2012_CarolFormsApp
{
    public class Common
    {
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
    }
}
