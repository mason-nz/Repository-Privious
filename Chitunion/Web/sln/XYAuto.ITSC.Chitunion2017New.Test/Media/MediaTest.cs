using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017New.Test.Media
{

    [TestClass]
    public class MediaTest
    {
     
        [TestMethod]
        public void GetWeiXinList()
        {
            int[] intList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var shuzu= JsonConvert.SerializeObject(new UserStatus {  Status=1, UserIDList = new int[] { 1,2,3} });

            List<int> temsss = new List<int> { 20, 30, 200, 300 };

            var listArray= JsonConvert.SerializeObject(temsss);

            int  weixinCount= MediaBll.Instance.GetWeiXinCount(new LuceneQuery { LastTime = "1900-01-01 00:00:00.000" });

            Dictionary<string, dynamic> list = MediaBll.Instance.GetMediaMatchingList(new MediaQueryArgs
            {
                ListType = "wx_list,wb_list",
                Keyword = "奥迪A6",
                PageIndex = 1,
                PageSize = 20,
                ProvinceID = -1,
                CityID = -24//,MaxPrice=0,MinPrice=50000,ADPositionID=6001
            });


            string tem1 = String.Format("{0:N}", 1234567.891);
            string tem2 = String.Format("{0:N}", 200981);
            string tem = JsonConvert.SerializeObject(list);
            string t = string.Empty;
        }
        /// <summary>
        /// 智能匹配列表
        /// </summary>
        [TestMethod]
        public void GetMediaMatchingList()
        {

            Dictionary<string, dynamic> list = MediaBll.Instance.GetMediaMatchingList(new MediaQueryArgs
            {
                ListType = "wx_list,wb_list,app_list",
                CityID = 1,
                ProvinceID = 1

            });

            string tem = JsonConvert.SerializeObject(list);

        }

    }

    public class UserStatus
    {
        public int Status { get; set; }

        public int[] UserIDList { get; set; }
    }
}
