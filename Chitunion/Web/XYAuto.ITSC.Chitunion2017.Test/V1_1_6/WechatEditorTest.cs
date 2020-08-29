using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ITSC.Chitunion2017.BLL;
using System.Collections.Generic;
namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_6
{
    [TestClass]
    public class WechatEditorTest
    {
        /// <summary>
        /// 测根据图文组ID查询图文列表
        /// </summary>
        [TestMethod]
        public void SelectGroupInfoByGroupID()
        {
            Dictionary<string, object> dic = BLL.WxEditor.WxArticleGroup.Instance.SelectGroupInfoByGroupID(3);
            JsonResult jr = UtilZKB.GetJsonDataByResult(dic, "Success");
            if (jr.Status == 0)
            {
                Console.WriteLine(jr.Result);
            }
            else
            {
                Console.WriteLine(jr.Message);
            }
        }
        /// <summary>
        /// 测根据从微信URL导入微信文章
        /// </summary>
        [TestMethod]
        public void ImportSingleGroups()
        {
            int groupID = 0;
            string msg = BLL.WxEditor.WxArticleGroup.Instance.ImportWxArticle("https://mp.weixin.qq.com/s?biz=MzIxMDc1MjA1OQ==&mid=2247484418&idx=1&sn=7ba0181c16da26ce6744932f08774877&scene=0#wechat_redirect", ref groupID);
            Console.WriteLine(msg);
        }
    }
}


