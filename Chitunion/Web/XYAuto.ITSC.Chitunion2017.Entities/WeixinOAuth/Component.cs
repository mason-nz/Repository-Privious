using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth
{

    public class Component
    {
        public int RecID { get; set; }

        public string AppID { get; set; }

        public string Secret { get; set; }

        public string Token { get; set; }

        public string EncodingAESKey { get; set; }

        public string VerifyTicket { get; set; }

        public DateTime TicketTime { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessTokenTime { get; set; }
    }
}
