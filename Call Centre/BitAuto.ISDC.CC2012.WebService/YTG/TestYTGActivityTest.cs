using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.WebService.YTGActivityService;

namespace BitAuto.ISDC.CC2012.WebService.YTG
{
    public class TestYTGActivityTest
    {
        static YTGActivityService.YTGActivityService service = new YTGActivityService.YTGActivityService();
        private static string key = "2D55A797-6386-4D35-A6B6-B340CF425B2E";

        private static Random random = new Random();

        public static Result InsertYTGActivity()
        {
            string id = DateTime.Now.ToString("yyMMddHHmmss") + random.Next(100, 999);

            YTGActivityInfo obj = new YTGActivityInfo();
            obj.ActivityID = id;
            obj.ActivityName = "活动主题";
            obj.SignBeginTime = DateTime.Today;
            obj.SignEndTime = DateTime.Today.AddHours(23);
            obj.ActiveBeginTime = DateTime.Today.AddDays(1);
            obj.ActiveEndTime = DateTime.Today.AddDays(1).AddHours(23);
            obj.Address = "活动地址";
            obj.CarSerials = "2948,2383,1564,1577,1580,1581";
            obj.Content = "活动内容";
            obj.Url = "http://www.baidu.com/";
            obj.Status = 1;
            return service.InsertOrUpdateYTGActivity(key, obj);
        }

        public static Result UpdateYTGActivity(string id)
        {
            YTGActivityInfo obj = new YTGActivityInfo();
            obj.ActivityID = id;
            obj.ActivityName = "活动主题" + random.Next(100, 999);
            obj.SignBeginTime = DateTime.Today;
            obj.SignEndTime = DateTime.Today.AddHours(23);
            obj.ActiveBeginTime = DateTime.Today.AddDays(1);
            obj.ActiveEndTime = DateTime.Today.AddDays(1).AddHours(23);
            obj.Address = "活动地址" + random.Next(100, 999);
            obj.CarSerials = "3746,3749,3757,3758,3957";
            obj.Content = "活动内容" + random.Next(100, 999);
            obj.Url = "http://www.baidu.com/";
            obj.Status = 1;
            return service.InsertOrUpdateYTGActivity(key, obj);
        }

        public static Result EndYTGActivity()
        {
            string activityid = "141222111626651";
            return service.EndYTGActivity(key, activityid);
        }
    }
}
