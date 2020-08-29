using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017New.Test.Media
{
    [TestClass]
    public class MediaUserTest
    {
        [TestMethod]
        public void WeiXinAudit()
        {
            MediaUserBll.Instance.WeiXinAudit(new UserBatchQueryArgs { Status=0,UserID=1434});
        }
        [TestMethod]
        public void GetPromotionChannelList()
        {
            var list = MediaUserBll.Instance.GetPromotionChannelList(1);
            string resultJson = JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 批量启用、禁用
        /// </summary>
        [TestMethod]
        public void UserEnableOrDisable()
        {
            int reuslt = MediaUserBll.Instance.UserEnableOrDisable(new UserBatchQueryArgs { Status = 0, UserIDList = new List<int> { 29, 30 }, ListType = "g_user" });
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        [TestMethod]
        public void GetMediaUserList()
        {
            var query = MediaUserBll.Instance.GetMediaUserList(new UserQueryArgs
            {
                ListType = "g_user",
                //Mobile = "13240110346"
                ApproveStatus=-2,
                Source = 3002,
                BeginTime = "2015-12-25",
                EndTime = "2018-01-02"
            });

            string resultJson = JsonConvert.SerializeObject(query);
        }
        /// <summary>
        /// 批量更新用户密码
        /// </summary>
        [TestMethod]
        public void UserResetPassword()
        {
            var query = MediaUserBll.Instance.UserResetPassword(new UserBatchQueryArgs
            {
                UserIDList = new List<int> { 29 },
                ListType = "g_user"
            });
        }
        [TestMethod]
        public void UserCertificationAudit()
        {
            var query = MediaUserBll.Instance.UserCertificationAudit(new UserBatchQueryArgs
            {
                UserID = 1434,
                Reason = "测试",
                Status = 3
            });
        }
        [TestMethod]
        public void GetMediaUserDetailInfo()
        {
            var query = MediaUserBll.Instance.GetMediaUserDetailInfo(3);

        }


    }
}
