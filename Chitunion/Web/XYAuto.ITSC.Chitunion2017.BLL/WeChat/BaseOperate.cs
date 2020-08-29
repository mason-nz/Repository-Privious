/********************************************************
*创建人：hant
*创建时间：2018/1/16 9:44:24 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.WeChat
{
    public class BaseOperate
    {
        private static string Token = string.Empty;
        private static string EncodingAESKey = string.Empty;
        private static string AppId = string.Empty;
        private static string AppSecret = string.Empty;

        static BaseOperate()
        {
            Token = ConfigurationManager.AppSettings["WeixinToken"];
            EncodingAESKey = ConfigurationManager.AppSettings["WeixinEncodingAESKey"];
            AppId = ConfigurationManager.AppSettings["WeixinAppId"];
            AppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];

        }

        /// <summary>
        /// 微信接入认证
        /// </summary>
        /// <returns></returns>
        public static string Auth(string signature, string timestamp, string nonce, string echostr)
        {
            //string Token = ConfigurationManager.AppSettings["WeixinToken"];//从配置文件获取Token
            if (CheckSignature.Check(signature, timestamp, nonce, Token))
            {
                return echostr;//返回随机字符串则表示验证通过
            }
            else
            {
                return "failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。 如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。";
            }
        }
    }
}
