using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.WebService.Qichedaquan;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.WebService.Qichedaquan
{
    [TestFixture]
    public class UserInfoHelperTest
    {
        [SetUp]
        public void UserInfoHelper()
        {

        }

        [Test]
        public void GetUserInfoByTokenTest([Values("8349eab3717b4610bc056fc62d85281a")] string user_token, [Values("5d81db99484c0019cab240b3d04e9a4a")] string appid)
        {
            //UserInfo user = XYAuto.ITSC.Chitunion2017.WebService.Qichedaquan.UserInfoHelper.Instance.GetUserInfoByToken(user_token, appid);
            string str = "{\"code\":10000,\"data\":{\"askPrice_mobile\":\"\",\"birth_time\":null,\"create_time\":1490236019000,\"is_deleted\":0,\"mobile\":\"12000000003\",\"nick_name\":\"做你英雄\",\"nick_name_index\":\"做你英雄\",\"password\":\"\",\"real_name\":\"\",\"update_time\":null,\"userDaquan\":{\"sign\":1,\"sum\":120,\"uid\":4},\"userQQ\":null,\"userWeibo\":null,\"userWeixin\":null,\"user_avatar\":\"http://img1.qcdqcdn.com/group1/M00/00/6A/o4YBAFjSGSmAfdAWAAAPN5fHaGI878.jpg\",\"user_businessplatform\":0,\"user_create_ip\":\"\",\"user_gender\":0,\"user_id\":4,\"user_platform\":1,\"user_sign\":\"\",\"user_source\":1,\"user_status\":1,\"user_token\":\"8349eab3717b4610bc056fc62d85281a\"},\"msg\":\"服务调用成功\",\"sub_code\":0}";

            UserResult user = JsonConvert.DeserializeObject<UserResult>(str);
            Assert.AreNotEqual(null, user);
        }
    }
}
