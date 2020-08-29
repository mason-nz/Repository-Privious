using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{

    public class CatchWeixinResultDTO
    {
        public bool IsTure { get; set; }
        public string Message { get; set; }
        public CatchWeixinModel WeChatData { get; set; }
    }

    public class CatchWeixinModel
    {

        public string WxId { get; set; }
        public string Biz { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string VerifyInfo { get; set; }
        public string QrCode { get; set; }
        public string HeadImg { get; set; }
        public int RegisteredCountry { get; set; }
        public string RegisteredCountryCn { get; set; }
        public int IsOverseas { get; set; }
        public string RegTime { get; set; }
        public string District { get; set; }
        public int Gender { get; set; }
        public int CustomerType { get; set; }
        public string RegisteredId { get; set; }
        public string GenericBusinessType { get; set; }
        public string FrontBusinessType { get; set; }
        public string EnterpriseType { get; set; }
        public string EnterpriseEstablishmentDate { get; set; }
        public string EnterpriseExpiredDate { get; set; }
        public string VerifyDate { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }




    }
}
