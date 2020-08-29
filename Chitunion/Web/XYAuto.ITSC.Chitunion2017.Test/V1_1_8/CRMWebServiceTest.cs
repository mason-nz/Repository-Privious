using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Linq;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_8
{
    [TestClass]
    public class CRMWebServiceTest
    {
        [TestMethod]
        public void GetCustomByEmpNoOrCustNameTest()
        {
            CRMWebService.CustomerServiceSoapClient crmWeb = new CRMWebService.CustomerServiceSoapClient();
            string ret = crmWeb.GetCustomByEmpNoOrCustName("测试", "");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ret);
            XmlNodeList xmlCust = doc.GetElementsByTagName("Table");

            var query = from XmlNode node in xmlCust
                        select new
                        {
                            a = node.FirstChild.InnerText,
                            b = node.LastChild.InnerText
                        };

            foreach (var item in query)
            {
                string ss = item.a;
                string ss1 = item.b;
            }
            NUnit.Framework.Assert.AreEqual(true, !string.IsNullOrEmpty(ret));
        }
    }
}
