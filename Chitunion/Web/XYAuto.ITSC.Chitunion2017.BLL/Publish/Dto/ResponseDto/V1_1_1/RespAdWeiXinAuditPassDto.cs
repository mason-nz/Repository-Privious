/********************************************************
*创建人：lixiong
*创建时间：2017/5/12 11:28:30
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1
{
    public class RespAdBaseDto
    {
        public string ADName { get; set; }
        public int MediaID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }
    }

    public class RespAdWeiXinAuditPassDto : RespAdBaseDto
    {
        public int PubID { get; set; }
        public string TermOfValidity { get; set; }
        public string ReferencePrice { get; set; }
        public int WxId { get; set; }
        public string SubmitUser { get; set; }
        public string AuditUser { get; set; }
        public string Mobile { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Source { get; set; }
        public string UserName { get; set; }
        public string Wx_StatusName { get; set; }

        [JsonIgnore]
        public string BusinessParams { get; set; }

        public bool IsAreaMedia { get; set; }

        public string RoleID { get; set; }
        public List<AdItemInfo> AdItemInfo { get; set; }
    }

    public class AdItemInfo
    {
        public string ReferencePrice { get; set; }
        public string TermOfValidity { get; set; }
        public string AdStatus { get; set; }
        public string PubID { get; set; }
        public string RoleId { get; set; }
    }
}