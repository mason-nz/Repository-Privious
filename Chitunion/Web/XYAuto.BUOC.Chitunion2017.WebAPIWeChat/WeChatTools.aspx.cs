using Newtonsoft.Json;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Senparc.Weixin.Helpers.Extensions;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using static XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    public partial class WeChatTools : System.Web.UI.Page
    {
        private string WeixinAppId = ConfigurationManager.AppSettings["WeixinAppId"];
        public string LocalWebImage = ConfigurationManager.AppSettings["LocalWebImage"];

        protected void Page_Load(object sender, EventArgs e)
        {
            string openid = HttpContext.Current.Request["openid"];
            if (openid != null && openid.Length > 0)
            {
                txtMenu.Text = openid;
            }
            else
            {
                txtMenu.Text = "没有openID";
            }
        }

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            string Domin = ConfigurationManager.AppSettings["Domin"];
            string HuoDong_Domin = ConfigurationManager.AppSettings["HuoDong_Domin"];
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("btnMenu_Click Index begin:" + Domin);
            Senparc.Weixin.Entities.WxJsonResult result = new WxJsonResult();
            try
            {
                //http://wx-ct.qichedaquan.com/api/OAuth2/index?returnUrl=http://wx-ct.qichedaquan.com/userManager/addIformation.html
                ButtonGroup bg = new ButtonGroup();

                string menu = $"{Domin}/api/OAuth2/index?returnUrl=";

                //分享赚钱 
                var subButton = new SubButton()
                {
                    name = "分享赚钱"
                };
                subButton.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "抢单赚钱",
                    url = $"{Domin}{"/index.html?channel=ctlmcaidan"}"

                });
                subButton.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "邀请有礼",
                    url = $"{HuoDong_Domin}{"/inviteManager/invite.html?channel=ctlmcaidan"}"
                });
                //subButton.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "女神节抽奖",
                //    url = "https://16226447-10.hd.faisco.cn/16226447/uwu6Si3MnBEZ-MSMKgAXIA/jrdpd.html?fromImgMsg=false"
                //});
                subButton.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "签到有礼",
                    url = $"{Domin}{"/moneyManager/sign.html?channel=ctlmcaidan"}"
                });

                subButton.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "提现规则",
                    //url = menu + $"{Domin}{"/cashManager/rule.html"}"
                    url = $"{Domin}{"/cashManager/rule.html?channel=ctlmcaidan"}"
                });

                //我的账户
                var subButton1 = new SubButton()
                {
                    name = "我的账户"
                };
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "提现",
                    url = $"{Domin}{"/cashManager/accountInfo.html?channel=ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "我的订单",
                    url = $"{Domin}{"/accountManager/order_list.html?channel=ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "个人信息",
                    url = $"{Domin}{"/userManager/userinfo.html?channel=ctlmcaidan"}"
                });
                //联系我们
                var subButton2 = new SubButton()
                {
                    name = "互动"
                };
                subButton2.sub_button.Add(new SingleClickButton()
                {
                    type = "click",
                    name = "备用微信号",
                    key = "BackupWeixinNum"
                });
                subButton2.sub_button.Add(new SingleClickButton()
                {
                    type = "click",
                    name = "商务合作",
                    key = "Cooperation"
                });
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    //type = "click",
                    //name = "加群交流",
                    //key = "Communication"
                    type = "view",
                    name = "撩兔妹",
                    url = $"http://mp.weixin.qq.com/s/Iw-q7xJDveR-QiEPamRB_A"
                });
                //subButton2.sub_button.Add(new SingleClickButton()
                //{
                //    type = "click",
                //    name = "万元大奖",
                //    key = "Winner"
                //});
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "意见反馈",
                //    url = menu + $"{Domin}{"/api/OpenIdTest.aspx"}"
                //});
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "测试授权",
                //    url = $"{Domin}{"/test/test.html"}"
                //});
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "测试授权(zxh)",
                //    url = $"{Domin}{"/accountManager/order_list.html"}"
                //});

                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "H5页面",
                //    url = $"{Domin}/h5/index.html"
                //});


                bg.button.Add(subButton);
                bg.button.Add(subButton1);
                bg.button.Add(subButton2);
                result = CommonApi.CreateMenu(ConfigurationManager.AppSettings["WeixinAppId"], bg);
                litlMenu.Text = result.errmsg;
                litlMenu.Visible = true;
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("Emnu error:", ex);
                litlMenu.Text = result.errmsg;
                litlMenu.Visible = true;
            }
        }


        protected void btnGetUserList_Click(object sender, EventArgs e)
        {
            var userList = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Get(WeixinAppId, txtNextOpenId.Value.ToString());
            if (userList != null)
            {
                litGetUserList.Text = JsonConvert.SerializeObject(userList);
                litGetUserList.Visible = true;
            }
        }


        protected void btnUser_Click(object sender, EventArgs e)
        {
            try
            {
                //var max = 100;
                string nextOpenId = null;
                var stop = false;
                List<UserInfoJson> userInfoList = new List<UserInfoJson>();
                var accessToken = AccessTokenContainer.GetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"]);
                while (!stop)
                {
                    var result = UserApi.Get(accessToken, nextOpenId);
                    litlMenu.Text = result.total.ToString();
                    litlMenu.Visible = true;
                    nextOpenId = result.next_openid;

                    foreach (var id in result.data.openid)
                    {
                        var userInfoResult = UserApi.Info(accessToken, id);
                        userInfoList.Add(userInfoResult);

                        if (!XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.IsExistOpneId(userInfoResult.openid))
                        {
                            ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();
                            wxuser.subscribe = userInfoResult.subscribe;
                            wxuser.openid = userInfoResult.openid;
                            wxuser.nickname = userInfoResult.nickname;
                            wxuser.sex = userInfoResult.sex;
                            wxuser.city = userInfoResult.city;
                            wxuser.country = userInfoResult.country;
                            wxuser.province = userInfoResult.province;
                            wxuser.language = userInfoResult.language;
                            wxuser.headimgurl = userInfoResult.headimgurl;
                            wxuser.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfoResult.subscribe_time);
                            wxuser.unionid = userInfoResult.unionid;
                            wxuser.remark = userInfoResult.remark;
                            wxuser.groupid = userInfoResult.groupid;
                            wxuser.tagid_list = string.Join(",", userInfoResult.tagid_list);
                            wxuser.UserID = 0;
                            wxuser.CreateTime = DateTime.Now;
                            wxuser.LastUpdateTime = wxuser.CreateTime;
                            //wxuser.QRcode = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode(ConfigurationManager.AppSettings["WeixinAppId"], userInfoResult.openid);
                            //wxuser.InvitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(wxuser.QRcode, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")));
                            wxuser.Status = userInfoResult.subscribe == 1 ? 0 : -1;
                            ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.InsetWeiXinAndUserInfo(wxuser);
                        }
                        //Console.WriteLine(userInfoList.Count + ".添加：" + userInfoResult.nickname);

                        //if (userInfoList.Count >= max)
                        //{
                        //    stop = true;
                        //    break;
                        //}
                    }

                    if (nextOpenId == null)
                    {
                        stop = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("[btnUser_Click]报错", ex);
            }
        }

        protected void btnIamge_Click(object sender, EventArgs e)
        {
            try
            {
                var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadTemporaryMedia(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeixinAppId"), UploadMediaFileType.image, $"{XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Domin")}/api/Images/oYYBAFpp0fGAXOdQAAe2ckdhV20518.jpg");
                litlMenu.Text = uploadResult.media_id;
                litlMenu.Visible = true;
            }
            catch (Exception ex)
            {
                litlMenu.Text = ex.ToString();
                litlMenu.Visible = true;
            }
        }

        protected void btnMenu0_Click(object sender, EventArgs e)
        {
            try
            {
                Senparc.Weixin.Entities.WxJsonResult result = new WxJsonResult();
                result = CommonApi.DeleteMenu(ConfigurationManager.AppSettings["WeixinAppId"]);
                litlMenu.Text = result.errmsg;
                litlMenu.Visible = true;
            }
            catch (Exception ex)
            {
                litlMenu.Text = ex.Message;
                litlMenu.Visible = true;
            }


        }

        protected void btnSB_Click(object sender, EventArgs e)
        {

            string qr = CreateQrCode(WeiXinActivityEnum.撒币活动);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, 430, 430);
            string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
            litlsb.Text = info.Result.CutImageUrl;
            litlsb.Visible = true;
        }

        protected void btnFoot_Click(object sender, EventArgs e)
        {
            string qr = CreateQrCode(WeiXinActivityEnum.文章来源弹窗);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, 430, 430);
            string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
            litlFoot.Text = info.Result.CutImageUrl;
            litlFoot.Visible = true;
        }

        protected void btnArticle_Click(object sender, EventArgs e)
        {
            string qr = CreateQrCode(WeiXinActivityEnum.文章详情底部);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, 430, 430);
            string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
            litlArticle.Text = info.Result.CutImageUrl;
            litlArticle.Visible = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string qr = CreateQrCode(WeiXinActivityEnum.大全pc右下角);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, 430, 430);
            string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
            Literal1.Text = info.Result.CutImageUrl;
            Literal1.Visible = true;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string qr = CreateQrCode(WeiXinActivityEnum.大全h5文章详情页);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, 430, 430);
            string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
            Literal2.Text = info.Result.CutImageUrl;
            Literal2.Visible = true;
        }


        private string CreateQrCode(WeiXinActivityEnum ActivityEnum, Senparc.Weixin.MP.QrCode_ActionName actionname = Senparc.Weixin.MP.QrCode_ActionName.QR_LIMIT_STR_SCENE)
        {

            string appId = ConfigurationManager.AppSettings["WeixinAppId"];
            string WeixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];

            AccessTokenContainer.Register(appId, WeixinAppSecret);
            try
            {
                StringBuilder json = new StringBuilder();

                json.Append("{").AppendFormat("\"t\":\"{0}\",", (int)ReqMessageType.场景).AppendFormat("\"v\":\"{0}\"", ActivityEnum.ToString()).Append("}");
                var qrCodeResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.Create(appId, 0, 1, actionname, json.ToString());
                return QrCodeApi.GetShowQrCodeUrl(qrCodeResult.ticket);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("[WeChatTools CreateQrCode]报错", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取永久素材统计情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetMediaCount_Click(object sender, EventArgs e)
        {
            //AccessTokenContainer.Register(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            //var accessToken = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetMediaCount(ConfigurationManager.AppSettings["WeixinAppId"]);
            litMediaCount.Text = JsonConvert.SerializeObject(uploadResult);
            litMediaCount.Visible = true;
        }

        /// <summary>
        /// 获取图片、视频、语音素材列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetOthersMediaList_Click(object sender, EventArgs e)
        {
            //AccessTokenContainer.Register(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            //var accessToken = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            MediaList_OthersResult uploadResult = null;
            switch (int.Parse(ddlGetOthersMediaListMediaType.Value))
            {
                case 0:
                    uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetOthersMediaList(WeixinAppId, UploadMediaFileType.image, 0, 10000);
                    break;
                case 1:
                    uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetOthersMediaList(WeixinAppId, UploadMediaFileType.voice, 0, 10000);
                    break;
                case 2:
                    uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetOthersMediaList(WeixinAppId, UploadMediaFileType.video, 0, 10000);
                    break;
                case 3:
                    uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetOthersMediaList(WeixinAppId, UploadMediaFileType.thumb, 0, 10000);
                    break;
                case 4:
                    uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetOthersMediaList(WeixinAppId, UploadMediaFileType.news, 0, 10000);
                    break;
                default:
                    return;
                    break;
            }

            litGetOthersMediaList.Text = JsonConvert.SerializeObject(uploadResult);
            litGetOthersMediaList.Visible = true;
        }

        protected void btnAddImage_Click(object sender, EventArgs e)
        {
            //AccessTokenContainer.Register(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            //var accessToken = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            string file = Server.MapPath(txtAddImageUrl.Value);
            var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadForeverMedia(WeixinAppId, file);
            litAddImageContent.Text = uploadResult.media_id + " | url= " + uploadResult.url;
            litAddImageContent.Visible = true;
        }

        protected void btnCooperation_Click(object sender, EventArgs e)
        {
            AccessTokenContainer.Register(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            //var accessToken = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);

            var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadForeverMedia(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["LocalImage"] + "\\hezuo.jpg");
            litlCooperation.Text = uploadResult.media_id;
            litlCooperation.Visible = true;
        }

        protected void btnCommunication_Click(object sender, EventArgs e)
        {
            AccessTokenContainer.Register(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);
            // var accessToken = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["WeixinAppSecret"]);

            var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadForeverMedia(ConfigurationManager.AppSettings["WeixinAppId"], ConfigurationManager.AppSettings["LocalWebImage"] + "\\yaoqing.jpg");
            litlCommunication.Text = uploadResult.media_id;
            litlCommunication.Visible = true;
        }

        protected void btnFootImg_Click(object sender, EventArgs e)
        {
            string qr = CreateQrCode(WeiXinActivityEnum.图文消息底部);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, 430, 430);
            string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
            litlFootImg.Text = info.Result.CutImageUrl;
            litlFootImg.Visible = true;
        }

        protected void btnYD_Click(object sender, EventArgs e)
        {
            string qr = CreateQrCode(WeiXinActivityEnum.任务列表引导关注);
            string postdata = string.Format("OriginHeadImageUrl={0}&Width={1}&Height={2}", qr, 430, 430);
            string qrresult = XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetFormatImg(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CutHeadImage"), postdata);
            ImgInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgInfo>(qrresult);
            litlYD.Text = info.Result.CutImageUrl;
            litlYD.Visible = true;
        }


        protected void btnUser2_Click(object sender, EventArgs e)
        {
            try
            {
                var userList = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUsers();
                var accessToken = AccessTokenContainer.GetAccessToken(ConfigurationManager.AppSettings["WeixinAppId"]);
                var sbSql = new StringBuilder();
                int i = 0;
                foreach (var user in userList)
                {
                    var result = UserApi.Info(accessToken, user.openid);
                    int status = -1;
                    if (result != null && result.subscribe == 1)
                        status = 0;
                    if (result != null)
                    {
                        if (result.subscribe == 1)
                            sbSql.AppendLine($@"
                                            UPDATE  Chitunion2017.dbo.LE_WeiXinUser
                                            SET     subscribe = {result.subscribe} ,
                                                    Status = {status} ,
                                                    subscribe_time = '{DateTimeHelper.GetDateTimeFromXml(result.subscribe_time)}'
                                            WHERE   openid = '{user.openid}'");
                        else
                            sbSql.AppendLine($@"
                                            UPDATE  Chitunion2017.dbo.LE_WeiXinUser
                                            SET     subscribe = {result.subscribe} ,
                                                    Status = {status}
                                            WHERE   openid = '{user.openid}'");
                        i++;
                        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"洗微信用户数据Index:{i},OpenID:{user.openid}");
                    }
                }
                var strSql = sbSql.ToString();
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"洗微信用户数据sql:{strSql}");
                if (!string.IsNullOrEmpty(strSql))
                    XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.ExecuteNonQuery(strSql);

                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"洗微信用户数据OK");
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("洗微信用户数据报错", ex);
            }
        }
    }
}