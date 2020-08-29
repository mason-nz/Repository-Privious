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
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using static XYAuto.ITSC.Chitunion2017.BLL.WeChat.ImageOperate;

namespace XYAuto.BUOC.Chitunion2017.Menu
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
                txtMenu.Text = "没有openID";
            }
        }
        protected void btnMenu_Click(object sender, EventArgs e)
        {
            XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("btnMenu_Click begin");
            string Domin = ConfigurationManager.AppSettings["Domin"];
            Senparc.Weixin.Entities.WxJsonResult result = new WxJsonResult();
            try
            {
                //http://wx-ct.qichedaquan.com/api/OAuth2/index?returnUrl=http://wx-ct.qichedaquan.com/userManager/addIformation.html
                ButtonGroup bg = new ButtonGroup();

                string menu = $"{Domin}/api2/OAuth2/index?returnUrl=";

                //分享赚钱 
                var subButton = new SingleViewButton()
                {
                    type = "view",
                    name = "💰抢任务",
                    //url = $"{Domin}{"/moneyManager/make_money.html"}"
                    url = $"{Domin}{"/index.html?channel=h5ctlmcaidan"}"

                };


                //我的账户
                var subButton1 = new SubButton()
                {
                    name = "我的收益"
                };
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "余额提现",
                    url = $"{Domin}{"/cashManager/accountInfo.html?channel=h5ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "我的订单",
                    url = $"{Domin}{"/accountManager/order_list.html?channel=h5ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "收益明细",
                    url = $"{Domin}{"/cashManager/profitList.html?channel=h5ctlmcaidan"}"
                });
                subButton1.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "个人信息",
                    url = $"{Domin}{"/userManager/userinfo.html?channel=h5ctlmcaidan"}"
                });
                //联系我们
                var subButton2 = new SubButton()
                {
                    name = "活动·服务"
                };
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "有奖问卷",
                //    url = $"https://www.wjx.cn/jq/23812303.aspx"
                //});
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "签到有礼",
                //    url = $"https://mp.weixin.qq.com/s/Mqb8Va7uTBds44fWY_YFtA"
                //});
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "邀请有礼",
                    url = $"{Domin}{"/inviteManager/invite.html?channel=h5ctlmcaidan"}"
                });
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "在线客服",
                    url = $"http://q.url.cn/CDq34f?_type=wpa&qidian=true"
                });
                subButton2.sub_button.Add(new SingleClickButton()
                {
                    type = "click",
                    name = "社群福利",
                    key = "Communication"
                });
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "个人信息",
                //    url = $"{Domin}{"/userManager/userinfo.html?channel=ctkzcd"}"
                //});
                subButton2.sub_button.Add(new SingleViewButton()
                {
                    type = "view",
                    name = "帮助中心",
                    url = $"{Domin}{"/cashManager/rule.html?channel=h5ctlmcaidan"}"
                });
                //subButton2.sub_button.Add(new SingleViewButton()
                //{
                //    type = "view",
                //    name = "抽奖活动",
                //    url = "https://hd.faisco.cn/16226447/uwu6Si3MnBF3gwRlt5n5mA/load.html?style=24"
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
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"btnMenu_Click ex.Message：{ex.Message}");
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"btnMenu_Click ex.StackTrace：{ex.StackTrace}");
                litlMenu.Text = result.errmsg;
                litlMenu.Visible = true;
            }
        }
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
        //        var subButton = new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "💰抢单赚钱",
        //            //url = $"{Domin}{"/moneyManager/make_money.html"}"
        //            url = $"{Domin}{"/index.html?channel=ctkzcd"}"

        //        };


        //        //我的账户
        //        var subButton1 = new SubButton()
        //        {
        //            name = "我的收益"
        //        };
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "余额提现",
        //            url = $"{Domin}{"/cashManager/accountInfo.html?channel=ctkzcd"}"
        //        });
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "我的订单",
        //            url = $"{Domin}{"/accountManager/order_list.html?channel=ctkzcd"}"
        //        });
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "收益明细",
        //            url = $"{Domin}{"/cashManager/profitList.html?channel=ctkzcd"}"
        //        });
        //        //联系我们
        //        var subButton2 = new SubButton()
        //        {
        //            name = "活动·服务"
        //        };
        //        //subButton2.sub_button.Add(new SingleViewButton()
        //        //{
        //        //    type = "view",
        //        //    name = "冲顶大会",
        //        //    url = $"https://hd.faisco.cn/16226447/uwu6Si3MnBEV-rvyU9-OsQ/load.html?style=0"
        //        //});
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "社群福利",
        //            //url = $"https://mp.weixin.qq.com/s?__biz=MzI1MDY0NTgzNA==&mid=100000766&idx=1&sn=fc068abb793519ee330331bf3c3fd2e7&scene=19#wechat_redirect"
        //            url = $"https://mp.weixin.qq.com/s/6wN_Zmm0ZeNC-xvvwzU4ww"
        //        });
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "在线客服",
        //            url = $"http://q.url.cn/CDq34f?_type=wpa&qidian=true"
        //        });
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "个人信息",
        //            url = $"{Domin}{"/userManager/userinfo.html?channel=ctkzcd"}"
        //        });
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "帮助中心",
        //            url = $"{Domin}{"/cashManager/rule.html?channel=ctkzcd"}"
        //        });
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "抽奖活动",
        //            url = "https://hd.faisco.cn/16226447/uwu6Si3MnBF3gwRlt5n5mA/load.html?style=24"
        //        });

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
        //        var subButton = new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "抢单赚钱",
        //            //url = $"{Domin}{"/moneyManager/make_money.html"}"
        //            url = $"{Domin}{"/index.html?channel=ctkzcd"}"

        //        };


        //        //我的账户
        //        var subButton1 = new SubButton()
        //        {
        //            name = "我的收益"
        //        };
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "帐户余额",
        //            url = $"{Domin}{"/cashManager/accountInfo.html?channel=ctkzcd"}"
        //        });
        //        subButton1.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "收益明细",
        //            url = $"{Domin}{"/cashManager/profitList.html?channel=ctkzcd"}"
        //        });                              
        //        //联系我们
        //        var subButton2 = new SubButton()
        //        {
        //            name = "服务活动"
        //        };               
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "撩兔妹",
        //            url = $"http://mp.weixin.qq.com/s/Iw-q7xJDveR-QiEPamRB_A"
        //        });
        //        subButton2.sub_button.Add(new SingleClickButton()
        //        {
        //            type = "click",
        //            name = "商务合作",
        //            key = "Cooperation"
        //        });
        //        subButton2.sub_button.Add(new SingleViewButton()
        //        {
        //            type = "view",
        //            name = "帮助中心",
        //            url = $"{Domin}{"/cashManager/rule.html?channel=ctkzcd"}"
        //        });

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
        //            //url = $"{Domin}{"/moneyManager/make_money.html"}"
        //            url = $"{Domin}{"/index.html"}"

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