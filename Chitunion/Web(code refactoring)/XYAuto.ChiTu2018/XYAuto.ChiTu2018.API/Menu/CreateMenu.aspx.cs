using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.API.Menu
{
    public partial class CreateMenu : System.Web.UI.Page
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
                txtMenu.Text = "没有openID NEW";
            }
        }
        protected void btnMenu_Click(object sender, EventArgs e)
        {
            string Domin = ConfigurationManager.AppSettings["Domin"];
            string HuoDong_Domin = ConfigurationManager.AppSettings["HuoDong_Domin"];
            Log4NetHelper.Default().Info("btnMenu_Click Index begin:" + Domin);
            Senparc.Weixin.Entities.WxJsonResult result = new WxJsonResult();
            try
            {
                //http://wx-ct.qichedaquan.com/api/OAuth2/index?returnUrl=http://wx-ct.qichedaquan.com/userManager/addIformation.html
                ButtonGroup bg = new ButtonGroup();

                string menu = $"{Domin}/api/OAuth2/index?returnUrl=";

                //分享赚钱 
                var subButton = new SingleViewButton()
                {
                    type = "view",
                    name = "抢单赚钱",
                    url = $"{Domin}{"/index.html?channel=ctlmcaidan"}"

                };
                //subButton.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "抢单赚钱",
                //    url = $"{Domin}{"/moneyManager/make_money.html?channel=ctlmcaidan"}"

                //});
                //subButton.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "邀请有礼",
                //    url = $"{HuoDong_Domin}{"/inviteManager/invite.html?channel=ctlmcaidan"}"
                //});
                ////subButton.sub_button.Add(new SingleViewButton()
                ////{
                ////    type = "view",
                ////    name = "女神节抽奖",
                ////    url = "https://16226447-10.hd.faisco.cn/16226447/uwu6Si3MnBEZ-MSMKgAXIA/jrdpd.html?fromImgMsg=false"
                ////});
                //subButton.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "签到有礼",
                //    url = $"{Domin}{"/moneyManager/sign.html?channel=ctlmcaidan"}"
                //});

                //subButton.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "提现规则",
                //    //url = menu + $"{Domin}{"/cashManager/rule.html"}"
                //    url = $"{Domin}{"/cashManager/rule.html?channel=ctlmcaidan"}"
                //});

                //我的收益
                var subButton1 = new SubButton()
                {
                    name = "我的收益"
                };
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "余额提现",
                    url = $"{Domin}{"/pages/balance.html?channel=ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "我的订单",
                    url = $"{Domin}{"/pages/order.html?channel=ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "收益明细",
                    url = $"{Domin}{"/pages/profitlist.html?channel=ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "提现规则",
                    url = $"{Domin}{"/pages/rule.html?channel=ctlmcaidan"}"
                });
                //活动·服务
                var subButton2 = new SubButton()
                {
                    name = "活动·服务"
                };
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "邀请有礼",
                    url = $"{HuoDong_Domin}{"/pages/invite.html?channel=ctlmcaidan"}"
                });
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "签到有礼",
                    url = $"{Domin}{"/pages/sign.html?channel=ctlmcaidan"}"
                });
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "社群福利",
                    url = $"https://mp.weixin.qq.com/s?__biz=MzI1MDY0NTgzNA==&mid=100000766&idx=1&sn=fc068abb793519ee330331bf3c3fd2e7&scene=19#wechat_redirect"
                });
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "在线客服",
                    url = $"http://q.url.cn/CDq34f?_type=wpa&qidian=true"
                });
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "个人信息",
                    url = $"{Domin}{"/pages/user.html?channel=ctlmcaidan"}"
                });
                //subButton2.sub_button.Add(new SingleClickButton()
                //{
                //    type = "click",
                //    name = "备用微信号",
                //    key = "BackupWeixinNum"
                //});
                //subButton2.sub_button.Add(new SingleClickButton()
                //{
                //    type = "click",
                //    name = "商务合作",
                //    key = "Cooperation"
                //});
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    //type = "click",
                //    //name = "加群交流",
                //    //key = "Communication"
                //    type = "view",
                //    name = "撩兔妹",
                //    url = $"http://mp.weixin.qq.com/s/Iw-q7xJDveR-QiEPamRB_A"
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
                Log4NetHelper.Default().Error("Emnu error:", ex);
                litlMenu.Text = result.errmsg;
                litlMenu.Visible = true;
            }
        }
        //protected void btnMenu_Click(object sender, EventArgs e)
        //{
        //    string Domin = ConfigurationManager.AppSettings["Domin"];
        //    string HuoDong_Domin = ConfigurationManager.AppSettings["HuoDong_Domin"];
        //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("btnMenu_Click Index begin:" + Domin);
        //    Senparc.Weixin.Entities.WxJsonResult result = new WxJsonResult();
        //    try
        //    {
        //        //http://wx-ct.qichedaquan.com/api/OAuth2/index?returnUrl=http://wx-ct.qichedaquan.com/userManager/addIformation.html
        //        ButtonGroup bg = new ButtonGroup();

        //        string menu = $"{Domin}/api/OAuth2/index?returnUrl=";

        //        //分享赚钱 
        //        var subButton = new SubButton()
        //        {
        //            name = "分享赚钱"
        //        };
        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "抢单赚钱",
        //            url = $"{Domin}{"/moneyManager/make_money.html?channel=ctlmcaidan"}"

        //        });
        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "邀请有礼",
        //            url = $"{HuoDong_Domin}{"/inviteManager/invite.html?channel=ctlmcaidan"}"
        //        });
        //        //subButton.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "女神节抽奖",
        //        //    url = "https://16226447-10.hd.faisco.cn/16226447/uwu6Si3MnBEZ-MSMKgAXIA/jrdpd.html?fromImgMsg=false"
        //        //});
        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "签到有礼",
        //            url = $"{Domin}{"/moneyManager/sign.html?channel=ctlmcaidan"}"
        //        });

        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "提现规则",
        //            //url = menu + $"{Domin}{"/cashManager/rule.html"}"
        //            url = $"{Domin}{"/cashManager/rule.html?channel=ctlmcaidan"}"
        //        });

        //        //我的账户
        //        var subButton1 = new SubButton()
        //        {
        //            name = "我的账户"
        //        };
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "提现",
        //            url = $"{Domin}{"/cashManager/accountInfo.html?channel=ctlmcaidan"}"
        //        });
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "我的订单",
        //            url = $"{Domin}{"/accountManager/order_list.html?channel=ctlmcaidan"}"
        //        });
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "个人信息",
        //            url = $"{Domin}{"/userManager/userinfo.html?channel=ctlmcaidan"}"
        //        });
        //        //联系我们
        //        var subButton2 = new SubButton()
        //        {
        //            name = "互动"
        //        };
        //        subButton2.sub_button.Add(new SingleClickButton()
        //        {
        //            type = "click",
        //            name = "备用微信号",
        //            key = "BackupWeixinNum"
        //        });
        //        subButton2.sub_button.Add(new SingleClickButton()
        //        {
        //            type = "click",
        //            name = "商务合作",
        //            key = "Cooperation"
        //        });
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            //type = "click",
        //            //name = "加群交流",
        //            //key = "Communication"
        //            type = "view",
        //            name = "撩兔妹",
        //            url = $"http://mp.weixin.qq.com/s/Iw-q7xJDveR-QiEPamRB_A"
        //        });
        //        //subButton2.sub_button.Add(new SingleClickButton()
        //        //{
        //        //    type = "click",
        //        //    name = "万元大奖",
        //        //    key = "Winner"
        //        //});
        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "意见反馈",
        //        //    url = menu + $"{Domin}{"/api/OpenIdTest.aspx"}"
        //        //});
        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "测试授权",
        //        //    url = $"{Domin}{"/test/test.html"}"
        //        //});
        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "测试授权(zxh)",
        //        //    url = $"{Domin}{"/accountManager/order_list.html"}"
        //        //});

        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "H5页面",
        //        //    url = $"{Domin}/h5/index.html"
        //        //});


        //        bg.button.Add(subButton);
        //        bg.button.Add(subButton1);
        //        bg.button.Add(subButton2);
        //        result = CommonApi.CreateMenu(ConfigurationManager.AppSettings["WeixinAppId"], bg);
        //        litlMenu.Text = result.errmsg;
        //        litlMenu.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("Emnu error:", ex);
        //        litlMenu.Text = result.errmsg;
        //        litlMenu.Visible = true;
        //    }
        //}
        //protected void btnMenu_Click(object sender, EventArgs e)
        //{
        //    XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("btnMenu_Click begin");
        //    string Domin = ConfigurationManager.AppSettings["Domin"];
        //    Senparc.Weixin.Entities.WxJsonResult result = new WxJsonResult();
        //    try
        //    {
        //        //http://wx-ct.qichedaquan.com/api/OAuth2/index?returnUrl=http://wx-ct.qichedaquan.com/userManager/addIformation.html
        //        ButtonGroup bg = new ButtonGroup();

        //        string menu = $"{Domin}/api2/OAuth2/index?returnUrl=";

        //        //分享赚钱 
        //        var subButton = new SubButton()
        //        {
        //            name = "分享赚钱"
        //        };
        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "抢单赚钱",
        //            url = $"{Domin}{"/moneyManager/make_money.html"}"

        //        });
        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "签到有礼",
        //            url = $"{Domin}{"/moneyManager/sign.html"}"
        //        });
        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "邀请有礼",
        //            url = $"{Domin}{"/inviteManager/invite.html"}"
        //        });
        //        subButton.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "提现规则",
        //            //url = menu + $"{Domin}{"/cashManager/rule.html"}"
        //            url = $"{Domin}{"/cashManager/rule.html"}"
        //        });

        //        //我的账户
        //        var subButton1 = new SubButton()
        //        {
        //            name = "我的账户"
        //        };
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "提现",
        //            url = $"{Domin}{"/cashManager/accountInfo.html"}"
        //        });
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "我的订单",
        //            url = $"{Domin}{"/accountManager/order_list.html"}"
        //        });
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "个人信息",
        //            url = $"{Domin}{"/userManager/userInfo.html"}"
        //        });
        //        //联系我们
        //        var subButton2 = new SubButton()
        //        {
        //            name = "联系我们"
        //        };
        //        subButton2.sub_button.Add(new SingleClickButton()
        //        {
        //            type = "click",
        //            name = "商务合作",
        //            key = "Cooperation"

        //        });
        //        subButton2.sub_button.Add(new SingleClickButton()
        //        {
        //            type = "click",
        //            name = "加群交流",
        //            key = "Communication"
        //        });
        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "意见反馈",
        //        //    url = menu + $"{Domin}{"/api/OpenIdTest.aspx"}"
        //        //});
        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "测试授权",
        //        //    url = $"{Domin}{"/test/test.html"}"
        //        //});
        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "测试授权(zxh)",
        //        //    url = $"{Domin}{"/accountManager/order_list.html"}"
        //        //});

        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "H5页面",
        //        //    url = $"{Domin}/h5/index.html"
        //        //});


        //        bg.button.Add(subButton);
        //        bg.button.Add(subButton1);
        //        bg.button.Add(subButton2);
        //        result = CommonApi.CreateMenu(ConfigurationManager.AppSettings["WeixinAppId"], bg);
        //        litlMenu.Text = result.errmsg;
        //        litlMenu.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"btnMenu_Click ex.Message：{ex.Message}");
        //        XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"btnMenu_Click ex.StackTrace：{ex.StackTrace}");
        //        litlMenu.Text = result.errmsg;
        //        litlMenu.Visible = true;
        //    }
        //}
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
    }
}