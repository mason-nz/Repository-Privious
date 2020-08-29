using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XYAuto.ITSC.Chitunion2017.BLL.APP;
using XYAuto.ITSC.Chitunion2017.Entities.APP;

namespace XYAuto.ITSC.Chitunion2017New.Test.APP
{
    [TestClass]
    public class GenericProfilesTest
    {
        [TestMethod]
        public void XmlConfig()
        {
            //string strPath = ConfigurationManager.AppSettings["ConfigArgsPath"] + "\\GenericProfiles.xml";
            //var NodeText = GenericProfilesBll.Instance.GetWelcomeBountyPrice();
            //XmlDocument doc = new XmlDocument();
            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.IgnoreComments = true;
            ////xmlFilePath:xml文件路径  
            //XmlReader reader = XmlReader.Create(strPath, settings);
            //doc.Load(reader);    //加载Xml文件

            //XmlElement rootElem = doc.DocumentElement;
            //string lastTime = string.Empty;
            //FeedbackModel tem = new FeedbackModel { CreateTime = DateTime.Now.ToString(), OpinionText = "cest", UserID = 1 };
            //var ceshi = JsonConvert.SerializeObject(tem);


            //var Nodel = rootElem.SelectSingleNode($"WithdrawSettings");
            //var shuzu = JsonConvert.SerializeObject(Nodel);


            //var tesfsm = Nodel.SelectSingleNode($"ContentRead");
            var NodelModel = GenericProfilesBll.Instance.GetConfigurationInfo(new ConfigurationModel
            {
                UserID = 112,
                NodeType = "pz_hytc"
            });
          
            var shuzuss = JsonConvert.SerializeObject(NodelModel);

            //XmlNodeList nodeList = Nodel.ChildNodes;

            //var temmm = Nodel.SelectSingleNode("ShareOrder");
            //if (nodeList != null)
            //{
            //    lastTime = nodeList[0].InnerText;
            //}

            //reader.Close();
            //reader.Dispose();


        }
    }
}