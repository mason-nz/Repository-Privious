using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.Common
{
    public class ConfigXmlInfo
    {

        public static XmlDocument doc = null;
        public static XmlElement rootElem;
        public static ConfigXmlInfo instance = null;
        public static readonly object padlock = new object();
        public static string strPath = ConfigurationManager.AppSettings["ConfigArgsPath"] + "\\ConfigArgs.xml";
        public ConfigXmlInfo()
        {

            Log4NetHelper.Debug("文件位置" + strPath);
            doc = new XmlDocument();
            doc.Load(strPath);    //加载Xml文件  
            rootElem = doc.DocumentElement;   //获取根节点  
        }
        public static ConfigXmlInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ConfigXmlInfo();
                        }
                    }
                }
                return instance;
            }
        }
        public string GetLastTime(string NodeName)
        {
            string lastTime = string.Empty;
            lock (this)
            {
                XmlNodeList nodeList = rootElem.SelectSingleNode($"IndexTime//{NodeName}").ChildNodes;
                if (nodeList != null)
                {
                    lastTime = nodeList[0].InnerText;
                }
                return lastTime;
            }
        }

        public void UpdateLastTime(DateTime? time, string NodeName)
        {
            lock (this)
            {
                XmlNodeList nodeList = rootElem.SelectSingleNode($"IndexTime//{NodeName}").ChildNodes;
                if (nodeList != null && time != null)
                {
                    nodeList[0].InnerText = Convert.ToDateTime(time).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                //string strPath = ConfigurationManager.AppSettings["ConfigArgsPath"] + "\\ConfigArgs.xml";
                doc.Save(strPath);
            }
        }
    }
}
