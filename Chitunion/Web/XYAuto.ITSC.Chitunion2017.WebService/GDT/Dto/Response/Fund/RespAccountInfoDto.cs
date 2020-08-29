/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 16:20:36
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Fund
{
    public class RespAccountInfoDto
    {
        [JsonProperty("account_id")]
        //广点通代理商子客户（简称子客）账户id
        public int AccountId { get; set; }

        [JsonProperty("daily_budget")]
        //日限额，单位为分，详见[日限额修改规则]
        public int DailyBudget { get; set; }

        [JsonProperty("system_status")]
        //客户系统状态
        public string SystemStatus { get; set; }

        [JsonProperty("reject_message")]
        //审核消息
        public string RejectMessage { get; set; }

        [JsonProperty("corporation_name")]
        //企业名称
        public string CorporationName { get; set; }

        [JsonProperty("contact_person")]
        //联系人姓名
        public string ContactPerson { get; set; }

        [JsonProperty("contact_person_telephone")]
        //联系人座机电话号码，格式为：区号-座机号，例如：0755-86013388
        public string ContactPersonTelephone { get; set; }
    }
}