using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.WebService.GDT
{
    public enum EnumCampaign_type
    {
        [EnumTextValue("普通展示广告")]
        CAMPAIGN_TYPE_NORMAL = 1,
        [EnumTextValue("微信公众号广告")]
        CAMPAIGN_TYPE_WECHAT_OFFICIAL_ACCOUNTS = 2,
        [EnumTextValue("微信朋友圈广告")]
        CAMPAIGN_TYPE_WECHAT_MOMENTS = 3
    }
}
