using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.Open.MessageHandlers;
using System.IO;
using Senparc.Weixin.Open.Entities.Request;
using Senparc.Weixin.Open;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers.WeChat
{
    public class CustomThirdPartyMessageHandler : ThirdPartyMessageHandler
    {
        public CustomThirdPartyMessageHandler(Stream inputStream, PostModel encryptPostModel)
            : base(inputStream, encryptPostModel)
        { }

        public override string OnComponentVerifyTicketRequest(RequestMessageComponentVerifyTicket requestMessage)
        {
            var openTicketPath = Server.GetMapPath("~/App_Data/OpenTicket");
            if (!Directory.Exists(openTicketPath))
            {
                Directory.CreateDirectory(openTicketPath);
            }

            //RequestDocument.Save(Path.Combine(openTicketPath, string.Format("{0}_Doc.txt", DateTime.Now.Ticks)));

            //记录ComponentVerifyTicket（也可以存入数据库或其他可以持久化的地方）
            using (TextWriter tw = new StreamWriter(Path.Combine(openTicketPath, string.Format("{0}.txt", RequestMessage.AppId))))
            {
                tw.Write(requestMessage.ComponentVerifyTicket);
                tw.Flush();
                tw.Close();
            }

            return base.OnComponentVerifyTicketRequest(requestMessage);
        }

        public override string OnUnauthorizedRequest(RequestMessageUnauthorized requestMessage)
        {
            //取消授权
            return base.OnUnauthorizedRequest(requestMessage);
        }
    }
}