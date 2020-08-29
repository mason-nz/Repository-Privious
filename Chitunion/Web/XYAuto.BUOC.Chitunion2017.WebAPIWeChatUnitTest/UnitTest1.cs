using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin.Helpers;
using System.Configuration;
using System.Web;
using Senparc.Weixin.MP.Helpers;
using System.Web.Configuration;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Senparc.Weixin.MP.Containers;
using System.IO;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.HttpUtility;
using System.Data;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3;
using static XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using System.Web.Http;
using Newtonsoft.Json;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChatUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();

            wxuser.openid = "owXr60unZWxQ16r34b5KUPa1kO10";

            wxuser.unionid = "userInfo.unionid";//userInfo.unionid;

            bool trt = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.InsetWeiXinAndUserInfo(wxuser);
        }


        [TestMethod]
        public void Inser()
        {
            //ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();


            //wxuser.AuthorizeTime = Convert.ToDateTime("0001/1/1 0:00:00");
            //wxuser.city = "石景山";
            //wxuser.country = "中国";
            //wxuser.CreateTime = Convert.ToDateTime("2018/1/24 16:14:18");
            //wxuser.groupid = 0;
            //wxuser.headimgurl = "http://wx.qlogo.cn/mmopen/lLDt97mMhiankCovbegeQPz4KkkBMwbtOsMZLUw0dib5JGhTRjtgdUzkCdpWxL8DJxVG2LuNJe5zQGuWBboVeh8z01ibSDdGXBp/132";
            //wxuser.InvitationQR = "http://192.168.3.71/group1/M00/17/AB/oYYBAFpoPv2AfY7AAAe3uaVnMrs508.png";
            //wxuser.Inviter = "";
            //wxuser.language = "zh_CN";
            //wxuser.LastUpdateTime = Convert.ToDateTime("2018/1/24 16:14:18");
            //wxuser.nickname = "tt";
            //wxuser.openid = "owXr60unZWxQ16r34b5KUPa1kO10";
            //wxuser.province = "北京";
            //wxuser.QRcode = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQEL8TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyQ2lwUXNuMVdmV2kxMDAwMDAwN0UAAgQMjmFaAwQAAAAA";
            //wxuser.remark = "";
            //wxuser.sex = 1;
            //wxuser.Status = 0;
            //wxuser.subscribe = 1;
            //wxuser.subscribe_time = Convert.ToDateTime("2018/1/24 16:14:18");
            //wxuser.tagid_list = "";
            //wxuser.unionid = "o_J_G02wIt32KqgSQMvqk4NRCwVU";
            //wxuser.UserID = 1594;



            //bool resultUserId = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.WeiXinUserOperation(wxuser);
        }

        [TestMethod]
        public void GetUserSceneByUserId_Test()
        {
            int userid = 1594;
            var item = XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3.LE_WXUserScene.Instance.GetUserSceneByUserId(0, userid);
        }

        [TestMethod]
        public void UpdateUserScene_test()
        {
            //string json = "{\"openid\":\"owXr60hNuzludjO7VegRW27_4nY4\",\"SceneInfo\":[{\"SceneID\":\"7\",\"SceneName\":\"职场\"},{\"SceneID\":\"11\",\"SceneName\":\"投资\"},{\"SceneID\":\"12\",\"SceneName\":\"理财\"},{\"SceneID\":\"15\",\"SceneName\":\"小众\"},{\"SceneID\":\"18\",\"SceneName\":\"Test\"}]}";
            //ITSC.Chitunion2017.Entities.DTO.V2_3.WXUserSceneResDTO res = Newtonsoft.Json.JsonConvert.DeserializeObject<ITSC.Chitunion2017.Entities.DTO.V2_3.WXUserSceneResDTO>(json);
            //var user = WeiXinUser.Instance.GetUserInfo(res.OpenId);
            //bool result = LE_WXUserScene.Instance.UpdateWeiXinUserScene(user.UserID,res);
        }

        [TestMethod]
        public void GetTaskListByUserId_Test()
        {
            int totalcount = 0;
            TaskResDTO res = new TaskResDTO();
            res.UserID = 1521;
            res.PageIndex = 1;
            res.PageSize = 10;
            res.SceneID = 0;
            var item = LE_Task.Instance.GetDataByPage(res, out totalcount);
        }

        [TestMethod]
        public void ImageOperate_Test()
        {
            string QRurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQGc8DwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyTF96enRvMVdmV2kxMDAwMDAwNzkAAgRTjmFaAwQAAAAA";

            int width = 180;
            int height = 180;
            string tt = ImageOperate.Instance.GetActivityQRcode(QRurl, width, height,out QRurl);

        }

        [TestMethod]
        public void get()
        {
            string tt = "http://192.168.3.71/group1/M00/17/AB/oYYBAFpn-uuAHAgfAAAumNxIOss410.JPG";
            string hh = tt.Substring(tt.LastIndexOf('/') + 1);
            string name = tt.Substring(tt.LastIndexOf('/'), tt.Length - tt.LastIndexOf('/')).Replace("/", "");
        }

        [TestMethod]
        public void GetTaskListByUserId()
        {
            int totalcount = 0;
            TaskResDTO res = new TaskResDTO();
            //res.OpenID = "owXr60hNuzludjO7VegRW27_4nY4";
            res.PageIndex = 1;
            res.PageSize = 20;
            res.SceneID = 0;
            var list = LE_Task.Instance.GetDataByPage(res, out totalcount);
        }

        [TestMethod]
        public void GetOrderByStatus()
        {
            int totalcount = 0;

            var list = ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetOrderByStatus(1521, 193002, 1, 10, out totalcount);
            var jsonResult = Util.GetJsonDataByResult(list, "Success", 0);

        }

        [TestMethod]
        public void GetOrderUrl()
        {
            int taskId = XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3.LE_Task.Instance.GetTaskIdByMaterialID(235);
            string orderurl = XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetOrderUrl(taskId,152);

            var task = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetTaskInfo(taskId);

            XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3.TemporaryOrderRepDTO entity = new TemporaryOrderRepDTO { TaskId = taskId, OrderUrl = orderurl, Synopsis = task.Synopsis, TaskName = task.TaskName, ImgUrl = task.ImgUrl };

            var jsonResult = Util.GetJsonDataByResult(task, "Success", 0);
        }

        [TestMethod]
        public void GetMedia()
        {
            int userId = 1520;
            var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);
            var MediaId = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetMediaId("wx3b4d1cdb1de3d00c", user.InvitationQR);
            InvitationQRAndUrlRepDTO entity = new InvitationQRAndUrlRepDTO { MediaID = MediaId, InvitationQR = user.InvitationQR };
        }

        [TestMethod]
        public void DownImage()
        {
            string tt = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.DownloadImage("http://192.168.3.71/group1/M00/17/AB/oYYBAFpn-uuAHAgfAAAumNxIOss410.JPG");
        }

        [TestMethod]
        public void WeiXinOperate()
        {
            var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo("owXr60o7HYwA3Gd40F9s-zh4hwDM");
            XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.WeiXinUserOperation(user);
        }


        [TestMethod]
        public void CombinImage_Test()
        {

            //string resultImg = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.CombinImage(@"D:\GitRoot（C）\A5信息系统研发\销售业务管理平台\赤兔联盟系统\XYAuto.BUOC.Chitunion2017.WebAPIWeChat\Images\01-赤兔联盟.jpg", @"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQEL8TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyQ2lwUXNuMVdmV2kxMDAwMDAwN0UAAgQMjmFaAwQAAAAA", Convert.ToInt32(192), Convert.ToInt32(632));

            string tt = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(@"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQEL8TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyQ2lwUXNuMVdmV2kxMDAwMDAwN0UAAgQMjmFaAwQAAAAA", 180, 180, out tt);
        }

        [TestMethod]
        public void GetOrderInfo()
        {
            int userId = 1532;
            var result = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetOrder(439, userId);
            var jsonResult = Util.GetJsonDataByResult(result, "Success", 0);
        }


        [TestMethod]
        public void json()
        {
            //StringBuilder json = new StringBuilder();
            //json.Append("{").AppendFormat("\"t\":\"{0}\",", (int)ITSC.Chitunion2017.Entities.Enum.WeChatSourceEnum.邀请).AppendFormat("\"v\":\"{0}\"", "ggggggggggggggggggggg").Append("}");
            //string tt = json.ToString();
            string tt = "qrscene_{\"t\":\"1\",\"v\":\"owXr60unZWxQ16r34b5KUPa1kO10\"}";
            //var QRJson = Newtonsoft.Json.JsonConvert.DeserializeObject<XYAuto.ITSC.Chitunion2017.Entities.QRJson>(tt.Replace("qrscene_", ""));
        }


        [TestMethod]
        public void ImgTest()
        {
            string InvitationQR = string.Empty;
            string imgBack = @"D:\GitRoot（C）\A5信息系统研发\销售业务管理平台\赤兔联盟系统\XYAuto.BUOC.Chitunion2017.WebAPIWeChat\Images\demoIcon.jpg";
            string img = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQGK8DwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyVElIM0FrRi1meWkxMDAwME0wN3gAAgRo025aAwQAAAAA";
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", img, 180, 180);
            string qr = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = null; //Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qr);
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(imgBack);
            System.Drawing.Image imgWarter = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.ReturnImage(info.Result.CutImageUrl);
            string file = System.Guid.NewGuid().ToString() + ".jpg";
            try
            {
                Bitmap bt = new Bitmap(imgSrc);

                using (Graphics g = Graphics.FromImage(bt))
                {
                    int level = 100; //图像质量 1-100的范围
                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    g.DrawImage(imgWarter, new Rectangle(192, 632,
                                                     imgWarter.Width,
                                                     imgWarter.Height),
                            0, 0, imgWarter.Width, imgWarter.Height, GraphicsUnit.Pixel);

                    EncoderParameters ep = new EncoderParameters();
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)level);
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo codec in codecs)
                    {
                        if (codec.MimeType == "image/jpeg")
                            ici = codec;
                    }
                    bt.Save(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + file, ici, ep);

                    //释放位图缓存
                    bt.Dispose();
                }
                //EncoderParameters ep = new EncoderParameters();
                //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
                //保存成图片
                //imgSrc.Save(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + file,);
                //imgSrc.Save(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalImg") + "\\" + file, ImageFormat.Jpeg);
                // postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LocalWebImage") + file, imgSrc.Width, imgSrc.Height);
                //string qr = GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
                //ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qr);
                //InvitationQR = info.Result.CutImageUrl;
                //imgSrc.Dispose();
                //imgWarter.Dispose();

            }
            catch (Exception)
            {
                imgSrc.Dispose();
                throw;
            }

            // InvitationQR;
        }


        [TestMethod]
        public void MediaId_test()
        {
            string pathimg = string.Empty;
            try
            {
                //hSoEcYIqWAi2rp4XYud53gqGrwwLJfff-nEzCsa9LkLabKwwIbc8RGXxhThRZJq8
                int userId = 1594;
                var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);
                pathimg = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.DownloadImage(user.InvitationQR);
                if (pathimg != null)
                {
                    AccessTokenContainer.Register("wx3b4d1cdb1de3d00c", "2326e662d87034cab3ac09abfcdb12c1");
                    var MediaId = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetMediaId("wx3b4d1cdb1de3d00c", pathimg);
                    InvitationQRAndUrlRepDTO entity = new InvitationQRAndUrlRepDTO { MediaID = MediaId, InvitationQR = user.InvitationQR };
                    XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.DeleteFile(pathimg);
                    var tt = Util.GetJsonDataByResult(entity, "Success", 0);
                }
                else
                {
                    var tt = Util.GetJsonDataByResult(null, "Fail", -1);
                }
            }
            catch (Exception ex)
            {

                var tt = Util.GetJsonDataByResult(ex.ToString(), "Fail", -1);
            }
        }

        [TestMethod]
        public void shuc_test()
        {
            AccessTokenContainer.Register("wx3b4d1cdb1de3d00c", "2326e662d87034cab3ac09abfcdb12c1");
            using (MemoryStream ms = new MemoryStream())
            {
                MediaApi.Get(ConfigurationManager.AppSettings["WeixinAppId"], "hSoEcYIqWAi2rp4XYud53gqGrwwLJfff-nEzCsa9LkLabKwwIbc8RGXxhThRZJq8", ms);
                Image img = Bitmap.FromStream(ms, true);
                img.Save(@"D:\Images\tt.jpg");
            }
        }

        [TestMethod]
        public void SubOrder()
        {
            bool tt = false;
            string urltt = System.Web.HttpUtility.UrlEncode("http://wx-ct.qichedaquan.com/news/20180123/235.html?utm_source=chitu&utm_term=2tkxIZ1lgw");
            string postdate = "TaskType=192001&TaskId=259&UserId=1608&ChannelId=101003&OrderUrl=" + urltt;
            string geturl = XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.PostWebRequest(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GetOrderUrlByPost"), postdate);
            ITSC.Chitunion2017.Entities.WeChat.OrderUrl url =  Newtonsoft.Json.JsonConvert.DeserializeObject<ITSC.Chitunion2017.Entities.WeChat.OrderUrl>(geturl);
            if (url.Status == 0)
            {
                tt = true;
            }

        }

        [TestMethod]
        public void OrderSun()
        {
            var result = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetOrder(439, 1532);
            var jsonResult = Util.GetJsonDataByResult(result, "Success", 0);

        }


        [TestMethod]
        public void getorderurl()
        {
            string orderUrl = string.Empty;
            DataSet ds = XYAuto.ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetOrderByTaskIdAndUserId(1, 2);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                orderUrl = ds.Tables[0].Rows[0]["OrderUrl"].ToString();
            }
            else
            {
                string MaterialUrl = XYAuto.ITSC.Chitunion2017.BLL.WeChat.Order.Instance.GetMaterialUrlByTaskId(1);
    
            }

        }


        [TestMethod]
        public void qc_test()
        {
            AccessTokenContainer.Register("wx3b4d1cdb1de3d00c", "2326e662d87034cab3ac09abfcdb12c1");
            string pathimg = string.Empty;
            int userId = 1594;
            var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);

            string userQC = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode("wx3b4d1cdb1de3d00c", user.openid, Senparc.Weixin.MP.QrCode_ActionName.QR_SCENE);

            string InvitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(userQC, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")),out pathimg);

           // pathimg = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.DownloadImage(InvitationQR);
            if (pathimg != null)
            {
                var MediaId = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetMediaId("wx3b4d1cdb1de3d00c", pathimg);
                InvitationQRAndUrlRepDTO entity = new InvitationQRAndUrlRepDTO { MediaID = MediaId, InvitationQR = user.InvitationQR };
                XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.DeleteFile(pathimg);
                //return Common.Util.GetJsonDataByResult(entity, "Success", 0);
            }
            else
            {
                //return Common.Util.GetJsonDataByResult(null, "Fail", -1);
            }
        }

        [TestMethod]
        public void XQ_Test()
        {
            string tt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            AccessTokenContainer.Register("wx3b4d1cdb1de3d00c", "2326e662d87034cab3ac09abfcdb12c1");
            try
            {
                string templateId = "xd-oXKxfJazUouilKoPtBppcjRmu9Ey8BbOT6SO3f4Q";
                var testData = new
                {
                    first = new TemplateDataItem("您好，您的提现操作已经成功", "#FF0000"),
                    keyword1 = new TemplateDataItem(string.Format("{0:F}", "2018-02-08"), "#000000"),
                    keyword2 = new TemplateDataItem("11.89", "#000000"),
                    remark = new TemplateDataItem("感谢您的使用", "#000000")
                };

                var result = TemplateApi.SendTemplateMessage("wx3b4d1cdb1de3d00c", "owXr60unZWxQ16r34b5KUPa1kO10", templateId, "/userManager/addIformation.html", testData, null);
                //return result.errmsg == "ok";
            }
            catch (Exception ex)
            {
                //Loger.Log4Net.Info("[WithdrawalsNotice]:" + ex.ToString() + "");
                //return false;
            }
        }


        [TestMethod]
        public void rr_Test()
        {
            AccessTokenContainer.Register("wx3b4d1cdb1de3d00c", "2326e662d87034cab3ac09abfcdb12c1");
            int userId = 1619;
            var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);
            string userQC = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode("wx3b4d1cdb1de3d00c", user.openid, Senparc.Weixin.MP.QrCode_ActionName.QR_SCENE);
            string invitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(userQC, userId);
            InvitationQRAndUrlRepDTO entity = new InvitationQRAndUrlRepDTO { MediaID = "", InvitationQR = invitationQR };
        }

        [TestMethod]
        public void IsAuth_Test()
        {
            //var isAuth = OAuthApi.Auth("7_BaJ6N_na6TMVNVDXUqZWCw1yj6XtlWaifoKjmATzhMJq9yWzW_mEiVxFdbRijAoE6Fd9DVgM4N_Ge3xJ3H172VZd4yVTqkmANLeKKB_FgnA", "ohvZzwHD0cqFXMf07ww8WRHDhyXk");
            //WebAPIWeChat.Controllers.OAuth2Controller ctl = new WebAPIWeChat.Controllers.OAuth2Controller();
            //ctl.Index("http://wxtest-ct.qichedaquan.com/cashManager/accountInfo.html");

            string appId = ConfigurationManager.AppSettings["WeixinAppId"];
            string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
            string Domin = ConfigurationManager.AppSettings["Domin"];
            string returnUrl = "http://wxtest-ct.qichedaquan.com/cashManager/accountInfo.html";
            string redirect_uri = $"{Domin}{"/api/OAuth2/UserInfoCallback"}?returnUrl={returnUrl}";
            string state = "ChiTu" + DateTime.Now.Millisecond;
            string redirect = OAuthApi.GetAuthorizeUrl(appId, redirect_uri, state, Senparc.Weixin.MP.OAuthScope.snsapi_userinfo);
            HttpContext.Current.Response.Redirect(redirect);
            return;
        }
        [TestMethod]
        public void IsValidActivity_Test()
        {
            //string[] strarray = str.Split(new string[] { "|" }, StringSplitOptions.None);
            var ctl = new WebAPIWeChat.Controllers.SignController();
            var ret = ctl.IsValidActivity(new ITSC.Chitunion2017.BLL.Controller.Dto.IsValidActivity.ReqDto()
            {
                ActivityType = (int)ITSC.Chitunion2017.BLL.Controller.Dto.IsValidActivity.EnumActivityType.签到有礼
            });
            Console.WriteLine(JsonConvert.SerializeObject(ret.Result));
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void GetSceneInfoByUserId_Test()
        {
            //string[] strarray = str.Split(new string[] { "|" }, StringSplitOptions.None);
            var ctl = new WebAPIWeChat.Controllers.TaskController();
            var ret = ctl.GetSceneInfoByUserId();
            Console.WriteLine(JsonConvert.SerializeObject(ret.Result));
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void GetInvitationMediaId_Test()
        {
            //string[] strarray = str.Split(new string[] { "|" }, StringSplitOptions.None);
            AccessTokenContainer.Register("wxf0eea6fec2756b45", "2e35c3d7acb3219b6d77415267d06975");
            var ctl = new WebAPIWeChat.Controllers.WeixinJSSDKController();
            var ret = ctl.GetInvitationMediaId();
            Console.WriteLine(JsonConvert.SerializeObject(ret.Result));
            Assert.AreEqual(0, ret.Status);
        }
    }

}

