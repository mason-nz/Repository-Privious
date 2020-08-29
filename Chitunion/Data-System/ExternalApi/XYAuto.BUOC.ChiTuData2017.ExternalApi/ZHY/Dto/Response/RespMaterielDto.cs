/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 20:21:24
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Response
{
    public class RespMaterielDto
    {
        [JsonIgnore]
        public int MaterielId { get; set; }

        [JsonProperty("MaterielID")]
        public int ArticleId { get; set; }

        public string Url { get; set; }
        public int PV { get; set; }
        public int UV { get; set; }
        public decimal JumpChance { get; set; }
        public ClueDto Clue { get; set; }
        public HeadmaterialDto HeadMaterial { get; set; }
        public List<WaistmaterialDto> WaistMaterial { get; set; }
    }

    public class ClueDto
    {
        public int PhoneClueCount { get; set; }//话单量
        public int InquiryCount { get; set; }//询价量
        public int ConversationCount { get; set; } //会话量
    }

    public class HeadmaterialDto
    {
        public int PV { get; set; }
        public int UV { get; set; }
        public ClikepvDto ClikeUV { get; set; }
        public ClikepvDto ClikePV { get; set; }
        public int ShareTotal { get; set; }//转发
        public ShareDto Share { get; set; }
        public int AgreeCount { get; set; }//点赞数
    }

    public class ClikeuvDto
    {
        public int Applet { get; set; }//小程序
        public int Phone { get; set; }//电话
        public int Inquiry { get; set; }//询价
        public int QA { get; set; }//问答
        public int HeadPortrait { get; set; }//头像
    }

    public class ClikepvDto
    {
        public int Applet { get; set; }
        public int Phone { get; set; }
        public int Inquiry { get; set; }
        public int QA { get; set; }
        public int HeadPortrait { get; set; }
    }

    public class ShareDto
    {
        public int WeChatFriend { get; set; }
        public int WeChatFriends { get; set; }
        public int QQ { get; set; }
    }

    public class WaistmaterialDto
    {
        [JsonProperty("MaterielID")]
        public int ArticleId { get; set; }

        public string Url { get; set; }
        public int PV { get; set; }
        public int UV { get; set; }
        public int WaistClikePV { get; set; }
        public int WaistClikeUV { get; set; }
        public ClikepvDto ClikeUV { get; set; }
        public ClikepvDto ClikePV { get; set; }
        public int ShareTotal { get; set; }
        public ShareDto Share { get; set; }
    }
}