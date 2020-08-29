/********************************************************
*创建人：lixiong
*创建时间：2017/8/10 14:42:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1.Materiel
{
    [TestClass]
    public class HttpClientTest
    {
        [TestMethod]
        public void Http_test()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            //get
            client.DownloadFile("http://www.chitunion.com/api/Materiel/DownloadZip?ChannelIds=10,11&MaterielId=3", "ddd.zip");

            //post

            byte[] postData = Encoding.UTF8.GetBytes("ChannelIds=10,11&MaterielId=3");
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var responseData = client.UploadData("http://www.chitunion.com/api/Materiel/DownloadZip", "post", postData);
            var data = Encoding.UTF8.GetString(responseData);// 解码
        }
    }
}