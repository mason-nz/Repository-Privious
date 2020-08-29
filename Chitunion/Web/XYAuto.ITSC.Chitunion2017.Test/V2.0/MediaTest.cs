using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.Test.V2._0
{
    [TestClass]
    public class MediaTest
    {
        private MediaMatchingController tem = new MediaMatchingController();
        [TestMethod]
        public void GetWeiXinList()
        {
            var query =tem.GetMediaMatchingList(new  MediaQueryArgs
            {
                ListType = "wx_list,wb_list,app_list"
            });

            string resultJson = JsonConvert.SerializeObject(query);
        }
        private UserMangeController ctl = new UserMangeController();
        [TestMethod]
        public void TestQueryUserBasicInfo()
        {
            var ret = ctl.QueryUserBasicInfo(new BLL.UserManage.Dto.ReqQueryUserBasicInfoDto() { UserID = 1299 });
            string str = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void GetWeiBoLuceneList()
        {
            //List<WeiBoModel> list = MediaBll.Instance.GetWeiBoLuceneList();

            //string t = string.Empty;

            //List<WeiXinListModel> list = MediaBll.Instance.GetMediaMatchingList(new MediaQueryArgs
            //{

            //    Keyword = "汽车"
            //});

            dynamic t = SplitHelper.GetEnumDescriptionList<PositionEnum>("6001");
        }
    }
}
